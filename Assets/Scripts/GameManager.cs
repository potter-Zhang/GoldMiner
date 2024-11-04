using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public LevelCreator levelCreator;
    public GameObject startpage;
    public GameObject endpage;
    public TMP_Text specialPage;
   
    public GameObject eventSystem;

    public int player_money;
    public int player_time;
    //public Text valueEffect;

    private bool onPlay;
    public bool win;
    
    public int level = 1;
    public int goal;
    private int adder;
    private int lastID;
    //private int goldenClock;
    private bool firstTime;

    private GameObject shop;
    private Text money;
    private Text time;
    private Text target;
    //private Text goalDisp;
    private Text explanation;
    public Miner miner;
    private RectTransform pointer;
    private bool fromSave;
    public int numOfFireCrakers;
    
    private float timer;
    private string[] myWords = { "���ͣ��������Լ���", "���¿�ʼ�ɣ��»�һ����", "�벻Ҫ����", "����Ϸ���е�����"};
    private string[] tips = { "��ϲ���أ�\nը��ֻ����ץ��ʯͷ��ʱ�������", "��ϲ���أ�\n���˲ݿ����ó�ɫ���Ӹ���ֵǮ", 
        "��ϲ���أ�\n�������˲��Ժ��ɫ����������ܻ���ֵ���", "��ϲ���أ�\nը���ͷ���ҩˮһ��ʹ��Ч������", 
        "��ϲ����\n������ˮ�ͻƽ�ʱ�Ӳ���ͬʱ����", "��ϲ���أ�\n�ڵ�ը�������кô���", "��ϲ���أ�\n����ҩˮÿ���زŻ����һ��",
        "��ϲ����\nը������������һ����Ŀ�Ժ�Ͳ��ܹ�����"};
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
        instance.firstTime = true;
        instance.lastID = 0;
        instance.player_time = 0;
        instance.player_money = 0;
        instance.level = 1;
        instance.goal = 0;
        instance.adder = 700;
        //instance.goldenClock = 0;
        instance.fromSave = false;
        instance.startpage = Instantiate(instance.startpage, Vector3.zero, Quaternion.identity);
        instance.endpage = Instantiate(instance.endpage, Vector3.zero, Quaternion.identity);
        instance.endpage.SetActive(false);
        instance.eventSystem = Instantiate(instance.eventSystem, Vector3.zero, Quaternion.identity);
    }

    public void StartGame()
    {
        Debug.Log("startgame");
        //instance.endpage.SetActive(false);
        instance.StartCoroutine(GoalDisplay());
        
        
    }

    IEnumerator GoalDisplay()
    {
        instance.startpage.transform.GetChild(0).gameObject.SetActive(false);
        instance.startpage.transform.GetChild(1).gameObject.SetActive(false);
        instance.startpage.transform.GetChild(3).gameObject.SetActive(false);

        var refGoal = instance.startpage.transform.GetChild(2).gameObject;
        Text goalDisp = refGoal.transform.GetChild(0).gameObject.GetComponent<Text>();
        instance.adder = 700 + 100 * (instance.level / 20) + 70 * (instance.level % 20 == 0 ? 20 : (instance.level % 20 - 1));
        //instance.adder = 0;
        instance.goal += instance.adder;
        Debug.Log("goal");
        goalDisp.text = "Ŀ�� " + instance.goal;
        refGoal.SetActive(true);
        instance.startpage.SetActive(true);
        yield return new WaitForSeconds(3);
        refGoal.SetActive(false);
        instance.startpage.SetActive(false);
        instance.InitGame();
    }

    void InitGameAux()
    {
        if (instance.firstTime)
        {
            instance.levelCreator = Instantiate(instance.levelCreator, Vector3.zero, Quaternion.identity);
            instance.money = GameObject.Find("LevelText").GetComponent<Text>();
            instance.time = GameObject.Find("LevelTime").GetComponent<Text>();
            instance.miner = GameObject.FindGameObjectWithTag("Miner").GetComponent<Miner>();
            instance.pointer = GameObject.Find("PointerImage").GetComponent<RectTransform>();
            instance.target = GameObject.Find("TargetText").GetComponent<Text>();
            
            instance.firstTime = false;
        }
    }
    public void InitGame()
    {
        instance.InitGameAux();
        instance.levelCreator.Show();
        instance.levelCreator.CreateLevel();
        instance.miner.Reset();


        instance.player_time += 60;//
        
        instance.target.text = " " + instance.goal + "(�ؿ� " + instance.level + ")";
        instance.onPlay = true;
    }

    public void StrengthEffect()
    {
        if (instance.lastID == 1 && instance.player_money >= 500)
        {
            instance.miner.strength = 2.5f;
            instance.player_money -= 500;
            instance.shop.transform.GetChild(0).gameObject.SetActive(false);
            //GameObject.Find("Strength").SetActive(false);
        }
        instance.explanation.text = "��" + instance.player_money + "\n������ˮ���ӿ����ץȡ�ٶȣ�"; 
        instance.lastID = 1;
        Debug.Log("strength");
    }

    public void DoubleMagicEffect()
    {
        if (instance.lastID == 4 && instance.player_money >= 888)
        {
            instance.miner.doubleMagic = 2;
            instance.player_money -= 888;
            instance.shop.transform.GetChild(3).gameObject.SetActive(false);
            //GameObject.Find("DoubleMagic").SetActive(false);
        }
        instance.explanation.text = "��" + instance.player_money + "\n����ҩˮ��������Ʒ��ֵ������";
        instance.lastID = 4;
        Debug.Log("doublemagic");
    }

    public void GoldenClockEffect()
    {
        if (instance.lastID == 5 && instance.player_money >= 500)
        {
            //instance.goldenClock = true;
            instance.player_time += 10;
            instance.player_money -= 500;
            instance.shop.transform.GetChild(4).gameObject.SetActive(false);
        }
        instance.explanation.text = "��" + instance.player_money + "\n�ƽ�ʱ�ӣ��Ӷ�10�룡";
        instance.lastID = 5;
        Debug.Log("goldclock");
    }

    public void LuckyPlantEffect()
    {
        if (instance.lastID == 2 && instance.player_money >= 400)
        {
            instance.levelCreator.LuckyEffect();
            instance.player_money -= 400;
            instance.shop.transform.GetChild(1).gameObject.SetActive(false);
        }
        instance.explanation.text = "��" + instance.player_money + "\n���˲ݣ��������������";
        instance.lastID = 2;
        Debug.Log("luckyplant");
    }

    public void FireCrakerEffect()
    {
        if (instance.lastID == 3 && instance.player_money >= 200)
        {
            instance.miner.AddFireCraker();
            instance.player_money -= 200;
            instance.shop.transform.GetChild(2).gameObject.SetActive(false);
        }
        instance.explanation.text = "��" + instance.player_money + "\nըҩ����ʯͷ��ը���ƽ���ʯ��";
        instance.lastID = 3;
        Debug.Log("firecraker");
    }

    public void Pause()
    {
        instance.onPlay = !instance.onPlay;
        instance.levelCreator.Pause(!instance.onPlay);
        instance.miner.Pause(!instance.onPlay);
        //Input.touchCount = 0;
        Debug.Log("pauer");
    }

    public void Shoot()
    {
        instance.miner.GetUserInput();
    }

    IEnumerator EndGame()
    {
        instance.miner.doubleMagic = 1;
        instance.miner.strength = 1;
        instance.levelCreator.Clear();
        instance.lastID = 0;
        var victory = instance.endpage.transform.GetChild(0).gameObject;
        Debug.Log(victory.name);
        var loss = instance.endpage.transform.GetChild(1).gameObject;
        Debug.Log(loss.name);
        instance.shop = instance.endpage.transform.GetChild(2).gameObject;
        bool flip = instance.level % 2 == 1;
        instance.shop.transform.GetChild(0).gameObject.SetActive(flip);
        instance.shop.transform.GetChild(1).gameObject.SetActive(true);
        instance.shop.transform.GetChild(2).gameObject.SetActive(instance.miner.getNumOfFireCrakers() <= 5);
        instance.shop.transform.GetChild(3).gameObject.SetActive(instance.level % 6 == 0);
        instance.shop.transform.GetChild(4).gameObject.SetActive(!flip);

        instance.shop.SetActive(false);
        Debug.Log("endgame") ;

        if (!instance.fromSave)
        {
            var vicText = victory.transform.GetChild(0).gameObject.GetComponent<Text>();
            vicText.text = tips[Random.Range(0, tips.Length)];
            victory.SetActive(win);

            loss.SetActive(!win);
            instance.endpage.SetActive(true);
            yield return new WaitForSeconds(3);
        } 
        else
        {
            victory.SetActive(false);
            loss.SetActive(false);
            instance.fromSave = false;
            instance.endpage.SetActive(true);
            Debug.Log("fromsave");
        }
        
        
        
        Save.saveByJson();
        Debug.Log("aftersave");
        if (instance.win)
        {
            Debug.Log("inwin");
            if (instance.level == 100)
            {
                var restart = loss.transform.GetChild(1).gameObject;
                restart.SetActive(false);
                loss.SetActive(true);
                var finalText = loss.transform.GetChild(0).gameObject.GetComponent<Text>();
                finalText.fontSize = 25;
                finalText.text = "��ϲ��ͨ���ˣ�\n������շ�����: " + instance.player_money + " ���浽�˵�" + instance.level + "��";
                finalText.text += "\n��������[��ݵ�����Ͷ�صı���]\n������Խ��ó̵��յ���~\n������������˧��������little prince������";
                yield return new WaitForSeconds(3);
                loss.SetActive(false);
                var special = Instantiate(instance.specialPage, Vector3.zero, Quaternion.identity);

            }
            else
            {
                Debug.Log("there");
                victory.SetActive(false);
                instance.shop.SetActive(true);
                Debug.Log("herer");
                
                instance.explanation = instance.shop.transform.GetChild(5).gameObject.GetComponent<Text>();
                instance.explanation.text = "��" + instance.player_money;
                Debug.Log("herer");
                Debug.Log(instance.explanation.text);
            }
            
        }  
        else
        {
            Debug.Log("inloss");
            var finalText = loss.transform.GetChild(0).gameObject.GetComponent<Text>();
            finalText.fontSize = 30;
            finalText.text = "��Ŷ����ʧ����:(\n������շ�����: " + instance.player_money + "\n���浽�˵�" + instance.level + "��\n";
            finalText.text += myWords[Random.Range(0, myWords.Length)];

        }
        
        
    }

    public void Restart()
    {  
        SceneManager.LoadScene("GameScene");
    }

    public void Reset()
    {
        instance.endpage.SetActive(false);
        instance.level++;
        instance.StartGame();
    }

    public void Continue()
    {
        if (Save.loadByJson())
        {
            instance.startpage.transform.GetChild(0).gameObject.SetActive(false);
            instance.startpage.transform.GetChild(1).gameObject.SetActive(false);
            instance.startpage.transform.GetChild(2).gameObject.SetActive(false);
            instance.startpage.transform.GetChild(3).gameObject.SetActive(false);
            instance.startpage.SetActive(false);
            if (instance.level == 20221225)
            {
                Instantiate(instance.specialPage, Vector3.zero, Quaternion.identity);
                instance.GetComponent<Camera>().backgroundColor = new Color(34 / 255f, 139 / 255f, 34 / 255f, 0.3f);
                return;
            }
            instance.fromSave = true;
            instance.InitGameAux();
            for (int i = 0; i < instance.numOfFireCrakers; i++)
                instance.miner.AddFireCraker();
            instance.levelCreator.Clear();
            Debug.Log("continue");
            instance.StartCoroutine(EndGame());
        }
           
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        
            
        if (!instance.onPlay)
            return;
        instance.timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
        if (instance.timer >= 1)
        {
            instance.timer = 0;
            instance.player_time--;
            instance.pointer.rotation = Quaternion.Euler(0, 0, instance.player_time * 6);
            if (instance.player_time <= 0)
            {
                onPlay = false;
                win = instance.player_money >= instance.goal;
                StartCoroutine(EndGame());
                    
               
            }
        }
        //Debug.Log("money + time");
        instance.money.text = " " + player_money;
        //Debug.Log("money = " + instance.money.text);
        instance.time.text = " " + player_time;
        //Debug.Log("time = " + instance.time.text);

    }
}


