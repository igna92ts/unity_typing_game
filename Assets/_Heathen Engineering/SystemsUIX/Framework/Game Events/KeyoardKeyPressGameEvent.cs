using HeathenEngineering.Scriptable;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.UIX
{
    [CreateAssetMenu(menuName = "Events/Keyboard Key Press Game Event")]
    public class KeyboardKeyPressGameEvent : GameEvent
    {
        public List<KeyboardKeyPressGameEventListener> boolListeners = new List<KeyboardKeyPressGameEventListener>();
        public List<UnityAction<KeyboardKey>> boolActions = new List<UnityAction<KeyboardKey>>();

        public void Raise(KeyboardKey value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                if (listeners[i] != null)
                    listeners[i].OnEventRaised();
            }

            for (int i = boolListeners.Count - 1; i >= 0; i--)
            {
                if (boolListeners[i] != null)
                    boolListeners[i].OnEventRaised(value);
            }

            for (int i = actions.Count - 1; i >= 0; i--)
            {
                if (actions[i] != null)
                    actions[i].Invoke();
            }

            for (int i = boolActions.Count - 1; i >= 0; i--)
            {
                if (boolActions[i] != null)
                    boolActions[i].Invoke(value);
            }
        }

        public void AddListener(KeyboardKeyPressGameEventListener listener)
        {
            boolListeners.Add(listener);
        }
        public void RemoveListener(KeyboardKeyPressGameEventListener listener)
        {
            boolListeners.Remove(listener);
        }
        public void AddListener(UnityAction<KeyboardKey> listener)
        {
            boolActions.Add(listener);
        }
        public void RemoveListener(UnityAction<KeyboardKey> listener)
        {
            boolActions.Remove(listener);
        }
    }
}
