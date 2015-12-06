using UnityEngine;
using System.Collections;

public abstract class EnemyNetwork : Entity {

	public Transform currTarget;
	private int currTargetID;
	private bool targetHasChanged;
	public int CurrTargetID{
		get {
			return currTargetID;
		}
		set {
			currTargetID = value;
			targetHasChanged = true;
		}
	}
	public bool original = true;

	public abstract void SetFromEState(int state);
	public abstract bool getChangedState();
	public abstract int getEState();

	private bool needUpdate;
	private float updateTimer = 0f;


	protected void Start() {
		GameObject obj = GameObject.Find ("NetworkManager");
		if (obj) {
			obj.GetComponent<NetworkManager> ().AddEnemy (this);
		}
		currentHealth = 0;
		changeHealthHard (-startingHealth);
		currentEnergy = startingEnergy;

	}

	void OnDestroy() {
		//GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ().RemoveEnemy (this);
	}

	protected void Update() {
		if (original) {
			if (updateTimer > 90f * Time.deltaTime) {
				needUpdate = true;
				updateTimer = 0f;
			}
			updateTimer += Time.deltaTime;
		}
	}

	public bool TargetChanged() {
		if (targetHasChanged) {
			targetHasChanged = false;
			return true;
		}
		return false;
	}

	public bool NeedsUpdate() {
		if (needUpdate) {
			needUpdate = false;
			return true;
		}
		return false;
	}

}
