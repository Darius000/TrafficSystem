using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WayPointNavigator : MonoBehaviour
{
    NavMeshAgent m_Agent;

    public WayPoint m_CurrentWaypoint;

    private int m_Direction;

    public bool DestinationReached { get { return m_Agent.remainingDistance < (m_Agent.radius + .1f); } private set { } }

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Direction = Mathf.RoundToInt(Random.Range(0f, 1f));

        m_Agent.SetDestination(m_CurrentWaypoint.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if(DestinationReached)
        {
            bool shouldBranch = false;

            if(m_CurrentWaypoint.m_Branches != null && m_CurrentWaypoint.m_Branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= m_CurrentWaypoint.BranchRatio ? true : false;
            }

            if (shouldBranch)
            {
                m_CurrentWaypoint = m_CurrentWaypoint.m_Branches[Random.Range(0, m_CurrentWaypoint.m_Branches.Count - 1)];
            }
            else
            {
                if (m_Direction == 0)
                {
                    if(m_CurrentWaypoint.m_Next != null)
                    {
                        m_CurrentWaypoint = m_CurrentWaypoint.m_Next;
                    }
                    else
                    {
                        m_CurrentWaypoint = m_CurrentWaypoint.m_Previous;
                        m_Direction = 1;
                    }
                }
                else if (m_Direction == 1)
                {
                    if(m_CurrentWaypoint.m_Previous  != null)
                    {
                        m_CurrentWaypoint = m_CurrentWaypoint.m_Previous;
                    }
                    else
                    {
                        m_CurrentWaypoint = m_CurrentWaypoint.m_Next;
                        m_Direction = 0;
                    }

                }
            }

            m_Agent.SetDestination(m_CurrentWaypoint.GetPosition());
        }
    }
}
