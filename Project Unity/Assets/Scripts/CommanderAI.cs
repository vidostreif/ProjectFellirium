using UnityEngine;
using System.Collections;
//using System;
using System.Collections.Generic;

public class CommanderAI : MonoBehaviour {

    public GameObject tower;//башня командира

    public float delayTimeCreateMob = 5; //пауза между соданиями моба
    public float delayTimeCreateMob2 = 6;
    public GameObject mob;//префаб моба
    public GameObject mob2;

    public bool managePlayer = false;//признак перехвата управления игроком

    public CommanderAI enemy;//командир враг
    public CommanderAI friend;//командир друг

    public List<Card> CardsInDeck; //массив карты в колоде
    public List<Card> ActionsCards; //активные карты

    private Hand hand; //рука с картами
    private Card selectionCard;//выбранная карта

    private float timeLastCreateMob;
    private float timeLastCreateMob2;
    private Transform thisTransform;

    public int energy = 0; //количество энергии
    private int pauseToIncreaseEnergy = 3;//время для увеличения энергии
    private float lastTimeToIncreaseEnergy = 0;//последний момент увеличения энергии

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

    void Update()
    {
        if (!managePlayer)//если не управляется игроком
        {
            CardSelectionImitation();

            int random = Random.Range(0, 50);

            if (selectionCard != null && random > 48)
            {
                if (selectionCard.cost <= energy)//если достаточно энергии
                {
                    PlayCard(selectionCard);//разыгрываем карту
                    selectionCard = null;
                }
            }
        }

        IncreaseEnergy();//проверка и увеличение количество энергии
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

    //работа с энергией
    private void IncreaseEnergy()
    {
        if (Time.time > lastTimeToIncreaseEnergy + pauseToIncreaseEnergy && energy < 10)//если прошло времени больше чем пауза и энергии меньше 10
        {
            energy++;//увеличиваем энергию на еденицу
            lastTimeToIncreaseEnergy = Time.time;
        }
    }

    //работа с картами
    //разыграть карту
    public void PlayCard(Card card)
    {
        if (card.cost <= energy)//если достаточно энергии
        {
            energy = energy - card.cost; //уменьшаем количество энергии

            AddCardInActionsCards(card);//добавляем карту в список активных

            card.Activate();//активировать карту

            //убираем карту из руки
            hand.RemoveCard(card);
            //назначаем нового родителя
            card.transform.parent = thisTransform;
            //определяем новое место и размер отображения карты
            MainScript.Instance.SlowlyMoveToNewPosition(card.transform, thisTransform.position, 0.5f);
        }
    }

    //добавляем карту в список активных карт
    public void AddCardInActionsCards(Card card)
    {
        ActionsCards.Add(card);
    }

    private void CardSelectionImitation()//имитация выбора карты
    {
        if (hand.Cards.Count > 0)//если в руке есть карты
        {
            int random = Random.Range(0, 50);

            if (selectionCard == null && random > 48)//если не определена выбранная карта с малой вероятностью определяем ее и поднимаем
            {
                int cardNumberForSelection = Random.Range(0, hand.Cards.Count);//рандомно определяем номер карты

                selectionCard = hand.Cards[cardNumberForSelection];// определяем выбранную карту

                selectionCard.PointerEnter();//имитируем наведение на карту для ее поднятия
            }
            else
            {
                if (random > 48)//с малой вероятностью возвращяем карту на место
                {
                    selectionCard.PointerExit();//возвращаем карту на место
                    selectionCard = null;
                }
            }
        }

    }
}
