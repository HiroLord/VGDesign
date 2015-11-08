/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public class StateMachine<T>
{
	T ownerObject;
	private State<T> currentState;

	public State<T> CurrentState 
	{
		get 
		{
			return currentState;
		}
		set 
		{
			if (currentState != null) 
			{
				currentState.onDisable ();
			}
			currentState = value;

			if (currentState != null) 
			{
				currentState.OnEnable (ownerObject, this);
			}
		}
	}

	public StateMachine(State<T> defaultState, T owner)
	{
		ownerObject = owner;
		currentState = defaultState;
		currentState.OnEnable (ownerObject, this);
	}

	public void Update()
	{
		if(currentState != null)
		{
			//Debug.Log("Stuff?");
			currentState.CheckForNewState ();
			currentState.Update ();
		}
	}
}
