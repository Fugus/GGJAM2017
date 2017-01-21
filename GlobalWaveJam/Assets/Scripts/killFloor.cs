using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killFloor : MonoBehaviour {

	public Vector3 SpawnPoint;
	
	void OnTriggerEnter (Collider col) {
		
		if (col.tag == "Player") {
			
			col.transform.position = SpawnPoint;
		}
	}

}
