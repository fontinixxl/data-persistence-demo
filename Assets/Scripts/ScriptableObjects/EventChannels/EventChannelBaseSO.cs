using UnityEngine;

public class EventChannelBaseSO : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea] public string description = string.Empty;
#endif
}
