using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue
{
    public string text;
    //0 boy 1 girl 2 NPC 3 stop
    public int person;
    

    public Dialogue(string t, int p)
    {
        text = t;
        person = p;
    }
}

public class Section
{
    public Section(Dialogue[] t) { texts = t; }
    public Dialogue[] texts;
}

public class UIControl : MonoBehaviour
{
    public Camera m_ManCamera;

    public GameObject m_GameControl;
   

    // Use this for initialization  
    void Start()
    {
        
        Person_Status = 0;
        Canvas_Status = 1;
        Element_Status = 0;
        dia_index = 8;
        is_Talk = true;

        //GameObject[] gm;
        //gm = GameObject.FindGameObjectsWithTag("ManMonster");


        //for (int i = 0; i < gm.Length; i++)
        //{
        //    gm[i].GetComponent<Monster>().isPause = true;
        //    Debug.Log(i);
        //}

        //m_gc.PauseMonster("Man");
        //m_gc.PauseMan();

        book_index = 0;
        book_collect = 6;
        section_index = 0;
        Dia_Start();
    }

    // Update is called once per frame  
    void Update()
    {
        //if (is_Talk)  Dialogue_Control();
        //if (Canvas_Status == 2) Book_Control();
    }

    #region Change Status

    public GameObject Girl_Head;
    public GameObject Boy_Head;
    public GameObject Girl;
    public GameObject Boy;



    public GameObject Fire;
    public GameObject Water;
    public GameObject Life;

    public float m_MaxHealth = 100.0f;
    public float m_CurHealth = 100.0f;

    //0 boy;1 girlGameObject
    private int Person_Status;
    //status = 0 main;1 game;2 book;3 set;
    private int Canvas_Status;
    //Element 0 Null,1 fire,2 water,3 life;
    private int Element_Status;

    public void Change_Canvas(int c) {
        Canvas_Status = c;
    }

    public void Change_Element(int e)
    {
        if (e == Element_Status)
            return;
        switch (Element_Status)
        {
            case 1:
                Fire.SetActive(false);
                break;
            case 2:
                Water.SetActive(false);
                break;
            case 3:
                Life.SetActive(false);
                break;
            default:
                break;
        }
        switch (e)
        {
            case 1:
                Fire.SetActive(true);
                Water.SetActive(false);
                Life.SetActive(false);
                break;
            case 2:
                Fire.SetActive(false);
                Water.SetActive(true);
                Life.SetActive(false);
                break;
            case 3:
                Fire.SetActive(false);
                Water.SetActive(false);
                Life.SetActive(true);
                break;
            default:
                Fire.SetActive(false);
                Water.SetActive(false);
                Life.SetActive(false);
                break;
        }
        Element_Status = e;
    }

    public void Change_Person()
    {
        if (Person_Status == 0)
        {
            Person_Status = 1;
            Girl_Head.SetActive(true);
            Boy_Head.SetActive(false);
        }
        else
        {
            Person_Status = 0;
            Girl_Head.SetActive(false);
            Boy_Head.SetActive(true);
        }
    }

    #endregion

    #region Blood

    public Texture2D HealthBg;
    public Texture2D Heathforce;
    public GameObject Man;

    void OnGUI()
    {
        if (Canvas_Status == 1)
            Health_Render();
    }

    public void GetDamage(float Damage)
    {
        m_CurHealth -= Damage;
    }
    void Health_Render()
    {
        if (Event.current.type != EventType.Repaint)
        {
            return;
        }
        Rect rectbg = new Rect(185, 60, 250, 54);

        GUI.DrawTexture(rectbg, HealthBg);

        float width = (m_CurHealth * 250.0f) / m_MaxHealth;

        if (width < 1) return;

        Rect rectfc = new Rect(186, 58, width, 58);
        GUI.DrawTexture(rectfc, Heathforce);


    }

    #endregion

    #region Dialogue System

    public Dialogue[][] m_Dialogue_all;


    public Text Dia_Text;
    public GameObject Dia_canvas;

    //dialogue index
    private int dia_index;
    private int section_index;
    public bool is_Talk;

    //e 0 按O键 1 点火 2 河面 3 NPC 4 日记 5 走到山洞前
    public void Begin_Dia(int e)
    {
        section_index = e;
        dia_index = 0;
        is_Talk = true;

        Dia_canvas.SetActive(true);
        Dialogue_Control();
    }

    
    public void Dialogue_Control()
    {
        Dialogue dialogue = m_Dialogue_all[section_index][dia_index];
        int tmp = dialogue.person;
        if (tmp == 3)
        {
            Dia_canvas.SetActive(false);
            is_Talk = false;

            //return;
        }
        else if (tmp == 2)
        {
            Boy.SetActive(false);
            Girl.SetActive(false);
        }
        else if (tmp == 0)
        {
            Boy.SetActive(true);
            Girl.SetActive(false);
        }
        else
        {
            Boy.SetActive(false);
            Girl.SetActive(true);
        }

        Dia_Text.text = dialogue.text;
    }

