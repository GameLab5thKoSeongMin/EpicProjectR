// Responsibility: Provides Korean player-facing text for the first playable UI while preserving source IDs.
using System;
using EpicProjectR.Application;
using EpicProjectR.Content;
using EpicProjectR.Domain;

namespace EpicProjectR.Presentation
{
    public static class FirstPlayableKoreanText
    {
        public const string OfficeTitle = "리세브라 해상 보험 심사국";
        public const string ContractDocketTitle = "계약 목록";
        public const string CurrentCaseTitle = "현재 계약";
        public const string DocumentBundleTitle = "제출 서류";
        public const string AbsoluteRuleSectionTitle = "절대 거절 사유";
        public const string ConsiderationRuleSectionTitle = "거절 고려 사유";
        public const string ApproveButton = "승인";
        public const string ConditionalApproveButton = "조건부 승인";
        public const string RejectButton = "거절";
        public const string NextContractButton = "다음 계약";
        public const string FinishButton = "종료";
        public const string EntryDocumentTitle = "심사 서류 도착";
        public const string EntryPrompt = "종을 울리거나 서류를 눌러 다음 신청인을 맞이하세요.";
        public const string DialogueReviewIntro = "신청인이 도착했습니다. 서류를 살피고 AR/CR 사유를 표시하세요.";
        public const string DecisionBoxTitle = "인수 결정서";
        public const string DecisionBoxPrompt = "판단서를 올려 결정을 제출합니다.";
        public const string DecisionDrawerTitle = "인수 결정서";
        public const string DecisionDrawerPrompt = "왼쪽은 거절, 오른쪽은 승인입니다.";
        public const string NoSelectedReasons = "선택된 사유 없음";
        public const string DecisionCompensationLabel = "손해보상금";
        public const string DecisionPremiumLabel = "보험료";
        public const string ContractorThanksLine = "감사합니다.";
        public const string ContractorRejectLine = "다음 기회를 기다리겠습니다.";
        public const string ReviewWaitingAudit = "판단 평가: 결정을 기다리는 중입니다.";
        public const string ReviewWaitingOutcome = "계약 결과: 아직 제출된 결정이 없습니다.";
        public const string ReviewWaitingSettlement = "정산: 아직 정산 항목이 없습니다.";
        public const string CompletePremium = "첫 플레이어블 심사 루프 완료.";
        public const string CompleteResult = "완료된 순서: C001, C002, C003.";
        public const string CompleteOutcome = "C003 사고 결과는 종료 전 결과 화면에서 확인할 수 있습니다.";
        public const string CompleteSettlement = "fixture 순서를 다시 보려면 Play Mode를 다시 시작하세요.";

        public static string HeaderMeta(TurnNumber turn, GameDate date, ContractId contractId, string status)
        {
            return $"턴 {turn}  |  날짜 {date}  |  계약 {contractId}  |  상태 {status}";
        }

        public static string CompleteHeader()
        {
            return "턴 1  |  날짜 1599-01-15  |  fixture 세션 완료";
        }

        public static string HudDate(GameDate date)
        {
            return $"{date.Year:0000}년 {date.Month:00}월 {date.Day:00}일";
        }

        public static string HudLedger(int ducats, int reputation)
        {
            return $"두카트 {ducats}\n평판 {reputation}";
        }

        public static string ReviewingStatus()
        {
            return "심사 중";
        }

        public static string ResultPostedStatus()
        {
            return "결과 게시됨";
        }

        public static string DocketLine(ContractId id, string marker)
        {
            return $"{id}  {marker}";
        }

        public static string DocketMarkerSubmitted()
        {
            return "제출 완료";
        }

        public static string DocketMarkerCurrent()
        {
            return "현재";
        }

        public static string DocketMarkerPending()
        {
            return "대기";
        }

        public static string CaseSummary(ContractCase contractCase)
        {
            return
                $"계약 ID: {contractCase.Id}\n" +
                $"테스트 케이스: {(contractCase.IsFixture ? "예" : "아니오")}\n" +
                $"신청인: {contractCase.ApplicantName}\n" +
                $"보험 종류: {ContractTypeLabel(contractCase.ContractType)}\n" +
                $"서류 묶음: {contractCase.BundleId}\n" +
                $"항로: {contractCase.RouteId}\n" +
                $"기본 보험료: {contractCase.BasePremium.Ducats} 두카트\n" +
                $"보장 금액: {contractCase.Coverage.Ducats} 두카트\n" +
                $"귀환 예정일: {contractCase.ReturnDate}";
        }

        public static string CompleteCaseSummary()
        {
            return "남은 계약이 없습니다.\n첫 플레이어블 fixture 계약 3건이 모두 제출되었습니다.";
        }

