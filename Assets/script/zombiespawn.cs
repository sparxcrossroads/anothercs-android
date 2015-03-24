using UnityEngine;
using System.Collections;

public class zombiespawn : MonoBehaviour {

    private Transform m_transform;
    public Transform[] m_zombie;

    public int m_count;
    private int m_maxcount=3;
    private float m_time=0;
    private int rand;
	// Use this for initialization
	void Start () {
        m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_count >= m_maxcount)
            return;
        m_time += Time.deltaTime;
        if (m_time > 1)
        {
            rand = Random.Range(0,3);
            Transform obj=(Transform)Instantiate(m_zombie[rand], m_transform.position, m_transform.rotation);
            zombie enemy = obj.GetComponent<zombie>();
            enemy.init(this);
            m_time = 0;
        }
	}
}
