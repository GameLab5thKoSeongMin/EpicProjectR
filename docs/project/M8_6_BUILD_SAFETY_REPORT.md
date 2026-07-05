# M8.6 Build Safety Report

Date: 2026-07-05

## Overall Result

Build-oriented safety improved. Runtime Presentation code no longer uses `UnityEditor` or `AssetDatabase`, selected reference assets are in a Resources-backed runtime-safe folder, and the pure C# fixture loop passed smoke validation.

## Asset Loading Strategy

- Runtime path root: `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/`
- Loader: `FirstPlayableAssetCatalog`
- Fonts: `Resources.Load<Font>()`, with OS dynamic font fallback if Unity import has not produced a usable font resource.
- PNGs: `Resources.Load<Texture2D>()`, then runtime `Sprite.Create()`.
- No Addressables, packages, ProjectSettings, scenes, prefabs, or ScriptableObject assets were added.

## Validation Performed

| Check | Result |
| --- | --- |
| `Get-Command Unity -ErrorAction SilentlyContinue` | No Unity CLI found on PATH. |
| `dotnet --version` | `10.0.301` available. |
| Pure C# smoke project under `%TEMP%/EpicProjectR_M8_6_Smoke` | Passed after approved `dotnet restore/run`. |
| Smoke behavior | Passed: C001 approve, C002 `AR01` reject, C003 `CR01`/`CR02` approve with 150% premium and deterministic accident, then sequence complete. |
| `rg -n "UnityEditor|AssetDatabase" EpicProjectR/Assets/Scripts` | No matches. |
| `rg -n "AccidentFlag|WillAccidentIfApproved" EpicProjectR/Assets/Scripts/Presentation` | No matches. |
| Runtime imported asset extensions | Only `.ttf` and `.png`. |
| Runtime imported forbidden extensions | No `.cs`, `.prefab`, `.unity`, `.asset`, `.mat`, `.anim`, `.controller`, or `.meta` files. |
| Project settings/packages/scenes path status | No active `Packages`, `ProjectSettings`, or `Assets/Scenes` changes reported. |
| Source ID exact strings | `C001`, `C002`, `C003`, `AR01`, `CR01`, `CR02`, and `R0005` remain exact in content/tests. |

## Validation Skipped

- Unity EditMode/PlayMode tests: skipped because Unity CLI is not available on PATH.
- Unity player build: skipped because no safe Unity build command is available in this environment.
- Manual visual QA: intentionally deferred to the next pass by M8.6 scope.

## Notes

- The first non-escalated `dotnet restore` failed because sandboxing could not read the user NuGet config. The approved escalated `dotnet restore/run` succeeded.
- The temporary smoke project emitted nullable warnings from the SDK compiler, but no errors; these warnings predate M8.6 and do not indicate fixture behavior failure.

## M8.7 Font Safety Addendum

Date: 2026-07-05

- Runtime font loading remains `Resources.Load<Font>()`.
- First playable title/body fonts still prefer `NotoSerifKR-Bold` and `NotoSerifKR-SemiBold`.
- Runtime fallback order now includes imported Korean-capable Resources fonts before OS fallback:
  - `Galmuri11-Bold`
  - `KyoboHandwriting2022khn`
  - `KyoboHandwriting2020pdy`
- TextMeshPro SDF assets, material presets, and TMP settings were not imported because this pass does not migrate uGUI `Text` to TMP and does not modify packages or project settings.
- No runtime script uses `UnityEditor` or `AssetDatabase`.
- Unity 6000.3.19f1 batchmode script compilation was attempted for M8.7 and reached `Tundra build success`; only existing `FindObjectOfType<T>()` obsolete warnings were reported.
