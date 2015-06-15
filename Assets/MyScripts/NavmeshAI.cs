using UnityEngine;
using System.Collections;

public class NavmeshAI : MonoBehaviour {

	private Transform target;
	public float health;
	public float damage;
	private float distance;

	private double timeShoot;
	private bool firstShoot=true;

	private Animator anim;
	// Use this for initialization
	public void CreateZombie (float walk_speed) {
		anim = GetComponent<Animator> ();
		anim.speed = walk_speed;

		target = GameObject.FindGameObjectWithTag ("Player").transform;

		this.GetComponent<NavMeshAgent> ().speed = walk_speed*0.7f;
		damage = 25;
		health = 100;
	}


	// Update is called once per frame
	void Update () {
		this.GetComponent<NavMeshAgent> ().destination = target.position;


		/* Elegxos gia Apostasi kai Shoot ston paikti */
		distance = Vector3.Distance(transform.position,target.position);
		if (distance <= 1.2f && firstShoot) {
			timeShoot = Time.time;
			Shoot ();
		} else if (distance <= 1.2f && !firstShoot) {
			if(Time.time > timeShoot + 1)
			{
				timeShoot = Time.time;
				Shoot();
			}
		}
		else if(distance > 1.2f) firstShoot = true;


	}

	void Shoot() {
		if (Time.time < timeShoot + 1) {
			target.GetComponentInChildren<PlayerShootingOffline> ().gotShot (damage);
			firstShoot = false;
		} else
			firstShoot = true;
	}
		


	public void GetShot(float damage){
		//Debug.Log ("I got shot "+damage + "  " + health);
		if(health > damage) health -= damage;
		else Destroy (this.gameObject);
	}

}
