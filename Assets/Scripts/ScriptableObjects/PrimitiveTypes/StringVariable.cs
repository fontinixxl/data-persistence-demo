using UnityEngine;

namespace ScriptableObjects.PrimitiveTypes
{
    [CreateAssetMenu(fileName = "MyStringVariable", menuName = "SO/Variables/StringVariable", order = 0)]
    public class StringVariable : EventChannelBaseSO
    {
        [SerializeField]
        private string value = "";

        public string Value
        {
            get => value;
            set => this.value = value;
        }
        
        private void OnEnable()
        {
            value = string.Empty;
        }
    }
}
