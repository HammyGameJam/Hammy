using UnityEngine;

[CreateAssetMenu(fileName = "Destructible", menuName = "DestructibleAudio")]
public class DestructibleAudio : ScriptableObject
{
	public AudioClip m_Clip;
	public float m_Volume = 1.0f;
}
