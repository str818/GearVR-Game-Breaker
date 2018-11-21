using UnityEngine;
using System.Collections;

public class RotateDoorH : MonoBehaviour {

    public int v = 50;
    public int dir = 1;
	
	void Update () {
	
        transform.Rotate(0,dir*v*Time.deltaTime,0);
	}
}
