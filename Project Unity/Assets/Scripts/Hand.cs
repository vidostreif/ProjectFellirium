using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

    public List<Card> thisCards;//все карты в руке
    public float length;

    public CommanderAI commander { get; private set; }

    // Use this for initialization
    void Start()
    {

        //указываем командира руки
        commander = GetComponent<Team>().commander;

        //инициализируем карты
        int sizeOfArray = thisCards.Count;// размер массива
        float startPosition = transform.position.x - (length/2);//первая позиция по горизонтали
        float translation = length / sizeOfArray;//смещение

        for (int i = 0; i < sizeOfArray; i++)
        {
            Vector3 newPosition = new Vector3(startPosition + (translation * i), transform.position.y, transform.position.z);
            //создаем карту и прикрепляем ее к руке
            GameObject newCard = Instantiate(thisCards[i].gameObject, newPosition, transform.rotation, transform) as GameObject;

            Team newCardTeam = newCard.GetComponent<Team>();
            //указываем командира карты
            newCardTeam.commander = commander;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}


}
