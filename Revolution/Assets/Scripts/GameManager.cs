using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject prefabBlackHole;
    public GameObject prefabSatellite;
    public GameObject prefabAsteroid;

    private readonly List<GameObject> celestialObjects = new List<GameObject>();

    // Use this for initialization
	void Start () {
	    StartCoroutine(GameLoop());
	}

	// Update is called once per frame
	void Update () {

	    // Update forces
	    foreach (var go in celestialObjects)
	    {
	        if (go == null) continue;

	        var cel = go.GetComponent<CelestialObject>();
	        cel.AddForceCelestial(celestialObjects);

	    }

	}

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(InitPlaying());
        yield return StartCoroutine(RoundPlaying());
    }

    private IEnumerator InitPlaying()
    {

        celestialObjects.Add((GameObject) Instantiate(prefabBlackHole, new Vector3(0, 0, 0), Quaternion.identity));
        celestialObjects[0].GetComponent<CelestialObject>().Init(true, new Vector2(0.0f, 0.0f));

        celestialObjects.Add((GameObject) Instantiate(prefabSatellite, new Vector3(-10f, 1, 0), Quaternion.identity));
        celestialObjects[1].GetComponent<CelestialObject>().Init(false, new Vector2(3.2f, -4));


        createBelt(celestialObjects[0]);


        yield return null;
    }

    private IEnumerator RoundPlaying()
    {
        while (true)
        {

            yield return null;
        }
    }


    private void createBelt(GameObject gameObject)
    {
        var radius = 4f;
        var radius_width = 0.4f;
        var density = 100f;
        var n = (int) radius * density / (2 * Math.PI);

        System.Random rnd1 = new System.Random();

        for (int i = 0; i < n; i++)
        {
            var theta = (float) (2*Math.PI*rnd1.NextDouble());
            var radiusDelta = (float) (radius_width*rnd1.NextDouble()) - radius_width/2;
            var center = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

            celestialObjects.Add((GameObject) Instantiate(prefabAsteroid,
                new Vector3(0, 0, 0), Quaternion.identity));
            celestialObjects[celestialObjects.Count-1].GetComponent<BeltAsteroid>().Init(true, 0.01f, radius+radiusDelta, theta, center);
        }


    }
}
