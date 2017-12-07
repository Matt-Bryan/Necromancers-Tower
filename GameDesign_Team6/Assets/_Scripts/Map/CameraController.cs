using UnityEngine;
using System.Collections;

/**
 * People that worked on this script: 
 * Lauren Kirk
**/

public class CameraController : MonoBehaviour
{

    private GameObject followedObject;

    //private Vector3 offset;

    void Start()
    {
        //offset = transform.position - followedObject.transform.position;
        if (followedObject == null) {
            followedObject = GameObject.FindWithTag("Player");
        }
    }

	public void resetCamera()
	{
		transform.position = new Vector3 (0, 0, 0);
		//followedObject.transform.position = new Vector3 (0, 0, 0);
	}

    void LateUpdate()
    {
        if (followedObject == null) {
            followedObject = GameObject.FindWithTag("Player");
        }
        transform.position = followedObject.transform.position + new Vector3(0, 0, -5);// + offset;
    }
}