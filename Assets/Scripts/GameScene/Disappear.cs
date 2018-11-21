using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// 控制下方提示信息消失的类
public class Disappear : MonoBehaviour {

	public float seconds;					// 等待时间

	void Start () {
		StartCoroutine (wait (seconds));	// 时间之后消失
	}

	IEnumerator wait (float seconds) {
		yield return new WaitForSeconds (seconds);
		gameObject.SetActive (false);
	}

}
