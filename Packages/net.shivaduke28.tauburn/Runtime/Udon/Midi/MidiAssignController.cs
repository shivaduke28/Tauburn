using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon;

namespace Tauburn.Midi
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), AddComponentMenu("Tauburn MidiAssignController")]
    public sealed class MidiAssignController : UdonSharpBehaviour
    {
        [SerializeField] MidiFloatInput[] midiFloatInputs;
        [SerializeField] MidiIntInput[] midiIntInputs;
        [SerializeField] InputField inputField;
        [SerializeField] GameObject menu;

        bool isAssigning;

        // called from Toggle
        public void ToggleAssign()
        {
            isAssigning = !isAssigning;
            foreach (var input in midiFloatInputs)
            {
                input.SetAssignActive(isAssigning);
            }
            foreach (var input in midiIntInputs)
            {
                input.SetAssignActive(isAssigning);
            }
        }

        public void ToggleMenu()
        {
            menu.SetActive(!menu.activeSelf);
        }

        // called from Button
        public void Save()
        {
            var result = "";
            foreach (var input in midiFloatInputs)
            {
                result += $"{input.MidiNumber},";
            }

            foreach (var input in midiIntInputs)
            {
                result += $"{input.MidiNumber},";
            }

            inputField.text = result;
        }

        // called from button
        public void Load()
        {
            var text = inputField.text;
            var length = text.Length;
            var numbers = new int[length];
            var str = "";
            var index = 0;
            for (var i = 0; i < length; i++)
            {
                var character = text[i];
                if (character != ',')
                {
                    str = $"{str}{character}";
                }
                else
                {
                    if (!int.TryParse(str, out var num))
                    {
                        num = -1;
                    }
                    numbers[index] = num;
                    index++;
                    str = "";
                }
            }

            var floatCount = midiFloatInputs.Length;
            for (var i = 0; i < floatCount && i < length; i++)
            {
                midiFloatInputs[i].ForceAssign(numbers[i]);
            }

            var offset = floatCount;
            var intCount = midiIntInputs.Length;
            for (var i = 0; i < intCount && i + offset < length; i++)
            {
                midiIntInputs[i].ForceAssign(numbers[i + offset]);
            }
        }

        public void Reset()
        {
            foreach (var input in midiIntInputs)
            {
                input.ResetAssignment();
            }
            foreach (var input in midiFloatInputs)
            {
                input.ResetAssignment();
            }
        }
    }
}
