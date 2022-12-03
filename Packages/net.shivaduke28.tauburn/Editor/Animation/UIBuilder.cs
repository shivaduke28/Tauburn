using System.Collections.Generic;
using System.Linq;
using Tauburn.Core;
using Tauburn.Editor.Animation;
using Tauburn.Midi;
using Tauburn.RuntimeEditor.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;

namespace Tauburn.RuntimeEditor.Animation
{
    public static class UIBuilder
    {
        public sealed class BuildContext
        {
            public readonly List<FloatParameterHandler> FloatParameterHandlers = new List<FloatParameterHandler>();
            public readonly List<IntParameterHandler> IntParameterHandlers = new List<IntParameterHandler>();
            public readonly List<MidiFloatInput> MidiFloatInputs = new List<MidiFloatInput>();
            public readonly List<MidiIntInput> MidiIntInputs = new List<MidiIntInput>();
        }

        public static void BuildAnimationUI(Animator animator, UIMarker prefab, BuildData buildData, Transform parent = null)
        {
            var context = new BuildContext();

            var canvas = CreateCanvas(parent);
            var uiMarker = GameObject.Instantiate(prefab, canvas.transform, false);
            uiMarker.gameObject.name = $"{prefab.name}_{buildData.displayName}";
            uiMarker.nameText.text = buildData.displayName;

            var animatorBridgeBuilder = new AnimatorBridgeBuilder(animator, buildData);
            var animatorBridge = animatorBridgeBuilder.CreateAnimatorBridge();

            // Float
            var inputFloatParameters = buildData.floatParameters;

            foreach (var (input, i) in inputFloatParameters.Select((x, i) => (x, i)))
            {
                animatorBridgeBuilder.AddFloatParameterHandler(input.name, i, context);
                CreateFloatParameterUI(uiMarker.floatParameterUIMarkerPrefab,
                    uiMarker.parametersRoot,
                    input,
                    i,
                    context);
            }

            // Int
            var inputIntParameters = buildData.intParameters;

            foreach (var (input, i) in inputIntParameters.Select((x, i) => (x, i)))
            {
                animatorBridgeBuilder.AddIntParameterHandler(input.name, i, context);
                CreateIntParameterUI(uiMarker.intParameterUIMarkerPrefab,
                    uiMarker.parametersRoot,
                    input,
                    i, context);
            }

            // MIDI Assign
            var midiAssign = uiMarker.midiAssignController;
            var serializedMidiAssign = new SerializedObject(midiAssign);
            var serializedMidiAssignFloat = serializedMidiAssign.FindProperty("midiFloatInputs");
            var serializedMidiAssignInt = serializedMidiAssign.FindProperty("midiIntInputs");

            serializedMidiAssignFloat.arraySize = context.MidiFloatInputs.Count;
            foreach (var (input, i) in context.MidiFloatInputs.Select((x, i) => (x, i)))
            {
                serializedMidiAssignFloat.GetArrayElementAtIndex(i).objectReferenceValue = input;
            }

            serializedMidiAssignInt.arraySize = context.MidiIntInputs.Count;
            foreach (var (input, i) in context.MidiIntInputs.Select((x, i) => (x, i)))
            {
                serializedMidiAssignInt.GetArrayElementAtIndex(i).objectReferenceValue = input;
            }

            animatorBridgeBuilder.Apply();
            serializedMidiAssign.ApplyModifiedProperties();
            animatorBridge.transform.SetParent(uiMarker.transform, false);

            Selection.activeGameObject = canvas.gameObject;
        }

        static FloatParameterUIMarker CreateFloatParameterUI(FloatParameterUIMarker prefab, Transform parent,
            BuildData.FloatParameter inputFloatParameter, int index, BuildContext context)
        {
            var marker = GameObject.Instantiate(prefab, parent, false);
            marker.nameText.text = inputFloatParameter.displayName;
            var parameterSync = marker.floatParameterSync;
            var serializedParameterSync = new SerializedObject(parameterSync);
            serializedParameterSync.FindProperty("parameterHandler").objectReferenceValue = context.FloatParameterHandlers[index];
            serializedParameterSync.ApplyModifiedProperties();
            context.MidiFloatInputs.Add(marker.midiFloatInput);
            return marker;
        }

        static IntParameterUIMarker CreateIntParameterUI(IntParameterUIMarker prefab, Transform parent,
            BuildData.IntParameter inputIntParameter, int index, BuildContext context)
        {
            var marker = GameObject.Instantiate(prefab, parent, false);
            marker.nameText.text = inputIntParameter.displayName;
            var parameterSync = marker.intParameterSync;
            var serializedParameterSync = new SerializedObject(parameterSync);
            serializedParameterSync.FindProperty("parameterHandler").objectReferenceValue = context.IntParameterHandlers[index];

            var serializedParameterProviders = serializedParameterSync.FindProperty("parameterProviders");
            var serializedParameterViews = serializedParameterSync.FindProperty("parameterViews");

            var states = inputIntParameter.states;
            var count = states.Length;

            serializedParameterProviders.arraySize = count * 2;
            serializedParameterViews.arraySize = count;

            for (var i = 0; i < states.Length; i++)
            {
                var state = states[i];
                var buttonMarker = GameObject.Instantiate(marker.intParameterButtonUIMarkerPrefab, marker.buttonsRoot, false);
                buttonMarker.valueText.text = state.displayName;
                serializedParameterProviders.GetArrayElementAtIndex(i * 2).objectReferenceValue = buttonMarker.intParameterButton;
                serializedParameterProviders.GetArrayElementAtIndex(i * 2 + 1).objectReferenceValue = buttonMarker.midiIntInput;
                serializedParameterViews.GetArrayElementAtIndex(i).objectReferenceValue = buttonMarker.intParameterButton;

                var serializedButton = new SerializedObject(buttonMarker.intParameterButton);
                serializedButton.FindProperty("parameterValue").intValue = state.value;
                serializedButton.ApplyModifiedProperties();
                var serializedMidiInput = new SerializedObject(buttonMarker.midiIntInput);
                serializedMidiInput.FindProperty("parameterValue").intValue = state.value;
                serializedMidiInput.ApplyModifiedProperties();

                context.MidiIntInputs.Add(buttonMarker.midiIntInput);
            }

            serializedParameterSync.ApplyModifiedProperties();
            return marker;
        }


        static Canvas CreateCanvas(Transform parent)
        {
            var go = new GameObject("Canvas");
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            const float scale = 0.01f;
            const float width = 200f;
            const float height = 200f;
            var rect = canvas.GetComponent<RectTransform>();
            rect.localScale = new Vector3(scale, scale, scale);
            rect.sizeDelta = new Vector2(width, height);
            rect.SetParent(parent, false);

            go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();
            go.AddComponent<VRCUiShape>();
            return canvas;
        }
    }
}
