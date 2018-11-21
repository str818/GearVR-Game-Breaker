using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCheck : MonoBehaviour {

	public GameObject N1;					// 数字精灵1
	public GameObject N2;					// 数字精灵2
    public GameObject countDownPanel;      //倒计数面板
	private float[] NumX = {0.0f, 0.21f, 0.5f, 0.75f, 0.0f, 0.24f, 0.49f, 0.74f, 0.0f, 0.25f};		// 数字数组
	private float[] NumY = {0.78f, 0.78f, 0.78f, 0.78f, 0.53f, 0.53f, 0.53f, 0.53f, 0.28f, 0.28f};		// 数字数组
    private int temp = -1;//记录小球之前分数

	void Update() {
		if (UIdata.sco >= 0) {						// 如果分数大于等于0
			showScore(UIdata.sco);                  	// 绘制计分板
            
            if(UIdata.sco<=5 && UIdata.sco!=temp)
            {
                showCountDown(UIdata.sco);                    //显示倒计数面板
                temp = UIdata.sco; //记录当前分数
            }
		}
        if (countDownPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {	// 如果动画播放完成1遍
            countDownPanel.SetActive(false);		// 消失
            countDownPanel.transform.position = new Vector3(0f, 0f, 8.91f);
        }
	}

	void showScore(int num) {                       // 绘制成绩方法

		// Debug.Log ("当前小球数：" + num);
		int n1;                                     // 十位数字
		int n2;                                     // 个位数字
		if (num >= 100) {                           // 大于一百，默认成绩为99
			n1 = 9;
			n2 = 9;
		} else {                                    // 整除10，获取十位数数字
			n1 = num / 10;                          // 对10取余，获取个位数字
			n2 = num % 10;
		}
		if (n1 == 0) {                              // 如果十位是零，不对十位进行绘制
			N1.SetActive(false);
		} else {
			N1.SetActive(true);
			N1.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2(NumX[n1], NumY[n1]);
		}
		if (n1 == 0 && n2 == 0) {                   // 十位和个位都是零，都不绘制
			N2.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2(NumX[n2], NumY[n2]);
		} else {
			N2.SetActive(true);
			N2.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2(NumX[n2], NumY[n2]);
		}
	}

    void showCountDown(int num) //显示倒计数面板
    {
        countDownPanel.SetActive(true);
        countDownPanel.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(NumX[num], NumY[num]);
        countDownPanel.GetComponent<Animator>().Play("CountDown");
        //StartCoroutine(WaitAndShow(1.0F));
    }
}
