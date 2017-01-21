using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineDeform : MonoBehaviour {

	public float frequency = 1;
	public float frequencyDecay = 0.1f;
	private float currentFrequency;

	public float startHeight = 1;

	private SphereCollider sphereCollider;

	// Use this for initialization
	void Start () {
		currentFrequency = frequency;

		sphereCollider = GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector3 newPosition = new Vector3 (0, 0, 0);
		transform.position = new Vector3 (transform.position.x, Mathf.Sin(1*Time.time)*startHeight, transform.position.z);
		currentFrequency += frequencyDecay * Time.deltaTime;*/
		sphereCollider.radius+=0.1f * Time.deltaTime;
	}
	
	void OnTriggerStay (Collider other){
		Debug.Log ("COLLIDE");
		if (other.tag == tag) {
			Mesh otherMesh = other.GetComponent<MeshFilter> ().mesh;
			Vector3[] vertices = otherMesh.vertices;
			Vector3[] normals = otherMesh.normals;

			MeshCollider otherMeshCollider = other.GetComponent<MeshCollider> ();

			int i = 0;
			while (i < vertices.Length) {
				Vector3 verticeWorldSpacePos = transform.TransformPoint (vertices[i]);
				float distance = Vector3.Distance (verticeWorldSpacePos, transform.position);
				if (distance<=sphereCollider.radius) {
					vertices[i] = new Vector3 (vertices[i].x, Mathf.Sin((distance-Time.time)*frequency)*startHeight, vertices[i].z);
				}
				i++;
			}
			otherMesh.vertices = vertices;
			if(otherMeshCollider) otherMeshCollider.sharedMesh = otherMesh;
		}
	}
}
