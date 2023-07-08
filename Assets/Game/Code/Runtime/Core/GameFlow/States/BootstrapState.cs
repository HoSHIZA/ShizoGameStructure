using Game.Core.Base.StateMachine;

namespace Game.Core.GameFlow.States
{
    public sealed class BootstrapState : State<GameStateMachine>
    {
        public BootstrapState()
        {
        }
        
        internal override void Enter()
        {
            StateMachine.ChangeState<MainMenuState>();
        }
        
        internal override void Exit()
        {
            
        }
    }
}