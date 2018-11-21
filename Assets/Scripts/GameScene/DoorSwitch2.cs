using UnityEngine;
using System.Collections;

public class DoorSwitch2 : MonoBehaviour {

	void Start () {
		openDoor ();		// 开门
	}

	// 开门的方法
	private void openDoor () {
		transform.parent.FindChild ("men").FindChild ("CubeLU").GetComponent<OpenNow> ().enabled = true;
		transform.parent.FindChild ("men").FindChild ("CubeRU").GetComponent<OpenNow> ().enabled = true;
		transform.parent.FindChild ("men").FindChild ("CubeLD").GetComponent<OpenNow> ().enabled = true;
		transform.parent.FindChild ("men").FindChild ("CubeRD").GetComponent<OpenNow> ().enabled = true;
		transform.GetComponent<Renderer> ().GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color (0.28f, 0.28f, 0.28f, 0.8f));	// 改变棱锥颜色
	}

}
