using System.Collections.Generic;

namespace Lesson.Libs.Common.Types.Extensions.Comparers
{
    public class DateRangeDatesEqualityComparer : IEqualityComparer<DateRange>
    {
        public static readonly DateRangeDatesEqualityComparer Instance = new DateRangeDatesEqualityComparer();

        public bool Equals( DateRange x, DateRange y )
        {
            return x.StartDate == y.StartDate
                && x.EndDate == y.EndDate;
        }

        public int GetHashCode( DateRange obj )
        {
            unchecked
            {
                return ( obj.StartDate.GetHashCode() * 397 ) ^ obj.EndDate.GetHashCode();
            }
        }
    }
}
