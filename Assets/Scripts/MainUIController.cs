using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    private static MainUIController instance;
    public static MainUIController Instance
    {
        get
        {
            return instance;
        }
    }
    public int score = 0;
    public int length = 0;
    public Text msgText;
    public Text scoreText;
    public Text lengthText;
    public Image bgImage;
    public Button pauseBtn;
    public Sprite[] pauseSprites;
    private Color tempColor;
    public bool hasBorder;
    public bool isPause = false;

    private void Awake()
    {
        Time.timeScale = 1;
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        //判断是否设置了边界
        if (PlayerPrefs.GetInt("border", 0) == 0)
        {
            //hasBorder标志位用于判断蛇头碰到边界是否死亡
            hasBorder = false;
            //遍历bgImage父物体下所有子物体
            foreach (Transform item in bgImage.gameObject.transform)
            {
                //在无边界模式下，禁用掉边界
                item.gameObject.GetComponent<Image>().enabled = false;
            }
        }
        else
        {
            hasBorder = true;
        }
    }

    private void Update()
    {
        switch (score / 100)
        {
            case 0:
                break;
            case 1:
                msgText.text = "阶段二";
                break;
            case 2:
                msgText.text = "阶段三";
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#CCEEFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段四";
                break;
            case 4:
            case 5:
                msgText.text = "阶段五";
                break;
            case 6:
                ColorUtility.TryParseHtmlString("#CCFFDB", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段六";
                break;
            case 7:
            case 8:
                msgText.text = "阶段七";
                break;
            case 9:
                ColorUtility.TryParseHtmlString("#FFDACC", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段八";
                break;
        }
    }

    public void UpdateUI(int s = 5, int l = 1)
    {
        score += s;
        length += l;
        scoreText.text = "得分：\n" + score;
        lengthText.text = "长度：\n" + length;
    }

    public void Pause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
            //按钮图片的切换
            pauseBtn.GetComponent<Image>().sprite = pauseSprites[1];
        }
        else
        {
            Time.timeScale = 1;
            pauseBtn.GetComponent<Image>().sprite = pauseSprites[0];
        }
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("StartScene");
    }
}