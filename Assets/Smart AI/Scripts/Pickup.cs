using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	[SerializeField] private ItemType currentType = ItemType.Coin;
	[SerializeField, Tooltip("Only assign if the type is a key")] private Transform doorTarget = null;
	
	public void CollectItem()
	{
		switch(currentType)
		{
			case ItemType.Coin: GameManager.instance.SetCoins += 1; break;
			case ItemType.Key: GameManager.instance.SetKeys += 1; break;
		}
		Destroy(gameObject);
	}
}
