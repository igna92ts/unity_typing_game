using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using HeathenEngineering.Tools;
using System.Reflection;
using HeathenEngineering.Serializable;
using UnityEngine.Events;

namespace HeathenEngineering.UIX
{
    [AddComponentMenu("Heathen/UIX/Keyboard/Keyboard Output Manager")]
    [RequireComponent(typeof(Keyboard))]
    public class KeyboardOutputManager : MonoBehaviour
    {
        public KeyboardOutputTargetType targetType;
        public bool autoLinkHID = true;
        private GameObject previousLinkedGameObject;
        public GameObject linkedGameObject;
        public List<Component> linkedBehaviours;
        public Component linkedBehaviour;
        public List<string> fields;
        public string field;
        public int insertPoint = -1;
        public int selectionLength = 0;
        public UnityEngine.UI.InputField lastInputField;
        private UnityEngine.UI.InputField _inputField;
        public TMPro.TMP_InputField LastTMProInputField;
        private TMPro.TMP_InputField _tmpInputField;
        public KeyboardKeyStrokeEvent keyStrokeEvent;

        private Keyboard keyboard;

        private void Start()
        {
            ValidateHost();
            keyboard.KeyboardKeyPressed.AddListener(keyPressed);
        }

        private bool ValidateHost()
        {
            if (keyboard == null)
            {
                keyboard = GetComponent<Keyboard>();
                if (keyboard == null)
                {
                    ConsoleLogger.LogWarning("[Keyboard Output Manager] Host keyboard not found, the output manager will be disabled.");
                    enabled = false;
                    return false;
                }
                else
                    return true;
            }
            else
                return true;
        }

        private void Update()
        {
            if (!ValidateHost())
                return;

            if (targetType == KeyboardOutputTargetType.EventSystem)
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    var inField = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.InputField>();
                    if (inField != null)
                        SetInputTarget(inField);
                    else
                    {
                        var tmInField = EventSystem.current.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>();
                        SetInputTarget(tmInField);
                    }
                }
            }

