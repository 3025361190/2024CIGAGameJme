using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public GameObject bullet;
    public Rigidbody2D bullet_rb;

    public float speed;

    public float normalSpeed = 10.0f;
    public float furySpeed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        speed = normalSpeed;
        bullet_rb = bullet.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetNormalSpeed()
    {
        speed = normalSpeed;
    }

    void SetFurySpeed()
    {
        speed = furySpeed;
    }


    void Fire(Vector2 direction)
    {
        bullet_rb.velocity = direction.normalized * speed;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Airwall"))
        {
            
        }
    }
}
