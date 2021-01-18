using System;
using System.Collections.Generic;

namespace Game.Utility
{

	// System.DayOfWeek is useless.

	public static class Day {

		[Flags]
		public enum Mask {

			None		= 0x00,

			Monday		= 0x01,
			Tuesday		= 0x02,
			Wednesday	= 0x04,
			Thursday	= 0x08,
			Friday		= 0x10,
			Saturday	= 0x20,
			Sunday		= 0x40,

			Weekdays	= Monday | Tuesday | Wednesday | Thursday | Friday,
			Weekend		= Saturday | Sunday,
			All			= Weekdays | Weekend,
		}

		private static Mask[] days;
		private static Dictionary<DayOfWeek, Mask> fromDOW;
		private static Dictionary<Mask, DayOfWeek> toDOW;

		static Day() {

			days = new Mask[] {
				Mask.Monday, 
				Mask.Tuesday, 
				Mask.Wednesday, 
				Mask.Thursday, 
				Mask.Friday, 
				Mask.Saturday, 
				Mask.Sunday
			};

			fromDOW = new Dictionary<DayOfWeek, Mask>();
			fromDOW[DayOfWeek.Monday] = Mask.Monday;
			fromDOW[DayOfWeek.Tuesday] = Mask.Tuesday;
			fromDOW[DayOfWeek.Wednesday] = Mask.Wednesday;
			fromDOW[DayOfWeek.Thursday] = Mask.Thursday;
			fromDOW[DayOfWeek.Friday] = Mask.Friday;
			fromDOW[DayOfWeek.Saturday] = Mask.Saturday;
			fromDOW[DayOfWeek.Sunday] = Mask.Sunday;

			toDOW = new Dictionary<Mask, DayOfWeek>();
			toDOW[Mask.Monday] = DayOfWeek.Monday;
			toDOW[Mask.Tuesday] = DayOfWeek.Tuesday;
			toDOW[Mask.Wednesday] = DayOfWeek.Wednesday;
			toDOW[Mask.Thursday] = DayOfWeek.Thursday;
			toDOW[Mask.Friday] = DayOfWeek.Friday;
			toDOW[Mask.Saturday] = DayOfWeek.Saturday;
			toDOW[Mask.Sunday] = DayOfWeek.Sunday;
		}

		public static IEnumerable<Mask> Days {
			get { return days; }
		}

		public static DayOfWeek ToDayOfWeek(Mask dayMask) {

			DayOfWeek result;
			if (toDOW.TryGetValue(dayMask, out result)) {
				return result;
			} else {
				throw new ArgumentException("No DayOfWeek for Day.Mask: " + dayMask);
			}
		}

		public static Mask FromDayOfWeek(DayOfWeek dayOfWeek) {
			return fromDOW[dayOfWeek];
		}

		public static bool IncludesDayOfWeek(Mask dayMask, DayOfWeek dayOfWeek) {
			Mask testMask = fromDOW[dayOfWeek];
			return ((dayMask & testMask) == testMask);
		}

		public static IList<DateTime> EnumerateDays(DateTime startDate, DateTime endDate, Day.Mask daysOfInterest) {
			return EnumerateDays(startDate, endDate, daysOfInterest, false);
		}

		// "startDate" can be before or after "endDate": the order of the resulting dates is dictated by the "descending" parameter.
		public static IList<DateTime> EnumerateDays(DateTime startDate, DateTime endDate, Day.Mask daysOfInterest, bool descending) {

			if (startDate > endDate) {
				DateTime tmp = startDate;
				startDate = endDate;
				endDate = tmp;
			}

			List<DateTime> dates = new List<DateTime>();
			DateTime date = startDate;
			while (date <= endDate) {

				if (Day.IncludesDayOfWeek(daysOfInterest, date.DayOfWeek)) {
					dates.Add(date);
				}

				date = date.AddDays(1);
			}

			// "dates" is now in ascending sorted order
			if (descending) {
				dates.Reverse();
			}

			return dates;
		}

		// "startDate" can be before or after "endDate": the sign of the result reflects this (negative if startDate is after endDate)
		public static int CountDays(DateTime startDate, DateTime endDate, Day.Mask daysOfInterest) {

			bool negate = false;
			if (startDate > endDate) {
				negate = true;
				DateTime tmp = startDate;
				startDate = endDate;
				endDate = tmp;
			}

			int count = 0;
			DateTime date = startDate;
			while (date < endDate) {

				if (Day.IncludesDayOfWeek(daysOfInterest, date.DayOfWeek)) {
					count++;
				}

				date = date.AddDays(1);
			}

			if (negate) count *= -1;

			return count;
		}

		public static IList<DateTime> EnumerateMonths(DateTime startDate, DateTime endDate) {
			return EnumerateMonths(startDate, endDate, false);
		}

		// Returns each of the firsts of the month for all dates in [startDate..endDate]
		public static IList<DateTime> EnumerateMonths(DateTime startDate, DateTime endDate, bool descending) {

			DateTime startMonth = new DateTime(startDate.Year, startDate.Month, 1);
			DateTime endMonth = new DateTime(endDate.Year, endDate.Month, 1);

			if (startMonth > endMonth) {
				DateTime tmp = startMonth;
				startMonth = endMonth;
				endMonth = tmp;
			}

			List<DateTime> dates = new List<DateTime>();
			DateTime date = startMonth;
			while (date <= endMonth) {
				dates.Add(date);
				date = date.AddMonths(1);
			}

			if (descending) {
				dates.Reverse();
			}

			return dates.AsReadOnly();
		}

		public static string ToStringForTracking(this DayOfWeek dow) {
			return dow.ToString().ToLower();
		}
	}
}
