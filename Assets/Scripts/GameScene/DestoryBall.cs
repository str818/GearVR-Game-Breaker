using UnityEngine;
using System.Collections;

public class DestoryBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
