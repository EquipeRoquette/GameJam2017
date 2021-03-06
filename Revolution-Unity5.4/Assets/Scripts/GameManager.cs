﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject prefabSatellite;
    EndOfLevel endManager;
    public int nbYarn = 3;

    private readonly List<CelestialObject> celestialObjects = new List<CelestialObject>();

    arrowControl aC;

    // Use this for initialization
	void Start () {
	    StartCoroutine(GameLoop());

       aC = FindObjectOfType<arrowControl>();
       endManager = FindObjectOfType<EndOfLevel>();
	}

	// Update is called once per frame
	void Update () {

      var objectsScene = FindObjectsOfType<DebrisBelt>();
      if (objectsScene != null)
      {
         foreach (var obj in objectsScene)
         {
            var ob = obj.getDebris();
            if (ob ==null) continue;

            foreach (var cel in ob)
            {
               celestialObjects.Add(cel.GetComponent<CelestialObject>());
            }
         }
      }



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

        // Look for all element on scene
        var objectsScene = FindObjectsOfType<CelestialObject>();

        foreach (var obj in objectsScene)
        {
            celestialObjects.Add(obj);
        }


//        var sat = (GameObject) Instantiate(prefabSatellite, new Vector3(0, 4, 0), Quaternion.identity);
//        celestialObjects.Add(sat.GetComponent<CelestialObject>());
//        celestialObjects[celestialObjects.Count-1].Init(false, new Vector2(-5, -3));

        Debug.Log("Object Scene" + celestialObjects.Count());

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
        var sat = (GameObject) Instantiate(prefabSatellite, position, Quaternion.identity);
        celestialObjects.Add(sat.GetComponent<CelestialObject>());
        celestialObjects[celestialObjects.Count-1].Init(false, speed);
    }

   public void notifyYarnTaken() {
      nbYarn--;
      if (nbYarn == 0) {
         Debug.Log ("END");
         endManager.endOfLevel (3);
      }
   }

}
