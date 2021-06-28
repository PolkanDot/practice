using System;
using System.Collections.Generic;
using System.Globalization;
using Lesson.Libs.Common.Types;

namespace LessonDebugExceptions
{
    public class DateRangePackage
    {
        public List<DateRange> DateRanges { get; set; }

        public DateRangePackage( List<DateRange> dateRanges )
        {
        }

        public override string ToString()
        {
            return "["
                + String.Join(
                    ", ",
                    DateRanges.ConvertAll(
                        dateRange => $"[{dateRange.StartDate.ToString( "yyyy-MM-dd", CultureInfo.InvariantCulture )},"
                            + $" {dateRange.EndDate.ToString( "yyyy-MM-dd", CultureInfo.InvariantCulture )}]" ) )
                + "]";
        }
    }
}
