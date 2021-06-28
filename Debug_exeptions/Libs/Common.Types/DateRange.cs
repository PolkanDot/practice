using System;

namespace Lesson.Libs.Common.Types
{
    public struct DateRange : IEquatable<DateRange>
    {
        private DateTime _startDateTime;
        private DateTime _endDateTime;

        public DateTime StartDateTime
        {
            get => _startDateTime;
            set
            {
                _startDateTime = value;
                Validate();
            }
        }

        public DateTime EndDateTime
        {
            get => _endDateTime;
            set
            {
                _endDateTime = value;
                Validate();
            }
        }

        public DateTime StartDate => StartDateTime.Date;
        public DateTime EndDate => EndDateTime.Date;

        public TimeSpan StartTime => StartDateTime.TimeOfDay;
        public TimeSpan EndTime => EndDateTime.TimeOfDay;

        public TimeSpan Duration => EndDateTime - StartDateTime;

        public int Days => Nights + 1;

        public int Nights => ( EndDate - StartDate ).Days;

        public DateRange( DateTime startDateTime, DateTime endDateTime )
        {
            _startDateTime = startDateTime;
            _endDateTime = endDateTime;
            Validate();
        }

        public override string ToString()
        {
            return $"[{StartDateTime}, {EndDateTime}]";
        }

        public override bool Equals( object obj )
        {
            return obj is DateRange dateRange && Equals( dateRange );
        }

        public bool Equals( DateRange other )
        {
            return StartDateTime == other.StartDateTime
                && EndDateTime == other.EndDateTime;
        }

        public override int GetHashCode()
        {
            int hashCode = 1816395913;
            hashCode = ( hashCode * -1521134295 ) + StartDateTime.GetHashCode();
            hashCode = ( hashCode * -1521134295 ) + EndDateTime.GetHashCode();
            return hashCode;
        }

        public static bool operator ==( DateRange range1, DateRange range2 )
        {
            return range1.Equals( range2 );
        }

        public static bool operator !=( DateRange range1, DateRange range2 )
        {
            return !( range1 == range2 );
        }

        private void Validate()
        {
            if ( EndDateTime == default )
            {
                return;
            }

            if ( StartDateTime > EndDateTime )
            {
                throw new ArgumentException( $"{nameof( DateRange )} {nameof( StartDateTime )} {StartDateTime} cannot follow {nameof( EndDateTime )} {EndDateTime}" );
            }
        }
    }
}
