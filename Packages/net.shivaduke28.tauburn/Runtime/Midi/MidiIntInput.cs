using Tauburn.Core;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Midi;

namespace Tauburn.Midi
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), RequireComponent(typeof(VRCMidiListener))]
    public sealed class MidiIntInput : IntParameterProvider
    {
        [SerializeField] int parameterValue;
        [SerializeField] GameObject assignRoot;
        [SerializeField] TextMeshProUGUI midiText;

        IntParameterHandler parameterHandler;
        bool isAssigning;
        int midiNumber = -1;

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
                midiNumber = number;
                midiText.text = number.ToString();
                isAssigning = false;
            }
            else if (number == midiNumber)
            {
                parameterHandler.Set(parameterValue);
            }
        }

        // called from AssignButton
        public void OnAssignButtonClick()
        {
            Debug.Log("assign button click");
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
