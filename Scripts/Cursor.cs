using UnityEngine;
using System.Collections;
using AssemblyCSharp;

/**
 * I want this class to be one of, if not the only class, with an Update() method in it. Those methods are expensive, and I want to limit them whenever possible.
 * */
public class Cursor : MonoBehaviour {

	public static Cursor instance;

	private Vector2 _coords;
	public Vector2 coords
	{
		get
		{
			return _coords;
		} set {
			_coords = value;
			StartCoroutine( MovedTime() );
		}
	}
	private bool moved;
	LayerMask onMap;
	int mask;

	private void Awake()
	{
		if ( instance == null )
			instance = this;

		moved = true;

		onMap = LayerMask.NameToLayer( "OnMap" );
		onMap = ~onMap;

		mask = 1 << 8;
	}

	private void Update()
	{
//		Debug.Log(moved);
		if ( moved )
		{
			// Left
			if ( Input.GetKey( Globals.Controls.cursorLeftMain ) || Input.GetKey( Globals.Controls.cursorLeftSecondary ) )
			{
				/**
				 * We need to make a new Vector2 that involves subtracting one from x
				 * 
				 * And then check if that is in the dictionary, and if so, update the position of cursor
				 * */

				UpdatePosition( Globals.WEST );
			}
			// Right
			else if ( Input.GetKey( Globals.Controls.cursorRightMain ) || Input.GetKey( Globals.Controls.cursorRightSecondary ) )
			{
				UpdatePosition( Globals.EAST );
			}
			// Up
			else if ( Input.GetKey( Globals.Controls.cursorUpMain ) || Input.GetKey( Globals.Controls.cursorUpSecondary ) )
			{
				UpdatePosition( Globals.NORTH );
			}
			// Down
			else if ( Input.GetKey( Globals.Controls.cursorDownMain ) || Input.GetKey( Globals.Controls.cursorDownSecondary ) )
			{
				UpdatePosition( Globals.SOUTH );
			}
			// Affirmative
			else if ( Input.GetKeyDown( Globals.Controls.cursorAffirmativeMain ) || Input.GetKeyDown( Globals.Controls.cursorAffirmativeSecondary ) )
			{
//				Debug.Log("Click");
				AnalyzeUnderCursor();
			}
		}

	}

	/**
	 * As the cursor is always going to be following the mouse, it's necessary to use a Physics2D raycaster to determine what you're clicking on
	 * */
	private void OnMouseDown()
	{
		AnalyzeUnderCursor();

	}

	private void AnalyzeUnderCursor()
	{
		// TODO: I set this to Vector2.zero to fix a bug, I don't know if it will have unintended consequences with my raycasting in the future
		RaycastHit2D[] hit = Physics2D.RaycastAll( this.transform.position, Vector2.zero, 2f, mask );

		if ( hit.Length == 0 )
			return;

		if ( hit[0].collider != null )
		{
			//			print(hit[1].collider.gameObject.name);
			AnalyzeRayClass( hit[0].collider.gameObject );
		}
	}

	/**
	 * This method analyzes the result of a RaycastHit2D collider's gameobject
	 * 
	 * And then it determines whether or not that particular object would implement the IClickable interface, and then calls it
	 * */
	private void AnalyzeRayClass( GameObject hitObject )
	{
		// this method takes in the string of the object hit, determining if gameobject has an IClickable method attached to it
		string tag = hitObject.tag;
		// I need to adjust the name to be a valid switch state expression. Meaning I need to analyze it, and take off the (clone) bit
//		tag = RemoveClone( tag );

		switch( tag )
		{
			case "Player":
				Player playerScript = hitObject.GetComponent<Player>();
				if ( !playerScript.moveGridSpawned )
					playerScript.OnMouseClick();
				else
					return;
				break;
			// TODO: Make something happen when you click enemies
			case "Enemy":
				break;
			case "MoveTile":
				MovementGrid moveScript = hitObject.GetComponent<MovementGrid>();
				moveScript.OnMouseClick();
			break;
			default:
				Debug.Log("AnalyzeRayClass was given an object that did not implement the IClickable interface.");
				Debug.Log("It was given: " + tag);
				break;
		}
	}

	/**
	 * A simple helper method for removing the "(Clone)" from a strings name
	 * */
	private string RemoveClone( string name )
	{
		int index = name.IndexOf( "(" );
		return name.Remove(index);
	}

	/**
	 * Returns true if there is a valid Tile to move in that direction
	 * 
	 * Returns false if there not
	 * */
	private bool CheckMovement( Vector2 direction, out Vector2 checkVector )
	{
		checkVector = new Vector2( this.coords.x + direction.x, this.coords.y + direction.y );

		// If a movement grid is spawned, check to see if there's a movement grid at that position
		if ( Globals.moveGridSpawned )
		{
			if ( Globals.moveGridList.ContainsKey( checkVector ) )
				return true;
			else
				return false;
		} else { // Otherwise just check for a regular tile at that position
			if ( Globals.tileList.ContainsKey( checkVector ) )
				return true;
			else
				return false;
		}
	}

	private void UpdatePosition( Vector2 direction )
	{
		Vector2 checkVector;

		if ( CheckMovement( direction, out checkVector ) )
		{
			// Update cursor position
			this.gameObject.transform.position = Globals.tileList[ checkVector ].transform.position;
			this.coords = checkVector;
		}
	}

	private IEnumerator MovedTime()
	{
		moved = false;
		yield return new WaitForSeconds(0.075f);
		moved = true;
	}

}





































