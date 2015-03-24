using UnityEngine;
using System.Collections;

public class zombie : MonoBehaviour {

    private Transform m_transform;
    private FPSPlayer m_player;
    private NavMeshAgent m_agent;
    private Animator m_ani;
    private Collider m_col;
    private gui m_gui;

    public int m_life = 2;

    private float m_moveSpeed = 0.5f;
    private float m_rotSpeed=90.0f;
    private float m_timer = 0;          //udate  relock player


    protected zombiespawn m_spawn;
    public void init(zombiespawn spawn)
    {
        m_spawn=spawn;
        m_spawn.m_count++;
    }

	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_player = GameObject.FindGameObjectWithTag("player").GetComponent<FPSPlayer>();
        m_agent = this.GetComponent<NavMeshAgent>();
        //m_agent.SetDestination(m_player.transform.position);

        m_ani = this.GetComponent<Animator>();
        m_col = this.GetComponent<Collider>();
        m_gui = GameObject.FindGameObjectWithTag("gui").GetComponent<gui>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //print("playaer -> :"+m_player.m_transform.position.ToString());
        if (m_player.m_life <= 0) return;
        AnimatorStateInfo stateinfo = m_ani.GetCurrentAnimatorStateInfo(0);


        //idle
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.idle") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);

            m_timer -= Time.deltaTime;
            if (m_timer > 0) return;

            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 2.0f)
                m_ani.SetBool("attack", true);
            else
            {
                m_timer = 0;
                m_agent.SetDestination(m_player.m_transform.position);

                m_ani.SetBool("run", true);
            }
        }

        //run
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.run") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);
            m_timer -= Time.deltaTime;

            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.m_transform.position);
                m_timer = 1;
            }

            float speed = m_moveSpeed * Time.deltaTime;
            m_agent.Move(m_transform.TransformDirection(new Vector3(0, 0, speed)));

            //attack
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 2.0f)
            {
                m_agent.ResetPath();
                m_ani.SetBool("attack", true);
            }
        }
       
        //attack
        if(stateinfo.nameHash==Animator.StringToHash("Base Layer.attack")&&!m_ani.IsInTransition(0))
        {          
            m_ani.SetBool("attack",false);
            // get the attack angle  考虑转身
            Vector3 oldangle=m_transform.eulerAngles;

            m_transform.LookAt(m_player.transform);
            float target=m_transform.eulerAngles.y;

            float speed=m_rotSpeed*Time.deltaTime;

            float angle=Mathf.MoveTowardsAngle(oldangle.y,target,speed);
            m_transform.eulerAngles=new Vector3(0,angle,0);

            if(stateinfo.normalizedTime>=1.0f)
            {
                m_player.onhurt(1);
                m_ani.SetBool("idle", true);
                m_timer=1;
            }
          

        }

	    //die
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.die") && !m_ani.IsInTransition(0))
        {
            m_agent.SetDestination(m_transform.position);
            m_col.isTrigger = true;
            Vector3 pos = m_transform.position;
            pos.y = -0.95f;
            m_transform.position = pos;

            if (stateinfo.normalizedTime >= 1.0f)
            {
                Destroy(this.gameObject, 5.0f);
            }

        }
	}

    public void OnDamage(int damage)
    {
       
        m_life -= damage;

        if (m_life == 0)
        {
            m_gui.setscore(10);
            m_spawn.m_count--;
            m_ani.SetBool("die", true);
        }
    }
}
