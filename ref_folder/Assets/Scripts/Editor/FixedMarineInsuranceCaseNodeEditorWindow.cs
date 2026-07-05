using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FixedMarineInsuranceCaseNodeEditorWindow : EditorWindow
{
    private const float ColumnGap = 24f;
    private const float HeaderHeight = 46f;
    private const float NodePadding = 10f;
    private const float LineHeight = 20f;

    private FixedMarineInsuranceCaseDefinition definition;
    private SerializedObject definitionObject;
    private Vector2 leftScroll;
    private Vector2 rightScroll;
    private string selectedDataKey;
    private Rect selectedFieldRect;
    private readonly Dictionary<string, Rect> conditionRects = new Dictionary<string, Rect>();

    public static void Open(FixedMarineInsuranceCaseDefinition definition)
    {
        FixedMarineInsuranceCaseNodeEditorWindow window =
            GetWindow<FixedMarineInsuranceCaseNodeEditorWindow>("Marine Case Nodes");

        window.definition = definition;
        window.definitionObject = definition != null ? new SerializedObject(definition) : null;
        window.minSize = new Vector2(940f, 560f);
        window.Show();
    }

    private void OnGUI()
    {
        if (definition == null)
        {
            EditorGUILayout.HelpBox("Fixed Marine Insurance Case Definition을 먼저 선택하세요.", MessageType.Info);
            return;
        }

        if (definitionObject == null)
            definitionObject = new SerializedObject(definition);

        definitionObject.Update();

        DrawHeader();

        MarineInsuranceTypeDefinition insuranceType =
            definition.ConfiguredCase != null ? definition.ConfiguredCase.insuranceType : null;

        if (insuranceType == null)
        {
            EditorGUILayout.HelpBox("Configured Case에 Insurance Type을 먼저 지정해야 합니다.", MessageType.Warning);
            definitionObject.ApplyModifiedProperties();
            return;
        }

        conditionRects.Clear();
        selectedFieldRect = Rect.zero;

        Rect contentRect = new Rect(
            0f,
            HeaderHeight,
            position.width,
            position.height - HeaderHeight);
        float columnWidth = (contentRect.width - ColumnGap) * 0.5f;

        Rect leftRect = new Rect(contentRect.x, contentRect.y, columnWidth, contentRect.height);
        Rect rightRect = new Rect(leftRect.xMax + ColumnGap, contentRect.y, columnWidth, contentRect.height);

        DrawDocumentColumn(leftRect, insuranceType);
        DrawRejectionColumn(rightRect, insuranceType);
        DrawConnections();

        definitionObject.ApplyModifiedProperties();
    }

    private void DrawHeader()
    {
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.Height(HeaderHeight)))
        {
            EditorGUILayout.LabelField(definition.name, EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Evaluate", EditorStyles.toolbarButton, GUILayout.Width(90f)))
                EvaluateCurrentCase();

            if (GUILayout.Button("Apply Goal", EditorStyles.toolbarButton, GUILayout.Width(90f)))
                ApplyCaseGoal();
        }
    }

    private void DrawDocumentColumn(Rect rect, MarineInsuranceTypeDefinition insuranceType)
    {
        GUILayout.BeginArea(rect);

        EditorGUILayout.LabelField("Documents", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("서류 데이터 값을 입력하고, 연결할 데이터 행을 선택하세요.", MessageType.None);

        leftScroll = EditorGUILayout.BeginScrollView(leftScroll);

        IReadOnlyList<GameObject> prefabs = insuranceType.DocumentPrefabs;

        if (prefabs == null || prefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("TypeDef에 등록된 서류 프리팹이 없습니다.", MessageType.Info);
        }
        else
        {
            for (int i = 0; i < prefabs.Count; i++)
                DrawDocumentNode(prefabs[i]);
        }

        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void DrawDocumentNode(GameObject prefab)
    {
        if (prefab == null)
            return;

        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField(prefab.name, EditorStyles.boldLabel);

            MarineInsuranceDocumentMetadata metadata =
                prefab.GetComponentInChildren<MarineInsuranceDocumentMetadata>(true);

            DrawDocumentMetadata(metadata);

            List<DocumentFieldInfo> fields = CollectDocumentFields(prefab);

            if (fields.Count == 0)
            {
                EditorGUILayout.LabelField("데이터 키 필드가 없습니다.");
                return;
            }

            for (int i = 0; i < fields.Count; i++)
                DrawDocumentField(fields[i]);
        }
    }

    private void DrawDocumentMetadata(MarineInsuranceDocumentMetadata metadata)
    {
        if (metadata == null)
            return;

        EditorGUILayout.LabelField("Document Id", metadata.DocumentId, EditorStyles.miniLabel);

        if (!metadata.HasSubmittedKey)
        {
            EditorGUILayout.LabelField("Submitted Key", "Not Set", EditorStyles.miniLabel);
            return;
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("Submitted Key", metadata.SubmittedKey, EditorStyles.miniLabel);

            if (GUILayout.Button("Submitted", EditorStyles.miniButtonLeft, GUILayout.Width(82f)))
                ApplyDocumentSubmissionState(metadata, true);

            if (GUILayout.Button("Missing", EditorStyles.miniButtonRight, GUILayout.Width(72f)))
                ApplyDocumentSubmissionState(metadata, false);
        }
    }

    private void ApplyDocumentSubmissionState(
        MarineInsuranceDocumentMetadata metadata,
        bool isSubmitted)
    {
        if (metadata == null || definition.ConfiguredCase == null)
            return;

        Undo.RecordObject(definition, "Edit Marine Document Submitted State");
        metadata.SetSubmitted(definition.ConfiguredCase, isSubmitted);
        selectedDataKey = metadata.SubmittedKey;
        EditorUtility.SetDirty(definition);
    }

    private void DrawDocumentField(DocumentFieldInfo field)
    {
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            Rect fieldRect = GUILayoutUtility.GetRect(1f, LineHeight);
            bool isSelected = selectedDataKey == field.key;
            string label = isSelected ? "● " + field.label : field.label;

            if (GUI.Button(fieldRect, label, EditorStyles.miniButtonLeft))
                selectedDataKey = field.key;

            if (isSelected && Event.current.type == EventType.Repaint)
                selectedFieldRect = fieldRect;

            EditorGUILayout.SelectableLabel(field.key, EditorStyles.miniLabel, GUILayout.Height(LineHeight));

            string oldValue = definition.ConfiguredCase.GetText(field.key);
            string oldCode = definition.ConfiguredCase.GetCode(field.key);
            string newValue = EditorGUILayout.TextField("Display Value", oldValue);
            string newCode = EditorGUILayout.TextField("Stable Code", oldCode == oldValue ? string.Empty : oldCode);

            if (newValue != oldValue || newCode != (oldCode == oldValue ? string.Empty : oldCode))
            {
                Undo.RecordObject(definition, "Edit Marine Case Data");
                definition.ConfiguredCase.SetValue(field.key, newValue, newCode);
                EditorUtility.SetDirty(definition);
            }
        }
    }

    private void DrawRejectionColumn(Rect rect, MarineInsuranceTypeDefinition insuranceType)
    {
        GUILayout.BeginArea(rect);

        EditorGUILayout.LabelField("Rejection Reasons", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("선택한 서류 데이터를 조건에 연결하거나, 조건값을 케이스에 적용하세요.", MessageType.None);

        rightScroll = EditorGUILayout.BeginScrollView(rightScroll);

        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> reasons = insuranceType.RejectionReasons;

        if (reasons == null || reasons.Count == 0)
        {
            EditorGUILayout.HelpBox("TypeDef에 등록된 거절 사유가 없습니다.", MessageType.Info);
        }
        else
        {
            for (int i = 0; i < reasons.Count; i++)
                DrawRejectionNode(reasons[i]);
        }

        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void DrawRejectionNode(MarineInsuranceRejectionReasonDefinition reason)
    {
        if (reason == null)
            return;

        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField(reason.DisplayName, EditorStyles.boldLabel);
            EditorGUILayout.LabelField(reason.Decision.ToString(), EditorStyles.miniLabel);

            SerializedObject reasonObject = new SerializedObject(reason);
            reasonObject.Update();

            DrawTemplateControls(reasonObject, reason);

            bool wasTargetReason = ContainsTargetReason(reason.Id);
            bool isTargetReason = EditorGUILayout.ToggleLeft("Use As Case Target", wasTargetReason);

            if (isTargetReason != wasTargetReason)
                SetTargetReason(reason.Id, isTargetReason);

            SerializedProperty conditionsProperty = reasonObject.FindProperty("conditions");

            if (conditionsProperty == null || !conditionsProperty.isArray || conditionsProperty.arraySize == 0)
            {
                EditorGUILayout.LabelField("조건이 없습니다.");
            }
            else
            {
                for (int i = 0; i < conditionsProperty.arraySize; i++)
                    DrawCondition(reasonObject, conditionsProperty.GetArrayElementAtIndex(i), reason, i);
            }

            reasonObject.ApplyModifiedProperties();
        }
    }

    private void DrawTemplateControls(
        SerializedObject reasonObject,
        MarineInsuranceRejectionReasonDefinition reason)
    {
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("Easy Rule Template", EditorStyles.miniBoldLabel);

            SerializedProperty templateProperty = reasonObject.FindProperty("ruleTemplate");
            if (templateProperty != null)
                EditorGUILayout.PropertyField(templateProperty, new GUIContent("Rule Template"));

            MarineInsuranceRejectionRuleTemplate template =
                templateProperty != null
                    ? (MarineInsuranceRejectionRuleTemplate)templateProperty.enumValueIndex
                    : MarineInsuranceRejectionRuleTemplate.Custom;

            DrawTemplateFields(reasonObject, template);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Apply Template"))
                {
                    reasonObject.ApplyModifiedProperties();
                    Undo.RecordObject(reason, "Apply Marine Rejection Rule Template");
                    reason.ApplyRuleTemplate();
                    EditorUtility.SetDirty(reason);
                    reasonObject.Update();
                }

                if (GUILayout.Button("Ping RejectDef", GUILayout.Width(110f)))
                    EditorGUIUtility.PingObject(reason);
            }
        }
    }

    private void DrawTemplateFields(
        SerializedObject reasonObject,
        MarineInsuranceRejectionRuleTemplate template)
    {
        switch (template)
        {
            case MarineInsuranceRejectionRuleTemplate.DocumentIsMissing:
                DrawReasonProperty(reasonObject, "dataKey", "Document Submitted Key");
                break;

            case MarineInsuranceRejectionRuleTemplate.DocumentIsSubmitted:
                DrawReasonProperty(reasonObject, "dataKey", "Document Submitted Key");
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentValueEquals:
                DrawReasonProperty(reasonObject, "dataKey", "Judgment Data Key");
                DrawReasonProperty(reasonObject, "expectedCode", "Reject Judgment Value");
                DrawReasonProperty(reasonObject, "safeCode", "Safe Judgment Value");
                DrawReasonProperty(reasonObject, "triggerDisplayValue", "Reject Display");
                DrawReasonProperty(reasonObject, "safeDisplayValue", "Safe Display");
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentValueMismatch:
                DrawReasonProperty(reasonObject, "dataKey", "First Data Key");
                DrawReasonProperty(reasonObject, "compareDataKey", "Compare Data Key");
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentNumberIsAtLeast:
                DrawReasonProperty(reasonObject, "dataKey", "Judgment Number Key");
                DrawReasonProperty(reasonObject, "numberValue", "Minimum Value");
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentNumberIsAtMost:
                DrawReasonProperty(reasonObject, "dataKey", "Judgment Number Key");
                DrawReasonProperty(reasonObject, "numberValue", "Maximum Value");
                break;
        }
    }

    private void DrawReasonProperty(
        SerializedObject reasonObject,
        string propertyName,
        string label)
    {
        SerializedProperty property = reasonObject.FindProperty(propertyName);

        if (property != null)
            EditorGUILayout.PropertyField(property, new GUIContent(label));
    }

    private void DrawCondition(
        SerializedObject reasonObject,
        SerializedProperty conditionProperty,
        MarineInsuranceRejectionReasonDefinition reason,
        int index)
    {
        SerializedProperty keyProperty = conditionProperty.FindPropertyRelative("dataKey");
        SerializedProperty operatorProperty = conditionProperty.FindPropertyRelative("conditionOperator");

        string conditionKey = keyProperty != null ? keyProperty.stringValue : string.Empty;
        string rectKey = reason.Id + ":" + index;

        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            Rect rowRect = GUILayoutUtility.GetRect(1f, LineHeight);

            if (Event.current.type == EventType.Repaint && conditionKey == selectedDataKey)
                conditionRects[rectKey] = rowRect;

            string operatorLabel = operatorProperty != null
                ? operatorProperty.enumDisplayNames[operatorProperty.enumValueIndex]
                : "Condition";

            GUI.Label(rowRect, operatorLabel + " / " + conditionKey, EditorStyles.miniBoldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.enabled = !string.IsNullOrEmpty(selectedDataKey);

                if (GUILayout.Button("Connect Selected Data"))
                {
                    Undo.RecordObject(reason, "Connect Marine Case Data Key");
                    keyProperty.stringValue = selectedDataKey;
                    SerializedProperty templateDataKeyProperty = reasonObject.FindProperty("dataKey");

                    if (templateDataKeyProperty != null)
                        templateDataKeyProperty.stringValue = selectedDataKey;

                    reasonObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(reason);
                }

                GUI.enabled = true;

                if (GUILayout.Button("Apply Reject Value"))
                    ApplyConditionValue(conditionProperty, true);

                if (GUILayout.Button("Apply Safe Value"))
                    ApplyConditionValue(conditionProperty, false);
            }
        }
    }

    private void ApplyConditionValue(SerializedProperty conditionProperty, bool shouldTrigger)
    {
        MarineInsuranceRejectionCondition condition =
            BuildConditionCopy(conditionProperty);

        if (condition == null)
            return;

        bool created = shouldTrigger
            ? condition.TryCreateTriggerValue(definition.ConfiguredCase, out string value, out string code)
            : condition.TryCreateSafeValue(definition.ConfiguredCase, out value, out code);

        if (!created)
            return;

        Undo.RecordObject(definition, "Apply Marine Rejection Condition Value");
        definition.ConfiguredCase.SetValue(condition.DataKey, value, code);
        EditorUtility.SetDirty(definition);
    }

    private MarineInsuranceRejectionCondition BuildConditionCopy(SerializedProperty conditionProperty)
    {
        return conditionProperty.boxedValue as MarineInsuranceRejectionCondition;
    }

    private bool ContainsTargetReason(string reasonId)
    {
        SerializedProperty idsProperty = GetTargetReasonIdsProperty();

        if (idsProperty == null || string.IsNullOrEmpty(reasonId))
            return false;

        for (int i = 0; i < idsProperty.arraySize; i++)
        {
            if (idsProperty.GetArrayElementAtIndex(i).stringValue == reasonId)
                return true;
        }

        return false;
    }

    private void SetTargetReason(string reasonId, bool isTarget)
    {
        SerializedProperty idsProperty = GetTargetReasonIdsProperty();

        if (idsProperty == null || string.IsNullOrEmpty(reasonId))
            return;

        if (isTarget)
        {
            SerializedProperty modeProperty =
                definitionObject.FindProperty("editorCaseGoal").FindPropertyRelative("mode");

            if (modeProperty != null)
                modeProperty.enumValueIndex = (int)MarineInsuranceCaseGoalMode.SpecificRejectionReasons;

            if (ContainsTargetReason(reasonId))
                return;

            int index = idsProperty.arraySize;
            idsProperty.InsertArrayElementAtIndex(index);
            idsProperty.GetArrayElementAtIndex(index).stringValue = reasonId;
            return;
        }

        for (int i = idsProperty.arraySize - 1; i >= 0; i--)
        {
            if (idsProperty.GetArrayElementAtIndex(i).stringValue == reasonId)
                idsProperty.DeleteArrayElementAtIndex(i);
        }
    }

    private SerializedProperty GetTargetReasonIdsProperty()
    {
        SerializedProperty goalProperty = definitionObject.FindProperty("editorCaseGoal");
        return goalProperty != null
            ? goalProperty.FindPropertyRelative("targetRejectionReasonIds")
            : null;
    }

    private List<DocumentFieldInfo> CollectDocumentFields(GameObject prefab)
    {
        List<DocumentFieldInfo> fields = new List<DocumentFieldInfo>();

        MarineInsuranceDocumentMetadata metadata =
            prefab.GetComponentInChildren<MarineInsuranceDocumentMetadata>(true);

        if (metadata != null && metadata.HasSubmittedKey)
        {
            fields.Add(new DocumentFieldInfo
            {
                label = metadata.DocumentDisplayName + " Submitted",
                key = metadata.SubmittedKey
            });
        }

        MonoBehaviour[] components = prefab.GetComponentsInChildren<MonoBehaviour>(true);

        for (int i = 0; i < components.Length; i++)
        {
            MonoBehaviour component = components[i];

            if (component == null || !(component is IUnderwritingCaseDocumentView))
                continue;

            SerializedObject componentObject = new SerializedObject(component);
            SerializedProperty iterator = componentObject.GetIterator();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (iterator.propertyType != SerializedPropertyType.String ||
                    !iterator.name.EndsWith("Key"))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(iterator.stringValue))
                    continue;

                fields.Add(new DocumentFieldInfo
                {
                    label = ObjectNames.NicifyVariableName(iterator.name),
                    key = iterator.stringValue
                });
            }
        }

        return fields;
    }

    private void DrawConnections()
    {
        if (string.IsNullOrEmpty(selectedDataKey) || selectedFieldRect == Rect.zero)
            return;

        Handles.BeginGUI();
        Color previousColor = Handles.color;
        Handles.color = new Color(0.25f, 0.55f, 1f, 0.9f);

        foreach (KeyValuePair<string, Rect> pair in conditionRects)
        {
            if (!pair.Key.Contains(":"))
                continue;

            Vector3 start = new Vector3(selectedFieldRect.xMax, selectedFieldRect.center.y + HeaderHeight, 0f);
            Vector3 end = new Vector3(position.width * 0.5f + ColumnGap * 0.5f, pair.Value.center.y + HeaderHeight, 0f);
            Handles.DrawBezier(start, end, start + Vector3.right * 80f, end + Vector3.left * 80f, Handles.color, null, 2f);
        }

        Handles.color = previousColor;
        Handles.EndGUI();
    }

    private void ApplyCaseGoal()
    {
        Undo.RecordObject(definition, "Apply Marine Insurance Case Goal");
        definition.ApplyEditorCaseGoal(0);
        EditorUtility.SetDirty(definition);
    }

    private void EvaluateCurrentCase()
    {
        MarineInsuranceRejectionEvaluationResult result =
            MarineInsuranceRejectionEvaluator.Evaluate(definition.ConfiguredCase);

        if (result == null || result.triggeredReasons == null || result.triggeredReasons.Count == 0)
        {
            Debug.Log(definition.name + ": no rejection reasons triggered.", definition);
            return;
        }

        Debug.Log(
            definition.name + ": triggered rejection reasons - " +
            string.Join(", ", result.GetTriggeredReasonNames()),
            definition);
    }

    private class DocumentFieldInfo
    {
        public string label;
        public string key;
    }
}
