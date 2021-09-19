using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	[SerializeField] private float rotationSpeed;
	[SerializeField] private bool x, y, z;

	private void Update()
	{
		if(x)
		{
			y = z = false;
			transform.Rotate(transform.up * (rotationSpeed * Time.deltaTime));
		}
		if(y)
		{
			x = z = false;
			transform.Rotate(transform.forward * (rotationSpeed * Time.deltaTime));
		}
		if(z)
		{
			y = x = false;
			transform.Rotate(transform.right * (rotationSpeed * Time.deltaTime));
		}
		
	}
}
