using Tauburn.Animation;
using Tauburn.Editor.UI;
using Tauburn.RuntimeEditor.Animation;
using Tauburn.RuntimeEditor.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Tauburn.Editor.Animation
{
    public sealed class AnimatorBridgeBuilder
    {
        class SerializedProperties
        {
            public SerializedProperty Animator;
            public SerializedProperty FloatParameters;
            public SerializedProperty FloatParameterSyncs;
            public SerializedProperty IntParameters;
            public SerializedProperty IntParameterSyncs;
        }

        public static void Build(AnimationBuildData[] buildDatas, LayoutUIMarker layoutUIMarkerPrefab)
        {
            var canvas = UIBuilder.CreateCanvas(null);
            var layoutContext = new UIBuilder.BuildLayoutContext();
            var layout = GameObject.Instantiate(layoutUIMarkerPrefab, canvas.transform, false);

            // Build AnimatorBridge and Content
            foreach (var buildData in buildDatas)
            {
                CreateAnimatorBridge(buildData, layout, layoutContext);
            }

            // MIDI Assign
            UIBuilder.SetupMidiAssignController(layout, layoutContext);

            var canvasRect = canvas.GetComponent<RectTransform>();
            var layoutRect = layout.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRect);
            canvasRect.sizeDelta = layoutRect.sizeDelta + new Vector2(50f, 20f);
            canvasRect.position = new Vector3(0, 1, 0);
            layoutRect.localPosition = -new Vector2(20f, 0f);
            Selection.activeGameObject = canvas.gameObject;
        }

        static AnimatorBridge CreateAnimatorBridge(AnimationBuildData animationBuildData, LayoutUIMarker layoutUIMarker,
            UIBuilder.BuildLayoutContext layoutContext)
        {
            var animator = animationBuildData.Animator;
            var buildData = animationBuildData.BuildData;

            var context = new UIBuilder.BuildContentContext();

            // まずAnimatorBridgeを作る
            var gameObject = new GameObject(nameof(AnimatorBridge));
            var animatorBridge = gameObject.AddComponent<AnimatorBridge>();

            var serializedObject = new SerializedObject(animatorBridge);
            var serializedProperties = new SerializedProperties
            {
                Animator = serializedObject.FindProperty("animator"),
                FloatParameters = serializedObject.FindProperty("floatParameters"),
                IntParameters = serializedObject.FindProperty("intParameters"),
                FloatParameterSyncs = serializedObject.FindProperty("floatParameterSyncs"),
                IntParameterSyncs = serializedObject.FindProperty("intParameterSyncs"),
            };

            serializedProperties.Animator.objectReferenceValue = animator;
            serializedProperties.FloatParameters.arraySize = buildData.floatParameters.Length;
            serializedProperties.FloatParameterSyncs.arraySize = buildData.floatParameters.Length;
            serializedProperties.IntParameters.arraySize = buildData.intParameters.Length;
            serializedProperties.IntParameterSyncs.arraySize = buildData.intParameters.Length;

            // Build UI Content
            var content = UIBuilder.BuildContent(layoutUIMarker, buildData, layoutContext, context);

            for (var i = 0; i < buildData.floatParameters.Length; i++)
            {
                var param = buildData.floatParameters[i];
                serializedProperties.FloatParameters.GetArrayElementAtIndex(i).objectReferenceValue = AddFloatParameterHandler(param.name, gameObject);
                serializedProperties.FloatParameterSyncs.GetArrayElementAtIndex(i).objectReferenceValue = context.FloatParameterSyncs[i];
            }

            for (var i = 0; i < buildData.intParameters.Length; i++)
            {
                var param = buildData.intParameters[i];
                serializedProperties.IntParameters.GetArrayElementAtIndex(i).objectReferenceValue = AddIntParameterHandler(param.name, gameObject);
                serializedProperties.IntParameterSyncs.GetArrayElementAtIndex(i).objectReferenceValue = context.IntParameterSyncs[i];
            }

            serializedObject.ApplyModifiedProperties();

            gameObject.transform.SetParent(content.transform, false);
            return animatorBridge;
        }

        static AnimationFloatParameter AddFloatParameterHandler(string parameterName, GameObject gameObject)
        {
            var go = new GameObject(parameterName);
            go.transform.SetParent(gameObject.transform, false);
            var handler = go.AddComponent<AnimationFloatParameter>();
            var serialized = new SerializedObject(handler);
            serialized.FindProperty("parameterName").stringValue = parameterName;
            serialized.ApplyModifiedProperties();
            return handler;
        }

        static AnimationIntParameter AddIntParameterHandler(string parameterName, GameObject gameObject)
        {
            var go = new GameObject(parameterName);
            go.transform.SetParent(gameObject.transform, false);
            var handler = go.AddComponent<AnimationIntParameter>();
            var serialized = new SerializedObject(handler);
            serialized.FindProperty("parameterName").stringValue = parameterName;
            serialized.ApplyModifiedProperties();

            return handler;
        }
    }
}
