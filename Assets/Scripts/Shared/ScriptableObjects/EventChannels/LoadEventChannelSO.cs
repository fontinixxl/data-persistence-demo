using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Fontinixxl.Shared.ScriptableObjects.EventChannels
{
	/// <summary>
	/// This class is used for scene-loading events.
	/// Takes a GameSceneSO of the location or menu that needs to be loaded, and a bool to specify if a loading screen needs to display.
	/// </summary>
	[CreateAssetMenu(menuName = "Events/Load Event Channel")]
	public class LoadEventChannelSO : EventChannelBaseSO
	{
		public UnityAction<AssetReference> OnLoadingRequested;

		public void RaiseEvent(AssetReference locationsToLoad)
		{
			if (OnLoadingRequested != null)
			{
				OnLoadingRequested.Invoke(locationsToLoad);
			}
			else
			{
				Debug.LogWarning("A Scene loading was requested, but nobody picked it up." +
				                 "Check why there is no SceneLoader already present, " +
				                 "and make sure it's listening on this Load Event channel.");
			}
		}
	}
}
