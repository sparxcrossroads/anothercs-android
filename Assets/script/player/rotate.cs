using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	// Use this for initialization
    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }

    void OnJoystickMoveEnd(MovingJoystick move)
    {
        if (move.joystickName == "rotate")
            print("rotate end");
    }
	
	// Update is called once per frame
	void OnJoystickMove(MovingJoystick move)
    {
        //print("luck");
        //if (move.joystickName != "rotate")
        //    return;
        print("luck2222");
  
    }
}
