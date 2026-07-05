# Game Overview

Last inspected: 2026-07-05.

## Working Title

Current working title from Notion: `리세브라의 언더라이터` / `Rissebra's Underwriter`.

## Genre

Maritime insurance underwriting simulation with document review, rule comparison, economic pressure, and narrative consequence systems.

## Setting

The game is set in the fictional port city of Rissebra during an Age-of-Sail-inspired period. All ports and personal names should be treated as fictional, even when they evoke real historical maritime naming styles.

## Player Role

The player is an underwriter. The player reviews ship, cargo, and mixed insurance applications, judges whether the documents meet the active rules, and accepts responsibility for later outcomes.

## Core Player Fantasy

The fantasy is not simply being a clerk who finds typos. The fantasy is being the person who decides whether risk is acceptable when documents, rules, rumors, compassion, pressure, and money all pull in different directions.

The player fantasy should be expressed through:

- Careful document comparison.
- Interpreting active underwriting criteria.
- Deciding whether to approve, conditionally approve, or reject when supported.
- Seeing approved contracts travel and later return, fail, or generate consequences.
- Facing letters, public notices, claims, inspections, gifts, and settlement reports that make prior decisions feel real.

## Core Emotional Experience

Confirmed by the main Notion plan:

- Objective judgment versus subjective judgment.
- Compassion, responsibility, despair, and recovery.
- Moments where the player was objectively correct but still loses money or faces painful narrative consequences.
- Special contractors each ask a different question about what it means to be an underwriter.

## Main Conflict / Dilemma

The central dilemma is the gap between "the correct rule answer" and "the human consequence."

Examples confirmed in design docs:

- Subjective information can be true or false and should not become a reliable formula.
- Some correct rejections still feel cruel.
- Some approvals against recommendation may end safely, while some correct approvals still accident.
- News changes the objective criteria, so old safe assumptions can become invalid.

## What the Game Is Not

- It is not a fantasy adventurer/monster countermeasure game under the current design direction.
- It is not a random contract generator as the main game source.
- It is not a probability-based accident simulation for approved flagged contracts.
- It is not a game where subjective information directly changes objective score.
- It is not a project where `ref_folder/` assets are production assets.
- It is not a Singleton-heavy Unity prototype architecture by default.

## Current Vocabulary for Code and Docs

Use these terms unless the human team says otherwise:

| Preferred term | Korean / design term | Use for | Status |
| --- | --- | --- | --- |
| Rissebra | 리세브라 | Player city | Current |
| Underwriter | 언더라이터 | Player role | Current |
| Contractor / Applicant | 계약자 / 청약자 | Person or organization applying | Current |
| Contract Case | 계약 / 심사 건 | One reviewable application slot | Current |
| Ship Insurance | 선박 보험 | Ship-focused insurance | Current |
| Cargo Insurance | 적하 보험 | Cargo-focused insurance | Current |
| Mixed Contract | 혼합 계약 | Ship + cargo review | Current |
| Document Bundle | 서류 조합 / 서류 묶음 | Required document set | Current |
| Absolute Rejection | 절대 거절 | Must-reject rule | Current |
| Rejection Consideration | 거절 고려 | Risk/price consideration rule | Current |
| AccidentFlag / Accident Potential | 사고 잠재 | Hidden deterministic accident marker | Current; hidden from player |
| News-driven Rule | 뉴스 기반 규칙 | Rule activated/changed by news | Current |
| Special Contractor | 특수 계약자 | Recurring narrative contractor | Current |
| After-story | 애프터 스토리 / 후일담 | Delayed narrative feedback | Current |
| Adventurer | 모험가 | Older/refactor prompt wording | Obsolete unless reconfirmed |
| Monster/Location Countermeasure | 몬스터/장소 대응책 | Old README concept | Obsolete |

## Implementation Note

Future implementation should use current maritime underwriting terminology in namespaces, classes, and docs. If a class needs a generic name, prefer `ContractCase`, `Contractor`, `UnderwritingRule`, `DocumentBundle`, and `Outcome` over older `Adventurer` or `Quest` terms.
