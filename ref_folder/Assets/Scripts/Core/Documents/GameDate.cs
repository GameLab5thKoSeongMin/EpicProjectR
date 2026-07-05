using System;

[Serializable]
public struct GameDate : IComparable<GameDate>
{
    public int year;
    public int month;
    public int day;

    public GameDate(int year, int month, int day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
    }

    public bool IsConfigured => year > 0 && month > 0 && day > 0;

    public int CompareTo(GameDate other)
    {
        return ToDateTime().CompareTo(other.ToDateTime());
    }

    public int DaysUntil(GameDate other)
    {
        return (other.ToDateTime() - ToDateTime()).Days;
    }

    public GameDate AddDays(int days)
    {
        if (!IsConfigured)
            return default(GameDate);

        DateTime date = ToDateTime().AddDays(days);
        return new GameDate(date.Year, date.Month, date.Day);
    }

    public override string ToString()
    {
        if (!IsConfigured)
            return string.Empty;

        return string.Format("{0}-{1:00}-{2:00}", year, month, day);
    }

    private DateTime ToDateTime()
    {
        int safeYear = Math.Max(1, Math.Min(9999, year));
        int safeMonth = Math.Max(1, Math.Min(12, month));
        int safeDay = Math.Max(1, Math.Min(DateTime.DaysInMonth(safeYear, safeMonth), day));
        return new DateTime(safeYear, safeMonth, safeDay);
    }
}
