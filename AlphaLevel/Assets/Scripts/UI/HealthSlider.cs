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
	}
}
