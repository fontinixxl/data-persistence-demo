using UnityEngine;

namespace Fontinixxl.ScriptableObjects.EventChannels
{
    public class EventChannelBaseSO : ScriptableObject
    {
#if UNITY_EDITOR
        [TextArea] public string description = string.Empty;
#endif
    }
}
