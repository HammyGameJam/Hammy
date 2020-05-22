using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleAudioManager : MonoBehaviour
{
    public int m_MaxConcurrentSounds = 3;

    private static DestructibleAudioManager m_Inst = null;

    private List<AudioSource> m_AudioSources = new List<AudioSource>();


    public static void PlayDestrucibleAudio(DestructibleAudio destructibleAudio)
    {
        foreach (AudioSource audio in m_Inst.m_AudioSources)
        {
            if (!audio.isPlaying)
            {
                audio.volume = destructibleAudio.m_Volume;
                audio.PlayOneShot(destructibleAudio.m_Clip);
                break;
            }
        }
    }



    private void Awake()
    {
        m_Inst = this;
    }

    private void Start()
    {
        for (int i = 0; i < m_MaxConcurrentSounds; ++i)
        {
            m_AudioSources.Add(gameObject.AddComponent<AudioSource>());
        }
    }
}
