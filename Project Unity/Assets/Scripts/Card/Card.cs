using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour {
    //public CommanderAI commander { get; private set; }

    public EnumCard type;
    public Team team { get; private set; }//наша команда
    public Vector3 startLocalPosition;//стартовая позиция карты относительно предка

    private bool unresolvedCard = true;//признак не разыгранности карты
    private bool drag = false; //карта перетаскивается
    


    void Awake()
    {
        team = GetComponent<Team>();
        startLocalPosition = transform.localPosition;
    }

    public void PointerClick()
    {
        if ((unresolvedCard && team.commander && !drag) // если эта карта не разыгранна и есть командир и не перетаскивается
            || drag && transform.localPosition.y > startLocalPosition.y + 5)// или если перетаскивается, то если выше на 3 поинта чем стартовая позиция
        {
            team.commander.PlayCard(this);
            unresolvedCard = false; //помечаем карту как разыгранная
        }        
    }

    public void PointerEnter()
    {
        if (unresolvedCard)// если эта карта не разыгранна
        {
            MainScript.Instance.SlowlyMoveToNewLocalPosition(transform, new Vector3(transform.localPosition.x, transform.localPosition.y + 2, transform.localPosition.z), 1, 2);
        }
    }

    public void PointerExit()
    {
        if (unresolvedCard)// если эта карта не разыгранна
        {
            MainScript.Instance.SlowlyMoveToNewLocalPosition(transform, startLocalPosition, 1, 2);
        }
    }

    public void Drag()
    {
        if (unresolvedCard)// если эта карта не разыгранна
        {
            MasterController.Instance.DragLocalObject(transform);
            drag = true;
        }
    }

    public void EndDrop()
    {
        if (unresolvedCard)// если эта карта не разыгранна
        {
            MasterController.Instance.DropLocalObject(true);//бросаем с возвратом на локальную позицию
        }
        else
        {
            MasterController.Instance.DropLocalObject(false);//бросаем без возврата на стартовую позицию
        }
        drag = false;
    }

    public void Activate()
    {
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
    
}
