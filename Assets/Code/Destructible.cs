using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct DestructibleDestroyedEvent : IGameEvent
{
	public GameObject destructible;
}


public class Destructible : MonoBehaviour
{
	public GameObject m_husk_prefab;
	public DestructibleAudio m_explosion_audio;

	public float m_breaking_impulse = 3.0f;

	public bool m_explode_on_destroy = true;
	public float m_min_explosion_force = 125.0f;
	public float m_max_explosion_force = 750.0f;
	public float m_explosion_radius = 20.0f;

	private void Explode(GameObject obj)
	{
		foreach (Transform tr in obj.transform)
		{
			Rigidbody rb = tr.gameObject.GetComponent<Rigidbody>();
			float force = Random.Range(m_min_explosion_force, m_max_explosion_force);
			rb.AddExplosionForce(force, obj.transform.position, m_explosion_radius);
		}
	}

	private void SpawnHusk()
	{
		if (m_husk_prefab != null)
		{
			GameObject husk = HamsterBallSimulator.Get().SpawnGameObject(m_husk_prefab, transform.position, transform.rotation);

			// Inherit physics properties
			Rigidbody my_rb = GetComponent<Rigidbody>();
			if (my_rb != null)
			{
				Vector3 velocity = my_rb.velocity;
				Vector3 angularVelocity = my_rb.angularVelocity;

				foreach (var rb in GetComponentsInChildren<Rigidbody>())
				{
					rb.velocity = velocity;
					rb.angularVelocity = angularVelocity;
				}
			}

			// Explode
			if (m_explode_on_destroy)
			{
				Explode(husk);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.impulse.magnitude > m_breaking_impulse)
		{
			if (collision.gameObject.CompareTag("Player"))
			{
				DestructibleDestroyedEvent e;
				e.destructible = gameObject;
				GameEvent<DestructibleDestroyedEvent>.PostTargeted(gameObject, e);
				GameEvent<DestructibleDestroyedEvent>.Post(e);

				SpawnHusk();
				Destroy(gameObject);

				if (m_explosion_audio != null)
				{
					DestructibleAudioManager.PlayDestrucibleAudio(m_explosion_audio);
				}
			}
		}
	}
}
