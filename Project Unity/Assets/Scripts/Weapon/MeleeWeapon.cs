using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour {
    [Header("Редактирование атрибутов:")]
    public int attackPause;
    public int damage;
    public float attackDistance;

    private Transform thisTransform;
    private PhysicalPerformance thisPhysicalPerformance;
    private float timeLastAttack = 0;
    private float timeLastSearch = 0;//время последнего вызова функции поиска цели

    //public CommanderAI commander { get; private set; }
    public Team team { get; private set; }//наша команда
    public GameObject target;

    // Use this for initialization
    void Start()
    {
        thisPhysicalPerformance = GetComponent<PhysicalPerformance>();
        thisTransform = GetComponent<Transform>();
        //commander = GetComponent<Team>().commander;
        team = GetComponent<Team>();

        //добовляем небольшой рандов в дистанцию атаки моба для красоты
        attackDistance = Random.Range(attackDistance * 0.99f, attackDistance * 1.01f);
    }

    void FixedUpdate()
    {
        if (thisPhysicalPerformance.isLive)
        {
            //ищем цель 10 раз в секунду
            if (Time.time > timeLastSearch + 0.2f)
            {
                target = MainScript.TargetSelection(thisTransform, team.commander, attackDistance);
                timeLastSearch = Time.time;
            }

            //если прошло время после последней атаки больше чем attackPause
            if (Time.time > timeLastAttack + attackPause)
            {
                //target = MainScript.TargetSelection(transform, commander, attackDistance);

                if (target != null)
                {
                    Attack(target);

                    ////если убили цель, то сразу ищем следующую
                    //if (!target.GetComponent<PhysicalPerformance>().isLive)
                    //{
                    //    target = MainScript.TargetSelection(transform, commander, attackDistance);
                    //}

                    timeLastAttack = Time.time;
                }

            }
        }
    }

    public void Attack(GameObject target)
    {
        //if (Time.time > timeLastAttack + attackPause)
        //{
            //берем скрипт physicalPerformance если он есть, иначе возвращается fallse
            PhysicalPerformance otherPhysicalPerformance = target.GetComponent<PhysicalPerformance>();
            if (otherPhysicalPerformance)//Если physicalPerformance есть
            {
                //наносим дамаг
                otherPhysicalPerformance.SetPhysicalDamage(damage);
            }
        //}
    }

}
