using UnityEngine;
using System.Collections;

public class RangeViewTriger : MonoBehaviour {

    public bool onTriggerStay = false;
    public Transform target;
    public CommanderAI commander { get; private set; }

    // Use this for initialization
    void Start () {
        //передаем ссылку на командира
        commander = transform.parent.GetComponent<Team>().commander;
    }
	

    void OnTriggerStay2D(Collider2D other)
    {
        PhysicalPerformance otherPhysicalPerformance = other.gameObject.GetComponent<PhysicalPerformance>();

        if (otherPhysicalPerformance != null)
        {
            //эсли это враг и он жив
            if (commander.enemy == otherPhysicalPerformance.commander && otherPhysicalPerformance.isLive)
            {           
                target = otherPhysicalPerformance.transform;
                onTriggerStay = true;
                //Debug.Log("OnCollisionEnter2D " + coll.gameObject);
            }
        }
    }
}
