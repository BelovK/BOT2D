using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
    

public class UI : MonoBehaviour
{
    public Text LR, CR, RR; // Вхожные значения
    public Text Output; // Вхожные значения
    public Text Gen, Fit;
    public Text SumFit, MaxSumFit;
    private bool ShowBest = false;
    private BOT CurBot;
    private Manager manager;
    private LineRenderer Line;
    private bool OpenG = false;
    float f = 0,maxf = 0;
    Color BG = Color.black;
    Color WG = Color.white;
    public Collider2D Walk;
    void Start ()
    {
       manager = GetComponent<Manager>();
       Line = GetComponentInChildren<LineRenderer>();
       Camera.main.backgroundColor = BG;
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        
        float maxFit = 0;
        f = 0;
        //float maxf = 0;
        for (int i = 0; i < manager.BotList.Count; i++)
        {

            f += manager.BotList[i].GetFitness();
            if (manager.BotList[i].GetFitness() > maxFit)
            {
                if (CurBot != null)
                    CurBot.GetComponent<SpriteRenderer>().color = Color.white;
                maxFit = manager.BotList[i].GetFitness();
                CurBot = manager.BotList[i];
            }
            //Show or hide best bots
            if (ShowBest && i < manager.BotList.Count / 2)
            {
                manager.BotList[i].GetComponent<SpriteRenderer>().enabled = false;
                Debug.Log("Hide");
                SpriteRenderer[] sr = manager.BotList[i].GetComponentsInChildren<SpriteRenderer>();
                foreach (var s in sr)
                {
                    s.enabled = false;
                }
            }
            else if (!ShowBest && manager.BotList[i].GetStatus())
            {
                manager.BotList[i].GetComponent<SpriteRenderer>().enabled = true;
                SpriteRenderer[] sr = manager.BotList[i].GetComponentsInChildren<SpriteRenderer>();
                foreach (var s in sr)
                {
                    s.enabled = true;
                }
            }
        }
        

        SumFit.text = f.ToString();
        //MaxSumFit.text = maxf.ToString();
        Fit.text = maxFit.ToString();
        if (CurBot != null)
        {
            CurBot.GetComponent<SpriteRenderer>().color = Color.green;
            LR.text = CurBot.GetLR().distance.ToString();
            CR.text = CurBot.GetFR().distance.ToString();
            RR.text = CurBot.GetRR().distance.ToString();
            Output.text = CurBot.GetOut().ToString();
            Gen.text = manager.GetNumberGen().ToString();
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GenDead()
    {
        if (f > maxf)
        {
            manager.SetBestTry();
            maxf = f;
            MaxSumFit.text = maxf.ToString();
        }
        int num = manager.GetNumberGen();
        if (num > Line.gameObject.GetComponent<RectTransform>().offsetMin.x * -10)
        {
            Line.gameObject.GetComponent<RectTransform>().offsetMin -= new Vector2(1,0);
        }
        Line.positionCount++;
        Line.SetPosition(num,new Vector3(num,float.Parse(SumFit.text)/5));
    }

    public void ShowBestSwitch()
    {
        ShowBest = !ShowBest;
    }
    public void OpenGraph()
    {
        if(!OpenG)
            Line.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-Line.gameObject.GetComponent<RectTransform>().offsetMin.x, Line.gameObject.GetComponent<RectTransform>().localPosition.y);
        else
            Line.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(Line.gameObject.GetComponent<RectTransform>().offsetMin.x - Line.gameObject.GetComponent<RectTransform>().offsetMin.x, Line.gameObject.GetComponent<RectTransform>().localPosition.y);
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void ColorSwitch()
    {
        if (Camera.main.backgroundColor == BG)
            Camera.main.backgroundColor = WG;
        else
        {
            Camera.main.backgroundColor = BG;
        }
    }

    public void ShowBestTry()
    {
        manager.ShowBest();
    }

    public void TurnDinamic()
    {
        Walk.enabled = !Walk.enabled;
        Walk.gameObject.GetComponent<SpriteRenderer>().enabled =
            !Walk.gameObject.GetComponent<SpriteRenderer>().enabled;
    }
}
