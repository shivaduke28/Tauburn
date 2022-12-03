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
        [SerializeField] TextMeshProUGUI midiText;
        [SerializeField] Animator animator;
        readonly int stateId = Animator.StringToHash("state");
        MidiAssignState state = MidiAssignState.None;

        IntParameterHandler parameterHandler;

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

        void SetState(MidiAssignState assignState)
        {
            state = assignState;
            switch (state)
            {
                case MidiAssignState.None:
                    animator.SetInteger(stateId, 0);
                    break;
                case MidiAssignState.Active:
                    animator.SetInteger(stateId, 1);
                    break;
                case MidiAssignState.Assigning:
                    animator.SetInteger(stateId, 2);
                    break;
            }
        }

        public void SetAssignActive(bool active)
        {
            SetState(active ? MidiAssignState.Active : MidiAssignState.None);
        }

        void OnMidiInput(int number)
        {
            if (state == MidiAssignState.Assigning)
            {
                MidiNumber = number;
                midiText.text = number.ToString();
                SetState(MidiAssignState.Active);
            }
            else if (number == MidiNumber)
            {
                parameterHandler.Set(parameterValue);
            }
        }

        // called from AssignButton
        public void OnAssignButtonClick()
        {
            Debug.Log(state);
            SetState(state == MidiAssignState.Active ? MidiAssignState.Assigning : MidiAssignState.Active);
        }

        public void ForceAssign(int number)
        {
            MidiNumber = number;
            midiText.text = number == -1 ? "none" : number.ToString();
            SetState(MidiAssignState.Active);
        }

        public void ResetAssignment()
        {
            midiText.text = "none";
            MidiNumber = -1;
            SetState(MidiAssignState.Active);
        }
    }
}
