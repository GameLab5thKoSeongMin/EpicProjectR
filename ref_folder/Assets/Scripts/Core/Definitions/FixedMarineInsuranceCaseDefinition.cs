using UnityEngine;

[CreateAssetMenu(menuName = "Marine Insurance/Case/Fixed Case")]
public class FixedMarineInsuranceCaseDefinition : MarineInsuranceCaseDefinition
{
    [SerializeField] private UnderwritingCase configuredCase = new UnderwritingCase();
    [SerializeField] private MarineInsuranceCaseGoal editorCaseGoal = new MarineInsuranceCaseGoal();

    public UnderwritingCase ConfiguredCase => configuredCase;
    public MarineInsuranceCaseGoal EditorCaseGoal => editorCaseGoal;

    public override UnderwritingCase CreateCase(int seed)
    {
        System.Random random = new System.Random(seed);
        return configuredCase != null
            ? configuredCase.CreateResolved(random)
            : new UnderwritingCase();
    }

#if UNITY_EDITOR
    public void ApplyEditorCaseGoal(int seed)
    {
        if (configuredCase == null)
            configuredCase = new UnderwritingCase();

        System.Random random = new System.Random(seed);
        MarineInsuranceCaseGoalApplier.Apply(configuredCase, editorCaseGoal, random);
    }
#endif
}
