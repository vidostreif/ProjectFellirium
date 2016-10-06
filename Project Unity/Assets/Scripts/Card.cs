using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour {
    [SerializeField] private CommanderAI commander;
    public EnumCard type;

    // Use this for initialization
    void Start () {

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
        }
        //else if (true)
        //{

        //}
        //else if (true)
        //{

        //}

    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void MouseDown()
    {
        PlayThisCard();
    }

    private void PlayThisCard()
    {
        
        switch (type)
        {
            case EnumCard.ImprovingCard: //если карта улучшения
                commander.AddCard(this);
                break;
            case EnumCard.AddWarCard:
               
                break;
            case EnumCard.AddEquipmentCard:

                break;
            case EnumCard.MagicCard:

                break;
        }
    }
}
