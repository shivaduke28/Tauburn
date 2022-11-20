using System;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;

namespace Tauburn.AudioLink.Editor
{
    [CustomEditor(typeof(AudioLinkBridge))]
    public class AudioLinkBridgeEditor : UnityEditor.Editor
    {
        AudioLinkBridge audioLinkBridge;

        void OnEnable()
        {
            audioLinkBridge = (AudioLinkBridge) target;
        }

        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target, false, false)) return;
            base.OnInspectorGUI();

            if (GUILayout.Button("Create AudioLink Sliders"))
            {
                var serializedParams = serializedObject.FindProperty("audioLinkFloatParameters");
                var values = Enum.GetValues(typeof(AudioLinkParameterType));
                serializedParams.arraySize = values.Length;
                foreach (AudioLinkParameterType value in values)
                {
                    var index = (int) value;
                    var serializedRef = serializedParams.GetArrayElementAtIndex(index);
                    var referencedObject = serializedRef.objectReferenceValue;
                    if (!(referencedObject is AudioLinkFloatParameter parameter))
                    {
                        var go = new GameObject(value.ToString());
                        go.transform.SetParent(audioLinkBridge.transform);
                        parameter = go.AddComponent<AudioLinkFloatParameter>();
                    }
                    var serializedParameter = new SerializedObject(parameter);
                    var type = serializedParameter.FindProperty("type");
                    type.enumValueIndex = (int) value;
                    serializedParameter.ApplyModifiedPropertiesWithoutUndo();
                    serializedRef.objectReferenceValue = parameter;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
