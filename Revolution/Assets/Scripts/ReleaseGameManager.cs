using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReleaseGameManager : MonoBehaviour
{
    public GameObject prefabBlackHole;
    public GameObject prefabSatellite;


    private readonly List<GameObject> celestialObjects = new List<GameObject>();

    // Use this for initialization
	void Start () {
	    StartCoroutine(GameLoop());
	}

    void OnCollisionEnter (Collision col)
    {
        Debug.Log("tesst");
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

        celestialObjects.Add((GameObject) Instantiate(prefabBlackHole, new Vector3(8, 0, 0), Quaternion.identity));
        celestialObjects.Add((GameObject) Instantiate(prefabBlackHole, new Vector3(-8, 0, 0), Quaternion.identity));

        celestialObjects[0].GetComponent<CelestialObject>().Init(true, new Vector2(0, 0));
        celestialObjects[1].GetComponent<CelestialObject>().Init(true, new Vector2(0, 0));

        yield return null;
    }

    private IEnumerator RoundPlaying()
    {
        while (true)
        {

            yield return null;
        }
    }

	public void launchSatellite(Vector3 position, Vector2 speed){
		celestialObjects.Add((GameObject) Instantiate(prefabSatellite, position, Quaternion.identity));
		celestialObjects[celestialObjects.Count-1].GetComponent<CelestialObject>().Init(false, speed);
	}

}
