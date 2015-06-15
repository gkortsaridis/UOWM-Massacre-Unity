using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponentInChildren<PlayerShootingOffline> ().gotShot (100);
		}
	}
}
