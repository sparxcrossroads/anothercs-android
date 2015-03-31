using UnityEngine;
using System.Collections;

public class show : MonoBehaviour {

	// Use this for initialization
    public GUITexture m_show;
	void Start () {
        m_show.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
