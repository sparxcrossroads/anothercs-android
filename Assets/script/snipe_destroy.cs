using UnityEngine;
using System.Collections;

public class snipe_destroy : MonoBehaviour {

	// Use this for initialization
    public float destroy_time = 2.0f;
	void Start () {
        Destroy(this.gameObject, destroy_time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
