using UnityEngine;
using System.Collections;

public class fx_destroy : MonoBehaviour {

    public float destroy_time=2.0f;
	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, destroy_time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
