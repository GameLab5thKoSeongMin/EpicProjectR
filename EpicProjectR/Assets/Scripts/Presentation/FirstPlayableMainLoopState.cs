// Responsibility: Provides presentation-only helpers for Main-scene parity loop state and labels.
using System;

namespace EpicProjectR.Presentation
{
    public static class FirstPlayableMainLoopState
    {
        public const int ConsiderationPremiumStepPercent = 10;

        public static int PremiumPercentForSelectedConsiderations(int selectedConsiderationCount)
        {
            return 100 + Math.Max(0, selectedConsiderationCount) * ConsiderationPremiumStepPercent;
        }

        public static string ApprovalLabelForSelectedConsiderations(int selectedConsiderationCount)
        {
            return selectedConsiderationCount > 0
                ? FirstPlayableKoreanText.ConditionalApproveButton
                : FirstPlayableKoreanText.ApproveButton;
        }
    }
}
