using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour {
    //public CommanderAI commander { get; private set; }

    public EnumCard type;

    private bool unresolvedCard = true;//признак не разыгранности карты
    public Team team { get; private set; }//наша команда


    void Awake()
    {
        team = GetComponent<Team>();
    }

    public void MouseDown()
    {
        if (unresolvedCard && team.commander)// если эта карта не разыгранна и есть командир
        {
            team.commander.PlayCard(this);
            unresolvedCard = false; //помечаем карту как разыгранная
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
