using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEditor;

public class ImprovingCard : MonoBehaviour {

    public GameObject[] authorizedObjects;//массив объектов к которым будут применяться улучения

    // Use this for initialization
    void Start () {
        ////отключаем все скрипты кроме этого
        //Component[] thisComponents = GetComponents<Component>();
        //foreach (Component thisComponent in thisComponents) //для каждого объекта в массиве
        //{
        //    //System.Type thisType = thisComponent.GetType();

        //    if (thisComponent != this)
        //    {
        //        DisableComponent(thisComponent);
        //    }
        //}
    }

    //проверяем что бы переданный объет был в списке объектов для которых применяет улучшение эта карта
    public void ToImprove(GameObject transferredObject)
    {
        foreach (var authorizedObject in authorizedObjects) //для каждого объекта в массиве
        {

            string nameAuthorizedObject = authorizedObject.name + "(Clone)";
            if (nameAuthorizedObject == transferredObject.name)//сравниваем имена объекта и его префаба
            {
                SearchMatchingComponentsToUpdate(transferredObject);
            }

        }
    }
    //поиск совпадающих компонентов для обновления
    private void SearchMatchingComponentsToUpdate(GameObject transferredObject) {
        Component[] thisComponents = GetComponents<Component>();
        Component[] objectComponents = transferredObject.GetComponents<Component>();
        foreach (var thisComponent in thisComponents) //для каждого объекта в массиве
        {
            System.Type thisType = thisComponent.GetType();

            foreach (var objectComponent in objectComponents) //для каждого объекта в массиве
            {
                System.Type objectType = objectComponent.GetType();

                if (thisType == objectType)//если типы компонентов совпадают
                {
                    UpdateParameters(objectComponent, thisComponent);//обновляем параметры объекта
                    break;//если нашли текущий компонент, то прерываем для поиска следующего
                }
            }
        }
    }

    private void UpdateParameters(Component objectComponent, Component thisComponent)
    {
        Type program = thisComponent.GetType();
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
        FieldInfo[] fields = program.GetFields(flags);

        for (int i = 0; i < fields.Length; i++)//перебор всех переменных
        { 
            var thisValue = fields[i].GetValue(thisComponent);
            var objectValue = fields[i].GetValue(objectComponent);
            if (objectValue != null)
            {
                if (thisValue.GetType() == typeof(int))//если тип целочисленный
                {
                    fields[i].SetValue(objectComponent, (int)objectValue + (int)thisValue);
                }
                if (thisValue.GetType() == typeof(float))//если тип число с плавающей запятой
                {
                    fields[i].SetValue(objectComponent, (float)objectValue + (float)thisValue);
                }
            }
            //Debug.Log(fields[i].GetValue(objectComponent));
        }
    }

    //private void DisableComponent(Component thisComponent)//отключение скрипта в независимости от его типа
    //{
    //    Type program = thisComponent.GetType();
    //    BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
    //    FieldInfo[] fields = program.GetFields(flags);

    //    for (int i = 0; i < fields.Length; i++)//перебор всех переменных
    //    {
    //        var thisValue = fields[i].GetValue(thisComponent);

    //        if (thisValue != null)
    //        {

    //        }
    //        //Debug.Log(fields[i].GetValue(objectComponent));
    //    }
    //}
}
