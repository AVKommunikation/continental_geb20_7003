-- Seite "Kameras" – Aumovio Geb.20 R.7003 (Q-Sys)
-- Kamera-Auswahl auf Preview/Live via Video-Router, Take, PTZ-Pad, Presets, Autotracking (ACPR).
-- Kameras: QSC NC-Serie PTZ (ptz.preset = Koordinaten-String, pan/tilt/zoom = Boolean).

local routerCamera = Component.New("routerCamera")
local acpr         = Component.New("acpr")   -- Autotracker, liegt auf Router-Input 1

-- Router-Ausgänge
local OUT_PREVIEW = 10
local OUT_LIVE    = 1

-- Router-Eingang des Autotrackers (Live-Feed bei Autotracking EIN)
local IN_TRACKER = 1

-- Enable-Pin der ACPR-Komponente. >>> BEI BEDARF ANPASSEN <<<
local ACPR_ENABLE = "enable"

-- Kameras in Reihenfolge der btnSelectCam-Buttons (1..6). Button 7 = Reserve.
local cameras = {
  { label = "Mitte",     input = 2, component = Component.New("camFrontCenter") },
  { label = "Übersicht", input = 3, component = Component.New("camRear")        },
  { label = "Front UL",  input = 4, component = Component.New("camFrontUL")     },
  { label = "Front UR",  input = 5, component = Component.New("camFrontUR")     },
  { label = "Front LL",  input = 6, component = Component.New("camFrontLL")     },
  { label = "Front LR",  input = 7, component = Component.New("camFrontLR")     },
}

-- Zuordnung PTZ-Pad-Button -> Kamera-Pin (bewegt die Preview-Kamera, hold-to-move)
local pad = {
  btnCamUp    = "tilt.up",
  btnCamDown  = "tilt.down",
  btnCamLeft  = "pan.left",
  btnCamRight = "pan.right",
  btnCamZoomIn  = "zoom.in",
  btnCamZoomOut = "zoom.out",
}

local selectedPreview = nil  -- Kamera-Index (1..6) oder nil
local selectedLive    = nil
local autotracking    = false

local function previewCam() return selectedPreview and cameras[selectedPreview] or nil end
local function liveCam()    return selectedLive    and cameras[selectedLive]    or nil end

-- Preset-Persistenz --------------------------------------------------------
-- Positionen key "camIndex:presetNr" -> ptz-Koordinaten-String. Ablage im
-- optionalen Text-Control "presetData" (Control-Werte überleben Reboot).
local json  = require("rapidjson")
local STORE = Controls.presetData
local presets = {}

local function pkey(cam, nr) return cam .. ":" .. nr end

local function savePresets()
  if STORE then STORE.String = json.encode(presets) end
end

local function loadPresets()
  if STORE and STORE.String ~= "" then
    local ok, t = pcall(json.decode, STORE.String)
    if ok and type(t) == "table" then presets = t end
  end
end

-- UI sperren (Autotracking EIN) --------------------------------------------
-- Alle Bedien-Buttons (außer btnAutotracking) deaktivieren + transparent.
-- Basis-CssClass wird gemerkt, damit die Design-Klasse erhalten bleibt.
local lockList = {}
local baseCss  = {}

