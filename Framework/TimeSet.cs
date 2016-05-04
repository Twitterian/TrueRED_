using System;
using System.Text.RegularExpressions;

namespace TrueRED.Framework
{
	public class TimeSet
	{
		public int Hour { get; private set; }
		public int Minute { get; private set; }

		public TimeSet( int Hour, int Minute = 0 )
		{
			this.Hour = Hour;
			this.Minute = Minute;
			if ( this.Minute > 60 )
			{
				this.Hour += Minute / 60;
				this.Minute %= 60;
			}
		}

		public TimeSet( DateTime now )
		{
			this.Hour = now.Hour;
			this.Minute = now.Minute;
		}

		public static bool Verification( TimeSet current, TimeSet start, TimeSet end )
		{
			if ( end.Hour < 24 )
			{
				if ( start <= current && current <= end )
				{
					return true;
				}
			}
			else
			{
				var tend = new TimeSet(end.Hour % 24, end.Minute);
				if ( start <= current || current <= tend )
				{
					return true;
				}
			}
			return false;
		}

		public static bool operator ==( TimeSet c1, TimeSet c2 )
		{
			try
			{
				if ( c1.Hour == c2.Hour && c1.Minute == c2.Minute ) return true;
			}
			catch { }
			return false;
		}

		public static bool operator !=( TimeSet c1, TimeSet c2 )
		{
			if ( c1 == c2 ) return false;
			else return true;
		}

		public static bool operator <( TimeSet c1, TimeSet c2 )
		{
			if ( c1.Hour < c2.Hour ) return true;
			else if ( c1.Hour == c2.Hour && c1.Minute < c2.Minute ) return true;
			else return false;
		}

		public static bool operator >( TimeSet c1, TimeSet c2 )
		{
			if ( c1.Hour > c2.Hour ) return true;
			else if ( c1.Hour == c2.Hour && c1.Minute > c2.Minute ) return true;
			else return false;
		}

		public static bool operator <=( TimeSet c1, TimeSet c2 )
		{
			if ( c1 == c2 || c1 < c2 ) return true;
			else return false;
		}

		public static bool operator >=( TimeSet c1, TimeSet c2 )
		{
			if ( c1 == c2 || c1 > c2 ) return false;
			else return true;
		}

		public override bool Equals( object obj )
		{
			return this == ( TimeSet ) obj;
		}

		public override int GetHashCode( )
		{
			return base.GetHashCode( );
		}

		public override string ToString( )
		{
			return string.Format( "{0}시 {1}분", this.Hour, this.Minute );
		}

		public static TimeSet FromString( string timeStr )
		{
			var hour = int.Parse(new Regex("[0-9]+시").Match(timeStr).Value.Replace("시", ""));
			var minute = int.Parse(new Regex("[0-9]+분").Match(timeStr).Value.Replace("분", ""));

			return new TimeSet( hour, minute );
		}

	}
}
