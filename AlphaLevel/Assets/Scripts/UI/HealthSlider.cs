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

	void Start() {
		slider = gameObject.GetComponentInChildren<Slider> ();
		entity = gameObject.GetComponentInParent<Entity> ();
	}
		
	void Update() {
		slider.value = entity.currentHealth;
		if (entity.currentHealth < 0) {
			slider.enabled = false;
		} else if ((slider.enabled = false) && (entity.currentHealth > 0)) {
			slider.enabled = true;
		}
	}
}
