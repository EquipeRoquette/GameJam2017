using System;
using UnityEngine;
using System.Collections;

public class Yarn : MonoBehaviour
{

    public bool isOrbiting = false;
    public float angularSpeed = 0.01f;

    private float angle = 0.0f;
    private Vector3 centerPos;
    private float radius = 4.0f;

	// Use this for initialization
	void Start () {

	    if (transform.parent != null)
	    {
	        centerPos = transform.parent.position;
	    }
	    else
	    {
	        centerPos = new Vector3(0,0,0);
	    }

	    radius = (centerPos-transform.position).magnitude;
	    angle = Mathf.Deg2Rad*Vector3.Angle(transform.position-centerPos, new Vector3(1, 0, 0));
	    if ((transform.position-centerPos).y < 0)
	        angle = -angle;


	}
	
	// Update is called once per frame
	void Update () {
	    // Dummy annimation untily external action
	    if (isOrbiting)
	    {
	        angle += angularSpeed;
            updatePosition();
	    }


	}

    void OnTriggerEnter2D(Collider2D other) {
        // Check if owner is black hole
        if (other.gameObject.tag != "debris" && other.gameObject.tag != "satellite") return;

        Destroy(gameObject);

        if (other.gameObject.tag == "debris")
        {
            Destroy(other.gameObject);
        }
    }

    public void updatePosition()
    {
        var x = (float) (radius * Math.Cos(angle));
        var y = (float) (radius * Math.Sin(angle));
        transform.position = new Vector3(x+centerPos.x, y+centerPos.y, 0);
    }
}
