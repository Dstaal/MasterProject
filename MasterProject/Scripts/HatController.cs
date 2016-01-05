using UnityEngine;
using System.Collections;

public class HatController : MonoBehaviour
{
    public static HatController StaticRef;

    private GameObject player;

    private Transform flyingHat;

    public bool canPickUp = false;
 //   public bool playerAtHat = false;

    // Use this for initialization

    void Awake()
    {
        if (StaticRef == null) // if theres is not allerdaye a static ref makes this the static ref
        {
        //    DontDestroyOnLoad(gameObject);
            StaticRef = this;
        }
        else if (StaticRef != this) // is the allready is a ref , destory this
        {
            Destroy(this);
        }
    }

    void Start()
    {

        player = GameController_script.playerRef;

        flyingHat = GameObject.FindGameObjectWithTag("FlyingHat").transform;

        //make sure hat is above tarrian

        Vector3 T_pos = transform.position;
        T_pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = T_pos;

        canPickUp = false;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 T_pos = flyingHat.transform.position;
        T_pos.y = Terrain.activeTerrain.SampleHeight(flyingHat.transform.position);

        if (flyingHat.transform.position.y <= T_pos.y  && charcterMovemnets.StaticRef.CurrentState != charcterMovemnets.State.inCutScreen)
        {
            
            flyingHat.transform.position = T_pos ;

            flyingHat.transform.rotation = player.transform.rotation;

            Debug.Log("hat under tarian : " + T_pos.y + " _hats pos y : " + flyingHat.transform.position.y);
        }

    }

    

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canPickUp == false)
        {
            Debug.Log("can't pick up yet. but call triiger as it should");
          
        }

        if (other.tag == "Player" && canPickUp == true)
        {

            charcterMovemnets.StaticRef.CurrentState = charcterMovemnets.State.inCutScreen;

            Debug.Log(" player thouched hat");
            // move and rotate flying hat to align with the player, otherwise ithe hat might fumble tought the air
            flyingHat.transform.position = new Vector3(player.transform.position.x, eventChecker.StaticRef.storedY, player.transform.position.z);
            flyingHat.transform.rotation = player.transform.rotation;

            eventChecker.StaticRef.haveHat = eventChecker.HatState.PickingUp;
        }
    }
}
