using UnityEngine;
using System.Collections;

public class PlusDisappear : MonoBehaviour {

	void Update () {
		if (transform.GetComponent<Animator> ().GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {		// 如果动画播放完成一次
			transform.gameObject.SetActive (false);													// 消失
		}
	}

}
