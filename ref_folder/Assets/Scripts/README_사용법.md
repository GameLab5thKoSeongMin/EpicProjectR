# UnderwritingSystem_CountermeasureBased

이 버전은 `몬스터 SO`와 `장소 SO`가 각각 자기 대응책 체크항목을 직접 가지고 있는 구조입니다.

기본 원칙:

```text
몬스터 SO
- 대응책 항목들 보유
- 예: 서리거인 불속성 대응 +10, 원거리 대응 +10

장소 SO
- 대응책 항목들 보유
- 이 장소에 서식 가능한 몬스터 SO 목록 보유
- 예: 북부 설산 혹한 대응 +10, 서식 가능 몬스터: 서리거인

장비 / 스킬 / 물품 SO
- 태그만 보유
- 예: 화염검 = Counter.Fire, Element.Fire, Attack.Melee
```

---

## 1. 폴더 배치

Unity 프로젝트에 아래처럼 넣는 것을 권장합니다.

```text
Assets/Scripts/UnderwritingSystem/
```

이 패키지는 namespace를 사용하지 않습니다.

---

## 2. 주요 코드 구조

```text
DefinitionBase.cs
RiskTagDefinition.cs
CoreDefinitions.cs
TaggedContentDefinition.cs
RiskRecords.cs
CountermeasureEntry.cs
UnderwritingDocuments.cs
UnderwritingManual.cs
UnderwritingEvaluation.cs
ManualCheckAudit.cs
ManualBookDatabase.cs
ContentCatalog.cs
AdventurerGenerationProfile.cs
CaseGenerationProfile.cs
CaseGoalProfile.cs
PlannedCaseGenerator.cs
UnderwritingTestRunner.cs
```

---

## 3. 태그 만들기

Create 메뉴:

```text
Create > Underwriting > Definitions > Risk Tag
```

추천 태그 예시:

```text
Counter.Fire
Counter.Poison
Element.Fire
Attack.Ranged
Attack.Melee
Attack.Magic
Supply.Water
Environment.ColdResist
Camping.Gear
Survival.Skill
Role.Healer
```

---

## 4. 콘텐츠 만들기

Create 메뉴:

```text
Create > Underwriting > Content > Equipment
Create > Underwriting > Content > Skill
Create > Underwriting > Content > Supply
```

예시:

```text
화염검
- Counter.Fire
- Element.Fire
- Attack.Melee

화염마법
- Counter.Fire
- Element.Fire
- Attack.Ranged
- Attack.Magic

장궁
- Attack.Ranged

해독제
- Counter.Poison
```

생성기가 사용할 콘텐츠는 `ContentCatalog`에 등록해야 합니다.

```text
Create > Underwriting > Generation > Content Catalog
```

---

## 5. 몬스터 만들기

Create 메뉴:

```text
Create > Underwriting > Risk > Monster
```

예시: 서리거인

```text
Countermeasures:

1)
Id: monster_frost_giant_fire
Title: 서리거인 불속성 대응
ManualText: 서리거인을 상대할 때 불속성 장비, 스킬, 물품이 있으면 +10점
Score: 10
MatchMode: PartyHasAnyTag
RequiredTags: Counter.Fire

2)
Id: monster_frost_giant_ranged
Title: 서리거인 원거리 대응
ManualText: 서리거인은 접근전 위험도가 높으므로 원거리 공격 수단이 있으면 +10점
Score: 10
MatchMode: PartyHasAnyTag
RequiredTags: Attack.Ranged
```

---

## 6. 장소 만들기

Create 메뉴:

```text
Create > Underwriting > Risk > Location
```

예시: 북부 설산

```text
TerrainName: 설산
ClimateName: 혹한
DistanceName: 장거리

Countermeasures:
- 혹한 대응 +10
- 장거리 야영 준비 +10

PossibleMonsters:
- 서리거인
- 설산 와이번
```

퀘스트 생성기는 `LocationRiskDefinition.PossibleMonsters`에서 몬스터를 고릅니다.

---

## 7. 메뉴얼북 만들기

