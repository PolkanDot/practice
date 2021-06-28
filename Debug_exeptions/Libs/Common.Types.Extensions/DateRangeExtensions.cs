using System;
using System.Collections.Generic;
using System.Linq;
using Lesson.Libs.Common.Types.Extensions.Comparers;

namespace Lesson.Libs.Common.Types.Extensions
{
    public static class DateRangeExtensions
    {
        public static IComparer<DateRange> Comparer { get; } = new DateRangeComparer();
        internal static readonly DateRangeEqualityComparer DateRangeEqualityComparer = DateRangeEqualityComparer.Instance;
        internal static readonly DateRangeDatesEqualityComparer DateRangeDatesEqualityComparer = DateRangeDatesEqualityComparer.Instance;

        public static IEnumerable<DateTime> ToEnumerable( this DateRange dateRange, bool shouldIncludeLast )
        {
            for ( DateTime date = dateRange.StartDate; date < dateRange.EndDate || ( shouldIncludeLast && date == dateRange.EndDate ); date = date.AddDays( 1 ) )
            {
                yield return date;
            }
        }

        public static SortedSet<DateTime> ToSortedSet( this DateRange dateRange, bool includeLast )
        {
            return new SortedSet<DateTime>( ToEnumerable( dateRange, includeLast ) );
        }

        public static List<DateRange> SubtractDateRanges( this DateRange x, DateRange y )
        {
            // есть пересечение диапазонов
            if ( y.Intersects( x, false ) )
            {
                var resultDateRanges = new List<DateRange>();
                if ( x.StartDateTime < y.StartDateTime )
                {
                    resultDateRanges.Add( new DateRange( x.StartDateTime, y.StartDateTime ) );
                }

                if ( x.EndDateTime > y.EndDateTime )
                {
                    resultDateRanges.Add( new DateRange( y.EndDateTime, x.EndDateTime ) );
                }

                return resultDateRanges;
            }

            return new List<DateRange>
            {
                new DateRange( x.StartDateTime, x.EndDateTime )
            };
        }

        public static List<DateRange> SubtractDateRanges( this DateRange dateRange, List<DateRange> excludingDateRanges )
        {
            var resultDateRanges = new List<DateRange>
            {
                dateRange
            };

            for ( int i = 0; i < resultDateRanges.Count; )
            {
                DateRange currentDateRange = resultDateRanges[ i ];

                foreach ( DateRange excludingDateRange in excludingDateRanges )
                {
                    List<DateRange> diffDateRanges = currentDateRange.SubtractDateRanges( excludingDateRange );
                    if ( diffDateRanges.Count != 1
                        || !diffDateRanges.Contains( currentDateRange, DateRangeEqualityComparer ) )
                    {
                        resultDateRanges.RemoveAt( i );
                        resultDateRanges.AddRange( diffDateRanges );
                        break;
                    }
                }

                if ( resultDateRanges.Contains( currentDateRange, DateRangeEqualityComparer ) )
                {
                    i++;
                }
            }

            resultDateRanges.Sort( Comparer);

            return resultDateRanges;
        }

        public static List<DateRange> SubtractDates( this DateRange firstDateRange, DateRange secondDateRange, bool shouldExcludeBounds )
        {
            DateRange firstMidnightDateRange = firstDateRange.AsMidnightDateRange();
            DateRange secondMidnightDateRange = secondDateRange.AsMidnightDateRange();

            if ( secondMidnightDateRange.Intersects( firstMidnightDateRange, shouldExcludeBounds ) )
            {
                var resultDateRanges = new List<DateRange>();

                // подневная тарификация
                if ( shouldExcludeBounds )
                {
                    if ( firstMidnightDateRange.StartDate < secondMidnightDateRange.StartDate )
                    {
                        resultDateRanges.Add( new DateRange( firstMidnightDateRange.StartDate, secondMidnightDateRange.StartDate.AddDays( -1 ) ) );
                    }

                    if ( firstMidnightDateRange.EndDate > secondMidnightDateRange.EndDate )
                    {
                        resultDateRanges.Add( new DateRange( secondMidnightDateRange.EndDate.AddDays( 1 ), firstMidnightDateRange.EndDate ) );
                    }

                    return resultDateRanges;
                }

                if ( firstMidnightDateRange.StartDate < secondMidnightDateRange.StartDate )
                {
                    resultDateRanges.Add( new DateRange( firstMidnightDateRange.StartDate, secondMidnightDateRange.StartDate ) );
                }

                if ( firstMidnightDateRange.EndDate > secondMidnightDateRange.EndDate )
                {
                    resultDateRanges.Add( new DateRange( secondMidnightDateRange.EndDate, firstMidnightDateRange.EndDate ) );
                }

                return resultDateRanges;
            }

            return new List<DateRange> { new DateRange( firstMidnightDateRange.StartDate, firstMidnightDateRange.EndDate ) };
        }

        public static int CompareTo( this DateRange x, DateRange y )
        {
            return Comparer.Compare( x, y );
        }

        public static bool EqualTo( this DateRange x, DateRange y )
        {
            return DateRangeEqualityComparer.Equals( x, y );
        }

        public static bool EqualDatesTo( this DateRange x, DateRange y )
        {
            return DateRangeDatesEqualityComparer.Equals( x, y );
        }

        public static bool Contains( this DateRange dateRange, DateTime dateTime )
        {
            return dateRange.StartDateTime <= dateTime && dateTime <= dateRange.EndDateTime;
        }

