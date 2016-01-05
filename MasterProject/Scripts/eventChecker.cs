using UnityEngine;
using System.Collections;

public class eventChecker : MonoBehaviour {

    public enum HatState { No, Yes, PickingUp };

    public static eventChecker StaticRef;

    public GameObject player;
    private Transform hat;
    public Transform book;
    private Transform hatSpwanTarget;
    public charcterMovemnets moveController;
    public GameObject stromPrefab;
    public GameObject flyingHatPrefab_C;
    public GameObject flyingHatPrefab_R;



    public HatState haveHat;

    private float eventCounter = 0;
    private float eventTimer = 0;

    public  float storedY = 0;
    public Vector3 storedPos;

  //  public Animator anim;
    public Animator book_anim;

    private Animator hat_anim;
    private SphereCollider sphereRadius;


    private GameObject flyingHat;
    private float distance;


    // Use this for initialization
    void Awake() {

        if (StaticRef == null) // if theres is not allerdaye a static ref makes this the static ref
        {
            DontDestroyOnLoad(gameObject);
            StaticRef = this;
        }
        else if (StaticRef != this) // is the allready is a ref , destory this
        {
            Destroy(this);
        }

        player = GameController_script.playerRef;

        sphereRadius = this.GetComponent<SphereCollider>();

        hat = GameObject.FindGameObjectWithTag("hat").transform;

        book = GameObject.FindGameObjectWithTag("book").transform;

        book_anim = book.GetComponentInChildren<Animator>();

        hat.gameObject.SetActive(true);

        book.gameObject.SetActive(false);

        haveHat = HatState.Yes;

        hatSpwanTarget = GameObject.FindGameObjectWithTag("hatSpawn").transform;

    }

