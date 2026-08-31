# continental_geb20_7003

Q-Sys Mediensteuerung Aumovio, Gebäude 20, Raum 7003.

## Systeme
- Q-Sys

## KNX / EIB-Anbindung

Die KNX-Anlage wird über ein **b+b EIB-Medientechnik-Gateway IP**
(ASCII-Protokoll über TCP) angebunden. Gateway läuft als TCP-Server,
Q-Sys verbindet als TCP-Client.

- Gateway: `192.168.100.104:10001` (TCP-Server)
- Plugin: [`plugins/EIB_Medientechnik_Gateway.lua`](plugins/EIB_Medientechnik_Gateway.lua)

### Plugin installieren
1. `plugins/EIB_Medientechnik_Gateway.lua` nach
   `Dokumente/QSC/Q-SYS Designer/Plugins/` kopieren.
2. In Q-Sys Designer erscheint das Plugin unter **Plugins → Developer →
   EIB Medientechnik Gateway (KNX)**. Ins Design ziehen.
3. Optional zu `.qplug` paketieren: Plugin im Designer öffnen und
   **Save As Plugin** wählen.

### Plugin-Seiten
- **Connection** – IP-Adresse und Port zur **Laufzeit** editierbar (Vorbelegung
  aus den Properties; Änderung verbindet neu), Online-LED, Status,
  Firmware-Version, Buttons *Reconnect / Provision / Refresh All*.
- **Setup** – Tabelle (`Address Count` Zeilen): pro Zeile Gruppenadresse,
  Datentyp (Dropdown) und frei vergebbarer Datenpunkt-**Name**. Button
  *Defaults laden* füllt die 10 Beispieladressen des Projekts.
- **Lights/Shades** – Prefab-Tabellen (`Light Count` / `Shade Count` Properties),
  eine Zeile je Gerät. Datenpunkte werden per Dropdown aus den Setup-Namen zugewiesen:
  - Light: DP-Spalten *On/Off*, *On/Off-Fb*, *Value*, *Dim* → Controls An/Aus,
    +/− (Heller/Dunkler), Wert-Fader, Fb-LED + Fb-Wert-Meter.
  - Shade: **ein** Datenpunkt (i.d.R. 4-bit) → Auf/Stop/Ab.
- **Debug** – rollierendes TX/RX-Log (mit Zeitstempel) plus *Clear*.

Design: Catppuccin-Mocha-Palette (feste Farben, theme-unabhängig).

### Verhalten
- Beim Verbinden normalisiert das Plugin die Gateway-Optionen
  (`OV+D+E-G+H-N-Q+R+S-X-`) und **provisioniert automatisch** jede definierte
  Gruppenadresse (`S<ga>:<format>,E` + `R<ga>`) – kein separates b+b-Terminal
  nötig.
- Datentyp → Gateway-Format: `1.*`/`3.*` → 1, `5.001` → 3, `5.004/5.010` → 5,
  `6/7/8/9/12/14/16.*` entsprechend.

> **Hinweis** zu 4-bit-Aktionen (Vorhänge/Sonnenschutz, Heller/Dunkler): Die
> Sende-Werte (`DIM_UP/DOWN`, `SHD_UP/DOWN/STOP`) stehen als Konstanten oben im
> Runtime-Teil des Plugins und sind je nach Aktor-DPT (3.007/3.008) anzupassen.

## Novastar LED-Controller

Die LED-Wand wird über einen **Novastar COEX-Controller** (MX/CX/KU-Serie, z.B.
MX40 Pro, MX30, MX20, KU20, CX40 Pro) gesteuert. Protokoll: *COEX Central
Control Protocol* – binäre Frames über **TCP Port 5200** (Controller = Server,
Q-Sys = Client).

- Plugin: [`plugins/Novastar_COEX_LED.lua`](plugins/Novastar_COEX_LED.lua)
- Funktionen: **Helligkeit** (0–100 %), **Blackout** (an=schwarz/aus=normal),
  **Freeze** (Standbild an/aus).

### Plugin installieren
1. `plugins/Novastar_COEX_LED.lua` nach
   `Dokumente/QSC/Q-SYS Designer/Plugins/` kopieren.
2. In Q-Sys Designer unter **Plugins → Developer → NovaStar COEX LED Controller**
   ins Design ziehen.

### Plugin-Seiten
- **Connection** – IP-Adresse und Port (Default 5200) zur **Laufzeit** editierbar,
  Online-LED, Status, Button *Reconnect*.
- **Control** – Helligkeits-Fader, Blackout-Toggle, Freeze-Toggle.
- **Debug** – rollierendes TX/RX-Log (Hex) plus *Clear*.

### Protokoll (Kurzreferenz)
Frame `[55 AA][Content][CS_L CS_H]`; Checksumme = Summe aller Bytes ab Index 2
(inkl. Datenbyte) `+ 0x5555`, little-endian.

| Funktion  | Frame (verifiziert)                                                 |
|-----------|--------------------------------------------------------------------|
| Helligkeit| `55 AA 00 00 FE FF 01 FF FF FF 01 00 01 00 00 02 01 00 XX <CS>` (XX=0…FF) |
| Blackout  | `… 01 00 00 01 00 02 01 00 FF` (an) / `… 00` (aus)                  |
| Freeze    | `… 01 00 02 01 00 02 01 00 FF` (an) / `… 00` (aus)                  |

Quelle: *NovaStar COEX Central Control Protocol Instructions V1.5.0*.
Presets werden aktuell nicht unterstützt (bei Bedarf ergänzbar: Reg `0x0a000002`).
