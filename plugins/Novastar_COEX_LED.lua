--[[ ===========================================================================
  NovaStar COEX LED-Controller  -->  Q-Sys Plugin
  ---------------------------------------------------------------------------
  Binaeres Frame-Protokoll ueber TCP (COEX Central Control Protocol).
  Controller = TCP-Server (Port 5200), Q-Sys = TCP-Client.

  Gilt fuer COEX-All-in-One-Controller:
    MX40 Pro, MX30, MX20, KU20, CX40 Pro, MX6000 Pro, MX2000 Pro.

  Frame:  [55 AA] [Protocol-Content] [CS_L CS_H]
  Checksumme: Summe aller Bytes ab Index 2 (alles nach 55 AA, inkl. Datenbyte)
              + 0x5555, & 0xFFFF, dann Low-Byte, High-Byte (little-endian).

  Funktionen (fest verdrahtet, gegen Doc-Beispiele verifiziert):
    Helligkeit   0..255  (Reg 0x02000001)  ... 00 -> CS 55 5A
    Blackout     FF/00   (Reg 0x02001000)  ... FF -> CS 54 5B
    Freeze       FF/00   (Reg 0x02001002)  ... FF -> CS 56 5B

  Drei Seiten: 1) Connection  2) Control  3) Debug
=========================================================================== ]]

PluginInfo = {
  Name         = "NovaStar~COEX LED Controller",
  Version      = "1.0",
  BuildVersion = "1.0.0.0",
  Id           = "4b951e80-97ec-4953-8915-490f86e95f95",
  Author       = "Bjoern Piffler",
  Description  = "NovaStar COEX LED-Controller (MX/CX/KU) ueber TCP: Helligkeit, Blackout, Freeze."
}

-- ==== Kommando-Templates (55 AA + Content, ohne Checksumme) ================
-- Nur das letzte Datenbyte variiert; Checksumme wird in frame() berechnet.
local T_BRIGHT = { 0x55,0xaa,0x00,0x00,0xfe,0xff,0x01,0xff,0xff,0xff,0x01,0x00,0x01,0x00,0x00,0x02,0x01,0x00 }
local T_BLACK  = { 0x55,0xaa,0x00,0x00,0xfe,0xff,0x01,0xff,0xff,0xff,0x01,0x00,0x00,0x01,0x00,0x02,0x01,0x00 }
local T_FREEZE = { 0x55,0xaa,0x00,0x00,0xfe,0xff,0x01,0xff,0xff,0xff,0x01,0x00,0x02,0x01,0x00,0x02,0x01,0x00 }

local function clamp(v, lo, hi) if v < lo then return lo elseif v > hi then return hi else return v end end

-- ==== Layout-Farben: Catppuccin Mocha (fest, theme-unabhaengig) ===========
local C_BG    = { 30, 30, 46 }
local C_GRP   = { 88, 91, 112 }
local C_TXT   = { 205, 214, 244 }
local C_BTN   = { 69, 71, 90 }
local C_ACT   = { 166, 227, 161 }
local C_BLUE  = { 137, 180, 250 }
local C_PEACH = { 250, 179, 135 }
local C_RED   = { 243, 139, 168 }
local C_LED   = { 166, 227, 161 }

function GetColor(props) return { 40, 60, 100 } end

function GetPrettyName(props)
  return "NovaStar COEX  " .. tostring(props["IP Address"].Value) .. ":" .. tostring(props["Port"].Value)
end

function GetPages(props)
  return { { name = "Connection" }, { name = "Control" }, { name = "Debug" } }
end

function GetProperties()
  return {
    { Name = "IP Address",    Type = "string",  Value = "192.168.100.105" },
    { Name = "Port",          Type = "integer", Min = 1, Max = 65535, Value = 5200 },
    { Name = "Reconnect (s)", Type = "integer", Min = 1, Max = 120,   Value = 5 },
    { Name = "Debug Print",   Type = "boolean", Value = false },
  }
end

function RectifyProperties(props) return props end

