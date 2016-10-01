using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhysicalPerformance : MonoBehaviour {
    [Header("Редактирование атрибутов:")]
    [SerializeField] private float hp;
    [SerializeField] private float mp;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float physicalResistance;
    [SerializeField] private float magicResistance;
    public float attackDistance = 0;//область зрения

    private MobAI thisMobAI;
    private SpriteRenderer thisSpriteRenderer;
    private Rigidbody2D thisRigidbody2D;

    public CommanderAI Commander { get; private set; }
    public bool isLive = true;
    

    // Use this for initialization
    void Start ()
    {
        thisMobAI = GetComponent<MobAI>();
        Commander = thisMobAI.commander;

        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        //ссылка на физику
        thisRigidbody2D = GetComponent<Rigidbody2D>();

        //добовляем небольшой рандов в дистанцию атаки моба для красоты
        attackDistance = Random.Range(attackDistance*0.99f, attackDistance * 1.01f);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetPhysicalDamage(float damage)
    {
        if (isLive)
        {
            //наносим урон
            damage = damage - (damage * physicalResistance / 100);
            hp -= damage;
            //Debug.Log("Нанесенный урон: " + damage);

            if (hp <= 0)
            {
                //если нанисли смертельный урон, то помечаем моба как убитый
                isLive = false;
                //Destroy(this.gameObject);
                //Debug.Log("Моб умер!");
            }

        }
    }

    //движется в указанном направлении
    public void Move(Vector3 directionOnTarget)
    {
        //если жив
        if (isLive)
        {
            //определяем в какую сторону должен двигаться моб и по ворачиваем спрайт
            if (directionOnTarget.x < 0)
            {
                thisSpriteRenderer.flipX = true;
            }
            else
            {
                thisSpriteRenderer.flipX = false;
            }

            //моб движется в указанном направлении с учетом рельефа
            thisRigidbody2D.velocity = new Vector2(directionOnTarget.normalized.x * Time.deltaTime * movementSpeed, thisRigidbody2D.velocity.y);

        }


    }

    //Останавливается
    public void StopMove()
    {
            thisRigidbody2D.velocity = new Vector2(0, thisRigidbody2D.velocity.y);
    }


}
