using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Tauburn.Core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public sealed class FloatParameterSync : UdonSharpBehaviour
    {
        [SerializeField] FloatParameterHandler parameterHandler;
        [SerializeField] FloatParameterInput[] inputs;

        [UdonSynced, FieldChangeCallback(nameof(SyncedValue))]
        float syncedValue;

        public float SyncedValue
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

        public void Set(float value)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = value;
            RequestSerialization();
        }
    }
}
