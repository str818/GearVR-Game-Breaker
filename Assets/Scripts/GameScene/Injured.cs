using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Injured : MonoBehaviour {

	Color flash_Color;									// 效果颜色
	public float flash_Speed = 5;						// 效果颜色变化速度

	void Start () {
		flash_Color = new Color (0.8f, 0.1f, 0.1f, 0.8f);
	}

	void Update () {

        //Material material=transform.gameObject.GetComponent<Renderer>
		if (UIdata.isInjured) {							// 如果受伤
            transform.gameObject.GetComponent<Renderer> ().GetComponent<MeshRenderer> ().material.SetColor ("_Color", flash_Color);	
			//transform.GetComponent<Mesh>(). = flash_Color;
		} else {
            transform.gameObject.GetComponent<Renderer>().GetComponent<MeshRenderer>().material.color = Color.Lerp(transform.gameObject.GetComponent<Renderer>().GetComponent<MeshRenderer>().material.color, Color.clear, flash_Speed * Time.deltaTime);
		}
		UIdata.isInjured = false;						// 标志位置为false
	}

}
