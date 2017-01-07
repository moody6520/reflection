using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    public Transform transform;
    public float speed;
    public string cntRoad;
    public string cntSurface;
	// Use this for initialization
    void Start()
    {
        this.transform = GameObject.Find("character").transform;
        speed = 0.1f;
        cntRoad = "B-0-4";
        cntSurface = "B";
    }
	
	// Update is called once per frame
	void Update () {
        //GameObject.Find("character").transform.localScale = new Vector3(7, 7, 7);
	}
    public void resetPosition()
    {
        this.transform.position =  GameObject.Find("character").transform.position;
    }
    public void forward(Transform tar)
    {
        float dx = (float)(tar.position[0] - this.transform.position[0]);
        float dy = (float)(tar.position[1] - this.transform.position[1]);
        float dz = (float)(tar.position[2] - this.transform.position[2]);
        float deltaX,deltaY,deltaZ;
        if (dx * dx + dy * dy + dz * dz < speed * speed * 1.1)
        {
            deltaX = dx;
            deltaY = dy;
            deltaZ = dz;
        }
        else
        {
            float deltaSpeed = speed / (Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz));
            deltaX = deltaSpeed * dx;
            deltaY = deltaSpeed * dy;
            deltaZ = deltaSpeed * dz;
        }
        //this.transform.Translate(deltaX, deltaY, deltaZ);
        Vector3 positionVector = this.transform.position;
        positionVector[0] += deltaX;
        positionVector[1] += deltaY;
        positionVector[2] += deltaZ;
        this.transform.position = positionVector;

        this.transform.LookAt(tar, new Vector3(
            GameObject.Find(cntSurface + "mark1").transform.position.x - GameObject.Find(cntSurface + "mark0").transform.position.x,
            GameObject.Find(cntSurface + "mark1").transform.position.y - GameObject.Find(cntSurface + "mark0").transform.position.y,
            GameObject.Find(cntSurface + "mark1").transform.position.z - GameObject.Find(cntSurface + "mark0").transform.position.z)
            );

    }
    public bool isReach(Transform point)
    {
        double dx = point.position[0] - this.transform.position[0];
        double dy = point.position[1] - this.transform.position[1];
        double dz = point.position[2] - this.transform.position[2];
        return dx < 0.1 && dx > -0.1 && dy < 0.1 && dy > -0.1 && dz < 0.1 && dz > -0.1;
    }
    public void setFinalPosition(Transform finalPosition)
    {
        this.transform.position = finalPosition.position;
    }
    public void setFinalRoute(string finalRoute)
    {
        this.cntRoad = finalRoute;
    }
    public void forwardNextSurface(List<double>point)
    {
        
    }
    public void bindToCntSurface()
    {
        GameObject.Find("character").transform.SetParent(GameObject.Find("Plane" + this.cntSurface).transform);
    }
    public void unbindCntSurface()
    {
        GameObject.Find("character").transform.SetParent(GameObject.Find("Cube").transform);
    }
    public void test()
    {
        this.transform.Translate(0.03f, 0.0f, 0.0f);
    }
    public void forcePosition(string surface)
    {
        if(surface.Contains(this.cntSurface))
            this.transform.localScale = (new Vector3(1, 700, 1));
    }

}
;