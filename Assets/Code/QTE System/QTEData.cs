using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_qte_data", menuName = "QTE")]
public class QTEData : ScriptableObject
{
	public QTEType m_type;

	public string m_input;

	public AnimationCurve m_mash_power_curve;
	public AnimationCurve m_mash_falloff_curve;
	public float m_mash_power = 0.1f;
	public float m_mash_falloff = 0.1f;
}

public enum QTEType
{
	Mash
}