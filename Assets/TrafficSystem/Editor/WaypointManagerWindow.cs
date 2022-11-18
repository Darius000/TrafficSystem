using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform Root;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("Root"));

        if(Root == null)
        {
            EditorGUILayout.HelpBox(new GUIContent("Root transfrom must be selected. Please assign a Root!"));
        }
        else
        {
            EditorGUILayout.BeginVertical("Box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {

        if (GUILayout.Button("Connect Loop"))
        {
            ConnectFirstAndLastWaypoint();
        }

        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }

        if(Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<WayPoint>()) 
        {
            if (GUILayout.Button("Add Branch"))
            {
                CreateBranch();
            }

            if (GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore();
            }

            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }

            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
            }

        }
    }

    private void CreateBranch()
    {
        GameObject waypointObject = new GameObject("Waypoint " + Root.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(Root, false);

        WayPoint wayPoint = waypointObject.GetComponent<WayPoint>();

        WayPoint branchedFrom = Selection.activeGameObject.GetComponent<WayPoint>();
        branchedFrom.m_Branches.Add(wayPoint);

        wayPoint.transform.position = branchedFrom.transform.position;
        wayPoint.transform.forward = branchedFrom.transform.forward;

        Selection.activeGameObject = wayPoint.gameObject;
    }

    private void ConnectFirstAndLastWaypoint()
    {
        if(Root.childCount > 2)
        {
            WayPoint First = Root.GetChild(0).GetComponent<WayPoint>();
            WayPoint Last = Root.GetChild(Root.childCount - 1).GetComponent<WayPoint>();

            First.m_Previous = Last;
            Last.m_Next = First;
        }
    }

    private void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + Root.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(Root, false);

        WayPoint wayPoint = waypointObject.GetComponent<WayPoint>();
        if(Root.childCount > 1) 
        {
            wayPoint.m_Previous = Root.GetChild(Root.childCount - 2).GetComponent<WayPoint>();
            wayPoint.m_Previous.m_Next = wayPoint;

            //place waypoint at last position
            wayPoint.transform.position = wayPoint.m_Previous.transform.position;
            wayPoint.transform.forward = wayPoint.m_Previous.transform.forward;
        }

        Selection.activeObject = wayPoint.gameObject;
    }

    private void CreateWaypointBefore()
    {
        GameObject waypointObject = new GameObject("Waypoint " + Root.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(Root, false);

        WayPoint newWaypoint = waypointObject.GetComponent<WayPoint>();

        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        if(selectedWaypoint.m_Previous != null)
        {
            newWaypoint.m_Previous = selectedWaypoint.m_Previous;
            selectedWaypoint.m_Previous.m_Next = newWaypoint;
        }

        newWaypoint.m_Next = selectedWaypoint;

        selectedWaypoint.m_Previous = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeObject = newWaypoint.gameObject;
    }

    private void CreateWaypointAfter()
    {
        GameObject waypointObject = new GameObject("Waypoint " + Root.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(Root, false);

        WayPoint newWaypoint = waypointObject.GetComponent<WayPoint>();

        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.m_Previous = selectedWaypoint;

        if(selectedWaypoint.m_Next != null)
        {
            selectedWaypoint.m_Next.m_Previous = newWaypoint;
            newWaypoint.m_Next = selectedWaypoint.m_Next;
        }

        selectedWaypoint.m_Next = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex() + 1);

        Selection.activeObject = newWaypoint.gameObject;
    }

    private void RemoveWaypoint()
    {
        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();

        if(selectedWaypoint.m_Next != null ) 
        { 
            selectedWaypoint.m_Next.m_Previous = selectedWaypoint.m_Previous;
        }

        if(selectedWaypoint.m_Previous != null )
        {
            selectedWaypoint.m_Previous.m_Next = selectedWaypoint.m_Next;
            Selection.activeObject = selectedWaypoint.m_Previous.gameObject;
        }

        DestroyImmediate( selectedWaypoint.gameObject );
    }
}
