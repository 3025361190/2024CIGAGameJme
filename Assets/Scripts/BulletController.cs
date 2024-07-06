using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class BulletController : MonoBehaviour
{
    //子弹预制体
    public GameObject bullet;
    //子弹的渲染器
    public SpriteRenderer sprite = null;
    //子弹的刚体
    public Rigidbody2D rb;

    //子弹目前的速度
    public float speed;

    //子弹正常的速度
    public float normalSpeed = 10.0f;
    //子弹狂暴的速度
    public float furySpeed = 20.0f;

    //加子弹模式：true   打怪模式：false
    public bool state;

    //分裂出来的子弹数组
    public GameObject[] nextBullet;
    //分裂出来的子弹数量
    public int childNum = 0;

    //子弹颜色素材数组
    public Sprite[] sprites;
    //子弹颜色变量
    public ColorType bulletCollor;

    //image的Transform
    public Transform imageTransform;


    // Start is called before the first frame update
    void Start()
    {
        //初始化速度
        speed = normalSpeed;
        //绑定刚体
        rb = GetComponent<Rigidbody2D>();
        //初始化子弹状态
        state = true;
        //初始化子弹颜色（随机）
        SetColor((ColorType)Random.Range(0, 5));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Fire(new Vector2(2, -1));
        }
    }

    //设置子弹颜色
    public void SetColor(ColorType color)
    {
        Debug.Log(color);
        bulletCollor = color;
        sprite.sprite = sprites[(int)bulletCollor];
    }

    //将子弹速度设置为正常
    void SetNormalSpeed()
    {
        speed = normalSpeed;
    }


    //将子弹速度设置为狂暴
    void SetFurySpeed()
    {
        speed = furySpeed;
    }


    //指定方向发射子弹
    void Fire(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
        matchdirection();
    }


    //随即方向发射子弹
    public void SetRandomDirection()
    {
        this.rb.velocity = Random.insideUnitCircle.normalized * speed;
    }


    //切换子弹状态
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

    // 将二元组映射成360度的角度
    float GetAngleFromVector2(Vector2 vector)
    {
        // 使用Mathf.Atan2计算弧度
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

        // 确保角度在0到360度之间
        if (angle < 0)
        {
            angle += 360;
        }

        return angle;
    }


    //根据速度方向更改子弹朝向
    public void matchdirection()
    {
        Vector2 direction = rb.velocity.normalized;
        float angle = GetAngleFromVector2(direction) + 90;
        imageTransform.rotation = UnityEngine.Quaternion.Euler(0, 0, angle);
    }


    //回收函数
    void Recycle(Vector3 positioin)
    {
        
    }





    //碰撞函数
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        Debug.Log(rb.velocity);
        
        //撞墙反弹
        if (collision.gameObject.CompareTag("AirWall_X"))
        {
            Debug.Log(rb.velocity);
            //水平速度反向
            rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
            matchdirection();
        }
        else if (collision.gameObject.CompareTag("AirWall_Y"))
        {
            Debug.Log(rb.velocity);
            //垂直速度反向
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
            matchdirection();
        }


        //碰撞敌人触发函数
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            //增加子弹场景中，碰撞相同颜色的敌人，子弹分裂
            if(state && collision.gameObject.GetComponent<Enemy>().enemyColor == bulletCollor) 
            {
                nextBullet[childNum] = Instantiate(bullet, transform.position, transform.rotation);
                SetRandomDirection();
                matchdirection();
                nextBullet[childNum].GetComponent<BulletController>().SetRandomDirection();
                nextBullet[childNum].GetComponent<BulletController>().matchdirection();
                nextBullet[childNum].GetComponent<BulletController>().SetColor(bulletCollor);
                childNum++;
            }
            else if(!state)
            {
                //打怪场景的碰撞操作
                collision.gameObject.GetComponent<Enemy>().HandleHit(bulletCollor);
                Destroy(this);
            }
        }
    }


    
}