    // Update is called once per frame
    void Update() {

        eventTimer = eventTimer - Time.deltaTime;

        if(GameController_script.animRef != null)
        {

            if (eventTimer <= 0)
            {

                if (GameController_script.animRef.GetCurrentAnimatorStateInfo(0).IsName("eventOne") || GameController_script.animRef.GetCurrentAnimatorStateInfo(0).IsName("eventTwo") || GameController_script.animRef.GetCurrentAnimatorStateInfo(0).IsName("eventThree") || GameController_script.animRef.GetCurrentAnimatorStateInfo(0).IsName("eventFour"))
                {
                    // Debug.Log("event animtion playing");
                    GameController_script.animRef.SetBool("IsWalking", false);
                    GameController_script.animRef.SetBool("IsRunning", false);
                }

                //      if (!anim.GetCurrentAnimatorStateInfo(0).IsName("eventOne") && !anim.GetCurrentAnimatorStateInfo(0).IsName("eventTwo") && !anim.GetCurrentAnimatorStateInfo(0).IsName("eventThree") && !anim.GetCurrentAnimatorStateInfo(0).IsName("eventFour"))
                if (GameController_script.animRef.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
                {
                    // Debug.Log("event animtion NOT playing");
                    charcterMovemnets.StaticRef.CurrentState = charcterMovemnets.State.canMove;

                }

                if (GameController_script.animRef.GetCurrentAnimatorStateInfo(0).IsName("eventThree") && GameController_script.animRef.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.80f)
                {

                    if (haveHat == HatState.Yes)
                    {
                        Debug.Log(GameController_script.StaticRef.currentScenario);

                        if (GameController_script.StaticRef.currentScenario == GameController_script.scenario.scenario_C)
                        {
                            flyingHat = SpwanHat(flyingHatPrefab_C);
                            eventTimer = 1;
                        }


                        if (GameController_script.StaticRef.currentScenario == GameController_script.scenario.scenario_R)
                        {
                            flyingHat = SpwanHat(flyingHatPrefab_R);
                            eventTimer = 1;
                        }

                        hat_anim = flyingHat.GetComponentInChildren<Animator>();

                        haveHat = HatState.No; // removeing this. spwan loads of hats. could be a funny outtake clip
                        hat.gameObject.SetActive(false);
                        GameController_script.animRef.SetBool("haveHat", false);

                    }
                }
            }
        }

        if (haveHat == HatState.PickingUp )
        {
            Debug.Log("in pickup state");

            charcterMovemnets.StaticRef.CurrentState = charcterMovemnets.State.inCutScreen;

            GameController_script.animRef.Play("PickUp");
            hat_anim.Play("PickedUp"); 

            if (hat_anim.GetCurrentAnimatorStateInfo(0).IsName("PickedUp") && hat_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.87f)
            {

                Destroy(flyingHat, 0f);
                haveHat = HatState.Yes; 
                hat.gameObject.SetActive(true);
                GameController_script.animRef.SetBool("haveHat", true);

                charcterMovemnets.StaticRef.CurrentState = charcterMovemnets.State.canMove;

            }

        }

        if (GameController_script.animRef != null)
        {

            if (GameController_script.animRef.GetCurrentAnimatorStateInfo(0).IsName("open"))
            {
                book.gameObject.SetActive(true);
                book_anim.Play("openBook");
                charcterMovemnets.StaticRef.CurrentState = charcterMovemnets.State.inCutScreen;
            }

            if (GameController_script.animRef.GetCurrentAnimatorStateInfo(0).IsName("open") && GameController_script.animRef.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && GameController_script.StaticRef.currentState == GameController_script.State.running)
            {

                GameController_script.StaticRef.mainMenu.alpha = 1;
                GameController_script.StaticRef.mainMenu.blocksRaycasts = true;
                GameController_script.StaticRef.mainMenu.interactable = true;

                Debug.Log("trying to toggle menu after anition open");
                GameController_script.StaticRef.currentState = GameController_script.State.options;
            }

            if (GameController_script.animRef.GetCurrentAnimatorStateInfo(0).IsName("close"))
            {
                book_anim.Play("closeBook");
                GameController_script.StaticRef.mainMenu.alpha = 0;
                GameController_script.StaticRef.mainMenu.blocksRaycasts = false;
                GameController_script.StaticRef.mainMenu.interactable = false;
            }
            if (GameController_script.animRef.GetCurrentAnimatorStateInfo(0).IsName("close") && GameController_script.animRef.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && GameController_script.StaticRef.currentState == GameController_script.State.options)
            {

                Debug.Log("trying to toggle menu after anition close");
                GameController_script.StaticRef.currentState = GameController_script.State.running;
                book.gameObject.SetActive(false);
                GameController_script.animRef.Play("idel");
                charcterMovemnets.StaticRef.CurrentState = charcterMovemnets.State.canMove;
            }
        }


    }

    
    void OnTriggerExit(Collider other)
    {
        
        // move event spehre
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        if (GameController_script.animRef != null)
        {
            if (eventCounter ==2) //bug test mode, fix order
            {
                PlayEvent("eventOne");
                makeSandStrom();
                eventCounter = eventCounter + 1;
            }
            if (eventCounter == 1)
            {
                PlayEvent("eventTwo");
                makeSandStrom();
                eventCounter = eventCounter + 1;
            }
            if (eventCounter == 0)
            {
                PlayEvent("eventThree");
                makeSandStrom();
                eventCounter = eventCounter + 1;

                storedY = player.transform.position.y; // importen, store and remembers the play y pos, for then the it is picked up, so it can be placed on his head again
                storedPos = player.transform.position;
            }
       

            if (haveHat == HatState.No)
            {

                distance = Vector3.Distance(player.transform.position, flyingHat.transform.position);
                Debug.Log("player - hat dist = : " + distance);



                if (distance >= 22)  
                {
                    Debug.Log("x distance grewater 22");
                    GameController_script.animRef.Play("eventFour");
                    charcterMovemnets.StaticRef.CurrentState = charcterMovemnets.State.inCutScreen;
                    eventTimer = 1;
                    sphereRadius.radius = 35;
                }

            }
        }
    }

    void makeSandStrom()
    {
        GameObject newStrom = (GameObject)Instantiate(stromPrefab, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 4), player.transform.rotation);

        Destroy(newStrom, 10f);
    }


    public void PlayEvent(string nameOfEvent)
    {

        GameController_script.animRef.Play(nameOfEvent);

        // nned timer, otherwise the notplayinmg trigger before the is playing as the haapen at the same time
        eventTimer = 1;
        charcterMovemnets.StaticRef.CurrentState = charcterMovemnets.State.inCutScreen;       

    }

    public GameObject SpwanHat(GameObject prefabToSpwan)
    {
        GameObject newFlyingHat = (GameObject)Instantiate(prefabToSpwan, new Vector3(hatSpwanTarget.transform.position.x, hatSpwanTarget.transform.position.y, hatSpwanTarget.transform.position.z), hatSpwanTarget.transform.rotation);

        return newFlyingHat;
    }


}
