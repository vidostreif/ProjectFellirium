using UnityEngine;
using System.Collections;

public class UIMagic : MonoBehaviour {
    

    public void onMagic1()
    {
        MainScript.magicChoice(0);//выбираем первую магию из списка
    }

    public void onMagic2()
    {
        MainScript.magicChoice(1);//выбираем первую магию из списка
    }

    public void onMagic3()
    {
        MainScript.magicChoice(2);//выбираем первую магию из списка
    }

}
