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

    public CommanderAI commander { get; private set; }

    void Start ()
    {
        thisPhysicalPerformance = GetComponent<PhysicalPerformance>();
        thisLongRangeWeapon = GetComponent<LongRangeWeapon>();
        thisMeleeWeapon = GetComponent<MeleeWeapon>();
        commander = GetComponent<Team>().commander;
    }

    // Update is called once per frame
    void FixedUpdate() {
        //если жив
        if (thisPhysicalPerformance.isLive)
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

                //float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                ////если дистанция маленькая, и есть оружие ближнего боя, то бьем оружием ближнего боя
                //if (distanceToTarget <= 2 && thisMeleeWeapon)
                //{
                //    thisMeleeWeapon.Attack(target);
                //}
                //else if(distanceToTarget >= 2 && thisLongRangeWeapon)//иначе если есть оружие дальнего боя и расстояние больше указанного
                //{                    
                //    //стреляем
                //    thisLongRangeWeapon.Shot(target, commander);
                //}

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
