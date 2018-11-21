using UnityEngine;
using System.Collections;

public class RotateFan : MonoBehaviour {

	private int v = 100;

	void Update () {
		transform.Rotate (v * Time.deltaTime, 0, 0);
	}

}
