using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Base.StateMachine
{
    /// <summary>
    /// An abstract class for creating a state machine.
    /// </summary>
    /// <typeparam name="T">The type of the state machine.</typeparam>
    /// <remarks>
    /// The class provides a mechanism for switching between different states, setting the initial state, updating the states and checking if a state exists in the state machine.
    /// </remarks>
    public abstract class StateMachine<T> where T : StateMachine<T>
    {
        /// <summary>
        /// Called when the state changes.
        /// </summary>
        /// <returns>State Type.</returns>
        public event Action<Type> OnStateChanged;
        
        private Dictionary<Type, State<T>> _states;
        private State<T> _initialState;
        private State<T> _currentState;
        private bool _isSetup;
        
        /// <summary>
        /// Sets up the state machine with an array of states, and initializes the current state.
        /// </summary>
        /// <param name="states">An array of states to set up the state machine.</param>
        /// <param name="initUseFirstState">If true, initializes the current state with the first state in the array.</param>
        protected void Setup(State<T>[] states, bool initUseFirstState = false)
        {
            if (_isSetup) return;
            
            _states = new Dictionary<Type, State<T>>(states.Length);
            
            foreach (var state in states)
            {
                state.SetStateMachine(this);
                
                TryAddState(state);
            }
            
            if (initUseFirstState && _states.Count > 0)
            {
                SetInitialState(_states.First().Value.GetType());

                _isSetup = true;
            }
        }
        
        /// <summary>
        /// Gets the type of the current state.
        /// </summary>
        /// <returns>The type of the current state.</returns>
        public Type GetCurrentStateType()
        {
            return _currentState.GetType();
        }
        
        /// <summary>
        /// Changes the current state of the state machine to the specified type of state.
        /// </summary>
        /// <typeparam name="TState">The type of the state to change to.</typeparam>
        /// <returns>Returns true if the state transition was successful, false otherwise.</returns>
        public bool ChangeState<TState>() where TState : State<T>
        {
            return ChangeState(typeof(TState));
        }
        
        /// <summary>
        /// Changes the current state of the state machine to the specified type of state.
        /// </summary>
        /// <param name="type">The type of the state to change to.</param>
        /// <returns>Returns true if the state transition was successful, false otherwise.</returns>
        public bool ChangeState(Type type)
        {
            var state = GetState(type);
            
            if (state == null) return false;
            if (state == _currentState) return false;
            
            _currentState?.Exit();
            _currentState = state;
            
            OnStateChanged?.Invoke(_currentState.GetType());
            
            _currentState.Enter();
            
            return true;
        }
        
        /// <summary>
        /// Returns a value indicating whether the state machine has a state of the specified type.
        /// </summary>
        /// <typeparam name="TState">The type of the state to check.</typeparam>
        /// <returns>Returns true if the state machine has the specified state, false otherwise.</returns>
        public bool HasState<TState>() where TState : State<T>
        {
            return HasState(typeof(TState));
        }
        
        /// <summary>
        /// Returns a value indicating whether the state machine has a state of the specified type.
        /// </summary>
        /// <param name="type">The type of the state to check.</param>
        /// <returns>Returns true if the state machine has the specified state, false otherwise.</returns>
        public bool HasState(Type type)
        {
            return _states.ContainsKey(type);
        }
        
        public bool TypeBelongsStateMachine(Type type)
        {
            return type.GetGenericArguments().Contains(typeof(StateMachine<T>));
        }
        
        /// <summary>
        /// Updates the state machine, executing the Update method of the current state.
        /// </summary>
        public void Update()
        {
            _currentState.Update();
        }

        /// <summary>
        /// Updates the state machine, executing the FixedUpdate method of the current state.
        /// </summary>
        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }
        
        /// <summary>
        /// Updates the state machine, executing the LateUpdate method of the current state.
        /// </summary>
        public void LateUpdate()
        {
            _currentState.LateUpdate();
        }
        
        /// <summary>
        /// Attempts to add a state to the state machine.
        /// </summary>
        /// <param name="state">The state to add to the state machine.</param>
        /// <returns>Returns true if the state was added successfully, false otherwise.</returns>
        protected bool TryAddState(State<T> state)
        {
            return _states.TryAdd(state.GetType(), state);
        }
        
        /// <summary>
        /// Sets the initial state of the state machine to the specified state type.
        /// </summary>
        /// <typeparam name="TState">The type of the state to set as the initial state.</typeparam>
        /// <remarks>
        /// This method can only be called during the setup phase of the state machine, before any state changes
        /// have occurred. If called after setup is complete, this method has no effect.
        /// </remarks>
        protected void SetInitialState<TState>() where TState : State<T>
        {
            SetInitialState(typeof(TState));
        }
        
        /// <summary>
        /// Sets the initial state of the state machine to the specified state type.
        /// </summary>
        /// <param name="type">The type of the state to set as the initial state.</param>
        /// <remarks>
        /// This method can only be called during the setup phase of the state machine, before any state changes
        /// have occurred. If called after setup is complete, this method has no effect.
        /// </remarks>
        protected void SetInitialState(Type type)
        {
            if (_isSetup) return;
            
            _initialState = GetState(type);
            
            ChangeState(_initialState.GetType());
            
            _isSetup = true;
        }
        
        private State<T> GetState(Type type)
        {
            return _states.ContainsKey(type) ? _states[type] : null;
        }
    }
}