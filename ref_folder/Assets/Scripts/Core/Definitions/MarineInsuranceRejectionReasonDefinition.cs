using System.Collections.Generic;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

public enum MarineInsuranceRejectionDecision
{
    Reject,
    ConsiderReject
}

public enum MarineInsuranceRejectionConditionMatchMode
{
    All,
    Any
}

public enum MarineInsuranceRejectionConditionOperator
{
    TextMissing,
    TextEquals,
    TextNotEquals,
    TextContains,
    TextDoesNotContain,
    TextMatchesKey,
    TextDiffersFromKey,
    NumberAtLeast,
    NumberGreaterThan,
    NumberAtMost,
    NumberLessThan,
    NumberAfterTextAtLeast,
    DateBeforeKey,
    DateAfterKey,
    DateBeforeOrSameKey,
    DateAfterOrSameKey
}

public enum MarineInsuranceNumberSource
{
    FirstNumber,
    LastNumber
}

public enum MarineInsuranceConditionValueSource
{
    [InspectorName("Display Text")]
    DisplayText,

    [InspectorName("Judgment Value")]
    StableCode
}

public enum MarineInsuranceRejectionRuleTemplate
{
    [InspectorName("Custom / Advanced")]
    Custom,

    [InspectorName("Document Is Missing")]
    DocumentIsMissing,

    [InspectorName("Document Is Submitted")]
    DocumentIsSubmitted,

    [InspectorName("Judgment Value Equals")]
    JudgmentValueEquals,

    [InspectorName("Judgment Value Mismatch")]
    JudgmentValueMismatch,

    [InspectorName("Judgment Number Is At Least")]
    JudgmentNumberIsAtLeast,

    [InspectorName("Judgment Number Is At Most")]
    JudgmentNumberIsAtMost
}

[Serializable]
public class MarineInsuranceRejectionCondition
{
    [SerializeField] private string dataKey;
    [SerializeField] private MarineInsuranceRejectionConditionOperator conditionOperator =
        MarineInsuranceRejectionConditionOperator.TextEquals;
    [SerializeField] private MarineInsuranceConditionValueSource valueSource =
        MarineInsuranceConditionValueSource.DisplayText;
    [SerializeField] private string expectedText;
    [SerializeField] private string expectedCode;
    [SerializeField] private string compareDataKey;
    [SerializeField] private float numberValue;
    [SerializeField] private MarineInsuranceNumberSource numberSource = MarineInsuranceNumberSource.LastNumber;
    [SerializeField] private string numberMarkerText;
    [SerializeField] private bool ignoreCase = true;
    [SerializeField] private bool useCustomGenerationValues;
    [SerializeField] private string triggerValueOverride;
    [SerializeField] private string safeValueOverride;
    [SerializeField] private string triggerCodeOverride;
    [SerializeField] private string safeCodeOverride;

    public string DataKey => dataKey;
    public MarineInsuranceRejectionConditionOperator ConditionOperator => conditionOperator;
    public MarineInsuranceConditionValueSource ValueSource => valueSource;
    public string ExpectedText => expectedText;
    public string ExpectedCode => expectedCode;
    public string CompareDataKey => compareDataKey;
    public float NumberValue => numberValue;
    public MarineInsuranceNumberSource NumberSource => numberSource;
    public string NumberMarkerText => numberMarkerText;
    public bool IgnoreCase => ignoreCase;
    public bool UseCustomGenerationValues => useCustomGenerationValues;
    public string TriggerValueOverride => triggerValueOverride;
    public string SafeValueOverride => safeValueOverride;
    public string TriggerCodeOverride => triggerCodeOverride;
    public string SafeCodeOverride => safeCodeOverride;

    public static MarineInsuranceRejectionCondition TextEqualsCode(
        string dataKey,
        string expectedCode,
        string safeCode,
        string triggerDisplayValue,
        string safeDisplayValue)
    {
        MarineInsuranceRejectionCondition condition = new MarineInsuranceRejectionCondition();
        condition.dataKey = dataKey;
        condition.conditionOperator = MarineInsuranceRejectionConditionOperator.TextEquals;
        condition.valueSource = MarineInsuranceConditionValueSource.StableCode;
        condition.expectedCode = expectedCode;
        condition.useCustomGenerationValues = true;
        condition.triggerValueOverride = !string.IsNullOrEmpty(triggerDisplayValue)
            ? triggerDisplayValue
            : expectedCode;
        condition.safeValueOverride = !string.IsNullOrEmpty(safeDisplayValue)
            ? safeDisplayValue
            : safeCode;
        condition.triggerCodeOverride = expectedCode;
        condition.safeCodeOverride = safeCode;
        return condition;
    }

