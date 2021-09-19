using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Waypoints : MonoBehaviour
{
    [Header("Agents and Waypoints")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private Transform content;
    
    [Header("Colours")]
    [SerializeField] private Color waypointColor;
    [SerializeField] private Color agentColor;
    
    [Space]
    
    [SerializeField, Tooltip("max distance before going to the next waypoint")] private float distance;
    [SerializeField, Tooltip("DO NOT TICK IN PLAYMODE (enables waypoint creation in editor)")] private bool debug = false;
    private int count;
    [SerializeField] private int index;

    /// <summary> Modified from original code still works the same premise just simplified </summary>
    /// <param name="_agentPos">the agents transform for comparing the distance</param>
    public Transform SetNextWaypoint(Transform _agentPos) // from agent index output the transform
    {
        // when the agent reaches the current waypoint
        // change to the next waypoint in the list
        if(Vector3.Distance(_agentPos.transform.position, waypoints[index].position) < distance)
        {
            // if the agent reaches the end of the count it will then start going backwards down the list
            index += count;
            switch (index >= waypoints.Count - 1)
            {
                case true:
                    count = -1;
                    //break;
                    break;
                default:
                {
                    if (index <= 0)
                    {
                        count = 1;
                    }

                    break;
                }
            }
        }

        return waypoints[index];
    }
    
    private void OnDrawGizmos()
    {
        if(waypoints.Count >= 2)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                // adds lines between each waypoint to display the path
                var x = i + 1;
                if(x < waypoints.Count) Debug.DrawLine(waypoints[i].position, waypoints[x].position, waypointColor, .5f);
            }
        }
        
        // if in debug mode
        if (debug)
        {  
            // will clear all waypoints
            waypoints.Clear();
            // then grab all waypoints that are a child of the waypoint manager
            foreach (Transform child in content)
            {
                // and adds them to the list making sure not to add duplicates
                if(!waypoints.Contains(child))
                    waypoints.Add(child);
            }
        }
    }
}