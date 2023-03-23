using System;
using Game.Core.Base.EventBus;
using Game.Core.Base.SceneManagement;
using Game.Core.Base.ServiceLocator;
using Game.Core.Base.StateMachine;
using Game.Core.GameFlow.States;
using UnityEngine;

namespace Game.Core.GameFlow
{
    /// <summary>
    /// Represents the state machine that controls the game flow in the GameStateMachine.
    /// </summary>
    public sealed class GameStateMachine : StateMachine<GameStateMachine>
    {
        /// <summary>
        /// Class constructor, in which all necessary dependencies are passed.
        /// To add a new state, modify the states variable, all states must be bound to the GameStateMachine.
        /// </summary>
        public GameStateMachine(IServiceLocator services, ISceneLoader sceneLoader, IEventBus eventBus)
        {
            var states = new State<GameStateMachine>[]
            {
                new BootstrapState(services, sceneLoader, eventBus),
                new MainMenuState(services, sceneLoader, eventBus),
            };
            
            OnStateChanged += LogWhenStateChanged;
            
            Setup(states, true);
        }
        
        private static void LogWhenStateChanged(Type type)
        {
            if (!GameRunner.RuntimeConfig.GameStateMachineLogging) return;

            Debug.Log($"<color=cyan>[GAME] [GAME STATE MACHINE]</color> Enter to <color=orange>{type.Name}</color>.");
        }
    }
}