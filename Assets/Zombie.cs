using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed;
    //攻击力
    public int atk;
    public GameObject currentPlant;
    public float timer;
    public float cd;
    //血量
    public int maxHp;
    public int currentHp;
    public GameObject zombieHead;
    public bool isHaveHead;
    
    // 死亡相关
    private bool isDead = false;
    public float dieAnimationTime = 1.5f; // 死亡动画时长，根据实际动画调整

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        isHaveHead = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 如果已经死亡，不执行后续逻辑
        if (isDead) return;

        GetComponent<Animator>().SetFloat("Hp", 1.0f * currentHp / maxHp);
        if (1.0f * currentHp / maxHp <= 0.2f && isHaveHead == true)
        {
            //掉脑袋
            GameObject obj = GameObject.Instantiate(zombieHead);
            obj.transform.position = transform.position + new Vector3(0.88f, -0.35f, 0);
            Destroy(obj, 0.9f);
            isHaveHead = false;
        }
        transform.Translate(speed * Time.deltaTime * Vector3.left);
        if (currentPlant != null)
        {
            timer += Time.deltaTime;
            if (timer >= cd)
            {
                currentPlant.GetComponent<PlantHP>().hp -= atk;
                timer = 0;
            }
        }
        else
        {
            speed = 0.5f;
            GetComponent<Animator>().SetBool("IsEat", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果已经死亡，不处理碰撞
        if (isDead) return;

        if (collision.tag == "plant")
        {
            speed = 0f;
            //执行吃的动画
            GetComponent<Animator>().SetBool("IsEat", true);
            currentPlant = collision.gameObject;
        }
        else if (collision.tag == "peabullet")
        {
            currentHp -= collision.GetComponent<Bullet>().atk;
            Destroy(collision.gameObject);
            if (currentHp <= 0)
            {
                Die();
            }
        }
    }

    // 死亡处理
    void Die()
    {
        // 双重保护：已经死亡就直接返回，防止重复触发
        if (isDead) return;
        isDead = true;
        
        // 停止移动
        speed = 0f;
        
        // 停止吃植物动画
        GetComponent<Animator>().SetBool("IsEat", false);
        
        // 显式设置Hp为0，确保Animator收到死亡状态
        // （因为OnTriggerEnter2D和Update执行顺序不确定，保险起见手动设置）
        GetComponent<Animator>().SetFloat("Hp", 0f);
        
        // 如果你的Animator用的是IsDead布尔参数，取消下面这行注释
        // GetComponent<Animator>().SetBool("IsDead", true);
        
        // 如果你的Animator用的是Die触发器参数，取消下面这行注释
        // GetComponent<Animator>().SetTrigger("Die");
        
        // 禁用碰撞体，防止死亡过程中继续被子弹击中导致重复触发
        GetComponent<Collider2D>().enabled = false;
        
        // 延迟销毁，等待死亡动画播放完
        Invoke("DestroyZombie", dieAnimationTime);
    }

    void DestroyZombie()
    {
        Destroy(gameObject);
    }
}


