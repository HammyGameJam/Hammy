using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleController : MonoBehaviour
{
    public Text subtitleText;
    private Animator anim;
    
    public static SubtitleController Get()
    {
        return (SubtitleController)GameObject.FindObjectOfType<SubtitleController>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        anim = subtitleText.GetComponent<Animator>();
        anim.enabled = false;
        subtitleText.enabled = false;
    }

    public void PlaySubtitle(string text)
    {
        anim.enabled = true;
        subtitleText.enabled = true;
        subtitleText.text = text;
    }
}
