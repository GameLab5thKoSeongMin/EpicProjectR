using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(menuName = "Marine Insurance/Case/Random Case")]
public class RandomMarineInsuranceCaseDefinition : MarineInsuranceCaseDefinition
{
    private const string SubmittedCode = "submitted";
    private const string HullDamageLevelKey = "hull.damage.level";
    private const string HullDamageNoneCode = "none";
    private const string HullDamageMiddleCode = "middle";

    [SerializeField] private List<MarineInsuranceTypeDefinition> insuranceTypePool =
        new List<MarineInsuranceTypeDefinition>();

    [Header("Case Goal")]
    [SerializeField] private MarineInsuranceCaseGoal caseGoal = new MarineInsuranceCaseGoal();
    [SerializeField, Min(1)] private int validationAttempts = 8;

    [Header("Localization")]
    [SerializeField] private MarineInsuranceLocalizedTextResolver textResolver =
        new MarineInsuranceLocalizedTextResolver();

    [Header("NPC")]
    [SerializeField] private DocumentTextSource npcName = new DocumentTextSource();
    [SerializeField] private DocumentSpriteSource npcSprite = new DocumentSpriteSource();

    [Header("Economy")]
    [SerializeField, Min(0)] private int minimumPremium = 1000;
    [SerializeField, Min(0)] private int maximumPremium = 5000;
    [SerializeField, Min(0)] private int minimumCompensationAmount = 10000;
    [SerializeField, Min(0)] private int maximumCompensationAmount = 50000;
    [SerializeField, Min(0)] private int minimumPatience = 1;
    [SerializeField, Min(0)] private int maximumPatience = 3;
    [SerializeField, Min(1)] private int minimumContractDurationTurns = 3;
    [SerializeField, Min(1)] private int maximumContractDurationTurns = 3;

    [Header("Data Pool")]
    [SerializeField] private List<CaseDataEntry> dataPool = new List<CaseDataEntry>();
    [SerializeField] private List<CaseDataSpriteEntry> spriteDataPool = new List<CaseDataSpriteEntry>();

    public IReadOnlyList<MarineInsuranceTypeDefinition> InsuranceTypePool => insuranceTypePool;

    public override UnderwritingCase CreateCase(int seed)
    {
        UnderwritingCase fallbackCase = null;

        for (int attempt = 0; attempt < Mathf.Max(1, validationAttempts); attempt++)
        {
            System.Random random = new System.Random(seed + attempt * 9973);
            UnderwritingCase underwritingCase = CreateNaturalCase(random);
            fallbackCase = underwritingCase;

            if (IsGoalSatisfied(underwritingCase, caseGoal))
                return underwritingCase;
        }

        if (fallbackCase != null)
        {
            System.Random fallbackRandom = new System.Random(seed);
            MarineInsuranceCaseGoalApplier.Apply(fallbackCase, caseGoal, fallbackRandom);
            RefreshLocalizedDisplayValues(fallbackCase);
            return fallbackCase;
        }

        return new UnderwritingCase();
    }

    private UnderwritingCase CreateNaturalCase(System.Random random)
    {
        UnderwritingCase underwritingCase = new UnderwritingCase();
        underwritingCase.insuranceType = PickInsuranceType(random);

        ApplyDataPool(underwritingCase, random);
        ApplySpriteDataPool(underwritingCase, random);

        MarineInsuranceRandomCaseFacts facts =
            MarineInsuranceRandomCaseFacts.Create(random, textResolver);

        WriteNpc(underwritingCase, random);
        WriteEconomy(underwritingCase, random);

        List<MarineInsuranceRejectionReasonDefinition> targetReasons =
            MarineInsuranceCaseGoalApplier.SelectTargetReasons(
                underwritingCase.insuranceType,
                caseGoal,
                random);

        WriteFacts(underwritingCase, facts);
        WriteSubmittedDocuments(underwritingCase);
        ApplyTargetReasonsNaturally(underwritingCase, targetReasons, random);
        RefreshLocalizedDisplayValues(underwritingCase);
        return underwritingCase;
    }

