using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IWrappable
{
    public int Size { get; set; }

    public float Torque { get; set; }

    public Vector2 Pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    public float WrapMargin { get; set; }

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    public void ApplyTorque(float deltaTime)
    {
        _body.AddTorque(Torque * deltaTime);
    }

    public void SetVelocity(Vector2 velocity)
    {
        _body.velocity = velocity;
    }

    Rigidbody2D _body;
}
