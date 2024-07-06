using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class BulletController : MonoBehaviour
{
    //�ӵ�Ԥ����
    public GameObject bullet;
    //�ӵ�����Ⱦ��
    public SpriteRenderer sprite = null;
    //�ӵ��ĸ���
    public Rigidbody2D rb;

    //�ӵ�Ŀǰ���ٶ�
    public float speed;

    //�ӵ����յ��ٶ�
    public float recycleSpeed = 15.0f;

    //�ӵ��������ٶ�
    public float normalSpeed = 10.0f;
    //�ӵ��񱩵��ٶ�
    public float furySpeed = 20.0f;

    //���ӵ�ģʽ��true   ���ģʽ��false
    public bool state;

    //���ѳ������ӵ�����
    public GameObject[] nextBullet;
    //���ѳ������ӵ�����
    public int childNum = 0;

    //�ӵ���ɫ�ز�����
    public Sprite[] sprites;
    //�ӵ���ɫ����
    public ColorType bulletCollor;

    //image��Transform
    public Transform imageTransform;

    //��̨ʵ�������ڻ����ӵ�ʱ׷��
    public GameObject Turret;

    //׷��״̬��־
    public bool is_trace = false;


    // Start is called before the first frame update

    public void Awake()
    {
        //��ʼ���ٶ�
        speed = normalSpeed;
        //�󶨸���
        rb = GetComponent<Rigidbody2D>();
        //��ʼ���ӵ�״̬
        state = true;
        //��ʼ���ӵ���ɫ�������
        SetColor((ColorType)Random.Range(0, 5));
    }

    void Start()
    {
        

        //Fire(new Vector2(2, -1));


        //���ݳ���״̬��ʼ���ӵ�״̬
        //..............
        //..............
    }

    // Update is called once per frame
    void Update()
    {
        
        //ʵʱ�����ӵ�����
        matchdirection();
    }
    public void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    Fire(new Vector2(2, -1));
        //}
        if (is_trace)
        {
            // �����µ�λ��
            Vector3 newPosition = Vector3.MoveTowards(rb.position, Turret.transform.position, recycleSpeed * Time.fixedDeltaTime);

            // ʹ�� MovePosition �����ƶ�����
            rb.MovePosition(newPosition);
        }
    }

    //�����ӵ���ɫ
    public void SetColor(ColorType color)
    {
        Debug.Log(color);
        bulletCollor = color;
        sprite.sprite = sprites[(int)bulletCollor];
    }

    //���ӵ��ٶ�����Ϊ����
    void SetNormalSpeed()
    {
        speed = normalSpeed;
    }


    //���ӵ��ٶ�����Ϊ��
    void SetFurySpeed()
    {
        speed = furySpeed;
    }


    //ָ���������ӵ�
    void Fire(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
        //matchdirection();
    }


    //�漴�������ӵ�
    public void SetRandomDirection()
    {
        this.rb.velocity = Random.insideUnitCircle.normalized * speed;
    }


    //�л��ӵ�״̬
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

    // ����Ԫ��ӳ���360�ȵĽǶ�
    float GetAngleFromVector2(Vector2 vector)
    {
        // ʹ��Mathf.Atan2���㻡��
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

        // ȷ���Ƕ���0��360��֮��
        if (angle < 0)
        {
            angle += 360;
        }

        return angle;
    }


    //�����ٶȷ�������ӵ�����
    public void matchdirection()
    {
        Vector2 direction = rb.velocity.normalized;
        float angle = GetAngleFromVector2(direction) + 90;
        imageTransform.rotation = UnityEngine.Quaternion.Euler(0, 0, angle);
    }


    //���պ���
    void Recycle(Vector3 positioin)
    {
        for (int i = 0; i < childNum; i++)
        {
            nextBullet[i].GetComponent<BulletController>().Recycle(positioin);
        }

        is_trace = true;
    }





    //��ײ����
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        Debug.Log(rb.velocity);
        
        //ײǽ����
        if (collision.gameObject.CompareTag("AirWall_X"))
        {
            if(state)
            {
                rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
            }
            else
            {
                Destroy(bullet);
            }
            Debug.Log(rb.velocity);
            //Destroy(this);
            //ˮƽ�ٶȷ���
            
            //matchdirection();
        }
        else if (collision.gameObject.CompareTag("AirWall_Y"))
        {

            if (state)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
            }
            else
            {
                Destroy(bullet);
            }
            Debug.Log(rb.velocity);
            //Destroy(this);
            //��ֱ�ٶȷ���
            
            //matchdirection();
        }


        //��ײ���˴�������
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            //�����ӵ������У���ײ��ͬ��ɫ�ĵ��ˣ��ӵ�����
            if(state && collision.gameObject.GetComponent<Enemy>().enemyColor == bulletCollor) 
            {
                nextBullet[childNum] = Instantiate(bullet, transform.position, transform.rotation);
                SetRandomDirection();
                //matchdirection();
                nextBullet[childNum].GetComponent<BulletController>().SetRandomDirection();
                nextBullet[childNum].GetComponent<BulletController>().matchdirection();
                nextBullet[childNum].GetComponent<BulletController>().SetColor(bulletCollor);
                childNum++;
            }
            else if(!state)
            {
                //��ֳ�������ײ����
                collision.gameObject.GetComponent<Enemy>().HandleHit(bulletCollor);
                Destroy(bullet);
            }
        }



        //��ײ��̨��������
        else if(collision.gameObject.CompareTag("Turret") && is_trace == true)
        {
            //������̨�ӵ�����+1�ĺ���
            //............
            //............
            Destroy(bullet);
        }
    }


    
}
