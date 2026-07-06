// Responsibility: Centralizes measured Main.unity UI RectTransform targets for first playable parity.
using UnityEngine;

namespace EpicProjectR.Presentation
{
    public static class FirstPlayableMainSceneRectSpec
    {
        public static readonly Vector2 ReferenceResolution = new Vector2(1920f, 1080f);

        public static readonly Vector2 HeaderPosition = new Vector2(0f, -40f);
        public static readonly Vector2 HeaderSize = new Vector2(0f, 80f);

        public static readonly Vector2 EntryDocumentPosition = new Vector2(76f, -370f);
        public static readonly Vector2 EntryDocumentSize = new Vector2(168f, 122f);
        public static readonly Vector2 EntryBellPosition = new Vector2(-92f, -372f);
        public static readonly Vector2 EntryBellSize = new Vector2(86f, 68f);
        public static readonly Vector2 EntryContractorStartPosition = new Vector2(0f, -318f);
        public static readonly Vector2 EntryContractorPosition = new Vector2(-340f, -258f);
        public static readonly Vector2 EntryContractorSize = new Vector2(286f, 360f);

        public static readonly Vector2 DocketOpenPosition = new Vector2(-430f, -48f);
        public static readonly Vector2 DocketClosedPosition = new Vector2(-820f, -48f);
        public static readonly Vector2 DocketSize = new Vector2(340f, 720f);

        public static readonly Vector2 ContractorOpenPosition = new Vector2(326f, 178f);
        public static readonly Vector2 ContractorClosedPosition = new Vector2(88f, 178f);
        public static readonly Vector2 ContractorSize = new Vector2(290f, 360f);

        public static readonly Vector2 DialogueOpenPosition = new Vector2(288f, -254f);
        public static readonly Vector2 DialogueClosedPosition = new Vector2(-390f, -254f);
        public static readonly Vector2 DialogueSize = new Vector2(522f, 350f);

        public static readonly Vector2 ReviewDocumentPosition = new Vector2(430f, 96f);
        public static readonly Vector2 ReviewDocumentSize = new Vector2(154f, 110f);
        public static readonly Vector2 ReviewBellPosition = new Vector2(176f, 94f);
        public static readonly Vector2 ReviewBellSize = new Vector2(70f, 56f);

        public static readonly Vector2 WorkbenchOpenPosition = new Vector2(92f, -42f);
        public static readonly Vector2 WorkbenchClosedPosition = new Vector2(1320f, -42f);
        public static readonly Vector2 WorkbenchSize = new Vector2(920f, 884f);
        public static readonly Vector2 DocumentBoardSize = new Vector2(872f, 668f);

        public static readonly Vector2 ShelfOpenPosition = new Vector2(-224f, -42f);
        public static readonly Vector2 ShelfClosedPosition = new Vector2(430f, -42f);
        public static readonly Vector2 ShelfSize = new Vector2(336f, 884f);
        public static readonly Vector2 RulePanelImageSize = new Vector2(306f, 62f);
        public const float RuleRowHeight = 62f;

        public static readonly Vector2 FinalDecisionBoxPosition = new Vector2(-224f, 86f);
        public static readonly Vector2 FinalDecisionBoxSize = new Vector2(306f, 82f);

        public static readonly Vector2 DecisionDrawerOpenPosition = new Vector2(-224f, 292f);
        public static readonly Vector2 DecisionDrawerClosedPosition = new Vector2(-224f, -132f);
        public static readonly Vector2 DecisionDrawerSize = new Vector2(306f, 398f);
        public static readonly Vector2 DecisionButtonSize = new Vector2(112f, 64f);

        public static readonly Vector2[] DocumentPositions =
        {
            new Vector2(-250f, 176f),
            new Vector2(46f, 154f),
            new Vector2(-238f, -162f),
            new Vector2(130f, -116f)
        };

        public static readonly Vector2[] DocumentSizes =
        {
            new Vector2(252f, 300f),
            new Vector2(252f, 300f),
            new Vector2(319f, 333f),
            new Vector2(415f, 449f)
        };
    }
}
