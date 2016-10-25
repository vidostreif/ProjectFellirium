using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour {
    //public CommanderAI commander { get; private set; }

    public EnumCard type;
    public Team team { get; private set; }//наша команда
    public Vector3 startLocalPosition;//стартовая позиция карты относительно предка

    private bool unresolvedCard = true;//признак не разыгранности карты
    


    void Awake()
    {
        team = GetComponent<Team>();
        startLocalPosition = transform.localPosition;
    }

    public void PointerClick()
    {
        if (unresolvedCard && team.commander)// если эта карта не разыгранна и есть командир
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
