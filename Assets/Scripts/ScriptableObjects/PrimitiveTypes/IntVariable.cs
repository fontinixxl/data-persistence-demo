using UnityEngine;

namespace ScriptableObjects.PrimitiveTypes
{
    [CreateAssetMenu(fileName = "MyIntVariable", menuName = "SO/Variables/IntVariable", order = 1)]
    public class IntVariable : ScriptableObject
    {
        
#if UNITY_EDITOR
        [Multiline]
        [SerializeField] private string developerDescription = string.Empty;
#endif
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
