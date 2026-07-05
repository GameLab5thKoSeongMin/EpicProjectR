using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarineInsuranceRejectionReasonDefinition))]
public class MarineInsuranceRejectionReasonDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawCoreFields();
        DrawTemplateFields();

        EditorGUILayout.Space(8f);

        if (GUILayout.Button("Apply Template To Conditions"))
        {
            MarineInsuranceRejectionReasonDefinition definition =
                (MarineInsuranceRejectionReasonDefinition)target;

            Undo.RecordObject(definition, "Apply Marine Rejection Rule Template");
            definition.ApplyRuleTemplate();
            EditorUtility.SetDirty(definition);
            serializedObject.Update();
        }

        EditorGUILayout.Space(8f);
        DrawAdvancedFields();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCoreFields()
    {
        DrawScriptField();

        DrawProperty("id");
        DrawProperty("displayName");
        DrawProperty("iconSprite");
        DrawProperty("description");

        EditorGUILayout.Space(6f);
        DrawProperty("decision");
        DrawProperty("dialogueResponseKey", "Dialogue Response Key");
    }

    private void DrawTemplateFields()
    {
        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Easy Rule Template", EditorStyles.boldLabel);

        SerializedProperty templateProperty = DrawProperty("ruleTemplate");

        if (templateProperty == null)
            return;

        MarineInsuranceRejectionRuleTemplate template =
            (MarineInsuranceRejectionRuleTemplate)templateProperty.enumValueIndex;

        switch (template)
        {
            case MarineInsuranceRejectionRuleTemplate.DocumentIsMissing:
                DrawProperty("dataKey", "Document Submitted Key");
                DrawHelp("Triggers when the document submitted judgment value is empty.");
                break;

            case MarineInsuranceRejectionRuleTemplate.DocumentIsSubmitted:
                DrawProperty("dataKey", "Document Submitted Key");
                DrawHelp("Triggers when the document submitted judgment value exists.");
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentValueEquals:
                DrawProperty("dataKey", "Judgment Data Key");
                DrawProperty("expectedCode", "Reject Judgment Value");
                DrawProperty("safeCode", "Safe Judgment Value");
                DrawProperty("triggerDisplayValue", "Reject Display Value");
                DrawProperty("safeDisplayValue", "Safe Display Value");
                DrawHelp("Triggers when the judgment value equals Reject Judgment Value.");
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentValueMismatch:
                DrawProperty("dataKey", "First Data Key");
                DrawProperty("compareDataKey", "Compare Data Key");
                DrawHelp("Triggers when the two judgment values differ.");
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentNumberIsAtLeast:
                DrawProperty("dataKey", "Judgment Number Key");
                DrawProperty("numberValue", "Minimum Value");
                DrawHelp("Triggers when the judgment value is greater than or equal to this number.");
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentNumberIsAtMost:
                DrawProperty("dataKey", "Judgment Number Key");
                DrawProperty("numberValue", "Maximum Value");
                DrawHelp("Triggers when the judgment value is less than or equal to this number.");
                break;

            case MarineInsuranceRejectionRuleTemplate.Custom:
                DrawHelp("Turn on Advanced Condition Editor below to edit conditions manually.");
                break;
        }
    }

    private void DrawAdvancedFields()
    {
        SerializedProperty advancedProperty = DrawProperty("showAdvancedConditionEditor", "Advanced Condition Editor");

        if (advancedProperty == null || !advancedProperty.boolValue)
        {
            EditorGUILayout.HelpBox(
                "Conditions are generated from the template. Turn on Advanced only when manual edits are needed.",
                MessageType.Info);
            return;
        }

        DrawProperty("relatedDataKeys");
        DrawProperty("conditionMatchMode");
        DrawProperty("conditions");
    }

    private SerializedProperty DrawProperty(string propertyName)
    {
        return DrawProperty(propertyName, null);
    }

    private SerializedProperty DrawProperty(string propertyName, string label)
    {
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property == null)
            return null;

        if (string.IsNullOrEmpty(label))
            EditorGUILayout.PropertyField(property, true);
        else
            EditorGUILayout.PropertyField(property, new GUIContent(label), true);

        return property;
    }

    private void DrawScriptField()
    {
        SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");

        if (scriptProperty == null)
            return;

        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.PropertyField(scriptProperty);
    }

    private void DrawHelp(string message)
    {
        EditorGUILayout.HelpBox(message, MessageType.None);
    }
}