    public static MarineInsuranceRejectionCondition TextDiffersFromKeyCode(
        string dataKey,
        string compareDataKey)
    {
        MarineInsuranceRejectionCondition condition = new MarineInsuranceRejectionCondition();
        condition.dataKey = dataKey;
        condition.compareDataKey = compareDataKey;
        condition.conditionOperator = MarineInsuranceRejectionConditionOperator.TextDiffersFromKey;
        condition.valueSource = MarineInsuranceConditionValueSource.StableCode;
        return condition;
    }

    public static MarineInsuranceRejectionCondition TextMissing(string dataKey)
    {
        MarineInsuranceRejectionCondition condition = new MarineInsuranceRejectionCondition();
        condition.dataKey = dataKey;
        condition.conditionOperator = MarineInsuranceRejectionConditionOperator.TextMissing;
        condition.valueSource = MarineInsuranceConditionValueSource.StableCode;
        return condition;
    }

    public static MarineInsuranceRejectionCondition TextPresent(string dataKey)
    {
        MarineInsuranceRejectionCondition condition = new MarineInsuranceRejectionCondition();
        condition.dataKey = dataKey;
        condition.conditionOperator = MarineInsuranceRejectionConditionOperator.TextNotEquals;
        condition.valueSource = MarineInsuranceConditionValueSource.StableCode;
        condition.expectedCode = string.Empty;
        return condition;
    }

    public static MarineInsuranceRejectionCondition Number(
        string dataKey,
        MarineInsuranceRejectionConditionOperator conditionOperator,
        float numberValue)
    {
        MarineInsuranceRejectionCondition condition = new MarineInsuranceRejectionCondition();
        condition.dataKey = dataKey;
        condition.conditionOperator = conditionOperator;
        condition.valueSource = MarineInsuranceConditionValueSource.StableCode;
        condition.numberValue = numberValue;
        condition.numberSource = MarineInsuranceNumberSource.LastNumber;
        return condition;
    }

    public static MarineInsuranceRejectionCondition NumberAfterTextAtLeast(
        string dataKey,
        string markerCode,
        float numberValue)
    {
        MarineInsuranceRejectionCondition condition = Number(
            dataKey,
            MarineInsuranceRejectionConditionOperator.NumberAfterTextAtLeast,
            numberValue);
        condition.numberMarkerText = markerCode;
        return condition;
    }

    public static MarineInsuranceRejectionCondition DateCompareCode(
        string dataKey,
        string compareDataKey,
        MarineInsuranceRejectionConditionOperator conditionOperator)
    {
        MarineInsuranceRejectionCondition condition = new MarineInsuranceRejectionCondition();
        condition.dataKey = dataKey;
        condition.compareDataKey = compareDataKey;
        condition.conditionOperator = conditionOperator;
        condition.valueSource = MarineInsuranceConditionValueSource.StableCode;
        return condition;
    }

