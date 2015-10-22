using UnityEngine;
using System.Collections;
// For Dictionaries
using System.Collections.Generic;
// To go through Files
using System.IO;

/**
 * This class is designed to exist as a helper class.
 * 
 * There are methods for spawning prefabs in particular ways to work with the grid system.
 * */
public class Spawner : MonoBehaviour {

	/**
	 * Here is a list of things that Spawner needs to help accomplish for the first Prototype
	 * 
	 * - Determine what spawning styles are needed to help create the map
	 * - Work with the map to be able to spawn in all the necessary tiles
	 * 
	 * */

	public static Spawner instance;

	private void Awake()
	{
		Globals.ReadPrefabs();
		if ( instance == null )
			instance = this;
	}

	/* I'm going to begin to re-organize this particular class
	 * 
	 * Now, it's not very well written, how to spawn prefab at coords goes. Currently, it's utilizing a vector2 spawnposition to make the prefab spawn at a particular place
	 * 
	 * Now, while that does still technically work, for anyone looking at the code, including myself, this doesn't make the most sense. What I should really want to input into the method
	 * is just the x and y co-ordinate. I've design a 2D system for the map, and to just need to know the co-ordinate is really nice. And that is what I should aim for.
	 * 
	 * That means the conversion, that does involve the co-ordinate in terms of two simpler integers, into the actual vector 3 position in space, should happen at the most simple method. Not at the more abstracted ones.
	 * So, we're goign to re-write SpawnPrefabAtCoords to take in a very simple co-ordinate setup, which IT will convert into the correct 3D spacing, with still the illusion of 2D
	 * */

// Spawn single prefab at co-ordinate methods

	// Spawn prefab at a co-ordinate, into the default hierarchy
	public GameObject SpawnPrefabAtCoords( string spawnString, Vector2 coords )
	{
		// Get the prefab data
		GameObject spawnPrefab = Globals.prefabList[ spawnString ];
		// Spawn said prefab as game object at the given co-ordinate
		GameObject spawnItem = (GameObject)Instantiate( spawnPrefab,
		                                               new Vector3( this.transform.position.x + ( coords.x * Globals.spriteWidth ),
		            												this.transform.position.y + ( coords.y * Globals.spriteHeight ),
		         												    this.transform.position.z),
		                                               Quaternion.identity );
		// Set the co-ordinates for the newly spawned object
		SetCoordsForPrefab( spawnString, spawnItem, coords );
		// return the object
		return spawnItem;
	}

	// Spawn prefab at a co-ordinate, into the default hierarchy, at a particular alpha
	public GameObject SpawnPrefabAtCoords( string spawnString, Vector2 coords, float alpha )
	{
		// Get the prefab data
		GameObject spawnPrefab = Globals.prefabList[ spawnString ];
		// Spawn said prefab as game object at the given co-ordinate
		GameObject spawnItem = (GameObject)Instantiate( spawnPrefab,
		                                               new Vector3( this.transform.position.x + ( coords.x * Globals.spriteWidth ),
		            this.transform.position.y + ( coords.y * Globals.spriteHeight ),
		            this.transform.position.z),
		                                               Quaternion.identity );
		// Set the co-ordinates for the newly spawned object
		SetCoordsForPrefab( spawnString, spawnItem, coords );

		// Set the alpha component of the new object
		SpriteRenderer spawnSprite = spawnItem.GetComponent<SpriteRenderer>();
		if ( spawnSprite != null )
		{
			Color spawnColor = spawnSprite.color;
			spawnSprite.material.color = new Color( spawnColor.r, spawnColor.g, spawnColor.b, alpha );
		}
		else {
			Debug.Log("SpawnPrefabAtCoords() was asked to change the alpha of a game object that had no sprite renderer.");
		}

		// return the object
		return spawnItem;
	}

