/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */
using UnityEngine;
using System.Collections;

public class Explosive : Entity {
	public GameObject explosionRadius;
	private ExplodeAllInRadius boom;
	private AudioSource src;

	void Start() {
		boom = explosionRadius.GetComponent<ExplodeAllInRadius> ();
		src = gameObject.GetComponent<AudioSource> ();
	}

	public override void TakeDamage(int amount, Vector3 hitPoint)
	{
		Debug.Log ("rawr");
		currentHealth = 0;
		explode ();
	}

	void explode() {
		var exp = GetComponent<ParticleSystem>();
		exp.Play();
		src.Play ();
		boom.Boom (exp.duration);
		Destroy(gameObject, exp.duration/2);
	}
}
