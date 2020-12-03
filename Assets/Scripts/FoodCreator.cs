using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodCreator : MonoBehaviour
{

    private static FoodCreator instance;
    public static FoodCreator Instance
    {
        get
        {
            return instance;
        }
    }
    public int xMinLimit = 11;
    public int xMaxLimit = 20;
    public int yMinLimit = 11;
    public int yMaxLimit = 11;
    //要和蛇头移动步长一致
    public int step = 30;
    public GameObject foodPrefabs;
    //设置
    public GameObject rewardPrefabs;
    //保存食物的Sprites
    public Sprite[] foodSprites;
    private Transform foodHolder;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        foodHolder = GameObject.FindGameObjectWithTag("FoodRoot").transform;
        CreateFood(false);
    }

    public void CreateFood(bool isReward)
    {
        //随机取的一个食物的下标
        int index = Random.Range(0, foodSprites.Length);
        //实例化food预制体
        GameObject food = Instantiate(foodPrefabs);
        //将食物的image source改为选中下标的食物
        food.GetComponent<Image>().sprite = foodSprites[index];
        //将food设置为foodHolder的子物体，false会使得food保持局部坐标不变
        food.transform.SetParent(foodHolder, false);
        //随机取得食物生成位置
        int x = Random.Range(-xMinLimit, xMaxLimit + 1);
        int y = Random.Range(-yMinLimit, yMaxLimit + 1);
        food.transform.localPosition = new Vector3(x * step, y * step, 0);
        //判断是否生成奖励
        if (isReward)
        {
            //同理
            GameObject reward = Instantiate(rewardPrefabs);
            reward.transform.SetParent(foodHolder, false);
            x = Random.Range(-xMinLimit, xMaxLimit);
            y = Random.Range(-yMinLimit, yMaxLimit);
            reward.transform.localPosition = new Vector3(x * step, y * step, 0);
        }
    }
}
