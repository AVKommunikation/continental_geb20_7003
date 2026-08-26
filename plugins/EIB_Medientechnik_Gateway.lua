--[[ ===========================================================================
  b+b EIB-Medientechnik-Gateway IP  -->  Q-Sys Plugin (KNX/EIB)
  ---------------------------------------------------------------------------
  Zeilenbasiertes ASCII-Protokoll ueber TCP (CR / 0x0D terminiert).
  Gateway = TCP-Server, Q-Sys = TCP-Client (Default 192.168.100.104:10001).

  Drei Seiten:
    1) Connection    - IP/Port, Status, Reconnect, Provision
    2) Setup         - Tabelle: GA | Datentyp (Dropdown) | Name (Datenpunkt)
    3) Lights/Shades - Prefab-Bloecke, Zuordnung per Dropdown ueber Setup-Namen

  Anlegen im Gateway: pro Datenpunkt "S<ga>:<n>,E" (Format n + Empfang frei),
  Schreiben "W<ga>=<v>", Lesen "R<ga>". Empfang "[PH>]HH/M/UUU=value".
=========================================================================== ]]

PluginInfo = {
  Name         = "b+b~EIB Medientechnik Gateway (KNX)",
  Version      = "1.0",
  BuildVersion = "1.0.0.0",
  Id           = "800979aa-3cec-4fca-815d-bd8d28fe8398",
  Author       = "Bjoern Piffler",
  Description  = "KNX/EIB-Anbindung ueber das b+b EIB-Medientechnik-Gateway IP (ASCII/TCP)."
}

