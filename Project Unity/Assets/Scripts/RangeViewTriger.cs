using UnityEngine;
using System.Collections;

public class RangeViewTriger : MonoBehaviour {

    public bool onTriggerStay = false;
    public Transform target;
    public Team team { get; private set; }//наша команда

    // Use this for initialization
    void Start () {
        //передаем ссылку на командира
        //commander = transform.parent.GetComponent<Team>().commander;
        team = GetComponent<Team>();
    }
	

    void OnTriggerStay2D(Collider2D other)
    {
        PhysicalPerformance otherPhysicalPerformance = other.gameObject.GetComponent<PhysicalPerformance>();

        if (otherPhysicalPerformance != null)
        {
            //эсли это враг и он жив
            if (team.commander.enemy == otherPhysicalPerformance.team.commander && otherPhysicalPerformance.isLive)
            {           
                target = otherPhysicalPerformance.transform;
                onTriggerStay = true;
                //Debug.Log("OnCollisionEnter2D " + coll.gameObject);
            }
        }
    }
}
