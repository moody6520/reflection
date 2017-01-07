using UnityEngine;
using System.Collections;

public class CubeControl : MonoBehaviour {
    //   public Vector3 mousePos;
    //// Use this for initialization
    //void Start () {

    //}
    //   IEnumerator onMouseDown()
    //   {
    //       mousePos = Input.mousePosition;
    //       while   (Input.GetMouseButton(0))
    //       {
    //           Vector3 offset = mousePos - Input.mousePosition;
    //           transform.Rotate(Vector3.up * offset.x, Space.World);
    //           transform.Rotate(Vector3.right * offset.y, Space.World);
    //           mousePos = Input.mousePosition;
    //           yield return null;
    //       }
    //   }

    //   // Update is called once per frame
    //   void Update () {
    //       onMouseDown();
    //   }

    Vector3 StartPosition;
    Vector3 previousPosition;
    Vector3 offset;
    Vector3 finalOffset;
    Vector3 eulerAngle;

    public MapController mapController;
    public Character character;
    public GameObject cube;

    bool rotating = false;
    bool isSlide;
    float angle;

    void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPosition = Input.mousePosition;
            previousPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("up and reset pos");
            mapController.setPointsPosition();
            character.resetPosition();
            
        }
        if (Input.GetMouseButton(0))
        {
            offset = Input.mousePosition - previousPosition;
            previousPosition = Input.mousePosition;
            transform.Rotate(Vector3.Cross(offset, Vector3.forward).normalized, offset.magnitude, Space.World);
         


        }
        
    }
}