    private void WriteNpc(UnderwritingCase underwritingCase, System.Random random)
    {
        underwritingCase.npcName = npcName != null ? npcName.Resolve(random) : string.Empty;
        underwritingCase.npcSprite = npcSprite != null ? npcSprite.Resolve(random) : null;
    }

    private void ApplyDataPool(UnderwritingCase underwritingCase, System.Random random)
    {
        if (dataPool == null)
            return;

        for (int i = 0; i < dataPool.Count; i++)
        {
            CaseDataEntry entry = dataPool[i];
            if (entry == null) continue;

            string value = entry.value != null
                ? entry.value.Resolve(random)
                : string.Empty;

            string code = entry.codeValue != null
                ? entry.codeValue.Resolve(random)
                : string.Empty;

            underwritingCase.SetValue(entry.key, value, code);
        }
    }

    private void ApplySpriteDataPool(UnderwritingCase underwritingCase, System.Random random)
    {
        if (spriteDataPool == null)
            return;

        for (int i = 0; i < spriteDataPool.Count; i++)
        {
            CaseDataSpriteEntry entry = spriteDataPool[i];
            if (entry == null) continue;

            underwritingCase.spriteData.Add(new CaseDataSpriteEntry
            {
                key = entry.key,
                value = entry.value != null
                    ? entry.value.CreateResolved(random)
                    : DocumentSpriteSource.Direct(null)
            });
        }
    }

    private void WriteEconomy(UnderwritingCase underwritingCase, System.Random random)
    {
        int minPremium = Mathf.Min(minimumPremium, maximumPremium);
        int maxPremium = Mathf.Max(minimumPremium, maximumPremium);
        int minCompensation = Mathf.Min(minimumCompensationAmount, maximumCompensationAmount);
        int maxCompensation = Mathf.Max(minimumCompensationAmount, maximumCompensationAmount);
        int minPatience = Mathf.Min(minimumPatience, maximumPatience);
        int maxPatience = Mathf.Max(minimumPatience, maximumPatience);
        int minDuration = Mathf.Min(minimumContractDurationTurns, maximumContractDurationTurns);
        int maxDuration = Mathf.Max(minimumContractDurationTurns, maximumContractDurationTurns);

        underwritingCase.premium = random.Next(minPremium, maxPremium + 1);
        underwritingCase.compensationAmount = random.Next(minCompensation, maxCompensation + 1);
        underwritingCase.patience = random.Next(minPatience, maxPatience + 1);
        underwritingCase.contractDurationTurns = random.Next(minDuration, maxDuration + 1);
        SetNumber(underwritingCase, "route.duration.turns", underwritingCase.contractDurationTurns);
    }

    private void WriteFacts(
        UnderwritingCase underwritingCase,
        MarineInsuranceRandomCaseFacts facts)
    {
        Set(underwritingCase, "ship.name", facts.shipName);
        Set(underwritingCase, "ship.owner.name", facts.ownerName);
        Set(underwritingCase, "ship.captain", facts.captainName);

        SetDate(underwritingCase, "route.departure.date", facts.departureDate);
        SetDate(underwritingCase, "route.expected.return.date", facts.expectedReturnDate);
        Set(underwritingCase, "route.departure.port", facts.departurePort);
        Set(underwritingCase, "route.destination.port", facts.destinationPort);
        SetWeather(underwritingCase, facts.weatherCode);

        SetAge(underwritingCase, "registration.build.year", facts.shipAgeYears);
        SetNumber(underwritingCase, "ship.age.years", facts.shipAgeYears);
        SetNumber(underwritingCase, "ship.build.age.years", facts.shipAgeYears);
        SetNumber(underwritingCase, "registration.build.age.years", facts.shipAgeYears);
        SetDateUntil(underwritingCase, "registration.expiration.date", facts.registrationExpirationDate);

        SetHullInspectionResult(underwritingCase, facts.hullInspectionFailed);
        SetAccidentCount(underwritingCase, facts.accidentCountLast12Months);
        SetRepairRecord(
            underwritingCase,
            facts.minorRepairCount,
            facts.structuralRepairCount,
            facts.majorRepairCount);
        SetBoolean(underwritingCase, "hull.defect.exists", facts.hasHullDefect);
        SetHullDamageLevel(
            underwritingCase,
            facts.hasHullDefect ? HullDamageMiddleCode : HullDamageNoneCode);
    }

