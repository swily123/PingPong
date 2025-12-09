using System;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public event Action CollisionEntered;
    
    private void OnCollisionEnter(Collision other)
    {
        var collidedRigidbody = other.collider.attachedRigidbody;
        
        if (collidedRigidbody!= null)
        {
            if (collidedRigidbody.GetComponent<Ball>() != null)
            {
                CollisionEntered?.Invoke();
            }
        }
    }
}