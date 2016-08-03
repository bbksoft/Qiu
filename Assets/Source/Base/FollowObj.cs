using UnityEngine;
using System.Collections;

public class FollowObj : MonoBehaviour {

    public GameObject obj;
    public Vector3 off;

    Vector3 curOff;
    Vector3 toOff;

    public void SetXValue(float v)
    {
        toOff = off * v;
    }      

    void Awake()
    {
        curOff = off;
        toOff  = off;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (obj != null)
        {
            curOff = Vector3.Lerp(curOff, toOff, 0.01f);
            transform.position = obj.transform.position + curOff;
        }
	}
}
