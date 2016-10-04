using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour {
    [Header("Редактирование атрибутов:")]
    [SerializeField] private float attackPause;
    [SerializeField] private float damage;

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
