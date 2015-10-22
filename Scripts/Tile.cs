using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using AssemblyCSharp;

public class Tile : MonoBehaviour, IClickable, IHover {

	// Be able to hold the visual aspect of the tile.
	public Sprite tileSprite;
//	public Sprite createSprite;
//	public Sprite playSprite;
	// The co-ordinate of the tile at hand.
	public Vector2 coords;

	// Whether or not a character is allowed to move on said tile
	public bool movement;
	// Whether or not the tile is visible
	// TODO: Make visible a property, setting to true makes sure that the tile is visible, setting to false makes sure that the tile is not visible
	public bool visible;
	// The variable that determines whether or not a switch is actually on this tile, as in stepping on it will activate said switch
	public bool switchOn;

	private void Awake()
	{
		tileSprite = this.GetComponent<SpriteRenderer>().sprite;
		Globals.spriteHeight = tileSprite.bounds.size.x;
		Globals.spriteWidth = tileSprite.bounds.size.y;
	}

	#region IClickable implementation

	public void OnMouseClick ()
	{
		return;
//		throw new System.NotImplementedException ();
	}

	#endregion

	#region IHover implementation
	public void OnMouseEnter ()
	{

		// If no movement grid is currently spawned
		if ( !Globals.moveGridSpawned )
		{
			if ( Cursor.instance.coords != this.coords )
			{
				Cursor.instance.gameObject.transform.position = this.gameObject.transform.position;
				Cursor.instance.coords = this.coords;
			}
		}
	}
	public void OnMouseExit ()
	{
		return;
//		throw new System.NotImplementedException ();
	}
	#endregion
}