	// Spawn prefab at a co-ordinate, making it the child of spawnParent
	public GameObject SpawnPrefabAtCoords( string spawnString, Vector2 coords, GameObject spawnParent )
	{
		// Get the prefab data
		GameObject spawnPrefab = Globals.prefabList[ spawnString ];
		// Spawn said prefab as game object at the given co-ordinate
		GameObject spawnItem = (GameObject)Instantiate( spawnPrefab,
		                                                new Vector3( this.transform.position.x + ( coords.x * Globals.spriteWidth ),
		                                                             this.transform.position.y + ( coords.y * Globals.spriteHeight ),
		                                                             this.transform.position.z),
		                                                Quaternion.identity );
		// Set the parent of the newly spawned object
		spawnItem.transform.parent = spawnParent.transform;
		// Set the co-ordinates for the newly spawned object
		SetCoordsForPrefab( spawnString, spawnItem, coords );
		// return the object
		return spawnItem;
	}

	// Spawn prefab at a co-ordiante, making it the child of spawnParent, and at a particular alpha
	public GameObject SpawnPrefabAtCoords( string spawnString, Vector2 coords, GameObject spawnParent, float alpha )
	{
		// Get the prefab data
		GameObject spawnPrefab = Globals.prefabList[ spawnString ];
		// Spawn said prefab as game object at the given co-ordinate
		GameObject spawnItem = (GameObject)Instantiate( spawnPrefab,
		                                               new Vector3( this.transform.position.x + ( coords.x * Globals.spriteWidth ),
														            this.transform.position.y + ( coords.y * Globals.spriteHeight ),
														            this.transform.position.z),
		                                               Quaternion.identity );
		// Set the parent of the newly spawned object
		spawnItem.transform.parent = spawnParent.transform;
		// Set the co-ordinates for the newly spawned object
		SetCoordsForPrefab( spawnString, spawnItem, coords );

		// Set the alpha component of the new object
		SpriteRenderer spawnSprite = spawnItem.GetComponent<SpriteRenderer>();
		if ( spawnSprite != null )
		{
			Color spawnColor = spawnSprite.color;
			spawnSprite.material.color = new Color( spawnColor.r, spawnColor.g, spawnColor.b, alpha );
		}
		else {
			Debug.Log("SpawnPrefabAtCoords() was asked to change the alpha of a game object that had no sprite renderer.");
		}

		// return the object
		return spawnItem;
	}

// Spawning prefabs ONLY in a particular direction

	// Spawns prefabs in a horizontal path, at a particular distance equi-distant from the coords
	public void SpawnPrefabOnlyHorizontal( string spawnString, Vector2 coords, int distance, GameObject spawnParent, bool center )
	{
		// Builds the same number horizontal in both directions from the center
		for ( int width = distance; width > 0; width-- )
		{
			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x + width, coords.y ), spawnParent );
//			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x + ( Globals.spriteWidth * width ), coords.y ), spawnParent, (int)coords.x + width, (int)coords.y + height );
			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x - width, coords.y ), spawnParent );
//			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x - ( Globals.spriteWidth * width ), coords.y ), spawnParent, (int)coords.x + -width, (int)coords.y + height );
		}
		// Spawn the center point
		if ( center )
			SpawnPrefabAtCoords( spawnString, coords, spawnParent );
