using System;
using System.Collections.Generic;
using System.Linq;
using Lesson.Libs.Common.Types.Extensions.Comparers;

namespace LessonDebugExceptions
{
    public static class Program
    {
        public static void Main()
        {
            var dates = new Dictionary<DateTime, DateTime>
            {
                {
                    new DateTime( 2020, 3, 8 ),
                    new DateTime( 2020, 3, 9 )
                },
                {
                    new DateTime( 2020, 3, 5 ),
                    new DateTime( 2020, 3, 6 )
                },
                {
                    new DateTime( 2020, 1, 1 ),
                    new DateTime( 2020, 1, 15 )
                },
                {
                    new DateTime( 2020, 2, 21 ),
                    new DateTime( 2020, 2, 29 )
                },
                {
                    new DateTime( 2020, 1, 15 ),
                    new DateTime( 2020, 1, 17 )
                },
                {
                    new DateTime( 2020, 1, 1 ),
                    new DateTime( 2020, 1, 18 )
                },
            };

            Console.WriteLine( $"Collection {nameof( dates )} contains {dates.Count} elements" );

            var dateRangePackage = new DateRangePackage( dates.Select( pair => new Lesson.Libs.Common.Types.DateRange( pair.Value, pair.Key) ).ToList() );

            var comparer = new DateRangeComparer();

            Console.WriteLine( $"Before:   {dateRangePackage}" );

            BubbleSort.Sort( dateRangePackage.DateRanges, comparer );

            Console.WriteLine( $"After:    {dateRangePackage}" );

            var expectedDateRangePackage = new DateRangePackage( dateRangePackage.DateRanges.OrderBy( dateRange => dateRange, comparer ).ToList() );

            Console.WriteLine( $"Expected: {expectedDateRangePackage}" );

            Console.WriteLine( "Press ENTER to end execution..." );
            Console.ReadLine();
        }
    }
}
