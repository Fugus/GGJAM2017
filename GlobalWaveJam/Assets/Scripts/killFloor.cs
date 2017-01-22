using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killFloor : MonoBehaviour {

	public Vector3 SpawnPoint;
	
	void OnTriggerEnter (Collider col) {

        Player p = col.GetComponent<Player>();
        if (p != null)
            p.Die(gameObject);
            

		//if (col.tag == "Player") {
  //          Debug.Log(col.gameObject.name);
		//	col.transform.position = SpawnPoint;
		//}
	}

}
