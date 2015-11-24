using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	bool playingOpening;
	public GameObject mainMenu;
	public GameObject bingBang;
	Vector3 endPosition;
	public Text[] menu;
	int currentSelection;
	string oldCopy;
	float sizeTimer = 0f;
	bool getBigger = true;

	public GameObject credits;
	GameObject createdCredits;

	// Use this for initialization
	void Start () {
		playingOpening = true;
		endPosition = bingBang.transform.position;
		bingBang.transform.position = new Vector3 (15f, 3.19f, 20f);
		oldCopy = menu [currentSelection].text;
		menu[currentSelection].text = "- " + menu[currentSelection].text + " -";
		createdCredits = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (bingBang.transform.position != endPosition) {
			bingBang.transform.position = Vector3.Lerp(bingBang.transform.position, endPosition, Time.deltaTime * 3);
		}
		handleMenuInput ();
		bounceSelection ();
		sizeTimer += 1;
	}

	void selectConnect() {
		Debug.Log ("Connect selected");
	}

	void selectHost() {
		Debug.Log ("Host selected");
	}

	void selectCredits() {
		if (!createdCredits) {
			createdCredits = (GameObject)Instantiate (credits, new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0));
			createdCredits.transform.parent = mainMenu.transform;
			foreach (RectTransform s in createdCredits.GetComponentsInChildren<RectTransform>()) {
				s.anchoredPosition = new Vector3(0,0);
			}
		} else {
			Destroy(createdCredits);
			createdCredits = null;
		}
	}

	void bounceSelection() {
		if ((sizeTimer % 5) == 0) {
			if ((menu [currentSelection].fontSize < 28) && (getBigger)) {
				menu [currentSelection].fontSize = menu [currentSelection].fontSize + 1;
			} else if ((menu [currentSelection].fontSize >= 28) && (getBigger)) {
				getBigger = false;
			} else if ((menu [currentSelection].fontSize < 20) && (!getBigger)) {
				getBigger = true;
			} else {
				menu [currentSelection].fontSize = menu [currentSelection].fontSize - 1;
			}
		}
	}

	void handleSelection(string name) {
		if (name.Equals ("Connect")) {
			selectConnect ();
		} else if (name.Equals ("Host")) {
			selectHost ();
		} else if (name.Equals ("Credits")) {
			selectCredits();
		}
	}

	bool canMove = true;
	bool canSelect = true;

	void handleMenuInput() {
		float h = Input.GetAxisRaw ("Vertical");
		float yaxis = ControlInputWrapper.GetAxis(ControlInputWrapper.Axis.LeftStickY);
		int size = menu.Length;
		int t = currentSelection;
		if (h != 0 && canMove) {
			canMove = false;
			if (h < 0) {
				currentSelection = (currentSelection + 1);
				if (currentSelection > size - 1) {
					currentSelection = size - 1;
				}
			} else if (h > 0) {
				currentSelection = (currentSelection - 1);
				if (currentSelection < 0) {
					currentSelection = 0;
				}
			}
		} else if (h == 0){
			canMove = true;
		}

		if (t != currentSelection) {
			//reset old menu item
			menu[t].text = oldCopy;
			oldCopy = menu[currentSelection].text;
			//set to selected version
			menu[currentSelection].text = "- " + menu[currentSelection].text + " -";
			sizeTimer = 0f;
		}

		//handles selections
		if (Input.GetKey ("space") || ControlInputWrapper.GetButton (ControlInputWrapper.Buttons.RightBumper)) {
			if (canSelect) {
				handleSelection (oldCopy);
				canSelect = false;
			}
		} else {
			canSelect = true;
		}
	}
}
