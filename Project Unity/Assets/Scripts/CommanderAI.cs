using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CommanderAI : MonoBehaviour {

    public GameObject tower;//башня командира

    public float delayTimeCreateMob = 5; //пауза между соданиями моба
    public float delayTimeCreateMob2 = 6;
    public GameObject mob;//префаб моба
    public GameObject mob2;

    public bool managePlayer = false;//признок перехвата управления игроком

    public CommanderAI enemy;//командир враг
    public CommanderAI friend;//командир друг

    public List<Card> CardsInDeck; //массив карты в колоде
    public List<Card> CardsInHand; //массив карты в руке
    public List<Card> ActionsCards; //активные карты
    

    private float timeLastCreateMob;
    private float timeLastCreateMob2;
    private Transform thisTransform;

    // Use this for initialization
    void Start()
    {
        thisTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Если прошло достаточно времени после создания последнего моба
        if (Time.time > timeLastCreateMob + delayTimeCreateMob)
        {
            //создаем моба
            GameObject newMob = MainScript.CreateMob(this, mob, tower.transform);

            //обновляем параметры моба
            foreach (var card in ActionsCards) //для каждого объекта в массиве карт
            {
                if (card)
                {
                    if (card.type == EnumCard.ImprovingCard)// если карта улучшения
                    {
                        ImprovingCard improvingCard = card.GetComponent<ImprovingCard>();
                        if (improvingCard)
                        {
                            improvingCard.ToImprove(newMob);
                        }
                    }
                }
            }

            //указываем время создания последнего моба
            timeLastCreateMob = Time.time;
        }

        //Если прошло достаточно времени после создания последнего моба
        if (Time.time > timeLastCreateMob2 + delayTimeCreateMob2)
        {
            //создаем моба
            GameObject newMob = MainScript.CreateMob(this, mob2, tower.transform);

            //обновляем параметры моба
            foreach (var card in ActionsCards) //для каждого объекта в массиве карт
            {
                if (card)
                {
                    if (card.type == EnumCard.ImprovingCard)// если карта улучшения
                    {
                        ImprovingCard improvingCard = card.GetComponent<ImprovingCard>();
                        if (improvingCard)
                        {
                            improvingCard.ToImprove(newMob);
                        }
                    }
                }
            }

            //указываем время создания последнего моба
            timeLastCreateMob2 = Time.time;
        }
    }

    //передает позицию вражеской башни
    public Transform GetEnemyTowerTransform()
    {
        return enemy.tower.transform;
    }
    


    //работа с картами
    //разыграть карту
    public void PlayCard(Card card)
    {
        if (card.type == EnumCard.ImprovingCard)
        {
            //добавляем карту в список активных
            AddCardInActionsCards(card);
        }

        //убираем карту из руки
        DeleteCardFromHand(card);
    }

    //добавляем карту в список активных карт
    public void AddCardInActionsCards(Card card)
    {
        ActionsCards.Add(card);
    }

    //убираем карту из руки
    public void DeleteCardFromHand(Card card)
    {
        //убираем карту из руки
        CardsInHand.Remove(card);

        //определяем новое место и размер отображения карты
        card.MovingToANewPlace(thisTransform.position, 0.5f);
    }

}
