using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Player : Character, IClickable {

	public static Player instance;
	public bool action
	{
		get
		{
			return _action;
		}
		set
		{
			if ( value ) // is true
			{
				_action = true;
				if ( _movement ) // is true
					// End the Round
					return;
			} else
				_action = false;
		}
	}
	public bool movement
	{
		get
		{
			return _movement;
		}
		set
		{
			if ( value )// is true
			{
				_movement = true;
				if ( _action ) // is true
					// End the Round
					return;
			} else
				_movement = false;
		}
			
	}

	private Vector2 newTurnPosition;
	private bool _action;
	private bool _movement;
	private GameObject tileHost;

	private void Awake()
	{
		tileHost = this.gameObject.transform.FindChild("MoveTileHost").gameObject;

		if ( tileHost == null )
			Debug.Log( "Player class was unable to find \"MoveTileHost\" among it's children." );
	}

	#region IClickable implementation
		
	public void OnMouseClick ()
	{
//		print("Clicked on Player");
		Spawner.instance.SpawnPrefabInDiamondGrid( "PlayerMoveTile", new Vector2( this.transform.position.x, this.transform.position.y ),
		                                           this.moveTiles, this.tileHost, 0.5f );
		//		throw new System.NotImplementedException ();
	}
	
	#endregion

}




































