using UnityEngine;
using System.Collections;

public class ParticleDamage : MonoBehaviour {
	//public Image deadImage;
	public AudioSource audio;
	public AudioClip explode;
	public AudioClip bang;
	public AudioClip thud;
	public Movement move;
	Rigidbody r;
	void Start() {
		r = GetComponent<Rigidbody> ();
	}
	
	void OnParticleCollision(GameObject other) {
		Debug.Log ("rawr");
		transform.position = new Vector3 (0, 0, 0);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.CompareTag ("Kill")) {
			r.AddForce (new Vector3(0,1000,0), ForceMode.Impulse);
			Debug.Log ("hit wall");
			audio.PlayOneShot (explode);
			move.SetRagdoll (true);
		}

		if (collision.collider.CompareTag ("MetalicSnake")) {
			audio.PlayOneShot (bang);
		}

		if (collision.collider.CompareTag ("Shield")) {
			audio.PlayOneShot (thud);
		}
	}
}
