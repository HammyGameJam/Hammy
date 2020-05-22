using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskImplosionScript : MonoBehaviour
{
    public float m_min_force = 125.0f;
    public float m_max_force = 750.0f;
    public float m_radius = 10.0f;

    public void Explode()
    {
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            float force = Random.Range(m_min_force, m_max_force);
            rb.AddExplosionForce(force, transform.position, m_radius);
        }
    }

    void Start()
    {
        Explode();
    }
}
