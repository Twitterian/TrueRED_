using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueRED.Framework
{
	public class TimeSet
	{
		public TimeSet( int Hour, int Minute = 0 )
		{
			this.Hour = Hour;
			this.Minute = Minute;
		}

		public int Hour { get; private set; }
		public int Minute { get; private set; }

		public static TimeSet GetCurrentTimeset( DateTime date )
		{
			return new TimeSet( date.Hour, date.Minute );
		}

		// 검증 함수 검증 필요
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
			else if ( c1.Minute < c2.Minute ) return true;
			else return false;
		}

		public static bool operator >( TimeSet c1, TimeSet c2 )
		{
			if ( c1.Hour > c2.Hour ) return true;
			else if ( c1.Minute > c2.Minute ) return true;
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

	}
}
