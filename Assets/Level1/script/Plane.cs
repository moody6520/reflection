using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour {
    public int circle = 0;
    public float localX = 0;
    public float localY = 0;
	// Use this for initialization
	void Start () {
        Vector3 local = transform.localRotation.eulerAngles;
        localX = local.x;
        localY = local.y;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
