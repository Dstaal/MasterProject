using UnityEngine;
using System.Collections;

public class windBubleScript : MonoBehaviour {

    private GameObject player;
    private Animator hat_anim;
    private Transform flyingHat;
    private Transform eventSphere;
 //   public eventChecker eventChecker_script;
 //   public HatController hatController_script;

    public GameObject target1;
    public GameObject target2;
    public GameObject target3;
    public GameObject targetLeft;
    public GameObject targetRight;
    public GameObject targetBack;

    public GameObject guide;

    public bool moving = false;


    private Transform dummyHat;
    private float timesFlown = 0;
    private float distance = 0;
   
    private float eventTimer = 0;

    string animationPlaying = "Fly";

    private GameObject nextTarget ;

    // Use this for initialization
    void Start () {

        player = GameController_script.playerRef;

        flyingHat = GameObject.FindGameObjectWithTag("FlyingHat").transform;

   //     eventSphere = GameObject.FindGameObjectWithTag("EventSphere").transform;

   //     eventChecker_script = eventSphere.GetComponentInChildren<eventChecker>();

    //    hatController_script = flyingHat.GetComponentInChildren<HatController>();

        hat_anim = flyingHat.GetComponentInChildren<Animator>();

        moving = false;

    }

    // Update is called once per frame
    void Update()
    {
        

        eventTimer = eventTimer - Time.deltaTime;

   
        
            if (timesFlown >= 1)
            {
                HatController.StaticRef.canPickUp = true;
            }
            if (moving == true || timesFlown <= 0)
            {
                HatController.StaticRef.canPickUp = false;
            }
        

        if (hat_anim.GetCurrentAnimatorStateInfo(0).IsName(animationPlaying) && moving == true && hat_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            {

            MoveHatTo(nextTarget);
            // reset aniamtion pos
            hat_anim.Play("waiting");
            moving = false;

            }

        CheckGuide(25, 12);
    }


    void OnTriggerEnter(Collider other)
    {   
        if (other.tag == "Player")
        {
            Debug.Log("in wind buble tiem to move hat");

            if (HatController.StaticRef.canPickUp == false)
            {
               
                CheckLimits();
                WhichAnimation();

                moving = true;

                timesFlown = timesFlown + 1;
                eventTimer = 1;
            }
        }

    
    }

    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && moving == false)
        {
            Debug.Log("player still inside wind bubble");

            if (HatController.StaticRef.canPickUp == false)
            {
             
                CheckLimits();
                WhichAnimation();

                moving = true;

                timesFlown = timesFlown + 1;
                eventTimer = 1;
            }
        }
    }

    void WhichAnimation()
    {
        int rand = Random.Range(1, 101);

        if (rand >= 1 && rand <= 25)
        {
            animationPlaying = "Fly";
            nextTarget = target1;
        }

        if (rand >= 26 && rand <= 35)
        {
            animationPlaying = "Fly2";
            nextTarget = target2;
        }

        if (rand >= 36 && rand <= 57)
        {
            animationPlaying = "Fly3";
            nextTarget = target3;
        }

        if (rand >= 58 && rand <= 84)
        {
            animationPlaying = "FlyLeft";
            nextTarget = targetLeft;
        }

        if (rand >= 85 && rand <= 90)
        {
            animationPlaying = "FlyRight";
            nextTarget = targetRight;
        }

        if (rand >= 91 && rand <= 101) // need to check targetBack pos ass well
        {
            animationPlaying = "FlyBack";
            nextTarget = targetBack;
        }

        hat_anim.Play(animationPlaying);
        Debug.Log("tryed to play aniamtion" + animationPlaying);
    }

    void MoveHatTo(GameObject targetName)
    {
        Vector3 T_pos = transform.position;
        T_pos.y = Terrain.activeTerrain.SampleHeight(transform.position);

        flyingHat.transform.position = new Vector3(targetName.transform.position.x, T_pos.y, targetName.transform.position.z);

        Debug.Log("animtion done and trying to move hat");

        moving = false;

    }

    void CheckLimits()
    {
        if (this.transform.position.z >= 400)
        {
            Debug.Log("hat passed 480 z an dtrying to rotate");

            flyingHat.transform.Rotate(0, 180, 0, Space.Self);        

        }

        if (this.transform.position.z <= 100)
        {
            Debug.Log("hat under 20 z an dtrying to rotate");

            flyingHat.transform.Rotate(0, 180, 0, Space.Self);

        }
        if (this.transform.position.x >= 400)
        {
            Debug.Log("hat passed 480 x an dtrying to rotate");

            flyingHat.transform.Rotate(0, 180, 0, Space.Self);

        }

        if (this.transform.position.x <= 100)
        {
            Debug.Log("hat under 20 x an dtrying to rotate");

            flyingHat.transform.Rotate(0, 180, 0, Space.Self);

        }
    }

    void CheckGuide(float turnOffRange, float maxSize) // a bug somewhere, sets the scale on fly3 to be more the maxSize
    {
      
        distance = Vector3.Distance(player.transform.position, flyingHat.transform.position) - turnOffRange;

        if (distance >= maxSize)
        {
            distance = maxSize;
        }

        if (distance <= maxSize)
        {
           
            guide.transform.localScale = new Vector3(distance, 25, distance);
        }

        if (distance <= 0)
        {
            guide.SetActive(false);
        }
        if (distance >= 1)
        {
            guide.SetActive(true);
        }
    }


}