//		SpawnPrefabAtCoords( spawnString, coords, spawnParent, 0, height );
		
	}

	// Spawns prefabs in a horizontal path, at a particular distance equi-distant from the coords, at a particular alpha
	public void SpawnPrefabOnlyHorizontal( string spawnString, Vector2 coords, int distance, GameObject spawnParent, bool center, float alpha )
	{
		// Builds the same number horizontal in both directions from the center
		for ( int width = distance; width > 0; width-- )
		{
			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x + width, coords.y ), spawnParent, alpha );
			//			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x + ( Globals.spriteWidth * width ), coords.y ), spawnParent, (int)coords.x + width, (int)coords.y + height );
			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x - width, coords.y ), spawnParent, alpha );
			//			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x - ( Globals.spriteWidth * width ), coords.y ), spawnParent, (int)coords.x + -width, (int)coords.y + height );
		}
		// Spawn the center point
		if ( center )
			SpawnPrefabAtCoords( spawnString, coords, spawnParent, alpha );
		//		SpawnPrefabAtCoords( spawnString, coords, spawnParent, 0, height );
		
	}

	// Spawns prefabs in a horizontal path, at a particular distance equi-distant from the coords, at a particular alpha
	public void SpawnPrefabOnlyHorizontal( string spawnString, Vector2 centerCoord, Vector2 coords, int distance, GameObject spawnParent, bool center, float alpha, string restriction )
	{
		if ( restriction == "MOVETILE" )
		{
			// Builds the same number horizontal in both directions from the center
			for ( int width = distance; width > 0; width-- )
			{
				if ( PathFromCoordToCenter( new Vector2( coords.x + width, coords.y ), centerCoord ) )
					SpawnPrefabAtCoords( spawnString, new Vector2( coords.x + width, coords.y ), spawnParent, alpha );
				if ( PathFromCoordToCenter( new Vector2( coords.x - width, coords.y ), centerCoord ) )
					SpawnPrefabAtCoords( spawnString, new Vector2( coords.x - width, coords.y ), spawnParent, alpha );
			}
			// Spawn the center point
			if ( center )
				SpawnPrefabAtCoords( spawnString, coords, spawnParent, alpha );
			//		SpawnPrefabAtCoords( spawnString, coords, spawnParent, 0, height );
		}
		
	}

	// I'm going to need a method that builds only vertically
	public void SpawnPrefabOnlyVertical( string spawnString, Vector2 spawnPosition, int distance )
	{
		// Builds the same number vertical in both directions from the center
	}

// Spawning only SOME prefab in a direction

	// This method exists for when there is an uneven amount that needs to be build left / right
	public void SpawnPrefabSomeHorizontal( string spawnString, Vector2 coords, int leftDistance, int rightDistance, GameObject spawnParent, bool center )
	{
		for ( int left = leftDistance; left > 0; left-- )
			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x - left, coords.y ), spawnParent );
		//			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x - ( Globals.spriteWidth * left ), coords.y ), spawnParent, -left, height );
		for ( int right = rightDistance; right > 0; right-- )
			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x + right, coords.y ), spawnParent );
		//			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x + ( Globals.spriteWidth * right ), coords.y ), spawnParent, right, height );
		// Spawn center Tile
		if ( center )
			SpawnPrefabAtCoords( spawnString, coords, spawnParent );
		//		SpawnPrefabAtCoords( spawnString, coords, spawnParent, 0, height );
	}

	 // This method exists for when there is an uneven amount that needs to be built up / down
	public void SpawnPrefabSomeVertical( string spawnString, Vector2 coords, int upDistance, int downDistance, GameObject spawnParent, bool center )
	{
		// Builds upDistance up from the spawnPosition, and downDistance down
		for ( int up = upDistance; up > 0; up-- )
			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x, coords.y + up ), spawnParent );
//			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x, spawnPosition.y + ( Globals.spriteHeight * up ) ), spawnParent, width, up );
		for ( int down = downDistance; down > 0; down-- )
			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x, coords.y - down ), spawnParent );
//			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x, spawnPosition.y - ( Globals.spriteHeight * down ) ), spawnParent, width, -down );
		// Spawn center Tile
		if ( center )
			SpawnPrefabAtCoords( spawnString, coords, spawnParent );
//		SpawnPrefabAtCoords( spawnString, spawnPosition, spawnParent, width, 0 );
	}
	
