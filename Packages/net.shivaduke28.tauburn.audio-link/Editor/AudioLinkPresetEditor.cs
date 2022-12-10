using UnityEditor;
using UnityEngine;

namespace Tauburn.AudioLink.Editor
{
    [CustomEditor(typeof(AudioLinkPreset))]
    public sealed class AudioLinkPresetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Reset"))
            {
                var preset = (AudioLinkPreset) target;
                Undo.RecordObject(preset, "AudioLinkPreset Reset values");
                preset.ResetValues();
            }
        }
    }
}
