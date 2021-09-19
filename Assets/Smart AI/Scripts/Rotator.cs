using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	[SerializeField] private float rotationSpeed;
	[SerializeField] private float maxTime = 2;
	[SerializeField] private bool x, y, z;
	
	private float timer = 0;

	private void Update()
	{
		timer += Time.deltaTime;
		if(timer >= maxTime)
		{
			rotationSpeed *= -1;
			timer = 0;
		}
		
		if(x) // x axis
		{
			y = z = false;
			var rotation = transform.localEulerAngles;
			rotation.x += rotationSpeed * Time.deltaTime;			
			transform.localRotation = Quaternion.Euler(rotation);
		}
		if(y) // y axis
		{
			x = z = false;

			var rotation = transform.localEulerAngles;
			rotation.y += rotationSpeed * Time.deltaTime;			
			transform.localRotation = Quaternion.Euler(rotation);
		}
		if(z) // z axis
		{
			y = x = false;
			var rotation = transform.localEulerAngles;
			rotation.z += rotationSpeed * Time.deltaTime;			
			transform.localRotation = Quaternion.Euler(rotation);
		}
		
	}
}
