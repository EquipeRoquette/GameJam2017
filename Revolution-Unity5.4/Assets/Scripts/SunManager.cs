using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunManager : MonoBehaviour {
    public int m_Number;
    public MainManager m_Manager;

	void OnMouseDown()
    {
        if (m_Manager.GetLevel() == m_Number)
            m_Manager.StartLevel();
        else
            m_Manager.SetLevel(m_Number);

    }
}
