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
    //[SerializeField] private float attackDistance;//область зрения

    //private MobAI thisMobAI;
    private SpriteRenderer thisSpriteRenderer;
    private Rigidbody2D thisRigidbody2D;

    public CommanderAI commander { get; private set; }
    public bool isLive { get; private set; }

    //public float GetAttackDistance()
    //{
    //    return attackDistance;
    //}
    // Use this for initialization
    void Start ()
    {
        Team thisMobTeam = GetComponent<Team>();

        if (thisMobTeam)
        {
            commander = thisMobTeam.commander;
        }

        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        //ссылка на физику
        thisRigidbody2D = GetComponent<Rigidbody2D>();


        isLive = true;
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
                //если нанисли смертельный урон, то помечаем как убитый
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

            //ToTurn(thisRigidbody2D.velocity);//повернуть

            //моб движется в указанном направлении с учетом рельефа
            thisRigidbody2D.velocity = new Vector2(directionOnTarget.normalized.x * Time.deltaTime * movementSpeed, thisRigidbody2D.velocity.y);

        }


    }

    //Останавливается
    public void StopMove()
    {
            thisRigidbody2D.velocity = new Vector2(0, thisRigidbody2D.velocity.y);
    }


    ////поворачиваем в указанную сторону по указаной оси y
    //public void ToTurn(Vector3 targetPosition)
    //{
    //    //поворачиваем снаряд в сторону движения
    //    var angle = Vector2.Angle(Vector2.left, targetPosition);//угол между направлением и осью х
    //    transform.eulerAngles = new Vector3(0f, 0f, 0 < targetPosition.y ? -angle : angle);//поворачиваем с учетом увеличивается высота или уменьшается
    //}
}
