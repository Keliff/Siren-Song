using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Character : MonoBehaviour, IHover
{
	public Vector2 coords;
	public BoxCollider2D boxCollider;
	public int maxHealth;
	public int currentHealth;
	public int moveTiles;
	

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
}
