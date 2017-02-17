using UnityEngine;
using System.Collections;

public class arrowControl : MonoBehaviour {
	public Vector2 mouseDiff;
	public float arrowAngle = 0F;
	public float distance = 0F;
	public float forceFactor = 5.0F;
	// Use this for initialization

    private GameManager rGM;

	void Start () {
	    rGM = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		mouseDiff = getMouseDiff();

		setArrowAngle (mouseDiff);

		distance = getMouseDistance (mouseDiff);

		this.transform.localScale = getArrowScale (distance);

		//arrow.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
		if (rGM!= null && Input.GetMouseButtonDown (0)) {
			Debug.Log ("Pressed left click.");
			rGM.launchSatellite(this.transform.position, getMouseDiff().normalized * 2 * getPower () * forceFactor);
		}

	}

	Vector2 getMouseDiff() {		
		Vector3 v3Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
		v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
		v3Pos = (v3Pos - this.transform.position);
		return new Vector2(v3Pos.x, v3Pos.y);
	}

	void setArrowAngle(Vector2 diff) {
		if (diff.x != 0)
			arrowAngle = Mathf.Atan (diff.y / diff.x) * Mathf.Rad2Deg;
		if (diff.x < 0) 
			arrowAngle = 180 + arrowAngle;

		Quaternion target = Quaternion.Euler(0, 0, arrowAngle);
		this.transform.rotation = target;
	}

	float getMouseDistance(Vector2 diff) {
		return diff.magnitude;
	}

	Vector3 getArrowScale(float distance) {
		return new Vector3 (1, 1, 1) * getPower() / 2;
	}

	float getPower() {
		float distance = getMouseDistance (getMouseDiff());
		distance = Mathf.Min (distance, 24);
		distance = Mathf.Max (distance, 2);
		distance = Mathf.Pow (distance, 1F/4F);
		return distance;
	}
}
