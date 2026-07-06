# M8.14 Verification And Gap Report

Status: Completed as a static verification and implementation plan pass on 2026-07-06.

## Clarification Verified

Confirmed: the latest request corrects the intended Main-like loop. The first screen must show a bright background, top HUD, bell, and small presented document with no visible character. Bell starts character arrival only. The presented document opens review only after arrival. In review, only the left presented document returns to Main; workstation documents remain review-only draggable papers.

Confirmed: `using_image/배경화면.png` exists at the repository root and is the intended mandatory background asset. The pasted prompt path was mojibake, but the filesystem contains the decoded filename.

Confirmed: `ref_folder/Assets/Scenes/Main.unity` was inspected only as visual/layout reference. No `ref_folder/` files were modified.

## Current Problems Rechecked

- Wrong/dark background: partially present before this pass because Presentation still loaded the existing Resources `main_total` first.
- Top-left title clipping: likely at 1920x1080 because the HUD title rect was 680 px and used a large fixed font.
- Date not centered: static rect was center-anchored, but the date width was narrow and needed stronger 1920 safety.
- Character entering from wrong direction: present before this pass; entry start was left-side/off-center, not center-origin.
- Review transition: present but still rect-tuning dependent; panel tweening existed.
- Chat UI behavior: accumulated bubbles and scroll existed, but scroll sensitivity and 1920 panel fit needed hardening.
- Right rejection panels: filtered reason rendering existed; row fit needed narrower shelf tuning.
- Decision paper toggle: open/close existed; close needed a downward hide guard.
- Top HUD covered: HUD is built last and remains above review UI; 1920 static recheck still required.
- 1920x1080 clipping: rects needed explicit proportions and best-fit text.

## Classes Requiring Correction

- `FirstPlayableAssetCatalog`: prefer the required local background image.
- `FirstPlayableMainSceneRectSpec`: tune entry positions, center-origin arrival, review proportions, shelf, and drawer sizes.
- `FirstPlayableView`: adjust HUD text fit, entry animation, chat scroll sensitivity, right-panel layout, rule row text fit, and drawer close behavior.

## Execution Plan

1. M8.15: background loader, HUD width safety, entry bell/document/character arrival rects.
2. M8.16: review layout rects, chat scroll tuning, left composition/workstation separation.
3. M8.17: right-panel row fit, paper decision toggle, preserve CR premium/incentive behavior.
4. M8.18: static 1920 checks, forbidden API/source-ID checks, final parity report.
