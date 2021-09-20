using System.Collections.Generic;
using SmartAI.Doors;

using System;

using UnityEngine;

public class Disabler : MonoBehaviour
{
	[SerializeField] private List<Switch> switches = new List<Switch>();
	private int index = 0;
	private void Update()
	{
		var a = switches[0].Activated;
		var b = switches[1].Activated;

		if(a || b)
		{
			if(!a) switches[0].gameObject.SetActive(false);
			if(!b) switches[1].gameObject.SetActive(false);
		}
		else foreach(var @switch in switches) @switch.gameObject.SetActive(true);
	}

	private void OnDrawGizmos()
	{
		foreach(Switch @switch in switches)
		{
			if(@switch != null)
			{
				var posA = transform.position;
				var posB = @switch.transform.position;
                 
				Gizmos.color = @switch.Activated? Color.red : Color.green;
				Gizmos.DrawLine(posA, posB);
			}
		}        
	}
}
