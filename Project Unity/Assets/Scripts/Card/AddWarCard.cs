using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddWarCard : MonoBehaviour
{
    [System.Serializable]
    public struct WarParameters
    {
        public GameObject mob;
        public int number;
    }

    public WarParameters[] wars;

    //private bool active = false;
    //private float TimeActivate;
    //private CommanderAI commander;

    //void Start()
    //{
    //    commander = GetComponent<Card>().commander;
    //}

    void Update()
    {
        //if (active)
        //{
        //    Vector3 towerPosition = commander.tower.transform.position;
        //    //создаем мобов из списка 
        //    foreach (var war in wars) //для каждого объекта в массиве мобов
        //    {
        //        //создаем моба
        //        GameObject mob = (GameObject)Instantiate(war.mob, commander.tower.transform.position, Quaternion.identity);
        //        MobAI mobAI = mob.GetComponent<MobAI>();
        //        if (mobAI)
        //        {

        //        }
        //        else
        //        {
        //            Debug.Log("У мобе не найден компонент MobAI");
        //        }
        //    }
        //}

    }

    public void Activate()
    {
        //active = true;
        //TimeActivate = Time.time;

        CommanderAI commander = GetComponent<Card>().commander;//командир карты
        Vector3 towerPosition = commander.tower.transform.position;//позиция башни командира

        //создаем мобов из списка 
        foreach (var war in wars) //для каждого объекта в массиве мобов
        {
            for (int i = 0; i < war.number; i++)
            {
                //создаем моба
                GameObject mob = (GameObject)Instantiate(war.mob, new Vector3(Random.Range(towerPosition.x - 2, towerPosition.x + 2), towerPosition.y, towerPosition.z), Quaternion.identity);
                MobAI mobAI = mob.GetComponent<MobAI>();
                if (mobAI)
                {
                    mobAI.commander = commander;
                }
                else
                {
                    Debug.Log("У мобе не найден компонент MobAI");
                }
            }
        }
    }
}


