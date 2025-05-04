
# Projektorganisation – TourPlanner

## 🎯 Projektziel

Entwicklung einer plattformübergreifenden Desktop-App mit .NET **MAUI** zur Verwaltung von Touren und TourLogs – inklusive Kartenanzeige, Wetterintegration, Full-Text-Search und PDF-Reports.

  
---
  

## 🧩 Architektur & Technik

- **Architekturmuster:** MVVM (Model-View-ViewModel)

- **UI-Framework:** .NET MAUI (wegen macOS-Kompatibilität)

- **Schichten:** UI · BL (Business Logic) · DAL (ORM & DB) · Logging

- **Datenbank:** PostgreSQL via ORM (Entity Framework)

- **API-Integration:** OpenRouteService (Routing + Bild), Open-Meteo o. Ä. (Wetter)

- **Testing:** Eigene Test-Assembly mit UnitTests

- **Build-Tools:** Git, evtl. MAUI CLI, Visual Studio

  
---


## 📂 Projektstruktur

- `TourPlanner.UI`: MAUI/XAML Views & ViewModels

- `TourPlanner.BL`: Geschäftslogik, Services

- `TourPlanner.DAL`: ORM, Repositories, Entities

- `TourPlanner.Logging`: Logging-Komponenten

- `TourPlanner.Tests`: Testprojekt für UnitTests

  

---

  

## 👥 Teamaufteilung & Rollen


### 🧑‍💻 David

- UI-Gesamtverantwortung (MAUI, Views, Layout)

- Tour-Detail-Ansicht und TourLogs

- Full-Text-Search + dynamisches UI

- Wetterintegration (Unique Feature)

- Dokumentation (UML, Protokoll)

- Teile des ORM-Mappings (z. B. TourLogEntity)

  

### 👨‍💻 Alex

- Tour-Liste (UI-Teil)

- CRUD-Logik mit BL und DAL

- ORM-Grundstruktur + `DbContext`

- OpenRouteService-API für Routing, Distanz, Kartenbild

- UnitTests, Logging & Feinschliff

  

---

## 📆 Zeitplan (Wochenweise)


| Woche | Aufgabenbereich                                       |

|-------|--------------------------------------------------------|

| 0     | Modelle, DB-Schema, Wireframes, MVVM-Recherche         |

| 1     | BL (David), DAL (Alex), Grundstruktur & Tests          |

| 2     | UI (David + Alex), CRUD-Services, responsives Layout   |

| 3     | Full-Text-Search (David), Routing-API (Alex), Reports  |

| 4     | Wetter, Dokumentation (David), Tests & Logging (Alex)  |

  
---

## 💡 Besonderheiten

- UI wird **responsive**: Bei kleinen Screens wird eine **Tab-Navigation** statt dem Split-Layout verwendet.

- Tourliste kann aufgeklappt/zugeklappt werden.

- **„View“-Menüpunkt** erlaubt das Ausblenden einzelner UI-Komponenten.

- Komponenten kommunizieren lose über Services/Interfaces.

- Testdaten und MockViewModels ermöglichen frühe UI-Entwicklung ohne fertige BL.