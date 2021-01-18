using System;
using System.Globalization;


namespace Game.Utility
{

    public static class TimeUtils
    {
        const int Now = 5;
        const int Second = 1;
        const int Minute = 60 * Second;
        const int Hour = 60 * Minute;
        const int Day = 24 * Hour;
        const int Month = 30 * Day;

		public const string DATE_FORMAT = "yyyy-MM-dd";
		public const string TIME_FORMAT = "HH:mm:ss";
		public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
		
		private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Converts a DateTime to a relative time string.
		/// </summary>
		public static string ToRelativeTimeString(this DateTime timestamp)
        {

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - timestamp.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta <= Now)
            {
                return "Now";
            }

            if (delta < Minute)
            {
                return string.Format("{0} seconds ago", ts.Seconds);
            }

            if (delta < 2 * Minute)
            {
                return "A minute ago";
            }

            if (delta < Hour)
            {
                return string.Format("{0} minutes ago", ts.Minutes);
            }

            if (delta < 2 * Hour)
            {
                return "An hour ago";
            }

            if (delta < 24 * Hour)
            {
                return string.Format("{0} hours ago", ts.Hours);
            }

            if (delta < 48 * Hour)
            {
                return "Yesterday";
            }

            if (delta < 30 * Day)
            {
                return string.Format("{0} days ago", ts.Days);
            }

            if (delta < 12 * Month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "One month ago" : string.Format("{0} months ago", months);
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "One year ago" : string.Format("{0} years ago", years);
            }
        }

		public static string FormatElapsedSeconds(int seconds)
		{
			TimeSpan elapsed = TimeSpan.FromSeconds(seconds);
			return FormatTimeSpan(elapsed);
		}

		public static string FormatElapsedSeconds(long seconds)
		{
			TimeSpan elapsed = TimeSpan.FromSeconds(seconds);
			return FormatTimeSpan(elapsed);
		}

		// Returns a string of the form h:mm:ss, [m]m:ss, or :ss 
		public static string FormatTimeSpan(TimeSpan elapsed)
		{
			if (elapsed.Hours > 0)
			{
				return elapsed.Hours.ToString("0") + ":" + elapsed.Minutes.ToString("00") + ":" + elapsed.Seconds.ToString("00");
			}
			else if (elapsed.Minutes > 0)
			{
				return elapsed.Minutes.ToString() + ":" + elapsed.Seconds.ToString("00");
			}
			else
			{
				return ":" + elapsed.Seconds.ToString("00");
			}
		}

		public static DateTime Parse(string s)
		{
			return Parse(s, DateTimeStyles.None);
		}

