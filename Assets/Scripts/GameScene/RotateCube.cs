using UnityEngine;
using System.Collections;

public class RotateCube : MonoBehaviour {

    private int v = 50;

	void Update () {
        transform.Rotate(0,v*Time.deltaTime,0);
	}

}
