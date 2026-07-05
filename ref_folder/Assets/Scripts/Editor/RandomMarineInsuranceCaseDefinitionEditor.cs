using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(RandomMarineInsuranceCaseDefinition))]
public class RandomMarineInsuranceCaseDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "m_Script");

        SerializedProperty insuranceTypePoolProperty =
            serializedObject.FindProperty("insuranceTypePool");
        SerializedProperty caseGoalProperty =
            serializedObject.FindProperty("caseGoal");

        List<MarineInsuranceTypeDefinition> insuranceTypes =
            MarineInsuranceCaseGoalEditorUtility.ReadInsuranceTypePool(insuranceTypePoolProperty);

        MarineInsuranceCaseGoalEditorUtility.DrawTargetReasonSelection(
            caseGoalProperty,
            insuranceTypes);

        serializedObject.ApplyModifiedProperties();
    }
}
