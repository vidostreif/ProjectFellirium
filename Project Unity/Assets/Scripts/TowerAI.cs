using UnityEngine;
using System.Collections;

public class TowerAI : MonoBehaviour {

    public CommanderAI commander { get; private set; }
    //public float minAttackDistance = 3;
    //public float maxAttackDistance = 10;

    private LongRangeWeapon thisLongRangeWeapon;
    private PhysicalPerformance thisPhysicalPerformance;

    // Use this for initialization
    void Start () {
        commander = GetComponent<Team>().commander;
        thisLongRangeWeapon = GetComponent<LongRangeWeapon>();
        thisPhysicalPerformance = GetComponent<PhysicalPerformance>();

        ////и ищем своего командира
        //GameObject[] commanders = GameObject.FindGameObjectsWithTag("Commander");
        //foreach (GameObject commander in commanders)
        //{
        //    CommanderAI commanderAI = commander.GetComponent<CommanderAI>();
        //    if (commanderAI)
        //    {
        //        if (commanderAI.tower == this.gameObject)//если у командира указана эта башня 
        //        {
        //            Commander = commanderAI;
        //        }
        //    }
        //}

        //if (Commander == null)
        //{
        //    Debug.Log("Башня " + gameObject + " не нашла своего командира!");
        //}

    }
	
	// Update is called once per frame
	void FixedUpdate() {

        //если убили башню
        if (!thisPhysicalPerformance.isLive)
        {
            Debug.Log("Башня " + gameObject.ToString() + " проиграла!");
            //рестар уровня
            Application.LoadLevel(Application.loadedLevel);
        }

        //if (commander)//если есть командир
        //{
        //    //находим цель
        //    GameObject target = MainScript.TargetSelection(transform, commander, thisPhysicalPerformance.GetAttackDistance());

        //    //если есть цель, то стреляем
        //    if (target != null)
        //    {
        //        //стреляем
        //        thisLongRangeWeapon.Shot(target, commander);
        //    }
        //}
        //else
        //{
        //     //сделать визуальное представление отсутствия командира
        //}


    }
}
