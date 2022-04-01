using Fontinixxl.Shared.ScriptableObjects.EventChannels;
using UnityEngine;

namespace Fontinixxl.Shared.ScriptableObjects.ScriptableTypes
{
    [CreateAssetMenu(fileName = "MyIntVariable", menuName = "SO/Variables/IntVariable", order = 1)]
    public class IntVariable : EventChannelBaseSO
    {
        public int Value;
        public bool ResetOnStart;
        public int DefaultValue;
        
        public void SetValue(int value)
        {
            Value = value;
        }

        public void ApplyChange(int amount)
        {
            Value += amount;
        }

        private void OnEnable()
        {
            if (ResetOnStart) SetValue(DefaultValue);
        }
    }
}
