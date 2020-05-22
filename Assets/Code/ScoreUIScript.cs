using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreUIScript : MonoBehaviour
{
    //public Text m_Text;
    public TextMeshProUGUI m_Text;

    private void Start()
    {
        m_Text.text = "";
    }

    private void Update()
    {
        if (PointTrackerScript.Get() != null)
        {
            m_Text.text = "Score: " + PointTrackerScript.Get().Points.ToString("F0");
        }
    }
}
