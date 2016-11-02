using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    //public float sensitivity;   

    //private float Boundary = 5; //размер рамки при нахождении в которой будет передвигаться камера
    private Camera myCam;
    //private Vector3 startMousePosition = new Vector3(0,0,0);

    //[SerializeField] private Vector2 minXAndY; //хранит минимальные значения в пределах которых может передвигаться камера
    //[SerializeField] private Vector2 maxXAndY; //хранит максимальные значения в пределах которых может передвигаться камера
    //[SerializeField] private float minOrthographicSize; //хранит мин значение в пределах которых может зумироваться камера
    //[SerializeField] private float maxOrthographicSize; //хранит макс значение в пределах которых может зумироваться камера


    [SerializeField] private SpriteRenderer boundsMap; // спрайт, в рамках которого будет перемещаться камера
    [SerializeField] private bool useBounds = true; // использовать или нет, границы для камеры
    //[SerializeField] private float smooth = 2.5f; // сглаживание при следовании за персонажем
    private Vector3 min, max;

    //// Use this for initialization
    //void Start () {

    //    myCam = GetComponent<Camera>();       

    //}

    void Awake()
    {
        //_use = this;
        myCam = GetComponent<Camera>();
        myCam.orthographic = true;
        CalculateBounds();
        //SetUniform();
    }

    void Update()
    {
        //SetUniform();

        ////зум
        //myCam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
        //// удерживаем камеру в указанных пределах 
        ////по зуму
        //myCam.orthographicSize = Mathf.Clamp(myCam.orthographicSize, minOrthographicSize, maxOrthographicSize);
        
        //корректируем камеру
        transform.position = MoveInside(transform.position, new Vector3(min.x, min.y, transform.position.z), new Vector3(max.x, max.y, transform.position.z));
    }

    ////корректировка камеры
    //private void cameraAdjustment()
    //{
    //    Transform myCamTransform = myCam.transform;
                
    //    // удерживаем камеру в указанных пределах 
    //    //по зуму
    //    myCam.orthographicSize = Mathf.Clamp(myCam.orthographicSize, minOrthographicSize, maxOrthographicSize);
    //    //по позиции  с учетом зума
    //    float positionX = Mathf.Clamp(myCamTransform.position.x, minXAndY.x / myCam.orthographicSize, maxXAndY.x / myCam.orthographicSize);
    //    float positionY = Mathf.Clamp(myCamTransform.position.y, minXAndY.y / myCam.orthographicSize, maxXAndY.y / myCam.orthographicSize);

    //    //сглажываем 
    //    //myCamTransform.position = Vector3.Lerp(myCamTransform.position, new Vector3(positionX, positionY, transform.position.z), 1);
    //    myCamTransform.position = new Vector3(positionX, positionY, transform.position.z);
    //}


    // если в процессе игры, было изменено разрешение экрана
    // или параметр "Orthographic Size", то следует сделать вызов данной функции повторно
    public void CalculateBounds()
    {
        if (boundsMap == null) return;
        Bounds bounds = Camera2DBounds();
        min = bounds.max + boundsMap.bounds.min;
        max = bounds.min + boundsMap.bounds.max;
    }

    //определение границ
    Bounds Camera2DBounds()
    {
        float height = myCam.orthographicSize * 2;
        return new Bounds(Vector3.zero, new Vector3(height * myCam.aspect, height, 0));
    }

    //не даем камере выйте за границы указанного спрайта
    Vector3 MoveInside(Vector3 current, Vector3 pMin, Vector3 pMax)
    {
        if (!useBounds || boundsMap == null) return current;
        current = Vector3.Max(current, pMin);
        current = Vector3.Min(current, pMax);
        return current;
    }

    ////указываем размер камеры равный разрешению экрана
    //private void SetUniform()
    //{
    //    float orthographicSize = myCam.pixelHeight / 2;
    //    if (orthographicSize != myCam.orthographicSize)
    //        myCam.orthographicSize = orthographicSize;
    //}
}
