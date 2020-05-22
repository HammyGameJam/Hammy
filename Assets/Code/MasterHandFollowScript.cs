using UnityEngine;
using UnityEngine.AI;

struct MasterHandTouchedTargetEvent : IGameEvent
{
}

[RequireComponent(typeof(Rigidbody))]
public class MasterHandFollowScript : MonoBehaviour
{
    public NavMeshAgent m_ghost_agent;
    public GameObject m_target = null;

    public float m_speed = 0.0f;
    public float m_rotate_speed = 0.0f;

    private Rigidbody rb;

    private void Start()
    {
        m_ghost_agent.speed = m_speed;
        rb = GetComponent<Rigidbody>();
    }

    private void Rotate()
    {
        Vector3 direction = (m_target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, m_rotate_speed * Time.fixedDeltaTime);
    }

    private void Move()
    {
        Vector3 targetPos = m_target.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 20.0f, NavMesh.AllAreas))
        {
            targetPos = hit.position;
        }

        NavMeshPath path = new NavMeshPath();
        m_ghost_agent.CalculatePath(targetPos, path);

        float delta = m_speed * Time.fixedDeltaTime;
        for (int i = 0; i < path.corners.Length; ++i)
        {
            Vector3 corner = path.corners[i];

            Vector3 src = new Vector3(m_ghost_agent.transform.position.x, corner.y, m_ghost_agent.transform.position.z);
            if (Vector3.Distance(src, corner) > delta)
            {
                Vector3 realTarget = new Vector3(corner.x, m_target.transform.position.y, corner.z);

                bool is_jump_corner = (i + 1 < path.corners.Length) ? (Mathf.Abs(corner.y - path.corners[i+1].y) > 3.0f) : false;

                if (is_jump_corner)
                {
                    bool directly_above_corner = Mathf.Abs(transform.position.x - corner.x) < 0.8f && Mathf.Abs(transform.position.z - corner.z) < 0.8f;
                    if (directly_above_corner)
                    {
                        // Warp right to the corner
                        m_ghost_agent.transform.position = corner;

                        // Let the native AI handle the jump
                        m_ghost_agent.destination = m_target.transform.position;
                    }
                    else
                    {
                        m_ghost_agent.isStopped = true;
                        m_ghost_agent.ResetPath();

                        m_ghost_agent.transform.position = Vector3.MoveTowards(m_ghost_agent.transform.position, realTarget, delta);
                    }
                }
                else
                {
                    m_ghost_agent.isStopped = true;
                    m_ghost_agent.ResetPath();

                    m_ghost_agent.transform.position = Vector3.MoveTowards(m_ghost_agent.transform.position, realTarget, delta);
                }

                transform.position = Vector3.MoveTowards(transform.position, realTarget, delta);
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_target != null)
        {
            Rotate();
            Move();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == m_target)
        {
            GameEvent<MasterHandTouchedTargetEvent>.Post();
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (m_target != null)
            {
                NavMeshPath path = new NavMeshPath();
                m_ghost_agent.CalculatePath(m_target.transform.position, path);

                for (int i = 0; i < path.corners.Length - 1; ++i)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
                }
            }
        }
#endif
    }
}
