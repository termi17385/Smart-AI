using UnityEngine.AI;
using UnityEngine;

namespace SmartAI.Artificial_Intelligence
{
	public static class AIUtils
	{
		/// <summary> Gets a random position randomly inside a unit sphere
		/// then returns that position to be used by the AI </summary>
		/// <param name="_origin">The Agents position</param>
		/// <param name="_distance">how far to check</param>
		/// <param name="_layermask">filters out certain areas</param>
		public static Vector3 RandomNavSphere (Vector3 _origin, float _distance, int _layermask) 
		{
			// gets a random direction from a unit sphere 
			Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _distance;
			randomDirection += _origin; 

			// makes sure that the direction is touching the navmesh
			NavMesh.SamplePosition (randomDirection, out NavMeshHit navHit, _distance, _layermask);
			return navHit.position;
		}
	}
}