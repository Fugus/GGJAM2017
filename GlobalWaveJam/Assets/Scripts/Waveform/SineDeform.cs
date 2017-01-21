using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineDeform : MonoBehaviour {

	//Propagation of wave
	public float propagationSpeedPerSec = 2;
	public float maxPropagation = 10;

	//Wave Movement duration AFTER maxPropagation is achieved
	public float decay = 1;

	//Amount of waves
	public float frequency = 1;
	//public float frequencyDecay = 0.1f;
	private float currentFrequency;

	//Height of waves
	public float startHeight = 1;

	//WaveTimer
	private float waveTimer = 1;

	private SphereCollider sphereCollider;

	// Use this for initialization
	void Awake () {
		currentFrequency = frequency;

		sphereCollider = GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector3 newPosition = new Vector3 (0, 0, 0);
		transform.position = new Vector3 (transform.position.x, Mathf.Sin(1*Time.time)*startHeight, transform.position.z);
		currentFrequency += frequencyDecay * Time.deltaTime;*/

		waveTimer += Time.deltaTime*propagationSpeedPerSec;
		//Debug.Log (Time.time);

		/*if(sphereCollider.radius < maxPropagation) */
			sphereCollider.radius += propagationSpeedPerSec * Time.deltaTime;
		
		/*if (sphereCollider.radius > maxPropagation)
			sphereCollider.radius = maxPropagation;*/

		if (sphereCollider.radius /*==*/ > maxPropagation) {
			//Start wave movement decay
			propagationSpeedPerSec -= Time.deltaTime*decay;
			if (propagationSpeedPerSec < 0)
				propagationSpeedPerSec = 0;
		}
	}
	
	void OnTriggerStay (Collider other){
		if (other.tag == tag) {
			Mesh otherMesh = other.GetComponent<MeshFilter> ().mesh;
			Vector3[] vertices = otherMesh.vertices;
			Vector3[] normals = otherMesh.normals;

			MeshCollider otherMeshCollider = other.GetComponent<MeshCollider> ();

			int i = 0;
			while (i < vertices.Length) {
				Vector3 verticeWorldSpacePos = other.transform.TransformPoint (vertices[i]);
				float distance = Vector3.Distance (verticeWorldSpacePos, transform.position);
				//Debug.Log (distance);
				if (distance<=sphereCollider.radius) {
					float distanceHeightRatio = startHeight - distance/maxPropagation;
					if (distanceHeightRatio < 0)
						distanceHeightRatio = 0;
					
					vertices[i] = new Vector3 (vertices[i].x, Mathf.Sin((distance-(waveTimer))*(frequency))*(/*startHeight*2/waveTimer*/distanceHeightRatio), vertices[i].z);
				}
				i++;
			}

			otherMesh.vertices = vertices;
			if(otherMeshCollider) otherMeshCollider.sharedMesh = otherMesh;
		}
	}
}
using System.Collections.Generic;
using UnityEngine;

public class SineDeform : MonoBehaviour {

	//Propagation of wave
	public float propagationSpeedPerSec = 2;
	public float maxPropagation = 10;

	//Wave Movement duration AFTER maxPropagation is achieved
	public float decay = 1;

	//Amount of waves
	public float frequency = 1;
	//public float frequencyDecay = 0.1f;
	private float currentFrequency;

	//Height of waves
	public float startHeight = 1;

	//WaveTimer
	private float waveTimer = 1;

	private SphereCollider sphereCollider;

	// Use this for initialization
	void Awake () {
		currentFrequency = frequency;

		sphereCollider = GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector3 newPosition = new Vector3 (0, 0, 0);
		transform.position = new Vector3 (transform.position.x, Mathf.Sin(1*Time.time)*startHeight, transform.position.z);
		currentFrequency += frequencyDecay * Time.deltaTime;*/

		waveTimer += Time.deltaTime*propagationSpeedPerSec;
		//Debug.Log (Time.time);

		/*if(sphereCollider.radius < maxPropagation) */
			sphereCollider.radius += propagationSpeedPerSec * Time.deltaTime;
		
		/*if (sphereCollider.radius > maxPropagation)
			sphereCollider.radius = maxPropagation;*/

		if (sphereCollider.radius /*==*/ > maxPropagation) {
			//Start wave movement decay
			propagationSpeedPerSec -= Time.deltaTime*decay;
			if (propagationSpeedPerSec < 0)
				propagationSpeedPerSec = 0;
				propagationSpeedPerSec = 0;
			
			if (propagationSpeedPerSec == 0)
				Destroy (this.gameObject);
		}
	}
	
	void OnTriggerStay (Collider other){
		if (other.tag == tag) {
			Mesh otherMesh = other.GetComponent<MeshFilter> ().mesh;
			Vector3[] vertices = otherMesh.vertices;
			Vector3[] normals = otherMesh.normals;

			MeshCollider otherMeshCollider = other.GetComponent<MeshCollider> ();

			int i = 0;
			while (i < vertices.Length) {
				Vector3 verticeWorldSpacePos = other.transform.TransformPoint (vertices[i]);
				float distance = Vector3.Distance (verticeWorldSpacePos, transform.position);
				//Debug.Log (distance);
				if (distance<=sphereCollider.radius) {
					float distanceHeightRatio = startHeight - distance/maxPropagation;
					if (distanceHeightRatio < 0)
						distanceHeightRatio = 0;
					
					vertices[i] = new Vector3 (vertices[i].x, Mathf.Sin((distance-(waveTimer))*(frequency))*(/*startHeight*2/waveTimer*/distanceHeightRatio), vertices[i].z);
				}
				i++;
			}

			otherMesh.vertices = vertices;
			if(otherMeshCollider) otherMeshCollider.sharedMesh = otherMesh;
		}
	}
}