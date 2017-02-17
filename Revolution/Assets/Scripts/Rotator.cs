using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {	

    void Start()
    {
        transform.Rotate(new Vector3(0, 1, 0), Random.Range(0,100));
        transform.Rotate(new Vector3(1, 0, 0), Random.Range(-45, 45));
    }
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 1, 0), 5);
    }
}
