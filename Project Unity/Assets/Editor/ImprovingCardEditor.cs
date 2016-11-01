using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

[CustomEditor(typeof(ImprovingCard))] //Сообщаем редактору, что это класс для кастомизации вкладки инспектора, компонента "ImprovingCard"
//[CanEditMultipleObjects]
class ImprovingCardEditor : Editor
{ //Наследуем наш класс кастомизации, от редактора Юнити

    GameObject[] overallGameObject;

    public override void OnInspectorGUI()
    { //Сообщаем редактору, что этот инспектор заменит прежний (встроеный)

        base.OnInspectorGUI();//отрисовка стандартного интерфейса

        EditorGUILayout.Separator();//отступ

        ImprovingCard myImprovingCard = (ImprovingCard)target;

        //Создание своего стиля текста
        GUIStyle myStyle = new GUIStyle();
        myStyle.richText = true;
        myStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.LabelField("Массив параметров для изменения:", myStyle);

        int LastSizeOfArray = myImprovingCard.sizeOfArray; //сохраняем последний размер массива
        myImprovingCard.sizeOfArray = EditorGUILayout.IntField("Размер массива", myImprovingCard.sizeOfArray);

        if (LastSizeOfArray != myImprovingCard.sizeOfArray)//если изменили размер массива, то пересоздаем массив объектов
        {
            myImprovingCard.arrayForImproving = new ImprovingCard.StructForImproving[myImprovingCard.sizeOfArray];
            overallGameObject = new GameObject[myImprovingCard.sizeOfArray];
        }

        if (overallGameObject == null)//если массив не инициализирован
        {
            overallGameObject = new GameObject[myImprovingCard.sizeOfArray];
        }

        if (myImprovingCard.arrayForImproving != null)//если инициализирован массив
        {
            for (int i = 0; i < myImprovingCard.arrayForImproving.Length; i++)
            {
                EditorGUI.indentLevel = 1;//сдвиг названий

                overallGameObject[i] = (GameObject)EditorGUILayout.ObjectField("Объект", overallGameObject[i], typeof(GameObject));

                if (overallGameObject[i] != null)
                {
                    Component[] objectComponents = overallGameObject[i].GetComponents<Component>();//находим все компоненты объекта
                    
                    string[] allComponentsName = FindComponentsName(overallGameObject[i]);//все названия компонентов

                    //myImprovingCard.arrayForImproving[i].components = new ImprovingCard.StructForImproving.StructComponents[allComponentsName.Length];
                    //for (int j = 0; j < myImprovingCard.arrayForImproving[i].components.Length; j++)//перебор всех компонентов
                    //{
                    //    myImprovingCard.arrayForImproving[i].components[j].componentName = allComponentsName[j];//название компонента
                    //    myImprovingCard.arrayForImproving[i].components[j].variablesNames = FindParametersName(objectComponents[j]);//названия переменных компонента
                    //}

                    List<ImprovingCard.StructForImproving.StructComponents> temporaryArray = new List<ImprovingCard.StructForImproving.StructComponents>();//временный список для выделения только значимых компонентов
                    for (int j = 0; j < allComponentsName.Length; j++)//перебор всех компонентов
                    {
                        string[] variablesNames = FindParametersName(objectComponents[j]);//названия переменных компонента

                        if (variablesNames.Length > 0)//если есть хоть одна доступная переменная, то добавляем в массив
                        {
                            ImprovingCard.StructForImproving.StructComponents thisStruct = new ImprovingCard.StructForImproving.StructComponents(allComponentsName[j], variablesNames);//создаем структуру с нужными параметрами
                            //thisStruct.componentName = allComponentsName[j];//название компонента
                            //thisStruct.variablesNames = variablesNames;
                            temporaryArray.Add(thisStruct);
                        }
                    }

                    myImprovingCard.arrayForImproving[i].components = temporaryArray.ToArray();//сохраняем полученные данные в наш массив

                    myImprovingCard.arrayForImproving[i].indexComponent = 0;//обнуляем индекс выбранного компонента
                    overallGameObject[i] = null;//удаляем объект
                }

                if (myImprovingCard.arrayForImproving[i].components != null)//если инициализирована структура
                {
                    EditorGUI.indentLevel = 2;//сдвиг названий

                    string[] nameComponents = new string[myImprovingCard.arrayForImproving[i].components.Length]; //обновляем массив названий компонентов             
                    for (int j = 0; j < nameComponents.Length; j++)//перебор всех компонентов
                    {
                        nameComponents[j] = myImprovingCard.arrayForImproving[i].components[j].componentName;
                    }

                    int LastSelectedindexComponent = myImprovingCard.arrayForImproving[i].indexComponent; //сохраняем индекс выбранного ранее компонента

                    myImprovingCard.arrayForImproving[i].indexComponent = EditorGUILayout.Popup("Компонент", myImprovingCard.arrayForImproving[i].indexComponent, nameComponents);//выбираем компонент

                    if (LastSelectedindexComponent != myImprovingCard.arrayForImproving[i].indexComponent)//если выбрали другой компонет, то збрасываем индекс выбранного параметра
                    {
                        myImprovingCard.arrayForImproving[i].indexParameter = 0;
                    }

                    myImprovingCard.arrayForImproving[i].indexParameter = EditorGUILayout.Popup("Параметр", myImprovingCard.arrayForImproving[i].indexParameter, myImprovingCard.arrayForImproving[i].components[myImprovingCard.arrayForImproving[i].indexComponent].variablesNames);//выбираем параметр

                    myImprovingCard.arrayForImproving[i].value = EditorGUILayout.IntField("Значение", myImprovingCard.arrayForImproving[i].value);
                }
            }
        }
    }

    private string[] FindComponentsName(GameObject thisGameObject)//находим все компоненты и возвращаем массив с их названиями
    {
        Component[] objectComponents = thisGameObject.GetComponents<Component>();//находим все компоненты объекта
        string[] nameComponents = new string[objectComponents.Length];//создаем массив
        for (int i = 0; i < objectComponents.Length; i++)//перебор всех компонентов
        {
            System.Type thisType = objectComponents[i].GetType();
            nameComponents[i] = thisType.ToString();
        }

        return nameComponents;//возвращаем массив названий найденных переменных
    }

    private string[] FindParametersName(Component thisComponent)//находим все доступные переменные и возвращаем массив с их названиями
    {
        //List<string> nameParameters = new List<string>();
        Type program = thisComponent.GetType();
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        FieldInfo[] fields = program.GetFields(flags);

        string[] nameParameters = new string[fields.Length];//создаем массив

        for (int i = 0; i < fields.Length; i++)//перебор всех переменных
        {
            var thisValue = fields[i].GetValue(thisComponent);

            if (thisValue != null)
            {
                if (thisValue.GetType() == typeof(int) || thisValue.GetType() == typeof(float))//если тип целочисленный или с плавающей запятой
                {
                    nameParameters[i] = fields[i].Name;//добавляем в массив названий переменных
                }
            }
        }

        return nameParameters;//возвращаем массив названий найденных переменных
    }
}



