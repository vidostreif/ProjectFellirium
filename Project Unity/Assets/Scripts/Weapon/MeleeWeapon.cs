using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour {
    public float attackPause = 0.5f;
    public float damage;

    private Transform thisTransform;
    private float timeLastAttack = 0;
    

    public void Attack(GameObject target)
    {
        if (Time.time > timeLastAttack + attackPause)
        {
            //берем скрипт physicalPerformance если он есть, иначе возвращается fallse
            PhysicalPerformance otherPhysicalPerformance = target.GetComponent<PhysicalPerformance>();
            if (otherPhysicalPerformance)//Если physicalPerformance есть
            {
                //наносим дамаг
                otherPhysicalPerformance.SetPhysicalDamage(damage);
            }
        }
    }

}