// Each method from here uses a variety of the above methods to create a particular organization for the prefab spawn.

	// Spawns prefabs in a line of the direction of your choosing
	// This method doe NOT spawn on the center tile. It uses that a basis of where to start, but does not spawn on that point
	public void SpawnPrefabInLine( string spawnString, Vector2 coords, Vector2 direction, int distance, GameObject spawnParent )
	{
		for ( int i = 1; i <= distance; i++ )
		{
			SpawnPrefabAtCoords( spawnString,
			                     new Vector2( coords.x + ( direction.x * i ),
			                                  coords.y + ( direction.y * i ) ),
			                     spawnParent );
		}
	}

	// Spawns prefabs in a line of the direction of your choosing
	// This method doe NOT spawn on the center tile. It uses that a basis of where to start, but does not spawn on that point
	// Also controls of the alpha of the prefab!
	public void SpawnPrefabInLine( string spawnString, Vector2 coords, Vector2 direction, int distance, GameObject spawnParent, float alpha )
	{
		for ( int i = 1; i <= distance; i++ )
		{
			SpawnPrefabAtCoords( spawnString,
			                    new Vector2( coords.x + ( direction.x * i ),
			            					 coords.y + ( direction.y * i ) ),
			                    spawnParent,
			                    alpha );
		}
	}

	// Spawns prefabs in a line of the direction of your choosing
	// This method doe NOT spawn on the center tile. It uses that a basis of where to start, but does not spawn on that point
	// Also controls of the alpha of the prefab!
	// Also takes in a string restriction
	public void SpawnPrefabInLine( string spawnString, Vector2 coords, Vector2 direction, int distance, GameObject spawnParent, float alpha, string restriction )
	{

		for ( int i = 1; i <= distance; i++ )
		{
			// If there is no Tile at the particular co-ordinate, this restriction does not want to spawn anything there.
			if ( restriction == "MOVETILE" )
			{
				// If no Tile is at the co-ordinate
				// Or if there is something else occupying that Tile
				if ( !IfTileIsAtCoord( new Vector2( coords.x + ( direction.x * i ), coords.y + ( direction.y * i ) ) ) || IfTileIsOccupied( new Vector2( coords.x + ( direction.x * i ), coords.y + ( direction.y * i ) ) ) )
					return;
			}

			SpawnPrefabAtCoords( spawnString,
			                    new Vector2( coords.x + ( direction.x * i ),
			           						 coords.y + ( direction.y * i ) ),
			                    spawnParent,
			                    alpha );
		}
	}

	// Spawns prefabs in a diagonal line of your choosing
	public void SpawnPrefabInDiagonal( string spawnString, Vector2 coords, Vector2 direction, int distance, GameObject spawnParent, bool center )
	{
		for ( int i = 1; i <= distance; i++ )
		{
			SpawnPrefabAtCoords( spawnString, new Vector2( coords.x + ( direction.x * i ), coords.y + ( direction.y * i ) ), spawnParent );
		}
		if ( center )
			SpawnPrefabAtCoords( spawnString, coords, spawnParent );
	}

	// Spawns prefabs in diagonals at every direction
	public void SpawnPrefabInDiagonalCross( string spawnString, Vector2 coords, int distance, GameObject spawnParent )
	{
		// Spawn all diagonal directions
		SpawnPrefabInDiagonal( spawnString, coords, Globals.NORTHEAST, distance, spawnParent, false );
		SpawnPrefabInDiagonal( spawnString, coords, Globals.SOUTHEAST, distance, spawnParent, false );
		SpawnPrefabInDiagonal( spawnString, coords, Globals.SOUTHWEST, distance, spawnParent, false );
		SpawnPrefabInDiagonal( spawnString, coords, Globals.NORTHWEST, distance, spawnParent, false );
		// Spawn center
		SpawnPrefabAtCoords( spawnString, coords, spawnParent );
	}

	// Spawns prefab in a cross shape, given a distance.
	public void SpawnPrefabInCross( string spawnString, Vector2 coords, int distance, GameObject spawnParent )
	{
		// I'll want another method that utilizes spawning prefabs in a line
		SpawnPrefabInLine( spawnString, coords, Globals.NORTH, distance, spawnParent );
		SpawnPrefabInLine( spawnString, coords, Globals.SOUTH, distance, spawnParent );
		SpawnPrefabInLine( spawnString, coords, Globals.EAST, distance, spawnParent );
		SpawnPrefabInLine( spawnString, coords, Globals.WEST, distance, spawnParent );
	}

	// Spawns prefab in a cross shape, given a distance.
	// Variation of the above method that controls the alpha of the spawned prefab
	public void SpawnPrefabInCross( string spawnString, Vector2 coords, int distance, GameObject spawnParent, float alpha )
	{
		// I'll want another method that utilizes spawning prefabs in a line
		SpawnPrefabInLine( spawnString, coords, Globals.NORTH, distance, spawnParent, alpha );
		SpawnPrefabInLine( spawnString, coords, Globals.SOUTH, distance, spawnParent, alpha );
		SpawnPrefabInLine( spawnString, coords, Globals.EAST, distance, spawnParent, alpha );
		SpawnPrefabInLine( spawnString, coords, Globals.WEST, distance, spawnParent, alpha );
	}

	// Spawns prefab in a cross shape, given a distance.
	// Variation of the above method that controls the alpha of the spawned prefab
	// This also takes in a restriction
	public void SpawnPrefabInCross( string spawnString, Vector2 coords, int distance, GameObject spawnParent, float alpha, string restriction )
	{
		// I'll want another method that utilizes spawning prefabs in a line
		SpawnPrefabInLine( spawnString, coords, Globals.NORTH, distance, spawnParent, alpha, restriction );
		SpawnPrefabInLine( spawnString, coords, Globals.SOUTH, distance, spawnParent, alpha, restriction );
		SpawnPrefabInLine( spawnString, coords, Globals.EAST, distance, spawnParent, alpha, restriction );
		SpawnPrefabInLine( spawnString, coords, Globals.WEST, distance, spawnParent, alpha, restriction );
	}

	/**
	 * This method spawns a diamond grid, which is my name for a movement grid type shape.
	 * 
	 * It utilizes SpawnPrefabInCross and SpawnPrefabOnlyHorizontal, ommitting the center value
	 * */
	public void SpawnPrefabInDiamondGrid( string spawnString, Vector2 coords, int distance, GameObject spawnParent )
	{
		
		// I'll want another method that will spawn prefabs in a cross
		SpawnPrefabInCross( spawnString, coords, distance, spawnParent );
		
		for ( int row = distance - 1, amount = 1; row > 0; row--, amount++ )
		{
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( coords.x, coords.y + row ), amount, spawnParent, false );
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( coords.x, coords.y - row ), amount, spawnParent, false );
		}
		
		// Spawn the center tile
		SpawnPrefabAtCoords( spawnString, coords, spawnParent );
		
	}

	/**
	 * This is a variation method for SpawnPrefabInDiamondGrid that allows the set of the alpha of the spawning prefab
	 * */
	public void SpawnPrefabInDiamondGrid( string spawnString, Vector2 coords, int distance, GameObject spawnParent, float alpha )
	{
		
		// I'll want another method that will spawn prefabs in a cross
		SpawnPrefabInCross( spawnString, coords, distance, spawnParent, alpha  );
//		SpawnPrefabInCross( spawnString, coords, distance, spawnParent );
		
		for ( int row = distance - 1, amount = 1; row > 0; row--, amount++ )
		{
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( coords.x, coords.y + row ), amount, spawnParent, false, alpha );
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( coords.x, coords.y - row ), amount, spawnParent, false, alpha );
		}
		
		// Spawn the center tile
		SpawnPrefabAtCoords( spawnString, coords, spawnParent, alpha );
		
	}

	public void SpawnPrefabInDiamondGrid( string spawnString, Vector2 coords, int distance, GameObject spawnParent, float alpha, string restriction )
	{

		/**
		 * I need to be able to take in a string called restriction. This will allow the user to have a way to NOT spawn a particular prefab somewhere if the restriction is not met.
		 * 
		 * Such as, in my current of designing, I need to only spawn a movement grid piece if there is in-fact a tile there.
		 * 
		 * The actual restriction and what it does is dependent on what I need at the time.
		 * 
		 * For this current situation, I'll need it to check if a tile is there, and then I will also need to make sure that there is a path back to the center tile when dealing with the only horizontal
		 * or with the line, it'll be much easier and I'll just need to check whether or not to continue in a particular direction
		 **/

		// I'll want another method that will spawn prefabs in a cross
		SpawnPrefabInCross( spawnString, coords, distance, spawnParent, alpha, restriction  );

		// I want the row to start at 1, going upwards to the distance - 1
		for ( int row = distance - 1, amount = 1; row > 0; row--, amount++ )
		{
			SpawnPrefabOnlyHorizontal( spawnString, coords, new Vector2( coords.x, coords.y + row ), amount, spawnParent, false, alpha, restriction );
			SpawnPrefabOnlyHorizontal( spawnString, coords, new Vector2( coords.x, coords.y - row ), amount, spawnParent, false, alpha, restriction );
		}
		
		// Spawn the center tile
		// I don't need to check this one for restriction, because the center is the piece that the whatever is spawning the diamond grid is on
		// I know it is safe.
		SpawnPrefabAtCoords( spawnString, coords, spawnParent, alpha );
	}



