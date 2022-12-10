using Tauburn.Midi;
using UnityEngine;

namespace Tauburn.RuntimeEditor.UI
{
    public sealed class LayoutUIMarker : MonoBehaviour
    {
        public Transform contentsRoot;
        public ContentUIMarker contentUIMarkerPrefab;
        public MidiAssignController midiAssignController;
    }
}
