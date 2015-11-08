/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;

public abstract class State<T> 
{
	// The State will have the object that is within the current state, and the state machine	
		// that if follows
	protected T ownerObject;
	protected StateMachine<T> ownerStateMachine;

	// Every state will check and update
	public abstract void CheckForNewState();

	public abstract void Update();

	public virtual void OnEnable(T owner, StateMachine<T> newStateMachine)
	{
		ownerObject = owner;
		ownerStateMachine = newStateMachine;
	}

	// Basis to follow when the state is over
	public virtual void onDisable()
	{

	}
}