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

-- ==== Layout-Farben: Catppuccin Mocha (fest, theme-unabhaengig) ===========
local C_BG    = { 30, 30, 46 }    -- base   Seitenhintergrund
local C_GRP   = { 88, 91, 112 }   -- surface2  Rahmen
local C_TXT   = { 205, 214, 244 } -- text
local C_SUB   = { 166, 173, 200 } -- subtext
local C_BTN   = { 69, 71, 90 }    -- surface1  neutrale Buttons
local C_ACT   = { 166, 227, 161 } -- green   positiv/primaer
local C_BLUE  = { 137, 180, 250 } -- blue
local C_MAUVE = { 203, 166, 247 } -- mauve
local C_PEACH = { 250, 179, 135 } -- peach   Stop
local C_RED   = { 243, 139, 168 } -- red
local C_LED   = { 166, 227, 161 } -- green   LED an

function GetColor(props) return { 40, 60, 100 } end

function GetPrettyName(props)
  return "EIB Gateway (KNX)  " .. tostring(props["IP Address"].Value) .. ":" .. tostring(props["Port"].Value)
end

function GetPages(props)
  return { { name = "Connection" }, { name = "Setup" }, { name = "Lights/Shades" }, { name = "Debug" } }
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
  add({ Name = "IPAddress",    ControlType = "Text", Count = 1, UserPin = true, PinStyle = "Both" })
  add({ Name = "Port",         ControlType = "Text", Count = 1, UserPin = true, PinStyle = "Both" })
  add({ Name = "Online",       ControlType = "Indicator", IndicatorType = "Led",  Count = 1, UserPin = true, PinStyle = "Output" })
  add({ Name = "Status",       ControlType = "Indicator", IndicatorType = "Text", Count = 1, UserPin = true, PinStyle = "Output" })
  add({ Name = "Version",      ControlType = "Indicator", IndicatorType = "Text", Count = 1 })
  add({ Name = "Reconnect",    ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Input" })
  add({ Name = "Provision",    ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Input" })
  add({ Name = "RefreshAll",   ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Input" })
  add({ Name = "LoadDefaults", ControlType = "Button", ButtonType = "Trigger", Count = 1 })
  add({ Name = "DebugLog",     ControlType = "Indicator", IndicatorType = "Text", Count = 1, UserPin = true, PinStyle = "Output" })
  add({ Name = "ClearLog",     ControlType = "Button", ButtonType = "Trigger", Count = 1 })

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
    for _, a in ipairs({ "OnOff", "OnOffFb", "Value", "Dim" }) do
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

  -- Shade blocks (ein Datenpunkt, i.d.R. 4-bit)
  for k = 1, props["Shade Count"].Value do
    local p = "S" .. k .. "_"
    add({ Name = p .. "DP", ControlType = "Text", Count = 1 })
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

  -- Fester dunkler Seitenhintergrund (garantiert Kontrast, theme-unabhaengig)
  graphics[#graphics + 1] = { Type = "GroupBox", Position = { 0, 0 }, Size = { 1060, 620 },
    Fill = C_BG, StrokeWidth = 0, CornerRadius = 0 }

  local function grp(text, x, y, w, h)
    graphics[#graphics + 1] = { Type = "GroupBox", Text = text, HTextAlign = "Left",
      Position = { x, y }, Size = { w, h }, Color = C_TXT, StrokeColor = C_GRP, StrokeWidth = 1, CornerRadius = 6 }
  end
  local function label(text, x, y, w, h, align)
    graphics[#graphics + 1] = { Type = "Label", Text = text, Position = { x, y }, Size = { w, h },
      Color = C_TXT, FontSize = 12, HTextAlign = align or "Left" }
  end

  if page == 1 then
    -- Connection
    grp("Verbindung", 8, 8, 400, 150)
    label("IP-Adresse", 20, 36, 90, 20)
    layout["IPAddress"] = { Style = "Text", Position = { 120, 34 }, Size = { 180, 24 }, Color = C_TXT }
    label("Port", 20, 64, 90, 20)
    layout["Port"]      = { Style = "Text", Position = { 120, 62 }, Size = { 90, 24 }, Color = C_TXT }
    label("Online", 20, 94, 90, 24)
    layout["Online"]  = { Style = "Led",  Position = { 120, 94 }, Size = { 24, 24 }, Color = C_LED }
    label("Status", 20, 124, 90, 24)
    layout["Status"]  = { Style = "Text", Position = { 120, 122 }, Size = { 280, 24 }, Color = C_TXT }

    grp("Firmware / Aktionen", 8, 168, 400, 150)
    label("Version", 20, 194, 90, 24)
    layout["Version"]      = { Style = "Text",   Position = { 120, 192 }, Size = { 280, 24 }, Color = C_TXT }
    layout["Reconnect"]    = { Style = "Button", Legend = "Reconnect",   Position = { 20, 228 },  Size = { 110, 30 }, Color = C_BLUE }
    layout["Provision"]    = { Style = "Button", Legend = "Provision",   Position = { 140, 228 }, Size = { 110, 30 }, Color = C_ACT }
    layout["RefreshAll"]   = { Style = "Button", Legend = "Refresh All", Position = { 260, 228 }, Size = { 110, 30 }, Color = C_BTN }

  elseif page == 2 then
    -- Setup table
    local n = props["Address Count"].Value
    label("Group Address", 40, 10, 130, 18)
    label("Data Type",     180, 10, 200, 18)
    label("Name",          390, 10, 190, 18)
    label("Feedback",      590, 10, 120, 18)
    layout["LoadDefaults"] = { Style = "Button", Legend = "Defaults laden", Position = { 720, 8 }, Size = { 120, 26 }, Color = C_MAUVE }
    local y0, rh = 34, 26
    for i = 1, n do
      local y = y0 + (i - 1) * rh
      label(tostring(i) .. ".", 8, y + 3, 28, 20, "Right")
      layout["GA_"   .. i] = { Style = "Text",     Position = { 40, y },  Size = { 130, 22 }, Color = C_TXT }
      layout["Type_" .. i] = { Style = "ComboBox", Position = { 180, y }, Size = { 200, 22 }, Color = C_TXT }
      layout["Name_" .. i] = { Style = "Text",     Position = { 390, y }, Size = { 190, 22 }, Color = C_TXT }
      layout["Fb_"   .. i] = { Style = "Text",     Position = { 590, y }, Size = { 120, 22 }, Color = C_ACT }
    end

  elseif page == 3 then
    -- Lights / Shades als Tabellen
    -- Spalten (x, w)
    local cIdx  = { 8,   30 }
    local cOn   = { 44,  110 }   -- On/Off DP
    local cOnFb = { 160, 110 }   -- On/Off Fb DP
    local cVal  = { 276, 110 }   -- Value DP
    local cDim  = { 392, 110 }   -- Dim DP
    local cAn   = { 508, 50 }
    local cAus  = { 562, 50 }
    local cHel  = { 616, 58 }
    local cDun  = { 678, 58 }
    local cWert = { 740, 150 }   -- Fader
    local cLed  = { 896, 24 }
    local cMet  = { 926, 92 }    -- Meter
    local rh = 30

    -- Lights: Titel + Spaltenkopf
    label("Lights", 8, 8, 200, 18)
    local hL = 30
    label("#",         cIdx[1],  hL, cIdx[2],  16, "Center")
    label("On/Off DP", cOn[1],   hL, cOn[2],   16)
    label("On/Off Fb", cOnFb[1], hL, cOnFb[2], 16)
    label("Value DP",  cVal[1],  hL, cVal[2],  16)
    label("Dim DP",    cDim[1],  hL, cDim[2],  16)
    label("An",        cAn[1],   hL, cAn[2],   16, "Center")
    label("Aus",       cAus[1],  hL, cAus[2],  16, "Center")
    label("Heller",    cHel[1],  hL, cHel[2],  16, "Center")
    label("Dunkler",   cDun[1],  hL, cDun[2],  16, "Center")
    label("Wert",      cWert[1], hL, cWert[2], 16, "Center")
    label("Fb",        cLed[1],  hL, cLed[2],  16, "Center")
    label("Fb-Wert",   cMet[1],  hL, cMet[2],  16, "Center")

    local lc = props["Light Count"].Value
    local y0 = hL + 22
    for j = 1, lc do
      local p, y = "L" .. j .. "_", y0 + (j - 1) * rh
      label(tostring(j), cIdx[1], y + 4, cIdx[2], 18, "Center")
      layout[p .. "OnOff"]    = { Style = "ComboBox", Position = { cOn[1],   y }, Size = { cOn[2],   24 }, Color = C_TXT }
      layout[p .. "OnOffFb"]  = { Style = "ComboBox", Position = { cOnFb[1], y }, Size = { cOnFb[2], 24 }, Color = C_TXT }
      layout[p .. "Value"]    = { Style = "ComboBox", Position = { cVal[1],  y }, Size = { cVal[2],  24 }, Color = C_TXT }
      layout[p .. "Dim"]      = { Style = "ComboBox", Position = { cDim[1],  y }, Size = { cDim[2],  24 }, Color = C_TXT }
      layout[p .. "On"]       = { Style = "Button", Legend = "An",  Position = { cAn[1],  y }, Size = { cAn[2],  26 }, Color = C_ACT }
      layout[p .. "Off"]      = { Style = "Button", Legend = "Aus", Position = { cAus[1], y }, Size = { cAus[2], 26 }, Color = C_BTN }
      layout[p .. "Brighter"] = { Style = "Button", Legend = "+",   Position = { cHel[1], y }, Size = { cHel[2], 26 }, Color = C_BTN }
      layout[p .. "Darker"]   = { Style = "Button", Legend = "-",   Position = { cDun[1], y }, Size = { cDun[2], 26 }, Color = C_BTN }
      layout[p .. "ValueSet"] = { Style = "Fader",  Position = { cWert[1], y }, Size = { cWert[2], 26 }, Color = C_BLUE }
      layout[p .. "Fb"]       = { Style = "Led",    Position = { cLed[1], y }, Size = { cLed[2], 24 }, Color = C_LED }
      layout[p .. "FbValue"]  = { Style = "Meter",  Position = { cMet[1], y }, Size = { cMet[2], 24 }, Color = C_ACT }
    end

    -- Shades-Tabelle unterhalb
    local yTitle = y0 + lc * rh + 16
    local yH = yTitle + 22
    local yS = yH + 22
    label("Shades", 8, yTitle, 200, 18)
    label("#",                  cIdx[1], yH, cIdx[2], 16, "Center")
    label("Datenpunkt (4-bit)", cOn[1],  yH, 240, 16)
    label("Auf",  cVal[1],       yH, 60, 16, "Center")
    label("Stop", cVal[1] + 66,  yH, 60, 16, "Center")
    label("Ab",   cVal[1] + 132, yH, 60, 16, "Center")
    for k = 1, props["Shade Count"].Value do
      local p, y = "S" .. k .. "_", yS + (k - 1) * rh
      label(tostring(k), cIdx[1], y + 4, cIdx[2], 18, "Center")
      layout[p .. "DP"]      = { Style = "ComboBox", Position = { cOn[1], y }, Size = { 226, 24 }, Color = C_TXT }
      layout[p .. "UpBtn"]   = { Style = "Button", Legend = "Auf",  Position = { cVal[1],       y }, Size = { 60, 26 }, Color = C_BTN }
      layout[p .. "StopBtn"] = { Style = "Button", Legend = "Stop", Position = { cVal[1] + 66,  y }, Size = { 60, 26 }, Color = C_PEACH }
      layout[p .. "DownBtn"] = { Style = "Button", Legend = "Ab",   Position = { cVal[1] + 132, y }, Size = { 60, 26 }, Color = C_BTN }
    end

  else
    -- Debug: TX/RX-Log
    label("TX / RX Log", 8, 8, 300, 18)
    layout["ClearLog"] = { Style = "Button", Legend = "Clear", Position = { 720, 8 }, Size = { 120, 26 }, Color = C_RED }
    layout["DebugLog"] = { Style = "Text", Position = { 8, 36 }, Size = { 832, 520 }, Color = C_TXT,
      HTextAlign = "Left", VTextAlign = "Top", WordWrap = true }
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

  local DEF_IP   = Properties["IP Address"].Value
  local DEF_PORT = tostring(Properties["Port"].Value)
  local RECON = tonumber(Properties["Reconnect (s)"].Value) or 5
  local DBG  = Properties["Debug Print"].Value

  -- IP/Port zur Laufzeit aus den Controls (Default aus den Properties)
  local function curIP()   local v = Controls.IPAddress.String; return (v ~= "" and v) or DEF_IP end
  local function curPort() return tonumber(Controls.Port.String) or tonumber(DEF_PORT) or 10001 end

  local function dbg(s) if DBG then print(s) end end

  -- ---- Debug-Log (TX/RX) fuer die Debug-Seite ----
  local LOGN = 80
  local logLines = {}
  local function stamp()
    local t = os.date("*t")
    return string.format("%02d:%02d:%02d", t.hour, t.min, t.sec)
  end
  local function logAdd(dir, msg)
    table.insert(logLines, 1, stamp() .. "  " .. dir .. "  " .. tostring(msg))
    while #logLines > LOGN do table.remove(logLines) end
    Controls.DebugLog.String = table.concat(logLines, "\n")
  end

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
      if sock.IsConnected then sock:Write(cmd .. "\r"); logAdd("TX >", cmd) else logAdd("TX (offen)", cmd) end
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
      for _, a in ipairs({ "OnOff", "OnOffFb", "Value", "Dim" }) do setChoices("L" .. j .. "_" .. a) end
    end
    for k = 1, SC do setChoices("S" .. k .. "_DP") end
  end

  -- ---- Feedback anwenden ----
  local function applyFeedback(cga, valStr)
    gaValue[cga] = valStr
    local row = gaToRow[cga]
    if row then Controls["Fb_" .. row].String = valStr end
    local num = tonumber(valStr) or 0
    for j = 1, LC do
      local fb  = s("L" .. j .. "_OnOffFb")
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
    logAdd("RX <", line)
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
    local ip, port = curIP(), curPort()
    Controls.Status.String = "Verbinde " .. ip .. ":" .. port
    sock:Connect(ip, port)
  end

  -- ---- Control-Handler ----
  Controls.Reconnect.EventHandler    = function() sock:Disconnect(); connect() end
  Controls.IPAddress.EventHandler    = function() sock:Disconnect(); connect() end
  Controls.Port.EventHandler         = function() sock:Disconnect(); connect() end
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
  Controls.ClearLog.EventHandler = function() logLines = {}; Controls.DebugLog.String = "" end

  -- Setup-Aenderungen -> Maps neu
  for i = 1, AC do
    for _, f in ipairs({ "GA_", "Type_", "Name_" }) do
      local c = Controls[f .. i]
      if c then c.EventHandler = function() rebuildMaps(); refreshAllFeedback() end end
    end
  end

  -- Zuordnungs-Dropdowns -> Feedback neu bewerten
  for j = 1, LC do
    for _, a in ipairs({ "OnOff", "OnOffFb", "Value", "Dim" }) do
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
    if dp.kind == "step" then          -- Normalfall: ein 4-bit-DP fuer Auf/Ab/Stop
      v = (action == "Up" and SHD_UP) or (action == "Down" and SHD_DOWN) or SHD_STOP
    elseif dp.kind == "bit" then       -- 1-bit UpDown (DPT 1.008): 0=Auf, 1=Ab, Stop=1
      v = (action == "Up" and 0) or 1
    else
      v = (action == "Up" and 1) or (action == "Down" and 0) or 0
    end
    writeName(name, v)
  end
  for k = 1, SC do
    local p = "S" .. k .. "_"
    Controls[p .. "UpBtn"].EventHandler   = function() shadeSend(s(p .. "DP"), "Up") end
    Controls[p .. "DownBtn"].EventHandler = function() shadeSend(s(p .. "DP"), "Down") end
    Controls[p .. "StopBtn"].EventHandler = function() shadeSend(s(p .. "DP"), "Stop") end
  end

  -- ---- Init ----
  Controls.Online.Boolean = false
  Controls.DebugLog.String = ""
  if Controls.IPAddress.String == "" then Controls.IPAddress.String = DEF_IP end
  if Controls.Port.String == "" then Controls.Port.String = DEF_PORT end
  for i = 1, AC do Controls["Type_" .. i].Choices = typeLabels() end
  rebuildMaps()
  connect()
end