        public static bool Contains( this DateRange x, DateRange y )
        {
            return x.Contains( y.StartDateTime ) && x.Contains( y.EndDateTime );
        }

        public static bool Intersects( this DateRange x, DateRange y, bool shouldIncludeEndings )
        {
            if ( shouldIncludeEndings )
            {
                return y.StartDateTime <= x.EndDateTime && y.EndDateTime >= x.StartDateTime;
            }

            // есть пересечение диапазонов
            return y.StartDateTime < x.EndDateTime && y.EndDateTime > x.StartDateTime;
        }

        public static SortedSet<DateRange> MergeDatesToDateRanges( this IEnumerable<DateTime> dates )
        {
            return MergeDatesToDateRangesWithSpecifiedDatesGap( dates, 1 );
        }

        /// <summary>
        /// Соединяет даты в диапазоны с указанным
        /// максимальным разрывом между датами
        /// </summary>
        /// <param name="dates">Список дат</param>
        /// <param name="gapBetweenDates">Максимальный промежуток между датами</param>
        /// Например для дат 2018-10-20 и 2018-10-22 gap будет равен 2
        /// <returns>Отсортированный сгрупиованный список DateRanges</returns>
        public static SortedSet<DateRange> MergeDatesToDateRangesWithSpecifiedDatesGap( this IEnumerable<DateTime> dates, int gapBetweenDates )
        {
            if ( !dates.Any() )
            {
                return new SortedSet<DateRange>();
            }

            if ( !( dates is SortedSet<DateTime> sortedDates ) )
            {
                sortedDates = new SortedSet<DateTime>( dates );
            }

            var dateRanges = new SortedSet<DateRange>( Comparer);

            DateTime startDate = sortedDates.First();
            DateTime endDate = startDate;
            DateRange dateRange;
            var gap = new TimeSpan( gapBetweenDates, 0, 0, 0 );

            foreach ( DateTime currentDate in sortedDates )
            {
                if ( currentDate.Date > endDate.Date.Add( gap ) )
                {
                    dateRange = new DateRange( startDate, endDate );
                    dateRanges.Add( dateRange );

                    startDate = currentDate;
                    endDate = currentDate;
                    continue;
                }

                endDate = currentDate;
            }

            dateRange = new DateRange( startDate, endDate );
            dateRanges.Add( dateRange );

            return dateRanges;
        }

        /// <summary>
        /// Удаляет вложенные диапазоны.
        /// Склеивает пограничные и пересекающиеся диапазоны.
        /// </summary>
        public static SortedSet<DateRange> JoinDateRanges( this IEnumerable<DateRange> dateRanges )
        {
            var sortedDateRanges = new SortedSet<DateRange>( dateRanges, Comparer);
            for ( int i = 0; i < sortedDateRanges.Count - 1; )
            {
                DateRange currentDateRange = sortedDateRanges.ElementAt( i );
                DateRange nextDateRange = sortedDateRanges.ElementAt( i + 1 );

                if ( nextDateRange.StartDateTime <= currentDateRange.EndDateTime )
                {
                    DateRange mergedDateRange = new DateRange(
                        currentDateRange.StartDateTime,
                        new DateTime( Math.Max( currentDateRange.EndDateTime.Ticks, nextDateRange.EndDateTime.Ticks ) ) );
                    sortedDateRanges.Remove( currentDateRange );
                    sortedDateRanges.Remove( nextDateRange );
                    sortedDateRanges.Add( mergedDateRange );
                }
                else
                {
                    i++;
                }
            }

            return sortedDateRanges;
        }

        public static DateRange? Intersect( this DateRange x, DateRange y, bool shouldIncludeEndings )
        {
            if ( y.Intersects( x, shouldIncludeEndings ) )
            {
                DateTime startDateTime = x.StartDateTime > y.StartDateTime
                    ? x.StartDateTime
                    : y.StartDateTime;
                DateTime endDateTime = x.EndDateTime < y.EndDateTime
                    ? x.EndDateTime
                    : y.EndDateTime;

                return new DateRange( startDateTime, endDateTime );
            }

            return null;
        }

        public static DateTime As( this DateTime original, DateTimeKind kind )
        {
            if ( original.Kind == kind )
            {
                return original;
            }

            switch ( kind )
            {
                case DateTimeKind.Utc:
                    return original.Kind == DateTimeKind.Local ? original.ToUniversalTime() : new DateTime( original.Ticks, kind );
                case DateTimeKind.Local:
                    return original.Kind == DateTimeKind.Utc ? original.ToLocalTime() : new DateTime( original.Ticks, kind );
                default:
                    return new DateTime( original.Ticks, kind );
            }
        }

        public static IEnumerable<DateRange> Split( this DateRange dateRange, int chunkSize )
        {
            if ( chunkSize <= 0 )
            {
                yield break;
            }

            DateTime chunkEnd;
            DateTime start = dateRange.StartDate;
            while ( ( chunkEnd = start.AddDays( chunkSize - 1 ) ) < dateRange.EndDate )
            {
                yield return new DateRange( start, chunkEnd );
                start = chunkEnd.AddDays( 1 );
            }

            yield return new DateRange( start, dateRange.EndDate );
        }

        public static DateRange AsMidnightDateRange( this DateRange dateRange )
        {
            return new DateRange( dateRange.StartDate, dateRange.EndDate );
        }
    }
}
