using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float _speed = 100f;
    private float _timeToDestroy = 3f;
    public Rigidbody rb;

    private void OnEnable()
    {
        Destroy(gameObject, _timeToDestroy); 
    }

    private void Update()
    {
        rb.velocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);
        Destroy(gameObject);
    }
}
