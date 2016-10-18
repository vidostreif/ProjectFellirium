using System;
using UnityEngine;
using System.Collections.Generic;

public class MainScript : MonoBehaviour {
    public CommanderAI playerCommander { get; private set; }

    private GameObject[] commanders;
    // Use this for initialization
    void Start () {

        //находим коммандира которым должен управлять игрок
        commanders = GameObject.FindGameObjectsWithTag("Commander");
        foreach (GameObject commader in commanders) //для каждого моба в массиве
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
        Team TeamAI = newMob.GetComponent<Team>();
        TeamAI.commander = commander;

        return newMob;
    }

    //Выбор цели
    public static GameObject TargetSelection(Transform transformCalling, CommanderAI commander, float maxAttackDistance, float minAttackDistance = 0)
    {
        float closestMobDistance = maxAttackDistance; //дистанция до ближайшей цели
        GameObject nearestmob = null; //инициализация переменной ближайшей цели
        //GameObject[] sortingTargets = GameObject.FindGameObjectsWithTag("Mob, "); //находим всех мобов с тегом Mob и создаём массив для сортировки
        PhysicalPerformance[] sortingTargets = FindObjectsOfType(typeof(PhysicalPerformance)) as PhysicalPerformance[]; //находим все объекты с компонентом PhysicalPerformance


        foreach (PhysicalPerformance target in sortingTargets) //для каждой цели в массиве
        {
            float distance = Vector3.Distance(target.transform.position, transformCalling.position);//дистанция до текущей цели

            //если дистанция до цели в указанных пределах, и меньше чем до предыдущей проверенной цели
            if (distance > minAttackDistance && distance < maxAttackDistance && distance < closestMobDistance)
            {
                //Узнаем враг ли он и жив ли
                //PhysicalPerformance targetPhysicalPerformance = target.GetComponent<PhysicalPerformance>();
                //if (targetPhysicalPerformance)
                //{
                    //float MobDistance = Vector3.Distance(mob.transform.position, transformCalling.position); //Меряем дистанцию от цели до пушки, записываем её в переменную

                    if (target.commander == commander.enemy && target.isLive)
                    {
                        closestMobDistance = distance; //дистанция до ближайшей цели
                        nearestmob = target.gameObject;//устанавливаем его как ближайшая
                    }
                //}

            }
        }

        //if (nearestmob == null)//если не нашли мобов в поле зрения, то ищем башню противника
        //{
        //    sortingTargets = GameObject.FindGameObjectsWithTag("Tower"); //находим всех башни
        //}

        return nearestmob; //возвращаем ближайшую цель
    }

    public static List<GameObject> FindObjectsInRadiusWithTag(Vector2 position, float Radius, string tag)//поиск объектов по тегу в радиусе
    {
        List<GameObject> objectsToInteract = new List<GameObject>();//список найденных объектов

        GameObject[] findeObjects = GameObject.FindGameObjectsWithTag(tag); //находим всех объекты с тегом  и создаём массив из них

        foreach (GameObject currentObject in findeObjects) //для каждого объекта в массиве
        {

            float distance = Vector2.Distance(currentObject.transform.position, position);//дистанция до объекта
            if (distance <= Radius)//если объект находиться в радиусе действия
            {               
                //добавляем в список
                objectsToInteract.Add(currentObject);                
            }
        }

        return objectsToInteract;
    }

    
    public static List<GameObject> FindObjectsInRadiusWithComponent(Vector2 position, float Radius, Type component)//поиск объектов по тегу в радиусе
    {
        List<GameObject> objectsToInteract = new List<GameObject>();//список найденных объектов

        Component[] findeObjects = FindObjectsOfType(component) as Component[]; //находим всех объекты с компонентом и создаём массив из них

        foreach (var currentObject in findeObjects) //для каждого объекта в массиве
        {

            float distance = Vector2.Distance(currentObject.transform.position, position);//дистанция до объекта
            if (distance <= Radius)//если объект находиться в радиусе действия
            {
                //добавляем в список
                objectsToInteract.Add(currentObject.gameObject);
                
            }
        }

        return objectsToInteract;
    }
}
