using UnityEngine;
using System.Collections;

public class PerFrameRaycast : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public RaycastHit GetHitInfo()
    {
        RaycastHit info;
        bool hit = Physics.Raycast(
            this.transform.position,
        this.transform.TransformDirection(Vector3.forward),
        out info,
        100);
        
        return info;
    }
}
