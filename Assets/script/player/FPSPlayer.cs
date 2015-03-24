using UnityEngine;
using System.Collections;

public class FPSPlayer : MonoBehaviour
{

    // Use this for initialization
    private CharacterController m_ch;
    public Transform m_transform;
    private Animator m_ani;
    private Transform m_slight;
    public LayerMask m_layer;
    private gui m_gui;

    public Transform m_fx_level;
    public Transform m_fx_blood;

    private float m_moveSpeed = 2.5f;
    private float m_jumpSpeed = 8.0f;
    //private float m_rotaspeed = 100.0f;
    public int m_life = 5;
    private float m_gravity = 20.0f;
    //0 walk 1 run
    //private int fire_WalkorRun = 0;
    private float m_timer = 0;
    private int sign_fire = 0;
    
    private Vector3 movedirection=Vector3.zero;
    
    //sound
    public AudioClip m_audio_jump;
    public AudioClip m_audio_reload;
    public AudioClip m_audio_fire;
    public AudioClip m_audio_die;


    void Start()
    {
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();
        m_ani = this.GetComponent<Animator>();
        m_slight = GameObject.FindGameObjectWithTag("slight").transform;
        m_gui = GameObject.FindGameObjectWithTag("gui").GetComponent<gui>();

        Screen.lockCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_life <= 0)
        {
            //audio.PlayOneShot(m_audio_die);
            return;
        }
        else
            Screen.lockCursor = true;

        m_timer += Time.deltaTime;

        m_transform.eulerAngles=new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        AnimatorStateInfo stateinfo = m_ani.GetCurrentAnimatorStateInfo(0);


        if (m_ch.isGrounded)
        {
            movedirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            movedirection = transform.TransformDirection(movedirection);
            movedirection *= m_moveSpeed;

            if (Input.GetButton("Jump"))
            {
                //sound jump
                this.audio.PlayOneShot(m_audio_jump);

                movedirection.y = m_jumpSpeed;
            }

            // sound reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                this.audio.PlayOneShot(m_audio_reload);             
            }

        }

        // sound fire
        m_timer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1"))
        {
            //yield return new WaitForSeconds(2);
            sign_fire = 1;
            m_timer = 0;
    
        }
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.reload"))
            sign_fire = 0;
        if (m_timer > 0.15 && sign_fire == 1 && m_gui.m_bullet > 0)
        {
            this.audio.PlayOneShot(m_audio_fire);
            create_fire();
            m_gui.setbullet(1);
            sign_fire = 0;
        }

        movedirection.y -= m_gravity * Time.deltaTime;

        m_ch.Move(movedirection * Time.deltaTime);

        //transform.Rotate(0, 0, 0);

         // @animation
         // idle
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.idle") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);

            if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
                m_ani.SetBool("walk", true);

            if (Input.GetButton("Jump"))
                m_ani.SetBool("jump", true);

            if (Input.GetButton("Fire1")&&m_gui.m_bullet>0)
                m_ani.SetBool("idlefire", true);

            if (Input.GetKey(KeyCode.R))
            {
                print("reload");
                m_ani.SetBool("reload", true);
            }
        }

        // walk
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.walk") && !m_ani.IsInTransition(0))
        {
          
            m_ani.SetBool("walk", false);

            if (!Input.anyKey)
                m_ani.SetBool("idle", true);

            if (Input.GetButton("Jump"))
                m_ani.SetBool("jump", true);

            if (Input.GetButton("Fire1") && m_gui.m_bullet > 0)
                m_ani.SetBool("walkfire", true);

            if (Input.GetKey(KeyCode.R))
                m_ani.SetBool("reload", true);

        }

        // jump
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.jump") && !m_ani.IsInTransition(0))
        {

            m_ani.SetBool("jump", false);

            if (m_ch.isGrounded)
                m_ani.SetBool("idle", true);

        }

        // idlefire
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.idlefire") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idlefire", false);

           if (stateinfo.normalizedTime > 1.0f && m_timer > 1.5f)
             {
               print("idlefire->idle "+m_timer.ToString());
                 m_ani.SetBool("idle", true);
             }

            if (Input.GetKey(KeyCode.R))
            {
                print("reload");
                m_ani.SetBool("reload", true);
            }

     
        }

        // walkfire
        if (stateinfo.nameHash == Animator.StringToHash("Base Layer.walkfire") && !m_ani.IsInTransition(0))
        {
          
            m_ani.SetBool("walkfire", false);

            if (m_timer > 0.5f)
            {
                print("walkfire ->walk  m_timer:" + m_timer.ToString());

                m_ani.SetBool("walk", true);
            }

            if (Input.GetKey(KeyCode.R))
                m_ani.SetBool("reload", true);

        }

        //reload
         if (stateinfo.nameHash == Animator.StringToHash("Base Layer.reload") && !m_ani.IsInTransition(0))
         {
             m_ani.SetBool("reload", false);
             //finsh
             if (stateinfo.normalizedTime > 1.0f)
             {
                 m_gui.reload();
                 m_ani.SetBool("idle", true);
             }

         }

    }
    void create_fire()
    {
        RaycastHit info;
        bool hit = Physics.Raycast(
            m_slight.position,
            m_slight.TransformDirection(Vector3.forward),
            out info,
            100,
            m_layer);
        if (hit)
        {
            print(info.transform.tag.ToString());
            if (info.transform.tag.CompareTo("zombie") == 0)
            {
                zombie zombie = info.transform.GetComponent<zombie>();
                zombie.OnDamage(1);

                Instantiate(m_fx_blood, info.point, info.transform.rotation);
            }
            else
                Instantiate(m_fx_level, info.point, info.transform.rotation);
        }
    }

    public void onhurt(int hurtlevel)
    {
    
        Vector3 blood_pos = m_transform.position;
        blood_pos.y += 1.5f;

        Instantiate(m_fx_blood, blood_pos, transform.rotation);
        m_life -= hurtlevel;

        m_gui.setlife();

    }

}
