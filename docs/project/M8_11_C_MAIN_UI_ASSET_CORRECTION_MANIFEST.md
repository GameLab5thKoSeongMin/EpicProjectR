# M8.11-C Main UI Asset Correction Manifest

Assets were copied only from Main scene dependencies or already documented direct Main dependencies.

| Role | Original asset | Copied asset path | Type | Status | Runtime load path | Current UI uses it |
| --- | --- | --- | --- | --- | --- | --- |
| Background | `ref_folder/Assets/Images/Main_Total.png` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/MainSceneUI/Background/main_total.png` and existing Resources copy | PNG | exact | `FirstPlayable/.../Background/main_total` | Yes |
| Decision paper | `ref_folder/Assets/Images/Paper texture 2.png` | `.../Documents/paper_texture_2.png` | PNG | exact | `FirstPlayable/.../Documents/paper_texture_2` | Yes |
| Paper prop | `ref_folder/Assets/Images/Old/paper.png` | `.../Documents/paper_prop.png` | PNG | exact | `FirstPlayable/.../Documents/paper_prop` | Yes |
| Letter prop | `ref_folder/Assets/Images/Old/편지UI.png` | `.../Documents/letter_ui.png` | PNG | exact | `FirstPlayable/.../Documents/letter_ui` | Yes |
| Approve tab | `ref_folder/.../UI_tab_dim.png` | `.../UI/ui_tab_dim.png` | PNG | exact | `FirstPlayable/.../UI/ui_tab_dim` | Yes |
| Reject tab | `ref_folder/.../UI_tab_Off.png` | `.../UI/ui_tab_off.png` | PNG | exact | `FirstPlayable/.../UI/ui_tab_off` | Yes |
| Small button | `ref_folder/.../UI_BtnIcon_x.png` | `.../UI/ui_btn_icon_x.png` | PNG | exact | `FirstPlayable/.../UI/ui_btn_icon_x` | Yes |
| Contractor visual | `ref_folder/Assets/Images/임시_캐릭터 마법사.png` | `.../Characters/contractor_temp.png` | PNG | exact | `FirstPlayable/.../Characters/contractor_temp` | Yes, if import succeeds |
| Dialogue bubble | `ref_folder/Assets/Images/Old/말풍선.png` | `.../UI/speech_bubble.png` | PNG | exact | `FirstPlayable/.../UI/speech_bubble` | Yes, if import succeeds |
| Bell/decision prop | `ref_folder/Assets/Images/Old/Bell_Full.png` | `.../UI/bell_full.png` | PNG | exact | `FirstPlayable/.../UI/bell_full` | Yes, if import succeeds |

## Skipped Assets

| Role | Asset | Reason |
| --- | --- | --- |
| AR sliced panel | `UI_Right_Middle_Paper.aseprite` | `.aseprite`; no safe export pipeline in this pass. |
| CR sliced panel | `UI_Right_Middle_Paper_revert.aseprite` | `.aseprite`; no safe export pipeline in this pass. |
| Shelf line art | `UI_Right_Middle_Line (1).aseprite` | `.aseprite`; no safe export pipeline in this pass. |
| Workbench/table blue | `Table_Blue_2 (1).aseprite`, `Table_Blue (1).aseprite` | `.aseprite`; skipped to avoid importer/settings coupling. |
| Main document prefab paper | `Paper_3.aseprite` | `.aseprite`; generated document cards use safe paper texture. |
| TMP SDF assets | TextMeshPro `.asset` files | Skipped to avoid TMP/material/package/settings changes. |

No old C# scripts, prefabs, scenes, ScriptableObjects, packages, or project settings were copied.
