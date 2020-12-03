using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


public class SnakeHead : MonoBehaviour
{
    public List<Transform> bodyList = new List<Transform>();
    public float velocity = 0.35f;
    //每一步蛇头移动距离
    public int step;
    //x轴蛇头移动增量
    private int x;
    //y轴蛇头移动增量
    private int y;
    public Transform snackRoot;
    public AudioClip eatClip;
    public AudioSource audioSource;
    public AudioClip dieClip;
    public GameObject bodyPrefab;
    public GameObject dieEffect;
    public Sprite[] bodySprites = new Sprite[2];

    private Vector3 headPos;
    private bool isDie;

 

    private void Awake()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh01"));
        bodySprites[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0101"));
        bodySprites[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0102"));
    }

    private void Start()
    {
        //初始化，让蛇头可以向上移动
        x = 0;
        y = step;
        //InvokeRepeating等待0秒，然后每隔velocity时间调用Move方法
        InvokeRepeating("Move", 0, velocity);
        Debug.Log(this.name + " start");
       
        
       
    }

    private void Update()
    {
        //如果游戏暂停或者蛇已经死亡，就不再让蛇移动
        if (MainUIController.Instance.isPause == true || isDie == true)
        {
            return;
        }
        //虚拟轴控制移动
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Input.GetKeyDown键按下瞬间
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CancelInvoke先取消之前的InvokeRepeating命令
            CancelInvoke();
            //将间隔调用“Move”方法的时间减小，则蛇移动变快
            InvokeRepeating("Move", 0, velocity - 0.2f);
        }
        //Input.GetKeyUp键抬起瞬间
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity);
        }
        //如果此时y = -step说明蛇正在向下移动，为了防止蛇目前在向下移动，突然向上移动，加y != -step判断，以下同理
        if (v > 0 && y != -step)
        {
            //设置当头上下左右移动的时候，蛇头的方向和移动方向一致，以下同理
            //Quaternion代表四元数，identity表示初始旋转角度，可理解为new Vector(0,0,0)
            gameObject.transform.localRotation = Quaternion.identity;
            //设置蛇的移动方向，x = 0,y = step说明蛇头在Y轴向上移动，以下同理
            x = 0;
            y = step;
        }
        if (v < 0 && y != step)
        {
            //Quaternion.Euler将欧拉角转化为四元数，需要注意欧拉角要与移动方向匹配
            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            x = 0;
            y = -step;
        }
        if (h < 0 && x != step)
        {
            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            x = -step;
            y = 0;
        }
        if (h > 0 && x != -step)
        {
            gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            x = step;
            y = 0;
        }
    }

    void Move()
    {
        //获取当前蛇头移动的局部坐标
        headPos = gameObject.transform.localPosition;
        //将蛇头当前的移动位置加上x轴和y轴的移动增量，实现蛇头的移动
        gameObject.transform.localPosition = new Vector3(headPos.x + x, headPos.y + y, 0);

        //刚开始bodyList为空，防止报空指针
        if (bodyList.Count > 0)
        {
            for (int i = bodyList.Count - 2; i >= 0; i--)
            {
                //将前一节蛇尾的位置赋予后一节
                bodyList[i + 1].localPosition = bodyList[i].localPosition;
            }
            //将原来蛇头的位置赋予给下标为0的蛇尾，也就是蛇头后一节的蛇尾
            bodyList[0].localPosition = headPos;
        }

        //方法二：将蛇尾最后一节移至蛇头的位置
        //if (bodyList.Count > 0)
        //{
        //bodyList.Last()获取list最后的元素
        //    bodyList.Last().localPosition = headPos;
        //Insert将元素插入到指定位置
        //    bodyList.Insert(0, bodyList.Last());
        //RemoveAt移除指定下标的元素
        //    bodyList.RemoveAt(bodyList.Count - 1);
        //}
    }

    void AddBody()
    {
        audioSource.clip = eatClip;
        audioSource.Play();
        //三元运算符，如果bodyList.Count被2模除则返回0，否则返回1，控制身体奇偶数轮换颜色
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;
        //new Vector3(2000, 2000, 0)先将身体实例化在屏幕外
        GameObject newBody = Instantiate(bodyPrefab, new Vector3(2000, 2000, 0), Quaternion.identity);
        newBody.GetComponent<Image>().sprite = bodySprites[index];
        newBody.transform.SetParent(snackRoot, false);
        //将新生成的蛇身加入到bodyList中
        bodyList.Add(newBody.transform);
    }

    void Die()
    {
        audioSource.clip = dieClip;
        audioSource.Play();
        //取消蛇移动的InvokeRepeating调用
        CancelInvoke();
        isDie = true;
        //实例化死亡效果
        Instantiate(dieEffect, transform.position - Vector3.forward, Quaternion.identity);
        //数据持久化PlayerPrefs以键值对的形式保存数据，可以保存int、string、float三种数据类型，类似字典
        //Set用于保存数据，Get用于得到数据
        PlayerPrefs.SetInt("lastLength", MainUIController.Instance.length);
        PlayerPrefs.SetInt("lastScore", MainUIController.Instance.score);
        //如果当前分数高于最高分数，就将最高数据进行更换
        //GetInt("bestScore", 0)如果当前有bestScore值的话，则返回该值，如果没有，则返回0
        if (PlayerPrefs.GetInt("bestScore", 0) < MainUIController.Instance.score)
        {
            PlayerPrefs.SetInt("bestLength", MainUIController.Instance.length);
            PlayerPrefs.SetInt("bestScore", MainUIController.Instance.score);
        }
        //开启协成，调用GameOver方法
        StartCoroutine(GameOver(1.5f));
    }

    IEnumerator GameOver(float t)
    {
        //停顿t秒执行下面语句
        yield return new WaitForSeconds(t);
        //重新加载到开始场景
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI();
            AddBody();
            //(Random.Range(0, 100) < 20) ? true : false 三元运算符 随机值小于20则返回true，否则false
            FoodCreator.Instance.CreateFood((UnityEngine.Random.Range(0, 100) < 20) ? true : false);
        }
        else if (collision.gameObject.CompareTag("Reward"))
        {
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI(UnityEngine.Random.Range(5, 15) * 10);
            AddBody();
        }
        else if (collision.gameObject.CompareTag("Body"))
        {
            Die();
        }
        else
        {
            if (MainUIController.Instance.hasBorder)
            {
                Die();
            }
            else
            {
                //判断碰撞到物体的名字
                switch (collision.gameObject.name)
                {
                    case "Up":
                        //-transform.localPosition.y + 30 需要注意这里添加一个偏差值，防止蛇头刚移动到底部就碰到底部的碰撞器，下面同理
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "Down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                        break;
                    case "Left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 210, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "Right":
                        transform.localPosition = new Vector3(-375, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
        }
    }
}
