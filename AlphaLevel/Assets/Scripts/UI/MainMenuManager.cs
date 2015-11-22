using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	bool playingOpening;
	public GameObject bingBang;
	Vector3 endPosition;
	public Text[] menu;
	int currentSelection;
	string oldCopy;
	float sizeTimer = 0f;
	bool getBigger = true;

	// Use this for initialization
	void Start () {
		playingOpening = true;
		endPosition = bingBang.transform.position;
		bingBang.transform.position = new Vector3 (15f, 3.19f, 20f);
		oldCopy = menu [currentSelection].text;
		menu[currentSelection].text = "- " + menu[currentSelection].text + " -";
	}
	
	// Update is called once per frame
	void Update () {
		if (bingBang.transform.position != endPosition) {
			bingBang.transform.position = Vector3.Lerp(bingBang.transform.position, endPosition, Time.deltaTime * 5);
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
			selectConnect();
		} else if (name.Equals ("Host")) {
			selectHost ();
		}
	}

	void handleMenuInput() {
		float h = Input.GetAxisRaw ("Vertical");
		float yaxis = ControlInputWrapper.GetAxis(ControlInputWrapper.Axis.LeftStickY);
		int size = menu.Length;
		int t = currentSelection;
		if (h > 0) {
			currentSelection = (currentSelection + 1);
			if (currentSelection > size -1) {
				currentSelection = size - 1;
			}
		} else if (h < 0) {
			currentSelection = (currentSelection - 1);
			if (currentSelection < 0) {
				currentSelection = 0;
			}
		}

		if (h != 0) {
			//reset old menu item
			menu[t].text = oldCopy;
			oldCopy = menu[currentSelection].text;
			//set to selected version
			menu[currentSelection].text = "- " + menu[currentSelection].text + " -";
			sizeTimer = 0f;
		}

		//handles selections
		if (Input.GetKey ("space") || ControlInputWrapper.GetButton(ControlInputWrapper.Buttons.RightBumper)) {
			handleSelection (oldCopy);
		}
	}
}
