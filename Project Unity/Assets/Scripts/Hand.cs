using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

    public List<Card> thisCards;//все карты в руке
    public float length;

    // Use this for initialization
    void Start()
    {
        int sizeOfArray = thisCards.Count;// размер массива
        float startPosition = transform.position.x - (length/2);//первая позиция по горизонтали
        float translation = length / sizeOfArray;//смещение

        for (int i = 0; i <= sizeOfArray; i++)
        {
            Vector3 newPosition = new Vector3(startPosition + (translation * i), transform.position.y, transform.position.z);
            //создаем карту
            GameObject newCard = Instantiate(thisCards[i], newPosition, transform.rotation) as GameObject;

        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}


}
