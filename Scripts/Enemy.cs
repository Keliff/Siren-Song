using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyCSharp;
using ExtensionMethods;
using System;
using System.Threading;

public class Enemy : Character, IClickable {

	public static Enemy instance;
	// TODO: Have a DropTable variable eventually
	// The player this Enemy is currently tareting
	public Player target;
	// The current distance to their target, measured in number of Tiles
	public uint distanceToTarget;
	// How close a Player needs to be to aggro this particular Enemy
	public uint activationDistance;
	// Whether or not the Enemy has been aggro'd
	public bool activated;
	public int initiative;
	// TODO: Have character have the _movement variable, so that way player can still do it's movement / action thing and then both Enemy and NPC will still have a movement variable
	public int _movement;

	// TEST
	public bool executing;

	private List<Player> allPlayers;

	private void Awake()
	{
		allPlayers = Globals.playerList;
		Globals.enemyList.Add(this);
	}

	private void OnEnable()
	{
//		Globals.Turn.NewEnemyTurn += NewTurn;
	}

	private void OnDisable()
	{
//		Globals.Turn.NewEnemyTurn -= NewTurn;
	}

	private void OnDestroy()
	{
		Globals.enemyList.Remove(this);
	}

	public void AI()
	{

		StartCoroutine( AISequence() );

	}

	IEnumerator WaitSeconds( float seconds )
	{
		yield return new WaitForSeconds( seconds );
	}

	public IEnumerator AISequence()
	{
		print( "IN AI SEQUENCE" );
		SpawnMoveGrid();
		yield return new WaitForSeconds(1);
		print("->First second waited");
		ClosestPositionToTarget();
		yield return new WaitForSeconds(1);
		print("->Second second waited");
		DeleteMovementGrid();
		yield return new WaitForSeconds(1);
		print("->Third second waited");
	}

	IEnumerator StartSpawnMoveGrid()
	{
		yield return new WaitForSeconds(1);
		SpawnMoveGrid();
	}

	IEnumerator StartClosestPositionToTarget()
	{
		yield return new WaitForSeconds(1);
		ClosestPositionToTarget();
	}

	IEnumerator StartDeleteMovementGrid()
	{
		yield return new WaitForSeconds(1);
		DeleteMovementGrid();
	}
	
		//TODO: Set up these time intervals as a constant somewhere. Like have a quick time, a long time, etc.


	private void SpawnMoveGrid()
	{
		print("IN SPAWN MOVE GRID");
		Spawner.instance.SpawnPrefabInDiamondGrid( "EnemyMoveTile", this.coords, this.moveTiles, EnemyHost.instance.moveTileHost, 0.5f, "MOVETILE"  );
		print("->Spawned grid");
		Globals.moveGridSpawned = true;
	}

	private void ClosestPositionToTarget()
	{
		print("IN CLOSEST POSITION");
		// Find target's position
		Vector2 position = this.coords;
		Vector2 direction = target.coords.DirectionFromThisToOther( this.coords );

		Vector2 firstDir = new Vector2( target.coords.x + direction.x, target.coords.y );
		Vector2 secondDir = new Vector2( target.coords.x, target.coords.y + direction.y );
		// Determine what the closest movement gird position to that target is
		// if the distance to the player < this.moveTiles, then we can start from the player's position and test the directions away from them
		// There is a + 1 to the moveTiles because we're really testing if the Enemy can get next to the player, not on top of them
		// It also checks to ensure that those spaces next to the player are not occupied
		if ( target.coords.TileDifference( this.coords ) <= ( this.moveTiles + 1 ) && !Spawner.instance.IfTileIsOccupied( firstDir ) && !Spawner.instance.IfTileIsOccupied( secondDir ) )
		{
			// If this if triggers, the correct position will be one of these two potential positions
			if ( Globals.moveGridList.ContainsKey( firstDir ) )
				position = firstDir;
			else if ( Globals.moveGridList.ContainsKey( secondDir ) )
				position = secondDir;
		} else // if ( targetCoords.TileDifference( this.coords ) > this.moveTiles )
		{
			// Now it's the direction of the enemy TOWARDS the target
			direction = direction.FlipDirection();

			Dictionary< Vector2, GameObject > tempDic = DirectionMoveGrid( direction, Globals.moveGridList );
			Dictionary< Vector2, int > tempDiff = TileDistancesOfMoveGrid( tempDic );
			List<int> diffs = tempDiff.Values.ToList();

			int min = diffs.Min();
			int count = 0;
			List<Vector2> tempList = new List<Vector2>();
			bool parallel = false;

			// It doesn't matter if there is 1 or more of the same min, the loop is the same and so is the setting of it.
			// So it's a quicker, cheaper process just to iterate through the dictionary once, and break early if it's parallel
			foreach( Vector2 key in tempDiff.Keys )
			{
				if ( tempDiff[ key ] == min )
				{
					count++;
					tempList.Add( key );
					position = key;
					// If the prospective tile is parallel with the target
					if ( key.IfParallel( target.coords ) )
					{
						parallel = true;
						// Then we want that position and should break out of the foreach loop
						break;
					}
				}
			}

			if ( !parallel && count > 1 )
			{
				System.Random rnd = new System.Random();
				
				position = tempList[ rnd.Next( count ) ];
			}
		}

		this.coords = position;
		this.transform.position = Globals.moveGridList[ position ].transform.position;

	}

//	public void NewTurn()
//	{
//		print("in enemy new turn");
//		// Have it find a target and print out who it thinks it target should be for now
//		target = FindPlayerTarget();
//
//		print( target.name );
//	}