        public static string ActiveRuleStatus(int activeRuleCount)
        {
            return string.Empty;
        }

        public static string MainLoopPremium(int selectedConsiderationCount)
        {
            var percent = FirstPlayableMainLoopState.PremiumPercentForSelectedConsiderations(selectedConsiderationCount);
            return $"{DecisionPremiumLabel}: {percent}%";
        }

        public static string DecisionCompensation(MoneyAmount coverage)
        {
            return $"{DecisionCompensationLabel}: {coverage.Ducats} 두카트";
        }

        public static string SelectedReasonSummary(int selectedAbsoluteCount, int selectedConsiderationCount)
        {
            if (selectedAbsoluteCount == 0 && selectedConsiderationCount == 0)
            {
                return NoSelectedReasons;
            }

            return $"선택 사유: 절대 거절 {selectedAbsoluteCount}개, 거절 고려 {selectedConsiderationCount}개";
        }

        public static string SessionCompleteStatus()
        {
            return "세션 완료.";
        }

        public static string PreSubmitPremium(int considerationCount)
        {
            if (considerationCount == 0)
            {
                return "보험료 제안: 승인 시 표준 100% 보험료를 적용합니다.";
            }

            var percent = considerationCount == 1 ? 125 : 150;
            var note = considerationCount >= 3 ? " 거절 권고." : string.Empty;
            return $"보험료 제안: CR 항목 기준 승인 시 {percent}% 보험료를 적용합니다.{note}";
        }

        public static string SubmittedPremium(int multiplierPercent, bool rejectRecommended, string considerationIds)
        {
            var recommendation = rejectRecommended ? "CR 누적으로 거절 권고." : "AR 차단 사유가 없으면 인수 진행 가능.";
            return $"보험료 제안: {multiplierPercent}%  |  고려 사유 {considerationIds}. {recommendation}";
        }

        public static string SubmittedPremium(int multiplierPercent, bool rejectRecommended, int considerationCount)
        {
            var recommendation = rejectRecommended ? "고려 사유 누적으로 거절 권고." : "절대 거절 사유가 없으면 인수 진행 가능.";
            return $"보험료 제안: {multiplierPercent}%  |  고려 사유 {considerationCount}개. {recommendation}";
        }

        public static string AuditResult(DecisionAuditResult audit, string correctIds, string missedIds, string extraIds)
        {
            return
                $"판단 평가: {(audit.IsCorrectAction ? "적정 판단" : "오판")}\n" +
                $"제출 결정 {DecisionLabel(audit.Decision)}  |  정답 결정 {DecisionLabel(audit.CorrectAction)}\n" +
                $"점수 변화 {audit.ScoreDelta.Value}\n" +
                $"맞게 표시한 사유 {correctIds}  |  놓친 사유 {missedIds}  |  잘못 표시한 사유 {extraIds}";
        }

        public static string OutcomeResult(SubmissionResult result)
        {
            var summary = result.ActiveContractCreated
                ? OutcomeSummary(result.Outcome)
                : "거절된 계약은 일반 사고를 만들지 않았습니다.";

            return
                $"계약 결과: {summary}\n" +
                $"활성 계약 생성: {(result.ActiveContractCreated ? "예" : "아니오")}";
        }

        public static string OutcomeSummary(OutcomeResult outcome)
        {
            if (outcome.AccidentOccurred)
            {
                return "귀환 시 사고가 발생했습니다. 첫 플레이어블 결정론 결과가 처리되었습니다.";
            }

            return "계약 선박이 무사히 귀환했습니다.";
        }

        public static string SettlementSummary(SettlementResult settlement, string renderedLineItems)
        {
            return $"정산: 자금 {settlement.TotalMoneyDelta}, 점수 {settlement.TotalScoreDelta}\n{renderedLineItems}";
        }

        public static string SettlementLineItem(SettlementLineItem item)
        {
            var contract = item.ContractId.HasValue ? item.ContractId.Value.ToString() : "-";
            return $"{SettlementLabel(item.Label)} ({contract}) 자금 {item.MoneyDelta.Ducats}, 점수 {item.ScoreDelta.Value}";
        }

        public static string DocumentTitle(DocumentRecord document)
        {
            var title = DocumentKindLabel(document.Kind);
            return document.Submitted ? title : $"{title} (미제출)";
        }

        public static string[] DialogueLines(ContractCase contractCase)
        {
            var applicant = contractCase != null ? contractCase.ApplicantName : "신청인";
            return new[]
            {
                $"{applicant} 님이 접수대 앞에 섰습니다.",
                "제출된 서류의 이름, 날짜, 항로를 차례대로 확인해 주세요.",
                "사유가 보이면 오른쪽 판에 표시하고, 인수 결정서를 열어 결정을 제출하세요."
            };
        }

