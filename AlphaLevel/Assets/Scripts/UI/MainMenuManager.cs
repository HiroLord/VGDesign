/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	bool playingOpening;
	public GameObject mainMenu;
	public GameObject bingBang;
	public GameObject fireflies;
	Vector3 endPosition;
	Vector3 endFireflies;
	public Text[] menu;
	int currentSelection = 0;
	int oldSelection = 0;
	string oldCopy;
	float sizeTimer = 0f;
	bool getBigger = true;
	public float fireflyWait = 10f;
	float fTimer;

	public GameObject credits;
	GameObject createdCredits;

	// Use this for initialization
	void Start () {
		playingOpening = true;
		endPosition = bingBang.transform.position;
		endFireflies = fireflies.transform.position;
		fireflies.transform.position = new Vector3(-40f, 3.19f, -40f);
		bingBang.transform.position = new Vector3 (15f, 3.19f, 20f);
		oldCopy = menu [currentSelection].text;
		menu[currentSelection].text = "- " + menu[currentSelection].text + " -";
		createdCredits = null;
		fTimer = 0;
	}

	public void hoverOption(int num) {
		//reset old menu item
		if (currentSelection != num) {
			oldSelection = currentSelection;
			menu [oldSelection].text = oldCopy;
			menu[oldSelection].fontSize = 25;
			currentSelection = num;
			oldCopy = menu [currentSelection].text;
			//set to selected version
			menu [currentSelection].text = "- " + menu [currentSelection].text + " -";
			sizeTimer = 0f;
		}
	}

	// Update is called once per frame
	void Update () {
		if (bingBang.transform.position != endPosition) {
			bingBang.transform.position = Vector3.Lerp(bingBang.transform.position, endPosition, Time.deltaTime * 3);
		}
		handleMenuInput ();
		bounceSelection ();
		sizeTimer += 1;
		if ((fTimer >= fireflyWait) && (fireflies.transform.position != endFireflies)) {
			fireflies.transform.position = Vector3.Lerp (fireflies.transform.position, endFireflies, Time.deltaTime * 3);
		} else if (fTimer < fireflyWait) {
			fTimer += Time.deltaTime;
		}
		//Debug.Log (fTimer);
	}

	public void selectConnect() {
		Debug.Log ("Join selected");
		Begin (true);
	}

	public void selectHost() {
		Debug.Log ("Host selected");
		Begin (true);
	}

	private void Begin(bool connection) {
		NetworkManager man = GameObject.Find ("NetworkManager").GetComponent<NetworkManager>();
		man.Host = connection;
		Application.LoadLevel ("IslandStart");
	}

	public void selectCredits() {
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
		Debug.Log (name);
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
		int nextSelection = t;
		if (h != 0 && canMove) {
			canMove = false;
			if (h < 0) {
				nextSelection = (currentSelection + 1);
				if (nextSelection > size - 1) {
					nextSelection = 0;
				}
			} else if (h > 0) {
				nextSelection = (currentSelection - 1);
				if (nextSelection < 0) {
					nextSelection = size - 1;
				}
			}
		} else if (h == 0){
			canMove = true;
		}

		if (t != nextSelection) {
			oldSelection = t;
			hoverOption(nextSelection);
		}

		//handles selections
		if (Input.GetKey ("space") || Input.GetKey ("return") || ControlInputWrapper.GetButton (ControlInputWrapper.Buttons.RightBumper)) {
			if (canSelect) {
				handleSelection(menu[currentSelection].name);
				canSelect = false;
			}
		} else {
			canSelect = true;
		}
	}
}
