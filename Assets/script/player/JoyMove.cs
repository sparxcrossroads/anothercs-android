using UnityEngine;
using System.Collections;

public class JoyMove : MonoBehaviour {

	// Use this for initialization

    public Animator m_ani;
    private CharacterController m_ch;
    public AnimatorStateInfo stateinfo;
    public GameObject fx_blood;
    private gui _gui;

    public int m_life = 20;

    private float gravity = 8.0f;
    private float moveSpeed = 3.5f;
    private float jumpSpeed = 4.0f;

    private Vector3 movedirection;
    private Vector3 moveGravity;

    private bool inJump = false;
    public bool inFire = false;
    private bool inreload = false;
    private bool inrun = false;

    private float m_time=0;

    void Start()
    {
        m_ani = GetComponent<Animator>();
        m_ch = GetComponent<CharacterController>();
        _gui = GameObject.FindGameObjectWithTag("gui").GetComponent<gui>();

    }
    void Update()
    {
        if (m_life <= 0) return;
        // get aniamtor.stateinfo 
        stateinfo = m_ani.GetCurrentAnimatorStateInfo(0);

        //gravity
        if (!m_ch.isGrounded)
        {
            moveGravity.y -= gravity * Time.deltaTime;
            m_ch.Move(moveGravity * Time.deltaTime);
        }
        else
        {
            moveGravity = Vector3.zero;
        }

        //idle
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.idle") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);
            if (inFire)
                m_ani.SetBool("idlefire", true);

        }
        //jump
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.jump") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("jump", false);
            print("ss");
            if (m_ch.isGrounded)
            {
                m_ani.SetBool("idle", true);
                inJump = false;
            }
        }
        //walk
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.walk") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("walk", false);
            if (inFire)
                m_ani.SetBool("walkfire", true);
        }

        //idlefire
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.idlefire") && !m_ani.IsInTransition(0))
        {

            m_ani.SetBool("idlefire", false);

            if (!inFire)
                m_ani.SetBool("idle", true);


        }

        //walkfire
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.idlefire") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("walkfire", false);

            if (!inFire)
                m_ani.SetBool("walk", false);
        }

        //reload
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.reload") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("reload", false);
            if (stateinfo.normalizedTime > 1.0f)
            {
                print("sss");
                _gui.resetbullet();
                m_ani.SetBool("idle", true);
                inreload = false;
                print("##relaod-> :" + inreload.ToString());
            }
            //else 
            //{
            //    // 防止在reload没做完就转化到idel的bug 和后面可能的 ->run
            //    inreload = true;
            //}

        }

        //run
         if (stateinfo.nameHash == Animator.StringToHash("Base Layer.run") && !m_ani.IsInTransition(0))
         {
             m_ani.SetBool("run", false);
             print("run-?");
             print("inrun:" + inrun.ToString());

             if (!inrun && !inreload)
             {
                 m_time = 0;
                 inrun = false;
                 m_ani.SetBool("idle", true);
             }
             if (inFire)
                 m_ani.SetBool("runfire", true);
         }

        //runfire
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.runfire") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("runfire", false);
            if (!inFire)
                m_ani.SetBool("run", true);
        }
    }

    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoyStickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }
	
    void OnJoystickMoveEnd(MovingJoystick move)
    {
        m_ani.SetBool("walk", false);
        // stop joystick 
        if (move.joystickName == "MoveJoystick" && !inJump)
        {
            m_time = 0;
            inrun = false;
            m_ani.SetBool("idle", true);
        }
    }

    void OnJoyStickMove(MovingJoystick move)
    {
        m_ani.SetBool("idle", false);
        
        if(move.joystickName!="MoveJoystick")
        {
            return;
        }

        //during reload freeze
        if (inreload)
            return;

        float joyPositionX = move.joystickAxis.x;
        float joyPositionY = move.joystickAxis.y;


        if (joyPositionX * joyPositionX + joyPositionY * joyPositionY > 0.95)
        {
            m_time += Time.deltaTime;

            if (m_time > 0.25f)
            {
                m_ani.SetBool("run", true);
                inrun = true;
            }
        }
        else
            inrun = false;

        print(inrun.ToString());
        if (inrun)
            moveSpeed = 5.5f;
        else
        
            moveSpeed = 3.5f;

        if(joyPositionY!=0||joyPositionX!=0)
        {
            transform.LookAt(new Vector3(transform.position.x + joyPositionX,
                transform.position.y,
                transform.position.z + joyPositionY));

           
                movedirection = Vector3.forward*Time.deltaTime*moveSpeed;

                m_ch.Move(transform.TransformDirection(movedirection));
           
                m_ani.SetBool("walk", true);
        }
    }
    
    void jump()
    {
        //print("jump");
        m_ani.SetBool("jump", true);
        if(!inreload)
            inJump = true;
        moveGravity.y += jumpSpeed;
        m_ch.Move(moveGravity * Time.deltaTime);
               
    }

    void fire()
    {
        print("fire");
        inFire = true;
        
    }
    void nofire()
    {
        print("nofire");
        inFire = false;
    }

    void reload()
    {
        inreload = true;
        m_ani.SetBool("reload", true);
        print("button relaod"+inreload.ToString());
    }

    public void onhurt(int hurtlevel)
    {
        if (m_life < 0)
            return;
        m_life -= hurtlevel;
        float _x = Random.Range(-0.2f, 0.2f);
        float _y = Random.Range(1.3f, 1.8f);
        float _z = Random.Range(-0.2f, 0.2f);
        Vector3 _ran_pos = new Vector3(_x, _y, _z);
        Instantiate(fx_blood, transform.position+_ran_pos, transform.rotation);
        _gui.setlife();
    }
}
