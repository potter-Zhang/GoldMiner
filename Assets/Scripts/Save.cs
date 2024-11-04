using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save
{
    public int player_money;
    public int target;
    public int level;
    public int numOfFireCrakers;
    public int win;
    static Save CreateSave()
    {
        Save save = new Save();
        save.player_money = GameManager.instance.player_money;
        save.target = GameManager.instance.goal;
        save.level = GameManager.instance.level;
        save.numOfFireCrakers = GameManager.instance.miner.getNumOfFireCrakers();
        save.win = GameManager.instance.win ? 1 : 0;
        return save;
    }

    static public void saveByJson()
    {
        /*
        Save save = CreateSave();
        string JsonString = JsonUtility.ToJson(save);
        StreamWriter sw = new StreamWriter(Application.dataPath + "Recover.json");
        sw.Write(JsonString);
        sw.Close();
        */
        Save save = CreateSave();

        PlayerPrefs.SetInt("player_money", save.player_money);
        PlayerPrefs.SetInt("target", save.target);
        PlayerPrefs.SetInt("level", save.level);
        PlayerPrefs.SetInt("numOfFireCrakers", save.numOfFireCrakers);
        PlayerPrefs.SetInt("win", save.win);


    }

    static public bool loadByJson()
    {
        /*
        bool exist = File.Exists(Application.dataPath + "Recover.json");
        if (exist)
        {
            StreamReader sr = new StreamReader(Application.dataPath + "Recover.json");
            string JsonString = sr.ReadToEnd();
            sr.Close();
            Save save = JsonUtility.FromJson<Save>(JsonString);
            if (!save.win)
            {
                return false;
            }
            GameManager.instance.win = save.win;
            GameManager.instance.player_money = save.player_money;
            GameManager.instance.goal = save.target;
            if (save.level != 20221225)
                GameManager.instance.level = Mathf.Max(0, Mathf.Min(save.level, 100));
            else
                GameManager.instance.level = 20221225;
            GameManager.instance.numOfFireCrakers = Mathf.Min(5, save.numOfFireCrakers);
            
            
        }

        return exist;
        */
        int win = PlayerPrefs.GetInt("win");
        if (win == 0)
        {
            return false;
        }
        GameManager.instance.win = true;
        GameManager.instance.player_money = PlayerPrefs.GetInt("player_money");
        GameManager.instance.goal = PlayerPrefs.GetInt("target");
        int level = PlayerPrefs.GetInt("level");
        GameManager.instance.level = Mathf.Max(0, Mathf.Min(level, 100));
        GameManager.instance.numOfFireCrakers = Mathf.Min(5, PlayerPrefs.GetInt("numOfFireCrakers"));
        return true;


    }
}
