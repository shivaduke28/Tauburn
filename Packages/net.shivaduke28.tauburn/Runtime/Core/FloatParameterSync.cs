using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Tauburn.Core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual), DefaultExecutionOrder(-1)]
    public sealed class FloatParameterSync : FloatParameterHandler
    {
        [SerializeField] FloatParameterHandler parameterHandler;
        [SerializeField] FloatParameterProvider[] parameterProviders;
        [SerializeField] FloatParameterView[] parameterViews;

        [UdonSynced, FieldChangeCallback(nameof(SyncedValue))]
        float syncedValue;

        public float SyncedValue
        {
            get => syncedValue;
            set
            {
                syncedValue = value;
                parameterHandler.Set(value);
                foreach (var view in parameterViews)
                {
                    view.UpdateView(value);
                }
            }
        }

        void Start()
        {
            foreach (var provider in parameterProviders)
            {
                provider.Register(this);
            }
        }

        public override void Set(float value)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            SyncedValue = value;
            RequestSerialization();
        }
    }
}
