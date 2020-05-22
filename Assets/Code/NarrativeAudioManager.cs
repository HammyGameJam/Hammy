using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NarrativeAudioManager : MonoBehaviour
{
    private static NarrativeAudioManager m_inst = null;

    private AudioSource m_audio;

    public static NarrativeAudioManager Get()
    {
        return m_inst;
    }


    public bool PlayLine(NarrativeLine line)
    {
        if (m_audio.isPlaying)
        {
            return false;
        }

        m_audio.clip = line.m_clip;
        m_audio.Play();

        return true;
    }

    public bool IsPlaying()
    {
        return m_audio.isPlaying;
    }



    private void Awake()
    {
        Debug.Assert(GameObject.FindObjectsOfType<NarrativeAudioManager>().Length == 1, "There should only be one NarrativeAudioManager in the scene.");
        m_inst = this;
    }

    private void Start()
    {
        m_audio = GetComponent<AudioSource>();
    }
}
