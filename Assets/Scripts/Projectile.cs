using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Life;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 velocity)
    {
        _body.velocity = velocity;
    }

    Rigidbody2D _body;
}