    public bool IsMet(UnderwritingCase underwritingCase)
    {
        string value = GetValue(underwritingCase, dataKey);
        string compareValue = GetValue(underwritingCase, compareDataKey);
        string expectedValue = GetExpectedValue();

        switch (conditionOperator)
        {
            case MarineInsuranceRejectionConditionOperator.TextMissing:
                return string.IsNullOrWhiteSpace(value);

            case MarineInsuranceRejectionConditionOperator.TextEquals:
                return TextEquals(value, expectedValue);

            case MarineInsuranceRejectionConditionOperator.TextNotEquals:
                return !TextEquals(value, expectedValue);

            case MarineInsuranceRejectionConditionOperator.TextContains:
                return Contains(value, expectedValue);

            case MarineInsuranceRejectionConditionOperator.TextDoesNotContain:
                return !Contains(value, expectedValue);

            case MarineInsuranceRejectionConditionOperator.TextMatchesKey:
                return TextEquals(value, compareValue);

            case MarineInsuranceRejectionConditionOperator.TextDiffersFromKey:
                return !TextEquals(value, compareValue);

            case MarineInsuranceRejectionConditionOperator.NumberAtLeast:
                return TryReadNumber(value, out float atLeastNumber) && atLeastNumber >= numberValue;

            case MarineInsuranceRejectionConditionOperator.NumberGreaterThan:
                return TryReadNumber(value, out float greaterNumber) && greaterNumber > numberValue;

            case MarineInsuranceRejectionConditionOperator.NumberAtMost:
                return TryReadNumber(value, out float atMostNumber) && atMostNumber <= numberValue;

            case MarineInsuranceRejectionConditionOperator.NumberLessThan:
                return TryReadNumber(value, out float lessNumber) && lessNumber < numberValue;

            case MarineInsuranceRejectionConditionOperator.NumberAfterTextAtLeast:
                return TryReadNumberAfterText(value, numberMarkerText, out float markedNumber) &&
                       markedNumber >= numberValue;

            case MarineInsuranceRejectionConditionOperator.DateBeforeKey:
                return TryReadDate(value, out DateTime beforeDate) &&
                       TryReadDate(compareValue, out DateTime beforeCompareDate) &&
                       beforeDate < beforeCompareDate;

            case MarineInsuranceRejectionConditionOperator.DateAfterKey:
                return TryReadDate(value, out DateTime afterDate) &&
                       TryReadDate(compareValue, out DateTime afterCompareDate) &&
                       afterDate > afterCompareDate;

            case MarineInsuranceRejectionConditionOperator.DateBeforeOrSameKey:
                return TryReadDate(value, out DateTime beforeOrSameDate) &&
                       TryReadDate(compareValue, out DateTime beforeOrSameCompareDate) &&
                       beforeOrSameDate <= beforeOrSameCompareDate;

            case MarineInsuranceRejectionConditionOperator.DateAfterOrSameKey:
                return TryReadDate(value, out DateTime afterOrSameDate) &&
                       TryReadDate(compareValue, out DateTime afterOrSameCompareDate) &&
                       afterOrSameDate >= afterOrSameCompareDate;

            default:
                return false;
        }
    }

    public bool TryCreateTriggerValue(UnderwritingCase underwritingCase, out string value)
    {
        return TryCreateGeneratedValue(underwritingCase, true, out value, out _);
    }

    public bool TryCreateSafeValue(UnderwritingCase underwritingCase, out string value)
    {
        return TryCreateGeneratedValue(underwritingCase, false, out value, out _);
    }

    public bool TryCreateTriggerValue(UnderwritingCase underwritingCase, out string value, out string code)
    {
        return TryCreateGeneratedValue(underwritingCase, true, out value, out code);
    }

    public bool TryCreateSafeValue(UnderwritingCase underwritingCase, out string value, out string code)
    {
        return TryCreateGeneratedValue(underwritingCase, false, out value, out code);
    }

