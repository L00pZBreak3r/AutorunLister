using System;
using System.Globalization;

namespace AutorunLibrary.Helpers.TaskScheduler
{
  internal static class DateTimeUtil
  {
    //private const string DATETIME_FORMAT_STRING = "yyyy-MM-dd HH:mm:ss.fff";
    private const string TIMESPAN_FORMAT_STRING = "G";
    internal const string V2BoundaryDateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'FFFK";
    internal static readonly CultureInfo DefaultDateCulture = CultureInfo.CreateSpecificCulture("en-US");

    public static DateTime StringToDateTime(string v)
    {
      return DateTime.ParseExact(v, V2BoundaryDateFormat, DefaultDateCulture);
    }

    public static DateTime StringToDateTime(string v, DateTime defval)
    {
      try
      {
        return StringToDateTime(v);
      }
      catch
      {
        return defval;
      }
    }

    public static string DateTimeToString(DateTime v)
    {
      return v.ToString(V2BoundaryDateFormat, DefaultDateCulture);
    }

    public static string DateTimeToString(DateTime? v, string defval = null)
    {
      return ((v != null) && v.HasValue) ? v.Value.ToString(V2BoundaryDateFormat, DefaultDateCulture) : defval;
    }

    public static string DateTimeToString(DateTime? v, DateTime defval)
    {
      return DateTimeToString(v, DateTimeToString(defval));
    }

    public static TimeSpan StringToTimeSpan(string v)
    {
      return TimeSpan.ParseExact(v, TIMESPAN_FORMAT_STRING, DefaultDateCulture);
    }

    public static TimeSpan StringToTimeSpan(string v, TimeSpan defval)
    {
      try
      {
        return StringToTimeSpan(v);
      }
      catch
      {
        return defval;
      }
    }

    public static string TimeSpanToString(TimeSpan v)
    {
      return v.ToString(TIMESPAN_FORMAT_STRING, DefaultDateCulture);
    }

    public static string TimeSpanToString(TimeSpan? v, string defval = null)
    {
      return ((v != null) && v.HasValue) ? v.Value.ToString(TIMESPAN_FORMAT_STRING, DefaultDateCulture) : defval;
    }

    public static string TimeSpanToString(TimeSpan? v, TimeSpan defval)
    {
      return TimeSpanToString(v, TimeSpanToString(defval));
    }
  }
}
