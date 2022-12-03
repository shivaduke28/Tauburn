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
        [SerializeField] TextMeshProUGUI midiText;
        [SerializeField] Animator animator;
        readonly int stateId = Animator.StringToHash("state");
        MidiAssignState state = MidiAssignState.None;

        const float OneOver127 = 1f / 127f;

        FloatParameterHandler parameterHandler;
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

        void OnMidiInput(int number, float value)
        {
            if (state == MidiAssignState.Assigning)
            {
                MidiNumber = number;
                midiText.text = number.ToString();
                SetState(MidiAssignState.Active);
            }
            else if (number == MidiNumber)
            {
                parameterHandler.Set(value);
            }
        }

        // called from AssignButton
        public void OnAssignButtonClick()
        {
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