    private bool TryCreateGeneratedValue(
        UnderwritingCase underwritingCase,
        bool shouldTrigger,
        out string value,
        out string code)
    {
        value = string.Empty;
        code = string.Empty;

        if (string.IsNullOrEmpty(dataKey))
            return false;

        if (useCustomGenerationValues)
        {
            string overrideValue = shouldTrigger ? triggerValueOverride : safeValueOverride;
            string overrideCode = shouldTrigger ? triggerCodeOverride : safeCodeOverride;
            if (!string.IsNullOrEmpty(overrideValue) || !string.IsNullOrEmpty(overrideCode))
            {
                value = !string.IsNullOrEmpty(overrideValue) ? overrideValue : overrideCode;
                code = overrideCode;
                return true;
            }
        }

        string compareValue = GetValue(underwritingCase, compareDataKey);
        string expectedValue = GetExpectedValue();

        switch (conditionOperator)
        {
            case MarineInsuranceRejectionConditionOperator.TextMissing:
                value = shouldTrigger ? string.Empty : "submitted";
                code = value;
                return true;

            case MarineInsuranceRejectionConditionOperator.TextEquals:
                value = shouldTrigger ? expectedValue : MakeDifferentText(expectedValue);
                code = GetGeneratedCode(shouldTrigger, expectedValue);
                return true;

            case MarineInsuranceRejectionConditionOperator.TextNotEquals:
                value = shouldTrigger ? MakeDifferentText(expectedValue) : expectedValue;
                code = GetGeneratedCode(!shouldTrigger, expectedValue);
                return true;

            case MarineInsuranceRejectionConditionOperator.TextContains:
                value = shouldTrigger ? expectedValue : MakeTextWithout(expectedValue);
                code = GetGeneratedCode(shouldTrigger, expectedValue);
                return true;

            case MarineInsuranceRejectionConditionOperator.TextDoesNotContain:
                value = shouldTrigger ? MakeTextWithout(expectedValue) : expectedValue;
                code = GetGeneratedCode(!shouldTrigger, expectedValue);
                return true;

            case MarineInsuranceRejectionConditionOperator.TextMatchesKey:
                value = shouldTrigger ? compareValue : MakeDifferentText(compareValue);
                code = value;
                return true;

            case MarineInsuranceRejectionConditionOperator.TextDiffersFromKey:
                value = shouldTrigger ? MakeDifferentText(compareValue) : compareValue;
                code = value;
                return true;

            case MarineInsuranceRejectionConditionOperator.NumberAtLeast:
                value = FormatNumber(shouldTrigger ? Mathf.Ceil(numberValue) : Mathf.Ceil(numberValue) - 1f);
                code = value;
                return true;

            case MarineInsuranceRejectionConditionOperator.NumberGreaterThan:
                value = FormatNumber(shouldTrigger ? Mathf.Floor(numberValue) + 1f : numberValue);
                code = value;
                return true;

            case MarineInsuranceRejectionConditionOperator.NumberAtMost:
                value = FormatNumber(shouldTrigger ? numberValue : Mathf.Floor(numberValue) + 1f);
                code = value;
                return true;

            case MarineInsuranceRejectionConditionOperator.NumberLessThan:
                value = FormatNumber(shouldTrigger ? Mathf.Ceil(numberValue) - 1f : numberValue);
                code = value;
                return true;

            case MarineInsuranceRejectionConditionOperator.NumberAfterTextAtLeast:
                float generatedNumber = shouldTrigger
                    ? Mathf.Ceil(numberValue)
                    : Mathf.Max(0f, Mathf.Ceil(numberValue) - 1f);
                value = (numberMarkerText ?? string.Empty) + " " + FormatNumber(generatedNumber) + "회";
                code = (numberMarkerText ?? string.Empty) + " " + FormatNumber(generatedNumber);
                return true;

            case MarineInsuranceRejectionConditionOperator.DateBeforeKey:
                return TryCreateRelativeDate(compareValue, shouldTrigger ? -1 : 1, out value, out code);

            case MarineInsuranceRejectionConditionOperator.DateAfterKey:
                return TryCreateRelativeDate(compareValue, shouldTrigger ? 1 : -1, out value, out code);

            case MarineInsuranceRejectionConditionOperator.DateBeforeOrSameKey:
                return TryCreateRelativeDate(compareValue, shouldTrigger ? 0 : 1, out value, out code);

            case MarineInsuranceRejectionConditionOperator.DateAfterOrSameKey:
                return TryCreateRelativeDate(compareValue, shouldTrigger ? 0 : -1, out value, out code);

            default:
                return false;
        }
    }

    private string GetValue(UnderwritingCase underwritingCase, string key)
    {
        if (underwritingCase == null)
            return string.Empty;

        return valueSource == MarineInsuranceConditionValueSource.StableCode
            ? underwritingCase.GetCode(key)
            : underwritingCase.GetText(key);
    }

    private string GetExpectedValue()
    {
        if (valueSource == MarineInsuranceConditionValueSource.StableCode &&
            !string.IsNullOrEmpty(expectedCode))
        {
            return expectedCode;
        }

        return expectedText;
    }

    private string GetGeneratedCode(bool matchesExpected, string expectedValue)
    {
        if (valueSource != MarineInsuranceConditionValueSource.StableCode)
            return string.Empty;

        if (matchesExpected)
            return !string.IsNullOrEmpty(expectedCode) ? expectedCode : expectedValue;

        return MakeDifferentText(!string.IsNullOrEmpty(expectedCode) ? expectedCode : expectedValue);
    }

    private bool TextEquals(string left, string right)
    {
        StringComparison comparison = ignoreCase
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        return string.Equals(left ?? string.Empty, right ?? string.Empty, comparison);
    }

