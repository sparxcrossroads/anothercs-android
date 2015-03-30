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
    private bool inaim = false;

    //audioclip
    public AudioClip m_audio_jump;
    public AudioClip m_audio_reload;
    public AudioClip m_audio_run;

    private float m_time=0;
    private float m_time_audio = 0;

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
            if (inFire)
                m_ani.SetInteger("index", 11);

        }
        //idleaim
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.idleaim") && !m_ani.IsInTransition(0))
        {
            if (!inaim)
                m_ani.SetInteger("index", 14);
        }
        //jump
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.jump") && !m_ani.IsInTransition(0))
        {
            if (m_ch.isGrounded)
            {
                m_ani.SetInteger("index", 1);
                inJump = false;
            }
        }
        //walk
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.walk") && !m_ani.IsInTransition(0))
        {
            if (inFire)
                m_ani.SetInteger("index",21);
        }

        //idlefire
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.idlefire") && !m_ani.IsInTransition(0))
        {
            print("idlefire");
            if (!inFire)
                m_ani.SetInteger("index", 12);


        }

        //walkfire
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.walkfire") && !m_ani.IsInTransition(0))
        {
            if (!inFire)
                m_ani.SetInteger("index", 2);
        }

        //reload
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.reload") && !m_ani.IsInTransition(0))
        {
            if (stateinfo.normalizedTime > 1.0f)
            {
                _gui.resetbullet();
                m_ani.SetInteger("index", 1);
                inreload = false;
            }

        }

        //run
         if (stateinfo.nameHash == Animator.StringToHash("Base Layer.run") && !m_ani.IsInTransition(0))
         {

             if (!inrun && !inreload)
             {
                 m_time = 0;
                 inrun = false;
                 //m_ani.SetInteger("index", 2);
             }
             if (inFire)
                 m_ani.SetInteger("index", 31);
         }

        //runfire
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.runfire") && !m_ani.IsInTransition(0))
        {
            if (!inFire)
                m_ani.SetInteger("index", 3);
        }
    }

    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoyStickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }

    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= OnJoyStickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }
    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= OnJoyStickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }

    void OnJoystickMoveEnd(MovingJoystick move)
    {
        // stop joystick 
        if (move.joystickName == "MoveJoystick" && !inJump)
        {
            m_time = 0;
            inrun = false;
            m_ani.SetInteger("index", 1);
            //intouch = false;
        }
    }

    void OnJoyStickMove(MovingJoystick move)
    {
        if(move.joystickName!="MoveJoystick")
        {
            return;
        }

        //intouch = true;
        float joyPositionX = move.joystickAxis.x;
        float joyPositionY = move.joystickAxis.y;

        if (joyPositionX * joyPositionX + joyPositionY * joyPositionY > 0.95)
        {
            m_time += Time.deltaTime;

            if (m_time > 0.1f&&!inJump)
            {
                m_time_audio += Time.deltaTime;
                //print("run");
                if (m_time_audio > 1.5f)
                {
                    this.audio.PlayOneShot(m_audio_run);
                    m_time_audio = -1;
                }
                if(!inFire)
                    m_ani.SetInteger("index", 3);
                inrun = true;
            }
        }
        else
            inrun = false;

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
            if(!inrun&&!inJump&&!inFire)
                m_ani.SetInteger("index", 2);
        }      
    }
    
    void jump()
    {
        //print("jump");
        m_ani.SetInteger("index", 4);
        if(!inreload)
            inJump = true;
        moveGravity.y += jumpSpeed;
        m_ch.Move(moveGravity * Time.deltaTime);

        audio.PlayOneShot(m_audio_jump);
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
        m_ani.SetInteger("index", 5);

        this.audio.PlayOneShot(m_audio_reload);
    }

    void idleaim()
    {
        inaim = true;
        m_ani.SetInteger("index", 13);
    }

    void noidleaim()
    {
        inaim = false;
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
