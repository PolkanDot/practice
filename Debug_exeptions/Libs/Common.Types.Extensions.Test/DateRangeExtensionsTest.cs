using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lesson.Libs.Common.Types.Extensions.Test
{
    [TestClass]
    public class DateRangeExtensionsTest
    {
        [TestMethod]
        public void DateRangeExtensions_ToSortedSet_TwoDaysIncludeLast_BothDatesIncluded()
        {
            var dateRange = new DateRange( DateTime.Today, DateTime.Today.AddDays( 1 ) );

            SortedSet<DateTime> dates = dateRange.ToSortedSet( true );

            Assert.AreEqual( 2, dates.Count );
            CollectionAssert.Contains( dates, dateRange.StartDate );
            CollectionAssert.Contains( dates, dateRange.EndDate );
        }

        [TestMethod]
        public void DateRangeExtensions_ToSortedSet_TwoDaysNotIncludeLast_FirstDateIncluded()
        {
            var dateRange = new DateRange( DateTime.Today, DateTime.Today.AddDays( 1 ) );

            SortedSet<DateTime> dates = dateRange.ToSortedSet( false );

            Assert.AreEqual( 1, dates.Count );
            CollectionAssert.Contains( dates, dateRange.StartDate );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDateRanges_NoIntersectionAtLastDay_ResultOriginalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 18, 12, 0, 0 ), new DateTime( 2016, 6, 19, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDateRanges( dateRange2 );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDateTime, dateRange1.EndDateTime ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDateRanges_FourDaysSubtractTwoDaysInBetween_DiscreteSubtractTwoAdditionalRangesWithTwoDays()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 16, 12, 0, 0 ), new DateTime( 2016, 6, 17, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDateRanges( dateRange2 );

            Assert.AreEqual( 2, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDateTime, dateRange2.StartDateTime ) ) );
            Assert.IsTrue( additionalDateRanges.Last().EqualTo( new DateRange( dateRange2.EndDateTime, dateRange1.EndDateTime ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDateRanges_TwoDateRangesWithFirstAndLastDatesEqualButDifferentTime_DiscreteSubtractTwoAdditionalRangesWithOneDay()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 18, 0, 0 ), new DateTime( 2016, 6, 18, 9, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDateRanges( dateRange2 );

            Assert.AreEqual( 2, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDateTime, dateRange2.StartDateTime ) ) );
            Assert.IsTrue( additionalDateRanges.Last().EqualTo( new DateRange( dateRange2.EndDateTime, dateRange1.EndDateTime ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDateRanges_SecondDateRangeStartsBeforeFirst_DiscreteSubtractOneAdditionalRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 9, 0, 0 ), new DateTime( 2016, 6, 18, 9, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDateRanges( dateRange2 );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges.Last().EqualTo( new DateRange( dateRange2.EndDateTime, dateRange1.EndDateTime ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDateRanges_SecondDateRangeEndsAfterFirst_DiscreteSubtractOneAdditionalRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 18, 0, 0 ), new DateTime( 2016, 6, 18, 18, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDateRanges( dateRange2 );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDateTime, dateRange2.StartDateTime ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDateRanges_SecondDateRangeIncludesFirst_NoneAdditionalRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 18, 0, 0 ), new DateTime( 2016, 6, 18, 9, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDateRanges( dateRange2 );

            Assert.AreEqual( 0, additionalDateRanges.Count );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_NightStayUnitKindFourDaysSubtractTwoDaysInBetween_TwoAdditionalDateRangesIncludingIntersectedDates()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 16, 12, 0, 0 ), new DateTime( 2016, 6, 17, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, false );

            Assert.AreEqual( 2, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate, dateRange2.StartDate ) ) );
            Assert.IsTrue( additionalDateRanges.Last().EqualTo( new DateRange( dateRange2.EndDate, dateRange1.EndDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_NightStayUnitKindFirstDateRangeStartsAtStartDateOfSecond_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 14, 0, 0 ), new DateTime( 2016, 6, 17, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, false );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange2.EndDate, dateRange1.EndDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_NightStayUnitKindFirstDateRangeEndsAtEndDateOfSecond_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 16, 14, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, false );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate, dateRange2.StartDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_NightStayUnitKindFirstDateRangeStartsAtEndDateOfSecond_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 12, 14, 0, 0 ), new DateTime( 2016, 6, 15, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, false );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate, dateRange1.EndDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_NightStayUnitKindFirstDateRangeEndsAtStartDateOfSecond_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 13, 14, 0, 0 ), new DateTime( 2016, 6, 19, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, false );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate, dateRange1.EndDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_NightStayUnitKindDateRangesHaveNoIntersection_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 20, 14, 0, 0 ), new DateTime( 2016, 6, 21, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, false );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate, dateRange1.EndDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_DayStayUnitKindFourDaysSubtractTwoDaysInBetween_TwoAdditionalDateRangesExcludingIntersectedDates()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 16, 12, 0, 0 ), new DateTime( 2016, 6, 17, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, true );

            Assert.AreEqual( 2, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate, dateRange2.StartDate.AddDays( -1 ) ) ) );
            Assert.IsTrue( additionalDateRanges.Last().EqualTo( new DateRange( dateRange2.EndDate.AddDays( 1 ), dateRange1.EndDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_DayStayUnitKindFirstDateRangeStartsAtStartDateOfSecond_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 14, 0, 0 ), new DateTime( 2016, 6, 17, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, true );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange2.EndDate.AddDays( 1 ), dateRange1.EndDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_DayStayUnitKindFirstDateRangeEndsAtEndDateOfSecond_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 16, 14, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, true );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate, dateRange2.StartDate.AddDays( -1 ) ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_DayStayUnitKindFirstDateRangeStartsAtEndDateOfSecond_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 12, 14, 0, 0 ), new DateTime( 2016, 6, 15, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, true );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate.AddDays( 1 ), dateRange1.EndDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_DayStayUnitKindFirstDateRangeEndsAtStartDateOfSecond_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 18, 14, 0, 0 ), new DateTime( 2016, 6, 19, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, true );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate, dateRange1.EndDate.AddDays( -1 ) ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_SubtractDates_DayStayUnitKindDateRangesHaveNoIntersection_OneAdditionalDateRange()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 16, 14, 0, 0 ), new DateTime( 2016, 6, 21, 12, 0, 0 ) );

            List<DateRange> additionalDateRanges = dateRange1.SubtractDates( dateRange2, true );

            Assert.AreEqual( 1, additionalDateRanges.Count );
            Assert.IsTrue( additionalDateRanges[ 0 ].EqualTo( new DateRange( dateRange1.StartDate, dateRange1.EndDate ) ) );
        }

        [TestMethod]
        public void DateRangeExtensions_CompareTo_FirstDateRangeStartDateBeforeSecond_NegativeCompareResult()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 16, 14, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            int compareResult = dateRange1.CompareTo( dateRange2 );

            Assert.AreEqual( -1, compareResult );
        }

        [TestMethod]
        public void DateRangeExtensions_CompareTo_FirstDateRangeStartDateAfterSecond_PositiveCompareResult()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 16, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 14, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            int compareResult = dateRange1.CompareTo( dateRange2 );

            Assert.AreEqual( 1, compareResult );
        }

        [TestMethod]
        public void DateRangeExtensions_CompareTo_FirstDateRangeEndDateAfterSecond_PositiveCompareResult()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 19, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            int compareResult = dateRange1.CompareTo( dateRange2 );

            Assert.AreEqual( 1, compareResult );
        }

        [TestMethod]
        public void DateRangeExtensions_CompareTo_FirstDateRangeEndDateBeforeSecond_NegativeCompareResult()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 17, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            int compareResult = dateRange1.CompareTo( dateRange2 );

            Assert.AreEqual( -1, compareResult );
        }

        [TestMethod]
        public void DateRangeExtensions_CompareTo_DateRangesAreEqual_ZeroCompareResult()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            int compareResult = dateRange1.CompareTo( dateRange2 );

            Assert.AreEqual( 0, compareResult );
        }

        [TestMethod]
        public void DateRangeExtensions_EqualTo_DateRangesAreEqual_ReturnTrue()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 13, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            bool equalityResult = dateRange1.EqualTo( dateRange2 );

            Assert.IsTrue( equalityResult );
        }

        [TestMethod]
        public void DateRangeExtensions_EqualTo_DifferentDateRanges_ReturnFalse()
        {
            var dateRange1 = new DateRange( new DateTime( 2016, 6, 15, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2016, 6, 16, 12, 0, 0 ), new DateTime( 2016, 6, 18, 12, 0, 0 ) );

            bool equalityResult = dateRange1.EqualTo( dateRange2 );

            Assert.IsFalse( equalityResult );
        }

        [TestMethod]
        public void DateRangeExtensions_Contains_DateInDateRange_ReturnsTrue()
        {
            var dateRange = new DateRange( new DateTime( 2017, 1, 1 ), new DateTime( 2017, 2, 1 ) );
            var dateTime = new DateTime( 2017, 1, 15 );

            bool result = dateRange.Contains( dateTime );

            Assert.IsTrue( result );
        }

        [TestMethod]
        public void DateRangeExtensions_Contains_DateNotInDateRange_ReturnsFalse()
        {
            var dateRange = new DateRange( new DateTime( 2017, 1, 1 ), new DateTime( 2017, 2, 1 ) );
            var dateTime = new DateTime( 1990, 1, 15 );

            bool result = dateRange.Contains( dateTime );

            Assert.IsFalse( result );
        }

        [TestMethod]
        public void DateRangeExtensions_MergeDatesToDateRanges_DatesDifferenceMoreOneDayButItsNextDays_ReturnsSingleDateRange()
        {
            // Arrange
            var dates = new List<DateTime>
            {
                new DateTime( 2017, 9, 11, 1, 0, 0 ),
                new DateTime( 2017, 9, 12, 12, 0, 0 ),
                new DateTime( 2017, 9, 12, 13, 0, 0 ),
            };

            // Act
            SortedSet<DateRange> dateRanges = dates.MergeDatesToDateRanges( );

            var expectedDateRanges = new SortedSet<DateRange>
            {
                new DateRange( new DateTime( 2017, 9, 11, 1, 0, 0 ), new DateTime( 2017, 9, 12, 13, 0, 0 ) ),
            };

            // Assert
            Assert.AreEqual( expectedDateRanges.Count, dateRanges.Count );
            foreach ( DateRange expectedDateRange in expectedDateRanges )
            {
                Assert.IsTrue( dateRanges.Contains( expectedDateRange ) );
            }
        }

        [TestMethod]
        public void DateRangeExtensions_MergeDatesToDateRanges_NotSortedDays_ReturnsCorrectDateRanges()
        {
            // Arrange
            var dates = new List<DateTime>
            {
                new DateTime( 2017, 9, 12, 12, 0, 0 ),
                new DateTime( 2017, 9, 11, 1, 0, 0 ),
                new DateTime( 2017, 9, 12, 13, 0, 0 ),
                new DateTime( 2017, 9, 15, 1, 0, 0 ),
                new DateTime( 2017, 9, 14, 1, 0, 0 ),
            };

            // Act
            SortedSet<DateRange> dateRanges = dates.MergeDatesToDateRanges( );

            var expectedDateRanges = new SortedSet<DateRange>( DateRangeExtensions.Comparer)
            {
                new DateRange( new DateTime( 2017, 9, 11, 1, 0, 0 ), new DateTime( 2017, 9, 12, 13, 0, 0 ) ),
                new DateRange( new DateTime( 2017, 9, 14, 1, 0, 0 ), new DateTime( 2017, 9, 15, 1, 0, 0 ) ),
            };

            // Assert
            Assert.AreEqual( expectedDateRanges.Count, dateRanges.Count );
            foreach ( DateRange expectedDateRange in expectedDateRanges )
            {
                Assert.IsTrue( dateRanges.Contains( expectedDateRange ) );
            }
        }

        [TestMethod]
        public void DateRangeExtensions_MergeDatesToDateRanges_FarLastDate_ExtraLastDateRangeWithLastDate()
        {
            // Arrange
            var dates = new List<DateTime>
            {
                new DateTime( 2017, 9, 12, 12, 0, 0 ),
                new DateTime( 2017, 9, 11, 1, 0, 0 ),
                new DateTime( 2017, 9, 12, 13, 0, 0 ),
                new DateTime( 2017, 9, 15, 1, 0, 0 ),
                new DateTime( 2017, 9, 14, 1, 0, 0 ),
                new DateTime( 2017, 9, 17, 1, 0, 0 ),
            };

            // Act
            SortedSet<DateRange> dateRanges = dates.MergeDatesToDateRanges( );

            var expectedDateRanges = new SortedSet<DateRange>( DateRangeExtensions.Comparer)
            {
                new DateRange( new DateTime( 2017, 9, 11, 1, 0, 0 ), new DateTime( 2017, 9, 12, 13, 0, 0 ) ),
                new DateRange( new DateTime( 2017, 9, 14, 1, 0, 0 ), new DateTime( 2017, 9, 15, 1, 0, 0 ) ),
                new DateRange( new DateTime( 2017, 9, 17, 1, 0, 0 ), new DateTime( 2017, 9, 17, 1, 0, 0 ) ),
            };

            // Assert
            Assert.AreEqual( expectedDateRanges.Count, dateRanges.Count );
            foreach ( DateRange expectedDateRange in expectedDateRanges )
            {
                Assert.IsTrue( dateRanges.Contains( expectedDateRange ) );
            }
        }

        [TestMethod]
        public void DateRangeExtensions_MergeDatesToDateRanges_OneDate_OneDateRange()
        {
            // Arrange
            var dates = new List<DateTime>
            {
                new DateTime( 2017, 9, 11, 1, 0, 0 ),
            };

            // Act
            SortedSet<DateRange> dateRanges = dates.MergeDatesToDateRanges( );

            var expectedDateRanges = new SortedSet<DateRange>( DateRangeExtensions.Comparer)
            {
                new DateRange( new DateTime( 2017, 9, 11, 1, 0, 0 ), new DateTime( 2017, 9, 11, 1, 0, 0 ) ),
            };

            // Assert
            Assert.AreEqual( expectedDateRanges.Count, dateRanges.Count );
            foreach ( DateRange expectedDateRange in expectedDateRanges )
            {
                Assert.IsTrue( dateRanges.Contains( expectedDateRange ) );
            }
        }

        [TestMethod]
        public void DateRangeExtensions_JoinDateRanges_ZeroDateRanges_ReturnsZeroDateRanges()
        {
            // Arrange
            IEnumerable<DateRange> dateRanges = new List<DateRange>();

            // Act
            SortedSet<DateRange> result = dateRanges.JoinDateRanges();

            // Assert
            Assert.AreEqual( 0, result.Count );
        }

        [TestMethod]
        public void DateRangeExtensions_JoinDateRanges_OneDateRange_ReturnsOneDateRange()
        {
            // Arrange
            IEnumerable<DateRange> dateRanges = new List<DateRange>
            {
                new DateRange( new DateTime( 2019, 9, 11, 1, 0, 0 ), DateTime.MaxValue )
            };

            // Act
            SortedSet<DateRange> result = dateRanges.JoinDateRanges();

            // Assert
            Assert.AreEqual( 1, result.Count );
            Assert.AreEqual( new DateTime( 2019, 9, 11, 1, 0, 0 ), result.ElementAt( 0 ).StartDateTime );
            Assert.AreEqual( DateTime.MaxValue, result.ElementAt( 0 ).EndDateTime );
        }

        [TestMethod]
        public void DateRangeExtensions_JoinDateRanges_TwoDateRanges_ReturnsSortedDateRanges()
        {
            // Arrange
            IEnumerable<DateRange> dateRanges = new List<DateRange>
            {
                new DateRange( new DateTime( 2019, 9, 11, 1, 0, 0 ), DateTime.MaxValue ),
                new DateRange( new DateTime( 2017, 1, 11, 1, 1, 1 ), new DateTime( 2018, 1, 31, 1, 1, 59 ) )
            };

            // Act
            SortedSet<DateRange> result = dateRanges.JoinDateRanges();

            // Assert
            Assert.AreEqual( 2, result.Count );
            Assert.AreEqual( new DateTime( 2017, 1, 11, 1, 1, 1 ), result.ElementAt( 0 ).StartDateTime );
            Assert.AreEqual( new DateTime( 2018, 1, 31, 1, 1, 59 ), result.ElementAt( 0 ).EndDateTime );
            Assert.AreEqual( new DateTime( 2019, 9, 11, 1, 0, 0 ), result.ElementAt( 1 ).StartDateTime );
            Assert.AreEqual( DateTime.MaxValue, result.ElementAt( 1 ).EndDateTime );
        }

        [TestMethod]
        public void DateRangeExtensions_JoinDateRanges_TwoNotSortedBoundaryDateRanges_ReturnsOneJoinedDateRange()
        {
            // Arrange
            IEnumerable<DateRange> dateRanges = new List<DateRange>
            {
                new DateRange( new DateTime( 2019, 9, 11, 1, 0, 0 ), DateTime.MaxValue ),
                new DateRange( new DateTime( 2017, 1, 11, 1, 1, 1 ), new DateTime( 2019, 9, 11, 1, 0, 00 ) )
            };

            // Act
            SortedSet<DateRange> result = dateRanges.JoinDateRanges();

            // Assert
            Assert.AreEqual( 1, result.Count );
            Assert.AreEqual( new DateTime( 2017, 1, 11, 1, 1, 1 ), result.ElementAt( 0 ).StartDateTime );
            Assert.AreEqual( DateTime.MaxValue, result.ElementAt( 0 ).EndDateTime );
        }

        [TestMethod]
        public void DateRangeExtensions_JoinDateRanges_TwoNotSortedIntersectedDateRanges_ReturnsOneJoinedDateRange()
        {
            // Arrange
            IEnumerable<DateRange> dateRanges = new List<DateRange>
            {
                new DateRange( new DateTime( 2019, 9, 11, 1, 0, 0 ), new DateTime(2020, 1, 1) ),
                new DateRange( new DateTime( 2017, 1, 11, 1, 1, 1 ), new DateTime( 2019, 10, 11, 1, 0, 00 ) )
            };

            // Act
            SortedSet<DateRange> result = dateRanges.JoinDateRanges();

            // Assert
            Assert.AreEqual( 1, result.Count );
            Assert.AreEqual( new DateTime( 2017, 1, 11, 1, 1, 1 ), result.ElementAt( 0 ).StartDateTime );
            Assert.AreEqual( new DateTime( 2020, 1, 1 ), result.ElementAt( 0 ).EndDateTime );
        }

        [TestMethod]
        public void DateRangeExtensions_JoinDateRanges_SixNotSortedIntersectedAndBoundaryDateRanges_ReturnsJoinedDateRanges()
        {
            // Arrange
            IEnumerable<DateRange> dateRanges = new List<DateRange>
            {
                new DateRange( new DateTime( 2017, 1, 11, 1, 1, 1 ), new DateTime( 2018, 1, 1, 1, 0, 0 ) ),
                new DateRange( new DateTime( 2018, 2, 11, 1, 0, 0 ), new DateTime( 2018, 10, 1 ) ),
                new DateRange( new DateTime( 2018, 10, 1 ), new DateTime( 2019, 1, 1 ) ),
                new DateRange( new DateTime( 2019, 2, 1 ), new DateTime( 2019, 12, 1 ) ),
                new DateRange( new DateTime( 2019, 11, 1 ), new DateTime( 2020, 12, 1 ) ),
                new DateRange( new DateTime( 2022, 11, 1 ), DateTime.MaxValue ),
            };

            // Act
            SortedSet<DateRange> result = dateRanges.JoinDateRanges();

            // Assert
            Assert.AreEqual( 4, result.Count );
            Assert.AreEqual( new DateTime( 2017, 1, 11, 1, 1, 1 ), result.ElementAt( 0 ).StartDateTime );
            Assert.AreEqual( new DateTime( 2018, 1, 1, 1, 0, 0 ), result.ElementAt( 0 ).EndDateTime );
            Assert.AreEqual( new DateTime( 2018, 2, 11, 1, 0, 0 ), result.ElementAt( 1 ).StartDateTime );
            Assert.AreEqual( new DateTime( 2019, 1, 1 ), result.ElementAt( 1 ).EndDateTime );
            Assert.AreEqual( new DateTime( 2019, 2, 1 ), result.ElementAt( 2 ).StartDateTime );
            Assert.AreEqual( new DateTime( 2020, 12, 1 ), result.ElementAt( 2 ).EndDateTime );
            Assert.AreEqual( new DateTime( 2021, 11, 1 ), result.ElementAt( 3 ).StartDateTime );
            Assert.AreEqual( DateTime.MaxValue, result.ElementAt( 3 ).EndDateTime );
        }

        [TestMethod]
        public void DateRangeExtensions_JoinDateRanges_TwoNestedDateRanges_ReturnsWithoutOneDateRanges()
        {
            // Arrange
            IEnumerable<DateRange> dateRanges = new List<DateRange>
            {
                new DateRange( new DateTime( 2019, 3, 1 ), new DateTime( 2019, 4, 1 ) ),
                new DateRange( new DateTime( 2019, 5, 1 ), new DateTime( 2019, 5, 1 ) ),
            };

            // Act
            SortedSet<DateRange> result = dateRanges.JoinDateRanges();

            // Assert
            Assert.AreEqual( 1, result.Count );
            Assert.AreEqual( new DateTime( 2019, 2, 1 ), result.ElementAt( 0 ).StartDateTime );
            Assert.AreEqual( new DateTime( 2019, 5, 1 ), result.ElementAt( 0 ).EndDateTime );
        }

        [TestMethod]
        public void DateRangeExtensions_JoinDateRanges_TwoKindOfNestedButOneOfThemInfinityDateRanges_ReturnsJoinedDateRanges()
        {
            // Arrange
            IEnumerable<DateRange> dateRanges = new List<DateRange>
            {
                new DateRange( new DateTime( 2019, 3, 1 ), DateTime.MaxValue ),
                new DateRange( new DateTime( 2019, 2, 1 ), new DateTime( 2019, 5, 1 ) ),
            };

            // Act
            SortedSet<DateRange> result = dateRanges.JoinDateRanges();

            // Assert
            Assert.AreEqual( 1, result.Count );
            Assert.AreEqual( new DateTime( 2019, 2, 1 ), result.ElementAt( 0 ).StartDateTime );
            Assert.AreEqual( DateTime.MaxValue, result.ElementAt( 0 ).EndDateTime );
        }

        [TestMethod]
        public void DateRangeExtensions_JoinDateRanges_SixNestedAndBoundaryDateRanges_ReturnsJoinedDateRanges()
        {
            // Arrange
            IEnumerable<DateRange> dateRanges = new List<DateRange>
            {
                new DateRange( new DateTime( 2017, 1, 11, 1, 1, 1 ), new DateTime( 2018, 1, 1, 1, 0, 0 ) ),
                new DateRange( new DateTime( 2018, 2, 11, 1, 0, 0 ), new DateTime( 2018, 10, 1 ) ),
                new DateRange( new DateTime( 2018, 3, 1 ), new DateTime( 2018, 9, 1 ) ),
                new DateRange( new DateTime( 2019, 2, 1 ), new DateTime( 2019, 12, 1 ) ),
                new DateRange( new DateTime( 2019, 11, 1 ), new DateTime( 2020, 12, 1 ) ),
                new DateRange( new DateTime( 2021, 11, 1 ), DateTime.MaxValue ),
            };

            // Act
            SortedSet<DateRange> result = dateRanges.JoinDateRanges();

            // Assert
            Assert.AreEqual( 4, result.Count );
            Assert.AreEqual( new DateTime( 2017, 1, 11, 1, 1, 1 ), result.ElementAt( 0 ).StartDateTime );
            Assert.AreEqual( new DateTime( 2018, 1, 1, 1, 0, 0 ), result.ElementAt( 0 ).EndDateTime );
            Assert.AreEqual( new DateTime( 2018, 2, 11, 1, 0, 0 ), result.ElementAt( 1 ).StartDateTime );
            Assert.AreEqual( new DateTime( 2018, 10, 1 ), result.ElementAt( 1 ).EndDateTime );
            Assert.AreEqual( new DateTime( 2019, 2, 1 ), result.ElementAt( 2 ).StartDateTime );
            Assert.AreEqual( new DateTime( 2020, 12, 1 ), result.ElementAt( 2 ).EndDateTime );
            Assert.AreEqual( new DateTime( 2021, 11, 1 ), result.ElementAt( 3 ).StartDateTime );
            Assert.AreEqual( DateTime.MaxValue, result.ElementAt( 3 ).EndDateTime );
        }

        [TestMethod]
        public void DateRangeExtensions_MergeDatesToDateRangesWithSpecifiedDatesGap_ZeroGap_DateRangeForEachDate()
        {
            // Arrange
            var dates = new List<DateTime>
            {
                new DateTime( 2017, 9, 11, 1, 0, 0 ),
                new DateTime( 2017, 9, 12, 1, 0, 0 ),
                new DateTime( 2017, 9, 13, 1, 0, 0 )
            };

            // Act
            SortedSet<DateRange> dateRanges = dates.MergeDatesToDateRangesWithSpecifiedDatesGap( 0 );

            var expectedDateRanges = new SortedSet<DateRange>( DateRangeExtensions.Comparer)
            {
                new DateRange( new DateTime( 2017, 9, 11, 1, 0, 0 ), new DateTime( 2017, 9, 11, 1, 0, 0 ) ),
                new DateRange( new DateTime( 2017, 9, 12, 1, 0, 0 ), new DateTime( 2017, 9, 12, 1, 0, 0 ) ),
                new DateRange( new DateTime( 2017, 9, 13, 1, 0, 0 ), new DateTime( 2017, 9, 13, 1, 0, 0 ) ),
            };

            // Assert
            Assert.AreEqual( expectedDateRanges.Count, dateRanges.Count );
            foreach ( DateRange expectedDateRange in expectedDateRanges )
            {
                Assert.IsTrue( dateRanges.Contains( expectedDateRange ) );
            }
        }

        [TestMethod]
        public void DateRangeExtensions_MergeDatesToDateRangesWithSpecifiedDatesGap_GapEqualsOne_DateRangeForSubsequentedDates()
        {
            // Arrange
            var dates = new List<DateTime>
            {
                new DateTime( 2017, 9, 11, 1, 0, 0 ),
                new DateTime( 2017, 9, 12, 1, 0, 0 ),
                new DateTime( 2017, 9, 13, 1, 0, 0 ),
                new DateTime( 2017, 9, 16, 1, 0, 0 ),
                new DateTime( 2017, 9, 17, 1, 0, 0 )
            };

            // Act
            SortedSet<DateRange> dateRanges = dates.MergeDatesToDateRangesWithSpecifiedDatesGap( 1 );

            var expectedDateRanges = new SortedSet<DateRange>( DateRangeExtensions.Comparer)
            {
                new DateRange( new DateTime( 2017, 9, 11, 1, 0, 0 ), new DateTime( 2017, 9, 13, 1, 0, 0 ) ),
                new DateRange( new DateTime( 2017, 9, 16, 1, 0, 0 ), new DateTime( 2017, 9, 17, 1, 0, 0 ) )
            };

            // Assert
            Assert.AreEqual( expectedDateRanges.Count, dateRanges.Count );
            foreach ( DateRange expectedDateRange in expectedDateRanges )
            {
                Assert.IsTrue( dateRanges.Contains( expectedDateRange ) );
            }
        }

        [TestMethod]
        public void DateRangeExtensions_MergeDatesToDateRangesWithSpecifiedDatesGap_GapEqualsTwoOrMore_DateRangeForDatesWithSpecifiedGap()
        {
            // Arrange
            var dates = new List<DateTime>
            {
                new DateTime( 2017, 9, 11, 1, 0, 0 ),
                new DateTime( 2017, 9, 13, 1, 0, 0 ),
                new DateTime( 2017, 9, 15, 1, 0, 0 ),

                new DateTime( 2017, 9, 18, 1, 0, 0 ),
                new DateTime( 2017, 9, 19, 1, 0, 0 )
            };

            // Act
            SortedSet<DateRange> dateRanges = dates.MergeDatesToDateRangesWithSpecifiedDatesGap( 2 );

            var expectedDateRanges = new SortedSet<DateRange>( DateRangeExtensions.Comparer)
            {
                new DateRange( new DateTime( 2017, 9, 11, 1, 0, 0 ), new DateTime( 2017, 9, 15, 1, 0, 0 ) ),
                new DateRange( new DateTime( 2017, 9, 18, 1, 0, 0 ), new DateTime( 2017, 9, 19, 1, 0, 0 ) )
            };

            // Assert
            Assert.AreEqual( expectedDateRanges.Count, dateRanges.Count );
            foreach ( DateRange expectedDateRange in expectedDateRanges )
            {
                Assert.IsTrue( dateRanges.Contains( expectedDateRange ) );
            }
        }

        [DataTestMethod]
        [DataRow( "OneMomentWithEndnings", 12, 12, 12, 12, true )]
        [DataRow( "SameDateRangeWithEndings", 12, 13, 12, 13, true )]
        [DataRow( "SameDateRangeWithoutEndings", 12, 13, 12, 13, false )]
        [DataRow( "FollowingDateRangesWithEndings", 12, 13, 13, 14, true )]
        public void DateRangeExtensions_Intersects_HasIntersection_ReturnsTrue(
            string scenario,
            int firstStartDate,
            int firstEndDate,
            int secondStartDate,
            int secondEndDate,
            bool shouldIncludeEndings )
        {
            // Arrange
            var dateRange1 = new DateRange( new DateTime( 2019, 7, firstStartDate, 12, 0, 0 ), new DateTime( 2019, 7, firstEndDate, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2019, 7, secondStartDate, 12, 0, 0 ), new DateTime( 2019, 7, secondEndDate, 12, 0, 0 ) );

            // Act
            bool result = dateRange1.Intersects( dateRange2, shouldIncludeEndings );

            // Assert
            Assert.IsTrue( result );
        }

        [DataTestMethod]
        [DataRow( "OneMomentWithoutEndings", 12, 12, 12, 12, false )]
        [DataRow( "FollowingDateRangesWithoutEndings", 12, 13, 13, 14, false )]
        [DataRow( "NoIntersection", 12, 13, 14, 14, true )]
        [DataRow( "NoIntersectionWithoutEndings", 12, 13, 14, 14, false )]
        public void DateRangeExtensions_Intersects_NoIntersection_ReturnsFalse(
            string scenario,
            int firstStartDate,
            int firstEndDate,
            int secondStartDate,
            int secondEndDate,
            bool shouldIncludeEndings )
        {
            // Arrange
            var dateRange1 = new DateRange( new DateTime( 2019, 7, firstStartDate, 12, 0, 0 ), new DateTime( 2019, 7, firstEndDate, 12, 0, 0 ) );
            var dateRange2 = new DateRange( new DateTime( 2019, 7, secondStartDate, 12, 0, 0 ), new DateTime( 2019, 7, secondEndDate, 12, 0, 0 ) );

            // Act
            bool result = dateRange1.Intersects( dateRange2, shouldIncludeEndings );

            // Assert
            Assert.IsFalse( result );
        }
    }
}
