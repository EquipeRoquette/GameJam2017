using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CelestialObject : MonoBehaviour
{

    public bool isFixed = true;
    protected Rigidbody2D m_Body;
    protected Vector2 velocityInit;

    public CelestialObject()
    {
        Init(isFixed, new Vector2(0,0));
    }


    // Use this for initialization
	void Start () {
	    m_Body = GetComponent<Rigidbody2D>();
	    m_Body.velocity = velocityInit;
	}
	
	void Update () {

	}

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

    void OnCollisionEnter2D(Collision2D coll) {
        // Check if owner is black hole
        if (tag == "black_hole")
        {
            if (coll.gameObject.tag == "satellite")
            {
                Destroy(coll.gameObject);
            } else if (coll.gameObject.tag == "debris")
            {
                Destroy(coll.gameObject);
            }
        }

    }


    public void Init(bool isFixed, Vector2 velocity)
    {
        SetIsFixed(isFixed);
        SetVelocity(velocity);
    }

    public double GetMass()
    {
        if (m_Body != null)
        {
            return m_Body.mass;
        }
        else
        {
            return 0;
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (m_Body != null)
            m_Body.velocity = velocity;
        else
            velocityInit = velocity;
    }

    public void SetMass(float mass)
    {
        if(m_Body != null)
            m_Body.mass = mass;
    }

    public void SetIsFixed(bool isFixed)
    {
        this.isFixed = isFixed;
    }

    public bool GetIsFixed()
    {
        return isFixed;
    }

    private Vector2 GetPosition()
    {
        return m_Body != null ? m_Body.position : new Vector2(0, 0);
    }

    public virtual void AddForceCelestial(List<CelestialObject> list_Objects)
    {
        if (m_Body == null || list_Objects == null || isFixed) return;

        var forceSum = GetSumForceInteraction(this, list_Objects);

        m_Body.AddForce(forceSum);
    }

    protected static Vector2 GetSumForceInteraction(CelestialObject reference, List<CelestialObject> list_Objects)
    {
        var forceSum = new Vector2(0,0);
        foreach (var cel in list_Objects)
        {
            // Check if valid
            if (cel == null || cel == reference)
                continue;

            forceSum += GetForceInteraction(reference, cel);

        }

        return forceSum;
    }

    protected static Vector2 GetForceInteraction(CelestialObject obj1, CelestialObject obj2)
    {
        var r = obj2.GetPosition() - obj1.GetPosition();
        var m1 = (float)obj1.GetMass();
        var m2 = (float)obj2.GetMass();
        if (r.magnitude < 1)
            r = r.normalized;
        return r.normalized * (( m1 * m2) / (float)Math.Pow(r.magnitude,2));
    }

}
