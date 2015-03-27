using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour {

    public static gui Instance = null;

    public int m_score = 0;
    private JoyMove m_play;

    //ui name
    private GUIText text_bullet;
    private GUIText text_score;
    private GUIText text_life;
    private GUITexture hp_red;
    private GUITexture hp_black;

    private Transform trans_red;

    public int m_bullet = 100;

    private bool m_pause = false;

	// Use this for initialization
	void Start () {
        Instance = this;
        m_play = GameObject.FindGameObjectWithTag("player").GetComponent<JoyMove>();

        //text_life = this.transform.FindChild("text_life").GetComponent<GUIText>();
        text_bullet = this.transform.FindChild("text_bullet").GetComponent<GUIText>();
        text_score = this.transform.FindChild("text_score").GetComponent<GUIText>();
        text_life = this.transform.FindChild("text_life").GetComponent<GUIText>();
        hp_red = this.transform.FindChild("hp_red").GetComponent<GUITexture>();
        hp_black = this.transform.FindChild("hp_black").GetComponent<GUITexture>();


        // 缩放y 轴 抖动效果
        trans_red = this.transform.FindChild("hp_red").GetComponent<Transform>();

        text_life.text = "life: " + m_play.m_life.ToString();
        text_bullet.text = "bullet: " +m_bullet.ToString();

        text_score.text = "score: " + m_score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_play.m_life <= 0)
            return;
        if (m_play.m_life <= 5)
        {
            float _red = Random.Range(0, 0.2f);
            trans_red.localScale = new Vector3(0, _red, 1);
        }

        hp_black.pixelInset = new Rect(-64,-29,200,58);
        hp_red.pixelInset = new Rect(-64, -29, 10 * m_play.m_life, 58);

	}
    void OnGUI()
    {
        
        if(m_play.m_life<=0)
        {
            GUI.color = Color.red;

            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = 40;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height-100), "you died");

            //restart
            GUI.skin.label.fontSize = 30;
            if (GUI.Button(new Rect(Screen.width * 0.5f - 150, Screen.height * 0.5f, 300, 40), "continue"))
                Application.LoadLevel(Application.loadedLevelName);
        }
       
        if(m_pause)
        {
            GUI.color = Color.red;
            GUI.skin.box.fontSize = 16;
            GUI.Box(new Rect(Screen.width * 0.5f-200, Screen.height * 0.5f-100, 350, 150),"menu (push again return to scene)");
            GUI.color = Color.white;
            if (GUI.Button(new Rect(Screen.width * 0.5f - 160, Screen.height * 0.5f - 40, 100, 60), "Restart"))
            {
                Application.LoadLevel(Application.loadedLevelName);
                Time.timeScale = 1;
            }
            if (GUI.Button(new Rect(Screen.width * 0.5f, Screen.height * 0.5f - 40, 100, 60), "Exit"))
                Application.Quit();

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
    public void setbullet()
    {
        if (m_bullet <= 0)
            return;
        m_bullet--;
        text_bullet.text = "bullet: " + m_bullet.ToString();
    }
    public void resetbullet()

    {
        m_bullet = 100;
        text_bullet.text = "bullet: " + m_bullet.ToString();
    }

    void menu()
    {
        m_pause = !m_pause;
        Time.timeScale = m_pause ? 0 : 1;

        print("sss");
    }
  
}
