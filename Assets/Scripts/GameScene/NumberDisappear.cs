using UnityEngine;
using System.Collections;

// 用于提示的九个数字的消失类
public class NumberDisappear : MonoBehaviour {

	void Update () {
		if (transform.GetComponent<Animator> ().GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {	// 如果动画播放完成1遍
			transform.gameObject.SetActive (false);		// 消失
		}
	}



}
