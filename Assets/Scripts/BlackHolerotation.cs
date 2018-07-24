using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHolerotation : MonoBehaviour {
    //float angle = 0;
    public float speed = 7.0f; //2*PI in degress is 360, so you get 5 seconds to complete a circle
    //float radius = 5;

    void Start () {
       
    }
	
	void Update () {
        transform.Rotate(0.0f, Time.deltaTime * speed,0.0f);
    }
}
