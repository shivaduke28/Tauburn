using Tauburn.Core;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Midi;

namespace Tauburn.Midi
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), RequireComponent(typeof(VRCMidiListener)), AddComponentMenu("Tauburn MidiIntInput")]
    public sealed class MidiIntInput : IntParameterProvider
    {
        [SerializeField] int parameterValue;
        [SerializeField] GameObject assignRoot;
        [SerializeField] TextMeshProUGUI midiText;

        IntParameterHandler parameterHandler;
        bool isAssigning;
        public int MidiNumber { get; private set; } = -1;

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

        // IntParameterProvider
        public override void Register(IntParameterHandler parameterHandler)
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

        void OnMidiInput(int number)
        {
            if (isAssigning)
            {
                MidiNumber = number;
                midiText.text = number.ToString();
                isAssigning = false;
            }
            else if (number == MidiNumber)
            {
                parameterHandler.Set(parameterValue);
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