    private bool Contains(string value, string text)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(text))
            return false;

        StringComparison comparison = ignoreCase
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        return value.IndexOf(text, comparison) >= 0;
    }

    private bool TryReadNumber(string value, out float number)
    {
        MatchCollection matches = Regex.Matches(value ?? string.Empty, @"-?\d+(\.\d+)?");

        if (matches.Count == 0)
        {
            number = 0f;
            return false;
        }

        Match match = numberSource == MarineInsuranceNumberSource.FirstNumber
            ? matches[0]
            : matches[matches.Count - 1];

        return float.TryParse(
            match.Value,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out number);
    }

    private bool TryReadNumberAfterText(string value, string marker, out float number)
    {
        number = 0f;

        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(marker))
            return false;

        StringComparison comparison = ignoreCase
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        int markerIndex = value.IndexOf(marker, comparison);

        if (markerIndex < 0)
            return false;

        string slicedValue = value.Substring(markerIndex + marker.Length);
        Match match = Regex.Match(slicedValue, @"-?\d+(\.\d+)?");

        return match.Success &&
               float.TryParse(
                   match.Value,
                   NumberStyles.Float,
                   CultureInfo.InvariantCulture,
                   out number);
    }

    private bool TryReadDate(string value, out DateTime date)
    {
        Match koreanDateMatch = Regex.Match(
            value ?? string.Empty,
            @"(?<year>\d{4})\s*년\s*(?<month>\d{1,2})\s*월\s*(?<day>\d{1,2})\s*일");

        if (koreanDateMatch.Success &&
            int.TryParse(koreanDateMatch.Groups["year"].Value, out int year) &&
            int.TryParse(koreanDateMatch.Groups["month"].Value, out int month) &&
            int.TryParse(koreanDateMatch.Groups["day"].Value, out int day))
        {
            try
            {
                date = new DateTime(year, month, day);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        return DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
    }

    private bool TryCreateRelativeDate(string compareValue, int dayOffset, out string value, out string code)
    {
        if (!TryReadDate(compareValue, out DateTime compareDate))
        {
            value = string.Empty;
            code = string.Empty;
            return false;
        }

        DateTime date = compareDate.AddDays(dayOffset);
        value = FormatKoreanDate(date);
        code = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return true;
    }

    private string MakeDifferentText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "different";

        if (TextEquals(text, "불합격"))
            return "항해 가능";

        if (TextEquals(text, "악천후"))
            return "양호";

        return text + " 불일치";
    }

    private string MakeTextWithout(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "normal";

        if (TextEquals(text, "악천후"))
            return "양호";

        if (TextEquals(text, "불합격"))
            return "항해 가능";

        return "none";
    }

    private string FormatNumber(float value)
    {
        if (Mathf.Approximately(value, Mathf.Round(value)))
            return Mathf.RoundToInt(value).ToString(CultureInfo.InvariantCulture);

        return value.ToString("0.##", CultureInfo.InvariantCulture);
    }

    private string FormatKoreanDate(DateTime date)
    {
        return string.Format(CultureInfo.InvariantCulture, "{0:0000}년 {1:00}월 {2:00}일", date.Year, date.Month, date.Day);
    }
}

[CreateAssetMenu(menuName = "Marine Insurance/Rejection Reason")]
public class MarineInsuranceRejectionReasonDefinition : DefinitionBase
{
    [SerializeField] private MarineInsuranceRejectionDecision decision = MarineInsuranceRejectionDecision.Reject;
    [SerializeField] private string dialogueResponseKey;
    [TextArea]
    [SerializeField] private string missedReasonPaperText;
    [SerializeField] private MarineInsuranceRejectionRuleTemplate ruleTemplate;
    [SerializeField] private string dataKey;
    [SerializeField] private string compareDataKey;
    [SerializeField] private string expectedCode;
    [SerializeField] private string safeCode;
    [SerializeField] private float numberValue;
    [SerializeField] private string triggerDisplayValue;
    [SerializeField] private string safeDisplayValue;
    [SerializeField] private bool showAdvancedConditionEditor;
    [SerializeField] private List<string> relatedDataKeys = new List<string>();
    [SerializeField] private MarineInsuranceRejectionConditionMatchMode conditionMatchMode =
        MarineInsuranceRejectionConditionMatchMode.All;
    [SerializeField] private List<MarineInsuranceRejectionCondition> conditions =
        new List<MarineInsuranceRejectionCondition>();