	private Dictionary< Vector2, GameObject > DirectionMoveGrid( Vector2 direction, Dictionary< Vector2, GameObject > baseDic )
	{
		Dictionary< Vector2, GameObject > tempDic = new Dictionary< Vector2, GameObject >();
//		Dictionary< Vector2, int > tileDist = new Dictionary< Vector2, int >();

		foreach( Vector2 key in baseDic.Keys )
		{
			// If the key is in the correct direction, then add to tempDic
			if ( direction == Globals.NORTH || direction == Globals.NORTHEAST || direction == Globals.NORTHWEST )
			{
				if ( key.y > this.coords.y )
					tempDic.Add( key, baseDic[ key ] );
			} else if ( direction == Globals.SOUTH || direction == Globals.SOUTHEAST || direction == Globals.SOUTHWEST )
			{
				if ( key.y < this.coords.y )
					tempDic.Add( key, baseDic[ key ] );
			}

			else if ( direction == Globals.EAST || direction == Globals.NORTHEAST || direction == Globals.SOUTHEAST )
			{
				if ( key.x > this.coords.x )
					tempDic.Add( key, baseDic[ key ] );
			} else if ( direction == Globals.WEST || direction == Globals.NORTHWEST || direction == Globals.SOUTHWEST )
			{
				if ( key.x < this.coords.x )
					tempDic.Add( key, baseDic[ key ] );
			}
		}

		return tempDic;
	}

	private Dictionary< Vector2, int > TileDistancesOfMoveGrid( Dictionary< Vector2, GameObject > baseDic )
	{
		Dictionary< Vector2, int > tempDic = new Dictionary<Vector2, int>();

		foreach( Vector2 key in baseDic.Keys )
		{
			tempDic.Add( key, target.coords.TileDifference( key ) );
		}

		return tempDic;
	}

	private bool HasDuplicateInt( int[] arrayList, int num, int firstIndex )
	{
		for ( int i = firstIndex + 1; i < arrayList.Length; i++ )
		{
			if ( arrayList[ i ] == num )
				return true;
		}

		return false;
	}

	// Returns the Player closest to this Enemy
	// Chooses closeness of Player over health currently
	public Player FindPlayerTarget()
	{
		// All the different distances the Players are from this particular Enemy
		int[] diffDistances;
		// Instantiating an array of exact size, for all the current players on the map
		diffDistances = new int[ allPlayers.Count ];
		for ( int i = 0; i < allPlayers.Count; i++ )
		{
			// The array stores the number of tiles the particular players are from this enemy
			diffDistances[ i ] = allPlayers[ i ].coords.TileDifference( this.coords );
		}
		// Finds the first occurrence of the smallest distance a player is away from this enemy
		int smallest = diffDistances.Min();
		// The index of the array the closest player is
		int index = Array.IndexOf( diffDistances, smallest );

		// If there's more than one of the smallest number, that means more than one player is the same distance away
		if ( HasDuplicateInt( diffDistances, smallest, index ) )
		{
			// The health of the player at the current index
			int indexHealth = allPlayers[ index ].currentHealth;

			// Starts at index + 1 so it can start analyzing all the other players and waste less time iterating through the array
			for ( int i = index + 1; i < diffDistances.Length; i++ )
			{
				// If the distance at i index is also the smallest distance
				if ( diffDistances[ i ] == smallest )
				{
					// If the health of the first player is greater than this current player
					// TODO: Test to see if this should be < or >
					if ( indexHealth >= allPlayers[ i ].currentHealth )
					{
						// Change the index
						index = i;
						// Update the indexHealth
						indexHealth = allPlayers[ i ].currentHealth;
					}
				}
			}
		}

		// Assign the target
		target = allPlayers[ index ];
		// Assign the distance to the target
		distanceToTarget = (uint)diffDistances[ index ];

		// return target
		return target;
	}

	// TODO: Put this into Character, as everyone who inherits from it will need it at some point
	public void DeleteMovementGrid()
	{
		print("IN DELETE GRID");

		foreach ( Transform moveGrid in EnemyHost.instance.moveTileHost.transform )
		{
			Destroy( moveGrid.gameObject );
		}
		
		Globals.moveGridSpawned = false;
		Globals.moveGridList.Clear();
	}

	#region IClickable implementation
	
	public void OnMouseClick ()
	{
//		throw new System.NotImplementedException ();
	}
	
	#endregion

}
























