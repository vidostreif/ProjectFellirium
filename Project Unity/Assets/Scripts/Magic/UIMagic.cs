using UnityEngine;
using System.Collections;

public class UIMagic : MonoBehaviour {
    public Magic commanderMagic;//командир которым управляет игрок

    public void onMagic1()
    {
        commanderMagic.magicChoice(0);//выбираем первую магию из списка
    }

    public void onMagic2()
    {
        commanderMagic.magicChoice(1);//выбираем первую магию из списка
    }

    public void onMagic3()
    {
        commanderMagic.magicChoice(2);//выбираем первую магию из списка
    }

}
