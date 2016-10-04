using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Magic : MonoBehaviour {

    private CommanderAI commander;
    public float thisExplosionForce = 5000;
    public float thisExplosionRadius = 15;

    public GameObject bulletPrefab; //префаб стрелы
    public int numberOfArrows = 20; //количество стрел
    public float damageOfArrows = 30; // урон стрел

    public EnumAirMagic[] magicList; //список магии
    private EnumAirMagic currentMagic = 0;//выбранная магия

    // Use this for initialization
    void Start()
    {
        commander = GetComponent<CommanderAI>();
    }

    // Update is called once per frame
    void Update()
    {

        //Если нажали левую кнопку мыши
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentMagic)
            {
                case EnumAirMagic.BlowingOffBulletExplosion:
                    //создаем взрыв в месте клика
                    EffectsBulletsOnRadius(EnumAirMagic.BlowingOffBulletExplosion, thisExplosionForce, Camera.main.ScreenToWorldPoint(Input.mousePosition), thisExplosionRadius);
                    break;
                case EnumAirMagic.BlowingOffBullets:
                    //перенаправляем снаряды в сторону врага
                    EffectsBulletsOnRadius(EnumAirMagic.BlowingOffBullets, thisExplosionForce, Camera.main.ScreenToWorldPoint(Input.mousePosition), thisExplosionRadius);
                    break;
                case EnumAirMagic.StormOfArrows:
                    //перенаправляем снаряды в сторону врага
                    CreateStormOfArrows(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    break;
            }

            //currentMagic = 0;//обозначаем 
            
        }
    }

    public void magicChoice(int magicNumber)//выбор текущей магии игроком
    {
        //если размер списка соответсвует переданному значению
        if (magicList.Length > magicNumber)
        {
            currentMagic = magicList[magicNumber];
        }
        
    }

    public List<GameObject> FindObjectsInRadius(Vector2 position, float Radius, string tag)//поиск объектов по тегу в радиусе
    {
        List<GameObject> objectsToInteract = new List<GameObject>();//список для объектов для взаимодействия
        GameObject[] findeObjects = GameObject.FindGameObjectsWithTag(tag); //находим всех объекты с тегом  и создаём массив из них

        foreach (GameObject currentObject in findeObjects) //для каждого объекта в массиве
        {
            float distance = Vector2.Distance(currentObject.transform.position, position);//дистанция до объекта
            if (distance <= Radius)//если объект находиться в радиусе действия
            {
                //добавляем в список
                objectsToInteract.Add(currentObject);
            }
        }

        return objectsToInteract;
    }

    //воздействует на снаряды определенной магией, с определенной силой, в определенной позиции, в определенном радиусе
    public void EffectsBulletsOnRadius(EnumAirMagic magic, float force, Vector3 position, float radius)
    {
        //находим все снаряды в радиусе
        List<GameObject> objectsToInteract = FindObjectsInRadius(position, radius, "Bullet");
        foreach (GameObject currentObject in objectsToInteract) //для каждого объекта в массиве
        {
            Rigidbody2D currentObjectRigidbody2D = currentObject.GetComponent<Rigidbody2D>();
            BulletScript currentObjectBulletScript = currentObject.GetComponent<BulletScript>();
            if (currentObjectRigidbody2D && currentObjectBulletScript)//если есть физика и скрип снаряда
            {
                if (currentObjectBulletScript.enemy == commander)//если снаряд против нашей команды
                {
                    //меняем врага
                    currentObjectBulletScript.enemy = commander.enemy;
                    //воздействуем на объект
                    if (magic == EnumAirMagic.BlowingOffBulletExplosion)//если взрыв
                    {
                        var dir = (currentObjectRigidbody2D.transform.position - position);
                        float wearoff = 1 - (dir.magnitude / radius);
                        currentObjectRigidbody2D.AddForce(dir.normalized * force * wearoff);
                    }
                    else if (magic == EnumAirMagic.BlowingOffBullets)//если возврат во врага
                    {
                        currentObjectRigidbody2D.velocity = (-currentObjectRigidbody2D.velocity);
                    }
                    
                }

            }
        }
    }

    public void CreateStormOfArrows(Vector3 position)//град стрел
    {
        //создаем стрелы
        for (int i = 0; i <= numberOfArrows; i++)
        {
            //расчитывем направление выстрела
            Vector3 vShotDirection = (Vector3)commander.enemy.transform.position - position;
            vShotDirection.Normalize();

            //создаем снаряд в рандомном месте рядом с указанной позицией
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, new Vector2(Random.Range(position.x - 2, position.x + 2), Random.Range(position.y - 2, position.y + 2)), Quaternion.identity);
            BulletScript newBulletBulletScript = newBullet.GetComponent<BulletScript>();
            Rigidbody2D newBulletRigidbody = newBullet.GetComponent<Rigidbody2D>();
            //указываем кто враг
            newBulletBulletScript.enemy = commander.enemy;
            //указываем урон
            newBulletBulletScript.Damage = damageOfArrows;

            //стреляем
            Vector2 attackForce = vShotDirection * newBulletRigidbody.mass * Mathf.Sqrt(newBulletRigidbody.gravityScale) * Random.Range(308f, 311f);
            //указываем направление снаряда
            newBulletBulletScript.ToTurn(attackForce);
            
            newBulletRigidbody.AddForceAtPosition(attackForce, new Vector2(0, 0));
        }

    }
}
