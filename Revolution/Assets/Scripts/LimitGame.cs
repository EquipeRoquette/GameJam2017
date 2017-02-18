using UnityEngine;
using System.Collections;

public class LimitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit2D(Collider2D other) {
        // Check if owner is black hole
        Debug.Log("Test");
        if (other.gameObject.tag == "satellite")
        {
            Destroy(other.gameObject);
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        } else if (other.gameObject.tag == "debris")
        {
            Destroy(other.gameObject);
        }
    }

}
