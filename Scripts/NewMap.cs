using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class NewMap : MonoBehaviour {

	public GameObject canvas;

	public InputField xInput;
	public InputField yInput;
	
	// This is the onEndEdit method for yInput, as that is who this script will attach to
	public void OnEndEdit()
	{
		int xNum = int.Parse( xInput.text );
		int yNum = int.Parse( yInput.text );
		
		Map.instance.CreateBlankMap( "Tile", xNum, yNum );

		Destroy( canvas );
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
