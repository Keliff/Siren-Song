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
			if ( value ) // if true
			{
				_action = true;
			} else // if false
			{
				_action = false;
				if ( !_movement ) // if also false
					round = false;
				// End the Round
				return;
			}
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
			if ( value ) // if true
			{
				_movement = true;
			} else // if false
			{
				_movement = false;
				if ( !_action ) // if also false
					round = false;
				// End the Round
				return;
			}
		}
			
	}
	public bool round
	{
		get
		{
			return _round;
		}
		set
		{
			if ( value == false )
			{
				_round = false;
				EndRound();
			}
			else
				_round = true;
		}
	}
	private Vector2 newTurnPosition;
	private bool _action;
	private bool _movement;
	private bool _round;
	
	// TODO: Actually set the player instance somewhere in the script
	private void Awake()
	{
		Globals.playerList.Add( this );
		Globals.Turn.NewPlayerTurn += NewTurn;
	}

	private void OnDestroy()
	{
		Globals.Turn.NewPlayerTurn -= NewTurn;
	}

	// Whenever it's a new turn for the Player, what values are necessary to set
	public void NewTurn()
	{
		print("New turn called: " + this.name);
		_movement = true;
		_action = true;
		// This is basically asking if this is the first round of the player, or a subsequent one
		if ( round == false )
		{
			round = true;
			// Flip Grey state
		}
	}


	private void EndRound()
	{
		_round = false;
		// If all players have finished their round
		if ( Globals.CheckPlayerRounds() )
		{
			// Then change to enemy turn
			StartCoroutine( WaitForEnemyTurn() );
		}
		// Flip Grey state

	}

	IEnumerator WaitForEnemyTurn()
	{
		yield return new WaitForSeconds( 0.75f );
		Globals.Turn.characterTurn = Globals.Turn.ENEMY_TURN;
	}

	public void DeleteMovementGrid()
	{
		print("DELETING " + this.name + " 's MOVEMENT GRID ");
		foreach ( Transform moveGrid in PlayerHost.instance.moveTileHost.transform )
		{
			Destroy( moveGrid.gameObject );
		}

		Globals.moveGridSpawned = false;
		Globals.moveGridList.Clear();
		this.movement = false;
	}

	#region IClickable implementation
		
	public void OnMouseClick ()
	{
//		print("Clicked on Player");
//		print(movement);

		// Whenever a Player is clicked, they are the current active Player
		instance = this;

		if ( movement && !Globals.moveGridSpawned )
		{
			// TODO: Make any restriction / thing you send into a spawn prefab a constant stored in globals
			// We want no magic numbers / text if we can avoid it
			Spawner.instance.SpawnPrefabInDiamondGrid( "PlayerMoveTile", this.coords,
			                                           this.moveTiles, PlayerHost.instance.moveTileHost, 0.5f, "MOVETILE" );
			Globals.moveGridSpawned = true;
		}
		//		throw new System.NotImplementedException ();
	}
	
	#endregion

}




































