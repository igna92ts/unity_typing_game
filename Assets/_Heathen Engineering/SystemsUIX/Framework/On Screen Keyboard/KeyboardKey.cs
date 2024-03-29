﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace HeathenEngineering.UIX
{
    [AddComponentMenu("Heathen/UIX/Keyboard/Keyboard Key")]
    [RequireComponent(typeof(RectTransform), typeof(UnityEngine.UI.Button), typeof(EventHelper))]
    public class KeyboardKey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        /// <summary>
        /// The parent keyboard which the key belongs to
        /// </summary>
        [HideInInspector]
        public Keyboard keyboard;

        /// <summary>
        /// Defines how the key should be handled when press events occure
        /// </summary>
        [Tooltip("What type of key determins how the key responds when pressed")]
        public KeyboardKeyType keyType;
        /// <summary>
        /// Defines the visual representation and the return value on press of the key
        /// </summary>
        [SerializeField]
        public KeyboardKeyGlyph keyGlyph;

        public KeyboardKeyStrokeEvent pressed;
        public KeyboardKeyStrokeEvent isDown;
        public KeyboardKeyStrokeEvent isUp;
        public KeyboardKeyStrokeEvent isClicked;
        [HideInInspector]
        public RectTransform selfRectTransform;
        public Serialization.KeyboardKeyTemplate template;
        private UnityEngine.UI.Button button;
        private EventHelper trigger;
        private bool keyboardFound = false;
        private bool isHeld = false;
        private float holdTime = 0;

        #region Unity Functions
        void Awake()
        {
            selfRectTransform = transform as RectTransform;
            ValidateKeyboard();
        }
        
        // Use this for initialization
        void Start()
        {
            ValidateKeyboard();

            if(button != null)
            {
                trigger.onPointerDown += Trigger_onPointerDown;
                trigger.onPointerUp += Trigger_onPointerUp;
                trigger.onPointerExit += Trigger_onPointerExit;
                button.onClick.AddListener(new UnityEngine.Events.UnityAction(Press));
                trigger.onSelect += Selected;
            }
        }

        void Update()
        {
            if(keyboard != null && keyboard.useKeyHold.Value && isHeld)
            {
                if(holdTime + keyboard.repeateTime.Value < Time.unscaledTime)
                {
                    holdTime = Time.unscaledTime;
                    Press();
                }
            }
        }
        #endregion

        #region Heathen Functions
        private void Trigger_onPointerExit(object sender, PointerEventData argument)
        {
            isHeld = false;
        }

        private void Trigger_onPointerDown(object sender, PointerEventData argument)
        {
            isHeld = true;
            holdTime = Time.unscaledTime;
        }

        private void Trigger_onPointerUp(object sender, PointerEventData argument)
        {
            isHeld = false;
        }

        public void Selected(object sender, BaseEventData data)
        {
            keyboard.ActiveKey = this;
        }

        public void Press()
        {
            if(keyType == KeyboardKeyType.WhiteSpace || keyType == KeyboardKeyType.Backspace || keyType == KeyboardKeyType.Delete)
            {
                EditorParseKeyCode = true;
                keyGlyph.DefaultFromCode(keyGlyph.code);
            }

            if (!keyboard.useShiftToggle && (keyGlyph.code == KeyCode.LeftShift || keyGlyph.code == KeyCode.RightShift))
                return;

            if (!keyboard.useAltGrToggle && (keyGlyph.code == KeyCode.AltGr))
                return;
            
            if (pressed != null)
                pressed.Invoke(this);
        }

        public string PressAndParse(string source, int inputIndex = -1)
        {
            Press();
            return ToString(source, inputIndex);
        }

        public KeyCode PressAndCode()
        {
            Press();
            return ToKeyCode();
        }

        public void UpdateTemplate(ref Serialization.KeyboardKeyTemplate template)
        {
            if (template == null)
                template = keyGlyph;
            else
            {
                template.AltGr = keyGlyph.altGrString;
                template.Shifted = keyGlyph.shiftedString;
                template.Normal = keyGlyph.normalString;
                template.Code = keyGlyph.code;    
            }

            if (selfRectTransform == null)
                selfRectTransform = GetComponent<RectTransform>();

            template.KeyType = keyType;
            template.KeySize = selfRectTransform.sizeDelta;
            template.KeyOffset = selfRectTransform.anchoredPosition3D;
            template.KeyRotation = selfRectTransform.localEulerAngles;

            if (keyGlyph.normalDisplay != null)
                template.DisplayNormal = keyGlyph.normalDisplay.text;

            if (keyGlyph.shiftedDisplay != null)
                template.DisplayShifted = keyGlyph.shiftedDisplay.text;

            if (keyGlyph.altGrDisplay != null)
                template.DisplayAltGr = keyGlyph.altGrDisplay.text;

            if (keyGlyph.shiftedAltGrDisplay != null)
                template.DisplayShiftedAltGr = keyGlyph.shiftedAltGrDisplay.text;
        }

        public Serialization.KeyboardKeyTemplate ToTemplate()
        {
            Serialization.KeyboardKeyTemplate template = keyGlyph;
            
            if (selfRectTransform == null)
                selfRectTransform = GetComponent<RectTransform>();

            template.KeySize = selfRectTransform.sizeDelta;
            template.KeyOffset = selfRectTransform.anchoredPosition3D;
            template.KeyRotation = selfRectTransform.localEulerAngles;
            template.KeyType = keyType;

            if (keyGlyph.normalDisplay != null)
                template.DisplayNormal = keyGlyph.normalDisplay.text;

            if (keyGlyph.shiftedDisplay != null)
                template.DisplayShifted = keyGlyph.shiftedDisplay.text;

            if (keyGlyph.altGrDisplay != null)
                template.DisplayAltGr = keyGlyph.altGrDisplay.text;

            if (keyGlyph.shiftedAltGrDisplay != null)
                template.DisplayShiftedAltGr = keyGlyph.shiftedAltGrDisplay.text;

            return template;
        }

        public KeyCode ToKeyCode()
        {
            return keyGlyph.code;
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string source, int inputIndex = -1)
        {
            if (keyType == KeyboardKeyType.Backspace && !string.IsNullOrEmpty(source))
            {
                if (inputIndex < 0 || inputIndex >= source.Length - 1)
                    return source.Substring(0, source.Length - 1);
                else if (inputIndex > 0)
                    return source.Remove(inputIndex - 1, 1);
                else
                    return source;
            }

            else if (keyType == KeyboardKeyType.Delete && !string.IsNullOrEmpty(source) && inputIndex > 0 && inputIndex < source.Length - 1)
                return source.Remove(inputIndex, 1);

            else if (keyType == KeyboardKeyType.Modifier && (keyGlyph.code == KeyCode.LeftShift || keyGlyph.code == KeyCode.RightShift || keyGlyph.code == KeyCode.CapsLock))
            {
                switch(keyboard.state)
                {
                    case KeyboardState.Normal:
                        keyboard.state = KeyboardState.Shifted;
                        break;
                    case KeyboardState.Shifted:
                        keyboard.state = KeyboardState.Normal;
                        break;
                    case KeyboardState.AltGr:
                        keyboard.state = KeyboardState.ShiftedAltGr;
                        break;
                    case KeyboardState.ShiftedAltGr:
                        keyboard.state = KeyboardState.AltGr;
                        break;
                }
                return source;
            }
            else if (keyType == KeyboardKeyType.Modifier && keyGlyph.code == KeyCode.AltGr)
            {
                switch (keyboard.state)
                {
                    case KeyboardState.Normal:
                        keyboard.state = KeyboardState.AltGr;
                        break;
                    case KeyboardState.Shifted:
                        keyboard.state = KeyboardState.ShiftedAltGr;
                        break;
                    case KeyboardState.AltGr:
                        keyboard.state = KeyboardState.Normal;
                        break;
                    case KeyboardState.ShiftedAltGr:
                        keyboard.state = KeyboardState.Shifted;
                        break;
                }
                return source;
            }

            else if (keyType == KeyboardKeyType.Character || keyType == KeyboardKeyType.WhiteSpace)
            {
                string result = string.IsNullOrEmpty(source) ? "" : source;
                if (inputIndex < 0 || inputIndex >= result.Length - 1)
                {
                    switch (keyboard.state)
                    {
                        case KeyboardState.AltGr:
                            return result + keyGlyph.altGrString;
                        case KeyboardState.Normal:
                            return result + keyGlyph.normalString;
                        case KeyboardState.Shifted:
                            return result + keyGlyph.shiftedString;
                        default:
                            return result + keyGlyph.shiftedAltGrString;
                    }
                }
                else
                {
                    switch (keyboard.state)
                    {
                        case KeyboardState.AltGr:
                            return result.Insert(inputIndex, keyGlyph.altGrString);
                        case KeyboardState.Normal:
                            return result.Insert(inputIndex, keyGlyph.normalString);
                        case KeyboardState.Shifted:
                            return result.Insert(inputIndex, keyGlyph.shiftedString);
                        default:
                            return result.Insert(inputIndex, keyGlyph.shiftedAltGrString);
                    }
                }
            }

            else
                return source;
        }

        public bool ValidateKeyboard()
        {
            button = GetComponent<UnityEngine.UI.Button>();

            trigger = GetComponent<EventHelper>();

            keyboardFound = GetKeyboard();

            if(keyGlyph.code == KeyCode.Tab || keyGlyph.code == KeyCode.Space)
            {
                EditorParseKeyCode = true;
                keyGlyph.DefaultFromCode(keyGlyph.code);
            }

            if (keyboardFound)
                keyboard.RegisterKey(this);
            return keyboardFound && button != null && trigger != null;
        }

        private bool GetKeyboard()
        {
            if (keyboard == null)
                keyboard = GetComponentInParent<Keyboard>();

            return keyboard != null;
        }

        //TODO: temp multi touch support event handlers
        #region Temp multi-touch support
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isDown != null)
                isDown.Invoke( this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isUp != null)
                isUp.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isClicked != null)
                isClicked.Invoke(this);

            //Only call pressed if there is no button and we are not a modifier type key
            if (button == null && keyType != KeyboardKeyType.Modifier)
            {
                if (pressed != null)
                    pressed.Invoke(this);
            }
        }
        #endregion

        #endregion

        public bool EditorParseKeyCode;
    }

    public enum KeyboardKeyType
    {
        Character,
        WhiteSpace,
        Modifier,
        Function,
        Delete,
        Backspace
    }
}
