using Tauburn.Core;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Midi;

namespace Tauburn.Input
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), RequireComponent(typeof(VRCMidiListener))]
    public sealed class MidiFloatInput : FloatParameterInput
    {
        [SerializeField] Toggle assignToggle;
        [SerializeField] TextMeshProUGUI midiText;
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] Animator animator;
        [SerializeField] Slider slider;

        readonly int valueId = Animator.StringToHash("value");
        const float OneOver127 = 1f / 127f;

        bool isAssigning;
        bool isEditingSlider;
        int midiNumber = -1;

        // FloatParameterInput
        public override void Set(float value)
        {
            valueText.text = value.ToString("N");
            animator.SetFloat(valueId, value);
            isEditingSlider = true;
            slider.value = value;
            isEditingSlider = false;
        }

        // VRCMidiListener
        public override void MidiNoteOn(int channel, int number, int velocity)
        {
            OnMidiInput(number, velocity * OneOver127);
        }

        // VRCMidiListener
        public override void MidiNoteOff(int channel, int number, int velocity)
        {
            OnMidiInput(number, 0);
        }

        // VRCMidiListener
        public override void MidiControlChange(int channel, int number, int value)
        {
            OnMidiInput(number, value * OneOver127);
        }

        void OnMidiInput(int number, float value)
        {
            if (isAssigning)
            {
                midiNumber = number;
                midiText.text = number.ToString();
                assignToggle.isOn = false;
            }
            else if (number == midiNumber)
            {
                parameterSync.Set(value);
            }
        }

        // called from Toggle
        public void OnToggleAssign()
        {
            isAssigning = assignToggle.isOn;
        }

        // called from Slider
        public void OnSliderValueChange()
        {
            if (isEditingSlider) return;
            parameterSync.Set(slider.value);
        }

        public void ResetAssignment()
        {
            midiText.text = "none";
            midiNumber = -1;
            assignToggle.isOn = false;
        }
    }
}