-- ==== Datentyp-Dropdown  <->  Gateway-Format n =============================
-- kind: bit=1-bit, step=4-bit relativ, level=Wert, string=Text
local TYPES = {
  { label = "1-bit [1.*]",                fmt = 1,  kind = "bit",   min = 0,    max = 1 },
  { label = "4-bit [3.*]",                fmt = 1,  kind = "step",  min = 0,    max = 15 },
  { label = "1-Byte 0-100% [5.001]",      fmt = 3,  kind = "level", min = 0,    max = 100 },
  { label = "1-Byte 0-255 [5.004/5.010]", fmt = 5,  kind = "level", min = 0,    max = 255 },
  { label = "1-Byte signed [6.*]",        fmt = 6,  kind = "level", min = -128, max = 127 },
  { label = "2-Byte unsigned [7.*]",      fmt = 7,  kind = "level", min = 0,    max = 65535 },
  { label = "2-Byte signed [8.*]",        fmt = 8,  kind = "level", min = -32768, max = 32767 },
  { label = "2-Byte float [9.*]",         fmt = 9,  kind = "level", min = -671088, max = 670760 },
  { label = "4-Byte unsigned [12.*]",     fmt = 12, kind = "level", min = 0,    max = 4294967295 },
  { label = "4-Byte float [14.*]",        fmt = 14, kind = "level", min = 0,    max = 4294967295 },
  { label = "String 14 [16.*]",           fmt = 16, kind = "string" },
}
local function typeLabels()
  local t = {}
  for _, e in ipairs(TYPES) do t[#t + 1] = e.label end
  return t
end
local function typeByLabel(lbl)
  for _, e in ipairs(TYPES) do if e.label == lbl then return e end end
  return nil
end

-- 10 Beispieladressen (Screenshot) fuer "Defaults laden"
local DEFAULTS = {
  { ga = "7/0/100", t = "1-Byte 0-255 [5.004/5.010]", n = "Kuehldecke Dimmen" },
  { ga = "7/0/101", t = "1-bit [1.*]",                n = "Kuehldecke Schalten" },
  { ga = "7/0/102", t = "1-bit [1.*]",                n = "Zentral EA" },
  { ga = "7/0/103", t = "1-Byte 0-100% [5.001]",      n = "Downlights Dimmen" },
  { ga = "7/0/104", t = "1-bit [1.*]",                n = "Downlights Schalten" },
  { ga = "7/0/106", t = "4-bit [3.*]",                n = "Vorhaenge Innen Zu" },
  { ga = "7/0/107", t = "4-bit [3.*]",                n = "Vorhaenge Innen Auf" },
  { ga = "7/0/7",   t = "1-bit [1.*]",                n = "Belegt" },
  { ga = "7/4/53",  t = "4-bit [3.*]",                n = "Sonnenschutz AufAb" },
  { ga = "7/4/248", t = "4-bit [3.*]",                n = "Sonnenschutz Stopp" },
}

local function clamp(v, lo, hi) if v < lo then return lo elseif v > hi then return hi else return v end end

-- ==== Layout-Farben =======================================================
local C_BG    = { 30, 30, 30 }
local C_GRP   = { 60, 60, 60 }
local C_TXT   = { 210, 210, 210 }
local C_BTN   = { 60, 90, 140 }
local C_ACT   = { 70, 130, 80 }

function GetColor(props) return { 40, 60, 100 } end

function GetPrettyName(props)
  return "EIB Gateway (KNX)  " .. tostring(props["IP Address"].Value) .. ":" .. tostring(props["Port"].Value)
end

function GetPages(props)
  return { { name = "Connection" }, { name = "Setup" }, { name = "Lights/Shades" } }
end

function GetProperties()
  return {
    { Name = "IP Address",    Type = "string",  Value = "192.168.100.104" },
    { Name = "Port",          Type = "integer", Min = 1, Max = 65535, Value = 10001 },
    { Name = "Reconnect (s)", Type = "integer", Min = 1, Max = 120,   Value = 5 },
    { Name = "Debug Print",   Type = "boolean", Value = false },
    { Name = "Address Count", Type = "integer", Min = 1, Max = 64,    Value = 10 },
    { Name = "Light Count",   Type = "integer", Min = 0, Max = 16,    Value = 2 },
    { Name = "Shade Count",   Type = "integer", Min = 0, Max = 16,    Value = 2 },
  }
end

function RectifyProperties(props) return props end

-- ==== Controls ============================================================
function GetControls(props)
  local c = {}
  local function add(t) c[#c + 1] = t end

  -- Global
  add({ Name = "Online",       ControlType = "Indicator", IndicatorType = "Led",  Count = 1, UserPin = true, PinStyle = "Output" })
  add({ Name = "Status",       ControlType = "Indicator", IndicatorType = "Text", Count = 1, UserPin = true, PinStyle = "Output" })
  add({ Name = "Version",      ControlType = "Indicator", IndicatorType = "Text", Count = 1 })
  add({ Name = "Reconnect",    ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Input" })
  add({ Name = "Provision",    ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Input" })
  add({ Name = "RefreshAll",   ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Input" })
  add({ Name = "LoadDefaults", ControlType = "Button", ButtonType = "Trigger", Count = 1 })

  -- Setup rows
  for i = 1, props["Address Count"].Value do
    add({ Name = "GA_"   .. i, ControlType = "Text", Count = 1 })
    add({ Name = "Type_" .. i, ControlType = "Text", Count = 1 })
    add({ Name = "Name_" .. i, ControlType = "Text", Count = 1 })
    add({ Name = "Fb_"   .. i, ControlType = "Indicator", IndicatorType = "Text", Count = 1, UserPin = true, PinStyle = "Output" })
  end

  -- Light blocks
  for j = 1, props["Light Count"].Value do
    local p = "L" .. j .. "_"
    for _, a in ipairs({ "OnOff", "Value", "Dim", "Feedback" }) do
      add({ Name = p .. a, ControlType = "Text", Count = 1 })
    end
    add({ Name = p .. "On",       ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Both" })
    add({ Name = p .. "Off",      ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Both" })
    add({ Name = p .. "Brighter", ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Both" })
    add({ Name = p .. "Darker",   ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Both" })
    add({ Name = p .. "ValueSet", ControlType = "Knob", ControlUnit = "Integer", Min = 0, Max = 255, Count = 1, UserPin = true, PinStyle = "Both" })
    add({ Name = p .. "Fb",       ControlType = "Indicator", IndicatorType = "Led",   Count = 1, UserPin = true, PinStyle = "Output" })
    add({ Name = p .. "FbValue",  ControlType = "Indicator", IndicatorType = "Meter", Min = 0, Max = 255, Count = 1, UserPin = true, PinStyle = "Output" })
  end

  -- Shade blocks
  for k = 1, props["Shade Count"].Value do
    local p = "S" .. k .. "_"
    for _, a in ipairs({ "Up", "Down", "Stop" }) do
      add({ Name = p .. a, ControlType = "Text", Count = 1 })
    end
    add({ Name = p .. "UpBtn",   ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Both" })
    add({ Name = p .. "DownBtn", ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Both" })
    add({ Name = p .. "StopBtn", ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Both" })
  end

  return c
end

-- ==== Layout ==============================================================
function GetControlLayout(props)
  local layout, graphics = {}, {}
  local page = props["page_index"].Value

  local function grp(text, x, y, w, h)
    graphics[#graphics + 1] = { Type = "GroupBox", Text = text, HTextAlign = "Left",
      Position = { x, y }, Size = { w, h }, Color = C_GRP, StrokeWidth = 1, CornerRadius = 6 }
  end
  local function label(text, x, y, w, h, align)
    graphics[#graphics + 1] = { Type = "Text", Text = text, Position = { x, y }, Size = { w, h },
      Color = C_TXT, FontSize = 12, HTextAlign = align or "Left" }
  end

  if page == 1 then
    -- Connection
    grp("Verbindung", 8, 8, 380, 150)
    label("IP:  " .. tostring(props["IP Address"].Value), 20, 34, 360, 18)
    label("Port:  " .. tostring(props["Port"].Value), 20, 56, 360, 18)
    label("Online", 20, 84, 80, 24)
    layout["Online"]  = { Style = "Led",  Position = { 100, 84 }, Size = { 24, 24 }, Color = { 0, 190, 0 } }
    label("Status", 20, 116, 80, 24)
    layout["Status"]  = { Style = "Text", Position = { 100, 114 }, Size = { 270, 24 }, Color = C_TXT }

    grp("Firmware / Aktionen", 8, 168, 380, 150)
    label("Version", 20, 194, 80, 24)
    layout["Version"]      = { Style = "Text",   Position = { 100, 192 }, Size = { 270, 24 }, Color = C_TXT }
    layout["Reconnect"]    = { Style = "Button", Legend = "Reconnect",   Position = { 20, 228 },  Size = { 110, 30 }, Color = C_BTN }
    layout["Provision"]    = { Style = "Button", Legend = "Provision",   Position = { 140, 228 }, Size = { 110, 30 }, Color = C_ACT }
    layout["RefreshAll"]   = { Style = "Button", Legend = "Refresh All", Position = { 260, 228 }, Size = { 110, 30 }, Color = C_BTN }

  elseif page == 2 then
    -- Setup table
    local n = props["Address Count"].Value
    label("Group Address", 40, 10, 130, 18)
    label("Data Type",     180, 10, 200, 18)
    label("Name",          390, 10, 190, 18)
    label("Feedback",      590, 10, 120, 18)
    layout["LoadDefaults"] = { Style = "Button", Legend = "Defaults laden", Position = { 720, 8 }, Size = { 120, 26 }, Color = C_ACT }
    local y0, rh = 34, 26
    for i = 1, n do
      local y = y0 + (i - 1) * rh
      label(tostring(i) .. ".", 8, y + 3, 28, 20, "Right")
      layout["GA_"   .. i] = { Style = "Text",     Position = { 40, y },  Size = { 130, 22 }, Color = C_TXT }
      layout["Type_" .. i] = { Style = "ComboBox", Position = { 180, y }, Size = { 200, 22 }, Color = C_TXT }
      layout["Name_" .. i] = { Style = "Text",     Position = { 390, y }, Size = { 190, 22 }, Color = C_TXT }
      layout["Fb_"   .. i] = { Style = "Text",     Position = { 590, y }, Size = { 120, 22 }, Color = { 150, 180, 150 } }
    end

  else
    -- Lights / Shades
    local x = 8
    for j = 1, props["Light Count"].Value do
      local p = "L" .. j .. "_"
      grp("Light " .. j, x, 8, 300, 300)
      label("On/Off DP",   x + 12, 30,  90, 18)
      layout[p .. "OnOff"]    = { Style = "ComboBox", Position = { x + 110, 28 },  Size = { 178, 22 }, Color = C_TXT }
      label("Value DP",    x + 12, 56,  90, 18)
      layout[p .. "Value"]    = { Style = "ComboBox", Position = { x + 110, 54 },  Size = { 178, 22 }, Color = C_TXT }
      label("Dim DP",      x + 12, 82,  90, 18)
      layout[p .. "Dim"]      = { Style = "ComboBox", Position = { x + 110, 80 },  Size = { 178, 22 }, Color = C_TXT }
      label("Feedback DP", x + 12, 108, 90, 18)
      layout[p .. "Feedback"] = { Style = "ComboBox", Position = { x + 110, 106 }, Size = { 178, 22 }, Color = C_TXT }

      layout[p .. "On"]       = { Style = "Button", Legend = "An",      Position = { x + 12,  140 }, Size = { 84, 34 }, Color = C_ACT }
      layout[p .. "Off"]      = { Style = "Button", Legend = "Aus",     Position = { x + 104, 140 }, Size = { 84, 34 }, Color = C_BTN }
      layout[p .. "Fb"]       = { Style = "Led",    Position = { x + 250, 145 }, Size = { 24, 24 }, Color = { 0, 190, 0 } }
      layout[p .. "Brighter"] = { Style = "Button", Legend = "Heller",  Position = { x + 12,  184 }, Size = { 84, 34 }, Color = C_BTN }
      layout[p .. "Darker"]   = { Style = "Button", Legend = "Dunkler", Position = { x + 104, 184 }, Size = { 84, 34 }, Color = C_BTN }
      label("Wert", x + 12, 228, 60, 18)
      layout[p .. "ValueSet"] = { Style = "Fader",  Position = { x + 60,  226 }, Size = { 228, 30 }, Color = C_BTN }
      label("Fb-Wert", x + 12, 268, 60, 18)
      layout[p .. "FbValue"]  = { Style = "Meter",  Position = { x + 60,  266 }, Size = { 228, 26 }, Color = { 0, 160, 0 } }
      x = x + 312
    end

    x = 8
    local yShade = 320
    for k = 1, props["Shade Count"].Value do
      local p = "S" .. k .. "_"
      grp("Shade " .. k, x, yShade, 300, 190)
      label("Up DP",   x + 12, yShade + 22, 70, 18)
      layout[p .. "Up"]   = { Style = "ComboBox", Position = { x + 90, yShade + 20 }, Size = { 198, 22 }, Color = C_TXT }
      label("Down DP", x + 12, yShade + 48, 70, 18)
      layout[p .. "Down"] = { Style = "ComboBox", Position = { x + 90, yShade + 46 }, Size = { 198, 22 }, Color = C_TXT }
      label("Stop DP", x + 12, yShade + 74, 70, 18)
      layout[p .. "Stop"] = { Style = "ComboBox", Position = { x + 90, yShade + 72 }, Size = { 198, 22 }, Color = C_TXT }
      layout[p .. "UpBtn"]   = { Style = "Button", Legend = "Auf",  Position = { x + 12,  yShade + 110 }, Size = { 86, 40 }, Color = C_BTN }
      layout[p .. "StopBtn"] = { Style = "Button", Legend = "Stop", Position = { x + 104, yShade + 110 }, Size = { 86, 40 }, Color = C_ACT }
      layout[p .. "DownBtn"] = { Style = "Button", Legend = "Ab",   Position = { x + 196, yShade + 110 }, Size = { 86, 40 }, Color = C_BTN }
      x = x + 312
    end
  end

  return layout, graphics
end

--[[ =========================================================================
  RUNTIME
========================================================================= ]]
if Controls then
  -- Sende-Werte fuer relative Aktionen (leicht anpassbar):
  -- 4-bit DPT 3.x: bit3 = Richtung (up), bits0-2 = Schrittzahl (1 = Langlauf).
  local DIM_UP, DIM_DOWN = 9, 1               -- Heller / Dunkler
  local SHD_UP, SHD_DOWN, SHD_STOP = 9, 1, 0  -- Auf / Ab / Stop (4-bit)

  local AC = 0
  while Controls["GA_" .. (AC + 1)] do AC = AC + 1 end
  local LC = 0
  while Controls["L" .. (LC + 1) .. "_On"] do LC = LC + 1 end
  local SC = 0
  while Controls["S" .. (SC + 1) .. "_UpBtn"] do SC = SC + 1 end

  local IP   = Properties["IP Address"].Value
  local PORT = tonumber(Properties["Port"].Value) or 10001
  local RECON = tonumber(Properties["Reconnect (s)"].Value) or 5
  local DBG  = Properties["Debug Print"].Value

  local function dbg(s) if DBG then print(s) end end

  local sock = TcpSocket.New()
  sock.ReadTimeout = 0
  sock.ReconnectTimeout = RECON

  local rxbuf = ""
  local nameToDP = {}   -- name -> {ga, fmt, kind, min, max}
  local gaToRow  = {}   -- canonGA -> setup row index
  local gaValue  = {}   -- canonGA -> last value string

  local function s(name) local c = Controls[name]; return (c and c.String) or "" end

  -- "H/M/U" (mit/ohne Nullen) -> kanonisch "h/m/u"
  local function canon(ga)
    local h, m, u = tostring(ga):match("(%d+)%s*/%s*(%d+)%s*/%s*(%d+)")
    if not h then return nil end
    return tonumber(h) .. "/" .. tonumber(m) .. "/" .. tonumber(u)
  end

  -- ---- Sende-Warteschlange (entzerrt Provisionierung) ----
  local txq = {}
  local txTimer = Timer.New()
  txTimer.EventHandler = function()
    if #txq > 0 then
      local cmd = table.remove(txq, 1)
      if sock.IsConnected then sock:Write(cmd .. "\r") end
      dbg("TX: " .. cmd)
    else
      txTimer:Stop()
    end
  end
  local function tx(cmd) txq[#txq + 1] = cmd; txTimer:Start(0.03) end

  -- ---- Maps aus Setup-Tabelle aufbauen + Dropdowns fuellen ----
  local function rebuildMaps()
    nameToDP, gaToRow = {}, {}
    local names = {}
    for i = 1, AC do
      local ga  = canon(s("GA_" .. i))
      local te  = typeByLabel(s("Type_" .. i))
      local nm  = s("Name_" .. i)
      if ga and te then
        gaToRow[ga] = i
        if nm ~= "" then
          nameToDP[nm] = { ga = ga, fmt = te.fmt, kind = te.kind, min = te.min, max = te.max }
          names[#names + 1] = nm
        end
      end
    end
    table.sort(names)
    local choices = { "<none>" }
    for _, nm in ipairs(names) do choices[#choices + 1] = nm end
    local function setChoices(name)
      local c = Controls[name]
      if c then c.Choices = choices end
    end
    for j = 1, LC do
      for _, a in ipairs({ "OnOff", "Value", "Dim", "Feedback" }) do setChoices("L" .. j .. "_" .. a) end
    end
    for k = 1, SC do
      for _, a in ipairs({ "Up", "Down", "Stop" }) do setChoices("S" .. k .. "_" .. a) end
    end
  end

  -- ---- Feedback anwenden ----
  local function applyFeedback(cga, valStr)
    gaValue[cga] = valStr
    local row = gaToRow[cga]
    if row then Controls["Fb_" .. row].String = valStr end
    local num = tonumber(valStr) or 0
    for j = 1, LC do
      local fb  = s("L" .. j .. "_Feedback")
      local on  = s("L" .. j .. "_OnOff")
      local vd  = s("L" .. j .. "_Value")
      local ledSrc = (fb ~= "" and fb) or (on ~= "" and on) or nil
      if ledSrc and nameToDP[ledSrc] and nameToDP[ledSrc].ga == cga then
        Controls["L" .. j .. "_Fb"].Boolean = num ~= 0
      end
      if vd ~= "" and nameToDP[vd] and nameToDP[vd].ga == cga then
        Controls["L" .. j .. "_FbValue"].Value = num
      end
    end
  end

  local function refreshAllFeedback()
    for cga, v in pairs(gaValue) do applyFeedback(cga, v) end
  end

  -- ---- Empfang parsen ----
  local function handleLine(line)
    line = line:gsub("^%s+", ""):gsub("%s+$", "")
    if line == "" then return end
    local h, m, u, val = line:match("(%d+)/(%d+)/(%d+)%s*=%s*(.*)")
    if h then
      local cga = tonumber(h) .. "/" .. tonumber(m) .. "/" .. tonumber(u)
      if val ~= "*" then applyFeedback(cga, val) end
      dbg("RX GA " .. cga .. " = " .. tostring(val))
    elseif line:match("^EIB_Terminal") then
      Controls.Version.String = line
      dbg("RX: " .. line)
    else
      Controls.Status.String = line
      dbg("RX: " .. line)
    end
  end

  -- ---- Provisionierung ----
  local function provisionAll()
    rebuildMaps()
    for i = 1, AC do
      local ga = canon(s("GA_" .. i))
      local te = typeByLabel(s("Type_" .. i))
      if ga and te then
        tx("S" .. ga .. ":" .. te.fmt .. ",E")
        tx("R" .. ga)
      end
    end
  end

  -- ---- Schreiben ----
  local function writeName(name, value)
    local dp = nameToDP[name]
    if not dp then dbg("write: unbekannter DP '" .. tostring(name) .. "'"); return end
    tx("W" .. dp.ga .. "=" .. tostring(value))
  end

  -- ---- Socket-Events ----
  sock.Connected = function()
    Controls.Online.Boolean = true
    Controls.Status.String = "Verbunden"
    tx("OV+D+E-G+H-N-Q+R+S-X-")   -- Optionen normalisieren (dezimal, kein Echo/Quelle)
    tx("?V")
    provisionAll()
  end
  sock.Reconnect = function() Controls.Status.String = "Reconnect..." end
  sock.Closed  = function() Controls.Online.Boolean = false; Controls.Status.String = "Getrennt" end
  sock.Error   = function(_, err) Controls.Online.Boolean = false; Controls.Status.String = "Fehler: " .. tostring(err) end
  sock.Timeout = function() Controls.Status.String = "Timeout" end
  sock.Data = function()
    rxbuf = rxbuf .. sock:Read(sock.BufferLength)
    while true do
      local p = rxbuf:find("\r")
      if not p then break end
      local line = rxbuf:sub(1, p - 1)
      rxbuf = rxbuf:sub(p + 1)
      handleLine(line)
    end
  end

  local function connect()
    Controls.Status.String = "Verbinde " .. IP .. ":" .. PORT
    sock:Connect(IP, PORT)
  end

  -- ---- Control-Handler ----
  Controls.Reconnect.EventHandler    = function() sock:Disconnect(); connect() end
  Controls.Provision.EventHandler    = function() provisionAll() end
  Controls.RefreshAll.EventHandler    = function()
    for i = 1, AC do local ga = canon(s("GA_" .. i)); if ga then tx("R" .. ga) end end
  end
  Controls.LoadDefaults.EventHandler = function()
    for i = 1, math.min(AC, #DEFAULTS) do
      Controls["GA_" .. i].String   = DEFAULTS[i].ga
      Controls["Type_" .. i].String = DEFAULTS[i].t
      Controls["Name_" .. i].String = DEFAULTS[i].n
    end
    rebuildMaps()
    if sock.IsConnected then provisionAll() end
  end

  -- Setup-Aenderungen -> Maps neu
  for i = 1, AC do
    for _, f in ipairs({ "GA_", "Type_", "Name_" }) do
      local c = Controls[f .. i]
      if c then c.EventHandler = function() rebuildMaps(); refreshAllFeedback() end end
    end
  end

  -- Zuordnungs-Dropdowns -> Feedback neu bewerten
  for j = 1, LC do
    for _, a in ipairs({ "OnOff", "Value", "Dim", "Feedback" }) do
      local c = Controls["L" .. j .. "_" .. a]
      if c then c.EventHandler = function() refreshAllFeedback() end end
    end
  end

  -- Light-Bloecke
  for j = 1, LC do
    local p = "L" .. j .. "_"
    Controls[p .. "On"].EventHandler       = function() writeName(s(p .. "OnOff"), 1) end
    Controls[p .. "Off"].EventHandler      = function() writeName(s(p .. "OnOff"), 0) end
    Controls[p .. "Brighter"].EventHandler = function() writeName(s(p .. "Dim"), DIM_UP) end
    Controls[p .. "Darker"].EventHandler   = function() writeName(s(p .. "Dim"), DIM_DOWN) end
    Controls[p .. "ValueSet"].EventHandler = function(ctl)
      local nm = s(p .. "Value"); local dp = nameToDP[nm]
      local v = math.floor(ctl.Value + 0.5)
      if dp and dp.min and dp.max then v = clamp(v, dp.min, dp.max) end
      writeName(nm, v)
    end
  end

  -- Shade-Bloecke
  local function shadeSend(name, action)
    local dp = nameToDP[name]
    if not dp then return end
    local v
    if dp.kind == "step" then
      v = (action == "Up" and SHD_UP) or (action == "Down" and SHD_DOWN) or SHD_STOP
    elseif dp.kind == "bit" then
      v = (action == "Stop") and 1 or 1     -- getrennte GA je Aktion: Trigger=1
    else
      v = (action == "Up" and 1) or (action == "Down" and 0) or 0
    end
    writeName(name, v)
  end
  for k = 1, SC do
    local p = "S" .. k .. "_"
    Controls[p .. "UpBtn"].EventHandler   = function() shadeSend(s(p .. "Up"),   "Up") end
    Controls[p .. "DownBtn"].EventHandler = function() shadeSend(s(p .. "Down"), "Down") end
    Controls[p .. "StopBtn"].EventHandler = function() shadeSend(s(p .. "Stop"), "Stop") end
  end

  -- ---- Init ----
  Controls.Online.Boolean = false
  for i = 1, AC do Controls["Type_" .. i].Choices = typeLabels() end
  rebuildMaps()
  connect()
end
