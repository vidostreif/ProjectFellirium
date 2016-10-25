using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {

    public CommanderAI commander;

	// Use this for initialization
	void Start () {
        //проверка наличия командира
        if (!commander)
        {
            Debug.Log("У объекта " + gameObject.name + " не определен командир!");
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
