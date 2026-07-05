using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FixedMarineInsuranceCaseDefinition))]
public class FixedMarineInsuranceCaseDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        FixedMarineInsuranceCaseDefinition definition =
            (FixedMarineInsuranceCaseDefinition)target;

        SerializedProperty configuredCaseProperty =
            serializedObject.FindProperty("configuredCase");
        SerializedProperty insuranceTypeProperty =
            configuredCaseProperty != null
                ? configuredCaseProperty.FindPropertyRelative("insuranceType")
                : null;
        MarineInsuranceTypeDefinition insuranceType =
            insuranceTypeProperty != null
                ? insuranceTypeProperty.objectReferenceValue as MarineInsuranceTypeDefinition
                : null;

        MarineInsuranceCaseGoalEditorUtility.DrawTargetReasonSelection(
            serializedObject.FindProperty("editorCaseGoal"),
            new List<MarineInsuranceTypeDefinition> { insuranceType });

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Easy Case Setup", EditorStyles.boldLabel);

        if (GUILayout.Button("Open Case Node Editor"))
            FixedMarineInsuranceCaseNodeEditorWindow.Open(definition);

        if (GUILayout.Button("Apply Case Goal To Configured Case"))
        {
            serializedObject.ApplyModifiedProperties();
            Undo.RecordObject(definition, "Apply Marine Insurance Case Goal");
            definition.ApplyEditorCaseGoal(0);
            EditorUtility.SetDirty(definition);
            serializedObject.Update();
        }

        if (GUILayout.Button("Evaluate Configured Case"))
        {
            MarineInsuranceRejectionEvaluationResult result =
                MarineInsuranceRejectionEvaluator.Evaluate(definition.ConfiguredCase);

            Debug.Log(CreateEvaluationMessage(definition, result), definition);
        }
    }

    private string CreateEvaluationMessage(
        FixedMarineInsuranceCaseDefinition definition,
        MarineInsuranceRejectionEvaluationResult result)
    {
        if (result == null || result.triggeredReasons == null || result.triggeredReasons.Count == 0)
            return definition.name + ": no rejection reasons triggered.";

        return definition.name + ": triggered rejection reasons - " +
               string.Join(", ", result.GetTriggeredReasonNames());
    }
}
