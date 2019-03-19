using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public GameObject BotPrefab;
    public GameObject Target;

    private bool isTraning = false;
    private int populationSize = 50;
    private int generationNumber = 0;
    private int[] layers = new int[] { 7, 10, 10, 1 }; 
    private List<NeuralNetwork> nets;
    private List<NeuralNetwork> BestNets;
    public List<BOT> BotList = null;
    private bool isBest = false;

    void Timer()
    {
        isTraning = false;
        BroadcastMessage("GenDead");
    }


    void FixedUpdate()
    {
        if (isTraning == false)
        {
            if (generationNumber == 0)
            {
                InitBoomerangNeuralNetworks();
            }
            else if(!isBest)
            {
                nets.Sort();
                for (int i = 0; i < populationSize / 2; i++)
                {
                    nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                    nets[i].Mutate();
                    nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); 
                }

                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            generationNumber++;
            isTraning = true;
            Invoke("Timer", 5f);
            CreateBotBodies();
        }


      
    }

    private void CreateBotBodies()
    {
        if (BotList != null)
        {
            for (int i = 0; i < BotList.Count; i++)
            {
                GameObject.Destroy(BotList[i].gameObject);
            }

        }

        BotList = new List<BOT>();

        for (int i = 0; i < populationSize; i++)
        {
            BOT boomer = ((GameObject)Instantiate(BotPrefab, Vector3.zero , BotPrefab.transform.rotation)).GetComponent<BOT>();
            if(!isBest)
                boomer.Init(nets[i], Target.transform);
            else
            {
                boomer.Init(BestNets[i], Target.transform);
            }
            boomer.name = "bot" + i;
            BotList.Add(boomer);
        }

    }

    void InitBoomerangNeuralNetworks()
    {
        //population must be even
        if (populationSize % 2 != 0)
        {
            populationSize = 20;
        }

        nets = new List<NeuralNetwork>();
        BestNets = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
            BestNets.Add(net);
        }
    }

    public int GetNumberGen()
    {
        return generationNumber;
    }

    public void SetBestTry()
    {
        for(int i = 0; i < populationSize; i++)
            BestNets[i] = new NeuralNetwork(nets[i]);
    }

    public void ShowBest()
    {
        isBest = !isBest;
    }
}
