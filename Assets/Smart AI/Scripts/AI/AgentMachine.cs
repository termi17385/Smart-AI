using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SmartAI.Artificial_Intelligence
{
    public enum AgentStates
    {
        FollowPath, 
        Search,
        MoveTo,
        Interact,
    }

    public delegate void StateDelegate();
    public class AgentMachine : Agent
    {
        private Dictionary<AgentStates, StateDelegate> states = new Dictionary<AgentStates, StateDelegate>();
        [SerializeField] private AgentStates currentState = AgentStates.FollowPath;
        public void ChangeState(AgentStates _newState) => currentState = _newState;

        protected override void Start()
        {
            states.Add(AgentStates.FollowPath, NavigatePath);
            states.Add(AgentStates.MoveTo, MoveTowardsObject);
            states.Add(AgentStates.Search, WanderAround);
            states.Add(AgentStates.Interact, InteractWithObject);
            base.Start();
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
        }
    }
}