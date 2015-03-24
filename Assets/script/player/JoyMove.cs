using UnityEngine;
using System.Collections;

public class JoyMove : MonoBehaviour {

	// Use this for initialization

    private Animator m_ani;
    private CharacterController m_ch;
    private AnimatorStateInfo stateinfo;

    private float gravity = 8.0f;
    private float moveSpeed = 3.0f;
    private float jumpSpeed = 4.0f;

    private Vector3 movedirection;
    private Vector3 moveGravity;

    private bool inJump = false;
    private bool inFire = false;

    void Start()
    {
        m_ani = GetComponent<Animator>();
        m_ch = GetComponent<CharacterController>();

    }
    void Update()
    {
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
            print("error");
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

        float joyPositionX = move.joystickAxis.x;
        float joyPositionY = move.joystickAxis.y;

        if(joyPositionY!=0||joyPositionX!=0)
        {
            transform.LookAt(new Vector3(transform.position.x + joyPositionX,
                transform.position.y,
                transform.position.z + joyPositionY));

            if (m_ch.isGrounded)
            {
                movedirection = Vector3.forward*Time.deltaTime*moveSpeed;

                m_ch.Move(transform.TransformDirection(movedirection));
            }
            //transform.Translate(Vector3.forward * Time.deltaTime * 5);
            m_ani.SetBool("walk", true);
        }
    }
    
    void jump()
    {
        print("jump");
        m_ani.SetBool("jump", true);
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
}
