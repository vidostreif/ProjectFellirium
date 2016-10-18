﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MagicCard))] //Сообщаем редактору, что это класс для кастомизации вкладки инспектора, компонента "MagicCard"
class TestCustomize : Editor
{ //Наследуем наш класс кастомизации, от редактора Юнити

    public override void OnInspectorGUI()
    { //Сообщаем редактору, что этот инспектор заменит прежний (встроеный)

        MagicCard myMagicCard = (MagicCard)target;

        myMagicCard.magic = (EnumMagic)EditorGUILayout.EnumPopup("Магия", myMagicCard.magic);//выбор магии

        //в зависимости от выбранной магии показываем разные параметры, что бы не путаться
        //если град стрел
        if (myMagicCard.magic == EnumMagic.StormOfArrows)
        {
            myMagicCard.bulletPrefab = (GameObject)EditorGUILayout.ObjectField("Префаб стрелы", myMagicCard.bulletPrefab, typeof(GameObject));
            myMagicCard.damageOfArrows = EditorGUILayout.FloatField("Урон каждой стрелы", myMagicCard.damageOfArrows);
            myMagicCard.numberOfArrows = EditorGUILayout.IntField("Количество стрел", myMagicCard.numberOfArrows);
        }
        else if (myMagicCard.magic == EnumMagic.Explosion)
        {

        }
        

    }
}