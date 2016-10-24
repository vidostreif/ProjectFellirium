using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEditor;
using System.Collections.Generic;

public class ImprovingCard : MonoBehaviour {
    [Header("Объекты к которым применяется карта:")]
    public GameObject[] authorizedObjects;//массив объектов к которым будут применяться улучения
    [Header("Компоненты - исключения:")]
    public Component[] exceptionsComponents; //компоненты исключения

    private List<Component> thisComponents;//все компоненты карты

    //процедура активации карты
    public void Activate()
    {
        DeleteOnExceptionsList();//подготовка массива компонентов

        CommanderAI commander = GetComponent<Team>().commander;//командир карты

        //берем все объекты с компонентом Team
        Team[] findeObjects = FindObjectsOfType(typeof(Team)) as Team[];
        foreach (Team findeObject in findeObjects)//для всех найденых объектов
        {
            if (findeObject.commander == commander)//если объект под нашим командованием
            {
                Card findeObjectCardComponent = findeObject.GetComponent<Card>();

                if (findeObjectCardComponent == null)//убеждаемся, что это не карта
                {
                    //передаем его в следующую процедуру для улучшения
                    ToImprove(findeObject.gameObject);
                }
                
            }
        }
    }

    //проверяем что бы переданный объет был в списке объектов для которых применяет улучшение эта карта
    public void ToImprove(GameObject transferredObject)
    {
        //если указаны объекты для которых применяется карта
        if (authorizedObjects.Length > 0)
        {
            foreach (var authorizedObject in authorizedObjects) //для каждого объекта в массиве
            {
                if (authorizedObject != null)//если указан объект, а не пустая ссылка
                {
                    string nameAuthorizedObject = authorizedObject.name + "(Clone)";
                    if (nameAuthorizedObject == transferredObject.name)//сравниваем имена объекта и его префаба
                    {
                        SearchMatchingComponentsToUpdate(transferredObject);
                        break;//если нашли текущий компонент, то прерываем 
                    }
                }
                else//сообщаем, что бы знали что оставили пустую ссылку
                {
                    Debug.Log("В карте " + this.name + " оставили пустую ссылку на объект, на который она должна действовать");
                }
            }
        }
        else//иначе считаем, что карта применяется на все объекты
        {
            SearchMatchingComponentsToUpdate(transferredObject);
        }
    }

    //поиск совпадающих компонентов для обновления
    private void SearchMatchingComponentsToUpdate(GameObject transferredObject) {
        
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

                    //создаем эфект улучшения
                    SpecialEffectsHelper.Instance.ImprovingEffect(transferredObject.transform.position);

                    break;//если нашли текущий компонент, то прерываем для поиска следующего
                }
            }
        }
    }

    private void UpdateParameters(Component objectComponent, Component thisComponent)
    {
        Type program = thisComponent.GetType();
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
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

    private void DeleteOnExceptionsList()
    {
        thisComponents = new List<Component>(GetComponents<Component>());//получить все компоненты

        foreach (var exceptionsComponent in exceptionsComponents) //для каждого исключающего компонента
        {
            //удаляем этот компонент из массива из массива
            thisComponents.Remove(exceptionsComponent);
        }
    }
}
