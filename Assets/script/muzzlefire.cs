using UnityEngine;
using System.Collections;

public class muzzlefire : MonoBehaviour {

    public float minLife = 0.01f;
    public float maxlife = 0.02f;

    private float destroyTime;
	// Use this for initialization
	void Start () {
        destroyTime = Time.time + Random.Range(minLife, maxlife);
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.time>destroyTime)
        {
            Destroy(gameObject);
        }
        transform.LookAt(Camera.main.transform.position);
	}
}
