using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public sealed class BuildLayoutContext
        {
            public readonly List<MidiFloatInput> MidiFloatInputs = new List<MidiFloatInput>();
            public readonly List<MidiIntInput> MidiIntInputs = new List<MidiIntInput>();
        }

        public sealed class BuildContentContext
        {
            public readonly List<FloatParameterHandler> FloatParameterHandlers = new List<FloatParameterHandler>();
            public readonly List<IntParameterHandler> IntParameterHandlers = new List<IntParameterHandler>();
        }

        public static void BuildAnimationUI(LayoutUIMarker layoutUIMarkerPrefab, AnimationBuildData[] animationBuildDatum,
            Transform parent = null)
        {
            var context = new BuildLayoutContext();

            var canvas = CreateCanvas(parent);
            var layout = GameObject.Instantiate(layoutUIMarkerPrefab, canvas.transform, false);

            foreach (var animationBuildData in animationBuildDatum)
            {
                BuildAnimationContent(layout, animationBuildData, context);
            }

            // MIDI Assign
            var midiAssign = layout.midiAssignController;
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

            serializedMidiAssign.ApplyModifiedProperties();

            var canvasRect = canvas.GetComponent<RectTransform>();
            var layoutRect = layout.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRect);
            canvasRect.sizeDelta = layoutRect.sizeDelta + new Vector2(50f, 20f);
            layoutRect.localPosition = -new Vector2(20f, 0f);
            Selection.activeGameObject = canvas.gameObject;
        }

        static async Task ApplyCanvasRect(RectTransform canvasRect, RectTransform layoutRect)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1f));
        }

        static void BuildAnimationContent(LayoutUIMarker layoutUIMarker, AnimationBuildData animationBuildData, BuildLayoutContext layoutContext)
        {
            var contentContext = new BuildContentContext();
            var uiMarker = GameObject.Instantiate(layoutUIMarker.contentUIMarkerPrefab, layoutUIMarker.contentsRoot, false);
            var animator = animationBuildData.Animator;
            var buildData = animationBuildData.BuildData;
            uiMarker.gameObject.name = buildData.displayName;
            uiMarker.nameText.text = buildData.displayName;

            var animatorBridgeBuilder = new AnimatorBridgeBuilder(animator, buildData);
            var animatorBridge = animatorBridgeBuilder.CreateAnimatorBridge();

            // Float
            var inputFloatParameters = buildData.floatParameters;

            foreach (var (input, i) in inputFloatParameters.Select((x, i) => (x, i)))
            {
                animatorBridgeBuilder.AddFloatParameterHandler(input.name, i, contentContext);
                CreateFloatParameterUI(uiMarker.floatParameterUIMarkerPrefab,
                    uiMarker.parametersRoot,
                    input,
                    i,
                    contentContext,
                    layoutContext);
            }

            // Int
            var inputIntParameters = buildData.intParameters;

            foreach (var (input, i) in inputIntParameters.Select((x, i) => (x, i)))
            {
                animatorBridgeBuilder.AddIntParameterHandler(input.name, i, contentContext);
                CreateIntParameterUI(uiMarker.intParameterUIMarkerPrefab,
                    uiMarker.parametersRoot,
                    input,
                    i, contentContext,
                    layoutContext);
            }

            animatorBridgeBuilder.Apply();
            animatorBridge.transform.SetParent(uiMarker.transform, false);
        }

        static FloatParameterUIMarker CreateFloatParameterUI(
            FloatParameterUIMarker prefab,
            Transform parent,
            BuildData.FloatParameter inputFloatParameter,
            int index,
            BuildContentContext contentContext,
            BuildLayoutContext layoutContext)
        {
            var marker = GameObject.Instantiate(prefab, parent, false);
            marker.nameText.text = inputFloatParameter.displayName;
            var parameterSync = marker.floatParameterSync;
            var serializedParameterSync = new SerializedObject(parameterSync);
            serializedParameterSync.FindProperty("parameterHandler").objectReferenceValue = contentContext.FloatParameterHandlers[index];
            serializedParameterSync.ApplyModifiedProperties();
            layoutContext.MidiFloatInputs.Add(marker.midiFloatInput);
            return marker;
        }

        static IntParameterUIMarker CreateIntParameterUI(IntParameterUIMarker prefab,
            Transform parent,
            BuildData.IntParameter inputIntParameter,
            int index,
            BuildContentContext contentContext,
            BuildLayoutContext layoutContext)
        {
            var marker = GameObject.Instantiate(prefab, parent, false);
            marker.nameText.text = inputIntParameter.displayName;
            var parameterSync = marker.intParameterSync;
            var serializedParameterSync = new SerializedObject(parameterSync);
            serializedParameterSync.FindProperty("parameterHandler").objectReferenceValue = contentContext.IntParameterHandlers[index];

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


        static Canvas CreateCanvas(Transform parent)
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
