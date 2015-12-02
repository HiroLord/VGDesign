using UnityEngine;
using System.Collections;

public abstract class EnemyNetwork : Entity {

	public Transform currTarget;
	public int currTargetID;
	public bool original = true;

	public abstract void SetFromEState(int state);
	public abstract bool getChangedState();
	public abstract int getEState();

}
