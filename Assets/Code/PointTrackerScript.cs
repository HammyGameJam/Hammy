using UnityEngine;

struct PointsAddedEvent : IGameEvent
{
    public int amount;
    public int total;
}

public class PointTrackerScript : MonoBehaviour
{
    public int Points { get; private set; }


    public static PointTrackerScript Get()
    {
        return (PointTrackerScript)GameObject.FindObjectOfType<PointTrackerScript>();
    }

    public void AddPoints(int points)
    {
        Points += points;

        PointsAddedEvent e;
        e.amount = points;
        e.total = Points;
        GameEvent<PointsAddedEvent>.Post(e);
    }

    private void Awake()
    {
        Debug.Assert(GameObject.FindObjectsOfType<PointTrackerScript>().Length == 1, "Should only be one AvatarPointTracker script.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log(Points);
        }
    }
}
