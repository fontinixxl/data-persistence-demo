using UnityEngine;

namespace Fontinixxl.Gameplay
{
    public class DeathZone : MonoBehaviour
    {
        public GameController Manager;

        private void OnCollisionEnter(Collision other)
        {
            Destroy(other.gameObject);
            // TODO: refactor it to a event channel that GameController and InGameUIHandler will be listen to
            // TODO: .. Once the GameController receive it, will just switch to GameOver State
            Manager.GameOver();
        }
    }
}
