using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace TrafficSystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class WaypointGoalNavigator : MonoBehaviour
    {
        NavMeshAgent m_Agent;

        [SerializeField]
        private List<WayPoint> m_Open;


        private int m_CurrentWaypointIndex = 0;

        public WayPoint m_Start;
        public WayPoint m_Goal;

        private void Awake()
        {
            m_Agent = GetComponent<NavMeshAgent>();
        }

        // Use this for initialization
        void Start()
        {
            m_Open = new List<WayPoint>() { m_Start };

            AStar();

            if(m_CurrentWaypointIndex < m_Open.Count - 1)
                m_Agent.SetDestination(m_Open[m_CurrentWaypointIndex].GetPosition());
        }

        // Update is called once per frame
        void Update()
        {
            if(m_Agent.remainingDistance < m_Agent.radius + .1f)
            {
                if(m_CurrentWaypointIndex < m_Open.Count - 1)
                {
                    m_Agent.SetDestination(m_Open[++m_CurrentWaypointIndex].GetPosition());
                }
            }

        }

        float CalculateDistance(WayPoint a, WayPoint b)
        {
            return Vector3.Distance(m_Start.transform.position, a.transform.position);
        }

        /// <summary>
        /// g(n) start node to (n)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        float CalculateMovementCost(WayPoint current, WayPoint next)
        {
            return Vector3.Distance(current.transform.position, next.transform.position);
        }

        /// <summary>
        /// Euclidean Distance h(n) (n) node to goal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        float CalculateHeuristics(WayPoint next)
        {
            return Vector3.Distance(next.transform.position, m_Goal.transform.position);
        }


        float F(WayPoint current, WayPoint next)
        {
            return CalculateMovementCost(current, next) + CalculateHeuristics(next);
        }

        private void AStar()
        {
            //find smllest value
            WayPoint current = m_Start;


            while(current != m_Goal && current != null)
            {
                List<WayPoint> successors = new List<WayPoint>();

                if(!m_Open.Contains(current.m_Previous) && current.m_Previous)
                {
                    successors.Add(current.m_Previous);
                }

                if (!m_Open.Contains(current.m_Next) && current.m_Next)
                {
                    successors.Add(current.m_Next);
                }

                if (current.m_Branches != null)
                {
                    foreach(WayPoint wayPoint in current.m_Branches)
                    {
                        if(!m_Open.Contains(wayPoint) && wayPoint)
                        {
                            successors.Add(wayPoint);
                        }
                    }
                }

                if(successors.Count > 0)
                {
                    successors.Sort((WayPoint lhs, WayPoint rhs) => {
                        return F(current, lhs) < F(current, rhs) ? -1 : 1;
                    });

                    current = successors[0];
                    m_Open.Add(current);

                    if (current == m_Goal)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            
        }
    }
}