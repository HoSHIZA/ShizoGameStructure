namespace Game.Core.Base.StateMachine
{
    /// <summary>
    /// Base class for states used in a state machine.
    /// </summary>
    /// <typeparam name="T">The type of state machine this state is for.</typeparam>
    public abstract class State<T> where T : StateMachine<T>
    {
        /// <summary>
        /// The state machine this state is a part of.
        /// </summary>
        protected StateMachine<T> StateMachine { get; private set; }
        
        /// <summary>
        /// Sets the state machine for this state.
        /// </summary>
        /// <param name="stateMachine">The state machine to set.</param>
        protected internal void SetStateMachine(StateMachine<T> stateMachine)
        {
            if (StateMachine != null) return;
            
            StateMachine = stateMachine;
        }
        
        /// <summary>
        /// Called when this state is entered.
        /// </summary>
        internal abstract void Enter();
        
        /// <summary>
        /// Called when this state is exited.
        /// </summary>
        internal abstract void Exit();
        
        /// <summary>
        /// Called every frame to update this state.
        /// </summary>
        /// <remarks>Used at discretion.</remarks>
        public virtual void Update() { }
        
        /// <summary>
        /// Called every physics update to update this state.
        /// </summary>
        /// <remarks>Used at discretion.</remarks>
        public virtual void FixedUpdate() { }
        
        /// <summary>
        /// Called every frame after Update to update this state.
        /// </summary>
        /// <remarks>Used at discretion.</remarks>
        public virtual void LateUpdate() { }
    }
}