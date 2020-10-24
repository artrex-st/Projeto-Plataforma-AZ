using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform cameraTransform, playerTransform; // Camera Transform
	[Range(0f,100f)]
	public float speed=3;
	public List<Transform> cameraLimits;
	public float limitRange;
	void Start()
	{
		playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
		cameraTransform = GetComponent<Transform>();

	}
	void LateUpdate()
	{
		//SimpleFollow();
		ClampFollow();
	}

	void SimpleFollow()
	{
		cameraTransform.position = Vector3.Lerp(cameraTransform.position, new Vector3(playerTransform.position.x, playerTransform.position.y, cameraTransform.position.z), speed * Time.deltaTime);
	}

	void RoundFollow()
	{
		cameraTransform.position = Vector3.Lerp(cameraTransform.position, new Vector3(Mathf.Round(playerTransform.position.x), Mathf.Round(playerTransform.position.y), cameraTransform.position.z), speed * Time.deltaTime);
	}
	void MaxRangeFollow()
	{
		cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, new Vector3(playerTransform.position.x, playerTransform.position.y, cameraTransform.position.z), speed * Time.deltaTime);
	}
	void LimitFollow()
    {
        if (Mathf.Abs(playerTransform.position.y - cameraLimits[0].position.y) >= limitRange && Mathf.Abs(playerTransform.position.y - cameraLimits[2].position.y) >= limitRange)
        {
			cameraTransform.position = Vector3.Lerp(
				new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z), 
				new Vector3(cameraTransform.position.x, playerTransform.position.y, cameraTransform.position.z), 
				speed * Time.deltaTime);
        }

		if (Mathf.Abs(playerTransform.position.x - cameraLimits[1].position.x) >= limitRange && Mathf.Abs(playerTransform.position.x - cameraLimits[3].position.x) >= limitRange)
		{
			cameraTransform.position = Vector3.Lerp(
				new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z),
				new Vector3(playerTransform.position.x, cameraTransform.position.y, cameraTransform.position.z),
				speed * Time.deltaTime);
		}
	}
	void ClampFollow()
    {
		cameraTransform.position = Vector3.Lerp(cameraTransform.position,
			new Vector3(
			Mathf.Clamp(playerTransform.position.x, cameraLimits[3].position.x +9, cameraLimits[1].position.x -9),
			Mathf.Clamp(playerTransform.position.y, cameraLimits[2].position.y +5, cameraLimits[0].position.y -5),
			cameraTransform.position.z), speed * Time.deltaTime);
    }

}
