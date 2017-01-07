using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GetMouseSelect : MonoBehaviour {
    Transform preSelected = null;
    bool isRotating = false;
	void Start () {
	
	}
    public MapController mapController;
    public Character character;
    //public Material SelectMaterial;
    //public Material UnSelectMaterial;
	// Update is called once per frame
	void Update () {

        GameObject.Find("character").transform.localScale = (new Vector3(7, 7, 7));
	    if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                /*if (preSelected != null)
                {
                    preSelected.GetComponent<MeshRenderer>().material = UnSelectMaterial;
                }
                hit.transform.GetComponent<MeshRenderer>().material = SelectMaterial;
                Debug.Log('1');*/
                preSelected = hit.transform;
                Debug.Log("select"+preSelected);
                if (hit.collider.gameObject.name.Contains("Plane"))
                {
                    StartCoroutine(rotate90(preSelected, hit.collider.gameObject.name.Contains("B") ? true : false, hit.collider.gameObject.name));
                    if (hit.collider.gameObject.name.Contains(character.cntSurface))
                    {
                        character.bindToCntSurface();
                        character.resetPosition();
                    }
                    //TODO change the sideExit map
                    //WARNING hard code
                    mapController.rotateSurface(hit.collider.gameObject.name[hit.collider.gameObject.name.Length-1]+"");
                    mapController.setPointsPosition();
                    
                }
                else
                {
                    mapController.findRoute(character.transform, character.cntRoad, hit.transform, hit.collider.gameObject.name);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 position = hit.transform.position;
                Debug.Log("right click " + position.x + " " + position.y + " " + position.z);
            }
        }
	}
    IEnumerator rotate90(Transform tgt,bool isReverse,string surface)
    {
        Debug.Log("tgt:"+tgt);
        Plane p = tgt.GetComponent<Plane>();
        Debug.Log("p:" + p);
        isRotating = true;
        if (p != null)
        {
            Quaternion rt = tgt.localRotation;
            Vector3 er = rt.eulerAngles;
            p.circle += 1;
            for (int i = 0; i <= 90; i += 2)
            {
                rt = tgt.localRotation;
                er = rt.eulerAngles;
                er.z += isReverse ? -2 : 2;
                rt.eulerAngles = er;
                tgt.localRotation = rt;
                character.forcePosition(surface);
                yield return null;
            }
            //强制不修改x和y的值
            er.y = p.localY;
            er.x = p.localX;
            //旋转补正
            er.z = p.circle * (isReverse?-90:90);
            rt.eulerAngles = er;
            tgt.localRotation = rt;
            yield return "123";
        }
        Debug.Log("false of rotate");
        character.unbindCntSurface();
        isRotating = false;
    }
}
