using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public static class MarineInsuranceLocalizationAssetGenerator
{
    private const string OutputFolder = "Assets/Localization";
    private const string AutoCreateSessionKey = "MarineInsuranceLocalizationAssetGenerator.AutoCreateAttempted";

    [InitializeOnLoadMethod]
    private static void AutoCreateMissingBaseLocalizationAssets()
    {
        if (SessionState.GetBool(AutoCreateSessionKey, false))
            return;

        if (LocalizationEditorSettings.GetStringTableCollection("MarineNames") != null &&
            LocalizationEditorSettings.GetStringTableCollection("MarineDocuments") != null &&
            LocalizationEditorSettings.GetStringTableCollection("MarineFormats") != null &&
            LocalizationEditorSettings.GetStringTableCollection("Dialogue") != null)
        {
            return;
        }

        SessionState.SetBool(AutoCreateSessionKey, true);
        EditorApplication.delayCall += CreateBaseLocalizationAssets;
    }

    [MenuItem("Tools/Marine Insurance/Create Base Localization Assets")]
    public static void CreateBaseLocalizationAssets()
    {
        EnsureFolder(OutputFolder);

        List<Locale> locales = new List<Locale>(LocalizationEditorSettings.GetLocales());

        if (locales.Count == 0)
        {
            Debug.LogError("No Localization locales found. Create at least Korean and English locales first.");
            return;
        }

        CreateNamesTable(locales);
        CreateDocumentsTable(locales);
        CreateFormatsTable(locales);
        CreateDialogueTable(locales);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Created marine insurance base localization assets.");
    }

    private static void CreateNamesTable(IList<Locale> locales)
    {
        StringTableCollection collection = GetOrCreateCollection("MarineNames", locales);

        Add(collection, "ship.name.0", "청해호", "Blue Current");
        Add(collection, "ship.name.1", "금빛마루", "Golden Ridge");
        Add(collection, "ship.name.2", "남풍호", "Southwind");
        Add(collection, "ship.name.3", "은파호", "Silver Wake");
        Add(collection, "ship.name.4", "새벽누리", "Dawn Horizon");
        Add(collection, "ship.name.5", "푸른항로", "Azure Route");

        Add(collection, "owner.name.0", "한서해운", "Hansea Shipping");
        Add(collection, "owner.name.1", "동명상선", "Eastline Marine");
        Add(collection, "owner.name.2", "청파물류", "Blueport Logistics");
        Add(collection, "owner.name.3", "해림무역", "Maris Trade");
        Add(collection, "owner.name.4", "유성해운", "Yuseong Lines");
        Add(collection, "owner.name.5", "강산선박", "Kangsan Vessel");

        Add(collection, "captain.name.0", "김도윤", "Doyun Kim");
        Add(collection, "captain.name.1", "박서준", "Seojun Park");
        Add(collection, "captain.name.2", "이하늘", "Haneul Lee");
        Add(collection, "captain.name.3", "정민재", "Minjae Jung");
        Add(collection, "captain.name.4", "최유진", "Yujin Choi");
        Add(collection, "captain.name.5", "오지훈", "Jihoon Oh");

        Add(collection, "port.name.0", "부산항", "Busan Port");
        Add(collection, "port.name.1", "인천항", "Incheon Port");
        Add(collection, "port.name.2", "목포항", "Mokpo Port");
        Add(collection, "port.name.3", "여수항", "Yeosu Port");
        Add(collection, "port.name.4", "포항항", "Pohang Port");
        Add(collection, "port.name.5", "제주항", "Jeju Port");
    }

    private static void CreateDocumentsTable(IList<Locale> locales)
    {
        StringTableCollection collection = GetOrCreateCollection("MarineDocuments", locales);

        Add(collection, "document.submitted", "제출됨", "Submitted");
        Add(collection, "hull.inspection.pass", "항해 가능", "Seaworthy");
        Add(collection, "hull.inspection.fail", "불합격", "Failed");

        Add(collection, "route.weather.good", "양호", "Good");
        Add(collection, "route.weather.normal", "보통", "Normal");
        Add(collection, "route.weather.strong_wind", "악천후(강풍)", "Severe weather (strong wind)");
        Add(collection, "route.weather.high_wave", "악천후(높은 파도)", "Severe weather (high waves)");
        Add(collection, "route.weather.dense_fog", "악천후(짙은 해무)", "Severe weather (dense sea fog)");
        Add(collection, "route.weather.storm", "악천후(폭풍권)", "Severe weather (storm zone)");

        Add(collection, "common.exists", "있음", "Exists");
        Add(collection, "common.none", "없음", "None");
    }

    private static void CreateFormatsTable(IList<Locale> locales)
    {
        StringTableCollection collection = GetOrCreateCollection("MarineFormats", locales);

        AddSmart(collection, "date.default", "{0:0000}년 {1:00}월 {2:00}일", "{1:00}/{2:00}/{0:0000}");
        AddSmart(collection, "date.until", "{0:0000}년 {1:00}월 {2:00}일 까지", "Until {1:00}/{2:00}/{0:0000}");
        AddSmart(collection, "ship.age", "{0}년 전", "{0} years ago");
        AddSmart(collection, "accident.history", "최근 12개월 이내 {0}회", "{0} accident(s) in the last 12 months");
        AddSmart(collection, "repair.record", "경미 보수 {0}회 / 구조 보수 {1}회 / 대수리 {2}회", "Minor repairs {0} / Structural repairs {1} / Major repairs {2}");
    }

    private static void CreateDialogueTable(IList<Locale> locales)
    {
        StringTableCollection collection = GetOrCreateCollection("Dialogue", locales);

        Add(collection, "contractor.greeting.0", "안녕하십니까. {insurance.ship.owner.name} 소속 선박 {insurance.ship.name} 건으로 왔습니다.", "Good day. I am here regarding {insurance.ship.name}, owned by {insurance.ship.owner.name}.");
        Add(collection, "contractor.greeting.1", "{insurance.ship.name}의 보험 심사를 부탁드립니다.", "I would like to request an insurance review for {insurance.ship.name}.");
        Add(collection, "contractor.quest_intro.0", "출항일은 {insurance.departure.date}이고 목적지는 {route.destination.port}입니다.", "The departure date is {insurance.departure.date}, and the destination is {route.destination.port}.");
        Add(collection, "contractor.quest_intro.1", "필요한 서류는 모두 제출했다고 알고 있습니다.", "I believe all required documents have been submitted.");

        Add(collection, "response.default.truth.0", "확인하신 내용이 맞습니다.", "Your finding is correct.");
        Add(collection, "response.default.lie.0", "그 부분은 다시 확인해 주셨으면 합니다.", "I would like you to check that part again.");
        Add(collection, "response.document.registration_missing.0", "선박 등록증은 곧 제출하겠습니다.", "The ship registration certificate will be submitted shortly.");
        Add(collection, "response.document.hull_inspection_missing.0", "선체 검정서는 정비소에서 아직 도착하지 않았습니다.", "The hull inspection certificate has not arrived from the yard yet.");
        Add(collection, "response.document.route_missing.0", "항로 신고서는 항만 쪽 확인 후 제출하겠습니다.", "The route declaration will be submitted after port confirmation.");
        Add(collection, "response.hull.failed.0", "검정 판정이 불합격으로 적혀 있다면 심사에 반영해 주십시오.", "If the inspection result is marked failed, please reflect that in the review.");
        Add(collection, "response.weather.bad.0", "기상 예측이 좋지 않다면 조건부 판단이 필요할 수 있겠습니다.", "If the forecast is poor, a conditional decision may be needed.");
    }

    private static StringTableCollection GetOrCreateCollection(string collectionName, IList<Locale> locales)
    {
        StringTableCollection collection =
            LocalizationEditorSettings.GetStringTableCollection(collectionName);

        if (collection == null)
        {
            collection = LocalizationEditorSettings.CreateStringTableCollection(
                collectionName,
                OutputFolder,
                locales);
        }
        else
        {
            for (int i = 0; i < locales.Count; i++)
            {
                Locale locale = locales[i];

                if (collection.GetTable(locale.Identifier) == null)
                    collection.AddNewTable(locale.Identifier);
            }
        }

        return collection;
    }

    private static void Add(
        StringTableCollection collection,
        string key,
        string korean,
        string english)
    {
        foreach (StringTable table in collection.StringTables)
        {
            string value = IsKorean(table.LocaleIdentifier.Code) ? korean : english;
            StringTableEntry entry = table.GetEntry(key) ?? table.AddEntry(key, value);
            entry.Value = value;
            entry.IsSmart = false;
            EditorUtility.SetDirty(table);
        }

        EditorUtility.SetDirty(collection.SharedData);
    }

    private static void AddSmart(
        StringTableCollection collection,
        string key,
        string korean,
        string english)
    {
        foreach (StringTable table in collection.StringTables)
        {
            string value = IsKorean(table.LocaleIdentifier.Code) ? korean : english;
            StringTableEntry entry = table.GetEntry(key) ?? table.AddEntry(key, value);
            entry.Value = value;
            entry.IsSmart = true;
            EditorUtility.SetDirty(table);
        }

        EditorUtility.SetDirty(collection.SharedData);
    }

    private static bool IsKorean(string localeCode)
    {
        return !string.IsNullOrEmpty(localeCode) && localeCode.StartsWith("ko");
    }

    private static void EnsureFolder(string folder)
    {
        if (AssetDatabase.IsValidFolder(folder))
            return;

        string[] parts = folder.Split('/');
        string current = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string next = current + "/" + parts[i];

            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);

            current = next;
        }
    }
}
