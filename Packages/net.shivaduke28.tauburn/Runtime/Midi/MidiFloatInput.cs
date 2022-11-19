using Tauburn.Core;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Midi;

namespace Tauburn.Midi
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), RequireComponent(typeof(VRCMidiListener))]
    public sealed class MidiFloatInput : FloatParameterProvider
    {
        [SerializeField] GameObject assignRoot;
        [SerializeField] TextMeshProUGUI midiText;

        const float OneOver127 = 1f / 127f;

        FloatParameterHandler parameterHandler;
        bool isAssigning;
        int midiNumber = -1;


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

        public override void Register(FloatParameterHandler parameterHandler)
        {
            this.parameterHandler = parameterHandler;
        }

        public void SetAssignActive(bool active)
        {
            assignRoot.SetActive(active);
            if (!active)
            {
                isAssigning = false;
            }
        }

        void OnMidiInput(int number, float value)
        {
            if (isAssigning)
            {
                midiNumber = number;
                midiText.text = number.ToString();
                isAssigning = false;
            }
            else if (number == midiNumber)
            {
                parameterHandler.Set(value);
            }
        }

        // called from AssignButton
        public void OnAssignButtonClick()
        {
            isAssigning = true;
        }


        public void ResetAssignment()
        {
            midiText.text = "none";
            midiNumber = -1;
            isAssigning = false;
        }
    }
}
