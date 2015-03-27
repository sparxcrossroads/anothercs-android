using UnityEngine;
using System.Collections;

public class line : MonoBehaviour {

	// Use this for initialization

    private LineRenderer m_line;
    private int lengthofLineRenderer=2;
    public GameObject m_fx;
    private Transform m_transform;

    public  Transform m_pla;

	void Start () {
        m_transform = this.transform;
        m_line = GetComponent<LineRenderer>();
        m_line.useWorldSpace = true;
        m_line.SetVertexCount(lengthofLineRenderer);
        m_line.SetWidth(0.5f, 0.5f);

    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit info;
        bool hit = Physics.Raycast(
            m_transform.position,
            m_transform.TransformDirection(Vector3.forward),
            out info,
            100
        );
        if (hit)
        {
            //Instantiate(m_fx, info.point, transform.rotation);

            m_line.SetPosition(0, m_transform.position);
            m_line.SetPosition(1, info.point);

            print(m_transform.eulerAngles.ToString());

            m_pla.transform.position = info.point;
            m_pla.transform.eulerAngles = m_transform.eulerAngles+new Vector3(90,0,0);
        }
        else
        {
            m_line.SetPosition(0, m_transform.position);
            m_line.SetPosition(1,m_transform.TransformDirection(Vector3.forward)*100);

            m_pla.transform.position = m_transform.TransformDirection(Vector3.forward)*100;
        }

	}
}
