using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MarineInsuranceRejectionCondition))]
public class MarineInsuranceRejectionConditionDrawer : PropertyDrawer
{
    private const float LineSpacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect foldoutRect = LineRect(position, 0);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

        if (!property.isExpanded)
        {
            EditorGUI.EndProperty();
            return;
        }

        EditorGUI.indentLevel++;

        int lineIndex = 1;
        SerializedProperty operatorProperty = property.FindPropertyRelative("conditionOperator");

        DrawProperty(position, property, "dataKey", "Data Key", ref lineIndex);
        DrawProperty(position, operatorProperty, "Condition Operator", ref lineIndex);
        DrawProperty(position, property, "valueSource", "Value Source", ref lineIndex);

        MarineInsuranceRejectionConditionOperator conditionOperator =
            (MarineInsuranceRejectionConditionOperator)operatorProperty.enumValueIndex;

        DrawOperatorFields(position, property, conditionOperator, ref lineIndex);
        DrawProperty(position, property, "useCustomGenerationValues", "Use Custom Generation Values", ref lineIndex);

        SerializedProperty customGenerationProperty =
            property.FindPropertyRelative("useCustomGenerationValues");

        if (customGenerationProperty != null && customGenerationProperty.boolValue)
        {
            DrawProperty(position, property, "triggerValueOverride", "Trigger Display Value", ref lineIndex);
            DrawProperty(position, property, "safeValueOverride", "Safe Display Value", ref lineIndex);

            if (UsesStableCode(property))
            {
                DrawProperty(position, property, "triggerCodeOverride", "Trigger Code", ref lineIndex);
                DrawProperty(position, property, "safeCodeOverride", "Safe Code", ref lineIndex);
            }
        }

        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lineCount = 1;

        if (property.isExpanded)
        {
            lineCount += 3;

            SerializedProperty operatorProperty = property.FindPropertyRelative("conditionOperator");
            MarineInsuranceRejectionConditionOperator conditionOperator =
                (MarineInsuranceRejectionConditionOperator)operatorProperty.enumValueIndex;

            lineCount += GetOperatorFieldCount(conditionOperator);
            lineCount += 1;

            SerializedProperty customGenerationProperty =
                property.FindPropertyRelative("useCustomGenerationValues");

            if (customGenerationProperty != null && customGenerationProperty.boolValue)
            {
                lineCount += 2;

                if (UsesStableCode(property))
                    lineCount += 2;
            }
        }

        return lineCount * EditorGUIUtility.singleLineHeight +
               Mathf.Max(0, lineCount - 1) * LineSpacing;
    }

    private void DrawOperatorFields(
        Rect position,
        SerializedProperty property,
        MarineInsuranceRejectionConditionOperator conditionOperator,
        ref int lineIndex)
    {
        switch (conditionOperator)
        {
            case MarineInsuranceRejectionConditionOperator.TextEquals:
            case MarineInsuranceRejectionConditionOperator.TextNotEquals:
            case MarineInsuranceRejectionConditionOperator.TextContains:
            case MarineInsuranceRejectionConditionOperator.TextDoesNotContain:
                DrawProperty(
                    position,
                    property,
                    UsesStableCode(property) ? "expectedCode" : "expectedText",
                    UsesStableCode(property) ? "Expected Code" : "Expected Text",
                    ref lineIndex);
                DrawProperty(position, property, "ignoreCase", "Ignore Case", ref lineIndex);
                break;

            case MarineInsuranceRejectionConditionOperator.TextMatchesKey:
            case MarineInsuranceRejectionConditionOperator.TextDiffersFromKey:
                DrawProperty(position, property, "compareDataKey", "Compare Data Key", ref lineIndex);
                DrawProperty(position, property, "ignoreCase", "Ignore Case", ref lineIndex);
                break;

            case MarineInsuranceRejectionConditionOperator.NumberAtLeast:
            case MarineInsuranceRejectionConditionOperator.NumberGreaterThan:
            case MarineInsuranceRejectionConditionOperator.NumberAtMost:
            case MarineInsuranceRejectionConditionOperator.NumberLessThan:
                DrawProperty(position, property, "numberValue", "Number Value", ref lineIndex);
                DrawProperty(position, property, "numberSource", "Number Source", ref lineIndex);
                break;

            case MarineInsuranceRejectionConditionOperator.NumberAfterTextAtLeast:
                DrawProperty(position, property, "numberMarkerText", "Marker Text", ref lineIndex);
                DrawProperty(position, property, "numberValue", "Number Value", ref lineIndex);
                DrawProperty(position, property, "ignoreCase", "Ignore Case", ref lineIndex);
                break;

            case MarineInsuranceRejectionConditionOperator.DateBeforeKey:
            case MarineInsuranceRejectionConditionOperator.DateAfterKey:
            case MarineInsuranceRejectionConditionOperator.DateBeforeOrSameKey:
            case MarineInsuranceRejectionConditionOperator.DateAfterOrSameKey:
                DrawProperty(position, property, "compareDataKey", "Compare Data Key", ref lineIndex);
                break;
        }
    }

    private int GetOperatorFieldCount(MarineInsuranceRejectionConditionOperator conditionOperator)
    {
        switch (conditionOperator)
        {
            case MarineInsuranceRejectionConditionOperator.TextEquals:
            case MarineInsuranceRejectionConditionOperator.TextNotEquals:
            case MarineInsuranceRejectionConditionOperator.TextContains:
            case MarineInsuranceRejectionConditionOperator.TextDoesNotContain:
            case MarineInsuranceRejectionConditionOperator.TextMatchesKey:
            case MarineInsuranceRejectionConditionOperator.TextDiffersFromKey:
            case MarineInsuranceRejectionConditionOperator.NumberAtLeast:
            case MarineInsuranceRejectionConditionOperator.NumberGreaterThan:
            case MarineInsuranceRejectionConditionOperator.NumberAtMost:
            case MarineInsuranceRejectionConditionOperator.NumberLessThan:
                return 2;

            case MarineInsuranceRejectionConditionOperator.NumberAfterTextAtLeast:
                return 3;

            case MarineInsuranceRejectionConditionOperator.DateBeforeKey:
            case MarineInsuranceRejectionConditionOperator.DateAfterKey:
            case MarineInsuranceRejectionConditionOperator.DateBeforeOrSameKey:
            case MarineInsuranceRejectionConditionOperator.DateAfterOrSameKey:
                return 1;

            default:
                return 0;
        }
    }

    private bool UsesStableCode(SerializedProperty property)
    {
        SerializedProperty valueSourceProperty = property.FindPropertyRelative("valueSource");
        return valueSourceProperty != null &&
               valueSourceProperty.enumValueIndex == (int)MarineInsuranceConditionValueSource.StableCode;
    }

    private void DrawProperty(
        Rect position,
        SerializedProperty parent,
        string propertyName,
        string label,
        ref int lineIndex)
    {
        SerializedProperty property = parent.FindPropertyRelative(propertyName);
        DrawProperty(position, property, label, ref lineIndex);
    }

    private void DrawProperty(
        Rect position,
        SerializedProperty property,
        string label,
        ref int lineIndex)
    {
        if (property == null)
            return;

        EditorGUI.PropertyField(LineRect(position, lineIndex), property, new GUIContent(label));
        lineIndex++;
    }

    private Rect LineRect(Rect position, int lineIndex)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        return new Rect(
            position.x,
            position.y + lineIndex * (lineHeight + LineSpacing),
            position.width,
            lineHeight);
    }
}
