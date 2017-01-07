using UnityEngine;
using System.Collections;

public class Level1GetMouse : MonoBehaviour {
    Transform preSelected = null;
    bool isRotating = false;
    void Start()
    {

    }
    public MapController mapController;
    public Character character;
    //public Material SelectMaterial;
    //public Material UnSelectMaterial;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                /*if (preSelected != null)
                {
                    preSelected.GetComponent<MeshRenderer>().material = UnSelectMaterial;
                }
                hit.transform.GetComponent<MeshRenderer>().material = SelectMaterial;
                Debug.Log('1');*/
                preSelected = hit.transform;
                Debug.Log("select" + preSelected);
                if (hit.collider.gameObject.name.Contains("Plane"))
                {
                    StartCoroutine(rotate120(preSelected));
                    character.bindToCntSurface();
                    //TODO change the sideExit map
                    //WARNING hard code
                    mapController.rotateSurface(hit.collider.gameObject.name[hit.collider.gameObject.name.Length - 1] + "");
                    mapController.setPointsPosition();
                    character.resetPosition();
                }
                else
                {
                    /*List<double> tarPos = new List<double>();
                    tarPos.Add(hit.transform.position.x);
                    tarPos.Add(hit.transform.position.y);
                    tarPos.Add(hit.transform.position.z);*/
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
    IEnumerator rotate120(Transform tgt)
    {
        Debug.Log("tgt:" + tgt);
        Level1Plane p = tgt.GetComponent<Level1Plane>();
        Debug.Log("p:" + p);
        isRotating = true;
        if (p != null)
        {
            Quaternion rt = tgt.localRotation;
            Vector3 er = rt.eulerAngles;
            p.circle += 1;
            for (int i = 0; i <= 120; i += 2)
            {
                rt = tgt.localRotation;
                er = rt.eulerAngles;
                er.z += 2;
                rt.eulerAngles = er;
                tgt.localRotation = rt;
                yield return null;
            }
            //强制不修改x和y的值
            er.y = p.localY;
            er.x = p.localX;
            //旋转补正
            er.z = p.startz + p.circle * 120;
            rt.eulerAngles = er;
            tgt.localRotation = rt;
        }
        Debug.Log("false of rotate");
        character.unbindCntSurface();
        isRotating = false;
    }
}
