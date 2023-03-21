using System;

namespace Game.Core.Base.EventBus
{
    /// <summary>
    /// Represents an interface for an event bus.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Subscribes a delegate to an event type of T.
        /// </summary>
        /// <typeparam name="T">The event type to subscribe to.</typeparam>
        /// <param name="handler">The delegate to be executed when the event is fired.</param>
        void Subscribe<T>(Action<T> handler) where T : IEvent;
        
        /// <summary>
        /// Subscribes a delegate to an event type of T without the event argument.
        /// </summary>
        /// <typeparam name="T">The event type to subscribe to.</typeparam>
        /// <param name="handler">The delegate to be executed when the event is fired.</param>
        void Subscribe<T>(Action handler) where T : IEvent;
        
        /// <summary>
        /// Unsubscribes a delegate from an event type of T.
        /// </summary>
        /// <typeparam name="T">The event type to unsubscribe from.</typeparam>
        /// <param name="handler">The delegate to be removed from the subscribers list.</param>
        void Unsubscribe<T>(Action<T> handler) where T : IEvent;
        
        /// <summary>
        /// Unsubscribes a delegate without the event argument from an event type of T.
        /// </summary>
        /// <typeparam name="T">The event type to unsubscribe from.</typeparam>
        /// <param name="handler">The delegate to be removed from the subscribers list.</param>
        void Unsubscribe<T>(Action handler) where T : IEvent;
        
        /// <summary>
        /// Fires an event of type T to all subscribers.
        /// </summary>
        /// <typeparam name="T">The event type to be fired.</typeparam>
        void Fire<T>() where T : IEvent;
        
        /// <summary>
        /// Fires the given event to all subscribers of its type.
        /// </summary>
        /// <param name="eventToFire">The event to be fired.</param>
        void Fire(IEvent eventToFire);
    }
}