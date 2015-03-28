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
        if (move.joystickName != "rotate")
            return;

        float joyPositionX = move.joystickAxis.x;
        print(joyPositionX.ToString());

        if(joyPositionX!=0)
        {
            Vector3 _rotate=Vector3.zero;
            _rotate.y=joyPositionX*Time.deltaTime*120;

            transform.eulerAngles += _rotate;
        }
    }
}
