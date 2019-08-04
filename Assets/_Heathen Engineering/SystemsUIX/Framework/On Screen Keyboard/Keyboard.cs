using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using HeathenEngineering.Events;
using HeathenEngineering.Scriptable;

namespace HeathenEngineering.UIX
{
    [AddComponentMenu("Heathen/UIX/Keyboard/Keyboard")]
    public class Keyboard : MonoBehaviour
    {
        /// <summary>
        /// state is updated by the board and used by keys to properly display and report values
        /// do not modify this value manually
        /// </summary>
        [HideInInspector]
        public KeyboardState state = KeyboardState.Normal;
        [Tooltip("If off shift must be pressed and held to modify output. If on shift will behave like a toggle switch")]
        public BoolReference useShiftToggle = new BoolReference(true);
        [Tooltip("If off Alt Gr must be pressed and held to modify output. If on Alt Gr will behave like a toggle switch")]
        public BoolReference useAltGrToggle = new BoolReference(true);
        [Tooltip("If on keys will repeate while held")]
        public BoolReference useKeyHold = new BoolReference(true);
        public FloatReference repeateTime = new FloatReference(0.125f);

        /// <summary>
        /// If true then the board is currently behaving as if Caps Lock is toggled on
        /// </summary>
        public bool IsCapsLocked { get; set; }
        /// <summary>
        /// If true then the board is currently behaving as if the shift key is toggled on
        /// </summary>
        public bool IsShifted { get; set; }
        /// <summary>
        /// If true then the board is currently behaving as if the alt gr key is toggled on
        /// </summary>
        public bool IsAltGr { get; set; }
        private List<KeyboardKey> AltGrKeysHeld = new List<KeyboardKey>();
        private List<KeyboardKey> ShiftKeysHeld = new List<KeyboardKey>();
        [HideInInspector]
        public List<KeyboardKey> keys = new List<KeyboardKey>();
        [Header("Events")]
        [Tooltip("Game Event output for key press events")]
        public KeyboardKeyPressGameEvent KeyboardGameEvent;
        [Tooltip("Unity Event output for key press events")]
        public KeyboardKeyStrokeEvent KeyboardKeyPressed;
        [HideInInspector]
        public KeyboardKey ActiveKey;
        private KeyboardState statePrevious = KeyboardState.ShiftedAltGr;

        // Use this for initialization
        void Start()
        {
            UpdateKeyLinks();
        }

        void Update()
        {
            if (AltGrKeysHeld.Count > 0 || (useAltGrToggle && IsAltGr))
            {
                if ((ShiftKeysHeld.Count > 0 || (useShiftToggle && IsShifted)) || IsCapsLocked)
                    state = KeyboardState.ShiftedAltGr;
                else
                    state = KeyboardState.AltGr;
            }
            else if ((ShiftKeysHeld.Count > 0 || (useShiftToggle && IsShifted)) || IsCapsLocked)
                state = KeyboardState.Shifted;
            else
                state = KeyboardState.Normal;
            
            if (statePrevious != state)
                UpdateState();
        }

        /// <summary>
        /// Registeres a KeybaordKey to the keyboard allowing events to be tracked for the key
        /// </summary>
        /// <param name="key"></param>
        public void RegisterKey(KeyboardKey key)
        {
            if (!keys.Contains(key))
            {
                keys.Add(key);
                key.keyboard = this;
                key.pressed.AddListener(keyPressed);
                key.isDown.AddListener(keyIsDown);
                key.isUp.AddListener(keyIsUp);
            }
        }

        /// <summary>
        /// Updates the keyboards key links re-registering all keys
        /// </summary>
        public void UpdateKeyLinks()
        {
            foreach (KeyboardKey key in keys)
            {
                key.pressed.RemoveListener(keyPressed);
            }

            keys.Clear();

            foreach (KeyboardKey key in GetComponentsInChildren<KeyboardKey>())
            {
                RegisterKey(key);
            }
        }

