using UnityEngine;
using System.Collections;

public class camerafollow : MonoBehaviour {
    public float camera_height = 1.8f;
    public float camera_distance = 1.0f;

    private Transform player;
    private Transform m_cma;

    private Vector3 m_camrot;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("player").transform;
        m_cma = Camera.main.transform;
        m_camrot = m_cma.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {

        //look at palyer
        //camera.LookAt(player);

        float rv = Input.GetAxis("Mouse Y");
        float rh = Input.GetAxis("Mouse X");
        m_camrot.x -= rv;
        m_camrot.y += rh;
        m_cma.eulerAngles = m_camrot;

        //forward
        //camera.eulerAngles = new Vector3(camera.eulerAngles.x,
        //    player.eulerAngles.y,
        //    camera.eulerAngles.z);

        float angle = m_cma.eulerAngles.y;

        float deltax = camera_distance * Mathf.Sin(angle * Mathf.PI / 180);
        float deltaz = camera_distance * Mathf.Cos(angle * Mathf.PI / 180);


        m_cma.position = new Vector3(player.position.x - deltax + 0.5f,
            player.position.y + camera_height,
            player.position.z - deltaz);

        //Debug.Log("angle: " + angle + ",deltax:" + deltax + "deltaz: " + deltaz);
        //Debug.Log(player.position);
        //Debug.Log(camera.position);
	}
}
