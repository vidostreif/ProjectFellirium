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
    public List<Card> ActionsCards; //активные карты

    private Hand hand; //рука с картами
    private float timeLastCreateMob;
    private float timeLastCreateMob2;
    private Transform thisTransform;

    // Use this for initialization
    void Start()
    {
        //находим руку с картами
        Transform handTransform = transform.FindChild("Hand");
        hand = handTransform.GetComponent<Hand>();
        if (!hand)//если не нашли руку
        {
            Debug.LogError("Командир " + this + " потерял руку! Добавьте ему руку в качестве дочернего объекта");
        }
        else
        {
            
            if (managePlayer)//если командиром управляет игрок, то прикрепим руку камере
            {
                Transform mainCameraTransform = Camera.main.transform;

                float height = Camera.main.orthographicSize * 2;
                Bounds bounds = new Bounds(Vector3.zero, new Vector3(height * Camera.main.aspect, height, 0));
                float minCamY = bounds.min.y;

                hand.transform.position = new Vector3(mainCameraTransform.position.x, mainCameraTransform.position.y + minCamY + 2, mainCameraTransform.position.z + 1);
                hand.transform.parent = Camera.main.transform;
            }

            //назначим себя командиром руки
            handTransform.GetComponent<Team>().commander = this;
            //инициализация руки
            handTransform.GetComponent<Hand>().Initialization();
        }



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

        //добавляем карту в список активных
        AddCardInActionsCards(card);

        //активировать карту
        card.Activate();

        ////убираем карту из руки
        //DeleteCardFromHand(card);

        //убираем карту из руки
        hand.RemoveCard(card);
        //назначаем нового родителя
        card.transform.parent = thisTransform;
        //определяем новое место и размер отображения карты
        MainScript.Instance.SlowlyMoveToNewPosition(card.transform, thisTransform.position, 0.5f);
    }

    //добавляем карту в список активных карт
    public void AddCardInActionsCards(Card card)
    {
        ActionsCards.Add(card);
    }

    ////убираем карту из руки
    //public void DeleteCardFromHand(Card card)
    //{
    //    //убираем карту из руки
    //    hand.RemoveCard(card);
    //    //назначаем нового родителя
    //    card.transform.parent = thisTransform;
    //    //определяем новое место и размер отображения карты
    //    MainScript.Instance.SlowlyMoveToNewPosition(card.transform, thisTransform.position, 0.5f);
    //}


}
