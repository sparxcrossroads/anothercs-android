using UnityEngine;
using System.Collections;

public class m4_fire : MonoBehaviour {

	// Use this for initialization
    public GameObject muzzle_0;
    public GameObject muzzle_1;
    public GameObject fireExplosion;

    private JoyMove m_joymove;

    private float m_time = 0;
    private float idle_time = 0.03f;

    public int m_bullet=5;
	void Start () {
        m_joymove = GameObject.FindGameObjectWithTag("player").GetComponent<JoyMove>();
	}
	
	// Update is called once per frame
	void Update () {
        if(m_joymove.stateinfo.nameHash==Animator.StringToHash("Base Layer.reload")&&!m_joymove.m_ani.IsInTransition(0))
            m_time=0;

        if (m_bullet <= 0)
            m_joymove.m_ani.SetBool("reload", true);

        if (m_joymove.inFire)
        {
            m_time += Time.deltaTime;
            print("muzzle");
            if (m_time >0.3&&m_bullet>=0)
            {
                print("muzzle luck");
            _instantiate();
            m_bullet--;
            //bullet destroy();

            m_time = 0;
            }          
        }
	}
    void _instantiate()
    {
                Instantiate(muzzle_0, transform.position, transform.rotation);
                Instantiate(muzzle_1, transform.position, transform.rotation);

                RaycastHit info;
                bool hit = Physics.Raycast
                (
                transform.position,
                transform.TransformDirection(Vector3.forward),
                out info,
                100
                );

            if(hit)
                Instantiate(fireExplosion, info.point, info.transform.rotation);
    }


}
