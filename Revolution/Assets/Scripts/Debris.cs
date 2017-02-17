using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Debris : CelestialObject {

    private Vector2 centerOrbit;
    private float angularSpeed;
    private float radius;
    private float angle;


//	// Use this for initialization
//	void Start () {
//
//	}

    void OnTriggerEnter2D(Collider2D other) {
        // Check if owner is black hole
        if (tag == "debris")
        {
            if (other.gameObject.tag == "black_hole")
            {
                Destroy(gameObject);
            }
        }
    }

	// Update is called once per frame
	void Update ()
	{

	    // Dummy annimation untily external action
	    if (isFixed)
	    {
	        angle += angularSpeed;
	        updatePosition();
	    }
	}

    public void Init(bool isFixed, float angularSpeed, float radius, float angle, Vector2 centerOrbit)
    {
        base.Init(isFixed, new Vector2(0,0));
        this.angularSpeed = angularSpeed;
        this.radius = radius;
        this.angle = angle;
        this.centerOrbit = centerOrbit;

        updatePosition();
    }


    public override void AddForceCelestial(List<CelestialObject> list_Objects)
    {

        var g = getInteractionUser(this, list_Objects);

        if (isFixed)
        {
            if (g.magnitude > 10)
                isFixed = false;
        }
        else
        {
            base.AddForceCelestial(list_Objects);
            m_Body.AddForce(g);
        }


    }

    public void updatePosition()
    {
        var x = (float) (radius * Math.Cos(angle));
        var y = (float) (radius * Math.Sin(angle));
        transform.position = new Vector3(x+centerOrbit.x, y+centerOrbit.y, 0);
    }

    static Vector2 getInteractionUser(CelestialObject reference, List<CelestialObject> list_Objects)
    {
        var forceSum = new Vector2(0,0);
        foreach (var cel in list_Objects)
        {
            if (cel == null || cel.tag != "satellite") continue;

            forceSum += GetForceInteraction(reference, cel);

        }

        return 5*forceSum;
    }
}
