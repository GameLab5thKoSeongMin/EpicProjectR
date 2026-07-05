using System;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class MarineInsuranceLocalizedTextResolver
{
    [SerializeField] private bool useLocalization = true;
    [SerializeField] private string namesTable = "MarineNames";
    [SerializeField] private string documentsTable = "MarineDocuments";
    [SerializeField] private string formatsTable = "MarineFormats";

    [Header("Localized Name Pool Counts")]
    [SerializeField, Min(1)] private int shipNameCount = 6;
    [SerializeField, Min(1)] private int ownerNameCount = 6;
    [SerializeField, Min(1)] private int captainNameCount = 6;
    [SerializeField, Min(1)] private int portNameCount = 6;

    public string PickShipName(System.Random random)
    {
        return PickIndexed(namesTable, "ship.name", shipNameCount, random, FallbackShipNames);
    }

    public string PickOwnerName(System.Random random)
    {
        return PickIndexed(namesTable, "owner.name", ownerNameCount, random, FallbackOwnerNames);
    }

    public string PickCaptainName(System.Random random)
    {
        return PickIndexed(namesTable, "captain.name", captainNameCount, random, FallbackCaptainNames);
    }

    public string PickPortName(System.Random random)
    {
        return PickIndexed(namesTable, "port.name", portNameCount, random, FallbackPorts);
    }

    public string PickDifferentPortName(System.Random random, string current)
    {
        for (int i = 0; i < 12; i++)
        {
            string picked = PickPortName(random);
            if (picked != current)
                return picked;
        }

        return PickPortName(random);
    }

    public string Submitted()
    {
        return Get(documentsTable, "document.submitted", "Submitted");
    }

    public string ShipAge(int years)
    {
        return Format(formatsTable, "ship.age", years + " years ago", years);
    }

    public string DateDefault(int year, int month, int day)
    {
        string fallback = string.Format("{0:0000}-{1:00}-{2:00}", year, month, day);
        return Format(formatsTable, "date.default", fallback, year, month, day);
    }

    public string DateUntil(int year, int month, int day)
    {
        string fallback = string.Format("{0:0000}-{1:00}-{2:00} until", year, month, day);
        return Format(formatsTable, "date.until", fallback, year, month, day);
    }

    public string HullInspectionResult(bool isFailed)
    {
        return Get(
            documentsTable,
            isFailed ? "hull.inspection.fail" : "hull.inspection.pass",
            isFailed ? "Failed" : "Seaworthy");
    }

    public string AccidentHistory(int count)
    {
        return Format(
            formatsTable,
            "accident.history",
            "Last 12 months: " + count,
            count);
    }

    public string RepairRecord(int minor, int structural, int major)
    {
        return Format(
            formatsTable,
            "repair.record",
            "Minor " + minor + " / Structural " + structural + " / Major " + major,
            minor,
            structural,
            major);
    }

    public string BooleanExists(bool value)
    {
        return Get(documentsTable, value ? "common.exists" : "common.none", value ? "Exists" : "None");
    }

    public string Weather(string weatherCode)
    {
        return Get(documentsTable, "route.weather." + NormalizeWeatherCode(weatherCode), "Good");
    }

    public string NormalizeWeatherCode(string weatherCode)
    {
        if (string.IsNullOrEmpty(weatherCode))
            return "good";

        string lowered = weatherCode.ToLowerInvariant();

        if (lowered.Contains("storm") || lowered.Contains("bad"))
            return "storm";

        if (lowered.Contains("wind"))
            return "strong_wind";

        if (lowered.Contains("wave"))
            return "high_wave";

        if (lowered.Contains("fog"))
            return "dense_fog";

        if (lowered.Contains("normal"))
            return "normal";

        return lowered;
    }

    private string PickIndexed(
        string table,
        string keyPrefix,
        int count,
        System.Random random,
        string[] fallbackValues)
    {
        int safeCount = Mathf.Max(1, count);
        int index = random != null
            ? random.Next(0, safeCount)
            : UnityEngine.Random.Range(0, safeCount);

        string fallback = fallbackValues != null && fallbackValues.Length > 0
            ? fallbackValues[index % fallbackValues.Length]
            : keyPrefix + "." + index;

        return Get(table, keyPrefix + "." + index, fallback);
    }

    private string Get(string table, string entry, string fallback)
    {
        if (!useLocalization || string.IsNullOrEmpty(table) || string.IsNullOrEmpty(entry))
            return fallback;

        try
        {
            LocalizedString localizedString = new LocalizedString(table, entry);
            string value = localizedString.GetLocalizedString();
            return string.IsNullOrEmpty(value) ? fallback : value;
        }
        catch (Exception)
        {
            return fallback;
        }
    }

    private string Format(string table, string entry, string fallback, params object[] arguments)
    {
        if (!useLocalization || string.IsNullOrEmpty(table) || string.IsNullOrEmpty(entry))
            return fallback;

        try
        {
            LocalizedString localizedString = new LocalizedString(table, entry);
            localizedString.Arguments = arguments;
            string value = localizedString.GetLocalizedString();
            return string.IsNullOrEmpty(value) ? fallback : value;
        }
        catch (Exception)
        {
            return fallback;
        }
    }

    private static readonly string[] FallbackShipNames =
    {
        "Blue Current",
        "Morning Tide",
        "Silver Wake",
        "Harbor Star",
        "Sea Lantern",
        "Northwind"
    };

    private static readonly string[] FallbackOwnerNames =
    {
        "Hansea Shipping",
        "Eastline Marine",
        "Blueport Logistics",
        "Maris Trade",
        "Yuseong Lines",
        "Kangsan Vessel"
    };

    private static readonly string[] FallbackCaptainNames =
    {
        "Doyun Kim",
        "Seojun Park",
        "Haneul Lee",
        "Minjae Jung",
        "Yujin Choi",
        "Jihoon Oh"
    };

    private static readonly string[] FallbackPorts =
    {
        "Busan Port",
        "Incheon Port",
        "Mokpo Port",
        "Yeosu Port",
        "Pohang Port",
        "Jeju Port"
    };
}
