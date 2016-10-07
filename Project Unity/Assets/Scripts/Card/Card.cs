using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour {
    public CommanderAI commander;
    public EnumCard type;

    //private Component dopComponent;

    private Transform thisTransform;
    private Vector3 newPosition;
    private Vector3 newScale;

    // Use this for initialization
    void Start () {

        thisTransform = GetComponent<Transform>();
        newPosition = thisTransform.position;
        newScale = thisTransform.localScale;


        ////находим коммандира который указан GameController
        //GameObject[] gameControllers = GameObject.FindGameObjectsWithTag("GameController");

        //if (gameControllers.Length == 0)//если нет ни одного контроллера
        //{
        //    Debug.Log("На сцене нет ни одного GameController");
        //}
        //else if (gameControllers.Length > 1)// если больше одного контроллера
        //{
        //    Debug.Log("На сцене больше одного GameController");
        //}
        //else if (gameControllers.Length == 1)
        //{
        //    MainScript gameControllerMainScript = gameControllers[0].GetComponent<MainScript>();

        //    if (gameControllerMainScript)
        //    {
        //        commander = gameControllerMainScript.playerCommander;
        //    }
        //    else
        //    {
        //        Debug.Log("К GameController не прикреплен MainScript");
        //    }

        //}

        //определяем тип карты при создании
        if (GetComponent<ImprovingCard>())
        {
            type = EnumCard.ImprovingCard; //карта улучшения
            //dopComponent = GetComponent<ImprovingCard>();
        }

        if (GetComponent<AddWarCard>())
        {
            type = EnumCard.AddWarCard; //карта добавления мобов
            //dopComponent = GetComponent<AddWarCard>();
        }


    }

    void Update()
    {
        //определяем новое место и размер отображения карты
        thisTransform.position = Vector3.Lerp(thisTransform.position ,newPosition, Time.deltaTime * 1f);
        thisTransform.localScale = Vector3.Lerp(thisTransform.localScale, newScale, Time.deltaTime * 1f);
    }


    public void MouseDown()
    {
        commander.PlayCard(this);
    }

    public void Activate()
    {
        if (type == EnumCard.ImprovingCard)
        {
            
        }
        else if (type == EnumCard.AddWarCard)
        {
            AddWarCard dopComponent = GetComponent<AddWarCard>();
            dopComponent.Activate();
        }
    }

    //определяем новые место и размер карты
    public void MovingToANewPlace(Vector3 position, float Scale = 1)
    {
        newPosition = position;
        newScale = new Vector3(thisTransform.localScale.x * Scale, thisTransform.localScale.y * Scale, thisTransform.localScale.z * Scale);
    }
}
