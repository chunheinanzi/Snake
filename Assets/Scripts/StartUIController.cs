using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour
{
    public Text lastText;
    public Text bestText;
    public Toggle blue;
    public Toggle yellow;
    public Toggle border;
    public Toggle noBorder;

    private void Start()
    {
        Screen.SetResolution(1280, 720,false);
        lastText.text = "上次：长度"+PlayerPrefs.GetInt("lastLength",0)+",分数" + PlayerPrefs.GetInt("lastScore", 0);
        bestText.text = "最好：长度" + PlayerPrefs.GetInt("bestLength", 0) + ",分数" + PlayerPrefs.GetInt("bestScore", 0);
        //GetString如果sh已经有值，则判断是否为样式一小蛇，如果用户还没有设置，则默认为样式一
        if (PlayerPrefs.GetString("sh", "sb01") == "sb01")
        {
            blue.isOn = true;
            BlueSelected(true);
        }
        else
        {
            yellow.isOn = true;
            YellowSelected(true);
        }
        //有无边界的判断如上
        if (PlayerPrefs.GetInt("border", 0) == 1)
        {
            border.isOn = true;
            BorderSelected(true);
        }
        else
        {
            noBorder.isOn = true;
            NoBorderSelected(true);
        }
    }

    //选择了样式一小蛇
    public void BlueSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
            //如果选择该样式，修改图片颜色，用于区别
            blue.GetComponentInChildren<Image>().color = new Color32(1, 159, 232, 90);
        }
        else
        {
            blue.GetComponentInChildren<Image>().color = new Color32(1, 159, 232, 50);
        }
    }

    //选择了样式二小蛇
    public void YellowSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
            yellow.GetComponentInChildren<Image>().color = new Color32(1, 159, 232, 90);
        }
        else
        {
            yellow.GetComponentInChildren<Image>().color = new Color32(1, 159, 232, 50);
        }
    }

    //选择有边界模式
    public void BorderSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 1);
        }
    }

    //选择无边界模式
    public void NoBorderSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 0);
        }
    }

    //开始游戏
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
