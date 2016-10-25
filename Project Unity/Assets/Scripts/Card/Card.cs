using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour {
    public CommanderAI commander { get; private set; }
    public EnumCard type;

    //private Transform thisTransform;
    //private Vector3 newPosition;
    //private Vector3 newScale;
    private bool unresolvedCard = true;//признак не разыгранности карты

    // Use this for initialization
    void Start () {

        commander = GetComponent<Team>().commander;
        //проверка наличия командира
        if (!commander)
        {
            Debug.Log("У карты " + gameObject.name + " не определен командир!");
        }

        //thisTransform = GetComponent<Transform>();
        //newPosition = thisTransform.position;
        //newScale = thisTransform.localScale;

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

        if (GetComponent<MagicCard>())
        {
            type = EnumCard.MagicCard; //карта магии
            //dopComponent = GetComponent<AddWarCard>();
        }


    }

    void Update()
    {
        ////определяем новое место и размер отображения карты
        //thisTransform.position = Vector3.Lerp(thisTransform.position ,newPosition, Time.deltaTime * 1f);
        //thisTransform.localScale = Vector3.Lerp(thisTransform.localScale, newScale, Time.deltaTime * 1f);
    }


    public void MouseDown()
    {
        if (unresolvedCard && commander)// если эта карта не разыгранна и есть командир
        {
            commander.PlayCard(this);
            unresolvedCard = false; //помечаем карту как разыгранная
        }        
    }

    public void Activate()
    {
        if (type == EnumCard.ImprovingCard)
        {
            ImprovingCard dopComponent = GetComponent<ImprovingCard>();
            dopComponent.Activate();
        }
        else if (type == EnumCard.AddWarCard)
        {
            AddWarCard dopComponent = GetComponent<AddWarCard>();
            dopComponent.Activate();
        }
        else if (type == EnumCard.MagicCard)
        {
            MagicCard dopComponent = GetComponent<MagicCard>();
            dopComponent.Activate();
        }
    }

    ////определяем новые место и размер карты
    //public void MovingToANewPlace(Vector3 position, float Scale = 1)
    //{
    //    newPosition = position;
    //    newScale = new Vector3(thisTransform.localScale.x * Scale, thisTransform.localScale.y * Scale, thisTransform.localScale.z * Scale);
    //}
}