            if (autoLinkHID && EventSystem.current.currentSelectedGameObject != linkedGameObject)
            {
                foreach (KeyboardKey key in keyboard.keys)
                {
                    if (key != null)
                    {
                        if (Input.GetKeyDown(key.keyGlyph.code))
                        {
                            key.Press();
                        }
                    }
                }
            }
        }

        private void keyPressed(KeyboardKey key)
        {
            if (!enabled)
                return;

            if (!ValidateHost())
                return;

            switch(targetType)
            {
                case KeyboardOutputTargetType.Function:
                    keyStrokeEvent.Invoke(key);
                    break;
                default:
                    bool selectionReplaced = false;
                    if (linkedBehaviour != null && field != null)
                    {
                        string initalValue = GetLinkedFieldValue();

                        if (selectionLength > 0)
                        {
                            selectionReplaced = true;
                            initalValue = initalValue.Remove(insertPoint, selectionLength);
                        }

                        if ((key.keyType == KeyboardKeyType.Backspace || key.keyType == KeyboardKeyType.Delete) && selectionReplaced)
                        {
                            SetLinkedFieldValue(initalValue);
                            insertPoint++;
                        }
                        else
                        {
                            string resultingValue = key.ToString(initalValue, insertPoint);
                            SetLinkedFieldValue(resultingValue);
                            insertPoint += resultingValue.Length - initalValue.Length;
                        }

                        //insertPoint++;
                        selectionLength = 0;
                    }

                    if (linkedBehaviour.GetType() == typeof(UnityEngine.UI.InputField))
                    {
                        lastInputField.selectionAnchorPosition = insertPoint;
                    }
                    else if(linkedBehaviour.GetType() == typeof(TMPro.TMP_InputField))
                    {
                        LastTMProInputField.selectionAnchorPosition = insertPoint;
                    }
                    break;
            }
        }

        public void ManualSetInputTarget(UnityEngine.UI.InputField target)
        {
            if(target == null)
            {
                previousLinkedGameObject = null;
                linkedGameObject = null;
                linkedBehaviour = null;
                fields.Clear();
                field = string.Empty;
                lastInputField = null;
                LastTMProInputField = null;
                _inputField = null;
                _tmpInputField = null;
            }
            else
            {
                _inputField = target;
                _tmpInputField = null;
                lastInputField = target;
                LastTMProInputField = null;
                linkedGameObject = target.gameObject;
                linkedBehaviour = target;
                field = "text";
            }
        }

        public void ManualSetInputTarget(TMPro.TMP_InputField target)
        {
            if (target == null)
            {
                previousLinkedGameObject = null;
                linkedGameObject = null;
                linkedBehaviour = null;
                fields.Clear();
                field = string.Empty;
                lastInputField = null;
                LastTMProInputField = null;
                _inputField = null;
                _tmpInputField = null;
            }
            else
            {
                _inputField = null;
                _tmpInputField = target;
                lastInputField = null;
                LastTMProInputField = target;
                linkedGameObject = target.gameObject;
                linkedBehaviour = target;
                field = "text";
            }
        }

        public void ManualSetTextTarget(UnityEngine.UI.Text target)
        {
            if (target == null)
            {
                previousLinkedGameObject = null;
                linkedGameObject = null;
                linkedBehaviour = null;
                fields.Clear();
                field = string.Empty;
                lastInputField = null;
                _inputField = null;
            }
            else
            {
                _inputField = null;
                lastInputField = null;
                linkedGameObject = target.gameObject;
                linkedBehaviour = target;
                field = "text";
            }
        }

        /// <summary>
        /// Targets the indicated input field for the keyboard. This will also 'select' the InputField in Unity UI
        /// </summary>
        /// <param name="target"></param>
        public void SetInputTarget(UnityEngine.UI.InputField target)
        {
            if (!ValidateHost())
                return;

            _inputField = target;
            _tmpInputField = null;

            if (_inputField != null)
            {
                _inputField.Select();
                lastInputField = _inputField;
                if (_inputField.selectionAnchorPosition > _inputField.selectionFocusPosition)
                {
                    insertPoint = _inputField.selectionFocusPosition;
                    selectionLength = _inputField.selectionAnchorPosition - _inputField.selectionFocusPosition;
                }
                else if (_inputField.selectionFocusPosition > _inputField.selectionAnchorPosition)
                {
                    insertPoint = _inputField.selectionAnchorPosition;
                    selectionLength = _inputField.selectionFocusPosition - _inputField.selectionAnchorPosition;
                }
                else
                {
                    insertPoint = _inputField.selectionAnchorPosition;
                    selectionLength = 0;
                }
            }

            if (_inputField != null && EventSystem.current.currentSelectedGameObject != linkedGameObject)
            {
                lastInputField = _inputField;
                linkedGameObject = EventSystem.current.currentSelectedGameObject;
                linkedBehaviour = lastInputField;
                field = "text";
            }
        }

        /// <summary>
        /// Targets the indicated input field for the keyboard. This will also 'select' the InputField in Unity UI
        /// </summary>
        /// <param name="target"></param>
        public void SetInputTarget(TMPro.TMP_InputField target)
        {
            if (!ValidateHost())
                return;

            _tmpInputField = target;
            _inputField = null;

            if (_tmpInputField != null)
            {
                _tmpInputField.Select();
                LastTMProInputField = _tmpInputField;
                if (_tmpInputField.selectionAnchorPosition > _tmpInputField.selectionFocusPosition)
                {
                    insertPoint = _tmpInputField.selectionFocusPosition;
                    selectionLength = _tmpInputField.selectionAnchorPosition - _tmpInputField.selectionFocusPosition;
                }
                else if (_tmpInputField.selectionFocusPosition > _tmpInputField.selectionAnchorPosition)
                {
                    insertPoint = _tmpInputField.selectionAnchorPosition;
                    selectionLength = _tmpInputField.selectionFocusPosition - _tmpInputField.selectionAnchorPosition;
                }
                else
                {
                    insertPoint = _tmpInputField.selectionAnchorPosition;
                    selectionLength = 0;
                }
            }

            if (_tmpInputField != null && EventSystem.current.currentSelectedGameObject != linkedGameObject)
            {
                LastTMProInputField = _tmpInputField;
                linkedGameObject = EventSystem.current.currentSelectedGameObject;
                linkedBehaviour = LastTMProInputField;
                field = "text";
            }
        }

        /// <summary>
        /// Returns the list of Component behaviours that have a valid target string that can be targeted for input
        /// </summary>
        /// <returns></returns>
        public List<Component> GetLinkedBehaviour()
        {
            if (linkedGameObject != null)
            {
                List<Component> results = new List<Component>();
                foreach (Component com in linkedGameObject.GetComponents<Component>())
                {
                    bool hasString = false;
                    foreach (PropertyInfo info in com.GetType().GetProperties())
                    {
                        if (info.PropertyType == typeof(string))
                        {
                            hasString = true;
                            break;
                        }
                    }

                    if (hasString)
                        results.Add(com);
                }
                return results;
            }
            else
                return null;
        }

        /// <summary>
        /// Updates the linked component behaviour accoding to the Linked Game Object, Linked Behaviour and Field values
        /// </summary>
        public void ValidateLinkedData()
        {
            if (linkedGameObject == null)
                return;

            if ((linkedBehaviours == null || previousLinkedGameObject != linkedGameObject || linkedBehaviours.Count <= 0))
            {
                previousLinkedGameObject = linkedGameObject;
                linkedBehaviours = GetLinkedBehaviour();
            }

            if ((linkedBehaviour == null || !linkedBehaviours.Contains(linkedBehaviour)) && linkedBehaviours.Count > 0)
                linkedBehaviour = linkedBehaviours[0];

            if (fields == null || fields.Count < 3)
                fields = GetStringFieldsInBehaviour();

            if (field == null || !fields.Contains(field))
                field = fields[0];
        }

        /// <summary>
        /// Gets a list of the available string fields that can be targeted from the Linked Behaviour
        /// </summary>
        /// <returns></returns>
        public List<string> GetStringFieldsInBehaviour()
        {
            if (linkedBehaviour != null)
            {
                List<string> results = new List<string>();
                foreach (PropertyInfo info in linkedBehaviour.GetType().GetProperties())
                {
                    if (info.PropertyType == typeof(string))
                        results.Add(info.Name);
                }
                return results;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the string value of the linked field
        /// </summary>
        /// <returns></returns>
        public string GetLinkedFieldValue()
        {
            if (field != null && linkedBehaviour != null)
            {
                return linkedBehaviour.GetType().GetProperty(field).GetValue(linkedBehaviour, null).ToString();
            }
            return null;
        }

        /// <summary>
        /// Sets the string value of the linked field
        /// </summary>
        /// <param name="value"></param>
        public void SetLinkedFieldValue(string value)
        {
            if (field != null && linkedBehaviour != null)
            {
                linkedBehaviour.GetType().GetProperty(field).SetValue(linkedBehaviour, value, null);
            }
        }
    }
}
