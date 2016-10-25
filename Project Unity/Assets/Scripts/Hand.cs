using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

    public List<Card> Cards;//все карты в руке
    public float length;

    //public CommanderAI commander { get; private set; }
    public Team team { get; private set; }//наша команда
    
    public void Initialization()// ручная инициализация
    {
        team = GetComponent<Team>();

        //инициализируем карты
        int sizeOfArray = Cards.Count;// размер массива
        float startPosition = transform.position.x - (length/2);//первая позиция по горизонтали
        float translation = length / sizeOfArray;//смещение

        for (int i = 0; i < sizeOfArray; i++)
        {
            Vector3 newPosition = new Vector3(startPosition + (translation * i), transform.position.y, transform.position.z);
            //создаем карту и прикрепляем ее к руке
            GameObject newCard = Instantiate(Cards[i].gameObject, newPosition, transform.rotation, transform) as GameObject;

            Team newCardTeam = newCard.GetComponent<Team>();
            //указываем командира карты
            newCardTeam.commander = team.commander;
        }
    }
	
    public void RemoveCard(Card card)//процедура удаления и открепления карты от руки
    {
        Cards.Remove(card);
        card.transform.parent = null;
    }


}
