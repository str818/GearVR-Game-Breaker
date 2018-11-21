using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// 碰撞检测的方法
public class CollisionEffect : MonoBehaviour {

	public AudioSource audio_hitcube;		// 打击在Cube上的音效
	public AudioSource audio_hitzhui;		// 打击在棱锥上的音效
	public AudioSource audio_hitglass;		// 打击在玻璃板上的音效
	public AudioSource audio_hitswitch;		// 打击在开关按钮上的音效
	public GameObject threePre;			// 分数3的预制体
	public GameObject help;				// 获取help
	public GameObject plusPanel;			// 获取plusPanel
	private bool isHitZhui = false;		// 是否打击了棱锥上的标志位
	private bool isHitGlass = false;		// 是否打击了玻璃上的标志位
	private bool isHitSwitch = false;		// 是否打击了开关上的标志位
	private bool isHitSwitch2 = false;		// 是否打击了开关上的标志位
    private bool isHitSwitch3 = false;     // 是否打击了开关上的标志位

	// 小球碰撞检测的方法
	void OnCollisionEnter(Collision collisionInfo) {
		
		string name = collisionInfo.gameObject.name;		// 得到碰撞物体的名字
		if (name.Equals ("Cube")) {							// 如果打击在Cube上
			audio_hitcube.Play ();							// 播放打击Cube的音效
		} else if (name.Substring (0, 4).Equals ("zhui") && !isHitZhui) {	// 如果打击在棱锥上 
            audio_hitzhui.Play();							// 播放打击棱锥的音效
			collisionInfo.transform.gameObject.GetComponent<ZhuiDie>().enabled = true;
			collisionInfo.transform.gameObject.GetComponent<Renderer> ().GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color (1.0f, 1.0f, 1.0f, 0.8f));	// 改变棱锥颜色
			Vector3 position = collisionInfo.transform.position;	// 得到被撞棱锥的位置
			if (name.Length < 5) {							// 如果打击在非克隆棱锥上
                Rigidbody threeRi = ((GameObject)(Instantiate (threePre, new Vector3 (position.x, position.y + 2.5f, position.z), threePre.transform.rotation))).GetComponent<Rigidbody> ();	// 克隆分数3
				threeRi.GetComponent<MeshRenderer> ().enabled = true;	// 设置MeshRenderer属性为true
				if(UIdata.isGamePlaying)
                {
                    UIdata.sco += 3;							// 更新分数
                    plusPanel.SetActive(true);					// 提示加3 
                }
				
				if (!UIdata.isHitedZhui && UIdata.targetScene==1) {			// 如果没有打碎过棱锥并且位于第一关
					help.SetActive (true);					// 提示信息
                    UIdata.isHitedZhui = true;						// 是否打碎过棱锥置为true
				}
			}
			isHitZhui = true;								// 是否打击了棱锥上的标志位设置为true
		} else if (name.Substring (0, 4).Equals ("Glas") && !isHitGlass) {		// 如果打击在玻璃上
			audio_hitglass.Play ();							// 播放打击玻璃的音效
			isHitGlass = true;								// 是否打击了玻璃上的标志位设置为true
		} else if (name.Equals ("Switch") && !isHitSwitch) {		// 如果打击在第一种开关上
			audio_hitswitch.Play ();						// 播放打击开关的音效
			collisionInfo.transform.gameObject.GetComponent<DoorSwitch>().enabled = true;	// 开关被触发
			collisionInfo.transform.FindChild("Cube").gameObject.SetActive(false);			// 隐藏Cube提示
			isHitSwitch = true;								// 是否打击了开关上的标志位设置为true
		} else if (name.Equals ("Switch2") && !isHitSwitch2) {		// 如果打击在第二种开关上
			audio_hitswitch.Play ();						// 播放打击开关的音效
			collisionInfo.transform.gameObject.GetComponent<DoorSwitch2>().enabled = true;	// 开关被触发
			isHitSwitch2 = true;							// 是否打击了开关上的标志位设置为true
		}else if(name.Equals("Switch3")&&!isHitSwitch3){
            audio_hitswitch.Play();						// 播放打击开关的音效
            collisionInfo.transform.gameObject.GetComponent<DoorSwitch2>().enabled = true;	// 开关被触发
            isHitSwitch3 = true;	
        }

	}
}
