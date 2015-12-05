/**
 * Team: Fireflies
 * @author: Clayton Pierce, Sarah Alsmiller, Preston Turner, Justin Le, Sam Wood
 */


using UnityEngine;
using System.Collections;

public class BoulderMovement : MonoBehaviour {
	private bool gogo = false;
	public int rotateSpeed = 10;
	public float speed = 10f;
	public float stopx = 260f;
	public AudioClip startNoise;
	private float waitStart;
	public AudioSource src;
	public AudioClip raiseLoop;
	public AudioClip endRaise;
	public AudioClip endSound;
	public Quaternion endRotation;
	public Collider trigger;
	public Collider rigid;
	bool doneWithRolling = false;
	bool doneWithRaising = false;

	public GameObject stairs;
	Vector3 endStairs;

	void Start() {
		src.PlayOneShot (startNoise);
		waitStart = startNoise.length;
		endStairs = stairs.transform.position;
		stairs.transform.position = new Vector3 (stairs.transform.position.x, -50, stairs.transform.position.z);

	}

	// Update is called once per frame
	void Update () {
		gogo = true;
		if (waitStart >= 0) {
			waitStart -= Time.deltaTime;
			gogo = false;
		}

		if ((gogo) && (!doneWithRolling)) {
			Vector3 pos = gameObject.transform.position;
			Quaternion rot = gameObject.transform.rotation;
			if (pos.x < stopx) {
				if (src.isPlaying == false) {
					src.Play ();
				}
				gameObject.transform.Rotate (Vector3.down * rotateSpeed * Time.deltaTime);
				//or I should probably just lerp it, but this allows oppertinuty for up down shake?
				gameObject.transform.position = new Vector3(pos.x + speed * Time.deltaTime,pos.y,pos.z);
			} else {
				if ((!doneWithRolling)) {
					src.Stop ();
					src.PlayOneShot (endSound);
					gogo = false;
					trigger.enabled = false;
					rigid.enabled = true;
					doneWithRolling = true;
					src.clip = raiseLoop;
				}
			}
		}

		if (doneWithRolling) {
			if (stairs.transform.position != endStairs) {
				if (src.isPlaying == false) {
					src.Play ();
				}
				stairs.transform.position = Vector3.Lerp (stairs.transform.position,endStairs,Time.deltaTime);
			} else {
				if (!doneWithRaising) {
					src.Stop ();
					src.PlayOneShot (endRaise);
					doneWithRaising = true;
				}
			}
		}

	}
}
