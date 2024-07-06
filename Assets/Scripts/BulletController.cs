using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public GameObject bullet;
    public Rigidbody2D rb;

    public float speed;

    public float normalSpeed = 10.0f;
    public float furySpeed = 20.0f;

    //加子弹模式：true   打怪模式：false
    public bool state;

    public GameObject[] nextBullet;
    public int childNum = 0;


    // Start is called before the first frame update
    void Start()
    {
        speed = normalSpeed;
        rb = GetComponent<Rigidbody2D>();
        state = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Fire(new Vector2(2, -1));
        }
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
        rb.velocity = direction.normalized * speed;
    }

    void SwitchState()
    {
        if(state) 
        {
            state = false; 
        }
        else
        {
            state = true;
        }
    }

    public void SetRandomDirection()
    {
        this.rb.velocity = Random.insideUnitCircle.normalized * speed;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        Debug.Log(rb.velocity);
        if (collision.gameObject.CompareTag("AirWall_X"))
        {
            Debug.Log(rb.velocity);
            rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
        }
        else if (collision.gameObject.CompareTag("AirWall_Y"))
        {
            Debug.Log(rb.velocity);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {

            if(state) 
            {
                nextBullet[childNum] = Instantiate(bullet, transform.position, transform.rotation);
                SetRandomDirection();
                nextBullet[childNum].GetComponent<BulletController>().SetRandomDirection();
                childNum++;
            }
            else
            {
                //打怪场景的碰撞操作

            }
        }
    }


    
}
