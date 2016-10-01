using UnityEngine;
using System.Collections;

public class DepthEffect : MonoBehaviour {

    /// Скрипт создающий ощущение глубины (накладывается на фон)

    private Transform thisTransform;
    private Camera myCam;
    private Vector3 myCamStartTransformPosition; //стартовая позиция камеры
    private float speed;

    void Start()
    {
        //myCam = FindObjectOfType<Camera>();
        myCam = Camera.main;
        myCamStartTransformPosition = myCam.transform.position;
        thisTransform = transform;
        //вычисляем расстояние между камерой и фоном
        speed = Mathf.Abs(thisTransform.position.z - myCam.transform.position.z);
        //уменьшаем значение
        speed = speed * 0.05f;
    }
    
    void Update()
    {
        //смещаем фон
        thisTransform.position = GetVector(thisTransform.position, speed);
        //обновляем значение позиции камеры
        myCamStartTransformPosition = myCam.transform.position;
    }

    Vector3 GetVector(Vector3 position, float speed)
    {
        //функция определяет смещение фона в зависимости от смещения камеры
        float posX = position.x;
        posX += (myCam.transform.position.x - myCamStartTransformPosition.x) * speed;
        float posY = position.y;
        posY += (myCam.transform.position.y - myCamStartTransformPosition.y) * speed;
        return new Vector3(posX, posY, position.z);
    }
}
