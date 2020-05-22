using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

	public float minDistance = 1.0f;
	public float maxDistance = 4.0f;
	public float smooth = 10.0f;
	Vector3 dollyDir;
	public Vector3 dollyDirAdjusted;
	public float distance;
	private PhysicsScene scene;
	public LayerMask masks;

	// Use this for initialization
	void Start () {
		dollyDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
		scene = HamsterBallSimulator.getPhysicsScene();
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 desiredCameraPos = transform.parent.TransformPoint (dollyDir * maxDistance);
		RaycastHit hit;

		if(scene.Raycast(transform.parent.position, (desiredCameraPos - transform.parent.position).normalized, out hit, maxDistance * 2.0f, masks))
		{
			distance = Mathf.Clamp((hit.distance * 0.87f), minDistance, maxDistance);
		}
		else
		{
			distance = maxDistance;
		}

		transform.localPosition = Vector3.Lerp (transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
	}
}
