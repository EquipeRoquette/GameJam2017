using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CelestialObject : MonoBehaviour
{

    private bool isFixed = true;
    private Rigidbody2D m_Body;

    // Use this for initialization
	void Start () {

	}
	
	void Update () {

	}


    void OnCollisionEnter2D(Collision2D coll) {
        // Check if owner is black hole
        if (tag == "black_hole")
        {
            if (coll.gameObject.tag == "satellite")
            {
                Destroy(coll.gameObject);
            }
        }
    }


    public void Init(bool isFixed, Vector2 velocity)
    {
        m_Body = this.GetComponent<Rigidbody2D>();
        SetIsFixed(isFixed);
        SetVelocity(velocity);

    }

    public double GetMass()
    {
        return m_Body.mass;
    }

    public void SetVelocity(Vector2 velocity)
    {
        if(m_Body != null)
            m_Body.velocity = velocity;
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

    public void AddForceCelestial(List<GameObject> list_Objects)
    {
        if (m_Body == null || list_Objects == null) return;

        foreach (var go in list_Objects)
        {
            if (go == null) continue;

            var cel = go.GetComponent<CelestialObject>();
            if (cel != this && !isFixed)
            {
                m_Body.AddForce(GetForceInteraction(this, cel));
            }
        }
    }

    private static Vector2 GetForceInteraction(CelestialObject obj1, CelestialObject obj2)
    {
        var r = obj2.GetPosition() - obj1.GetPosition();
        var m1 = (float)obj1.GetMass();
        var m2 = (float)obj2.GetMass();
        if (r.magnitude < 1)
            r = r.normalized;
        return r.normalized * (( m1 * m2) / (float)Math.Pow(r.magnitude,2));
    }

}
