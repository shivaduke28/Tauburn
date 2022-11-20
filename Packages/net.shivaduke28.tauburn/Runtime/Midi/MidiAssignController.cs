using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Tauburn.Midi
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), AddComponentMenu("Tauburn MidiAssignController")]
    public sealed class MidiAssignController : UdonSharpBehaviour
    {
        [SerializeField] MidiIntInput[] midiIntInputs;
        [SerializeField] MidiFloatInput[] midiFloatInputs;
        [SerializeField] Toggle toggle;

        // called from Toggle
        public void OnAssignToggle()
        {
            var isOn = toggle.isOn;
            foreach (var input in midiIntInputs)
            {
                input.SetAssignActive(isOn);
            }
            foreach (var input in midiFloatInputs)
            {
                input.SetAssignActive(isOn);
            }
        }
    }
}
