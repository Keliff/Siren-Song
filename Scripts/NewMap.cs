using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class NewMap : MonoBehaviour {

	public GameObject canvas;

	public InputField xInput;
	public InputField yInput;

	public InputManager im;
	
	// This is the onEndEdit method for yInput, as that is who this script will attach to
	public void OnEndEdit()
	{
		// "" is what the InputField.text value has if the user has entered nothing.
		if ( xInput.text == "" )
			return;
		if ( yInput.text == "" )
			return;

		int xNum = int.Parse( xInput.text );
		int yNum = int.Parse( yInput.text );
		
		Map.instance.CreateBlankMap( "Tile", xNum, yNum );

		Destroy( canvas );
	}

	private void OnEnable()
	{
		im.enabled = true;
	}

	private void OnDisable()
	{
		im.enabled = false;
	}
	
	/**
	 * So what we need to accomplish is this:
	 * 
	 * - Get the value the user entered from the X input inputfield
	 * - Get the value from the Y inputfield
	 * - Take both and use those two numbers to form the map, also getting rid of the UI Canvas as well, as we won't need to make another one
	 * 
	 * 
	 * */
	
}
