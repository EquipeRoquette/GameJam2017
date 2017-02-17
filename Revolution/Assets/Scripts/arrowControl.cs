using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class arrowControl : MonoBehaviour {
	public ReleaseGameManager rGM;
	public Vector2 mouseDiff;
	public float arrowAngle = 0F;
	public float distance = 0F;
	public float forceFactor = 0.005F;

   public bool up = false;
   public bool down = false;

   public Vector2 velocity;
   public Vector3 rotations;
   public float rotation;

   public float speed = 20;

   //Quaternion targetAngle;

   void Awake () {
      AirConsole.instance.onMessage += OnMessage;
      AirConsole.instance.onConnect += OnConnect;
      AirConsole.instance.onDisconnect += OnDisconnect;
   }

   /// <summary>
   /// We start the game if 2 players are connected and the game is not already running (activePlayers == null).
   /// 
   /// NOTE: We store the controller device_ids of the active players. We do not hardcode player device_ids 1 and 2,
   ///       because the two controllers that are connected can have other device_ids e.g. 3 and 7.
   ///       For more information read: http://developers.airconsole.com/#/guides/device_ids_and_states
   /// 
   /// </summary>
   /// <param name="device_id">The device_id that connected</param>
   void OnConnect (int device_id) {
      if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0) {
         if (AirConsole.instance.GetControllerDeviceIds ().Count >= 1) {
            StartGame ();
         } 
      }
   }

   /// <summary>
   /// If the game is running and one of the active players leaves, we reset the game.
   /// </summary>
   /// <param name="device_id">The device_id that has left.</param>
   void OnDisconnect (int device_id) {
      int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
      if (active_player != -1) {
         if (AirConsole.instance.GetControllerDeviceIds ().Count >= 1) {
            StartGame ();
         } 
      }
   }

   /// <summary>
   /// We check which one of the active players has moved the paddle.
   /// </summary>
   /// <param name="from">From.</param>
   /// <param name="data">Data.</param>
   void OnMessage (int device_id, JToken data) {
      /*
      int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
      if (active_player != -1) {
         if (active_player == 0) {
            this.racketLeft.velocity = Vector3.up * (float)data ["move"];
         }
         if (active_player == 1) {
            this.racketRight.velocity = Vector3.up * (float)data ["move"];
         }
      }
      */
      /*
      Debug.Log (data);
      JToken joystick_left = data ["joystick-left"];
      if (joystick_left != null) {
         JToken message = joystick_left ["message"];
         if (message != null) {
            if (message ["x"] != null && message["y"] != null) {
               float x = (float)message ["x"];
               float y = -(float)message ["y"];

               setArrowAngle (new Vector2 (x, y));

               Debug.Log ("x:" + x + "  y:" + y);
            }
         }
      }
      */
      Debug.Log (data);
      JToken up_message = data ["up"];
      if (up_message != null) {
         JToken pressed = up_message ["pressed"];
         if (pressed != null) {
            up = (bool)pressed;
         }
      }
      JToken down_message = data ["down"];
      if (down_message != null) {
         JToken pressed = down_message ["pressed"];
         if (pressed != null) {
            down = (bool)pressed;
         }
      }
      JToken swipe = data ["swipeanalog-right"];
      if (swipe != null) {
         JToken message = swipe ["message"];
         if (message != null) {
            if (message ["speed"] != null) {
               rotations = this.transform.rotation.eulerAngles;
               rotation = rotations.z;
               velocity = new Vector2 (Mathf.Cos (rotation*Mathf.Deg2Rad), Mathf.Sin (rotation*Mathf.Deg2Rad)).normalized;
               rGM.launchSatellite (this.transform.position, velocity * (float) message["speed"] * forceFactor);
            }
         }
      }

     // this.transform.Rotate (Vector3.forward * (float)data ["move"]);
   }
      
   void OnDestroy () {

      // unregister airconsole events on scene change
      if (AirConsole.instance != null) {
         AirConsole.instance.onMessage -= OnMessage;
      }
   }

   void StartGame () {
      AirConsole.instance.SetActivePlayers (1);

   }

	// Use this for initialization
	void Start () {
     // targetAngle = this.transform.rotation;
	}

   void FixedUpdate() {
      if (up) {
         this.transform.Rotate (Vector3.forward * speed * Time.fixedDeltaTime);
      }

      if (down) {
         this.transform.Rotate (Vector3.back * speed * Time.fixedDeltaTime);
      }
   }
	
	// Update is called once per frame
	void Update () {		 /*
		mouseDiff = getMouseDiff();

		setArrowAngle (mouseDiff);

		distance = getMouseDistance (mouseDiff);

		this.transform.localScale = getArrowScale (distance);

		//arrow.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Pressed left click.");
			rGM.launchSatellite (this.transform.position, getMouseDiff().normalized * getPower () * forceFactor);
		} */

	}

	Vector2 getMouseDiff() {		
		Vector3 v3Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
		v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
		v3Pos = v3Pos - this.transform.position;
		return new Vector2(v3Pos.x, v3Pos.y);
	}

	void setArrowAngle(Vector2 diff) {
		if (diff.x != 0)
			arrowAngle = Mathf.Atan (diff.y / diff.x) * Mathf.Rad2Deg;
		if (diff.x < 0) 
			arrowAngle = 180 + arrowAngle;


		Quaternion target = Quaternion.Euler(0, 0, arrowAngle);
     // targetAngle = target;
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
