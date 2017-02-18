using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndOfLevel : MonoBehaviour {
   bool levelEnded = false;
   public int level;

   public Text text;
   public GameObject prefabWinnerStar;
   public GameObject prefabLoserStar;

   public string nextLevel;

   float endTime;

   public Vector3 star1Begin = new Vector3 (-10, 10, -2);
   public Vector3 star2Begin = new Vector3 (0, 10, -2);
   public Vector3 star3Begin = new Vector3 (10, 10, -2);

   Quaternion baseRotation = Quaternion.identity;


   GameObject star1;
   GameObject star2;
   GameObject star3;

   public float time1 = 1F;
   public float time2 = 1.4F;
   public float time3 = 2F;

   public float endHeight = 25;

	// Use this for initialization
	void Start () {
      baseRotation.eulerAngles = new Vector3 (0, -45, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

      if (levelEnded) {
         float time = Time.time;
         star1.transform.position = Vector3.Lerp (star1Begin, star1Begin + Vector3.down * endHeight, (time - endTime) / time1);
         star2.transform.position = Vector3.Lerp (star2Begin, star2Begin + Vector3.down * endHeight, (time - endTime) / time2);
         star3.transform.position = Vector3.Lerp (star3Begin, star3Begin + Vector3.down * endHeight, (time - endTime) / time3);

         if (time - endTime > 10) {
            SceneManager.LoadScene (nextLevel);
         }
      }


	}

   public void okTyped() {
      SceneManager.LoadScene (nextLevel);
   }


   public void endOfLevel(int stars) {
      MainManager.test = "success";
      levelEnded = true;
      endTime = Time.time;
      if (text != null) {
         text.text = "Level completed";
      }
      star1 = (GameObject) Instantiate(prefabWinnerStar, star1Begin, baseRotation);
      Rigidbody r1 = star1.GetComponent<Rigidbody>();
      r1.angularVelocity = new Vector3 (0, 1, 0);

      if (stars >= 2) {
         star2 = (GameObject)Instantiate (prefabWinnerStar, star2Begin, baseRotation);
         Rigidbody r2 = star2.GetComponent<Rigidbody>();
         r2.angularVelocity = new Vector3 (0, 1, 0);
         if (stars >= 3) {
            star3 = (GameObject)Instantiate (prefabWinnerStar, star3Begin, baseRotation);     
            Rigidbody r3 = star3.GetComponent<Rigidbody>();
            r3.angularVelocity = new Vector3 (0, 1, 0);
            MainManager.Trophies [level] = 3;
         } else {
            star3 = (GameObject)Instantiate (prefabLoserStar, star3Begin, baseRotation);
            MainManager.Trophies [level] = 2;
         }
      } else {
         star2 = (GameObject) Instantiate(prefabLoserStar, star2Begin, baseRotation);
         star3 = (GameObject) Instantiate(prefabLoserStar, star3Begin, baseRotation);
         MainManager.Trophies [level] = 1;
      }
   }
}
