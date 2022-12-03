using Tauburn.Midi;
using Tauburn.UI;
using TMPro;
using UnityEngine;

namespace Tauburn.RuntimeEditor.UI
{
    public sealed class IntParameterButtonUIMarker : MonoBehaviour
    {
        public TMP_Text valueText;
        public IntParameterButton intParameterButton;
        public MidiIntInput midiIntInput;
    }
}
