using Tauburn.Core;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Midi;

namespace Tauburn.Input
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), RequireComponent(typeof(VRCMidiListener))]
    public sealed class MidiIntInput : IntParameterInput
    {
        [SerializeField] int parameterValue;
        [SerializeField] Toggle assignToggle;
        [SerializeField] TextMeshProUGUI midiText;
        [SerializeField] Animator animator;

        readonly int isOnId = Animator.StringToHash("isOn");

        bool isAssigning;
        int midiNumber = -1;

        public override void Set(int value)
        {
            animator.SetBool(isOnId, parameterValue == value);
        }

        // VRCMidiListener
        public override void MidiNoteOn(int channel, int number, int velocity)
        {
            if (velocity == 0) return;
            OnMidiInput(number);
        }

        // VRCMidiListener
        // might not be used.
        public override void MidiNoteOff(int channel, int number, int velocity)
        {
            OnMidiInput(number);
        }

        // VRCMidiListener
        public override void MidiControlChange(int channel, int number, int _)
        {
            OnMidiInput(number);
        }

        void OnMidiInput(int number)
        {
            if (isAssigning)
            {
                midiNumber = number;
                midiText.text = number.ToString();
                assignToggle.isOn = false;
            }
            else if (number == midiNumber)
            {
                SetValue();
            }
        }

        // called from Button
        public void SetValue()
        {
            parameterSync.Set(parameterValue);
        }

        // called from Toggle
        public void OnToggleAssign()
        {
            isAssigning = assignToggle.isOn;
        }

        public void ResetAssignment()
        {
            midiText.text = "none";
            midiNumber = -1;
            assignToggle.isOn = false;
        }
    }
}
