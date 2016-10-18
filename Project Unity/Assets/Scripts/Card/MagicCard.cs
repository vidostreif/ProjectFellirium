using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicCard : MonoBehaviour {

    public EnumMagic magic; // тип магии

    //для магии "град стрел"
    public GameObject bulletPrefab; //префаб стрелы
    public int numberOfArrows = 20; //количество стрел
    public int damageFromArrow = 30; // урон стрел

    //для магии "взрыв"
    public int damageFromExplosion = 150; // урон от взрыва 
    public int radiusOfExplosion = 10; // радиус взрыва

    public void Activate()
    {
        //active = true;
        //TimeActivate = Time.time;

        CommanderAI commander = GetComponent<Team>().commander;//командир карты

        if (magic == EnumMagic.StormOfArrows)// если град стрел
        {
            //проверка наличия префаба стрелы
            if (!bulletPrefab)
            {
                Debug.Log("У карты " + gameObject.name + " нет префаба стрелы, магия не будет создана!");
                return;
            }
            //находим ближайшего противника
            GameObject target = MainScript.TargetSelection(commander.transform, commander, 500);
            //создаем град стрел
            CreateStormOfArrows(target.transform.position, commander);
        }
        else if (magic == EnumMagic.Explosion)// если взрыв
        {
            //находим ближайшего противника
            GameObject target = MainScript.TargetSelection(commander.transform, commander, 500);

            Explosion(damageFromExplosion, radiusOfExplosion, target.transform.position, commander);//магия взрыва
        }

    }

    public void CreateStormOfArrows(Vector3 targetPosition, CommanderAI commander)//град стрел
    {
        //расчитывем направление смещения места создания стрел
        Vector3 shiftDirection = (Vector3)targetPosition - commander.enemy.transform.position;
        //shiftDirection.Normalize();

        //точка создания стрел 
        Vector3 placeOfArrows = targetPosition + new Vector3(shiftDirection.x > 0 ? 10 : -10, 10, 0);

        //расчитывем направление выстрела
        Vector3 vShotDirection = (Vector3)targetPosition - placeOfArrows - new Vector3(shiftDirection.x > 0 ? 5 : -5, 0, 0);
        vShotDirection.Normalize();

        //создаем стрелы
        for (int i = 0; i <= numberOfArrows; i++)
        {

            //создаем снаряд в рандомном месте рядом с указанной позицией
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, new Vector2(Random.Range(placeOfArrows.x - 2, placeOfArrows.x + 2), Random.Range(placeOfArrows.y - 2, placeOfArrows.y + 2)), Quaternion.identity);
            BulletScript newBulletBulletScript = newBullet.GetComponent<BulletScript>();
            Rigidbody2D newBulletRigidbody = newBullet.GetComponent<Rigidbody2D>();
            //указываем кто враг
            newBulletBulletScript.enemy = commander.enemy;
            //указываем урон
            newBulletBulletScript.Damage = damageFromArrow;

            //стреляем
            Vector2 attackForce = vShotDirection * newBulletRigidbody.mass * Mathf.Sqrt(newBulletRigidbody.gravityScale) * Random.Range(708f, 711f);
            //указываем направление снаряда
            newBulletBulletScript.ToTurn(attackForce);

            newBulletRigidbody.AddForceAtPosition(attackForce, new Vector2(0, 0));
        }

    }

    
    public void Explosion(int damage, float radius, Vector3 positionExplosion, CommanderAI commander)//магия взрыва
    {
        //смещаем точку взрыва за врага
        //расчитывем направление смещения 
        float shiftDirection = positionExplosion.x - commander.enemy.transform.position.x;
        //shiftDirection.Normalize();

        //новая точка взрыва 
        positionExplosion = positionExplosion + new Vector3(shiftDirection > 0 ? -radius* 0.7f : radius * 0.7f, 3, 0);

        //находим все объекты с PhysicalPerformance в радиусе
        List<GameObject> objectsToInteract = MainScript.FindObjectsInRadiusWithComponent(positionExplosion, radius, typeof(PhysicalPerformance));
        foreach (GameObject currentObject in objectsToInteract) //для каждого объекта в массиве
        {
            Rigidbody2D currentObjectRigidbody2D = currentObject.GetComponent<Rigidbody2D>();
            PhysicalPerformance currentObjectPhysicalPerformance = currentObject.GetComponent<PhysicalPerformance>();
            if (currentObjectRigidbody2D && currentObjectPhysicalPerformance)//если есть физика и скрип физ параметров
            {
                if (currentObjectPhysicalPerformance.commander.enemy == commander)//если объект против нас
                {
                    //воздействуем на объект
                    currentObjectPhysicalPerformance.SetPhysicalDamage(damage);//наносим урон
                    //откидываем объект
                    var dir = (currentObjectRigidbody2D.transform.position - positionExplosion);
                    float wearoff = 1 - (dir.magnitude / radius);
                    currentObjectRigidbody2D.AddForce(dir.normalized * damage * 500 * wearoff);

                }

            }
        }
    }
}
