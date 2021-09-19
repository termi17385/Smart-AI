using UnityEngine;

namespace SmartAI.Doors
{
	public class LockSwitch : MonoBehaviour
	{
		[SerializeField] private LockedDoor unlockDoor;
		[SerializeField] private GameObject key;

		public bool used = false;
		
		public int UnlockDoor(int _key)
		{
			if(_key >= 1)
			{
				unlockDoor.locked = false;
				key.SetActive(true);
				used = true;
				return -1;
			}
			return 0;
		}
	}
}