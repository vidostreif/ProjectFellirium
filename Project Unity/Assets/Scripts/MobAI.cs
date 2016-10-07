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
    public CommanderAI commander;
   
    void Start ()
    {
        thisPhysicalPerformance = GetComponent<PhysicalPerformance>();
        thisLongRangeWeapon = GetComponent<LongRangeWeapon>();
        thisMeleeWeapon = GetComponent<MeleeWeapon>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        //если жив
        if (thisPhysicalPerformance.isLive)
        {
            GameObject target = null;
            //если есть оружие ближнего и дальнего боя то ищем врага на всем расстоянии поле зрения
            if (thisMeleeWeapon && thisLongRangeWeapon)
            {
                target = MainScript.TargetSelection(transform, commander, thisPhysicalPerformance.GetAttackDistance());
            }
            else if (thisMeleeWeapon)//если только ближнего боя, то ищем врага в блези
            {
                target = MainScript.TargetSelection(transform, commander, 2);
            }
            else if (thisLongRangeWeapon)//если только дальнего боя, то ищем врага в далеке
            {
                target = MainScript.TargetSelection(transform, commander, thisPhysicalPerformance.GetAttackDistance(), 2);
            }

            //если есть цель, тогда атакуем
            if (target != null)
            {
                //останавливаемся
                thisPhysicalPerformance.StopMove();

                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                //если дистанция маленькая, и есть оружие ближнего боя, то бьем оружием ближнего боя
                if (distanceToTarget <= 2 && thisMeleeWeapon)
                {
                    thisMeleeWeapon.Attack(target);
                }
                else if(distanceToTarget >= 2 && thisLongRangeWeapon)//иначе если есть оружие дальнего боя и расстояние больше указанного
                {                    
                    //стреляем
                    thisLongRangeWeapon.Shot(target, commander);
                }

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
