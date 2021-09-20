using System.Collections.Generic;
using UnityEngine;

namespace SmartAI.Artificial_Intelligence
{
    public enum AgentStates
    {
        FollowPath, 
        Search,
        MoveTo,
        Interact,
        Win
    }

    public delegate void StateDelegate();
    public class AgentMachine : Agent
    {
        private Dictionary<AgentStates, StateDelegate> states = new Dictionary<AgentStates, StateDelegate>();
        [SerializeField] private AgentStates currentState = AgentStates.FollowPath;
        private static readonly int win = Animator.StringToHash("Win");
        public void ChangeState(AgentStates _newState) => currentState = _newState;

        protected override void Start()
        {
            states.Add(AgentStates.Interact, InteractWithObject);
            states.Add(AgentStates.MoveTo, MoveTowardsObject);
            states.Add(AgentStates.FollowPath, NavigatePath);
            states.Add(AgentStates.Search, WanderAround);
            states.Add(AgentStates.Win, Winner);
            base.Start();
            Time.timeScale = 0.0f;
        }
        protected void Update()
        {
            // These two lines are used to run the state machine
            // it works by attempting to retrieve the relevant function for the current state.
            // then running the function if it successfully found it 
            if(states.TryGetValue(currentState, out StateDelegate state)) state.Invoke();
            else Debug.Log($"No State Was Set For {currentState}.");
            
            Debug.DrawLine(transform.position, navAgent.destination, Color.blue);
            tpCharacter.Move(navAgent.desiredVelocity, false, false);
            GameManager.instance.ActiveState(currentState);

            if(point.name.Contains("Goal")) ChangeState(AgentStates.Win);
        }
        private void Winner()
        {
            GetComponent<Animator>().SetBool(win, true);
            GameManager.instance.ActivateResetMenu();
        }
    }
}