    public MarineInsuranceRejectionDecision Decision => decision;
    public string DialogueResponseKey => dialogueResponseKey;
    public string MissedReasonPaperText => missedReasonPaperText;
    public MarineInsuranceRejectionRuleTemplate RuleTemplate => ruleTemplate;
    public bool ShowAdvancedConditionEditor => showAdvancedConditionEditor;
    public IReadOnlyList<string> RelatedDataKeys => relatedDataKeys;
    public MarineInsuranceRejectionConditionMatchMode ConditionMatchMode => conditionMatchMode;
    public IReadOnlyList<MarineInsuranceRejectionCondition> Conditions => conditions;

    public void ApplyRuleTemplate()
    {
        if (conditions == null)
            conditions = new List<MarineInsuranceRejectionCondition>();

        conditions.Clear();
        conditionMatchMode = MarineInsuranceRejectionConditionMatchMode.All;

        switch (ruleTemplate)
        {
            case MarineInsuranceRejectionRuleTemplate.DocumentIsMissing:
                conditions.Add(MarineInsuranceRejectionCondition.TextMissing(
                    ValueOrDefault(dataKey, "document.submitted")));
                break;

            case MarineInsuranceRejectionRuleTemplate.DocumentIsSubmitted:
                conditions.Add(MarineInsuranceRejectionCondition.TextPresent(
                    ValueOrDefault(dataKey, "document.submitted")));
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentValueEquals:
                conditions.Add(MarineInsuranceRejectionCondition.TextEqualsCode(
                    ValueOrDefault(dataKey, "data.key"),
                    ValueOrDefault(expectedCode, "reject_value"),
                    ValueOrDefault(safeCode, "safe_value"),
                    triggerDisplayValue,
                    safeDisplayValue));
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentValueMismatch:
                conditions.Add(MarineInsuranceRejectionCondition.TextDiffersFromKeyCode(
                    ValueOrDefault(dataKey, "first.data.key"),
                    ValueOrDefault(compareDataKey, "compare.data.key")));
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentNumberIsAtLeast:
                conditions.Add(MarineInsuranceRejectionCondition.Number(
                    ValueOrDefault(dataKey, "number.data.key"),
                    MarineInsuranceRejectionConditionOperator.NumberAtLeast,
                    NumberOrDefault(1f)));
                break;

            case MarineInsuranceRejectionRuleTemplate.JudgmentNumberIsAtMost:
                conditions.Add(MarineInsuranceRejectionCondition.Number(
                    ValueOrDefault(dataKey, "number.data.key"),
                    MarineInsuranceRejectionConditionOperator.NumberAtMost,
                    NumberOrDefault(0f)));
                break;
        }

        RefreshRelatedDataKeysFromConditions();
    }

    public bool IsTriggered(UnderwritingCase underwritingCase)
    {
        if (conditions == null || conditions.Count == 0)
            return false;

        if (conditionMatchMode == MarineInsuranceRejectionConditionMatchMode.Any)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                MarineInsuranceRejectionCondition condition = conditions[i];
                if (condition != null && condition.IsMet(underwritingCase))
                    return true;
            }

            return false;
        }

        for (int i = 0; i < conditions.Count; i++)
        {
            MarineInsuranceRejectionCondition condition = conditions[i];
            if (condition == null || !condition.IsMet(underwritingCase))
                return false;
        }

        return true;
    }

    private void RefreshRelatedDataKeysFromConditions()
    {
        if (relatedDataKeys == null)
            relatedDataKeys = new List<string>();

        relatedDataKeys.Clear();

        if (conditions == null)
            return;

        for (int i = 0; i < conditions.Count; i++)
        {
            MarineInsuranceRejectionCondition condition = conditions[i];

            if (condition == null)
                continue;

            AddRelatedDataKey(condition.DataKey);
            AddRelatedDataKey(condition.CompareDataKey);
        }
    }

    private void AddRelatedDataKey(string key)
    {
        if (string.IsNullOrEmpty(key) || relatedDataKeys.Contains(key))
            return;

        relatedDataKeys.Add(key);
    }

    private string ValueOrDefault(string value, string fallback)
    {
        return !string.IsNullOrEmpty(value) ? value : fallback;
    }

    private float NumberOrDefault(float fallback)
    {
        return numberValue > 0f ? numberValue : fallback;
    }
}
