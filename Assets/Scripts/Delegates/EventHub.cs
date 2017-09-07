using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EraseGame.Delegates;
using System;

namespace EraseGame
{
    public class EventHub : MonoBehaviour
    {
        ///<summary>
        /// Called when Aiming is complete
        ///</summary>
        public event EmptyEvent OnAimComplete;

        /// <summary>
        /// Called when the post-aim timer has completed, just before firing.
        /// </summary>
        public event EmptyEvent OnBreakTimerComplete;

        /// <summary>
        /// Called if the fire event failed (due to hitting a block or other error)
        /// </summary>
        public event EmptyEvent OnFireFailed;

        /// <summary>
        /// Called if the fire event was successful.
        /// </summary>
        public event EmptyEvent OnFireSuccess;

        ///<summary>
        /// Called when a block breaks
        ///</summary>
        public event BlockEvent OnBlockBreak;

        ///<summary>
        /// Called when a block is damaged
        ///</summary>
        public event BlockEvent OnBlockDamaged;

        ///<summary>
        /// Called when a new horizontal bar is spawned
        ///</summary>
        public event BarEvent OnBarSpawned;

        ///<summary>
        /// Called when the scene has finished scrolling into its new position
        ///</summary>
        public event BarEvent OnScrollComplete;

        /// <summary>
        /// The event hub that exists.
        /// </summary>
        private static EventHub _eventHub;

        /// <summary>
        /// Returns the static eventhub object.
        /// </summary>
        /// <returns>EventHub</returns>
        public static EventHub GetEventHub()
        {
            return _eventHub;
        }

        ///<summary>
        // Invokes the OnAimComplete event
        ///</summary>
        public void Invoke()
        {
            OnAimComplete?.Invoke();
        }

        ///<summary>
        // Invokes the OnScrollComplete event
        ///</summary>
        public void InvokeOnScrollComplete<T>(T sender, HorizontalBar bar)
        {
            Debug.Log($"OnScrollComplete Invoked by {sender.ToString()}");
            OnScrollComplete?.Invoke(bar);
        }

        /// <summary>
        /// Invokes the OnAimComplete event
        /// </summary>
        public void InvokeOnAimComplete<T>(T sender)
        {
            Debug.Log($"OnAimComplete Invoked by {sender.ToString()}");
            OnAimComplete?.Invoke();
        }

        /// <summary>
        /// Invokes the OnBreakTimerComplete event
        /// </summary>
        public void InvokeOnBreakTimerComplete<T>(T sender)
        {
            Debug.Log($"OnBreakTimerComplete Invoked by {sender.ToString()}");
            OnBreakTimerComplete?.Invoke();
        }

        ///<summary>
        // Invokes the OnBlockBreak event
        ///</summary>
        public void InvokeOnBlockBreak<T>(T sender, BreakableBlock block)
        {
            Debug.Log($"OnBlockBreak Invoked by {sender.ToString()}");
            OnBlockBreak?.Invoke(block);
        }

        ///<summary>
        // Invokes the OnBarSpawned event
        ///</summary>
        public void InvokeOnBarSpawned<T>(T sender, HorizontalBar bar)
        {
            Debug.Log($"OnBarSpawned Invoked by {sender.ToString()}");
            OnBarSpawned?.Invoke(bar);
        }

        ///<summary>
        // Invokes the OnBlockDamaged event
        ///</summary>
        public void InvokeOnBlockDamaged<T>(T sender, BreakableBlock block)
        {
            Debug.Log($"OnBlockDamaged Invoked by {sender.ToString()}");
            OnBlockDamaged?.Invoke(block);
        }

        /// <summary>
        /// Invokes the OnFireFailed event
        /// </summary>
        public void InvokeOnFireFailed<T>(T sender)
        {
            Debug.Log($"OnFireFailed Invoked by {sender.ToString()}");
            OnFireFailed?.Invoke();
        }

        /// <summary>
        /// Invokes the OnFireSuccess event
        /// </summary>
        public void InvokeOnFireSuccess<T>(T sender)
        {
            Debug.Log($"OnFireSuccess Invoked by {sender.ToString()}");
            OnFireSuccess?.Invoke();
        }

        private void Awake()
        {
            if (_eventHub == null)
            {
                DontDestroyOnLoad(this);
                _eventHub = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (_eventHub == this)
                _eventHub = null;
        }
    }
}