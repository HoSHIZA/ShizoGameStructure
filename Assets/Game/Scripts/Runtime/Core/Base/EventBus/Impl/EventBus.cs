using System;
using System.Collections.Generic;

namespace Game.Core.Base.EventBus
{
    /// <summary>
    /// Basic IEventBus implementation that manages events and their subscribers.
    /// </summary>
    public sealed class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();
        
        /// <inheritdoc/>
        public void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            var eventType = typeof(T);
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Delegate>();
            }

            _subscribers[eventType].Add(handler);
        }
        
        /// <inheritdoc/>
        public void Subscribe<T>(Action handler) where T : IEvent
        {
            var eventType = typeof(T);
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Delegate>();
            }

            _subscribers[eventType].Add(handler);
        }
        
        /// <inheritdoc/>
        public void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            var eventType = typeof(T);
            if (!_subscribers.ContainsKey(eventType)) return;

            _subscribers[eventType].Remove(handler);
        }
        
        /// <inheritdoc/>
        public void Unsubscribe<T>(Action handler) where T : IEvent
        {
            var eventType = typeof(T);
            if (!_subscribers.ContainsKey(eventType)) return;

            _subscribers[eventType].Remove(handler);
        }
        
        /// <inheritdoc/>
        public void Fire<T>() where T : IEvent
        {
            if (!_subscribers.TryGetValue(typeof(T), out var handlers)) return;

            foreach (var handler in handlers.ToArray())
            {
                if (handler is Action action)
                {
                    action.Invoke();
                }
                else
                {
                    throw new ArgumentException($"This event should have no arguments");
                }
            }
        }
        
        /// <inheritdoc/>
        public void Fire<T>(T eventToFire) where T : IEvent
        {
            if (!_subscribers.TryGetValue(typeof(T), out var handlers)) return;

            foreach (var handler in handlers.ToArray())
            {
                if (handler is Action<T> action)
                {
                    action.Invoke(eventToFire);
                }
                else
                {
                    throw new ArgumentException($"This event must take {typeof(T)}");
                }
            }
        }
    }
}