//	/**
//	 * This is a variation method for SpawnPrefabInDiamondGrid that allows the set of the alpha of the spawning prefab
//	 * 
//	 * It also takes in a bool that allows a restriction to be set of whether or not it can only be spawned if a tile is at that co-ordinate.
//	 * */
//	public void SpawnPrefabInDiamondGrid( string spawnString, Vector2 spawnPosition, int distance, GameObject spawnParent, float alpha, bool onTile )
//	{
//
//		Dictionary< Vector2, Tile > tempTileList = new Dictionary<Vector2, Tile>();
//
//		// I'll want another method that will spawn prefabs in a cross
//		SpawnPrefabInCross( spawnString, spawnPosition, distance, spawnParent, alpha );
//		
//		for ( int row = distance - 1, amount = 1; row > 0; row--, amount++ )
//		{
//
//			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( spawnPosition.x, spawnPosition.y + (row * Globals.spriteHeight) ), amount, row, spawnParent, false, alpha );
//			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( spawnPosition.x, spawnPosition.y - (row * Globals.spriteHeight) ), amount, -row, spawnParent, false, alpha );
////			SpawnPrefabOnlyHorizontal( spawnString, new Vector2(),  );
//		}
//		
//		// Spawn the center tile
//		SpawnPrefabAtCoords(spawnString, spawnPosition, spawnParent, (int)spawnPosition.x, (int)spawnPosition.y, alpha);
//		
//	}

	private bool IfTileIsAtCoord( Vector2 coords )
	{
		return Globals.tileList.ContainsKey( coords );
	}

	private bool IfTileIsAtCoord( Vector2 coords, Vector2 direction )
	{
		return Globals.tileList.ContainsKey( new Vector2( coords.x + direction.x, coords.y + direction.y ) );
	}

	// This method determines if particular classes reside at the tile location, and returns true if something other than a tile is occupied there
	// Designed to be called ONLY after a tile has been confirmed to be at this co-ordiante
	public bool IfTileIsOccupied( Vector2 coords )
	{
		if ( Globals.IfPlayerAtCoords( coords ) )
			return true;
		else if ( Globals.IfEnemyAtCoords( coords ) )
			return true;
		//TODO: When npc is defined
//		else if ( Globals.NPCList.ContainsKey( coords ) )
//			return true;
		else if ( Globals.switchList.ContainsKey( coords ) )
			return true;
		else
			return false;

	}

	// TODO: You can make this an extension to Vector2 pretty easily
	private Vector2 CombineVectors( Vector2 one, Vector2 two )
	{
		return new Vector2( one.x + two.x, one.y + two.y );
	}

	private bool PathFromCoordToCenter( Vector2 coords, Vector2 center )
	{
		/**
		 * This method's purpose is to determine whether or not the coords sent in to it and can find a path to the center
		 * */

		// If there is no tile there.
		if ( !IfTileIsAtCoord( coords ) )
			return false;

		Vector2 current = coords;
		// This is the direction the current Vector2 needs to travel to attempt to find a path to the center
		Vector2 direction = DiagonalDirectionFromCenter( coords, center );

		while ( current != center )
		{
			// TODO: Change all these if's into another method given how much their logic overlaps

			if ( IfVectorsParallel( current, center ) )
				return true;

			if ( direction == Globals.NORTHEAST )
			{

				if ( IfTileIsAtCoord( current, Globals.NORTH ) && !IfTileIsOccupied( CombineVectors( current, Globals.NORTH ) ) )
					current = CombineVectors( current, Globals.NORTH );
				else if ( IfTileIsAtCoord( current, Globals.EAST ) && !IfTileIsOccupied( CombineVectors( current, Globals.EAST ) ) )
					current = CombineVectors( current, Globals.EAST );
				else
					return false;
			} else if ( direction == Globals.SOUTHEAST )
			{
				if ( IfTileIsAtCoord( current, Globals.SOUTH ) && !IfTileIsOccupied( CombineVectors( current, Globals.SOUTH ) ) )
					current = CombineVectors( current, Globals.SOUTH );
				else if ( IfTileIsAtCoord( current, Globals.EAST ) && !IfTileIsOccupied( CombineVectors( current, Globals.EAST ) ) )
					current = CombineVectors( current, Globals.EAST );
				else
					return false;
			} else if ( direction == Globals.SOUTHWEST )
			{
				if ( IfTileIsAtCoord( current, Globals.SOUTH ) && !IfTileIsOccupied( CombineVectors( current, Globals.SOUTH ) ) )
					current = CombineVectors( current, Globals.SOUTH );
				else if ( IfTileIsAtCoord( current, Globals.WEST ) && !IfTileIsOccupied( CombineVectors( current, Globals.WEST ) ) )
					current = CombineVectors( current, Globals.WEST );
				else
					return false;
			} else if ( direction == Globals.NORTHWEST )
			{
				if ( IfTileIsAtCoord( current, Globals.NORTH ) && !IfTileIsOccupied( CombineVectors( current, Globals.NORTH ) ) )
					current = CombineVectors( current, Globals.NORTH );
				else if ( IfTileIsAtCoord( current, Globals.WEST ) && !IfTileIsOccupied( CombineVectors( current, Globals.WEST ) ) )
					current = CombineVectors( current, Globals.WEST );
				else
					return false;
			}



		}

		// If this while loop ever exits, that means that current found a path to the center, which means the answer is true
		return true;
	}

	// TODO: Convert this into an Extension method for Vector2
	private Vector2 DiagonalDirectionFromCenter( Vector2 coords, Vector2 center )
	{
		// From the coords perspective, it's determining the direction towards the center Vector2

		Vector2 temp = new Vector2( center.x - coords.x, center.y - coords.y );

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

	//TODO: Make this an extension method
	private bool IfVectorsParallel( Vector2 one, Vector2 two )
	{
		if ( one.x == two.x )
			return true;
		else if ( one.y == two.y )
			return true;
		else
			return false;
	}

	private void SetCoordsForPrefab( string spawnString, GameObject spawnItem, Vector2 coords )
	{

		switch( spawnString )
		{
		case "Tile":
			Tile tileScript = spawnItem.GetComponent<Tile>();
			tileScript.coords = coords;
			// Add the Tile to the global Dictionary
			Globals.tileList.Add( coords, spawnItem );
			break;
		case "Player 2":
		case "Player":
			Player playerScript = spawnItem.GetComponent<Player>();
			playerScript.coords = coords;
			break;
		case "Enemy":
			Enemy enemyScript = spawnItem.GetComponent<Enemy>();
			enemyScript.coords = coords;
			break;
		case "Cursor":
			Cursor cursorScript = spawnItem.GetComponent<Cursor>();
			cursorScript.coords = coords;
			break;
		case "PlayerMoveTile":
			MovementGrid playerMoveScript = spawnItem.GetComponent<MovementGrid>();
			playerMoveScript.coords = coords;
			playerMoveScript.playerSpawner = Player.instance;
//			playerMoveScript.moveGridSpawner = spawnItem;
			playerMoveScript.parentType = "Player";
			Globals.moveGridList.Add( coords, spawnItem );
			break;
		case "EnemyMoveTile":
			// TODO: Confirm nothing needs to happen in this case
//			MovementGrid enemyMoveScript = spawnItem.GetComponent<MovementGrid>();
//			enemyMoveScript.coords = coords;
//			enemyMoveScript.moveGridSpawner = spawnItem.transform.parent.parent.gameObject;
//			enemyMoveScript.parentType = "Enemy";

			Globals.moveGridList.Add( coords, spawnItem );
			break;
		// case "NPCMoveTile":
		default:
			Debug.Log("A class was sent into SetCoordsForPrefab without a way to set it's co-ordinate value.");
			break;
		} // end switch
	}
	
}

































