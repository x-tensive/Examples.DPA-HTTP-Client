namespace DpaHttpClient
{
    public enum FilterPeriodType
    {
        Day = 0,
        Today = 8,
        Yesterday = 9,
        WorkWeek = 1,
        Week = 2,
        Month = 3,
        LastNHours = 5,
        LastNDays = 6,
        LastNMinutes = 7,
        CurrentShift = 10,
        PreviousShift = 11,
        LastSelectedShift = 12,
        CustomPeriod = 4,
        CurrentWeek = 13,
        CurrentMonth = 14
    }
}
