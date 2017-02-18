using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class MainManager : MonoBehaviour {
    public Camera m_Camera;
    public Rigidbody[] m_Suns;
    public Text m_MessageText;
    public Text m_ObjectiveText;
    public AudioSource m_SwipeSound;

    public static string test = "test";

    public static int[] Trophies;

    
    public float m_DampTime=0.5f;

    [HideInInspector] public int level;
    private Vector3 offset;
    private Vector3 m_MoveVelocity;
    private bool m_MouseControl;
    private bool m_AxisControl;
    private bool m_ShowMenu;
    private bool m_Paused;
    

   void Awake () {
      AirConsole.instance.onMessage += OnMessage;
   }
      

   void OnMessage (int device_id, JToken data) {
      Debug.Log (data);
      JToken ok = data ["OK"];
      if (ok != null) {
         StartLevel ();
      }
      JToken swipe = data ["swipeanalog-right"];
      if (swipe != null) {
         JToken message = swipe ["message"];
         if (message != null) {
            if (message ["degree"] != null ) {
               float degree = (float)message ["degree"];
               if (degree > 300 || degree < 60) {
                  if (level < m_Suns.Length - 1) {
                     level++;
                     m_SwipeSound.Play();
                  }
               }
               if (degree > 120 && degree < 240) {
                  if (level > 0) {
                     level--;
                     m_SwipeSound.Play();
                  }
               }

               m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position, m_Suns[level].transform.position + offset, ref m_MoveVelocity, m_DampTime); ;

            }
         }
      }
   }


    // Use this for initialization
    void Start() {
        level = 0;
        offset = m_Camera.transform.position + m_Suns[0].transform.position;
        m_MouseControl = false;
        m_AxisControl = false;
        m_ShowMenu = false;
        m_Paused = false;
        if (Trophies == null) {
           Trophies = new int[m_Suns.Length];
        }
        
	}

    void Update()
    {
        if (Input.GetButtonDown("Submit") && !m_Paused)
            StartLevel();

        if (Input.GetButtonDown("Cancel"))
        {
            m_Paused = true^m_Paused;
            m_ShowMenu = true ^ m_ShowMenu;
            if (m_Paused)
                DisableMovement();
            else
                EnableMovement();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(!m_Paused)
            CameraUpdate();

        TextUpdate();
            
    }

    private void TextUpdate()
    {
        if(level == 0)
        {
            m_MessageText.text = "Tutorial";
        }
        else
        {
            m_MessageText.text = "Level " + (level);
        }
        
        
        m_ObjectiveText.text = "Pelottes Pattrapées: " + Trophies[level] + "/3"; 
    }

    private void CameraUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (m_AxisControl && level > 0 && Input.GetAxis("Horizontal") < 0)
        {
            m_AxisControl = false;
            level -= 1;
            m_SwipeSound.Play();
        }
        else if (m_AxisControl && level < m_Suns.Length - 1 && Input.GetAxis("Horizontal") > 0)
        {
            m_AxisControl = false;
            level += 1;
            m_SwipeSound.Play();
        }
        else if (m_MouseControl && level > 0 && Input.mousePosition.x < Screen.width / 4)
        {
            m_MouseControl = false;
            level -= 1;
            m_SwipeSound.Play();
        }
        else if (m_MouseControl && level < m_Suns.Length - 1 && Input.mousePosition.x > 3 * Screen.width / 4)
        {
            level += 1;
            m_MouseControl = false;
            m_SwipeSound.Play();
        }

        m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position, m_Suns[level].transform.position + offset, ref m_MoveVelocity, m_DampTime); ;

        if (Input.mousePosition.x > Screen.width / 4 && Input.mousePosition.x < 3 * Screen.width / 4)
            m_MouseControl = true;
        if (Input.GetAxis("Horizontal") == 0)
            m_AxisControl = true;
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(level+1);
    }

    public void SetLevel(int lvl)
    {
        level = lvl;
    }

    public int GetLevel()
    {
        return level;
    }

    void OnGUI()
    {
        if (m_ShowMenu)
        {
            int buttonWidth = 100;
            // Make a background box
            GUI.Box(new Rect(Screen.width / 2 - buttonWidth/2-20, Screen.height / 3, buttonWidth+40, 90), "Do you want to quit ?");

            // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth/2, Screen.height / 3 + 30, buttonWidth, 20), "Keep playing"))
            {
                m_Paused = false;
                m_ShowMenu = false;
                EnableMovement();
            }

            // Make the second button.
            if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth/2, Screen.height / 3 + 60, buttonWidth, 20), "Quit game"))
            {
                m_ShowMenu = false;
                Application.Quit();
            }
        }
    }

   void OnDestroy () {

      // unregister airconsole events on scene change
      if (AirConsole.instance != null) {
         AirConsole.instance.onMessage -= OnMessage;
      }
   }

    private void DisableMovement()
    {
        foreach(Rigidbody item in m_Suns)
        {
            foreach(Transform child in item.transform)
                if (child.gameObject.tag == "MovingRock")
                    child.GetComponent<PlanetMovement>().enabled = false;
        }
    }

    private void EnableMovement()
    {
        foreach (Rigidbody item in m_Suns)
        {
            foreach (Transform child in item.transform)
                if (child.gameObject.tag == "MovingRock")
                    child.GetComponent<PlanetMovement>().enabled = true;
        }
    }

}
