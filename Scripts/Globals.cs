using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

	// TODO: Make the diagonals
	public static Vector2 NORTH = Vector2.up;
	public static Vector2 SOUTH = -Vector2.up;
	public static Vector2 EAST = -Vector2.left;
	public static Vector2 WEST = Vector2.left;

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
	public static GameObject TileHost;
	public static Dictionary< Vector2, GameObject > TileList;

	/**
	 * This method is here purely to help set the various parent objects I'd like to keep track of.
	 * */
	public static void SetParentObjects()
	{
		TileHost = GameObject.Find("TileHost");
	}

}












































