using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MobAI : MonoBehaviour {
        
    private PhysicalPerformance thisPhysicalPerformance;
    private LongRangeWeapon thisLongRangeWeapon;
    private MeleeWeapon thisMeleeWeapon;
    private Transform enemyTowerTransform;
    private RangeViewTriger rangeViewScript;
    private bool canAct = true; //может действовать
    private Transform groundCheck;          // тригер определяющий землю
    private bool grounded = false;          // мы на земле?

    public CommanderAI commander { get; private set; }

    void Awake()
    {
        // поиск трансформа тригера определяющего землю
        groundCheck = transform.Find("groundCheck");
    }
    
    void Start ()
    {
        thisPhysicalPerformance = GetComponent<PhysicalPerformance>();
        thisLongRangeWeapon = GetComponent<LongRangeWeapon>();
        thisMeleeWeapon = GetComponent<MeleeWeapon>();
        commander = GetComponent<Team>().commander;
    }

    // Update is called once per frame
    void FixedUpdate() {

        // Определение земли
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        
        //если жив и может действовать
        if (thisPhysicalPerformance.isLive && canAct && grounded)
        {
            GameObject target = null;

            if (thisMeleeWeapon)//если только ближнего боя, то ищем врага в блези
            {
                target = thisMeleeWeapon.target;
            }
            else if (thisLongRangeWeapon)//если только дальнего боя, то ищем врага в далеке
            {
                target = thisLongRangeWeapon.target;
            }

            //если есть цель, тогда стоим на месте
            if (target != null)
            {
                //останавливаемся
                thisPhysicalPerformance.StopMove();


            }
            else//иначе движемся к вражеской башне
            {
                //получаем вектор идущий от моба к вражеской башне
                Vector3 directionOnTarget = commander.GetEnemyTowerTransform().position - transform.position;
                if (directionOnTarget.magnitude > 2)//если длина этого вектора больше двух
                {
                    thisPhysicalPerformance.Move(directionOnTarget);
                }
            }
        }


    }




}