-- ==== Controls ============================================================
function GetControls(props)
  local c = {}
  local function add(t) c[#c + 1] = t end

  add({ Name = "IPAddress",  ControlType = "Text", Count = 1, UserPin = true, PinStyle = "Both" })
  add({ Name = "Port",       ControlType = "Text", Count = 1, UserPin = true, PinStyle = "Both" })
  add({ Name = "Online",     ControlType = "Indicator", IndicatorType = "Led",  Count = 1, UserPin = true, PinStyle = "Output" })
  add({ Name = "Status",     ControlType = "Indicator", IndicatorType = "Text", Count = 1, UserPin = true, PinStyle = "Output" })
  add({ Name = "Reconnect",  ControlType = "Button", ButtonType = "Trigger", Count = 1, UserPin = true, PinStyle = "Input" })

  add({ Name = "Brightness",   ControlType = "Knob", ControlUnit = "Integer", Min = 0, Max = 100, Count = 1, UserPin = true, PinStyle = "Both" })
  add({ Name = "BrightnessFb", ControlType = "Indicator", IndicatorType = "Text", Count = 1, UserPin = true, PinStyle = "Output" })
  add({ Name = "Blackout",     ControlType = "Button", ButtonType = "Toggle", Count = 1, UserPin = true, PinStyle = "Both" })
  add({ Name = "Freeze",       ControlType = "Button", ButtonType = "Toggle", Count = 1, UserPin = true, PinStyle = "Both" })

  add({ Name = "DebugLog", ControlType = "Indicator", IndicatorType = "Text", Count = 1, UserPin = true, PinStyle = "Output" })
  add({ Name = "ClearLog", ControlType = "Button", ButtonType = "Trigger", Count = 1 })

  return c
end

-- ==== Layout ==============================================================
function GetControlLayout(props)
  local layout, graphics = {}, {}
  local page = props["page_index"].Value

  graphics[#graphics + 1] = { Type = "GroupBox", Position = { 0, 0 }, Size = { 860, 400 },
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
    grp("Verbindung (TCP, Port 5200)", 8, 8, 420, 170)
    label("IP-Adresse", 20, 40, 90, 20)
    layout["IPAddress"] = { Style = "Text", Position = { 120, 38 }, Size = { 200, 24 }, Color = C_TXT }
    label("Port", 20, 70, 90, 20)
    layout["Port"]      = { Style = "Text", Position = { 120, 68 }, Size = { 90, 24 }, Color = C_TXT }
    label("Online", 20, 102, 90, 24)
    layout["Online"]  = { Style = "Led",  Position = { 120, 102 }, Size = { 24, 24 }, Color = C_LED }
    label("Status", 20, 134, 90, 24)
    layout["Status"]  = { Style = "Text", Position = { 120, 132 }, Size = { 290, 24 }, Color = C_TXT }
    layout["Reconnect"] = { Style = "Button", Legend = "Reconnect", Position = { 20, 168 - 4 + 24 }, Size = { 120, 30 }, Color = C_BLUE }

  elseif page == 2 then
    -- Control
    grp("LED-Wand", 8, 8, 420, 240)

    label("Helligkeit", 20, 44, 100, 24)
    layout["Brightness"]   = { Style = "Fader", Position = { 120, 44 }, Size = { 230, 26 }, Color = C_BLUE }
    layout["BrightnessFb"] = { Style = "Text",  Position = { 356, 44 }, Size = { 56, 26 }, Color = C_ACT, HTextAlign = "Center" }

    label("Blackout", 20, 108, 100, 30)
    layout["Blackout"] = { Style = "Button", Legend = "Blackout", Position = { 120, 104 }, Size = { 130, 40 }, Color = C_RED }

    label("Freeze", 20, 168, 100, 30)
    layout["Freeze"]   = { Style = "Button", Legend = "Freeze", Position = { 120, 164 }, Size = { 130, 40 }, Color = C_PEACH }

  else
    -- Debug
    label("TX / RX Log", 8, 8, 300, 18)
    layout["ClearLog"] = { Style = "Button", Legend = "Clear", Position = { 720, 8 }, Size = { 120, 26 }, Color = C_RED }
    layout["DebugLog"] = { Style = "Text", Position = { 8, 36 }, Size = { 832, 340 }, Color = C_TXT,
      HTextAlign = "Left", VTextAlign = "Top", WordWrap = true }
  end

  return layout, graphics
end

--[[ =========================================================================
  RUNTIME
========================================================================= ]]
if Controls then
  local DEBUG = (Properties and Properties["Debug Print"] and Properties["Debug Print"].Value) or false
  local RECON = (Properties and Properties["Reconnect (s)"] and Properties["Reconnect (s)"].Value) or 5

  local function dbg(s) if DEBUG then print(s) end end

  -- ---- Debug-Log (rollierend) ----
  local logLines = {}
  local function logAdd(tag, hex)
    local ts = os.date("%H:%M:%S")
    logLines[#logLines + 1] = ts .. "  " .. tag .. "  " .. hex
    while #logLines > 200 do table.remove(logLines, 1) end
    Controls.DebugLog.String = table.concat(logLines, "\n")
  end
  local function toHex(str)
    local t = {}
    for i = 1, #str do t[i] = string.format("%02x", str:byte(i)) end
    return table.concat(t, " ")
  end

  -- ---- Frame bauen: Template-Bytes + Datenbyte + Checksumme -> Binaerstring
  local function frame(tmpl, dataByte)
    local sum = 0
    -- Content = ab Index 3 (Lua) = alles nach 55 AA, plus das Datenbyte
    for i = 3, #tmpl do sum = sum + tmpl[i] end
    sum = sum + dataByte
    sum = (sum + 0x5555) & 0xFFFF
    local bytes = {}
    for i = 1, #tmpl do bytes[i] = tmpl[i] end
    bytes[#bytes + 1] = dataByte
    bytes[#bytes + 1] = sum & 0xFF
    bytes[#bytes + 1] = (sum >> 8) & 0xFF
    local s = {}
    for i = 1, #bytes do s[i] = string.char(bytes[i]) end
    return table.concat(s)
  end

  local function curIP()   return (Controls.IPAddress.String ~= "" and Controls.IPAddress.String)
                                  or tostring(Properties["IP Address"].Value) end
  local function curPort() return tonumber(Controls.Port.String) or tonumber(Properties["Port"].Value) or 5200 end

  local sock = TcpSocket.New()
  sock.ReadTimeout = 0
  sock.ReconnectTimeout = RECON

  local function send(tmpl, dataByte)
    local pkt = frame(tmpl, dataByte)
    if sock.IsConnected then
      sock:Write(pkt)
      logAdd("TX >", toHex(pkt))
    else
      logAdd("TX (offen)", toHex(pkt))
    end
    dbg("TX: " .. toHex(pkt))
  end

  -- ---- Aktionen ----
  local function sendBrightness()
    local pct = clamp(math.floor((tonumber(Controls.Brightness.Value) or 0) + 0.5), 0, 100)
    local xx  = math.floor(pct / 100 * 255 + 0.5)
    Controls.BrightnessFb.String = pct .. " %"
    send(T_BRIGHT, xx)
  end
  -- Fader entprellen (schnelle Bewegungen zusammenfassen)
  local brTimer = Timer.New()
  brTimer.EventHandler = function() brTimer:Stop(); sendBrightness() end

  local function sendBlackout() send(T_BLACK,  Controls.Blackout.Boolean and 0xFF or 0x00) end
  local function sendFreeze()   send(T_FREEZE, Controls.Freeze.Boolean   and 0xFF or 0x00) end

  -- ---- Socket-Events ----
  sock.Connected = function()
    Controls.Online.Boolean = true
    Controls.Status.String  = "Verbunden"
  end
  sock.Reconnect = function() Controls.Status.String = "Reconnect..." end
  sock.Closed    = function() Controls.Online.Boolean = false; Controls.Status.String = "Getrennt" end
  sock.Error     = function(_, err) Controls.Online.Boolean = false; Controls.Status.String = "Fehler: " .. tostring(err) end
  sock.Timeout   = function() Controls.Status.String = "Timeout" end
  sock.Data = function()
    local data = sock:Read(sock.BufferLength)
    if data and #data > 0 then
      logAdd("RX <", toHex(data))
      -- Antwort-Frames beginnen mit AA 55 -> Geraet bestaetigt/ist erreichbar
      if data:byte(1) == 0xAA and data:byte(2) == 0x55 then
        Controls.Online.Boolean = true
        Controls.Status.String  = "OK"
      end
    end
  end

  local function connect()
    local ip, port = curIP(), curPort()
    Controls.Status.String = "Verbinde " .. ip .. ":" .. port
    sock:Connect(ip, port)
  end

  -- ---- Control-Handler ----
  Controls.Reconnect.EventHandler = function() sock:Disconnect(); connect() end
  Controls.IPAddress.EventHandler = function() sock:Disconnect(); connect() end
  Controls.Port.EventHandler      = function() sock:Disconnect(); connect() end
  Controls.Brightness.EventHandler = function() brTimer:Start(0.05) end
  Controls.Blackout.EventHandler   = function() sendBlackout() end
  Controls.Freeze.EventHandler     = function() sendFreeze() end
  Controls.ClearLog.EventHandler   = function() logLines = {}; Controls.DebugLog.String = "" end

  -- ---- Init ----
  if Controls.IPAddress.String == "" then Controls.IPAddress.String = tostring(Properties["IP Address"].Value) end
  if Controls.Port.String      == "" then Controls.Port.String      = tostring(Properties["Port"].Value) end
  Controls.BrightnessFb.String = clamp(math.floor((tonumber(Controls.Brightness.Value) or 0) + 0.5), 0, 100) .. " %"
  Controls.Online.Boolean = false
  connect()
end
