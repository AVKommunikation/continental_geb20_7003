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
- **Connection** – IP/Port (Properties), Online-LED, Status, Firmware-Version,
  Buttons *Reconnect / Provision / Refresh All*.
- **Setup** – Tabelle (`Address Count` Zeilen): pro Zeile Gruppenadresse,
  Datentyp (Dropdown) und frei vergebbarer Datenpunkt-**Name**. Button
  *Defaults laden* füllt die 10 Beispieladressen des Projekts.
- **Lights/Shades** – Prefab-Blöcke (`Light Count` / `Shade Count` Properties).
  Jeder Block wird über Dropdowns den Setup-Namen zugewiesen:
  - Light: On/Off-, Value-, Dim-, Feedback-Datenpunkt → Buttons An/Aus/Heller/
    Dunkler, Wert-Fader, Feedback-LED + Meter.
  - Shade: Up/Down/Stop-Datenpunkt → Buttons Auf/Ab/Stop.

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
