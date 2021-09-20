using UnityEngine;

/// <summary> when interacted with the pickup checks its current
/// type then does the action specific to that type </summary>
public class Pickup : MonoBehaviour
{
	[SerializeField] private ItemType currentType = ItemType.Coin;
	
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
