using UnityEngine;
using System.Collections;

public class MasterController : MonoBehaviour {

    public float sensitivity;
    public static MasterController Instance; // Синглтон
    private Vector3 startPosition = new Vector3(0, 0, 0);
    private Camera myCam;

    private Transform transforForLocalDAndD = null;
    private Vector3 startPositionLocalDAndD;

    void Awake()
    {
        // регистрация синглтона
        if (Instance != null)
        {
            Debug.LogError("Несколько экземпляров MasterController!");
        }

        Instance = this;

        myCam = Camera.main;//находим главную камеру
    }
    
    void Update()
    {
        ProcessingCameraMovement();//обработка перемещения камеры
        MoveDragObject();//процедура перемещения взятого объекта
    }

    public void DragLocalObject(Transform gameObjectTransform)//записываем данные для последующего перемещения объекта за мышкой или пальцем
    {
        MainScript.Instance.CheckForArrayForMoveAndRemove(gameObjectTransform);//удаления из массива для медленного перемещения(если объект там есть)

        transforForLocalDAndD = gameObjectTransform;
        startPositionLocalDAndD = gameObjectTransform.localPosition;//локальная позиция

        //если стартовая позиция мыши нулевая
        if (startPosition == new Vector3(0, 0, 0))
        {
            RecordStartPosition();
        }
    }

    public void DropLocalObject(bool returnToStartPosition)// удаляем данные об объекте который перемещяли мышкой или пальцем и возвращаем его на место
    {
        if (returnToStartPosition)
        {
            MainScript.Instance.SlowlyMoveToNewLocalPosition(transforForLocalDAndD, startPositionLocalDAndD, 1, 3);
        }

        transforForLocalDAndD = null;
        ResetStartPosition();//Обнуляем стартовую позицию
    }

    private void ProcessingCameraMovement()//процедура контролирующая перемещение камеры
    {
        //Если нажали 
        if (Input.GetButtonDown("Fire1") && transforForLocalDAndD == null)
        {
            //если стартовая позиция мыши нулевая
            if (startPosition == new Vector3(0, 0, 0))
            {
                RecordStartPosition();
            }
        }

        //Если удерживаем 
        if (Input.GetButton("Fire1") && transforForLocalDAndD == null)
        {
            //расчитываем вектор смещения
            Vector3 translation = startPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //передаем его камере новую позицию
            myCam.transform.Translate(translation);
            //переопределяем стартовую позицию мышки
            RecordStartPosition();
        }
        else if (transforForLocalDAndD == null)
        {
            //иначе проверяем другие способы перемещения
            // Если подвели указатель мыши к левому краю
            if (/*Input.mousePosition.x <= Boundary || */ Input.GetKey(KeyCode.LeftArrow))
                // Двигаем камеру
                this.transform.Translate(-sensitivity * Time.deltaTime, 0, 0);

            // Если подвели указатель мыши к правому краю
            if (/*Input.mousePosition.x >= Screen.width - Boundary ||*/ Input.GetKey(KeyCode.RightArrow))
                // Двигаем камеру
                this.transform.Translate(sensitivity * Time.deltaTime, 0, 0);

            // Если подвели указатель мыши к верхнему краю
            if (/*Input.mousePosition.y >= Screen.height - Boundary ||*/ Input.GetKey(KeyCode.UpArrow))
                // Двигаем камеру
                this.transform.Translate(0, sensitivity * Time.deltaTime, 0);

            // Если подвели указатель мыши к нижниму краю
            if (/*Input.mousePosition.y <= Boundary ||*/ Input.GetKey(KeyCode.DownArrow))
                // Двигаем камеру
                this.transform.Translate(0, -sensitivity * Time.deltaTime, 0);
        }

        //Если отпустили 
        if (Input.GetButtonUp("Fire1") && transforForLocalDAndD == null)
        {
            ResetStartPosition();//Обнуляем стартовую позицию
        }
    }

    private void MoveDragObject()//процедура перемещения взятого объекта
    {
        if (transforForLocalDAndD != null)
        {
            //расчитываем вектор смещения
            Vector3 translation = Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPosition;
            //передаем его взятому объекту
            transforForLocalDAndD.Translate(translation);
            //переопределяем стартовую позицию мышки
            RecordStartPosition();
        }
    }

    private void RecordStartPosition()//записываем стартовую позицию мыши или пальца
    {
        //записываем положение мышки
        startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void ResetStartPosition()//обнуляем значение стартовой позиции
    {
        startPosition = new Vector3(0, 0, 0);
    }
}
