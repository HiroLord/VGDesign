﻿using UnityEngine;
using System.Collections;

public abstract class EnemyNetwork : Entity {

	public Transform currTarget;
	public int currTargetID;
	public bool original = true;

	public abstract void SetFromEState(int state);
	public abstract bool getChangedState();
	public abstract int getEState();

	private bool needUpdate;
	private float updateTimer = 0f;

	protected void Update() {
		if (updateTimer > 90f * Time.deltaTime) {
			needUpdate = true;
			updateTimer = 0f;
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
