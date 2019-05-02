using System;

public class DateTimeUtility {

    static DateTime GetNextWeekday(DayOfWeek day)
    {
        DateTime result = DateTime.Now.AddDays(1);
        while (result.DayOfWeek != day)
            result = result.AddDays(1);
        return result;
    }
}