    public void Dialogue_Next() { Debug.Log("yes"); dia_index++; Dialogue_Control(); }

    void Dia_Start()
    {
        m_Dialogue_all = new Dialogue[9][];
        m_Dialogue_all[0] = new Dialogue[] {
           new Dialogue("杰克从昏迷中醒过来的时候，天色已经大白，他环顾四周，是一处荒无人烟的野外，周围空无一人。",2),
           new Dialogue("这里是哪里？我怎么会在这里？",0),
           new Dialogue("你能听见我吗？",1),
           new Dialogue("是谁？是谁在说话？",0),
           new Dialogue("是我，露丝，我是你的未婚妻，你大概已经忘记我了。我们出了一场事故，在这场事故中，我死了，你失忆了。",1),
           new Dialogue("你死了？",0),
           new Dialogue("是的，我现在是鬼魂，你看不见我。按F键，就能切换到我的视角。",1),
           new Dialogue(" ",3)
        };
        m_Dialogue_all[1] = new Dialogue[] {
           new Dialogue("我这里很暗，你在你的世界里点火，我这里就可以亮起来。使用F键，可以吸收怪物的技能，使用F键，可以释放技能。对灯释放火焰，就可以点火了。",1),
           new Dialogue(" ",3)
        };
        m_Dialogue_all[2] = new Dialogue[] {
           new Dialogue("好的，这里亮了。曾经上古有传说道，只要能找到鬼神，就能死而复生。我要去找鬼神恢复这一切，你愿不愿意与我一同去？",1),
           new Dialogue("鬼神？……你是我的未婚妻，我应该帮你的。不过我们怎么找到他呢？",2),
           new Dialogue("鬼神位东，去往前方的山洞吧。这一路上多有鬼怪，如果你疲累了，换回我的视角，我会帮你吸引他们的。",1),
           new Dialogue(" ",3)
        };
        m_Dialogue_all[3] = new Dialogue[] {
           new Dialogue("这条河上的桥已经断了，看来需要把河冻住。不过我这里并没有冰系怪物，这可怎么办呢？",0),
           new Dialogue(" ",3)
        };
        m_Dialogue_all[4] = new Dialogue[] {
            new Dialogue("我现在需要释放冰系法术",1),
            new Dialogue(" ",3)
        };
        m_Dialogue_all[7] = new Dialogue[] {
           new Dialogue("你来了。",2),
           new Dialogue("你是谁？",1),
           new Dialogue("你不需要认识我，从这里往回走，才是你应该走的路。",2),
           new Dialogue("不，我一定会活过来。",1),
           new Dialogue("纵使牺牲对你很重要的东西也不停止？",2),
           new Dialogue("……",1),
           new Dialogue("回去吧",2),
           new Dialogue("我，绝不停止。",1),
           new Dialogue(" ",3)
        };
        m_Dialogue_all[5] = new Dialogue[] {
           new Dialogue("这是什么？我的日记？为什么我的日记会在这里？算了，先收起来吧",0),
           new Dialogue(" ",3)
        };
        m_Dialogue_all[6] = new Dialogue[] {
           new Dialogue("前面就是山洞了",0),
           new Dialogue("鬼神应该就在山洞后面。走吧",1),
           new Dialogue(" ",3)
        };
        m_Dialogue_all[8] = new Dialogue[] {
           new Dialogue("噢，看看这是谁，又是来讨反生的钥匙的吗？",2),
           new Dialogue("……我的未婚妻死了，我要让她活过来。",0),
           new Dialogue("死或者活，都不是容易的事情。你想要她活，就要和我赌博。",2),
           new Dialogue("我赌。",0),
           new Dialogue("痛快！那么我给与你特别的优待，你同我的幻影战斗一番，若你赢了，我便让她活，若你死了，我便收回你二人的性命，何如？",2),
           new Dialogue("来吧！",0),
           new Dialogue("杰克！",1),
           new Dialogue("露丝？",0),
           new Dialogue("此战凶险，我会把我所拥有的技能传送给你，你可以使用技能与之一战。",1),
           new Dialogue("好。",0),
           new Dialogue(" ",3)
        };
        Begin_Dia(0);
    }

    #endregion

    #region memory book

    public Text Book_Text;

    public Text Collect_Text;

    private int book_index;

    private int book_collect;

    const int Max_Book = 6;

    #region text

    private string[] Book_text = {
        "“遇见了一个怪人。”",
        "“我好像想起了过去的事情。”",
        "“两人一同活了过来。”",
        "“两人共同坠入地狱。”",
        "“无限循环的世界”",
        "“诱骗。”",
    };

    #endregion

    public void Set_Book_Index(int b)
    {
        book_index = b;
    }

    public void Book_Control()
    {
        Book_Text.text = Book_text[book_index];
        Collect_Text.text = book_collect + " / " + Max_Book;
    }
    #endregion
   
}