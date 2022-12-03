using Tauburn.Core;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Midi;

namespace Tauburn.Midi
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), RequireComponent(typeof(VRCMidiListener)), AddComponentMenu("Tauburn MidiFloatInput")]
    public sealed class MidiFloatInput : FloatParameterProvider
    {
        [SerializeField] GameObject assignRoot;
        [SerializeField] TextMeshProUGUI midiText;

        const float OneOver127 = 1f / 127f;

        FloatParameterHandler parameterHandler;
        bool isAssigning;
        public int MidiNumber { get; private set; } = -1;


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
                MidiNumber = number;
                midiText.text = number.ToString();
                isAssigning = false;
            }
            else if (number == MidiNumber)
            {
                parameterHandler.Set(value);
            }
        }

        // called from AssignButton
        public void OnAssignButtonClick()
        {
            isAssigning = true;
        }

        public void ForceAssign(int number)
        {
            MidiNumber = number;
            midiText.text = number == -1 ? "none" : number.ToString();
            isAssigning = false;
        }

        public void ResetAssignment()
        {
            midiText.text = "none";
            MidiNumber = -1;
            isAssigning = false;
        }
    }
}
