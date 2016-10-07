using System;
using UnityEngine;

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
        MobAI mobAI = newMob.GetComponent<MobAI>();
        mobAI.commander = commander;

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

                    if (target.Commander == commander.enemy && target.isLive)
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

    //public CommanderAI Findcommander()
    //{        
    //    //и ищем своего командира
    //    foreach (GameObject commander in commaders)
    //    {
    //        CommanderAI commanderAI = commander.GetComponent<CommanderAI>();
    //        if (commanderAI)
    //        {
    //            if (commanderAI.tower == this.gameObject)//если у командира указана эта башня 
    //            {
    //                commaders = commanderAI;
    //            }
    //        }
    //    }
    //}
}
