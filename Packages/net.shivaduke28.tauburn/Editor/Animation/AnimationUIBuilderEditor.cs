using Tauburn.RuntimeEditor.Animation;
using UnityEditor;
using UnityEngine;

namespace Tauburn.Editor.Animation
{
    [CustomEditor(typeof(AnimationUIBuilder))]
    internal class AnimationUIBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(20f);
            if (GUILayout.Button("Create Animation UI"))
            {
                var uiBuilder = (AnimationUIBuilder) target;
                UIBuilder.BuildAnimationUI(uiBuilder.LayoutUIMarker, uiBuilder.AnimationBuildDatum, uiBuilder.transform);
            }
        }
    }
}
