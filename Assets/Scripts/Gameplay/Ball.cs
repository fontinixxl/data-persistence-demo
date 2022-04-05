using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fontinixxl.Gameplay
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody m_Rigidbody;

        private void OnEnable()
        {
            GameController.OnGameStart += ApplyInitVelocity;
        }

        private void OnDisable()
        {
            GameController.OnGameStart -= ApplyInitVelocity;
        }

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void ApplyInitVelocity()
        {
            var randomDirection = Random.Range(-1.0f, 1.0f);
            var forceDir = new Vector3(randomDirection, 1, 0);
            forceDir.Normalize();
            
            transform.SetParent(null);
            m_Rigidbody.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        }
    
        private void OnCollisionExit(Collision other)
        {
            var velocity = m_Rigidbody.velocity;
        
            //after a collision we accelerate a bit
            velocity += velocity.normalized * 0.01f;
        
            //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
            if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
            {
                velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
            }

            //max velocity
            if (velocity.magnitude > 3.0f)
            {
                velocity = velocity.normalized * 3.0f;
            }

            m_Rigidbody.velocity = velocity;
        }
    }
}
