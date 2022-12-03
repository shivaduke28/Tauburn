using Tauburn.Animation;
using Tauburn.RuntimeEditor.Animation;
using Tauburn.RuntimeEditor.UI;
using UnityEditor;
using UnityEngine;

namespace Tauburn.Editor.Animation
{
    public sealed class AnimatorBridgeBuilder
    {
        readonly Animator animator;
        readonly BuildData buildData;
        GameObject gameObject;
        SerializedObject serializedObject;

        SerializedProperties serializedProperties;

        class SerializedProperties
        {
            public SerializedProperty Animator;
            public SerializedProperty FloatParameters;
            public SerializedProperty INTParameters;
        }

        public AnimatorBridgeBuilder(Animator animator, BuildData buildData)
        {
            this.animator = animator;
            this.buildData = buildData;
        }

        public AnimatorBridge CreateAnimatorBridge()
        {
            gameObject = new GameObject(nameof(AnimatorBridge));
            var animatorBridge = gameObject.AddComponent<AnimatorBridge>();

            serializedObject = new SerializedObject(animatorBridge);
            serializedProperties = new SerializedProperties
            {
                Animator = serializedObject.FindProperty("animator"),
                FloatParameters = serializedObject.FindProperty("floatParameters"),
                INTParameters = serializedObject.FindProperty("intParameters"),
            };

            serializedProperties.Animator.objectReferenceValue = animator;
            serializedProperties.FloatParameters.arraySize = buildData.floatParameters.Length;
            serializedProperties.INTParameters.arraySize = buildData.intParameters.Length;
            return animatorBridge;
        }

        public AnimationFloatParameter AddFloatParameterHandler(string parameterName, int index, UIBuilder.BuildContext buildContext)
        {
            var go = new GameObject(parameterName);
            go.transform.SetParent(gameObject.transform, false);
            var handler = go.AddComponent<AnimationFloatParameter>();
            var serialized = new SerializedObject(handler);
            serialized.FindProperty("parameterName").stringValue = parameterName;
            serialized.ApplyModifiedProperties();

            serializedProperties.FloatParameters.GetArrayElementAtIndex(index).objectReferenceValue = handler;
            buildContext.FloatParameterHandlers.Add(handler);
            return handler;
        }

        public AnimationIntParameter AddIntParameterHandler(string parameterName, int index, UIBuilder.BuildContext buildContext)
        {
            var go = new GameObject(parameterName);
            go.transform.SetParent(gameObject.transform, false);
            var handler = go.AddComponent<AnimationIntParameter>();
            var serialized = new SerializedObject(handler);
            serialized.FindProperty("parameterName").stringValue = parameterName;
            serialized.ApplyModifiedProperties();

            serializedProperties.INTParameters.GetArrayElementAtIndex(index).objectReferenceValue = handler;
            buildContext.IntParameterHandlers.Add(handler);
            return handler;
        }

        public void Apply()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
