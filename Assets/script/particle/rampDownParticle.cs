using UnityEngine;
using System.Collections;

public class rampDownParticle : MonoBehaviour {

    public float delayTime = 0;
    public float delayPlusTime=0;
    public float rampDownTime = 1;
    public float oriMinEmission;
    public float oriMaxEmission;
	// Use this for initialization
	void Start () {
        oriMinEmission = particleEmitter.minEmission;
        oriMaxEmission = particleEmitter.maxEmission;

        particleEmitter.emit = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (delayTime + delayPlusTime > 0)
            delayTime -= Time.deltaTime;

        if (delayTime <= 0 && particleEmitter.emit == false)
            particleEmitter.emit = true;

        if((delayTime+delayPlusTime)<=0)
        {
            particleEmitter.minEmission = oriMinEmission * rampDownTime;
            particleEmitter.maxEmission = oriMaxEmission * rampDownTime;
            rampDownTime -= Time.deltaTime;

            if (rampDownTime < 0) { rampDownTime = 0; }
        }
	}
}
