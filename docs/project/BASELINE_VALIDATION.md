# Baseline Validation

Last updated: 2026-07-05.

## What Can Be Validated Now

| Check | Current method |
| --- | --- |
| Active Unity project path exists | `Test-Path EpicProjectR` |
| Reference folder exists | `Test-Path ref_folder` |
| Design source-of-truth exists | `Test-Path docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md` |
| Repository boundary docs exist | `Test-Path docs/project/REPOSITORY_BOUNDARIES.md` |
| Root `AGENTS.md` exists | `Test-Path AGENTS.md` |
| Unity project `AGENTS.md` exists | `Test-Path EpicProjectR/AGENTS.md` |
| Required M0 docs exist | `Test-Path` for each required output |
| No intentional production C# edits | File scope review |
| No intentional scene/prefab/asset/package/project setting edits | File scope review |
| No intentional `ref_folder/` edits | File scope review |

## What Cannot Be Fully Validated Yet

| Area | Reason |
| --- | --- |
| Unity edit-mode tests | M0 does not create test infrastructure or domain code. |
| Gameplay behavior | M0 does not implement gameplay. |
| Source spreadsheet content | Linked Excel workbooks are not available locally. |
| Exact AR/CR rule data | Full rule table is in inaccessible source data. |
| Claim sheet UI/content | `보험금 청구서 상세` page was accessible but empty. |

## Recommended Manual Checks

- Open `AGENTS.md` and confirm it reflects current repository boundaries.
- Open `EpicProjectR/AGENTS.md` and confirm Unity-specific guardrails are concise.
- Open `README.md` and confirm new teammates can identify the active project and reference-only project.
- Open `docs/project/MILESTONE_CHECKLIST.md` before preparing M1.
- Confirm M1 prompt explicitly states allowed and forbidden paths.

## Recommended Git Checks

Run:

```powershell
git status --short
```

If this fails because of user-level git configuration permissions, use:

```powershell
$env:GIT_CONFIG_GLOBAL='NUL'; git status --short
```

Expected M0 result:

- Documentation and guardrail files may be new or updated.
- No production C# files should be changed.
- No `.unity`, `.prefab`, `.asset`, `.meta`, package, project setting, or `ref_folder/` files should be changed by M0.

## Recommended Unity Checks

M0 does not require opening Unity.

For future implementation milestones:

- Run Unity edit-mode tests once available.
- Run editor/content validators once available.
- Run a manual scene smoke test only after a UI/scene milestone explicitly allows scene work.

## Recommended Future Test Commands

No project-specific test command is confirmed yet.

Future tasks should document the exact command once Unity Test Framework or CI test execution is configured.

## Known Current Limitations

- Plain `git status --short` failed in this environment due to permission denial when reading `C:/Users/rhtjd/AppData/Roaming/SPB_Data/.gitconfig`.
- Fallback git status with `GIT_CONFIG_GLOBAL=NUL` worked.
- Existing repository status showed `docs/` and `ref_folder/` as untracked at the root-level git view. M0 did not intentionally modify `ref_folder/`.
- File timestamp inspection can be noisy because Unity and reference project files may already have recent timestamps.

## M0 Required Checks

| Required check | Expected result |
| --- | --- |
| `git status --short` | Attempted; document failure if blocked. |
| Verify `ref_folder/` remains unchanged | M0 must not edit it. |
| Verify only guardrail/docs files changed in M0 | Confirm by changed-file review. |
| Verify active Unity project path exists | `EpicProjectR/` exists. |
| Verify `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md` exists | Must exist. |
| Verify `docs/project/REPOSITORY_BOUNDARIES.md` exists | Must exist. |
| Verify root `AGENTS.md` exists | Must exist. |
| Verify `EpicProjectR/AGENTS.md` exists | Must exist. |
| Verify no production C# files were modified | Must be true. |
| Verify no `.unity`, `.prefab`, `.asset`, `.meta`, package, or project setting files were modified unless intentionally documented | Must be true for M0. |

