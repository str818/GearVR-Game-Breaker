using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public GameObject CenterPanel;		// 获取CenterPanel
	public GameObject over;
    public GameObject victoryPanel;   //胜利面板

	void Update () {
		if (UIdata.sco == 0 && UIdata.isGamePlaying) {			// 如果小球数为0了
            UIdata.isGamePlaying = false; 
            over.SetActive(true);
			gameOver();					// 调用结束方法
		}
	}

	// 游戏结束的方法
	public void gameOver () {
		StartCoroutine(waitOver(2.5f));
        UIdata.isDead = true;        //死亡
	}

	IEnumerator waitOver (float seconds) {
		yield return new WaitForSeconds (seconds);
		//if (!UIdata.isDead && UIdata.sco == 0) {
			//UIdata.isDead = true;						// 死亡
			CenterPanel.SetActive (true);					// 打开中央画布
			CenterPanel.transform.FindChild("btgoon").gameObject.SetActive(false);	// 不绘制继续游戏按钮
            victoryPanel.SetActive(false);  //关闭胜利面板
			this.enabled = false;
		//}
	}

    //游戏胜利的方法
    public void gameWin()
    {
        victoryPanel.SetActive(true); //激活胜利面板
        StartCoroutine(waitOver(4f));//胜利
    }



}
