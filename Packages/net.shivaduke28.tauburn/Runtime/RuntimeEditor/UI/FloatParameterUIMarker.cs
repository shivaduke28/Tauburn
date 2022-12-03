using Tauburn.Core;
using Tauburn.Midi;
using Tauburn.UI;
using TMPro;
using UnityEngine;

namespace Tauburn.RuntimeEditor.UI
{
    public sealed class FloatParameterUIMarker : MonoBehaviour
    {
        public TMP_Text nameText;
        public FloatParameterSync floatParameterSync;
        public FloatParameterSlider floatParameterSlider;
        public MidiFloatInput midiFloatInput;
    }
}
