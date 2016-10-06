﻿using System;
using UnityEngine;

public class MainScript : MonoBehaviour {
    public CommanderAI playerCommander { get; private set; }

	// Use this for initialization
	void Start () {

        //находим коммандира которым должен управлять игрок
        GameObject[] commaders = GameObject.FindGameObjectsWithTag("Commander");
        foreach (GameObject commader in commaders) //для каждого моба в массиве
        {
            CommanderAI commaderAI = commader.GetComponent<CommanderAI>();

            if (commaderAI)
            {
                if (commaderAI.managePlayer)//если стоит галка "управляется игроком"
                {
                    playerCommander = commaderAI;
                }
            }
        }

        //если не определили командира сообщаем
        if (playerCommander == null)
        {
            Debug.Log("не удалось определить командира которым должен управлять игрок! Поставьте галку в командире.");
        }
    }

// Update is called once per frame
    void Update () {


    }

    internal static void magicChoice(int magic)
    {
        Debug.Log("Магия пока не работает");
    }

    //процедура создания моба в указанных координатах с указанным командиром
    public static GameObject CreateMob(CommanderAI commander, GameObject mob, Transform transformStartPosition)
    {
        //создаем моба
        GameObject newMob = Instantiate(mob, transformStartPosition.position, transformStartPosition.rotation) as GameObject;
        //newMob.name = mob.name;

        //указываем команду
        MobAI mobAI = newMob.GetComponent<MobAI>();
        mobAI.commander = commander;

        return newMob;
    }

    //Выбор цели
    public static GameObject TargetSelection(Transform transformCalling, CommanderAI commander, float maxAttackDistance, float minAttackDistance = 0)
    {
        float closestMobDistance = maxAttackDistance; //дистанция до ближайшего моба
        GameObject nearestmob = null; //инициализация переменной ближайшего моба
        GameObject[] sortingMobs = GameObject.FindGameObjectsWithTag("Mob"); //находим всех мобов с тегом Mob и создаём массив для сортировки

        foreach (GameObject mob in sortingMobs) //для каждого моба в массиве
        {
            float distance = Vector3.Distance(mob.transform.position, transformCalling.position);//дистанция до текущего моба

            //если дистанция до моба в указанных пределах, и меньше чем до предыдущи проверенного моба
            if (distance > minAttackDistance && distance < maxAttackDistance && distance < closestMobDistance)
            {
                //Узнаем враг ли он и жив ли
                PhysicalPerformance mobPhysicalPerformance = mob.GetComponent<PhysicalPerformance>();
                if (mobPhysicalPerformance)
                {
                    //float MobDistance = Vector3.Distance(mob.transform.position, transformCalling.position); //Меряем дистанцию от моба до пушки, записываем её в переменную

                    if (mobPhysicalPerformance.Commander == commander.enemy && mobPhysicalPerformance.isLive)
                    {
                        closestMobDistance = distance; //дистанция до ближайшего моба
                        nearestmob = mob;//устанавливаем его как ближайшего
                    }
                }

            }
        }
        return nearestmob; //возвращаем ближайшего моба
    }
}
