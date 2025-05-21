using System;
using System.Collections.Generic;
using gameplay.enums;
using UnityEngine;
using UnityEngine.UI;

namespace gameplay.ai
{
    /// <summary>
    /// Responsible for making decisions on behavior an agent
    /// </summary>
    public class CollectorFSM
    {
        public Dictionary<Type, CollectorState> States;
        /// <summary>
        /// Trigger transition on state change
        /// </summary>
        public CollectorState CurrentState
        {
            get => currentState;
            set => Transition(value);
        }
        private CollectorState currentState;
        private bool inTransition = false;

        public CollectorFSM(Dictionary<Type, CollectorState> states)
        {
            States = states;
        }

        /// <summary>
        /// Changes a state to the specified type
        /// </summary>
        /// <typeparam name="T">state to change to</typeparam>
        public virtual void ChangeState<T>() where T : CollectorState
        {
            CurrentState = States[typeof(T)];;
        }

        public void FixedUpdate()
        {
            CurrentState?.Update();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            CurrentState?.OnTriggerEnter2D(other);
        }

        /// <summary>
        /// trigger a transtion to the next state.
        /// This will run exit and enter of the old and new state respectively 
        /// </summary>
        /// <param name="nextState"></param>
        public void Transition(CollectorState nextState)
        {
            if (currentState == nextState || inTransition) 
            {
                return;
            }
            Debug.Log($"Transitioning to {nextState}");
            inTransition = true;
            
            currentState?.Exit();
            currentState = nextState;
            currentState.Enter();
            
            inTransition = false;            
        }
    }
}