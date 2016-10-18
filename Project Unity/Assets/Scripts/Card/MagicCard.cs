using UnityEngine;
using System.Collections;

public class MagicCard : MonoBehaviour {

    public EnumMagic magic; // тип магии

    //для магии "град стрел"
    public GameObject bulletPrefab; //префаб стрелы
    public int numberOfArrows = 20; //количество стрел
    public float damageOfArrows = 30; // урон стрел

    //для магии "взрыв"


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
            newBulletBulletScript.Damage = damageOfArrows;

            //стреляем
            Vector2 attackForce = vShotDirection * newBulletRigidbody.mass * Mathf.Sqrt(newBulletRigidbody.gravityScale) * Random.Range(708f, 711f);
            //указываем направление снаряда
            newBulletBulletScript.ToTurn(attackForce);

            newBulletRigidbody.AddForceAtPosition(attackForce, new Vector2(0, 0));
        }

    }
}
