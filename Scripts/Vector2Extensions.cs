using UnityEngine;
using System.Collections;

namespace ExtensionMethods
{
	public static class Vector2Extensions {

		public static int TileDifference( this Vector2 v, Vector2 compare )
		{
			Vector2 temp = SubtractVector2s( v, compare );
			return (int)( Mathf.Abs( temp.x ) + Mathf.Abs( temp.y ) );
		}

		public static Vector2 SubtractVector2s( Vector2 one, Vector2 two )
		{
			Vector2 temp = new Vector2();
			temp.x = one.x - two.x;
			temp.y = one.y - two.y;
			return temp;
		}

		// Direction of the first vector TOWARDS the second vector
		public static Vector2 DirectionFromThisToOther( this Vector2 coords, Vector2 center )
		{
			Vector2 temp = new Vector2( center.x - coords.x, center.y - coords.y );

			if ( temp.x == 0 )
			{
				if ( temp.y > 0 )
					return Globals.NORTH;
				else // temp.y < 0
					return Globals.SOUTH;
			} else if ( temp.y == 0 )
			{
				if ( temp.x > 0 )
					return Globals.EAST;
				else // temp.x < 0
					return Globals.WEST;
			}

			if ( temp.x > 0 )
			{
				if ( temp.y > 0 )
					return Globals.NORTHEAST;
				else // temp.y < 0
					return Globals.SOUTHEAST;
			} else // temp.x < 0
			{
				if ( temp.y > 0 )
					return Globals.NORTHWEST;
				else // temp.y < 0
					return Globals.SOUTHWEST;
			}
			
		}

		public static Vector2 FlipDirection( this Vector2 coords )
		{
			return new Vector2( -coords.x, -coords.y );
		}

		public static bool IfParallel( this Vector2 one, Vector2 two )
		{
			if ( one.x == two.x || one.y == two.y )
				return true;
			else
				return false;
		}
	}
}




























