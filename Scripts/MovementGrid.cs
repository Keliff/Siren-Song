using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MovementGrid : MonoBehaviour, IClickable, IHover {

	public Vector2 coords;
	public string parentType;
	public Player playerSpawner;
//	public GameObject moveGridSpawner;

	#region IClickable implementation
	public void OnMouseClick ()
	{
		UpdateParent();
	}
	#endregion

	#region IHover implementation

	public void OnMouseEnter ()
	{
		if ( Cursor.instance.coords != this.coords )
		{
			Cursor.instance.gameObject.transform.position = this.gameObject.transform.position;
			Cursor.instance.coords = this.coords;
		}
		
		return;
	}
	public void OnMouseExit ()
	{
		return;
		//		throw new System.NotImplementedException ();
	}

	#endregion

	private void UpdateParent()
	{
		if ( parentType == "Player" )
		{
			// TODO: Make all these things happen inside a method of Player instead, so it reads nicer
//			Player playerSpawner = moveGridSpawner.GetComponent<Player>();
			playerSpawner.transform.position = this.transform.position;
			playerSpawner.coords = this.coords;
			playerSpawner.DeleteMovementGrid();
			// Player has no movement left after a movement grid has been chosen and they have changed positions
//			playerSpawner.movement = false;
			// TODO: Get rid of this next line once we have player actions more fleshed out
			// This line is purely for testing purposes, to make the next Turn happen as there is no current way for a player to have an action
			playerSpawner.action = false;

		}
//		else if ( parentType == "Enemy" )
//		{
//			//TODO: Once you make your enemy script this can be uncommented
////			Enemy enemyScript = moveGridSpawner.GetComponent<Enemy>();
////			enemyScript.coords = this.coords;
//		}
	}
}







































