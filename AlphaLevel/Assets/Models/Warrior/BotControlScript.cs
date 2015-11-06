using UnityEngine;
using System.Collections;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]
public class BotControlScript : MonoBehaviour
{
	public float animSpeed = 1.5f;
	public AudioSource terrAudio;


	private Animator anim;							// The animator to the character
	private AnimatorStateInfo currentBaseState;		// Current state of the base layer
	

	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int attackState = Animator.StringToHash ("Base Layer.Attack");
	static int cheerState = Animator.StringToHash ("Base Layer.Cheer");
	static int runState = Animator.StringToHash ("Base Layer.BlendRunTurn");

	void Start ()
	{
		anim = GetComponent<Animator>();
		//terrAudio = Terrain.activeTerrain.GetComponent<AudioSource> ();
	}
	
	// This will update based on the physics engine
	void FixedUpdate ()
	{
		// Get the input based on WASD
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		// Set the Speed and Direction
		anim.SetFloat("Speed", v);			
		anim.SetFloat("Direction", h);		

		anim.speed = animSpeed;									// Set the animation Speed
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// Get the current state of the Animator

		
		// If we are in the idle state and press the spacebar then the character will attack
		if (currentBaseState.nameHash == idleState)
		{
			if(Input.GetButtonDown("Jump"))
			{
				anim.SetBool("Jab", true);
			}
			else if(Input.GetButtonDown("Fire3"))
			{
				anim.SetBool ("Rocky", true);
			}
		}
		else if(currentBaseState.nameHash == runState)
		{
			//if(!terrAudio.isPlaying)
			//	terrAudio.Play();
		}
		// If we are in attack state then set our attack variable to false
		else if(currentBaseState.nameHash == attackState)
		{
			anim.SetBool("Jab", false);

		}

		// If we are in cheer state then set Rocky to false
		else if(currentBaseState.nameHash == cheerState)
		{
			anim.SetBool("Rocky", false);
			
		}
	}
}
