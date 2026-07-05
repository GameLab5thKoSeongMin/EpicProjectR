# M8.11-A Main UI Image Usage Map

| Role | Main path | Source GUID | Source asset | Type | Preserve aspect | Current status |
| --- | --- | --- | --- | --- | ---: | --- |
| Background | `BackgroundImage` | `a03ee1cf9f1f8ad4e9606497cc8ee971` | `ref_folder/Assets/Images/Main_Total.png` | Simple | 0 | Already used. |
| Initial paper prop | `BackgroundImage/Paper` | `de45691cea43e2744b122201560fa950` | `ref_folder/Assets/Images/Old/paper.png` | Simple | 0 | Used as `paper_prop.png`. |
| Letter prop | `BackgroundImage/letter` | `a528993b306d8c744a3435f534d9831d` | `ref_folder/Assets/Images/Old/편지UI.png` | Simple | 0 | Used as `letter_ui.png`. |
| Board prop | `BackgroundImage/board` | `07bc754a6d0db8548aebe7de1904ae2d` | `ref_folder/Assets/Images/Old/ChatGPT Image 2026년 6월 16일 오전 02_41_47.png` | Simple | 0 | Skipped; not needed for current loop. |
| Shelf line art | `Shelf` | `2890e5d66fb62e641bd42f8fec4ef54a` | `ref_folder/Assets/Images/UI_Right_Middle_Line (1).aseprite` | Simple | 0 | Skipped; `.aseprite`. |
| AR panel paper | `EssentialPanel` | `a8985a8129f747c4599259ed8c6b5287` | `ref_folder/Assets/Images/UI_Right_Middle_Paper.aseprite` | Sliced | 0 | Skipped; `.aseprite`. |
| CR panel paper | `ConsideratePanel` | `813f9c3e0e2d95942ba62bc39f9272d8` | `ref_folder/Assets/Images/UI_Right_Middle_Paper_revert.aseprite` | Sliced | 0 | Skipped; `.aseprite`. |
| Section title plate | `EssentialPanel/Image/Image` | `97aefd1b729d6714d902557d148edda3` | `ref_folder/Assets/Images/Title.aseprite` | Simple | 0 | Skipped; `.aseprite`. |
| Workbench paper/table | `WorkBenchDisplay/GameObject` | `a4bbb5925e57df744a9be3c86f0c0854` | `ref_folder/Assets/Images/Table_Blue_2 (1).aseprite` | Simple | 0 | Skipped; `.aseprite`. |
| Decision background | decision `Image` | `e841f4797d679f24897de78dee37de2e` | `ref_folder/Assets/Images/Paper texture 2.png` | Simple | 0 | Already used. |
| Reject tab | `Reject` | `a48a6213bf51c5849a37da92a610ab26` | `ref_folder/.../UI_tab_Off.png` | Simple | 0 | Already used. |
| Accept tab | `Accept` | `b08924c552545c9449e191ce07988258` | `ref_folder/.../UI_tab_dim.png` | Simple | 0 | Already used. |
| Small icon button | `SkillPanel/Button` | `b199c7e3e923c864aaad2fe80448bc18` | `ref_folder/.../UI_BtnIcon_x.png` | Simple | 0 | Already used as fallback. |
| Document paper | document prefabs | `b2cfabbe404b0154cbb4da0dbb86b31b` | `ref_folder/Assets/Images/Paper_3.aseprite` | Simple | 0 | Skipped; `.aseprite`; generated paper texture used. |

## Text And Font Usage

Main uses TextMeshProUGUI heavily, with Kyobo/Galmuri SDF font assets. Current runtime UI remains uGUI `Text`, but loads Kyobo/Galmuri font files from safe Resources paths. TMP SDF `.asset` migration remains intentionally skipped to avoid package/settings drift.