    private void WriteSubmittedDocuments(UnderwritingCase underwritingCase)
    {
        IReadOnlyList<GameObject> prefabs =
            underwritingCase?.insuranceType != null
                ? underwritingCase.insuranceType.DocumentPrefabs
                : null;

        if (prefabs == null)
            return;

        for (int i = 0; i < prefabs.Count; i++)
        {
            GameObject prefab = prefabs[i];
            if (prefab == null)
                continue;

            MarineInsuranceDocumentMetadata metadata =
                prefab.GetComponentInChildren<MarineInsuranceDocumentMetadata>(true);

            if (metadata == null || !metadata.HasSubmittedKey)
                continue;

            underwritingCase.SetValue(metadata.SubmittedKey, textResolver.Submitted(), SubmittedCode);
        }
    }

    private void ApplyTargetReasonsNaturally(
        UnderwritingCase underwritingCase,
        IReadOnlyList<MarineInsuranceRejectionReasonDefinition> targetReasons,
        System.Random random)
    {
        if (targetReasons == null)
            return;

        for (int i = 0; i < targetReasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = targetReasons[i];
            IReadOnlyList<MarineInsuranceRejectionCondition> conditions =
                reason != null ? reason.Conditions : null;

            if (conditions == null)
                continue;

            for (int j = 0; j < conditions.Count; j++)
                ApplyConditionNaturally(underwritingCase, conditions[j], random);
        }
    }

    private void ApplyConditionNaturally(
        UnderwritingCase underwritingCase,
        MarineInsuranceRejectionCondition condition,
        System.Random random)
    {
        if (condition == null || string.IsNullOrEmpty(condition.DataKey))
            return;

        switch (condition.ConditionOperator)
        {
            case MarineInsuranceRejectionConditionOperator.TextMissing:
                underwritingCase.SetValue(condition.DataKey, string.Empty, string.Empty);
                return;

            case MarineInsuranceRejectionConditionOperator.TextEquals:
                SetNaturalCodeValue(
                    underwritingCase,
                    condition.DataKey,
                    condition.ExpectedCode,
                    condition.ExpectedText);
                return;

            case MarineInsuranceRejectionConditionOperator.TextNotEquals:
                SetDifferentNaturalValue(underwritingCase, condition.DataKey, condition.ExpectedCode);
                return;

            case MarineInsuranceRejectionConditionOperator.TextDiffersFromKey:
                SetNaturalMismatch(underwritingCase, condition.DataKey, condition.CompareDataKey, random);
                return;

            case MarineInsuranceRejectionConditionOperator.NumberAtLeast:
                SetNaturalNumber(
                    underwritingCase,
                    condition.DataKey,
                    Mathf.CeilToInt(condition.NumberValue) + random.Next(0, 6));
                return;

            case MarineInsuranceRejectionConditionOperator.NumberGreaterThan:
                SetNaturalNumber(
                    underwritingCase,
                    condition.DataKey,
                    Mathf.FloorToInt(condition.NumberValue) + 1 + random.Next(0, 6));
                return;

            case MarineInsuranceRejectionConditionOperator.NumberAtMost:
                SetNaturalNumber(
                    underwritingCase,
                    condition.DataKey,
                    Mathf.FloorToInt(condition.NumberValue) - random.Next(0, 4));
                return;

            case MarineInsuranceRejectionConditionOperator.NumberLessThan:
                SetNaturalNumber(
                    underwritingCase,
                    condition.DataKey,
                    Mathf.CeilToInt(condition.NumberValue) - 1 - random.Next(0, 4));
                return;

            case MarineInsuranceRejectionConditionOperator.DateBeforeKey:
                SetRelativeDate(
                    underwritingCase,
                    condition.DataKey,
                    condition.CompareDataKey,
                    -random.Next(1, 31));
                return;

            case MarineInsuranceRejectionConditionOperator.DateAfterKey:
                SetRelativeDate(
                    underwritingCase,
                    condition.DataKey,
                    condition.CompareDataKey,
                    random.Next(1, 31));
                return;

            case MarineInsuranceRejectionConditionOperator.DateBeforeOrSameKey:
                SetRelativeDate(
                    underwritingCase,
                    condition.DataKey,
                    condition.CompareDataKey,
                    -random.Next(0, 31));
                return;

            case MarineInsuranceRejectionConditionOperator.DateAfterOrSameKey:
                SetRelativeDate(
                    underwritingCase,
                    condition.DataKey,
                    condition.CompareDataKey,
                    random.Next(0, 31));
                return;
        }
    }

