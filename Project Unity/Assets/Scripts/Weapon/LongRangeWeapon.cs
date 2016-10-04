using UnityEngine;
using System.Collections;

public class LongRangeWeapon : MonoBehaviour {
    [Header("Редактирование атрибутов:")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float attackPause;
    [SerializeField] private float damage;

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
            Vector3 vShotDirection = new Vector3(target.transform.position.x, target.transform.position.y - 1, target.transform.position.z) - thisTransform.position;            
            vShotDirection.Normalize();
            
            //Дистанция до цели
            float distance = Vector3.Distance(target.transform.position, thisTransform.position);
            
            //расчетный угол
            float agleT = AgleBalistic(distance, distance * Mathf.Sqrt(distance));

            //создаем снаряд
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, thisTransform.position, Quaternion.identity);
            BulletScript newBulletBulletScript = newBullet.GetComponent<BulletScript>();
            Rigidbody2D newBulletRigidbody = newBullet.GetComponent<Rigidbody2D>();

            //Скорость цели 
            //если увеличиваем, то снижаем угол
            float targetSpeed = 0;
            if (distance > 5)
            {
                //targetSpeed = Mathf.Sqrt((Mathf.Abs(targetRigidbody2D.velocity.x) / distance / 1.9f / (newBulletRigidbody.gravityScale)));
                targetSpeed = ((Mathf.Abs(targetRigidbody2D.velocity.x - targetRigidbody2D.velocity.y/2)) / distance * 1.9f / Mathf.Sqrt(newBulletRigidbody.gravityScale));

                //targetSpeed = Mathf.Sqrt(Mathf.Abs(targetRigidbody2D.velocity.x) * (distance)/1700 / (newBulletRigidbody.gravityScale));

            }

            //корректировка на угол минус скорость цели
            vShotDirection = new Vector3(vShotDirection.x, vShotDirection.y + (agleT / 90) - targetSpeed, vShotDirection.z);
            
            //указываем кто враг
            newBulletBulletScript.enemy = commander.enemy;
            //указываем урон
            newBulletBulletScript.Damage = damage;
            //корректируем позицию
            newBullet.transform.position = thisTransform.position + vShotDirection;
            //указываем направление снаряда
            newBulletBulletScript.ToTurn(vShotDirection);
            
            //Стреляем
            Vector2 attackForce = vShotDirection * Mathf.Sqrt(distance) * newBulletRigidbody.mass * Mathf.Sqrt(newBulletRigidbody.gravityScale) * Random.Range(109f, 112f);
            newBulletRigidbody.AddForceAtPosition(attackForce, new Vector2(0, 0));
            

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
