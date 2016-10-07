using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour {
    [Header("Редактирование атрибутов:")]
    [SerializeField] private float attackPause;
    [SerializeField] private float damage;
    [SerializeField] private float attackDistance;

    private Transform thisTransform;
    private PhysicalPerformance thisPhysicalPerformance;
    private float timeLastAttack = 0;

    public CommanderAI commander { get; private set; }
    public GameObject target;

    // Use this for initialization
    void Start()
    {
        thisPhysicalPerformance = GetComponent<PhysicalPerformance>();
        thisTransform = GetComponent<Transform>();
        commander = GetComponent<Team>().commander;

        //добовляем небольшой рандов в дистанцию атаки моба для красоты
        attackDistance = Random.Range(attackDistance * 0.99f, attackDistance * 1.01f);
    }

    void Update()
    {
        if (thisPhysicalPerformance.isLive)
        {
            //если прошло время после последней атаки больше чем attackPause
            if (Time.time > timeLastAttack + attackPause)
            {
                target = MainScript.TargetSelection(transform, commander, attackDistance);

                if (target != null)
                {
                    Attack(target);
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
