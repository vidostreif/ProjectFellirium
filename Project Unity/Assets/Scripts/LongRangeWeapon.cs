using UnityEngine;
using System.Collections;

public class LongRangeWeapon : MonoBehaviour {

    public UnityEngine.GameObject bulletPrefab;
    //public float bulletSpeed = 50;
    public float attackPause = 0.5f;
    public float damage;
    public Vector3 attackForce;

    //public CommanderAI Commander { get; private set; }

    private Transform thisTransform;
    private float timeLastAttack = 0;

    // Use this for initialization
    void Start()
    {
        thisTransform = GetComponent<Transform>();
    }


    public void Shot(GameObject target, CommanderAI commander)
    {
        if (Time.time > timeLastAttack + attackPause)
        {
            Rigidbody2D targetRigidbody2D = target.GetComponent<Rigidbody2D>();           

            //расчитывем направление выстрела
            Vector3 vShotDirection = target.transform.position - thisTransform.position;            
            vShotDirection.Normalize();

            //создаем снаряд
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, thisTransform.position + vShotDirection, Quaternion.identity);
            BulletScript newBulletBulletScript = newBullet.GetComponent<BulletScript>();
            Rigidbody2D newBulletRigidbody = newBullet.GetComponent<Rigidbody2D>();

            //Дистанция до цели
            float distance = Vector3.Distance(target.transform.position, thisTransform.position);
            
            //расчетный угол
            float agleT = AgleBalistic(distance, distance * Mathf.Sqrt(distance));
            //Скорость цели если увеличиваем, то снижаем угол
            float targetSpeed = (Mathf.Abs(targetRigidbody2D.velocity.x*1.7f)) / distance / Mathf.Sqrt(newBulletRigidbody.gravityScale) ;
            //корректировка на угол минус скорость цели
            vShotDirection = new Vector3(vShotDirection.x, vShotDirection.y + (agleT / 90) - targetSpeed, vShotDirection.z);
                        
            //указываем кто враг
            newBulletBulletScript.enemy = commander.enemy;
            //указываем урон
            newBulletBulletScript.Damage = damage;
            //указываем направление снаряда
            newBulletBulletScript.ToTurn(vShotDirection);
            
            //Стреляем
            attackForce = vShotDirection * Mathf.Sqrt(distance) * newBulletRigidbody.mass * Mathf.Sqrt(newBulletRigidbody.gravityScale) * 110;
            newBulletRigidbody.AddForceAtPosition(attackForce, new Vector2(0, 0));
            //newBulletRigidbody.AddForceAtPosition(vShotDirection * Mathf.Sqrt(distance) * Random.Range(10.7f, 10.95f), new Vector2(0,0));

            //указываем время последнего выстрела
            timeLastAttack = Time.time;
        }
        
    }



    public float AgleBalistic(float distance, float speedBullet)
    {
        
        //Находим велечину гравитации
        float gravity = Physics.gravity.magnitude;

        float discr = Mathf.Pow(speedBullet, 4) - 4 * (-gravity * gravity / 4) * (-distance * distance);

        //если получилось число меньше нуля, то увеличиваем его до нуля
        if (discr<0)
        {
            discr = 0;
        }

        //Время полёта
        float t = ((-speedBullet * speedBullet) - Mathf.Sqrt(discr)) / (-gravity * gravity / 2);
        t = Mathf.Sqrt(t);
        float th = gravity * t * t / 8;
        //Угол пушки
        float agle = 180 * (Mathf.Atan(4 * th / distance) / Mathf.PI);

        //Возрощаем угол
        return (agle);
    }
}
