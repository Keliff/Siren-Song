using UnityEngine;
using System.Collections;

public class PlayerHost : MonoBehaviour {

	public static PlayerHost instance;
	public GameObject moveTileHost;

	private void Awake()
	{
		if ( instance == null )
			instance = this;

		moveTileHost = this.gameObject.transform.FindChild("MoveTileHost").gameObject;
	}

}