        /// <summary>
        /// Updates the keybaords modifier key status
        /// </summary>
        private void UpdateState()
        {
            statePrevious = state;
            switch (state)
            {
                case KeyboardState.Normal:
                    foreach (KeyboardKey key in keys)
                    {
                        if (key.keyGlyph.normal != null)
                            key.keyGlyph.normal.gameObject.SetActive(true);
                        if (key.keyGlyph.shifted != null)
                            key.keyGlyph.shifted.gameObject.SetActive(false);
                        if (key.keyGlyph.altGr != null)
                            key.keyGlyph.altGr.gameObject.SetActive(false);
                        if (key.keyGlyph.shiftedAltGr != null)
                            key.keyGlyph.shiftedAltGr.gameObject.SetActive(false);
                    }
                    break;
                case KeyboardState.Shifted:
                    foreach (KeyboardKey key in keys)
                    {
                        if (key.keyGlyph.normal != null)
                            key.keyGlyph.normal.gameObject.SetActive(false);
                        if (key.keyGlyph.shifted != null)
                            key.keyGlyph.shifted.gameObject.SetActive(true);
                        if (key.keyGlyph.altGr != null)
                            key.keyGlyph.altGr.gameObject.SetActive(false);
                        if (key.keyGlyph.shiftedAltGr != null)
                            key.keyGlyph.shiftedAltGr.gameObject.SetActive(false);
                    }
                    break;
                case KeyboardState.AltGr:
                    foreach (KeyboardKey key in keys)
                    {
                        if (key.keyGlyph.normal != null)
                            key.keyGlyph.normal.gameObject.SetActive(false);
                        if (key.keyGlyph.shifted != null)
                            key.keyGlyph.shifted.gameObject.SetActive(false);
                        if (key.keyGlyph.altGr != null)
                            key.keyGlyph.altGr.gameObject.SetActive(true);
                        if (key.keyGlyph.shiftedAltGr != null)
                            key.keyGlyph.shiftedAltGr.gameObject.SetActive(false);
                    }
                    break;
                case KeyboardState.ShiftedAltGr:
                    foreach (KeyboardKey key in keys)
                    {
                        if (key.keyGlyph.normal != null)
                            key.keyGlyph.normal.gameObject.SetActive(false);
                        if (key.keyGlyph.shifted != null)
                            key.keyGlyph.shifted.gameObject.SetActive(false);
                        if (key.keyGlyph.altGr != null)
                            key.keyGlyph.altGr.gameObject.SetActive(false);
                        if (key.keyGlyph.shiftedAltGr != null)
                            key.keyGlyph.shiftedAltGr.gameObject.SetActive(true);
                    }
                    break;
            }
        }

        /// <summary>
        /// Press the current active key
        /// </summary>
        public void PressKey()
        {
            if (ActiveKey != null)
                ActiveKey.Press();
        }
        /// <summary>
        /// Activate the selected key and press it
        /// </summary>
        /// <param name="key"></param>
        public void PressKey(KeyboardKey key)
        {
            if (key != null)
            {
                EventSystem.current.SetSelectedGameObject(key.gameObject);
                ActiveKey = key;
                key.Press();
            }
        }
        /// <summary>
        /// Find the key with the matching code and press it
        /// </summary>
        /// <param name="code"></param>
        public void PressKey(KeyCode code)
        {
            foreach (KeyboardKey key in keys)
            {
                if (key.keyGlyph.code == code)
                {
                    PressKey(key);
                    return;
                }
            }
        }
        
        /// <summary>
        /// Occures when a key is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="key"></param>
        public void keyPressed(KeyboardKey key)
        {
            if (key.keyGlyph.code == KeyCode.CapsLock)
                IsCapsLocked = !IsCapsLocked;

            if (useShiftToggle &&
                (key.keyGlyph.code == KeyCode.LeftShift || key.keyGlyph.code == KeyCode.RightShift))
                IsShifted = !IsShifted;

            if (useAltGrToggle &&
                (key.keyGlyph.code == KeyCode.AltGr))
                IsAltGr = !IsAltGr;

            if (KeyboardGameEvent != null)
                KeyboardGameEvent.Raise(key);

            if (KeyboardKeyPressed != null)
                KeyboardKeyPressed.Invoke(key);
        }

        #region Multi-touch code
        /// <summary>
        /// Occures when a key is down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="key"></param>
        private void keyIsDown(KeyboardKey key)
        {
            //For modifiers consider a down movement a press if the board is not in this state
            if (key.keyType == KeyboardKeyType.Modifier)
            {
                //If we arent in AltGr mode for the AltGr key then press AltGr
                if (key.keyGlyph.code == KeyCode.AltGr && !AltGrKeysHeld.Contains(key))
                {
                    AltGrKeysHeld.Add(key);
                }
                else if ((key.keyGlyph.code == KeyCode.LeftShift || key.keyGlyph.code == KeyCode.RightShift) && !ShiftKeysHeld.Contains(key))
                {
                    ShiftKeysHeld.Add(key);
                }
            }
        }

        /// <summary>
        /// Occures when a key was down and is released 'up'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="key"></param>
        private void keyIsUp(KeyboardKey key)
        {
            //For modifiers consider a down movement a press if the board is not in this state
            if (key.keyType == KeyboardKeyType.Modifier)
            {
                //If we arent in AltGr mode for the AltGr key then press AltGr
                if (key.keyGlyph.code == KeyCode.AltGr && AltGrKeysHeld.Contains(key))
                {
                    AltGrKeysHeld.Remove(key);
                }
                else if ((key.keyGlyph.code == KeyCode.LeftShift || key.keyGlyph.code == KeyCode.RightShift) && ShiftKeysHeld.Contains(key))
                {
                    ShiftKeysHeld.Remove(key);
                }
            }
        }
        #endregion
    }
}
