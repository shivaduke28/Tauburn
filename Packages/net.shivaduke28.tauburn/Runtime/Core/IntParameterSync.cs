using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Tauburn.Core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public sealed class IntParameterSync : UdonSharpBehaviour
    {
        [SerializeField] IntParameterHandler parameterHandler;
        [SerializeField] IntParameterInput[] inputs;

        [UdonSynced, FieldChangeCallback(nameof(SyncedValue))]
        int syncedValue;

        public int SyncedValue
        {
            get => syncedValue;
            set
            {
                syncedValue = value;
                parameterHandler.Set(value);
                foreach (var input in inputs)
                {
                    input.Set(value);
                }
            }
        }

        void Start()
        {
            foreach (var input in inputs)
            {
                input.Initialize(this);
            }
        }

        public void Set(int value)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = value;
            RequestSerialization();
        }
    }
}
