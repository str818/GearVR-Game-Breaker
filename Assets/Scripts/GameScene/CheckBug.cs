using UnityEngine;
using System.Collections;

public class CheckBug : MonoBehaviour {

	float mass;			// 质量索引

	void Start () {
		mass = gameObject.GetComponent<Rigidbody> ().mass;			// 初始化自身质量
	}

	void Update () {
		if (mass != 80.0f) {										// 如果自身质量不等于原本棱锥的质量
			// deleteChilds ();
			gameObject.GetComponent<Renderer>().GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color(1.0f, 1.0f, 1.0f, 0.8f));	// 改变颜色
			gameObject.GetComponent<ShatterTool>().enabled = false;
			gameObject.GetComponent<ShatterOnCollision>().enabled = false;
			gameObject.GetComponent<WorldUvMapper>().enabled = false;
			this.enabled = false;	// 消除此脚本，不然会影响FPS
		}
	}

}