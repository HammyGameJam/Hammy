using UnityEngine;

public class PointGiverScript : MonoBehaviour
{
    [SerializeField]
    private int m_points = 0;

    private void OnDestructibleDestroyed(DestructibleDestroyedEvent e)
    {
        PointTrackerScript.Get().AddPoints(m_points);
        GameEvent<DestructibleDestroyedEvent>.Unregister(OnDestructibleDestroyed);
    }

    private void Awake()
    {
        GameEvent<DestructibleDestroyedEvent>.RegisterTargeted(gameObject, OnDestructibleDestroyed);
    }

    private void OnDestroy()
    {
        GameEvent<DestructibleDestroyedEvent>.Unregister(OnDestructibleDestroyed);
    }
}