        public static string MissingDocumentMessage()
        {
            return "필수 서류가 이 묶음에 포함되지 않았습니다.";
        }

        public static string FieldLabel(DocumentField field)
        {
            switch (field.Id.ToString())
            {
                case "ShipName":
                    return "선박명";
                case "OwnerName":
                    return "소유주";
                case "DepartureDate":
                    return "출항일";
                case "HullAge":
                    return "선령";
                case "ExpiryDate":
                    return "만료일";
                case "InspectionResult":
                    return "검사 결과";
                case "AccidentHistory":
                    return "사고 이력";
                case "RepairHistory":
                    return "수리 이력";
                case "HullDefect":
                    return "선체 결함";
                case "WeatherForecast":
                    return "기상 예보";
                case "RouteRisk":
                    return "항로 위험";
                default:
                    return field.Label;
            }
        }

        public static string FieldValue(string value)
        {
            switch (value)
            {
                case "Passed":
                    return "합격";
                case "Failed":
                    return "불합격";
                case "None":
                    return "없음";
                case "Recent":
                    return "최근 기록 있음";
                case "Major":
                    return "대규모";
                case "Present":
                    return "있음";
                case "Clear":
                    return "맑음";
                case "Known":
                    return "확인된 항로";
                case "Storm":
                    return "폭풍";
                case "Uncharted":
                    return "미확인 항로";
                default:
                    return value;
            }
        }

        public static string RuleTitle(RuleDefinition rule)
        {
            switch (rule.Id.ToString())
            {
                case "AR01":
                    return "등록증 미제출";
                case "AR02":
                    return "선박명 불일치";
                case "AR03":
                    return "소유주 불일치";
                case "AR04":
                    return "등록 만료";
                case "AR05":
                    return "선체 검사서 미제출";
                case "AR06":
                    return "선체 검사 불합격";
                case "AR07":
                    return "항로 신고서 미제출";
                case "AR08":
                    return "출항일 불일치";
                case "CR01":
                    return "노후 선체";
                case "CR02":
                    return "사고 이력";
                case "CR03":
                    return "수리 이력";
                case "CR04":
                    return "선체 결함";
                case "CR05":
                    return "악천후";
                case "CR06":
                    return "미확인 항로";
                default:
                    return rule.Title;
            }
        }

        public static string RuleFindingText(bool isTriggered)
        {
            return isTriggered ? "현재 서류에서 해당 사유가 확인됩니다." : "현재 서류에서는 해당 사유가 확인되지 않습니다.";
        }

        public static string RuleSeverityMarker(RuleSeverity severity)
        {
            return severity == RuleSeverity.AbsoluteRejection ? "AR" : "CR";
        }

        private static string ContractTypeLabel(ContractType type)
        {
            switch (type)
            {
                case ContractType.Ship:
                    return "선박";
                case ContractType.Cargo:
                    return "화물";
                case ContractType.Mixed:
                    return "복합";
                default:
                    return type.ToString();
            }
        }

        private static string DecisionLabel(PlayerDecision decision)
        {
            switch (decision)
            {
                case PlayerDecision.Approve:
                    return "승인";
                case PlayerDecision.Reject:
                    return "거절";
                case PlayerDecision.ConditionalApprove:
                    return "조건부 승인";
                default:
                    return decision.ToString();
            }
        }

        private static string DocumentKindLabel(DocumentKind kind)
        {
            switch (kind)
            {
                case DocumentKind.ShipApplication:
                    return "선박 보험 신청서";
                case DocumentKind.ShipRegistration:
                    return "선박 등록증";
                case DocumentKind.HullInspection:
                    return "선체 검사서";
                case DocumentKind.RouteDeclaration:
                    return "항로 신고서";
                case DocumentKind.CargoApplication:
                    return "화물 보험 신청서";
                case DocumentKind.CargoManifest:
                    return "화물 적하 목록";
                case DocumentKind.LoadingCertificate:
                    return "선적 증명서";
                case DocumentKind.QuarantineCertificate:
                    return "검역 증명서";
                default:
                    return kind.ToString();
            }
        }

        private static string SettlementLabel(string label)
        {
            if (label == "Objective audit score")
            {
                return "객관 심사 점수";
            }

            if (label.StartsWith("Fixture commission at "))
            {
                var percent = label.Replace("Fixture commission at ", string.Empty);
                return $"fixture 수수료 {percent}";
            }

            if (label == "Fixture accident responsibility placeholder")
            {
                return "fixture 사고 책임 임시 정산";
            }

            return label;
        }
    }
}