    private void SetNaturalCodeValue(
        UnderwritingCase underwritingCase,
        string key,
        string expectedCode,
        string expectedText)
    {
        string code = !string.IsNullOrEmpty(expectedCode) ? expectedCode : expectedText;

        if (string.IsNullOrEmpty(code))
            return;

        if (key.Contains("weather"))
        {
            SetWeather(underwritingCase, code);
            return;
        }

        if (key.Contains("inspection") && key.Contains("result"))
        {
            SetHullInspectionResult(underwritingCase, IsFailureCode(code));
            return;
        }

        if (key.Contains("defect"))
        {
            SetBoolean(underwritingCase, key, IsTrueCode(code));
            return;
        }

        if (key.EndsWith(".submitted", StringComparison.Ordinal))
        {
            bool isSubmitted = !string.IsNullOrEmpty(code) && code != "missing";
            underwritingCase.SetValue(
                key,
                isSubmitted ? textResolver.Submitted() : string.Empty,
                isSubmitted ? SubmittedCode : string.Empty);
            return;
        }

        Set(underwritingCase, key, code, code);
    }

    private void SetDifferentNaturalValue(
        UnderwritingCase underwritingCase,
        string key,
        string expectedCode)
    {
        if (key.EndsWith(".submitted", StringComparison.Ordinal))
        {
            underwritingCase.SetValue(key, textResolver.Submitted(), SubmittedCode);
            return;
        }

        if (string.IsNullOrEmpty(expectedCode))
        {
            Set(underwritingCase, key, "normal", "normal");
            return;
        }

        Set(underwritingCase, key, expectedCode + "_other", expectedCode + "_other");
    }

    private void SetNaturalMismatch(
        UnderwritingCase underwritingCase,
        string key,
        string compareKey,
        System.Random random)
    {
        string value = PickMismatchValue(key, underwritingCase.GetCode(compareKey), random);
        Set(underwritingCase, key, value, value);
    }

    private string PickMismatchValue(string key, string compareValue, System.Random random)
    {
        string value;

        if (key.Contains("ship.name"))
            value = textResolver.PickShipName(random);
        else if (key.Contains("owner"))
            value = textResolver.PickOwnerName(random);
        else if (key.Contains("captain"))
            value = textResolver.PickCaptainName(random);
        else
            value = "mismatch-" + random.Next(100, 999).ToString(CultureInfo.InvariantCulture);

        if (value == compareValue)
            value += " 2";

        return value;
    }

    private void SetNaturalNumber(
        UnderwritingCase underwritingCase,
        string key,
        int number)
    {
        number = Mathf.Max(0, number);

        if (key.Contains("age") || key.Contains("build.year"))
        {
            SetAge(underwritingCase, key, number);
            SetNumber(underwritingCase, "ship.age.years", number);
            SetNumber(underwritingCase, "ship.build.age.years", number);
            SetNumber(underwritingCase, "registration.build.age.years", number);
            return;
        }

        if (key.Contains("accident"))
        {
            SetAccidentCount(underwritingCase, number);
            return;
        }

        if (key.Contains("structural"))
        {
            int major = ReadInt(underwritingCase.GetCode("hull.repair.major.count"));
            int minor = ReadInt(underwritingCase.GetCode("hull.repair.minor.count"));
            SetRepairRecord(underwritingCase, minor, number, major);
            return;
        }

        if (key.Contains("major"))
        {
            int structural = ReadInt(underwritingCase.GetCode("hull.repair.structural.count"));
            int minor = ReadInt(underwritingCase.GetCode("hull.repair.minor.count"));
            SetRepairRecord(underwritingCase, minor, structural, number);
            return;
        }

        SetNumber(underwritingCase, key, number);
    }

