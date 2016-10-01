using UnityEngine;
using System.Collections;
using System;

public class CommanderAI : MonoBehaviour {

    public GameObject tower;

    public float delayTimeCreateMob = 5;
    public float delayTimeCreateMob2 = 6;
    public GameObject mob;
    public GameObject mob2;

    public CommanderAI enemy;//командир враг
    public CommanderAI friend;//командир друг


    private float timeLastCreateMob;
    private float timeLastCreateMob2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Если прошло достаточно времени после создания последнего моба
        if (Time.time > timeLastCreateMob + delayTimeCreateMob)
        {
            //создаем моба
            MainScript.CreateMob(this, mob, tower.transform);

            //указываем время создания последнего моба
            timeLastCreateMob = Time.time;
        }

        //Если прошло достаточно времени после создания последнего моба
        if (Time.time > timeLastCreateMob2 + delayTimeCreateMob2)
        {
            //создаем моба
            MainScript.CreateMob(this, mob2, tower.transform);

            //указываем время создания последнего моба
            timeLastCreateMob2 = Time.time;
        }
    }

    //передает позицию вражеской башни
    public Transform GetEnemyTowerTransform()
    {
        return enemy.tower.transform;
    }

}
