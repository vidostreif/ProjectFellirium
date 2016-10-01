using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LoopedMotionBackground : MonoBehaviour {

    public float speed = 0;

    private Vector3 positionFirstObject; //первый объект
    private Vector3 positionSecondObject; //второй объект
    private Vector3 startPoitionParent;//стартовая позиция родителя
    private Transform transformParent;//позиция родителя
    private List<Transform> transformChilds;//список дочерних трансформов

    void Start()
    {
        transformParent = transform;
        //записываем стартовую позицию родителя
        startPoitionParent = transformParent.position;

        //инициализируем список
        transformChilds = new Transform[transform.childCount].ToList();
        int i = 0;
        foreach (Transform t in transform)
        {
            transformChilds[i++] = t;
        }

        //сортировка по позиции
        transformChilds = transformChilds.OrderBy( t => t.localPosition.x).ToList();

        //если больше одного элемента в списке
        if (transformChilds.Count > 1)
        {
            positionFirstObject = transformChilds[0].localPosition;//позиция крайнего объекта
            positionSecondObject = transformChilds[1].localPosition;//позиция следующего объекта
        }

    }

    // Update is called once per frame
    void Update () {

        //если первый объект добрался до позиции второго, то последний объект переносим на стартовую позицию
        if (transformChilds[0].localPosition.x >= positionSecondObject.x)
        {
            //высчитываем смещение родителя относительно своей стартовой позиции
            //Vector3 offset = transformParent.position - startPoitionParent;

            //перемещаем объект с учетом смещения
            Transform transform = transformChilds[transformChilds.Count - 1];
            transform.localPosition = positionFirstObject;

            //перемещаем последний объект в списке на первую позицию
            transformChilds.Remove(transform);
            transformChilds.Insert(0, transform);
        }

        //смещаем все объекты на заданную скорость
        foreach (var transformChild in transformChilds)
        {
            transformChild.localPosition = new Vector3(transformChild.localPosition.x + speed * Time.deltaTime, transformChild.localPosition.y, transformChild.localPosition.z);

        }
    }
}