    private void SetRelativeDate(
        UnderwritingCase underwritingCase,
        string key,
        string compareKey,
        int dayOffset)
    {
        if (!TryReadDate(underwritingCase.GetCode(compareKey), out DateTime compareDate) &&
            !TryReadDate(underwritingCase.GetText(compareKey), out compareDate))
        {
            return;
        }

        DateTime date = compareDate.AddDays(dayOffset);

        if (key.Contains("expiration"))
            SetDateUntil(underwritingCase, key, date);
        else
            SetDate(underwritingCase, key, date);
    }

    private void RefreshLocalizedDisplayValues(UnderwritingCase underwritingCase)
    {
        if (underwritingCase == null)
            return;

        RefreshDate(underwritingCase, "route.departure.date", false);
        RefreshDate(underwritingCase, "route.expected.return.date", false);
        RefreshDate(underwritingCase, "registration.expiration.date", true);
        RefreshAge(underwritingCase, "registration.build.year");
        RefreshWeather(underwritingCase);
        RefreshHullInspectionResult(underwritingCase);
        RefreshAccidentCount(underwritingCase);
        RefreshRepairRecord(underwritingCase);
        RefreshBoolean(underwritingCase, "hull.defect.exists");
    }

    private void RefreshDate(UnderwritingCase underwritingCase, string key, bool until)
    {
        if (!TryReadDate(underwritingCase.GetCode(key), out DateTime date))
            return;

        if (until)
            SetDateUntil(underwritingCase, key, date);
        else
            SetDate(underwritingCase, key, date);
    }

    private void RefreshAge(UnderwritingCase underwritingCase, string key)
    {
        if (int.TryParse(underwritingCase.GetCode(key), NumberStyles.Integer, CultureInfo.InvariantCulture, out int age))
            SetAge(underwritingCase, key, age);
    }

    private void RefreshWeather(UnderwritingCase underwritingCase)
    {
        string code = underwritingCase.GetCode("route.weather.forecast");
        if (!string.IsNullOrEmpty(code))
            SetWeather(underwritingCase, code);
    }

    private void RefreshHullInspectionResult(UnderwritingCase underwritingCase)
    {
        string code = underwritingCase.GetCode("hull.inspection.result");
        if (!string.IsNullOrEmpty(code))
            SetHullInspectionResult(underwritingCase, IsFailureCode(code));
    }

    private void RefreshAccidentCount(UnderwritingCase underwritingCase)
    {
        if (int.TryParse(underwritingCase.GetCode("hull.accident.history"), NumberStyles.Integer, CultureInfo.InvariantCulture, out int count))
            SetAccidentCount(underwritingCase, count);
    }

    private void RefreshRepairRecord(UnderwritingCase underwritingCase)
    {
        int minor = ReadInt(underwritingCase.GetCode("hull.repair.minor.count"));
        int structural = ReadInt(underwritingCase.GetCode("hull.repair.structural.count"));
        int major = ReadInt(underwritingCase.GetCode("hull.repair.major.count"));
        SetRepairRecord(underwritingCase, minor, structural, major);
    }

    private void RefreshBoolean(UnderwritingCase underwritingCase, string key)
    {
        string code = underwritingCase.GetCode(key);
        if (!string.IsNullOrEmpty(code))
            SetBoolean(underwritingCase, key, IsTrueCode(code));
    }

