// Responsibility: Centralizes measured Main.unity UI RectTransform targets for first playable parity.
using UnityEngine;

namespace EpicProjectR.Presentation
{
    public static class FirstPlayableMainSceneRectSpec
    {
        public static readonly Vector2 ReferenceResolution = new Vector2(1920f, 1080f);

        public static readonly Vector2 HeaderPosition = new Vector2(0f, -39f);
        public static readonly Vector2 HeaderSize = new Vector2(0f, 78f);

        public static readonly Vector2 EntryDocumentPosition = new Vector2(0f, -331f);
        public static readonly Vector2 EntryDocumentSize = new Vector2(420f, 310f);
        public static readonly Vector2 EntryBellPosition = new Vector2(-150f, -330f);
        public static readonly Vector2 EntryBellSize = new Vector2(120f, 92f);
        public static readonly Vector2 EntryContractorPosition = new Vector2(138f, -256f);
        public static readonly Vector2 EntryContractorSize = new Vector2(190f, 240f);

        public static readonly Vector2 DocketOpenPosition = new Vector2(-430f, -48f);
        public static readonly Vector2 DocketClosedPosition = new Vector2(-820f, -48f);
        public static readonly Vector2 DocketSize = new Vector2(340f, 720f);

        public static readonly Vector2 ContractorOpenPosition = new Vector2(290f, 120f);
        public static readonly Vector2 ContractorClosedPosition = new Vector2(88f, 120f);
        public static readonly Vector2 ContractorSize = new Vector2(280f, 354f);

        public static readonly Vector2 DialogueOpenPosition = new Vector2(375f, -220f);
        public static readonly Vector2 DialogueClosedPosition = new Vector2(-490f, -220f);
        public static readonly Vector2 DialogueSize = new Vector2(690f, 250f);

        public static readonly Vector2 WorkbenchOpenPosition = new Vector2(44f, -39f);
        public static readonly Vector2 WorkbenchClosedPosition = new Vector2(1371f, -39f);
        public static readonly Vector2 WorkbenchSize = new Vector2(869f, 1001f);
        public static readonly Vector2 DocumentBoardSize = new Vector2(820f, 760f);

        public static readonly Vector2 ShelfOpenPosition = new Vector2(-242f, -40f);
        public static readonly Vector2 ShelfClosedPosition = new Vector2(485f, -40f);
        public static readonly Vector2 ShelfSize = new Vector2(430f, 1001f);
        public static readonly Vector2 RulePanelImageSize = new Vector2(419f, 73f);
        public const float RuleRowHeight = 73f;

        public static readonly Vector2 FinalDecisionBoxPosition = new Vector2(-241f, 96f);
        public static readonly Vector2 FinalDecisionBoxSize = new Vector2(120f, 75f);

        public static readonly Vector2 DecisionDrawerOpenPosition = new Vector2(-241f, 200f);
        public static readonly Vector2 DecisionDrawerClosedPosition = new Vector2(-241f, -94f);
        public static readonly Vector2 DecisionDrawerSize = new Vector2(481f, 399f);
        public static readonly Vector2 DecisionButtonSize = new Vector2(120f, 65f);

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
