# Terms And Glossary

Last inspected: 2026-07-05.

| Term | Korean / source term | Meaning | Implementation note | Status |
| --- | --- | --- | --- | --- |
| Rissebra | 리세브라 | Fictional port city where the player works. | Use as setting anchor and route origin. | Current |
| Underwriter | 언더라이터 | Player role. Reviews contracts and decides risk. | Prefer in UI and code comments over generic judge/manager. | Current |
| Contractor / Applicant | 계약자 / 신청인 | Person requesting insurance. | Use `Contractor` or `Applicant`, not adventurer. | Current |
| Contract case | 계약 | One insurance request reviewed by the player. | Stable unit for `ContractID`. | Current |
| Ship insurance | 선박 보험 | Contract type focused on ship documents and route risk. | First implementation scope. | Current |
| Cargo insurance | 화물 보험 | Contract type focused on cargo documents and value risk. | Starts in full game turn 7. | Later |
| Mixed contract | 복합 / 혼합 계약 | Ship and cargo documents combined. | Bundle 7, full game turn 12 onward. | Later |
| Document bundle | 서류 묶음 | Group of documents available for a contract at a given complexity level. | `BundleID` 1-7 plus temporary certificates. | Current |
| Absolute Rejection | 인수 거절 / 절대 거절 | Rule condition that requires rejection. | Use `AR##` IDs. | Current |
| Rejection Consideration | 거절 고려 | Risky but potentially underwritable condition. | Use `CR##` IDs and premium multipliers. | Current |
| AccidentFlag | 사고 잠재 | Hidden deterministic accident marker. | Approval plus flag causes accident; no probability roll. | Current |
| News-driven rule | 뉴스 기반 기준 | Rule activated or modified by turn news. | Needs rule windows and active-news state. | Current |
| Subjective information | 주관 정보 | Contract-bound human/contextual information. | Affects outcome/narrative, not objective score. | Current |
| Special contractor | 특수 계약자 | Recurring named contractor with a dilemma arc. | IDs `SC01` to `SC15`. | Current |
| After-story | 애프터 스토리 | Later consequence event from a prior decision. | Board, letter, gift, inspection channels. | Current |
| RouteID | 항로 ID | Stable route/destination key. | `R0005` is Baleca for quarantine logic. | Current |
| CorrectAction | 정답 행동 | Objective expected decision for scoring. | Should be testable separately from subjective outcomes. | Current |
| Premium | 보험료 | Upfront payment/price for the contract. | CR rules can change recommended multiplier. | Current |
| Coverage | 보장액 | Insured payout exposure. | Exact economy impact is data-driven. | Current |
| Ducat | 두캇 | Currency. | Used in economy/settlement. | Current |
| Contract management map | 계약 관리함 | Map-like view of approved contracts and accident state. | Later UI milestone. | Current |
| Insurance claim sheet | 보험금 청구서 | Accident claim/result document. | Page exists but detail content is unresolved. | Unresolved |
| Adventurer | 모험가 | Older reference-project role term. | Do not use for current production terminology. | Obsolete |
| Monster / location countermeasure | 몬스터 / 장소 대책 | Older fantasy underwriting vocabulary. | Do not use for current maritime insurance implementation. | Obsolete |

## Naming Guidance

Prefer code and file names that describe the current maritime underwriting domain:

- `Contract`, `ContractCase`, `UnderwritingRule`, `DocumentBundle`, `Route`, `Applicant`, `SpecialContractor`.

Avoid carrying over reference-project names unless they describe reusable technical infrastructure without domain meaning.

