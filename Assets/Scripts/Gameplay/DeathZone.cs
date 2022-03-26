using UnityEngine;

namespace Gameplay
{
    public class DeathZone : MonoBehaviour
    {
        public GameController Manager;

        private void OnCollisionEnter(Collision other)
        {
            Destroy(other.gameObject);
            Manager.GameOver();
        }
    }
}
