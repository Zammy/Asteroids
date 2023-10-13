using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Life;

    public IAsteroidService AsteroidService { get; set; }

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 velocity)
    {
        _body.velocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        var asteroid = col.collider.GetComponent<Asteroid>();
        if (asteroid)
        {
            AsteroidService.AsteroidHit(asteroid, transform.up);
            Life = -100f;
        }
    }

    Rigidbody2D _body;
}
