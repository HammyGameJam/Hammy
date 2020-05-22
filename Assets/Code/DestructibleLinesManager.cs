using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleLinesManager : MonoBehaviour
{
    public List<NarrativeLine> m_Lines;

    public float m_MinCooldown = 20.0f;

    private Timer m_CooldownTimer = new Timer();

    private void OnDestructibleDestroyed(DestructibleDestroyedEvent e)
    {
        if (m_CooldownTimer.Valid && !m_CooldownTimer.Elapsed())
        {
            return;
        }

        if (HammyGameStateManager.Get().GameState == EGameState.FREE_ROAM && HammyGameStateManager.Get().GetFreeTimeLeft() < 5.0f)
        {
            return;
        }

        if (m_Lines.Count > 0)
        {
            NarrativeLine line = m_Lines[Random.Range(0, m_Lines.Count)];
            NarrativeAudioManager.Get().PlayLine(line);
        }

        m_CooldownTimer.Start(m_MinCooldown);
    }

    private void Awake()
    {
        GameEvent<DestructibleDestroyedEvent>.Register(OnDestructibleDestroyed);
    }

    private void OnDestroy()
    {
        GameEvent<DestructibleDestroyedEvent>.Unregister(OnDestructibleDestroyed);
    }
}
