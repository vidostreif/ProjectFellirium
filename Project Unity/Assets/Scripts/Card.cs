using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class Card : MonoBehaviour {
    public int odin;
    public int dva;
    private int tri;
    public int chetyre;

    // Use this for initialization
    void Start () {

        Component thisPhysicalPerformance = GetComponent<PhysicalPerformance>();
        UpdateParameters(thisPhysicalPerformance);
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void UpdateParameters(Component p)
    {
        //Card p = new Card();
        Type program = p.GetType();
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
        FieldInfo[] fields = program.GetFields(flags);
        string[] MasText = new string[fields.Length];
        for (int i = 0; i < fields.Length; i++)
            //MasText[i] = (string)fields[i].GetValue(p);
            Debug.Log(fields[i].GetValue(p));
        //Array.ForEach(MasText, Console.WriteLine);
    }
}
