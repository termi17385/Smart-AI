/*using System.Collections.Generic;
using UnityEngine.AI;
using SmartAI.Doors;
using UnityEngine;

namespace SmartAI.Artificial_Intelligence
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class TestAgent : MonoBehaviour
	{
		private TestAgentMachine states;
		
		[SerializeField] private float wanderingTime;
		[SerializeField] private int wanderRadius;
		public List<Transform> pointsOfInterests = new List<Transform>();

		private float timer;
		protected NavMeshAgent agent;
		
		[SerializeField] protected Transform testPath;
		protected Transform target;
		[SerializeField] private float maxDistance;

		protected virtual void Awake()
		{
			agent = GetComponent<NavMeshAgent>();
			states = GetComponent<TestAgentMachine>();
			timer = wanderingTime;
		}

		protected virtual void Update()
		{
			bool valid = agent.pathStatus != NavMeshPathStatus.PathInvalid;
			var debugInfo = $"PathStatus:{agent.pathStatus}\n ValidPath? {valid}";
			Debug.Log(debugInfo);
		}

		protected Transform ReturnTarget()
		{
			foreach(var obj in pointsOfInterests)
			{
				var dist = Vector3.Distance(transform.position, obj.position);
				if(dist <= maxDistance) { return obj; }
			}

			return null;
		}

		protected void AgentWander()
		{
			timer += Time.deltaTime;
			if(target != null) states.ChangeState(AgentStates.MoveTo);
			if(timer >= wanderingTime || agent.remainingDistance <= 0.5f)
			{
				Vector3 newPos = AIUtils.RandomNavSphere(transform.position, wanderRadius, -1);
				agent.SetDestination(newPos);
				agent.stoppingDistance = 0;
				agent.speed = 3.5f;
				timer = 0;
			}
			LookAtTarget();
			DetectDoor();
		}
		
		protected void AgentMoveToTarget(Vector3 _target)
		{
			agent.SetDestination(_target);
			LookAtTarget();
			agent.speed = 3.5f;
			agent.stoppingDistance = 2;
			
			if(target != null) if(Vector3.Distance(transform.position, target.position) <= 2) states.ChangeState(AgentStates.Interact);
			if(_target == testPath.position) DetectDoor(); 
		}
		
		protected void InteractWithObject(GameObject _targetObj)
		{
			if(_targetObj.TryGetComponent(out Switch obj))
			{
				if(!obj.Activated)
				{
					obj.ToggleSwitch();
					for(int i = pointsOfInterests.Count - 1; i >= 0; i--)
					{
						if(pointsOfInterests[i] == _targetObj.transform)
						{
							target = null;
							pointsOfInterests.RemoveAt(i);
							states.ChangeState(AgentStates.FollowPath);
							break;
						} 
					}
				}
			}	
		}
		
		protected void AgentStop() => agent.speed = 0;
		
		/// <summary> Makes the agent look
		/// towards the path they are taking </summary>
		public void LookAtTarget()
		{
			var position = transform.position;
			var position1 = agent.steeringTarget;
     
			// gets the normalised direction
			Vector3 direction = (position1 - position).normalized;
			// sets the look rotation
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
			// makes the agent rotate to look in that direction
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
		}
		
		private Color color = Color.green;
		[SerializeField] private LayerMask doorsOnly;

		private float timer2 = 0;
		private float maxTime = .5f;
		protected void DetectDoor()
		{
			Vector3 offset = transform.right * 3.5f;
			var ray1 = Physics.Raycast(transform.position, (transform.forward * 6) + offset, 6, doorsOnly);
			var ray2 = Physics.Raycast(transform.position, (transform.forward * 6), out RaycastHit hit,6, doorsOnly);
			var ray3 = Physics.Raycast(transform.position, (transform.forward * 6) - offset,  6,doorsOnly);
			bool hitDetection = ray1 || ray2 || ray3;

			color = hitDetection ? Color.red : Color.green;
			if(hitDetection)
			{
				GameObject gObject = hit.collider.gameObject;
				if(gObject.GetComponent<BaseDoor>().open) return;

				timer2 += Time.deltaTime;
				if(timer2 >= maxTime)
				{
					timer2 = 0;
				}
			}
		}
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, wanderRadius);
			Gizmos.color = color;
			Vector3 offset = transform.right * 3.5f;
			Gizmos.DrawRay(transform.position, (transform.forward * 6) + offset);
			Gizmos.DrawRay(transform.position, (transform.forward * 6));
			Gizmos.DrawRay(transform.position, (transform.forward * 6) - offset);
		}
	}
}*/