		// "s" must be in either Database.DATETIME_FORMAT or Database.DATE_FORMAT
		public static DateTime Parse(string s, DateTimeStyles options)
		{

			//return Convert.ToDateTime(s);			// The slow way

			string format;
			if (s.Length == DATETIME_FORMAT.Length)
			{
				format = DATETIME_FORMAT;
			}
			else if (s.Length == DATE_FORMAT.Length)
			{
				format = DATE_FORMAT;
			}
			else
			{
				throw new ArgumentException("Input string length does not match that of a supported date[time] format string");
			}

			return DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, options);
		}

		public static string DateString(DateTime d)
		{
			return d.ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
		}

		public static string DateTimeString(DateTime d)
		{
			return d.ToString(DATETIME_FORMAT, CultureInfo.InvariantCulture);
		}

		public static string TimeString(DateTime d)
		{
			return d.ToString(TIME_FORMAT, CultureInfo.InvariantCulture);
		}

		public static DateTime ParseSimpleDateTime(string dateTime)
		{
			return DateTime.Parse(dateTime, CultureInfo.InvariantCulture);
		}

		public static DateTime? TimeFromString(string time)
		{
			DateTime dateTime;
			if (DateTime.TryParseExact(time, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out dateTime))
			{
				return dateTime;
			}

			return null;
		}

		public static TimeZoneInfo NYC_TimeZone
		{
			get
			{
				try
				{
					// iOS, Android, .NET Core on Mac OS
					return TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
				}
				catch
				{
					// UWP, .NET Core on Windows
					return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
				}
			}
		}

		public static TimeZoneInfo California_TimeZone
		{
			get
			{
				try
				{
					// iOS, Android, .NET Core on Mac OS
					return TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
				}
				catch
				{
					// UWP, .NET Core on Windows
					return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
				}
			}
		}

		// TODO : Juhi: solution for time exception
		public static int NYC_UTC_OffsetSeconds => -14400;
		
		public static int GetUnixTime(DateTime utc) {
			return (int) (utc.Subtract(epoch)).TotalSeconds;
		}
		
    }

    // Converting DateTime from a string and between UTC/local can be relatively expensive in MonoTouch.
	// DateTimeLazy and WorldDateTimeLazy encapsulate a means to lazily call those methods as needed
	// from a reference DateTime string representation (expected to be in Database.DATETIME_FORMAT or DATE_FORMAT).

	public class DateTimeLazy
	{
		//public class DateTimeLazy : IComparable<DateTimeLazy> {

		protected string referenceDateTimeString;
		protected DateTime? referenceDateTime;

		protected bool parsed = false;

		public DateTimeLazy(string referenceDateTimeString)
		{
			this.referenceDateTimeString = referenceDateTimeString;
			//this.referenceDateTimeString = (referenceDateTimeString != null) ? referenceDateTimeString : String.Empty;
		}

		public DateTimeLazy(DateTime? referenceDateTime)
		{

			if (referenceDateTime.HasValue)
			{
				this.referenceDateTimeString = TimeUtils.DateString(referenceDateTime.Value);
			}

			this.referenceDateTime = referenceDateTime;

			this.parsed = true;
		}

		public virtual string ReferenceDateTimeString
		{
			get { return this.referenceDateTimeString; }
		}

		public DateTime? ReferenceDateTime
		{
			get
			{
				EnsureParsed();
				return this.referenceDateTime;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} [Absolute]", this.referenceDateTimeString);
		}

		protected virtual void EnsureParsed()
		{

			if (this.parsed) return;

			if (!String.IsNullOrEmpty(this.referenceDateTimeString))
			{
				this.referenceDateTime = TimeUtils.Parse(this.referenceDateTimeString);
			}

			this.parsed = true;
		}

		/*
		public virtual int CompareTo(DateTimeLazy other) {
			return this.referenceDateTimeString.CompareTo(other.referenceDateTimeString);
		}
		*/
	}

	public class WorldDateTimeLazy : DateTimeLazy
	{

		protected DateTimeKind referenceDateTimeKind;

		protected DateTime? dateTimeUTC;
		protected DateTime? dateTimeLocal;

		protected string dateTimeStringUTC;
		protected string dateTimeStringLocal;

		public WorldDateTimeLazy(string referenceDateTimeString, DateTimeKind referenceDateTimeKind) : base(referenceDateTimeString)
		{

			if (referenceDateTimeKind == DateTimeKind.Unspecified) throw new ArgumentException("Unsupported DateTimeKind: " + referenceDateTimeKind);

			this.referenceDateTimeKind = referenceDateTimeKind;
		}

		public virtual DateTimeKind ReferenceDateTimeKind
		{
			get { return this.referenceDateTimeKind; }
		}

		public DateTime? UTC_DateTime
		{
			get
			{
				EnsureParsed();
				return this.dateTimeUTC;
			}
		}

		public string UTC_DateTimeString
		{
			get
			{
				if (this.referenceDateTimeKind == DateTimeKind.Utc)
				{   // Optimization
					return this.referenceDateTimeString;
				}
				else
				{
					EnsureParsed();
					return this.dateTimeStringUTC;
				}
			}
		}

		public DateTime? LocalDateTime
		{
			get
			{
				EnsureParsed();
				return this.dateTimeLocal;
			}
		}

		public string LocalDateTimeString
		{
			get
			{
				if (this.referenceDateTimeKind == DateTimeKind.Local)
				{   // Optimization
					return this.referenceDateTimeString;
				}
				else
				{
					EnsureParsed();
					return this.dateTimeStringLocal;
				}
			}
		}

		public static bool IsEarlierThan(WorldDateTimeLazy a, WorldDateTimeLazy b)
		{
			DateTime aUTC = ((a != null) && a.UTC_DateTime.HasValue) ? a.UTC_DateTime.Value : DateTime.MaxValue;
			DateTime bUTC = ((b != null) && b.UTC_DateTime.HasValue) ? b.UTC_DateTime.Value : DateTime.MaxValue;
			return (aUTC.CompareTo(bUTC) < 0);
		}

		public static WorldDateTimeLazy Earliest(WorldDateTimeLazy a, WorldDateTimeLazy b)
		{
			return IsEarlierThan(a, b) ? a : b;
		}

		public static bool IsLaterThan(WorldDateTimeLazy a, WorldDateTimeLazy b)
		{
			DateTime aUTC = ((a != null) && a.UTC_DateTime.HasValue) ? a.UTC_DateTime.Value : DateTime.MinValue;
			DateTime bUTC = ((b != null) && b.UTC_DateTime.HasValue) ? b.UTC_DateTime.Value : DateTime.MinValue;
			return (aUTC.CompareTo(bUTC) > 0);
		}

		public static WorldDateTimeLazy Latest(WorldDateTimeLazy a, WorldDateTimeLazy b)
		{
			return IsLaterThan(a, b) ? a : b;
		}

		public override string ToString()
		{
			return string.Format("{0} [{1}]", this.referenceDateTimeString, this.referenceDateTimeKind);
		}

		protected override void EnsureParsed()
		{

			if (this.parsed) return;

			this.dateTimeStringUTC = String.Empty;
			this.dateTimeStringLocal = String.Empty;

			if (!String.IsNullOrEmpty(this.referenceDateTimeString))
			{

				this.referenceDateTime = TimeUtils.Parse(this.referenceDateTimeString);

				if (this.referenceDateTimeKind == DateTimeKind.Utc)
				{

					this.dateTimeUTC = this.referenceDateTime;
					this.dateTimeStringUTC = this.referenceDateTimeString;
					this.dateTimeLocal = this.referenceDateTime.Value.ToLocalTime();
					this.dateTimeStringLocal = TimeUtils.DateTimeString(this.dateTimeLocal.Value);

				}
				else if (this.referenceDateTimeKind == DateTimeKind.Local)
				{

					this.dateTimeUTC = this.referenceDateTime.Value.ToUniversalTime();
					this.dateTimeStringUTC = TimeUtils.DateTimeString(this.dateTimeUTC.Value);
					this.dateTimeLocal = this.referenceDateTime;
					this.dateTimeStringLocal = this.referenceDateTimeString;

				}
				else
				{
					throw new ArgumentException("Unsupported DateTimeKind: " + this.referenceDateTimeKind);
				}
			}

			this.parsed = true;
		}
	}

}
