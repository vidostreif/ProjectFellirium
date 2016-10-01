using UnityEngine;
using System.Collections;

public class MobAnimator : MonoBehaviour {
    public bool destroyThisObject = false;
    public float velocity;

    private Rigidbody2D thisRigidbody2D;
    private PhysicalPerformance thisPhysicalPerformance;
    private Animator thisAnimator;
    // Use this for initialization
    void Start () {
        //ссылка на тело моба
        thisPhysicalPerformance = GetComponent<PhysicalPerformance>();
        //ссылка на физику
        thisRigidbody2D = GetComponent<Rigidbody2D>();
        //ссылка на аниматор
        thisAnimator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {

        //Если закончилась анимация смерти, то удаляем этого моба
        if (destroyThisObject)
        {
            Destroy(this.gameObject);
        }

        velocity = thisRigidbody2D.velocity.magnitude;
        //указываем скорость передвижения по оси Х
        thisAnimator.SetFloat("Speed", velocity);
        //указываем жив ли моб
        thisAnimator.SetBool("Dead", !thisPhysicalPerformance.isLive);

    }
}
