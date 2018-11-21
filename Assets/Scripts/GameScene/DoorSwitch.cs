using UnityEngine;
using System.Collections;

// 门开关
public class DoorSwitch : MonoBehaviour {

	void Start () {
		openDoor ();		// 开门
	}

	// 开门的方法
	private void openDoor () {
		transform.parent.FindChild ("men").FindChild ("CubeL").GetComponent<OpenNow> ().enabled = true;
		transform.parent.FindChild ("men").FindChild ("CubeR").GetComponent<OpenNow> ().enabled = true;
		transform.GetComponent<Renderer> ().GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color (0.28f, 0.28f, 0.28f, 0.8f));	// 改变棱锥颜色
	}

}
