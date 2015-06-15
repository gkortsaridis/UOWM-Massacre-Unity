using UnityEngine;
using System.Collections;

public class MoveMyPlayer : MonoBehaviour {

	[SerializeField]  float speed;
	[SerializeField]  float jumpSpeed;
	[SerializeField]  AudioClip walkAudio;

	private Animator anim;
	private Rigidbody rig;



	void Start () {
		anim = GetComponent<Animator> ();
		rig = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		if (Input.GetKeyUp (KeyCode.Escape)) {
			Cursor.visible = true;
			Application.LoadLevel ("MainMenu");
		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) speed = 6;
		if (Input.GetKeyUp (KeyCode.LeftShift)) speed = 3;

		anim.speed = 1;

		if (h > 0.1f) {
			anim.SetBool("walk",true);
			moveRight (speed);
			//AudioSource.PlayClipAtPoint(walkAudio , this.transform.position);
		}
		else if(h < -0.1f)
		{
			anim.SetBool("walk",true);
			moveLeft(speed);
			//AudioSource.PlayClipAtPoint(walkAudio , this.transform.position);

		}
		if(v > 0.1f)
		{
			anim.SetBool("walk",true);
			moveForward(speed);
			//AudioSource.PlayClipAtPoint(walkAudio , this.transform.position);

		}
		else if(v < -0.1f)
		{
			anim.SetBool("walk",true);
		    moveBack(speed);
			//AudioSource.PlayClipAtPoint(walkAudio , this.transform.position);

		}
		else
		{
			anim.SetBool("walk",false);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			anim.SetTrigger("jump");
			anim.speed = 1f;
			rig.AddForce(Vector3.up *jumpSpeed);

		}

		if(Input.GetKeyUp(KeyCode.C))
		{
			if(anim)
			anim.SetBool("crouch",!anim.GetBool("crouch"));
		}

		if(Input.GetMouseButtonDown(1))
		{
			anim.SetBool("aim",true);
		}

		if(Input.GetMouseButtonUp(1))
		{
			anim.SetBool("aim",false);
		}

	}

	private void moveForward(float speed) {
		transform.localPosition += transform.forward * speed * Time.deltaTime;
	}
	
	private void moveBack(float speed) {
		transform.localPosition -= transform.forward * speed * Time.deltaTime;
	}
	
	private void moveRight(float speed) {
		transform.localPosition += transform.right * speed * Time.deltaTime;
	}
	
	private void moveLeft(float speed) {
		transform.localPosition -= transform.right * speed * Time.deltaTime;
	}
}
