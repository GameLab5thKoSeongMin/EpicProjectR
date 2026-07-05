# Outcomes Economy And After-story

Last inspected: 2026-07-05.

## Outcome Model

The result system has three overlapping layers:

| Layer | What it answers | Confirmed behavior |
| --- | --- | --- |
| Contract outcome | Did the approved contract return safely or suffer an accident? | `AccidentFlag = 1` plus approval/conditional approval causes a deterministic accident. |
| Economy outcome | What happened to score, money, rank, and survival pressure? | Evaluation score and money are separate tracks. |
| Narrative consequence | How does a prior decision return to the player emotionally or socially? | After-story events can appear as board posts, letters, gifts, or inspections. |

Rejected contracts do not produce accidents. Special contractors can still return through rejection-route narrative consequences.

## Economy Tracks

| Track | Purpose | Confirmed rules |
| --- | --- | --- |
| Evaluation score | Promotion and professional assessment | Checklist +2, correct action +5, objective wrong -10, bribe inspection penalty can affect evaluation. |
| Money | Survival pressure and personal finances | Salary, commission, renewal bonus, no-accident streak bonus, accident penalties, fines, bankruptcy risk. |

Subjective information does not directly penalize evaluation score.

## Income And Penalty Rules

Confirmed high-level rules:

- Salary is slightly above living cost.
- Commission rate increases by rank: 10, 12, 15, and 18 percent.
- Accident responsibility uses commission refund plus rank-based fixed penalty.
- Rank-based fixed accident penalties are 15, 25, 35, and 50 ducats.
- Renewal bonus is +5 ducats.
- No-accident streak bonus is +10 ducats per turn, capped at +30.
- Bribe money is immediate income, but the later inspection fine is 80 ducats and evaluation penalty is -100.
- Bankruptcy occurs if balance remains negative for two consecutive turns.

Exact per-rank thresholds, salaries, costs, and final simulation rows should come from `05_Economy.xlsx`.

## Settlement Presentation

The design points toward a receipt-style settlement view. It should show:

- Starting balance.
- Salary.
- Commission.
- Renewal bonus.
- No-accident streak bonus.
- Accident commission refunds.
- Accident fixed penalties.
- Inspection fines.
- Ending balance.
- Evaluation score/rank progress.

Settlement should avoid hiding the cause of deductions. Each line item should be traceable to a contract, event, or rank rule.

## After-story Channels

| Channel | Purpose | Notes |
| --- | --- | --- |
| Lobby/port board | Public outcome and community feedback | Can include announcements, flyers, thanks, and public consequences. |
| Letter | Private emotional feedback | Especially useful for special contractor arcs. |
| Gift/object | Persistent office reminder | Represents a decision that remains visible. |
| Inspection | Formal systemic consequence | Used for punishment, fines, bribe discovery, or official review. |

After-story events are driven by a previous contract and trigger condition, then output on a later turn/channel.

## Event Grammar

Confirmed pattern:

```text
SourceContractID + TriggerCondition -> OccurrenceTurn + Channel + Output
```

Examples of trigger condition categories:

- Approved.
- Conditionally approved.
- Rejected.
- Approved and accident occurred.
- Bribe accepted.

Branches can be mutually exclusive. Do not schedule all branches of one special contractor event at once.

## Throttling

Only a small number of board/letter events should surface per turn, roughly two. Overflow events should be queued rather than lost.

Tests should cover deterministic queue order once the final scheduler exists.

## Unresolved Details

| Topic | Current status | Implementation guidance |
| --- | --- | --- |
| Insurance claim detail sheet | Linked Notion page was empty | Build claim UI only after content fields are available, or use a minimal accident summary in debug/MVP. |
| Exact economy rows | `05_Economy.xlsx` inaccessible | Keep economy parameters data-driven. |
| Exact after-story events | `08_AfterStories.xlsx` inaccessible | Implement scheduler shape before final content. |
| Conditional approval economy | Not fully specified | Reserve model hooks; do not require in M1. |

