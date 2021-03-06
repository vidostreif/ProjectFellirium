﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour {

    public CommanderAI enemy;
    public int Damage { private get; set; }
    public bool explode = false; // взрываться
    public float radiusOfExplosion = 3; //радиус взрыва

    private float timeLive = 8; //время жизни
    private float timedeath; //что бы патрон не летел вечно, мы ограничим время его жизни
    private Rigidbody2D thisRigidbody2D;
    private SpriteRenderer thisSpriteRenderer;
    //private Vector3 lastPosition;
    private bool activeMissile = true;

    // Use this for initialization
    void Start () {
        timedeath = Time.time + timeLive; //время смерти патрона	
        thisRigidbody2D = GetComponent<Rigidbody2D>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        //lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        //Если активный снаряд
        if (activeMissile)
        {
            //if (lastPosition != transform.position)//если снаряд сместился
            //{

                ToTurn(thisRigidbody2D.velocity);//повернуть
                //lastPosition = transform.position;//записываем текущую позицию
            //}


        }

        if (Time.time > timedeath)// проверяем, не пора ли умирать патрону
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        thisSpriteRenderer.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activeMissile)//если активный снаряд
        {
            if (explode)//если взрывной снаряд
            {
                //находим все объекты с PhysicalPerformance в радиусе
                GameObject[] objectsToInteract = MainScript.FindObjectsInRadiusWithComponent(transform.position, radiusOfExplosion, typeof(PhysicalPerformance));
                foreach (GameObject currentObject in objectsToInteract) //для каждого объекта в массиве
                {
                    PhysicalPerformance currentObjectPhysicalPerformance = currentObject.GetComponent<PhysicalPerformance>();
                        //эсли это враг и он жив
                        if (enemy == currentObjectPhysicalPerformance.team.commander)
                        {
                        //то наносим дамаг
                        currentObjectPhysicalPerformance.SetPhysicalDamage(Damage);
                        }
                }                
            }
            else//иначе обрабатываем только объект в который попали
            {
                //берем скрипт physicalPerformance если он есть, иначе возвращается fallse
                PhysicalPerformance otherPhysicalPerformance = other.gameObject.GetComponent<PhysicalPerformance>();

                if (otherPhysicalPerformance)//Если physicalPerformance есть
                {
                    //эсли это враг и он жив
                    if (enemy == otherPhysicalPerformance.team.commander && otherPhysicalPerformance.isLive)
                    {
                        //то наносим дамаг
                        otherPhysicalPerformance.SetPhysicalDamage(Damage);
                    }
                }
            }

            DisableMissile(other.gameObject);//деативируем
        }

        

    }

    //поворачиваем в указанную сторону по указаной оси y
    public void ToTurn(Vector3 targetPosition)
    {
        //поворачиваем снаряд в сторону движения
        var angle = Vector2.Angle(Vector2.left, targetPosition);//угол между направлением и осью х
        transform.eulerAngles = new Vector3(0f, 0f, 0 < targetPosition.y ? -angle : angle);//поворачиваем с учетом увеличивается высота или уменьшается
    }

    //Декативируем снаряд
    private void DisableMissile(GameObject target)
    {
        if (thisRigidbody2D && activeMissile)//если есть риджи боди и снаряд активен
        {
            //отсанавливаем обьект
            thisRigidbody2D.velocity = new Vector3(0, 0, 0);
            thisRigidbody2D.isKinematic = true;
            //сообщаем, что снаряд более не активен
            activeMissile = false;

            //если взрывается
            if (explode)
            {
                //создаем эффект взрыва и сразу уничтожаем объект
                SpecialEffectsHelper.Instance.Explosion(transform.position);
                Destroy(gameObject);
            }
            else
            {
                //делаем снаряд дочерним для объекта в который попали
                transform.parent = target.transform;
                //уничтожаем через секунд
                Destroy(gameObject, 0.5f);
            }

        }
        
    }

}
