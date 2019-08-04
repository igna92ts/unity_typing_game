using UnityEngine;

namespace HeathenEngineering.UIX
{
    [AddComponentMenu("Heathen/Events/Keyboard Key Press Game Event Listener")]
    public class KeyboardKeyPressGameEventListener : MonoBehaviour
    {
        public KeyboardKeyPressGameEvent Event;

        public KeyboardKeyStrokeEvent KeyResponce;

        private void OnEnable()
        {
            if (Event != null)
                Event.AddListener(this);
        }

        private void OnDisable()
        {
            if (Event != null)
                Event.RemoveListener(this);
        }

        public void OnEventRaised(KeyboardKey value)
        {
            KeyResponce.Invoke(value);
        }
    }
}
