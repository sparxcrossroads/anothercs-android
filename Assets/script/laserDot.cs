using UnityEngine;
using System.Collections;

public class laserDot : MonoBehaviour {

    public float scrollSpeed = 0.5f;
    public float pulseSpeed = 1.5f;

    public float noiseSize = 1.0f;

    public float maxWidth = 0.5f;
    public float minWidth = 0.2f;

    public GameObject pointer=null;

    private LineRenderer lRenderer;
    private float aniTime = 0.0f;
    private float aniDir = 1.0f;

    private PerFrameRaycast raycast;
	// Use this for initialization
	void Start () {
        lRenderer = (LineRenderer)gameObject.GetComponent<LineRenderer>();

        aniTime = 0.0f;
        //pointer = this.gameObject;
        ChoseNewAnimationTargetCoroutine();
        raycast = GetComponent<PerFrameRaycast>();
	}

    IEnumerator ChoseNewAnimationTargetCoroutine()
    {
        while (true)
        {
            aniDir = aniDir * 0.9f + Random.Range(0.5f,1.5f)*0.1f;
            // maybe wrong
            yield return 0;
            minWidth = minWidth * 0.8f + Random.Range(0.1f, 1.0f) * 0.2f;
            yield return new WaitForSeconds(1.0f + Random.value * 2.0f - 1.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 _ren=renderer.material.mainTextureOffset;
        _ren.x+=Time.deltaTime * aniDir * scrollSpeed;

        renderer.material.mainTextureOffset=_ren;
        renderer.material.SetTextureOffset("_NoiseTex", new Vector2(-Time.time * aniDir * scrollSpeed, 0.0f));

        float aniFactor = Mathf.PingPong(Time.time * pulseSpeed, 1.0f);
        aniFactor = Mathf.Max(minWidth, aniFactor) * maxWidth;
        lRenderer.SetWidth(aniFactor, aniFactor);
       
        RaycastHit hitinfo = raycast.GetHitInfo();
        if(hitinfo.transform)
        {
            lRenderer.SetPosition(1, (hitinfo.distance * Vector3.forward));

            Vector2 _sca=renderer.material.mainTextureScale;
            _sca.x=(hitinfo.distance)*0.1f;
            renderer.material.mainTextureScale = _sca;
            renderer.material.SetTextureScale("_NoiseTex", new Vector2(0.1f * hitinfo.distance * noiseSize, noiseSize));

            if (pointer)
            {
                pointer.renderer.enabled = true;
                pointer.transform.position = hitinfo.point + (transform.position - hitinfo.point) * 0.1f;
                pointer.transform.rotation = Quaternion.LookRotation(hitinfo.normal, transform.up);
                pointer.transform.eulerAngles += new Vector3(90.0f, 0, 0);
            }
        }
        else
        {
            if (pointer)
                pointer.renderer.enabled = false;
            float maxDist = 200.0f;
            lRenderer.SetPosition(1, (maxDist * Vector3.forward));
            renderer.material.mainTextureScale += new Vector2(0.1f * (maxDist), 0);
            renderer.material.SetTextureScale("_NoiseTex", new Vector2(0.1f * (maxDist) * noiseSize, noiseSize));
        }
	
	}
}
