using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	private EventSystem system;

	private void Start ()
	{
		system = EventSystem.current;
	}

	private void Update ()
	{
		if ( Input.GetKeyDown(KeyCode.Tab) )
		{
			Selectable next = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ?
				system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp() : // On LeftShift
				system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();// On RightShift

			if ( next != null )
			{
				InputField inputField = next.GetComponent<InputField>();
				if ( inputField != null )
				{
					NextInputField( next, inputField );
				}
				else
				{
					next = Selectable.allSelectables[0];
					system.SetSelectedGameObject( next.gameObject );
				}
			}
		} else if ( Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) )
		{
			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

			if ( next != null )
			{
				InputField inputField = next.GetComponent<InputField>();

				if ( inputField != null )
				{
					NextInputField( next, inputField );
				}
			}
		}
	}

	private void NextInputField( Selectable next, InputField inputField )
	{
		// If it's an input field, also set the text caret
		inputField.OnPointerClick( new PointerEventData( system ) );
		
		EventSystem.current.SetSelectedGameObject( next.gameObject, new BaseEventData( system ) );
	}
}


