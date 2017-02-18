using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenu : MonoBehaviour {
    public Camera m_Camera;

    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = new Vector3(0, 0, 3);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = m_Camera.transform.position + offset;
	}
}
