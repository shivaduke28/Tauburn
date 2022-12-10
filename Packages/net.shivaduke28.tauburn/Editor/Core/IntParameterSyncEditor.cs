using Tauburn.Core;
using UnityEditor;
using UnityEngine;

namespace Tauburn.Editor.Core
{
    [CustomEditor(typeof(IntParameterSync))]
    public sealed class IntParameterSyncEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Collect Providers and Views in children"))
            {
                var sync = (IntParameterSync) target;
                Undo.RecordObject(sync, "IntParameterSync.CollectProvidersAndViewsInChildren");
                sync.CollectProvidersAndViewsInChildren();
            }
        }
    }
}
