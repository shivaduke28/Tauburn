using Tauburn.Midi;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;

namespace Editor.Midi
{
    [CustomEditor(typeof(MidiAssignController))]
    public sealed class MidiAssignControllerEditor : UnityEditor.Editor
    {
        MidiAssignController midiAssignController;
        SerializedProperty midiFloatInputs;
        SerializedProperty midiIntInputs;

        void OnEnable()
        {
            midiAssignController = (MidiAssignController) target;
            midiFloatInputs = serializedObject.FindProperty(nameof(midiFloatInputs));
            midiIntInputs = serializedObject.FindProperty(nameof(midiIntInputs));
        }

        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target, false, false)) return;
            base.OnInspectorGUI();

            GUILayout.Space(10f);
            if (GUILayout.Button("Collect all MIDI Inputs in Children."))
            {
                var floatInputs = midiAssignController.GetComponentsInChildren<MidiFloatInput>(true);
                var intInputs = midiAssignController.GetComponentsInChildren<MidiIntInput>(true);
                midiFloatInputs.arraySize = floatInputs.Length;
                midiIntInputs.arraySize = intInputs.Length;

                for (var i = 0; i < floatInputs.Length; i++)
                {
                    midiFloatInputs.GetArrayElementAtIndex(i).objectReferenceValue = floatInputs[i];
                }

                for (var i = 0; i < intInputs.Length; i++)
                {
                    midiIntInputs.GetArrayElementAtIndex(i).objectReferenceValue = intInputs[i];
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
