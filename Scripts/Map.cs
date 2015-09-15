using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	/**
	 * Goals for Prototype #1
	 * 
	 * - Be the class to call the Spawner methods to help create the Map
	 * 
	 * */
	public static Map instance;

	/**
	 * Map is a Class that will always be present, so it makes sense to help set the Globals values here before the game uses them.
	 * */
	private void Awake()
	{
		// Need to instantiate the Dictionary
		Globals.TileList = new Dictionary< Vector2, GameObject>();
		Globals.SetParentObjects();
		Globals.Controls.SetDefaultControls();
	}

	private void Start()
	{
		if ( instance == null )
			instance = this;

//		CreateBlankMap( "Tile", 6, 3 );
//		Globals.spriteHeight = ;
//		Globals.spriteWidth = ;
//		Spawner.instance.SpawnPrefabInLine( "Tile", new Vector2(0, 0), Globals.NORTH, 3, Globals.TileHost );
//		Spawner.instance.SpawnPrefabInDiamondGrid( "Tile", new Vector2( -1, -4 ), 3, Globals.TileHost, 0.5f );

	}

	public void CreateBlankMap( string spawnString, int x, int y )
	{
		// spawnString is the prefab
		// x is the map width
		// y is the map height

		// Check the x and y against the limits I have set
		if ( x > Globals.mapWidthLimit )
			x = Globals.mapWidthLimit;
		if ( y > Globals.mapHeightLimit )
			y = Globals.mapHeightLimit;

		if ( IsEven( x ) == false )
		{
			if ( IsEven( y ) == false )
				BuildFullOddMap( spawnString, x, y );
			else // y is even
				BuildPartialOddMap( spawnString, x, y );
		} else // x is even
		{
			if ( IsEven( y ) )
				BuildFullEvenMap( spawnString, x, y );
			else // y is odd
				BuildPartialEvenMap( spawnString, x, y );
		}

		// TEST
//		Spawner.instance.SpawnPrefabInDiamondGrid( "Tile", new Vector2( 0, 0 ), 3, Globals.TileHost );

		// Spawning the Cursor once the map has been formed
		Spawner.instance.SpawnPrefabAtCoords( "Cursor", new Vector2( 0, 0 ), 0, 0, 0.5f );
		Spawner.instance.SpawnPrefabAtCoords( "Player", new Vector2( 0, 0 ), 0, 0, 1f );

	}

	// When both are odd
	private void BuildFullOddMap( string spawnString, int x, int y )
	{
		x = x / 2;
		y = y / 2;

		for ( int height = y; height > 0; height-- )
		{
			Spawner.instance.SpawnPrefabOnlyHorizontal( spawnString, new Vector2( transform.position.x, transform.position.y + ( Globals.spriteHeight * height ) ), x, height, Globals.TileHost);
			// -height works here because it's used only to help set the co-ordinate of the spawned object, nothing more. It's not used in the calculation of anything.
			Spawner.instance.SpawnPrefabOnlyHorizontal( spawnString, new Vector2( transform.position.x, transform.position.y - ( Globals.spriteHeight * height ) ), x, -height, Globals.TileHost);
		}
		// Build the center Line
		Spawner.instance.SpawnPrefabOnlyHorizontal( spawnString, new Vector2( transform.position.x, transform.position.y ), x, 0, Globals.TileHost );
	}

	// When x is odd and y is even
	private void BuildPartialOddMap( string spawnString, int x, int y )
	{
		x = (x - 1) / 2;
		y = y / 2;

		for ( int width = x; width > 0; width-- )
		{
			Spawner.instance.SpawnPrefabSomeVertical( spawnString, new Vector2( transform.position.x + ( Globals.spriteWidth * width ), transform.position.y ), y - 1, y, width, Globals.TileHost );
			Spawner.instance.SpawnPrefabSomeVertical( spawnString, new Vector2( transform.position.x - ( Globals.spriteWidth * width ), transform.position.y ), y - 1, y, -width, Globals.TileHost );
		}
		// Build the center line
		Spawner.instance.SpawnPrefabSomeVertical( spawnString, new Vector2( transform.position.x, transform.position.y ), y - 1, y, 0, Globals.TileHost );
	}

	// When both are even
	private void BuildFullEvenMap( string spawnString, int x, int y )
	{
		x = x / 2;
		y = y / 2;

		for ( int left = x; left > 0; left-- )
		{
			Spawner.instance.SpawnPrefabSomeVertical( spawnString, new Vector2( transform.position.x - ( Globals.spriteWidth * left ), transform.position.y), y - 1, y, -left, Globals.TileHost );
		} for ( int right = x - 1; right > 0; right-- )
		{
			Spawner.instance.SpawnPrefabSomeVertical( spawnString, new Vector2( transform.position.x + ( Globals.spriteWidth * right ), transform.position.y), y - 1, y, right, Globals.TileHost );
		}
		Spawner.instance.SpawnPrefabSomeVertical( spawnString, new Vector2( transform.position.x, transform.position.y ), y - 1, y, 0, Globals.TileHost );
	}

	// When x is even and y is odd
	private void BuildPartialEvenMap( string spawnString, int x, int y )
	{
		x = x / 2;
		y = (y -1) / 2;

		for ( int height = y; height > 0; height-- )
		{
			Spawner.instance.SpawnPrefabSomeHorizontal( spawnString, new Vector2( transform.position.x, transform.position.y + ( Globals.spriteHeight * height ) ), x - 1, x, height, Globals.TileHost );
			Spawner.instance.SpawnPrefabSomeHorizontal( spawnString, new Vector2( transform.position.x, transform.position.y - ( Globals.spriteHeight * height ) ), x - 1, x, -height, Globals.TileHost );
		}
		Spawner.instance.SpawnPrefabSomeHorizontal( spawnString, new Vector2( transform.position.x, transform.position.y ), x - 1, x, 0, Globals.TileHost );
	}

	private bool IsEven( int num )
	{
		if ( num % 2 == 0 )
			return true;
		else
			return false;
	}

}



















