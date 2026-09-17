-- Seite "Kameras" – Aumovio Geb.20 R.7003 (Q-Sys)
-- Kamera-Auswahl auf Preview/Live via Video-Router, Take, PTZ-Pad, Presets, Autotracking (ACPR).
-- Kameras: QSC NC-Serie PTZ (ptz.preset = Koordinaten-String, pan/tilt/zoom = Boolean).

local routerCamera = Component.New("routerCamera")
local acpr         = Component.New("acpr")   -- Autotracker, liegt auf Router-Input 1

-- Router-Ausgänge
local OUT_PREVIEW = 10
local OUT_LIVE    = { 1, 2, 3, 4, 5, 6 }  -- Live-Feed geht auf alle diese Ausgänge

-- Router-Eingang des Autotrackers (Live-Feed bei Autotracking EIN)
local IN_TRACKER = 1

-- Bypass-Pin der ACPR-Komponente. Invertiert: Bypass EIN = Tracking AUS.
local ACPR_BYPASS = "TrackingBypass"

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
  btnCamUp      = "tilt.up",
  btnCamDown    = "tilt.down",
  btnCamLeft    = "pan.left",
  btnCamRight   = "pan.right",
  btnCamZoomIn  = "zoom.in",
  btnCamZoomOut = "zoom.out",
}

local selectedPreview = nil  -- Kamera-Index (1..6) oder nil
local selectedLive    = nil