Create 메뉴:

```text
Create > Underwriting > Manual > Manual Book Database
```

여기에 메뉴얼에 표시할 장소와 몬스터 SO를 등록합니다.

메뉴얼북은 고정 문서처럼 사용하지만, 텍스트는 SO에서 자동 생성할 수 있습니다.

```csharp
string text = ManualBookBuilder.BuildText(manualBookDatabase);
```

---

## 8. 심사 매뉴얼 만들기

Create 메뉴:

```text
Create > Underwriting > Manual > Scoring Manual
```

여기에는 다음 설정이 있습니다.

```text
ExperienceScoreAggregation
ConditionalMargin
MaxFinalScore
InvestigationOnDocumentWarning
CommonCountermeasures
```

`CommonCountermeasures`는 항상 활성화되는 체크항목입니다.
예: 힐러 확보, 3인 이상 파티 등.

---

## 9. 랜덤 생성 프로필 만들기

Create 메뉴:

```text
Create > Underwriting > Generation > Adventurer Profile
Create > Underwriting > Generation > Case Profile
Create > Underwriting > Generation > Case Goal Profile
```

`Case Profile`은 퀘스트와 파티 생성 풀입니다.

```text
QuestTypePool
QuestGradePool
LocationPool
AdventurerProfile
MinPartySize
MaxPartySize
```

`Case Goal Profile`은 목표 판정 확률입니다.

```text
Approved
ConditionalApproved
Rejected
InvestigationRequired
```

---

## 10. 테스트 실행

빈 GameObject에 `UnderwritingTestRunner`를 붙입니다.

필수 참조:

```text
CaseGenerationProfile
CaseGoalProfile
UnderwritingManual
ContentCatalog
ManualBookDatabase
```

Context Menu:

```text
Generate Planned Case
Evaluate Current Case
Audit Test Submission
Print Manual Book Text
```

---

## 11. 플레이어 체크 검증 방식

플레이어가 메뉴얼북에서 체크한 항목은 `CountermeasureEntry.Id`로 저장해야 합니다.

```csharp
PlayerManualSubmission submission = new PlayerManualSubmission();
submission.checkedCountermeasureIds.Add("monster_frost_giant_fire");
submission.writtenScore = 30;
submission.selectedDecision = UnderwritingDecision.Approved;
```

감사 로직:

```csharp
ManualCheckAuditResult audit = ManualCheckAuditor.Audit(
    evaluation,
    submission,
    manualBookDatabase);
```

이 결과로 다음을 구분할 수 있습니다.

```text
올바르게 체크한 점수
잘못 체크한 점수
놓친 점수
```

즉, 플레이어가 총점은 맞췄지만 엉뚱한 항목을 체크한 경우도 잡아낼 수 있습니다.

---

## 12. 핵심 흐름

```text
1. 장소 SO 랜덤 선택
2. 장소 SO의 PossibleMonsters에서 몬스터 선택
3. 장소 대응책 + 몬스터 대응책 + 공통 대응책이 이번 퀘스트의 활성 체크항목이 됨
4. 목표 점수에 맞게 대응책 일부를 충족하도록 모험가 파티 생성
5. 내부 평가는 활성 체크항목별로 점수 계산
6. 플레이어 체크는 CountermeasureEntry.Id 목록으로 비교
```

---

## 13. 콘텐츠 추가 흐름

새 몬스터 추가:

```text
MonsterRiskDefinition 생성
대응책 항목 작성
LocationRiskDefinition.PossibleMonsters에 등록
ManualBookDatabase.Monsters에 등록
```

새 장소 추가:

```text
LocationRiskDefinition 생성
장소 대응책 항목 작성
PossibleMonsters 등록
ManualBookDatabase.Locations에 등록
CaseGenerationProfile.LocationPool에 등록
```

새 장비/스킬/물품 추가:

```text
Equipment/Skill/Supply Definition 생성
태그 부여
ContentCatalog에 등록
```

코드 수정 없이 데이터 추가로 확장하는 것을 목표로 설계했습니다.
