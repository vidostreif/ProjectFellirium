using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    public GameObject pauseButton, changeGameSpeedButton, pausePanel;

    //пауза
    public void OnPause()
    {
        Time.timeScale = 0;
        //открываем панель паузы
        pausePanel.SetActive(true);
        //прячем кнопки
        pauseButton.SetActive(false);
        //changeGameSpeedButton.SetActive(false);

    }

    //плей
    public void OnUnPause()
    {
        Time.timeScale = 1;

        //закрываем панель паузы
        pausePanel.SetActive(false);
        //показываем кнопки
        pauseButton.SetActive(true);
        //changeGameSpeedButton.SetActive(false);
    }

    //изменяет скорость игры
    public void ChangeGameSpeed()
    {
        //если двойная скорость меняем на нормальную и наоборот
        if (Time.timeScale == 2)
        {
            Time.timeScale = 1;
        }
        else if (Time.timeScale == 1)
        {
            Time.timeScale = 2;
        }
        
    }
}