-- Exklusive Modus-Gruppe (Radio): Autotracking + Preset-Recall. Immer genau
-- einer aktiv oder "none". Buttons sind Toggle (Momentary bleibt nicht an, weil
-- der Button beim Loslassen seinen Wert selbst auf 0 zurücksetzt); der aktive
-- Zustand kommt vom Script (LED = Feedback). Jede Zeile = ein Modus + sein Button.
-- preset = Slot-Nummer (nil bei "auto"). Autotracking beenden nur durch
-- Preset-Wahl oder den externen cmdAutotrack (Privacy-Modul).
local cameraModes = {}
local function addMode(mode, button, preset)
  if button then cameraModes[#cameraModes + 1] = { mode = mode, button = button, preset = preset } end
end
addMode("auto", Controls.btnAutotracking)
if Controls.btnPresetRecall then
  for nr, btn in ipairs(Controls.btnPresetRecall) do addMode("preset" .. nr, btn, nr) end
end

local cameraMode = "none"   -- "auto" | "preset1".."presetN" | "none"

local function previewCam() return selectedPreview and cameras[selectedPreview] or nil end
local function liveCam()    return selectedLive    and cameras[selectedLive]    or nil end

-- Einen Eingang auf alle Live-Ausgänge routen.
local function routeLive(input)
  for _, out in ipairs(OUT_LIVE) do
    routerCamera["output." .. out .. ".select"].Value = input
  end
end

-- Preset-Persistenz --------------------------------------------------------
-- presets["p"..nr] = { cam = <Kamera-Index>, pos = <ptz-Koordinaten-String> }.
-- Ablage im optionalen Text-Control "presetData" (Control-Werte überleben Reboot).
-- String-Keys, damit rapidjson konsistent en-/decodiert (keine sparse-Array-Falle).
local json  = require("rapidjson")
local STORE = Controls.presetData
local presets = {}

local function pkey(nr) return "p" .. nr end

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
-- Alle Bedien-Buttons (außer den NO_LOCK-Ausnahmen) deaktivieren + transparent.
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
  Controls.txtCam[2].String = (cameraMode == "auto") and "Autotracking"
                              or (liveCam() and liveCam().label or "-")
end

local function updateSelectFeedback()
  for i, btn in ipairs(Controls.btnSelectCam) do
    btn.Boolean = (i == selectedPreview)
  end
end

-- Preset abrufen: gespeicherte Kamera auf Live schalten und Position anfahren.
local function recallPreset(nr)
  local p = presets[pkey(nr)]
  if not p then return end
  local cam = cameras[p.cam]
  if not cam then return end
  selectedLive = p.cam
  routeLive(cam.input)                                    -- gespeicherte Kamera auf Live
  if p.pos then cam.component["ptz.preset"].String = p.pos end  -- Position anfahren
end

-- Modus setzen: der EINE Umschaltpunkt der Radio-Gruppe.
-- m = "auto" | "presetN" | "none". Setzt Zustand, LEDs, Routing, ACPR-Bypass,
-- UI-Sperre und Labels. Button, Preset-Recall und externer Pin laufen alle hier durch.
-- cameraMode wird VOR den LEDs gesetzt: dadurch sind alle Handler, die das
-- programmatische LED-Setzen erneut auslöst, idempotent bzw. No-ops -> kein Guard nötig.
local function setCameraMode(m)
  local row
  for _, r in ipairs(cameraModes) do if r.mode == m then row = r end end

  -- Preset gewählt, aber Slot leer: nicht umschalten. LEDs auf den Ist-Zustand
  -- zurücksetzen, damit der gedrückte Toggle-Button nicht fälschlich anbleibt.
  if row and row.preset and not presets[pkey(row.preset)] then
    for _, r in ipairs(cameraModes) do r.button.Boolean = (r.mode == cameraMode) end
    return
  end

  cameraMode = m
  for _, r in ipairs(cameraModes) do r.button.Boolean = (r.mode == m) end  -- Radio-Feedback

  local auto   = (m == "auto")
  local bypass = acpr and acpr[ACPR_BYPASS]
  if bypass then bypass.Boolean = not auto end   -- Bypass invertiert: Tracking EIN = Bypass AUS

  if auto then
    routeLive(IN_TRACKER)
  elseif row and row.preset then
    recallPreset(row.preset)
  elseif selectedLive then
    routeLive(cameras[selectedLive].input)                -- "none": zuletzt gewählte Live-Kamera
  end

  setLocked(auto)
  updateLabels()
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
  if not previewCam() then return end
  selectedLive = selectedPreview
  setCameraMode("none")   -- manueller Take verlässt die Radio-Gruppe, routet die Preview-Kamera live
end

-- Preset speichern: aktuelle Preview-Kamera + deren Position im Slot ablegen.
local function savePreset(nr)
  local cam = previewCam()
  if not cam then return end
  presets[pkey(nr)] = { cam = selectedPreview, pos = cam.component["ptz.preset"].String }
  savePresets()
end

-- Event-Handler ------------------------------------------------------------

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

-- Modus-Buttons (Autotracking + Preset-Recall) = Toggle, Radio-Verhalten:
--   steigende Flanke                  -> diesen Modus wählen
--   fallende Flanke am aktiven Button -> wieder anschalten (bleibt an)
-- Dadurch lässt sich der aktive Modus nicht durch erneutes Drücken abwählen;
-- verlassen wird er nur durch einen anderen Modus, Take oder cmdAutotrack.
for _, row in ipairs(cameraModes) do
  row.button.EventHandler = function(ctl)
    if ctl.Boolean then setCameraMode(row.mode)
    elseif cameraMode == row.mode then ctl.Boolean = true end
  end
end

-- Externer Kommando-Pin (Core / Privacy-Modul): schaltet Autotracking
-- deterministisch. Aus -> "none", damit kein Button leuchtet.
if Controls.cmdAutotrack then
  Controls.cmdAutotrack.EventHandler = function(ctl)
    if ctl.Boolean then setCameraMode("auto")
    elseif cameraMode == "auto" then setCameraMode("none") end
  end
end

-- Preset-Buttons sind Trigger: direkt feuern, kein Boolean.
for nr, btn in ipairs(Controls.btnPresetSave) do
  btn.EventHandler = function() savePreset(nr) end
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

-- Init ---------------------------------------------------------------------

loadPresets()
local trackingOn = acpr and acpr[ACPR_BYPASS] and not acpr[ACPR_BYPASS].Boolean  -- Bypass invertiert
updateSelectFeedback()
setCameraMode(trackingOn and "auto" or "none")  -- setzt Labels, LEDs, Routing und Sperr-Zustand
