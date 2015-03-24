using UnityEngine;
using System.Collections;

public class smoothfollow : MonoBehaviour {

    public Transform target;

    private float distance = 5.0f;
    private float height = 5.0f;
    private float heightDamping = 1.0f;
    private float rotationDamping = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void LateUpdate()
    {

        if (!target) return;

        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        Vector3 cam_pos = target.position;
        cam_pos.y = currentHeight;

        transform.position = cam_pos;

        transform.position -= currentRotation * Vector3.forward * distance;
         

        transform.LookAt(target);
    }
}
