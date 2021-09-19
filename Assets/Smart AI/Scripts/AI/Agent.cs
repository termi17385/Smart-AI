using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using SmartAI.Doors;
using UnityEngine;

namespace SmartAI.Artificial_Intelligence
{
	[RequireComponent(typeof(NavMeshAgent), typeof(Waypoints))]
	public class Agent : MonoBehaviour
	{
		[SerializeField] private Vector3 point;
		public AgentMachine machine;
		
		protected ThirdPersonCharacter tpCharacter;
		protected NavMeshAgent navAgent;
		
		private Transform targetObject;
		private Waypoints mainPath;
		private NavMeshPath path;

		protected virtual void Start()
		{
			
			mainPath = GetComponent<Waypoints>();
			navAgent = GetComponent<NavMeshAgent>();

			tpCharacter = GetComponent<ThirdPersonCharacter>();
			machine = GetComponent<AgentMachine>();
			
			navAgent.updateRotation = false;
			path = new NavMeshPath();
		}

		/// <summary> Checks the distance to the specified target before interacting with it </summary>
		/// <param name="_transform">the target to check the distance with</param>
		private void CheckDistThenInteract(Transform _transform)
		{
			var dist = Vector3.Distance(transform.position, _transform.position);
			if(dist <= 2.5f) machine.ChangeState(AgentStates.Interact);
		}

		/// <summary> Navigates the given
		/// path towards a goal </summary>
		protected void NavigatePath()
		{
			// check if the path is valid making sure nothing is blocking the path
			point = mainPath.SetNextWaypoint(transform).position;
			navAgent.CalculatePath(point, path);
			Debug.Log(path.status);

			// if the path is valid follow it
			agentStopped = false;
			navAgent.speed = .5f;
			navAgent.stoppingDistance = 0;
			if(path.status == NavMeshPathStatus.PathComplete)
			{
				navAgent.SetDestination(point);
				SearchForObjectsOfInterest();
			}
			
			// else if the path is invalid go into search mode
			else if(path.status == NavMeshPathStatus.PathInvalid || path.status == NavMeshPathStatus.PathPartial)
			{
				machine.ChangeState(AgentStates.Search);
			}

			// if there is a blockage check for alternative routes or a door
			// if an alternative route is found go towards that route and follow it
			// else if the path is blocked by a door search around for a key
		}

		private readonly float searchRadius = 10;
		private bool agentStopped = false;

		/// <summary> Makes the agent wander around aimlessly until
		/// they find something of value of the path is open </summary>
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
					navAgent.speed = .5f;
				}
			} 
			else machine.ChangeState(AgentStates.FollowPath);
			SearchForObjectsOfInterest();
		}
		
		/// <summary> Searchs for any objects
		/// that can be collected or used </summary>
		protected void SearchForObjectsOfInterest()
		{
			// grabs any collider with the agents radius
			var colliders = Physics.OverlapSphere(transform.position, searchRadius);
			navAgent.CalculatePath(point, path);  // checks to make sure that the path isnt complete
			Debug.Log(path.status);
			
			// if the path is complete and the agent can move ignore everything and run towards the path
			if(path.status == NavMeshPathStatus.PathComplete) machine.ChangeState(AgentStates.FollowPath);
			
			// handles find the closest object thanks to a tutorial
			// then moves the agent towards it
			Transform tMin = null;
			float minDist = Mathf.Infinity;
			Vector3 currentPos = transform.position;
			
			// foreach collider in colliders
			foreach(var other in colliders)
			{
				// check if other object is of tag objects
				if(other.CompareTag("Objects"))
				{
					// checks the distance of the object to the min dist to see which is closer
					float dist = Vector3.Distance(other.transform.position, currentPos);
					if (dist < minDist)
					{
						// ignores any activated switches
						if(other.TryGetComponent(out Switch @switch))
							if(@switch.Activated) return;
						
						if(other.TryGetComponent(out LockSwitch @lock))
							if(@lock.used) return;
						
						// assigns the closest target
						tMin = other.transform;
						minDist = dist;
					}
					Debug.Log("found");
				}
			}
			targetObject = tMin; // moves the the closest target
			if(targetObject != null) machine.ChangeState(AgentStates.MoveTo);
		}
		
		/// <summary> handles moving the agent
		/// towards the object or target </summary>
		protected void MoveTowardsObject()
		{
			if(targetObject != null)
			{
				SetMoveToTarget(targetObject);
				CheckDistThenInteract(targetObject);
			}
			else machine.ChangeState(AgentStates.Search);
		}

		/// <summary> allows to hot swap out targets
		/// for the move towards method </summary>
		/// <param name="_target">what to move towards</param>
		private void SetMoveToTarget(Transform _target)
		{
			Debug.Log(_target.gameObject);
			navAgent.SetDestination(_target.position);
			if(!agentStopped)navAgent.speed = 1;
		}
		
		/// <summary> Handles interaction with
		/// certain objects when called </summary>
		protected void InteractWithObject()
		{
			// checks if the target isnt null first
			if(targetObject != null)
			{
				// depending on the target object do
				// a certain action for that object
				
				if(targetObject.name.Contains("Switch"))
				{
					var @switch = targetObject.GetComponent<Switch>();
					if(!@switch.Activated) @switch.ToggleSwitch();
				}
				
				else if(targetObject.name.Contains("Pickup"))
				{
					var pickup = targetObject.GetComponent<Pickup>();
					pickup.CollectItem();
					targetObject = null;	
				}
				else if(targetObject.name.Contains("Lock"))
				{
					if(!targetObject.GetComponent<LockSwitch>().used)
					{
						var @lock = targetObject.GetComponent<LockSwitch>();
						var key = GameManager.instance.SetKeys;
						GameManager.instance.SetKeys += @lock.UnlockDoor(key);
						targetObject = null;
					}
				}
			}
			// if no target object then swap states
			// also resets the target object after interaction
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