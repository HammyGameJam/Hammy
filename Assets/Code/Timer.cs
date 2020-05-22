using UnityEngine;

public class Timer
{
	public bool Valid { get; private set; }

	private float m_start_time;
	private float m_delay;

	public void Start(float time)
	{
		m_start_time = Time.time;
		m_delay = time;

		Valid = true;
	}

	public bool Elapsed()
	{
		if (!Valid)
		{
			return false;
		}

		return m_start_time + m_delay < Time.time;
	}

	public void Invalidate()
	{
		Valid = false;
	}

	public float TimeSince()
	{
		return Time.time - m_start_time;
	}

	public float TimeLeft()
	{
		return m_delay - TimeSince();
	}
}
