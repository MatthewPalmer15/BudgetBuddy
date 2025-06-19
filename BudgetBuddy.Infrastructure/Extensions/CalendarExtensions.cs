using System.Globalization;

namespace BudgetBuddy.Infrastructure.Extensions;

public static class CalendarExtensions
{
    public static (DateTime StartOfWeek, DateTime EndOfWeek) GetWeekStartAndEnd(this Calendar calendar, int year, int weekNumber, CalendarWeekRule weekRule = CalendarWeekRule.FirstFourDayWeek, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
    {
        var firstOfYear = new DateTime(year, 1, 1);

        var daysOffset = firstDayOfWeek - firstOfYear.DayOfWeek;

        var firstWeekStart = firstOfYear.AddDays(daysOffset);

        var firstWeek = calendar.GetWeekOfYear(firstOfYear, weekRule, firstDayOfWeek);
        if (firstWeek != 1)
        {
            firstWeekStart = firstWeekStart.AddDays(7);
        }

        var startOfWeek = firstWeekStart.AddDays((weekNumber - 1) * 7);
        var endOfWeek = startOfWeek.AddDays(6);

        return (startOfWeek.Date, endOfWeek.Date);
    }
}