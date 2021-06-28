using System.Collections.Generic;

namespace Lesson.Libs.Common.Types.Extensions.Comparers
{
    public class DateRangeComparer : IComparer<DateRange>
    {
        public static readonly DateRangeComparer Instance = new DateRangeComparer();

        public int Compare( DateRange x, DateRange y )
        {
            int startDateTimeCompareResult = x.StartDateTime.CompareTo( y.StartDateTime );

            if ( startDateTimeCompareResult != 0 )
            {
                return startDateTimeCompareResult;
            }

            int endDateTimeCompareResult = x.EndDateTime.CompareTo( y.EndDateTime );
            if ( endDateTimeCompareResult != 0 )
            {
                return endDateTimeCompareResult;
            }

            return 0;
        }
    }
}
