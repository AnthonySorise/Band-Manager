using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtilities
{
    public static string TimeSpanToString(this TimeSpan timespan)
    {
        string formatted = string.Format("{0}{1}{2}",
        timespan.Days > 0 ? string.Format("{0:0} day{1}, ", timespan.Days, timespan.Days == 1 ? string.Empty : "s") : string.Empty,
        timespan.Hours > 0 ? string.Format("{0:0} hour{1}, ", timespan.Hours, timespan.Hours == 1 ? string.Empty : "s") : string.Empty,
        timespan.Minutes > 0 ? string.Format("{0:0} minute{1}, ", timespan.Minutes, timespan.Minutes == 1 ? string.Empty : "s") : string.Empty);
        if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

        if (string.IsNullOrEmpty(formatted)) formatted = "0 minutes";

        return formatted;

    }
}
