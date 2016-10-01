using UnityEngine;
using System.Collections;

public class TowerAI : MonoBehaviour {

    public CommanderAI Commander { get; private set; }
    public float minAttackDistance = 3;
    public float maxAttackDistance = 10;
    private LongRangeWeapon thisLongRangeWeapon;

    // Use this for initialization
    void Start () {

        thisLongRangeWeapon = GetComponent<LongRangeWeapon>();
        GameObject[] Commanders = GameObject.FindGameObjectsWithTag("Commanders"); //находим всех командиров
        //и ищем своего командира
        foreach (GameObject commander in Commanders) 
        {
            CommanderAI commanderAI = commander.GetComponent<CommanderAI>();
            if (commanderAI)
            {
                if (commanderAI.tower == this.gameObject)//если у командира указана эта башня 
                {
                    Commander = commanderAI;
                }
            }
        }

        if (Commander == null)
        {
            Debug.Log("Башня " + gameObject + " не нашла своего командира!");
        }

    }
	
	// Update is called once per frame
	void FixedUpdate() {

        if (Commander)//если есть командир
        {
            //находим цель
            GameObject target = MainScript.TargetSelection(transform, Commander, maxAttackDistance, minAttackDistance);

            //если есть цель, то стреляем
            if (target != null)
            {
                //стреляем
                thisLongRangeWeapon.Shot(target, Commander);
            }
        }
        else
        {
             //сделать визуальное представление отсутствия командира
        }


    }
}
