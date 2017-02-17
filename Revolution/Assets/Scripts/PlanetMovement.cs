using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour {

    public Rigidbody m_Sun;
    public float m_RotationSpeed=1;
    public float m_Radius=1;

    private Rigidbody m_Rigidbody;
    private float m_time;

    // Use this for initialization
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();       
        
        m_RotationSpeed = m_RotationSpeed / 20;
        m_time = Random.Range(0, 1 / (m_RotationSpeed * Time.deltaTime));
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        float x = m_Radius * Mathf.Cos(2 * Mathf.PI * m_RotationSpeed * Time.fixedDeltaTime*m_time);
        float y = m_Radius * Mathf.Sin(2 * Mathf.PI * m_RotationSpeed * Time.fixedDeltaTime*m_time);

        m_Rigidbody.MovePosition(m_Sun.transform.position + new Vector3(x, y));
        //m_Rigidbody.transform.position += new Vector3(x, y);
        m_time++;
        if (m_time >= 1/(m_RotationSpeed*Time.deltaTime)) m_time = 0;
    }
}
