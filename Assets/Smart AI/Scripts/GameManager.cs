using SmartAI.Artificial_Intelligence;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
	Coin,
	Key
}
public class GameManager : MonoBehaviour
{
	public int SetCoins
	{
		get => coins;
		set => coins = value;
	}

	public int SetKeys
	{
		get => keys;
		set => keys = value;
	}

	[SerializeField] private int coins;
	[SerializeField] private int keys;

	private Agent agent;
	public static GameManager instance;
    void Start()
    {
	    if(instance == null) instance = this;
	    else Destroy(gameObject);

	    agent = FindObjectOfType<Agent>();
    }
}
