using SmartAI.Doors;

using System;
using System.Collections.Generic;

using UnityEngine.AI;
using UnityEngine;

namespace SmartAI.Artificial_Intelligence
{
	[RequireComponent(typeof(NavMeshAgent), typeof(Waypoints))]
	public class Agent : MonoBehaviour
	{
		[SerializeField] private Vector3 point;
		[SerializeField] private Transform targetObject;

		private NavMeshAgent navAgent;
		private AgentMachine machine;
		private Waypoints mainPath;
		private NavMeshPath path;

		protected virtual void Start()
		{
			mainPath = GetComponent<Waypoints>();
			navAgent = GetComponent<NavMeshAgent>();

			machine = GetComponent<AgentMachine>();
			path = new NavMeshPath();
		}

		protected virtual void Update()
		{
			if(targetObject != null)
			{
				var dist = Vector3.Distance(transform.position, targetObject.position);
				if(dist <= 3.5f)
				{
					agentStopped = true;
					navAgent.stoppingDistance = 1;
					
					navAgent.speed = 0;
					machine.ChangeState(AgentStates.Interact);
				}
			}
		}

		// follow the set path
		protected void NavigatePath()
		{
			// check if the path is valid making sure nothing is blocking the path
			point = mainPath.SetNextWaypoint(transform).position;
			navAgent.CalculatePath(point, path);
			Debug.Log(path.status);

			// follow the path
			agentStopped = false;
			navAgent.speed = 3.5f;
			navAgent.stoppingDistance = 0;
			if(path.status == NavMeshPathStatus.PathComplete) navAgent.SetDestination(point);
			else machine.ChangeState(AgentStates.Search);

			// if there is a blockage check for alternative routes or a door
			// if an alternative route is found go towards that route and follow it
			// else if the path is blocked by a door search around for a key
		}

		private readonly float searchRadius = 15;
		private bool agentStopped = false;

		protected void WanderAround()
		{
			point = mainPath.SetNextWaypoint(transform).position;
			navAgent.CalculatePath(point, path);
			
			if(path.status == NavMeshPathStatus.PathPartial)
			{
				if(navAgent.remainingDistance <= 0.5f)
				{
					Vector3 pos = AIUtils.RandomNavSphere(transform.position, 10, 1);
					navAgent.SetDestination(pos);
					navAgent.speed = 3.5f;
				}
			} 
			else machine.ChangeState(AgentStates.FollowPath);
			SearchForObjectsOfInterest();
		}
		
		protected void SearchForObjectsOfInterest()
		{
			var colliders = Physics.OverlapSphere(transform.position, searchRadius);
			navAgent.CalculatePath(point, path);
			Debug.Log(path.status);
			
			if(path.status == NavMeshPathStatus.PathComplete) machine.ChangeState(AgentStates.FollowPath);
			
			Transform tMin = null;
			float minDist = Mathf.Infinity;
			Vector3 currentPos = transform.position;
			
			foreach(var other in colliders)
			{
				if(other.CompareTag("Objects"))
				{
					float dist = Vector3.Distance(other.transform.position, currentPos);
					if (dist < minDist)
					{
						if(other.TryGetComponent(out Switch @switch))
							if(@switch.Activated) return;
						
						tMin = other.transform;
						minDist = dist;
					}
					Debug.Log("found");
				}
			}
			targetObject = tMin;
			if(targetObject != null) machine.ChangeState(AgentStates.MoveTo);
		}
		protected void MoveTowardsObject()
		{
			if(targetObject != null)
			{
				Debug.Log(targetObject.gameObject);
				navAgent.SetDestination(targetObject.position);
				if(!agentStopped)navAgent.speed = 4.5f;
			}
			else machine.ChangeState(AgentStates.Search);
		}
		protected void InteractWithObject()
		{
			if(targetObject != null)
			{
				if(targetObject.name.Contains("Switch"))
				{
					var @switch = targetObject.GetComponent<Switch>();
					if(!@switch.Activated) @switch.ToggleSwitch();
				}
			}
			targetObject = null;
			machine.ChangeState(AgentStates.FollowPath);
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, searchRadius);
		}

		// when detecting a door or a blockage search for alternative routes

		// if the door is closed search for a switch if it is locked search for the key then find the switch
	}
}