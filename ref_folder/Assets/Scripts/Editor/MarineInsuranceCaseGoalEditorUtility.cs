using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MarineInsuranceCaseGoalEditorUtility
{
    public static void DrawTargetReasonSelection(
        SerializedProperty caseGoalProperty,
        IReadOnlyList<MarineInsuranceTypeDefinition> insuranceTypes)
    {
        if (caseGoalProperty == null)
            return;

        SerializedProperty modeProperty = caseGoalProperty.FindPropertyRelative("mode");

        if (modeProperty == null ||
            modeProperty.enumValueIndex != (int)MarineInsuranceCaseGoalMode.SpecificRejectionReasons)
        {
            return;
        }

        SerializedProperty idsProperty =
            caseGoalProperty.FindPropertyRelative("targetRejectionReasonIds");

        if (idsProperty == null)
            return;

        List<MarineInsuranceRejectionReasonDefinition> reasons =
            CollectReasons(insuranceTypes);

        EditorGUILayout.Space(4f);
        EditorGUILayout.LabelField("Target Rejection Reasons", EditorStyles.boldLabel);

        if (reasons.Count == 0)
        {
            EditorGUILayout.HelpBox(
                "선택된 보험 TypeDef에 등록된 거절 사유가 없습니다.",
                MessageType.Info);
            return;
        }

        for (int i = 0; i < reasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = reasons[i];
            if (reason == null)
                continue;

            string reasonId = reason.Id;
            bool wasSelected = ContainsId(idsProperty, reasonId);
            bool isSelected = EditorGUILayout.ToggleLeft(reason.DisplayName, wasSelected);

            if (isSelected != wasSelected)
            {
                if (isSelected)
                    AddId(idsProperty, reasonId);
                else
                    RemoveId(idsProperty, reasonId);
            }
        }
    }

    public static List<MarineInsuranceTypeDefinition> ReadInsuranceTypePool(
        SerializedProperty insuranceTypePoolProperty)
    {
        List<MarineInsuranceTypeDefinition> insuranceTypes =
            new List<MarineInsuranceTypeDefinition>();

        if (insuranceTypePoolProperty == null || !insuranceTypePoolProperty.isArray)
            return insuranceTypes;

        for (int i = 0; i < insuranceTypePoolProperty.arraySize; i++)
        {
            SerializedProperty item = insuranceTypePoolProperty.GetArrayElementAtIndex(i);
            MarineInsuranceTypeDefinition insuranceType =
                item.objectReferenceValue as MarineInsuranceTypeDefinition;

            if (insuranceType != null && !insuranceTypes.Contains(insuranceType))
                insuranceTypes.Add(insuranceType);
        }

        return insuranceTypes;
    }

    private static List<MarineInsuranceRejectionReasonDefinition> CollectReasons(
        IReadOnlyList<MarineInsuranceTypeDefinition> insuranceTypes)
    {
        List<MarineInsuranceRejectionReasonDefinition> reasons =
            new List<MarineInsuranceRejectionReasonDefinition>();

        if (insuranceTypes == null)
            return reasons;

        for (int i = 0; i < insuranceTypes.Count; i++)
        {
            MarineInsuranceTypeDefinition insuranceType = insuranceTypes[i];
            IReadOnlyList<MarineInsuranceRejectionReasonDefinition> typeReasons =
                insuranceType != null ? insuranceType.RejectionReasons : null;

            if (typeReasons == null)
                continue;

            for (int j = 0; j < typeReasons.Count; j++)
            {
                MarineInsuranceRejectionReasonDefinition reason = typeReasons[j];
                if (reason != null && !ContainsReason(reasons, reason))
                    reasons.Add(reason);
            }
        }

        return reasons;
    }

    private static bool ContainsReason(
        List<MarineInsuranceRejectionReasonDefinition> reasons,
        MarineInsuranceRejectionReasonDefinition target)
    {
        for (int i = 0; i < reasons.Count; i++)
        {
            if (reasons[i] == target)
                return true;
        }

        return false;
    }

    private static bool ContainsId(SerializedProperty idsProperty, string id)
    {
        if (idsProperty == null || string.IsNullOrEmpty(id))
            return false;

        for (int i = 0; i < idsProperty.arraySize; i++)
        {
            if (idsProperty.GetArrayElementAtIndex(i).stringValue == id)
                return true;
        }

        return false;
    }

    private static void AddId(SerializedProperty idsProperty, string id)
    {
        if (string.IsNullOrEmpty(id) || ContainsId(idsProperty, id))
            return;

        int index = idsProperty.arraySize;
        idsProperty.InsertArrayElementAtIndex(index);
        idsProperty.GetArrayElementAtIndex(index).stringValue = id;
    }

    private static void RemoveId(SerializedProperty idsProperty, string id)
    {
        for (int i = idsProperty.arraySize - 1; i >= 0; i--)
        {
            if (idsProperty.GetArrayElementAtIndex(i).stringValue == id)
                idsProperty.DeleteArrayElementAtIndex(i);
        }
    }
}
