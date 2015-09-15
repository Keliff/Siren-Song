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

	/**
	 * This is the simplest of all the spawning methods, allowing you to send in a co-ordinate's x and y and spawn a particular prefab there.
	 * */
	// If there is no want to set the co-ordinate, this is the method to use
	public GameObject SpawnPrefabAtCoords( string spawnString, Vector2 spawnPosition, GameObject spawnParent )
	{
		// Get the prefab information
		GameObject spawnPrefab = Globals.prefabList[ spawnString ];
		// Spawn the prefab at the particular co-ordiante
		GameObject spawnItem = (GameObject)Instantiate( spawnPrefab, new Vector3( spawnPosition.x, spawnPosition.y, transform.position.z ), Quaternion.identity );

		spawnItem.transform.parent = spawnParent.transform;

		SetCoordsForPrefab( spawnString, spawnItem, (int)spawnPosition.x, (int)spawnPosition.y );

		return spawnItem;
	}

	// If there is a want to set the co-ordiante of the prefab, use this version of the method
	public GameObject SpawnPrefabAtCoords( string spawnString, Vector2 spawnPosition, GameObject spawnParent, int xCoord, int yCoord )
	{
		// Get the prefab information
		GameObject spawnPrefab = Globals.prefabList[ spawnString ];
		// Spawn the prefab at the particular co-ordiante
		GameObject spawnItem = (GameObject)Instantiate( spawnPrefab, new Vector3( spawnPosition.x, spawnPosition.y, transform.position.z ), Quaternion.identity );

		spawnItem.transform.parent = spawnParent.transform;

		SetCoordsForPrefab( spawnString, spawnItem, xCoord, yCoord);

		return spawnItem;
	}

	/**
	 * This method is the same as it's predecessor, it just also controls the alpha value of said prefab.
	 * */
	public GameObject SpawnPrefabAtCoords( string spawnString, Vector2 spawnPosition, GameObject spawnParent, int xCoord, int yCoord, float alpha )
	{
		// Get the prefab information
		GameObject spawnPrefab = Globals.prefabList[ spawnString ];
		// Spawn the prefab at the particular co-ordiante
		GameObject spawnItem = (GameObject)Instantiate( spawnPrefab, new Vector3( spawnPosition.x, spawnPosition.y, transform.position.z ), Quaternion.identity );

		spawnItem.transform.parent = spawnParent.transform;

		SetCoordsForPrefab( spawnString, spawnItem, xCoord, yCoord);

		// Set the alpha component of the new object
		SpriteRenderer spawnSprite = spawnItem.GetComponent<SpriteRenderer>();

		if ( spawnSprite != null )
		{
			Color spawnColor = spawnSprite.color;
			spawnSprite.material.color = new Color( spawnColor.r, spawnColor.g, spawnColor.b, alpha );
//			spawnItem.GetComponent<SpriteRenderer>().material.color.a = alpha;
		}
		else {
			Debug.Log("SpawnPrefabAtCoords() was asked to change the alpha of a game object that had no sprite renderer.");
		}

		return spawnItem;
	}

	/**
	 * This method is the same as it's predecessor, it just also controls the alpha value of said prefab, and also not setting to a parent, resulting in the default hierarchy
	 * */
	public GameObject SpawnPrefabAtCoords( string spawnString, Vector2 spawnPosition, int xCoord, int yCoord, float alpha )
	{
		// Get the prefab information
		GameObject spawnPrefab = Globals.prefabList[ spawnString ];
		// Spawn the prefab at the particular co-ordiante
		GameObject spawnItem = (GameObject)Instantiate( spawnPrefab, new Vector3( spawnPosition.x, spawnPosition.y, transform.position.z ), Quaternion.identity );

		SetCoordsForPrefab( spawnString, spawnItem, xCoord, yCoord);
		
		// Set the alpha component of the new object
		SpriteRenderer spawnSprite = spawnItem.GetComponent<SpriteRenderer>();
		
		if ( spawnSprite != null )
		{
			Color spawnColor = spawnSprite.color;
			spawnSprite.material.color = new Color( spawnColor.r, spawnColor.g, spawnColor.b, alpha );
			//			spawnItem.GetComponent<SpriteRenderer>().material.color.a = alpha;
		}
		else {
			Debug.Log("SpawnPrefabAtCoords() was asked to change the alpha of a game object that had no sprite renderer.");
		}
		
		return spawnItem;
	}

	public void SpawnPrefabOnlyHorizontal( string spawnString, Vector2 spawnPosition, int distance, int height, GameObject spawnParent )
	{
		// Builds the same number horizontal in both directions from the center
		for ( int width = distance; width > 0; width-- )
		{
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x + ( Globals.spriteWidth * width ), spawnPosition.y ), spawnParent, (int)spawnPosition.x + width, (int)spawnPosition.y + height );
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x - ( Globals.spriteWidth * width ), spawnPosition.y ), spawnParent, (int)spawnPosition.x + -width, (int)spawnPosition.y + height );
		}
		// Spawn the center point
		SpawnPrefabAtCoords( spawnString, spawnPosition, spawnParent, 0, height );
		
	}

	// Variation of the method that allows the control of whether or not the center tile will be spawned
	public void SpawnPrefabOnlyHorizontal( string spawnString, Vector2 spawnPosition, int distance, int height, GameObject spawnParent, bool center )
	{
		// Builds the same number horizontal in both directions from the center
		for ( int width = distance; width > 0; width-- )
		{
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x + ( Globals.spriteWidth * width ), spawnPosition.y ), spawnParent, (int)spawnPosition.x + width, (int)spawnPosition.y + height );
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x - ( Globals.spriteWidth * width ), spawnPosition.y ), spawnParent, (int)spawnPosition.x + -width, (int)spawnPosition.y + height );
		}
		// Spawn the center point
		if (center)
			SpawnPrefabAtCoords( spawnString, spawnPosition, spawnParent, 0, height );

	}

	// Variation of the method that allows the setting of the prefabs alpha
	public void SpawnPrefabOnlyHorizontal( string spawnString, Vector2 spawnPosition, int distance, int height, GameObject spawnParent, bool center, float alpha )
	{
		// Builds the same number horizontal in both directions from the center
		for ( int width = distance; width > 0; width-- )
		{
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x + ( Globals.spriteWidth * width ), spawnPosition.y ), spawnParent, (int)spawnPosition.x + width, (int)spawnPosition.y + height, alpha );
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x - ( Globals.spriteWidth * width ), spawnPosition.y ), spawnParent, (int)spawnPosition.x + -width, (int)spawnPosition.y + height, alpha );
		}
		// Spawn the center point
		if (center)
			SpawnPrefabAtCoords( spawnString, spawnPosition, spawnParent, 0, height, alpha );
		
	}

	// I'm going to need a method that builds only vertically
	public void SpawnPrefabOnlyVertical( string spawnString, Vector2 spawnPosition, int distance )
	{
		// Builds the same number vertical in both directions from the center
	}

	/**
	 * This method exists for when there is an uneven amount that needs to be built up / down
	 * */
	public void SpawnPrefabSomeVertical( string spawnString, Vector2 spawnPosition, int upDistance, int downDistance, int width, GameObject spawnParent )
	{
		// Builds upDistance up from the spawnPosition, and downDistance down
		for ( int up = upDistance; up > 0; up-- )
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x, spawnPosition.y + ( Globals.spriteHeight * up ) ), spawnParent, width, up );
		for ( int down = downDistance; down > 0; down-- )
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x, spawnPosition.y - ( Globals.spriteHeight * down ) ), spawnParent, width, -down );
		// Spawn center Tile
		SpawnPrefabAtCoords( spawnString, spawnPosition, spawnParent, width, 0 );
	}

	/**
	 * This method exists for when there is an uneven amount that needs to be build left / right
	 * */
	public void SpawnPrefabSomeHorizontal( string spawnString, Vector2 spawnPosition, int leftDistance, int rightDistance, int height, GameObject spawnParent )
	{
		for ( int left = leftDistance; left > 0; left-- )
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x - ( Globals.spriteWidth * left ), spawnPosition.y ), spawnParent, -left, height );
		for ( int right = rightDistance; right > 0; right-- )
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x + ( Globals.spriteWidth * right ), spawnPosition.y ), spawnParent, right, height );
		// Spawn center Tile
		SpawnPrefabAtCoords( spawnString, spawnPosition, spawnParent, 0, height );
	}

	// Spawns prefabs in a line of the direction of your choosing
	// This method doe NOT spawn on the center tile. It uses that a basis of where to start, but does not spawn on that point
	public void SpawnPrefabInLine( string spawnString, Vector2 spawnPosition, Vector2 direction, int distance, GameObject spawnParent )
	{
		for ( int i = 1; i <= distance; i++ )
		{
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x + ( direction.x * i * Globals.spriteWidth ), spawnPosition.y + ( direction.y * i * Globals.spriteHeight ) ), spawnParent,
			                   (int)spawnPosition.x + (int)direction.x * i, (int)spawnPosition.y + (int)direction.y * i );
		}
	}

	// Spawns prefabs in a line of the direction of your choosing
	// This method does NOT spawn on the center tile. It uses that a basis of where to start, but does not spawn on that point
	// Variation of the above method that controls the alpha of the spawned prefab
	public void SpawnPrefabInLine( string spawnString, Vector2 spawnPosition, Vector2 direction, int distance, GameObject spawnParent, float alpha )
	{
		for ( int i = 1; i <= distance; i++ )
		{
			SpawnPrefabAtCoords( spawnString, new Vector2( spawnPosition.x + ( direction.x * i * Globals.spriteWidth ), spawnPosition.y + ( direction.y * i * Globals.spriteHeight ) ), spawnParent,
			                    (int)spawnPosition.x + (int)direction.x * i, (int)spawnPosition.y + (int)direction.y * i, alpha );
		}
	}

	// Spawns prefab in a cross shape, given a distance.
	public void SpawnPrefabInCross( string spawnString, Vector2 spawnPosition, int distance, GameObject spawnParent )
	{
		// I'll want another method that utilizes spawning prefabs in a line
		SpawnPrefabInLine( spawnString, spawnPosition, Globals.NORTH, distance, spawnParent );
		SpawnPrefabInLine( spawnString, spawnPosition, Globals.SOUTH, distance, spawnParent );
		SpawnPrefabInLine( spawnString, spawnPosition, Globals.EAST, distance, spawnParent );
		SpawnPrefabInLine( spawnString, spawnPosition, Globals.WEST, distance, spawnParent );
	}

	// Spawns prefab in a cross shape, given a distance.
	// Variation of the above method that controls the alpha of the spawned prefab
	public void SpawnPrefabInCross( string spawnString, Vector2 spawnPosition, int distance, GameObject spawnParent, float alpha )
	{
		// I'll want another method that utilizes spawning prefabs in a line
		SpawnPrefabInLine( spawnString, spawnPosition, Globals.NORTH, distance, spawnParent, alpha );
		SpawnPrefabInLine( spawnString, spawnPosition, Globals.SOUTH, distance, spawnParent, alpha );
		SpawnPrefabInLine( spawnString, spawnPosition, Globals.EAST, distance, spawnParent, alpha );
		SpawnPrefabInLine( spawnString, spawnPosition, Globals.WEST, distance, spawnParent, alpha );
	}

	/**
	 * This method spawns a diamond grid, which is my name for a movement grid type shape.
	 * 
	 * It utilizes SpawnPrefabInCross and SpawnPrefabOnlyHorizontal, ommitting the center value
	 * */
	public void SpawnPrefabInDiamondGrid( string spawnString, Vector2 spawnPosition, int distance, GameObject spawnParent )
	{

		// I'll want another method that will spawn prefabs in a cross
		SpawnPrefabInCross( spawnString, spawnPosition, distance, spawnParent );

		for ( int row = distance - 1, amount = 1; row > 0; row--, amount++ )
		{
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( spawnPosition.x, spawnPosition.y + (row * Globals.spriteHeight) ), amount, row, spawnParent, false );
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( spawnPosition.x, spawnPosition.y - (row * Globals.spriteHeight) ), amount, -row, spawnParent, false );
		}

		// Spawn the center tile
		SpawnPrefabAtCoords( spawnString, spawnPosition, spawnParent );

	}

	/**
	 * This is a variation method for SpawnPrefabInDiamondGrid that allows the set of the alpha of the spawning prefab
	 * */
	public void SpawnPrefabInDiamondGrid( string spawnString, Vector2 spawnPosition, int distance, GameObject spawnParent, float alpha )
	{
		
		// I'll want another method that will spawn prefabs in a cross
		SpawnPrefabInCross( spawnString, spawnPosition, distance, spawnParent, alpha );
		
		for ( int row = distance - 1, amount = 1; row > 0; row--, amount++ )
		{
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( spawnPosition.x, spawnPosition.y + (row * Globals.spriteHeight) ), amount, row, spawnParent, false, alpha );
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( spawnPosition.x, spawnPosition.y - (row * Globals.spriteHeight) ), amount, -row, spawnParent, false, alpha );
		}
		
		// Spawn the center tile
		SpawnPrefabAtCoords(spawnString, spawnPosition, spawnParent, (int)spawnPosition.x, (int)spawnPosition.y, alpha);
		
	}

	/**
	 * This is a variation method for SpawnPrefabInDiamondGrid that allows the set of the alpha of the spawning prefab
	 * 
	 * It also takes in a bool that allows a restriction to be set of whether or not it can only be spawned if a tile is at that co-ordinate.
	 * */
	public void SpawnPrefabInDiamondGrid( string spawnString, Vector2 spawnPosition, int distance, GameObject spawnParent, float alpha, bool onTile )
	{

		Dictionary< Vector2, Tile > tempTileList = new Dictionary<Vector2, Tile>();

		// I'll want another method that will spawn prefabs in a cross
		SpawnPrefabInCross( spawnString, spawnPosition, distance, spawnParent, alpha );
		
		for ( int row = distance - 1, amount = 1; row > 0; row--, amount++ )
		{
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( spawnPosition.x, spawnPosition.y + (row * Globals.spriteHeight) ), amount, row, spawnParent, false, alpha );
			SpawnPrefabOnlyHorizontal( spawnString, new Vector2( spawnPosition.x, spawnPosition.y - (row * Globals.spriteHeight) ), amount, -row, spawnParent, false, alpha );
		}
		
		// Spawn the center tile
		SpawnPrefabAtCoords(spawnString, spawnPosition, spawnParent, (int)spawnPosition.x, (int)spawnPosition.y, alpha);
		
	}
	
	private void SetCoordsForPrefab( string spawnString, GameObject spawnItem, int x, int y )
	{
		Vector2 spawnPosition = new Vector2( x, y );

		switch( spawnString )
		{
		case "Tile":
			Tile tileScript = spawnItem.GetComponent<Tile>();
			tileScript.coords = spawnPosition;
			// Add the Tile to the global Dictionary
			Globals.TileList.Add( spawnPosition, spawnItem );
			break;
		case "Player":
			Player playerScript = spawnItem.GetComponent<Player>();
			playerScript.coords = spawnPosition;
			break;
		case "Cursor":
			Cursor cursorScript = spawnItem.GetComponent<Cursor>();
			cursorScript.coords = spawnPosition;
			break;
		case "PlayerMoveTile":
		case "EnemyMoveTile":
			MovementGrid moveScript = spawnItem.GetComponent<MovementGrid>();
			moveScript.coords = spawnPosition;
			break;
		default:
			Debug.Log("A class was sent into SetCoordsForPrefab without a way to set it's co-ordinate value.");
			break;
		}
	}
}

































