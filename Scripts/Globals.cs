using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public static class Globals {

	/**
	 * Prototype #1 Goals:
	 * 
	 * - Store any value that needs to be accessed across multiple scripts.
	 * 
	 * */

	public static class Controls
	{
		public static KeyCode cursorLeftMain;
		public static KeyCode cursorLeftSecondary;
		public static KeyCode cursorRightMain;
		public static KeyCode cursorRightSecondary;
		public static KeyCode cursorUpMain;
		public static KeyCode cursorUpSecondary;
		public static KeyCode cursorDownMain;
		public static KeyCode cursorDownSecondary;

		public static KeyCode cursorAffirmativeMain;
		public static KeyCode cursorAffirmativeSecondary;

		/**
		 * This method exists only to set the default controls, if the user hasn't set their own.
		 * */
		public static void SetDefaultControls()
		{
			cursorLeftMain = 		KeyCode.LeftArrow;
			cursorLeftSecondary = 	KeyCode.A;
			cursorRightMain = 		KeyCode.RightArrow;
			cursorRightSecondary = 	KeyCode.D;
			cursorUpMain = 			KeyCode.UpArrow;
			cursorUpSecondary = 	KeyCode.W;
			cursorDownMain = 		KeyCode.DownArrow;
			cursorDownSecondary = 	KeyCode.S;

			cursorAffirmativeMain = 		KeyCode.Return;
			cursorAffirmativeSecondary = 	KeyCode.KeypadEnter;
		}
	}

	public static class Turn
	{
		public static bool PLAYER_TURN = true;
		public static bool ENEMY_TURN = false;
		public static bool _characterTurn;
		public static bool characterTurn
		{
			get
			{
				return _characterTurn;
			} set
			{
				if ( value == _characterTurn )
					Debug.Log( "Turn attempted to set itself to the value it already is. This needs to be investigated." );
				if ( value == true )
				{
					_characterTurn = true;
					CallNewPlayerTurn();
					ChangeTurn();
				} else // value == false
				{
					_characterTurn = false;
					EnemyHost.instance.EnemyTurn();
//					characterTurn = Globals.Turn.PLAYER_TURN;
//					CallNewEnemyTurn();
				}
			}
		}
		public static uint _counter;
		public static uint counter
		{
			// public get private set
			get
			{
				return _counter;
			}
			private set
			{
				// set counter
				_counter = value;
				// Update visual counter in game
				UpdateTurn();
			}
		}
		private static void UpdateTurn()
		{
			Globals.turnText.text = _counter.ToString();
		}
		// This is for the initial setting of Turn.
		// Change turn is for every supsequent Turn.
		public static void SetTurn( uint turn )
		{
			counter = turn;
		}
		// This variable is set outside of Globals, but is only referenced inside of it
		// Used to determine whether turns are counting up or down
		// It's defaultly false, which is the most likely situation of a level, that turns will be counted upwards, and not down
		public static bool downCount
		{
			// private get public set
			private get;
			set;
		}
		private static uint ChangeTurn()
		{
			if ( downCount )
				counter -= 1;
			else
				counter += 1;
			return counter;
		}
		public delegate void PlayerTurn();
		public static event PlayerTurn NewPlayerTurn;
		private static void CallNewPlayerTurn()
		{
			// I only want to call the event from inside this static class
			NewPlayerTurn();
		}
//		public delegate void EnemyTurn();
//		public static event PlayerTurn NewEnemyTurn;
//		private static void CallNewEnemyTurn()
//		{
//			// I only want to call the event from inside this static class
//			// TODO: Determine if you need a delegate for enemy AI when you have EnemyHost doing so much work
////			NewEnemyTurn();
//		}
	} // Turn
	
	// Cardinal directions
	public static Vector2 NORTH = Vector2.up;
	public static Vector2 SOUTH = -Vector2.up;
	public static Vector2 EAST = -Vector2.left;
	public static Vector2 WEST = Vector2.left;
	// Diagonal directions
	public static Vector2 NORTHEAST = Vector2.one;
	public static Vector2 SOUTHEAST = new Vector2( 1, -1 );
	public static Vector2 SOUTHWEST = -Vector2.one;
	public static Vector2 NORTHWEST = -SOUTHEAST; // -1, 1

	public static bool moveGridSpawned;

	public static float spriteWidth;
	public static float spriteHeight;

	public static int mapWidthLimit = 15;
	public static int mapHeightLimit = 15;

	// A list of all the prefabs in the project
	public static Dictionary< string, GameObject > prefabList;
	private static bool prefabsRead;
	/**
	 * This method iterates through all the files in the prefab folder and then adds them to the list which I can access later.
	 * */
	public static void ReadPrefabs()
	{
		if ( !prefabsRead )
		{
			prefabsRead = true;
			prefabList = new Dictionary< string, GameObject >();

			string path = Application.dataPath + "/Resources/Prefabs/";
			DirectoryInfo directory = new DirectoryInfo( path );
			FileInfo[] info = directory.GetFiles();

			foreach( FileInfo file in info )
			{
				// To ignore the .meta files in the folder
				if ( file.Extension == ".prefab" )
				{
					string fileName = Path.GetFileNameWithoutExtension( file.Name );
					prefabList.Add( fileName, (GameObject)Resources.Load( "Prefabs/" + fileName ) );
				}
			}
		}
	}

	// This will be the parent object under which all the Tiles of the map will be spawned
	public static GameObject tileHost;
	public static GameObject enemyHost;
	public static Text turnText;
	public static Dictionary< Vector2, GameObject > tileList;
	public static List<Player> playerList;
	public static List<Enemy> enemyList;
	// TODO: When the NPC class is designed, uncomment this next line
//	public static List<NPC> NPCList;
	public static Dictionary< Vector2, GameObject > switchList;

	public static Dictionary< Vector2, GameObject > moveGridList;

	public static void InstantiateDictionaries()
	{
		tileList 	 = new Dictionary< Vector2, GameObject >();
		playerList   = new List<Player>();
		enemyList    = new List<Enemy>();
		// TODO: When the NPC class is designed, uncomment this next line
//		NPCList      = new List<NPC>();
		switchList 	 = new Dictionary< Vector2, GameObject >();
		moveGridList = new Dictionary< Vector2, GameObject >();

		// TODO: Hide this next line in a different method somewhere
		turnText = GameObject.Find("Turn").GetComponent<Text>();
	}

	public static bool IfPlayerAtCoords( Vector2 coords )
	{
		foreach( Player player in playerList )
		{
			if ( player.coords == coords )
			{
				return true;
			}
		}

		return false;
	}

	public static bool IfEnemyAtCoords( Vector2 coords )
	{
		foreach( Enemy enemy in enemyList )
		{
			if ( enemy.coords == coords )
			{
				return true;
			}
		}
		
		return false;
	}

	// Returns true is all players have finished their round
	// Returns false if any player has their round still available
	public static bool CheckPlayerRounds()
	{
		foreach( Player player in playerList )
		{
			if ( player.round ) // is true
				return false;
		}

		// If all player.round's are false
		return true;
	}

	/**
	 * This method is here purely to help set the various parent objects I'd like to keep track of.
	 * */
	public static void SetParentObjects()
	{
		tileHost = GameObject.Find("TileHost");
		enemyHost = GameObject.Find("EnemyHost");
	}

}












































