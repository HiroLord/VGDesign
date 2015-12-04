using UnityEngine;
using System.Collections;

public abstract class EnemyNetwork : Entity {

	public Transform currTarget;
	public int currTargetID;
	public bool original = true;

	public abstract void SetFromEState(int state);
	public abstract bool getChangedState();
	public abstract int getEState();

	private bool needUpdate;
	private float updateTimer = 0;

	protected void Update() {
		if (updateTimer > 60) {
			needUpdate = true;
		}
		updateTimer += Time.deltaTime;
	}

	public bool NeedsUpdate() {
		if (needUpdate) {
			needUpdate = false;
			return true;
		}
		return false;
	}

}
