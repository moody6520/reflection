using UnityEngine;
using System.Collections;

public class player_control : MonoBehaviour {
    public GameObject m_Man;
    //public GameObject m_Woman;
    float m_RunSpeed = 1.0f;
    //攻击间隔
    PersonDirection m_Direction = PersonDirection.Left;
    enum PersonDirection
    {
        //正常向前  
        Forward = 270,
        //正常向后  
        Backward = 90,
        //正常向左  
        Left = 0,
        //正常向右  
        Right = 180,
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }
    void FixedUpdate()
    {

        move();
    }
    void move()
    {
            if (Input.GetKey(KeyCode.S))
            {
                SetPersonDirection(PersonDirection.Backward);
                SetPersonAnimation();
            }
            //后退  
            if (Input.GetKey(KeyCode.W))
            {
                SetPersonDirection(PersonDirection.Forward);
                SetPersonAnimation();
            }
            //向左  
            if (Input.GetKey(KeyCode.D))
            {
                SetPersonDirection(PersonDirection.Right);
                SetPersonAnimation();
            }
            //向右  
            if (Input.GetKey(KeyCode.A))
            {
                SetPersonDirection(PersonDirection.Left);
                SetPersonAnimation();
            }

        //等待  
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
            m_Man.GetComponent<Animation>().wrapMode = WrapMode.Once;
            m_Man.GetComponent<Animation>().CrossFade("walkend", 0.1f);
        }
    }
    private void SetPersonDirection(PersonDirection mDir)
    {
        //根据目标方向与当前方向让角色旋转  
        if (m_Direction != mDir)
        {
            transform.Rotate(Vector3.up * (m_Direction - mDir));
            m_Direction = mDir;
        }
    }

    private void SetPersonAnimation()
    {
        m_Man.GetComponent<Animation>().wrapMode = WrapMode.Loop;
        m_Man.GetComponent<Animation>().CrossFade("walkloop");
        transform.Translate(Vector3.forward * m_RunSpeed * Time.deltaTime);

    }
}