local function addLock(ctl)
  if ctl then
    lockList[#lockList + 1] = ctl
    baseCss[ctl] = ctl.CssClass or ""
  end
end

-- Generisch: alle Controls dieses Scripts sperren – außer denen, die sichtbar
-- und bedienbar bleiben müssen. So werden auch die PTZ-Controls erfasst,
-- egal wie sie benannt sind.
-- Einzel-Controls werfen bei v[1] einen Fehler ("Property '1' ...") statt nil
-- zu liefern -> Array-Erkennung über pcall absichern.
-- btnPresetRecall bleibt bedienbar, damit man Autotracking per Recall beenden kann.
-- cmdAutotrack ist ein externer Kommando-Pin (Core) und darf nie gesperrt werden.
local NO_LOCK = { btnAutotracking = true, txtCam = true, presetData = true,
                  btnPresetRecall = true, cmdAutotrack = true }
for name, v in pairs(Controls) do
  if not NO_LOCK[name] then
    local isArray, first = pcall(function() return v[1] end)
    if isArray and first ~= nil then          -- Array-Control (Count > 1)
      for _, c in ipairs(v) do addLock(c) end
    else                                      -- Einzel-Control
      addLock(v)
    end
  end
end

local function setLocked(locked)
  for _, c in ipairs(lockList) do
    c.IsDisabled = locked
    c.CssClass   = locked and (baseCss[c] .. " at_locked") or baseCss[c]
  end
end

-- Feedback -----------------------------------------------------------------

local function updateLabels()
  if not Controls.txtCam then return end
  Controls.txtCam[1].String = previewCam() and previewCam().label or "-"
  Controls.txtCam[2].String = autotracking and "Autotracking"
                              or (liveCam() and liveCam().label or "-")
end

local function updateSelectFeedback()
  for i, btn in ipairs(Controls.btnSelectCam) do
    btn.Boolean = (i == selectedPreview)
  end
end

-- Autotracking (ACPR) ------------------------------------------------------

local function applyAutotracking()
  local en = acpr and acpr[ACPR_ENABLE]
  if en then en.Boolean = autotracking end
  if autotracking then
    routerCamera["output." .. OUT_LIVE .. ".select"].Value = IN_TRACKER
  elseif selectedLive then
    routerCamera["output." .. OUT_LIVE .. ".select"].Value = cameras[selectedLive].input
  end
  if Controls.btnAutotracking then Controls.btnAutotracking.Boolean = autotracking end
  setLocked(autotracking)
  updateLabels()
end

local function setAutotracking(on)
  autotracking = on
  applyAutotracking()
end

-- Aktionen -----------------------------------------------------------------

local function selectCam(index)
  local cam = cameras[index]
  if not cam then return end  -- z.B. Button 7 (Reserve)
  selectedPreview = index
  routerCamera["output." .. OUT_PREVIEW .. ".select"].Value = cam.input
  updateSelectFeedback()
  updateLabels()
end

local function take()
  local cam = previewCam()
  if not cam then return end
  selectedLive = selectedPreview
  routerCamera["output." .. OUT_LIVE .. ".select"].Value = cam.input
  updateLabels()
end

local function savePreset(nr)
  local cam = liveCam()
  if not cam then return end
  presets[pkey(selectedLive, nr)] = cam.component["ptz.preset"].String
  savePresets()
end

local function recallPreset(nr)
  if autotracking then setAutotracking(false) end  -- Autotracking aus, Live zurück auf Kamera
  local cam = liveCam()
  if not cam then return end
  local pos = presets[pkey(selectedLive, nr)]
  if pos then cam.component["ptz.preset"].String = pos end  -- Position anfahren, falls gespeichert
end

-- Event-Handler ------------------------------------------------------------
-- Buttons sind Momentary -> nur auf den Tastendruck (Boolean == true) reagieren.

-- Toggle-Buttons mit Radio-Verhalten: aktive Auswahl bleibt gesetzt, ein
-- programmatisch abgeschalteter (nicht gewählter) Button löst nichts aus.
for i, btn in ipairs(Controls.btnSelectCam) do
  btn.EventHandler = function(ctl)
    if ctl.Boolean then
      if cameras[i] then selectCam(i) else ctl.Boolean = false end  -- Button 7 = Reserve
    elseif i == selectedPreview then
      ctl.Boolean = true
    end
  end
end

-- btnTake ist ein Trigger: feuert direkt, kein Boolean.
if Controls.btnTake then Controls.btnTake.EventHandler = function() take() end end

-- Momentary-tauglich: bei jedem Tastendruck (steigende Flanke) umschalten,
-- Loslass-Event (Boolean=false) ignorieren. Funktioniert auch mit gehaltener Taste.
if Controls.btnAutotracking then
  Controls.btnAutotracking.EventHandler = function(ctl)
    if ctl.Boolean then setAutotracking(not autotracking) end
  end
end

-- Externer Kommando-Pin für den Core (Startup/Shutdown): setzt Autotracking
-- deterministisch auf den Pin-Zustand (true = an, false = aus), kein Toggle.
if Controls.cmdAutotrack then
  Controls.cmdAutotrack.EventHandler = function(ctl) setAutotracking(ctl.Boolean) end
end

-- Preset-Buttons sind Trigger: direkt feuern, kein Boolean.
for nr, btn in ipairs(Controls.btnPresetSave) do
  btn.EventHandler = function() savePreset(nr) end
end

for nr, btn in ipairs(Controls.btnPresetRecall) do
  btn.EventHandler = function() recallPreset(nr) end
end

-- PTZ-Pad "Vorschau steuern" -> Preview-Kamera (hold-to-move). Optional.
for name, pin in pairs(pad) do
  local ctl = Controls[name]
  if ctl then
    ctl.EventHandler = function(c)
      local cam = previewCam()
      if cam then cam.component[pin].Boolean = c.Boolean end
    end
  end
end

-- Mitte des Pads = Home-Position anfahren (Trigger, nur beim Drücken).
if Controls.btnCamHome then
  Controls.btnCamHome.EventHandler = function(c)
    local cam = previewCam()
    if cam and c.Boolean then cam.component["preset.home.load"].Boolean = true end
  end
end

-- ACPR-Status extern spiegeln (falls anderswo umgeschaltet).
if acpr and acpr[ACPR_ENABLE] then
  acpr[ACPR_ENABLE].EventHandler = function(c)
    if c.Boolean ~= autotracking then
      autotracking = c.Boolean
      applyAutotracking()
    end
  end
end

-- Init ---------------------------------------------------------------------

loadPresets()
autotracking = acpr and acpr[ACPR_ENABLE] and acpr[ACPR_ENABLE].Boolean or false
updateSelectFeedback()
applyAutotracking()  -- setzt Labels, Button-Feedback und Sperr-Zustand
