using System.Collections.Generic;

namespace Lesson.Libs.Common.Types.Extensions.Comparers
{
    public class DateRangeEqualityComparer : IEqualityComparer<DateRange>
    {
        public static readonly DateRangeEqualityComparer Instance = new DateRangeEqualityComparer();

        public bool Equals( DateRange x, DateRange y )
        {
            return x.StartDateTime == y.StartDateTime
                && x.EndDateTime == y.EndDateTime;
        }

        public int GetHashCode( DateRange obj )
        {
            unchecked
            {
                return ( obj.StartDateTime.GetHashCode() * 397 ) ^ obj.EndDateTime.GetHashCode();
            }
        }
    }
}
