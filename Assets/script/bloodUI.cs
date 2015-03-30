using UnityEngine;
using System.Collections;

public class bloodUI : MonoBehaviour {

    public Transform ui;

    //default distance form main camera
    private float fomat;
    private Transform head;

	// Use this for initialization
	void Start () {
        head = this.transform.FindChild("head").GetComponent<Transform>();
        fomat = Vector3.Distance(head.position, Camera.main.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        float newFomat = fomat / Vector3.Distance(head.position, Camera.main.transform.position);
        //ui.position = worldToUI(head.position);
        //ui.localScale = Vector3.one * newFomat;
        ui.position = head.position;
	}
    
    Vector3 worldToUI(Vector3 point)
    {
        Vector3 pt = Camera.main.WorldToScreenPoint(point);
        //我发现有时候UICamera.currentCamera 有时候currentCamera会取错，取的时候注意一下啊。
        //Vector3 ff = UICamera.currentCamera.ScreenToWorldPoint(pt);
        pt.z = 0;
        return pt;
    }
}
