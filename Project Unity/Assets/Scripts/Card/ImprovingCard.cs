using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEditor;
using System.Collections.Generic;

public class ImprovingCard : MonoBehaviour {
    [Header("Объекты к которым применяется карта:")]
    public GameObject[] authorizedObjects;//массив объектов к которым будут применяться улучения

    [HideInInspector]
    [System.Serializable]
    public struct StructForImproving// структура данных для улучшения
    {
        [System.Serializable]
        public struct StructComponents
        {
            public string componentName;//название компонента
            public string[] variablesNames;//Все переменные

            public StructComponents(string componentName, string[] variablesNames)//конструктор
            {
                this.componentName = componentName;
                this.variablesNames = variablesNames;
            }
        }

        public StructComponents[] components;//описание всех компонентов

        public int indexComponent; //индекс компонента
        public int indexParameter; //индекс параметра
        public int value; //значение улучшения
 
    }
    [HideInInspector]
    public StructForImproving[] arrayForImproving;//список структур данных для улучшения
    [HideInInspector]
    public int sizeOfArray;

    //процедура активации карты
    public void Activate()
    {
        //DeleteOnExceptionsList();//подготовка массива компонентов

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
        foreach (var ForImproving in arrayForImproving) //для каждого объекта в массиве
        {

            string componentNameForImproving = ForImproving.components[ForImproving.indexComponent].componentName;//берем название компонента для обновления
            string parametrNameForImproving = ForImproving.components[ForImproving.indexComponent].variablesNames[ForImproving.indexParameter];//берем название переменной для обновления

            foreach (var objectComponent in objectComponents) //для каждого объекта в массиве
            {
                System.Type objectType = objectComponent.GetType();

                string objectComponentName = objectType.ToString();//название компонента объекта

                if (componentNameForImproving == objectComponentName)//если типы компонентов совпадают
                {
                    UpdateParameters(objectComponent, parametrNameForImproving, ForImproving.value);//обновляем параметры объекта

                    //создаем эфект улучшения
                    SpecialEffectsHelper.Instance.ImprovingEffect(transferredObject.transform.position);

                    break;//если нашли текущий компонент, то прерываем для поиска следующего
                }
            }
        }
    }

    private void UpdateParameters(Component objectComponent, string parametrName, int thisValue)//поиск в компоненте нужной переменной и обновление ее значение
    {
        //выбираем все доступные переменные переданного компонента
        Type program = objectComponent.GetType();
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        FieldInfo[] fields = program.GetFields(flags);

        string objectParameterName = "";
        for (int i = 0; i < fields.Length; i++)//перебор всех переменных
        { 
            //var thisValue = fields[i].GetValue(thisComponent);
            var objectValue = fields[i].GetValue(objectComponent);
            if (objectValue != null)
            {
                objectParameterName = fields[i].Name;
                if (objectParameterName == parametrName)//если название переменной совпадает, то обнавляем значение
                {
                    if (objectValue.GetType() == typeof(int))//если тип целочисленный
                    {
                        fields[i].SetValue(objectComponent, (int)objectValue + (int)thisValue);
                    }
                    if (objectValue.GetType() == typeof(float))//если тип число с плавающей запятой
                    {
                        fields[i].SetValue(objectComponent, (float)objectValue + (float)thisValue);
                    }
                }
            }
            //Debug.Log(fields[i].GetValue(objectComponent));
        }
    }

    //private void DeleteOnExceptionsList()
    //{
    //    thisComponents = new List<Component>(GetComponents<Component>());//получить все компоненты

    //    foreach (var exceptionsComponent in exceptionsComponents) //для каждого исключающего компонента
    //    {
    //        //удаляем этот компонент из массива из массива
    //        thisComponents.Remove(exceptionsComponent);
    //    }
    //}
}