    private bool IsGoalSatisfied(
        UnderwritingCase underwritingCase,
        MarineInsuranceCaseGoal goal)
    {
        if (goal == null || goal.Mode == MarineInsuranceCaseGoalMode.Uncontrolled)
            return true;

        MarineInsuranceRejectionEvaluationResult result =
            MarineInsuranceRejectionEvaluator.Evaluate(underwritingCase);

        if (goal.Mode == MarineInsuranceCaseGoalMode.Approved)
            return result.triggeredReasons == null || result.triggeredReasons.Count == 0;

        if (goal.Mode == MarineInsuranceCaseGoalMode.Rejected)
            return result.HasRejectReason;

        IReadOnlyList<string> targetIds = goal.TargetRejectionReasonIds;
        if (targetIds == null || targetIds.Count == 0)
            return result.HasRejectReason;

        for (int i = 0; i < targetIds.Count; i++)
        {
            if (!ContainsTriggeredReason(result, targetIds[i]))
                return false;
        }

        return true;
    }

    private bool ContainsTriggeredReason(
        MarineInsuranceRejectionEvaluationResult result,
        string reasonId)
    {
        if (result?.triggeredReasons == null || string.IsNullOrEmpty(reasonId))
            return false;

        for (int i = 0; i < result.triggeredReasons.Count; i++)
        {
            MarineInsuranceRejectionReasonDefinition reason = result.triggeredReasons[i];
            if (reason != null && reason.Id == reasonId)
                return true;
        }

        return false;
    }

    private MarineInsuranceTypeDefinition PickInsuranceType(System.Random random)
    {
        if (insuranceTypePool == null || insuranceTypePool.Count == 0)
            return null;

        int index = random != null
            ? random.Next(0, insuranceTypePool.Count)
            : UnityEngine.Random.Range(0, insuranceTypePool.Count);

        return insuranceTypePool[index];
    }

    private void Set(UnderwritingCase underwritingCase, string key, string value)
    {
        Set(underwritingCase, key, value, value);
    }

    private void Set(UnderwritingCase underwritingCase, string key, string value, string code)
    {
        underwritingCase.SetValue(key, value, code);
    }

    private void SetNumber(UnderwritingCase underwritingCase, string key, int number)
    {
        string value = number.ToString(CultureInfo.InvariantCulture);
        Set(underwritingCase, key, value, value);
    }

    private void SetAge(UnderwritingCase underwritingCase, string key, int age)
    {
        string code = age.ToString(CultureInfo.InvariantCulture);
        Set(underwritingCase, key, textResolver.ShipAge(age), code);
    }

    private void SetDate(UnderwritingCase underwritingCase, string key, DateTime date)
    {
        Set(
            underwritingCase,
            key,
            textResolver.DateDefault(date.Year, date.Month, date.Day),
            FormatDateCode(date));
    }

    private void SetDateUntil(UnderwritingCase underwritingCase, string key, DateTime date)
    {
        Set(
            underwritingCase,
            key,
            textResolver.DateUntil(date.Year, date.Month, date.Day),
            FormatDateCode(date));
    }

    private void SetWeather(UnderwritingCase underwritingCase, string weatherCode)
    {
        string code = textResolver.NormalizeWeatherCode(weatherCode);
        Set(underwritingCase, "route.weather.forecast", textResolver.Weather(code), code);
    }

    private void SetHullInspectionResult(UnderwritingCase underwritingCase, bool isFailed)
    {
        Set(
            underwritingCase,
            "hull.inspection.result",
            textResolver.HullInspectionResult(isFailed),
            isFailed ? "fail" : "pass");
    }

    private void SetAccidentCount(UnderwritingCase underwritingCase, int count)
    {
        count = Mathf.Max(0, count);
        Set(
            underwritingCase,
            "hull.accident.history",
            textResolver.AccidentHistory(count),
            count.ToString(CultureInfo.InvariantCulture));
    }

