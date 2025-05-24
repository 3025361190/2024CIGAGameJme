using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
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
    public float recycleSpeed = 25.0f;

    //�ӵ��������ٶ�
    public float normalSpeed = 10.0f;
    //�ӵ��񱩵��ٶ�
    public float furySpeed = 20.0f;

    //���ӵ�ģʽ��true   ���ģʽ��false
    public bool state;

    //���ѳ������ӵ�����
    public Stack<GameObject> nextBullet = new();
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

    //����ʵ��
    public GameObject skillButton;

    //��������
    //public int splitLimit = 1;

    //��β��Ⱦ��
    public TrailRenderer trailRenderer;




    //// ������ȴʱ�䣨�룩
    //public float cooldownTime = 1f;
    //// �´ο���ʹ�ü��ܵ�ʱ��
    //private float nextUseTime = Time.time; 

    //����CD��־
    public bool CDflag = false;
    public GameObject prefabToSpawn;//xiaotude

    public bool ACFlag = false;


    // Start is called before the first frame update

    public void Awake()
    {
        //��ȡmanagerʵ��
        skillButton = GameObject.Find("SkillButton");
        //��ʼ���ٶ�
        speed = normalSpeed;
        //�󶨸���
        rb = GetComponent<Rigidbody2D>();
        //��ʼ���ӵ�״̬
        state = false;
        //��ʼ���ӵ���ɫ�������
        SetColor((ColorType)Random.Range(0, 5));

        Turret =  GameObject.FindGameObjectsWithTag("Turret")[0];
        if(Turret == null)
        {
            Debug.Log("cant find turret");
        }
        trailRenderer.enabled = false;
    }

    void Start()
    {
        

        //Fire(new Vector2(2, -1));
        bullet.transform.localScale = new Vector3(2f, 2f, 1f);


        //���ݳ���״̬��ʼ���ӵ�״̬
        SceneType sceneType = skillButton.GetComponent<SkillButton>().currentSceneType;

        if(sceneType == SceneType.QingTang)
        {
            state = true;
        }
        else
        {
            state = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(ACFlag == true)
        {
            bullet.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        
        //������
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //if (Input.GetKeyDown(KeyCode.V))
        //{

        //}
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            // Debug.Log(newPosition);
            // ʹ�� MovePosition �����ƶ�����
            rb.MovePosition(newPosition);
        }
    }

    //�����ӵ���ɫ
    public void SetColor(ColorType color)
    {
        //Debug.Log(color);
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
        Vector2 temp = Random.insideUnitCircle.normalized;
        rb.velocity = temp * speed;
        // Debug.Log(temp);
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
    public void Recycle()
    {
        is_trace = true;
        speed = recycleSpeed;
        // Debug.Log("���� by bullet");
        // for (int i = 0; i < childNum; i++)
        // {
        //     nextBullet.Pop().GetComponent<BulletController>().Recycle();
        // }
        foreach (var bullet in nextBullet)
        {
            bullet.GetComponent<BulletController>().Recycle();
        }
    }


    public void SetAcFlag()
    {
        SceneType sceneType = skillButton.GetComponent<SkillButton>().currentSceneType;

        if (sceneType == SceneType.QingTang)
        {
            state = true;
        }
        else
        {
            state = false;
        }

        ACFlag = true;
    }








    //��ײ����
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        //Debug.Log(rb.velocity);
        
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
            //Debug.Log(rb.velocity);
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
            //Debug.Log(rb.velocity);
            //Destroy(this);
            //��ֱ�ٶȷ���
            
            //matchdirection();
        }


        //��ײ���˴�������
        else if(collision.gameObject.CompareTag("Enemy") && !state)
        {
            ////�����ӵ������У���ײ��ͬ��ɫ�ĵ��ˣ��ӵ�����
            //if(state && collision.gameObject.GetComponent<Enemy>().enemyColor == bulletCollor) 
            //{
            //    nextBullet[childNum] = Instantiate(bullet, transform.position, transform.rotation);
            //    SetRandomDirection();
            //    //matchdirection();
            //    nextBullet[childNum].GetComponent<BulletController>().SetRandomDirection();
            //    nextBullet[childNum].GetComponent<BulletController>().matchdirection();
            //    nextBullet[childNum].GetComponent<BulletController>().SetColor(bulletCollor);
            //    childNum++;
            //}
            //else if(!state)
            //{
            //    //��ֳ�������ײ����
            //    collision.gameObject.GetComponent<Enemy>().HandleHit(bulletCollor);
            //    Destroy(bullet);
            //}

            collision.gameObject.GetComponent<Enemy>().HandleHit(bulletCollor);
            Destroy(bullet);

        }



        //��ײ��̨��������
        // else if(collision.gameObject.CompareTag("Turret") && is_trace == true)
        // {
        //     //������̨�ӵ�����+1�ĺ���
        //     //............
        //     //............
        //     Turret.GetComponent<PlayerController>().AddBullets(1);

        //     // sprite.enabled = false;
        //     // StartCoroutine(WaitSomeSecondsToDestory(100.0f));
        //     Instantiate(prefabToSpawn, transform.position, transform.rotation);
        //     Destroy(bullet);
            
        // }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Turret") && is_trace == true)
        {
            //������̨�ӵ�����+1�ĺ���
            //............
            //............
            Turret.GetComponent<PlayerController>().AddBullets(1);

            // sprite.enabled = false;
            // StartCoroutine(WaitSomeSecondsToDestory(100.0f));
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
            Destroy(bullet);
            
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && state && collision.gameObject.GetComponent<Enemy>().enemyColor == bulletCollor && !is_trace)
        {

            if(!CDflag)
            {
                // Debug.Log("try to split bullet");
                GameObject temp = Instantiate(bullet, transform.position, transform.rotation);
                temp.transform.localScale = new Vector3(1f, 1f, 1f);
                temp.GetComponent<BulletController>().trailRenderer.enabled = true;
                if (temp == null)
                {
                    Debug.Log("cant split bullet");
                }
                nextBullet.Push(temp);
                SetRandomDirection();
                bullet.transform.localScale = new Vector3(1f, 1f, 1f);
                temp.GetComponent<BulletController>().SetRandomDirection();
                childNum++;

                StartCoroutine(temp.GetComponent<BulletController>().WaitTwoSeconds());
                StartCoroutine(WaitTwoSeconds());
            }
            

            //nextUseTime = Time.time + cooldownTime;
            //temp.GetComponent<BulletController>().nextUseTime = Time.time + cooldownTime;
        }
    }

    // ����Э��
    IEnumerator WaitTwoSeconds()
    {
        CDflag = true;

        //GameObject temp = Instantiate(bullet, transform.position, transform.rotation);
        //nextBullet.Push(temp);
        //SetRandomDirection();
        //temp.GetComponent<BulletController>().SetRandomDirection();
        //childNum++;
        // �ȴ�����
        yield return new WaitForSeconds(1.5f);

        CDflag = false;

    }

    // IEnumerator WaitSomeSecondsToDestory(float waitTime)
    // {
    //     yield return new WaitForSeconds(waitTime);
    //     Destroy(bullet);
    // }



}
