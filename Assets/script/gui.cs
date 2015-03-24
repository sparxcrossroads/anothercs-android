using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour {

    public static gui Instance = null;

    public int m_score = 0;
    public int m_bullet = 10;
    private FPSPlayer m_play;

    //ui name
    private GUIText text_bullet;
    private GUIText text_score;
    private GUIText text_life;

	// Use this for initialization
	void Start () {
        Instance = this;
        m_play = GameObject.FindGameObjectWithTag("player").GetComponent<FPSPlayer>();
       
        text_life = this.transform.FindChild("text_life").GetComponent<GUIText>();
        text_bullet = this.transform.FindChild("text_bullet").GetComponent<GUIText>();
        text_score = this.transform.FindChild("text_score").GetComponent<GUIText>();

        text_life.text = "life: " + m_play.m_life.ToString();
        text_bullet.text = "bullet: " + m_bullet.ToString();
        text_score.text = "score: " + m_score.ToString();
	}
	
	// Update is called once per frame
	void Update () {

	}
    void OnGUI()
    {
        if(m_play.m_life<=0)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = 40;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "you died");

            //restart
            GUI.skin.label.fontSize = 30;
            if (GUI.Button(new Rect(Screen.width * 0.5f - 150, Screen.height * 0.75f, 300, 40), "continue"))
                Application.LoadLevel(Application.loadedLevelName);
        }
        if (m_bullet <= 0)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = 40;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "push r reload");
        }
    }

    public void setlife()
    {
        text_life.text = "life: " + m_play.m_life.ToString();
    }
    public void setscore(int score)
    {
        m_score += score;
        text_score.text = "score: " + m_score.ToString();
    }
    public void setbullet(int bullet)
    {
        m_bullet -= bullet;
        if (m_bullet <= 0)
        {
            m_bullet = 0;

            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "R reload");
        }
        text_bullet.text = "bullet: " + m_bullet.ToString();

        
    }
    public void reload()
    {
        m_bullet = 10;
        text_bullet.text = "bullet: " + m_bullet.ToString();     
    }
}
