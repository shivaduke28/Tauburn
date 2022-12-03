using Tauburn.Midi;
using TMPro;
using UnityEngine;

namespace Tauburn.RuntimeEditor.UI
{
    public sealed class UIMarker : MonoBehaviour
    {
        public TMP_Text nameText;
        public MidiAssignController midiAssignController;
        public Transform parametersRoot;
        public FloatParameterUIMarker floatParameterUIMarkerPrefab;
        public IntParameterUIMarker intParameterUIMarkerPrefab;
    }
}
