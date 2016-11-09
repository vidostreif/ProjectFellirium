using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

    public List<Card> Cards;//все карты в руке
    public float length;//ширина руки
    public float cardWidth;//ширина карты

    //public CommanderAI commander { get; private set; }
    public Team team { get; private set; }//наша команда
    
    public void Initialization()// ручная инициализация
    {
        team = GetComponent<Team>();

        //инициализируем карты
        for (int i = 0; i < Cards.Count; i++)
        {            
            //создаем карту и прикрепляем ее к руке
            GameObject newCard = Instantiate(Cards[0].gameObject, transform.position, transform.rotation, transform) as GameObject;

            Cards.Remove(Cards[0]);//удаляем префаб из массива
            Cards.Add(newCard.GetComponent<Card>());//добавляем созданную карту в массив

            Team newCardTeam = newCard.GetComponent<Team>();
            //указываем командира карты
            newCardTeam.commander = team.commander;
        }

        RedrawCards();//перерисовать карты
    }
	
    public void RemoveCard(Card card)//процедура удаления и открепления карты от руки
    {
        Cards.Remove(card);
        card.transform.parent = null;
        RedrawCards();
    }

    public void RedrawCards()//перерисовать карты
    {        
        int sizeOfArray = Cards.Count;// размер массива
        float widthCards = cardWidth * sizeOfArray;//считаем склоько в ширину все карты в руке
        if (length > widthCards)//если ширина руки больше ширина всех карт в руке, то уменьшаем ширину руки
        {
            length = widthCards;
        }        
        float startPosition = 0 - (length / 2) + (length/sizeOfArray/2);//первая позиция по горизонтали
        float translation = length / sizeOfArray;//смещение

        //смещаем все карты на новые позиции
        for (int i = 0; i < Cards.Count; i++)
        {
            Vector3 newPosition = new Vector3(startPosition + (translation * i), 0, 0);
            Cards[i].startLocalPosition = newPosition;
            MainScript.Instance.SlowlyMoveToNewLocalPosition(Cards[i].transform, newPosition, 1, 4);
        }
    }

}
