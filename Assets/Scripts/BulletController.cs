using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //�ӵ�Ԥ����
    public GameObject bullet;
    //�ӵ�����Ⱦ��
    public SpriteRenderer sprite;
    //�ӵ��ĸ���
    public Rigidbody2D rb;

    //�ӵ�Ŀǰ���ٶ�
    public float speed;

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


    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Fire(new Vector2(2, -1));
        }
    }

    //�����ӵ���ɫ
    public void SetColor(ColorType color)
    {
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


    //���պ���
    void Recycle(Vector3 positioin)
    {
        
    }





    //��ײ����
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        Debug.Log(rb.velocity);
        
        //ײǽ����
        if (collision.gameObject.CompareTag("AirWall_X"))
        {
            Debug.Log(rb.velocity);
            //ˮƽ�ٶȷ���
            rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
        }
        else if (collision.gameObject.CompareTag("AirWall_Y"))
        {
            Debug.Log(rb.velocity);
            //��ֱ�ٶȷ���
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
        }


        //��ײ���˴�������
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            //�����ӵ������У���ײ��ͬ��ɫ�ĵ��ˣ��ӵ�����
            if(state && collision.gameObject.GetComponent<Enemy>().enemyColor == bulletCollor) 
            {
                nextBullet[childNum] = Instantiate(bullet, transform.position, transform.rotation);
                SetRandomDirection();
                nextBullet[childNum].GetComponent<BulletController>().SetRandomDirection();
                nextBullet[childNum].GetComponent<BulletController>().SetColor(bulletCollor);
                childNum++;
            }
            else if(!state)
            {
                //��ֳ�������ײ����
                collision.gameObject.GetComponent<Enemy>().HandleHit(bulletCollor);
                Destroy(this);
            }
        }
    }


    
}
