using UnityEngine;
using System.Collections;

public class next : MonoBehaviour {

    private float m_time=0;
    private string s = "";
	// Use this for initialization
    void Start()
    {
        s = "第一次做的游戏(花了2个星期) 比较 简陋(很多图片和声音资源找不到) =_,=.....\n" + "游戏是第3人称的射击  打中怪 目前可能比较艰难，可以从一个斜坡通过单条缝跑到一个红箱子上 在瞄准 旋转射击，先当跑酷玩吧.. 右下灰色的是进入场景的按钮(不是中间的)";
    }

    void OnGUI()
    {
        m_time += Time.deltaTime;     
        if (m_time > 7 &&m_time<40)
        {
            GUI.color = Color.white;
            GUI.skin.label.fontSize= 20;
            GUI.skin.box.fontSize = 18;
            GUI.Box(new Rect(Screen.width * 0.5f - 200, Screen.height * 0.5f - 100, 450, 250), "tip");
            GUI.Label(new Rect(Screen.width*0.5f-150, Screen.height*0.5f-80,400, 200),s);
        }

    }
    void _next()
    {
        print("ssss");
        Application.LoadLevel(1);
    }
}
