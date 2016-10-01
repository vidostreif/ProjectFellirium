using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirMagic : MonoBehaviour {

    public CommanderAI commander;
    public float thisExplosionForce;
    public float thisExplosionRadius;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Если нажали левую кнопку мыши
        if (Input.GetMouseButtonDown(0))
        {

            //создаем взрыв в месте клика
            Explosion(Camera.main.ScreenToWorldPoint(Input.mousePosition), thisExplosionRadius, thisExplosionForce, commander);

        }
    }

    public void Explosion(Vector2 explosionPosition, float explosionRadius, float explosionForce, CommanderAI commander)
    {
        List<GameObject> objectsToInteract = new List<GameObject>();//список для объектов для взаимодействия
        GameObject[] findeObjects = GameObject.FindGameObjectsWithTag("Bullet"); //находим всех объекты с тегом  и создаём массив из них

        foreach (GameObject currentObject in findeObjects) //для каждого моба в массиве
        {
            float distance = Vector3.Distance(currentObject.transform.position, explosionPosition);//дистанция до объекта
            if (distance <= explosionRadius)//если объект находиться в радиусе действия
            {
                Rigidbody2D currentObjectRigidbody2D = currentObject.GetComponent<Rigidbody2D>();
                BulletScript currentObjectBulletScript = currentObject.GetComponent<BulletScript>();
                if (currentObjectRigidbody2D && currentObjectBulletScript)//если есть физика и скрип снаряда
                {
                    if (currentObjectBulletScript.enemy == commander)//если стрельба против нашей команды
                    {
                        //меняем командира
                        currentObjectBulletScript.enemy = commander.enemy;
                        //воздействуем на объект
                        AddExplosionForce(currentObjectRigidbody2D, explosionForce, explosionPosition, explosionRadius);
                    }
                    
                }

            }
        }

    }


    public void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }
}
