﻿/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {
	public float health;
	public Slider healthSlider;
	public PostDeath postDeath;
	private bool isDead = false;

	// Use this for initialization
	void Start () {
		health = 100f;
	}

	public void Damage(float amount) {
		health -= amount;

		if (health <= 0) {
			isDead = true;
			postDeath.isDead = true;
		}
		healthSlider.value = health;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
