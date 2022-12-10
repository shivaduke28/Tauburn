using System.Collections.Generic;
using System.Linq;
using Tauburn.Core;
using Tauburn.Midi;
using Tauburn.RuntimeEditor.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;

namespace Tauburn.Editor.UI
{
    public static class UIBuilder
    {
        public sealed class BuildLayoutContext
        {
            public readonly List<MidiFloatInput> MidiFloatInputs = new List<MidiFloatInput>();
            public readonly List<MidiIntInput> MidiIntInputs = new List<MidiIntInput>();
        }

        public sealed class BuildContentContext
        {
            public readonly List<FloatParameterSync> FloatParameterSyncs = new List<FloatParameterSync>();
            public readonly List<IntParameterSync> IntParameterSyncs = new List<IntParameterSync>();
        }

        // Build ContentUIMarkers as children of LayoutMarker following BuildData.
        public static ContentUIMarker BuildContent(LayoutUIMarker layoutUIMarker, BuildData buildData, BuildLayoutContext layoutContext,
            BuildContentContext contentContext)
        {
            var contentUIMarker = GameObject.Instantiate(layoutUIMarker.contentUIMarkerPrefab, layoutUIMarker.contentsRoot, false);
            contentUIMarker.gameObject.name = buildData.displayName;
            contentUIMarker.nameText.text = buildData.displayName;

            // Float
            var inputFloatParameters = buildData.floatParameters;

            foreach (var input in inputFloatParameters)
            {
                var parameterUI = CreateFloatParameterUI(contentUIMarker.floatParameterUIMarkerPrefab,
                    contentUIMarker.parametersRoot,
                    input,
                    layoutContext);
                contentContext.FloatParameterSyncs.Add(parameterUI.floatParameterSync);
            }

            // Int
            var inputIntParameters = buildData.intParameters;

            foreach (var input in inputIntParameters)
            {
                var parameterUI = CreateIntParameterUI(contentUIMarker.intParameterUIMarkerPrefab,
                    contentUIMarker.parametersRoot,
                    input,
                    layoutContext);
                contentContext.IntParameterSyncs.Add(parameterUI.intParameterSync);
            }

            return contentUIMarker;
        }

        // Insert MidiInputs in BuildLayoutContext to MidiAssignController.
        public static void SetupMidiAssignController(LayoutUIMarker layoutUIMarker, BuildLayoutContext layoutContext)
        {
            var midiAssign = layoutUIMarker.midiAssignController;
            var serializedMidiAssign = new SerializedObject(midiAssign);
            var serializedMidiAssignFloat = serializedMidiAssign.FindProperty("midiFloatInputs");
            var serializedMidiAssignInt = serializedMidiAssign.FindProperty("midiIntInputs");

            serializedMidiAssignFloat.arraySize = layoutContext.MidiFloatInputs.Count;
            foreach (var (input, i) in layoutContext.MidiFloatInputs.Select((x, i) => (x, i)))
            {
                serializedMidiAssignFloat.GetArrayElementAtIndex(i).objectReferenceValue = input;
            }

            serializedMidiAssignInt.arraySize = layoutContext.MidiIntInputs.Count;
            foreach (var (input, i) in layoutContext.MidiIntInputs.Select((x, i) => (x, i)))
            {
                serializedMidiAssignInt.GetArrayElementAtIndex(i).objectReferenceValue = input;
            }

            serializedMidiAssign.ApplyModifiedProperties();
        }

        static FloatParameterUIMarker CreateFloatParameterUI(
            FloatParameterUIMarker prefab,
            Transform parent,
            BuildData.FloatParameter inputFloatParameter,
            BuildLayoutContext layoutContext)
        {
            var marker = GameObject.Instantiate(prefab, parent, false);
            marker.nameText.text = inputFloatParameter.displayName;
            layoutContext.MidiFloatInputs.Add(marker.midiFloatInput);
            return marker;
        }

        static IntParameterUIMarker CreateIntParameterUI(IntParameterUIMarker prefab,
            Transform parent,
            BuildData.IntParameter inputIntParameter,
            BuildLayoutContext layoutContext)
        {
            var marker = GameObject.Instantiate(prefab, parent, false);
            marker.nameText.text = inputIntParameter.displayName;
            var parameterSync = marker.intParameterSync;
            var serializedParameterSync = new SerializedObject(parameterSync);

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
                layoutContext.MidiIntInputs.Add(buttonMarker.midiIntInput);
            }

            serializedParameterSync.ApplyModifiedProperties();
            return marker;
        }


        public static Canvas CreateCanvas(Transform parent)
        {
            var go = new GameObject("Canvas");
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            const float scale = 0.01f;
            var rect = canvas.GetComponent<RectTransform>();
            rect.localScale = new Vector3(scale, scale, scale);
            rect.SetParent(parent, false);

            go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();
            go.AddComponent<VRCUiShape>();
            return canvas;
        }
    }
}
