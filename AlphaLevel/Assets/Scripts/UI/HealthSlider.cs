/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthSlider : MonoBehaviour {
	Slider slider;
	Entity entity;
	GameObject child;

	void Start() {
		slider = gameObject.GetComponentInChildren<Slider> ();
		entity = gameObject.GetComponentInParent<Entity> ();
		child = gameObject.transform.GetChild (0).gameObject;
	}
		
	void Update() {
		slider.value = entity.currentHealth;
		if (entity.currentHealth < 0) {
			child.SetActive (false);
		} else if ((slider.enabled = false) && (entity.currentHealth > 0)) {
			child.SetActive (true);
		}
	}
}
