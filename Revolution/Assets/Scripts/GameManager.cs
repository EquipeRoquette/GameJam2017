using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject prefabBlackHole;
    public GameObject prefabSatellite;

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
	        if (cel != null && !cel.GetIsFixed())
	        {
	            cel.AddForceCelestial(celestialObjects);
	        }
	    }

	}

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(InitPlaying());
        yield return StartCoroutine(RoundPlaying());
    }

    private IEnumerator InitPlaying()
    {

        celestialObjects.Add((GameObject) Instantiate(prefabSatellite, new Vector3(-1.5f, 1, 0), Quaternion.identity));
        celestialObjects.Add((GameObject) Instantiate(prefabSatellite, new Vector3(1.5f, 1, 0), Quaternion.identity));
//        celestialObjects.Add((GameObject) Instantiate(prefabBlackHole, new Vector3(8, 0, 0), Quaternion.identity));
//        celestialObjects.Add((GameObject) Instantiate(prefabBlackHole, new Vector3(-8, 0, 0), Quaternion.identity));

        celestialObjects[0].GetComponent<CelestialObject>().Init(false, new Vector2(-0.2f, 3));
        celestialObjects[1].GetComponent<CelestialObject>().Init(false, new Vector2(3.2f, 0));
//        celestialObjects[2].GetComponent<CelestialObject>().Init(true, new Vector2(0, 0));
//        celestialObjects[3].GetComponent<CelestialObject>().Init(true, new Vector2(0, 0));

        yield return null;
    }

    private IEnumerator RoundPlaying()
    {
        while (true)
        {

            yield return null;
        }
    }

}
