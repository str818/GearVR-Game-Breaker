using UnityEngine;
using System.Collections;

public class MenuListener : MonoBehaviour {

	public GameObject[] buttons = new GameObject[10];	// 按钮数组
	public Transform centerEye;							// 摄像机中心位置
	public int Bit = -1;								// 哪一个按钮的标志

    public AudioSource music;                         	//音乐
    public AudioSource sound;                         	//音效

    public GameObject Panel;                         	// 选项面板
    public GameObject musicPanel;                       //音乐面板
    public GameObject soundPanel;                       //音效面板

    public GameObject guankaOnePanel;                   //关卡一面板
    public GameObject guankaTwoPanel;                   //关卡二面板

    public GameObject TipPanel;                     //提示面板

    public Material[] soundTexture;            			//音效纹理数组
    public Material[] musicTexture;            			//音乐纹理数组

	void Start(){
		UIdata.isHaveCamera = false; 
		UIdata.whitchScene = 0; //更新场景标志
        sound.volume = UIdata.soundVolume * UIdata.volumes[1];//调节按钮音效音量
        music.volume = UIdata.musicVolume * UIdata.volumes[1];//调节按钮音效音量
        soundPanel.GetComponent<MeshRenderer>().material = soundTexture[UIdata.soundIndex];//更改纹理图片
        musicPanel.GetComponent<MeshRenderer>().material = musicTexture[UIdata.musicIndex];//更改纹理图片
	}

	void Update () {
		Ray ray = new Ray (centerEye.position, centerEye.forward);				// 摄像机视野正前方的射线
		RaycastHit hit = new RaycastHit ();										// 实例化RaycastHit
		Bit = -1;
		for (int i = 0; i < buttons.Length; i++) {
			if (buttons [i].GetComponent<BoxCollider> ().Raycast (ray, out hit, 50.0f)) {	// 如果射线与按钮发生了碰撞
				if (buttons [i].transform.FindChild ("gl") != null) {
					buttons [i].transform.FindChild ("gl").gameObject.SetActive (true);
				} else if(hit.transform.name.Substring(0,1).Equals("x")){ //如果是选项或帮助按钮
					buttons [i].GetComponent<Renderer> ().GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color (0f, 0f, 0f, 1.0f));	// 改变颜色
                }else
                {
                    buttons[i].GetComponent<Renderer>().GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0.5f, 0.5f, 0.75f, 1.0f));
                }
                
				Bit = i;														// 更新标志
				UIdata.isTouchButton = true;									// 已经点击在按钮上设为true
			} else {
				if (buttons [i].transform.FindChild ("gl") != null) {
					buttons [i].transform.FindChild ("gl").gameObject.SetActive (false);
				} else {
					buttons [i].GetComponent<Renderer> ().GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color (1.0f, 1.0f, 1.0f, 1.0f));	// 改变颜色
				}
			}
		}
		if (Bit == -1) {
			UIdata.isTouchButton = false;										// 已经点击在按钮上设为true
		}
	}

    //从第一关开始
	public void start () {
        UIdata.setFirst();       //确定目标关卡为第一关
		UIdata.init();
        UIdata.sco = 25;   //初始化关卡分数
		Application.LoadLevel (1);
		//UIdata.init();
	}

    //从第二关开始
    public void startTwo() {
        UIdata.setSecond();         //确定目标关卡为第二关
        UIdata.sco = 10;   //初始化关卡分数
        UIdata.init();
        Application.LoadLevel(1);
    }


	public void soundSet () {
		Panel.SetActive (true);//显示选项面板
	}

    public void soundHide(){
        Panel.SetActive(false);//隐藏选项面板
    }

    //点击音效按钮
    public void soundHit(){
        UIdata.soundIndex = (UIdata.soundIndex + 1) % 4;       //增加音效等级
        UIdata.soundVolume = 0.1f * UIdata.soundIndex;//更改音效大小
        sound.volume = UIdata.soundVolume * UIdata.volumes[1];//调节按钮音效音量
        soundPanel.GetComponent<MeshRenderer>().material = soundTexture[UIdata.soundIndex];//更改纹理图片
    }

    //点击音乐按钮
    public void musicHit()
    {
        UIdata.musicIndex = (UIdata.musicIndex + 1) % 4;   //增加音乐等级
        UIdata.musicVolume = 0.1f * UIdata.musicIndex;//更改音效大小
        music.volume = UIdata.musicVolume * UIdata.volumes[1];//调节按钮音效音量
        musicPanel.GetComponent<MeshRenderer>().material = musicTexture[UIdata.musicIndex];//更改纹理图片
    }

    //显示提示面板
    public void showTip()
    {
        TipPanel.SetActive(true);//显示提示面板
    }

    //隐藏提示面板
    public void hideTip()
    {
        TipPanel.SetActive(false);//隐藏提示面板
    }

    //显示关卡面板一
    public void showGuankaOne()
    {
        guankaOnePanel.SetActive(true);
    }

    //隐藏关卡面板一
    public void hideGuankaOne()
    {
        guankaOnePanel.SetActive(false);
    }

    //显示关卡面板二
    public void showGuankaTwo()
    {
        guankaTwoPanel.SetActive(true);
    }

    //隐藏关卡面板二
    public void hideGuankaTwo()
    {
        guankaTwoPanel.SetActive(false);
    }
}
