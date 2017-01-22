using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineDeform : MonoBehaviour
{
	public Gradient heightVertexColor;
    public AnimationCurve waveHeightFromDistanceToEdge;
	[UnityEngine.Serialization.FormerlySerializedAsAttribute("LALA")]
    public AnimationCurve CurveShape;

    //Propagation of wave
    public float propagationSpeedPerSec = 2;
    public float maxPropagation = 10;

    //Wave Movement duration AFTER maxPropagation is achieved
    public float movementDecay = 1;

    //Amount of waves
    public float frequency = 1;

    //Height of waves
    public float waveHeight = 1;
    public bool higherHeightByDist = false;

    //WaveTimer
    private float waveTimer = 1;

    private SphereCollider sphereCollider;

    // Use this for initialization
    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        waveTimer += Time.deltaTime * propagationSpeedPerSec;
        //Debug.Log (Time.time);

        /*if(sphereCollider.radius < maxPropagation) */
        sphereCollider.radius += propagationSpeedPerSec * Time.deltaTime;

        /*if (sphereCollider.radius > maxPropagation)
			sphereCollider.radius = maxPropagation;*/

        if (sphereCollider.radius /*==*/ > maxPropagation)
        {
            //Start wave movement decay
            propagationSpeedPerSec -= Time.deltaTime * movementDecay;
            if (propagationSpeedPerSec < 0)
                propagationSpeedPerSec = 0;

            if (propagationSpeedPerSec == 0)
                Destroy(this.gameObject);
        }

        foreach (var deformableObject in deformableList)
        {

            Mesh otherMesh = deformableObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = otherMesh.vertices;
            Color[] vertexColors = otherMesh.colors;

Debug.LogFormat("nb vertices {0}, nbcolors {1}", vertices.Length, vertexColors.Length);

            MeshCollider otherMeshCollider = deformableObject.GetComponent<MeshCollider>();

            int i = 0;
            while (i < vertices.Length)
            {
                Vector3 verticeWorldSpacePos = deformableObject.transform.TransformPoint(vertices[i]);
                float distance = Vector3.Distance(verticeWorldSpacePos, transform.position);

                if (distance <= sphereCollider.radius)
                {
                    float waveRatio = (Mathf.Clamp(-distance + maxPropagation, 0, maxPropagation)) / maxPropagation;

                    float distanceToEdgeRatio = (sphereCollider.radius - distance) / sphereCollider.radius;

                    float currentWaveHeight = waveHeight * waveHeightFromDistanceToEdge.Evaluate(distanceToEdgeRatio);

                    float offset = Mathf.PI / 2f;
					float vertexHeightRatio = CurveShape.Evaluate(offset + (distance - waveTimer) * frequency / (2f * Mathf.PI));
                    float sinWave = 2 * vertexHeightRatio * currentWaveHeight;

                    vertices[i] = new Vector3(vertices[i].x, (vertices[i].y + sinWave) / 2, vertices[i].z);
					vertexColors[i] = heightVertexColor.Evaluate((vertexHeightRatio + 1) / 2);
                }
                i++;
            }

            otherMesh.vertices = vertices;
            otherMesh.colors = vertexColors;
            if (otherMeshCollider) otherMeshCollider.sharedMesh = otherMesh;
        }
    }

    private List<GameObject> deformableList = new List<GameObject>();
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == tag && !deformableList.Contains(other.gameObject)) deformableList.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        deformableList.Remove(other.gameObject);
    }

    // Good night sweet prince
    //	void OnTriggerStay (Collider other){
    //		if (other.tag == tag) {
    //			Mesh otherMesh = other.GetComponent<MeshFilter> ().mesh;
    //			Vector3[] vertices = otherMesh.vertices;
    //			Vector3[] normals = otherMesh.normals;
    //
    //			MeshCollider otherMeshCollider = other.GetComponent<MeshCollider> ();
    //
    //			int i = 0;
    //			while (i < vertices.Length) {
    //				Vector3 verticeWorldSpacePos = other.transform.TransformPoint (vertices[i]);
    //				float distance = Vector3.Distance (verticeWorldSpacePos, transform.position);
    //				//Debug.Log (distance);
    //				if (distance<=sphereCollider.radius) {
    //					float distanceHeightRatio = startHeight - distance/maxPropagation;
    //					if (distanceHeightRatio < 0)
    //						distanceHeightRatio = 0;
    //					
    //					vertices[i] = new Vector3 (vertices[i].x, Mathf.Sin((distance-(waveTimer))*(frequency))*(/*startHeight*2/waveTimer*/distanceHeightRatio), vertices[i].z);
    //				}
    //				i++;
    //			}
    //
    //			otherMesh.vertices = vertices;
    //			if(otherMeshCollider) otherMeshCollider.sharedMesh = otherMesh;
    //		}
    //	}
}