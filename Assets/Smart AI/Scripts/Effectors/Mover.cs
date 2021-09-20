using UnityEngine;

public class Mover : MonoBehaviour
{
	[SerializeField] private Transform obstacle, position, position2;
	private bool toggle;
	private float time;
	private float maxTime = 0.5f;

	// Script just handles moving the obstacles between points
	private void Update()
	{
		Transform newpos = null;
		newpos = toggle ? position : position2;
		var dist = Vector3.Distance(obstacle.position, newpos.position);
		
		if(dist <= 0.5f)
		{
			time += Time.deltaTime;
			if(time >= maxTime)
			{
				toggle = !toggle;
				time = 0;
			}
		}
		obstacle.position = Vector3.Lerp(obstacle.position, newpos.position, Time.deltaTime);
	}
}
