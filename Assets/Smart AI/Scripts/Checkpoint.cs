using SmartAI.Doors;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	[SerializeField] private Switch doorSwitch;

	private void OnTriggerEnter(Collider _other)
	{
		if(_other.CompareTag("Player")) doorSwitch.ToggleSwitch();
	}
}
