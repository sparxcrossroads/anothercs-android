using UnityEngine;
using System.Collections;

public class ray : MonoBehaviour {

    private Transform m_raypos;
    public LayerMask m_layer;
    public Transform m_fx_level;
    public Transform m_fx_blood;

	// Use this for initialization
	void Start () {
        m_raypos = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void create()
    {
        RaycastHit info;
        bool hit = Physics.Raycast(
            m_raypos.position,
            m_raypos.TransformDirection(Vector3.forward),
            out info,
            100,
            m_layer);
        if (hit)
            Instantiate(m_fx_level, info.point, info.transform.rotation);
    }
}
