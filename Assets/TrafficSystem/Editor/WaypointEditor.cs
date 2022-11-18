using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WaypointEditor
{
    [DrawGizmo(GizmoType.Pickable | GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    public static void OnDrawGizmo(WayPoint wayPoint, GizmoType gizmoType)
    {
        if((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }


        Gizmos.DrawSphere(wayPoint.transform.position, .1f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(wayPoint.transform.position + (wayPoint.transform.right * wayPoint.m_Width / 2f),
            wayPoint.transform.position - (wayPoint.transform.right * wayPoint.m_Width / 2f));

        if(wayPoint.m_Previous != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = wayPoint.transform.right * wayPoint.m_Width / 2f;
            Vector3 offsetTo = wayPoint.m_Previous.transform.right * wayPoint.m_Previous.m_Width / 2f;

            Gizmos.DrawLine(wayPoint.transform.position + offset, wayPoint.m_Previous.transform.position + offsetTo);
        }

        if(wayPoint.m_Next != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = wayPoint.transform.right * -wayPoint.m_Width / 2f;
            Vector3 offsetTo = wayPoint.m_Next.transform.right * -wayPoint.m_Next.m_Width / 2f;

            Gizmos.DrawLine(wayPoint.transform.position + offset, wayPoint.m_Next.transform.position + offsetTo);
        }

        if(wayPoint.m_Branches != null)
        {
            foreach(WayPoint branch in wayPoint.m_Branches)
            {
                if(branch == null) continue;
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(wayPoint.transform.position, branch.transform.position);
            }
        }
    }
}
