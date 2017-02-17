using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebrisBelt : MonoBehaviour {


    public float Radius = 4f;
    public float RadiusWidth = 0.4f;
    public float Density = 100.0f;
    public int Seed = 0;
    public GameObject prefabDebris;

    private Vector3 centerPos;
    private int n;
    private List<GameObject> celestialObjects = new List<GameObject>();


	// Use this for initialization
	void Start ()
	{

//	    var test = GetComponentInParent<GameObject>();
	    if (transform.parent != null)
	    {
	        centerPos = transform.parent.position;
	    }
	    else
	    {
            centerPos = new Vector3(0,0,0);
	    }


	    System.Random rnd = new System.Random(Seed);

	    n = (int) (Radius * Density / (2 * Math.PI));

        for (int i = 0; i < n; i++)
        {
            var theta = (float) (2*Math.PI*rnd.NextDouble());
            var radiusDelta = (float) (RadiusWidth*rnd.NextDouble()) - RadiusWidth/2;
            var center = new Vector2(centerPos.x, centerPos.y);

            celestialObjects.Add((GameObject) Instantiate(prefabDebris,
                new Vector3(0, 0, 0), Quaternion.identity));
            celestialObjects[celestialObjects.Count-1].GetComponent<Debris>().Init(true, 0.01f, Radius+radiusDelta, theta, center);
        }

	}

    // Update is called once per frame
	void Update () {
	
	}
}
