using SmartAI.Artificial_Intelligence;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


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
	
	[SerializeField] private Transform content;
	[SerializeField] private GameObject resetMenu;
	[SerializeField] private List<StateUI> stateUI = new List<StateUI>();

	[Serializable]
	private struct StateUI
	{
		// agent and updating the corresponding UI
		// used for checking the state of the
		public string name;
		public GameObject uiObject;
		public StateUI(string _name, GameObject _uiObject)
		{
			name = _name;
			uiObject = _uiObject;
		}
	}
	
	private Agent agent;
	public static GameManager instance;
    void Start()
    {
	    if(instance == null) instance = this;
	    else Destroy(gameObject);

	    agent = FindObjectOfType<Agent>();
		
	    // set up all the ui for the agents states assigning
	    // the names based of the state type
	    string[] stateNames = System.Enum.GetNames(typeof(AgentStates));
	    foreach(var state in stateNames)
	    {
		    var path = Resources.Load<GameObject>("State");
		    var obj = Instantiate(path, content);
		    var text = obj.transform.Find("StateText").GetComponent<TextMeshProUGUI>();
		    
		    text.text = state;
		    stateUI.Add(new StateUI(_name: state, _uiObject: obj));
	    }
    }

    public void PlayGame() => Time.timeScale = 1.0f;
    public void ResetGame() => SceneManager.LoadScene("Testing Grounds");
    
    /// <summary> Handles checking what
    /// state the agent is currently in </summary>
    /// <param name="_state">the current agent state</param>
    public void ActiveState(AgentStates _state)
    {
	    // checks if the state ui name matches the current state
	    // then changes the text color to indicate it is in that state
	    foreach(var state in stateUI)
	    {
		    var stateText = state.uiObject.transform.Find("StateText");
		    stateText.GetComponent<TextMeshProUGUI>().color = state.name == _state.ToString()
			    ? Color.green
			    : Color.black;
	    }
    }
    public void IncreaseTime(int _amt) => Time.timeScale = _amt;

    private float time;
    private float maxTime = 5;
    // timer before the menu opens
    public void ActivateResetMenu()
    {
	    time += Time.deltaTime;
	    if(time >= maxTime)
	    {
		    resetMenu.SetActive(true);
		    time = 0;
	    }
    }
}