    private void SetRepairRecord(
        UnderwritingCase underwritingCase,
        int minor,
        int structural,
        int major)
    {
        minor = Mathf.Max(0, minor);
        structural = Mathf.Max(0, structural);
        major = Mathf.Max(0, major);

        Set(
            underwritingCase,
            "hull.repair.record",
            textResolver.RepairRecord(minor, structural, major),
            "minor=" + minor + ";structural=" + structural + ";major=" + major);

        SetNumber(underwritingCase, "hull.repair.minor.count", minor);
        SetNumber(underwritingCase, "hull.repair.structural.count", structural);
        SetNumber(underwritingCase, "hull.repair.major.count", major);
    }

    private void SetBoolean(UnderwritingCase underwritingCase, string key, bool value)
    {
        Set(underwritingCase, key, textResolver.BooleanExists(value), value ? "true" : "false");

        if (key == "hull.defect.exists")
            SetHullDamageLevelIfUnset(
                underwritingCase,
                value ? HullDamageMiddleCode : HullDamageNoneCode);
    }

    private void SetHullDamageLevelIfUnset(UnderwritingCase underwritingCase, string code)
    {
        string currentCode = underwritingCase.GetCode(HullDamageLevelKey);

        if (string.IsNullOrEmpty(currentCode) || currentCode == HullDamageNoneCode)
            SetHullDamageLevel(underwritingCase, code);
    }

    private void SetHullDamageLevel(UnderwritingCase underwritingCase, string code)
    {
        Set(underwritingCase, HullDamageLevelKey, code, code);
    }

    private bool TryReadDate(string value, out DateTime date)
    {
        if (DateTime.TryParseExact(
                value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
        {
            return true;
        }

        return DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
    }

    private int ReadInt(string value)
    {
        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result)
            ? result
            : 0;
    }

    private string FormatDateCode(DateTime date)
    {
        return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    private bool IsFailureCode(string code)
    {
        if (string.IsNullOrEmpty(code))
            return false;

        string lowered = code.ToLowerInvariant();
        return lowered.Contains("fail") || lowered.Contains("reject");
    }

    private bool IsTrueCode(string code)
    {
        if (string.IsNullOrEmpty(code))
            return false;

        string lowered = code.ToLowerInvariant();
        return lowered == "true" ||
               lowered == "yes" ||
               lowered == "1";
    }

    private class MarineInsuranceRandomCaseFacts
    {
        public string shipName;
        public string ownerName;
        public string captainName;
        public string departurePort;
        public string destinationPort;
        public DateTime departureDate;
        public DateTime expectedReturnDate;
        public DateTime registrationExpirationDate;
        public int shipAgeYears;
        public bool hullInspectionFailed;
        public int accidentCountLast12Months;
        public int minorRepairCount;
        public int structuralRepairCount;
        public int majorRepairCount;
        public bool hasHullDefect;
        public string weatherCode;

        public static MarineInsuranceRandomCaseFacts Create(
            System.Random random,
            MarineInsuranceLocalizedTextResolver textResolver)
        {
            DateTime departureDate = DateTime.Today.AddDays(random.Next(7, 91));
            DateTime expectedReturnDate = departureDate.AddDays(random.Next(3, 21));

            MarineInsuranceRandomCaseFacts facts = new MarineInsuranceRandomCaseFacts();
            facts.shipName = textResolver.PickShipName(random);
            facts.ownerName = textResolver.PickOwnerName(random);
            facts.captainName = textResolver.PickCaptainName(random);
            facts.departurePort = textResolver.PickPortName(random);
            facts.destinationPort = textResolver.PickDifferentPortName(random, facts.departurePort);
            facts.departureDate = departureDate;
            facts.expectedReturnDate = expectedReturnDate;
            facts.registrationExpirationDate = expectedReturnDate.AddDays(random.Next(30, 731));
            facts.shipAgeYears = random.Next(1, 15);
            facts.hullInspectionFailed = false;
            facts.accidentCountLast12Months = 0;
            facts.minorRepairCount = random.Next(0, 3);
            facts.structuralRepairCount = 0;
            facts.majorRepairCount = 0;
            facts.hasHullDefect = false;
            facts.weatherCode = random.Next(0, 2) == 0 ? "good" : "normal";
            return facts;
        }
    }
}
