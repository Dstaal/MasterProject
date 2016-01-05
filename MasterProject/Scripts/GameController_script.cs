using UnityEngine;
using System.Collections;

public class GameController_script : MonoBehaviour {

    public enum Mode { Debug , questionare };

    public enum State { setup, starting, running, options, paused};

    public enum scenario { scenario_C, scenario_R, None };

    public static GameController_script StaticRef;

    public GameObject player_C;
    public GameObject player_R;

    public static GameObject playerRef;

    public GameObject startPoint;
    public GameObject eventSphere;
    public GameObject cameraRig;
    public Canvas canvas;

    public bool demograhpicOkey = false;
    public bool duringQesutionOkey = false;
    public bool finalQuestionOkey = false;

    public static Animator animRef;

    public CanvasGroup mainMenu;
    public CanvasGroup demograhpicQuestion;
    public CanvasGroup duringQuestions;


    public scenario currentScenario;
    public State currentState;
    public Mode currentMode;

    private bool escPressed = false;

   

    // Use this for initialization
    void Awake () {
        if(StaticRef == null) // if theres is not allerdaye a static ref makes this the static ref
        {
            DontDestroyOnLoad(gameObject);
            StaticRef = this;
        }
        else if(StaticRef != this) // is the allready is a ref , destory this
        {
            Destroy(this);
        }       

        currentState = State.setup;
      
        currentScenario = scenario.None; //remeber you set it to none here. if at some point you don't why the printed stuff keeps say none

        mainMenu = canvas.GetComponentInChildren<CanvasGroup>();

    }
	
	// Update is called once per frame
	void Update () {


        // check logic later then the other state are added
        if (Input.GetKeyDown("escape"))
        {
            escPressed = !escPressed;
            if (escPressed)
            {
              //  currentState = State.options;
                Debug.Log("esc pesssed now in options");
                animRef.Play("open");
                eventChecker.StaticRef.book_anim.Play("openBook");

            }
            else
            {
              //  currentState = State.running;
                Debug.Log("esc pesssed now leave options");
                animRef.Play("close");
          

            }
        }


        if (currentScenario == scenario.scenario_C && currentState == State.starting)
        {
            SetUpPlayer(player_C);
        }

        if (currentScenario == scenario.scenario_R && currentState == State.starting)
        {
            SetUpPlayer(player_R);
        }

    }

    void SpwanGameObj(GameObject spwanObj)
    {
       Instantiate(spwanObj, new Vector3(startPoint.transform.position.x, startPoint.transform.position.y, startPoint.transform.position.z), startPoint.transform.rotation);
    }

    private GameObject SpwanPlayer(GameObject spwanObj)
    {
        GameObject newPlayer;
        newPlayer = (GameObject)Instantiate(spwanObj, new Vector3(startPoint.transform.position.x, startPoint.transform.position.y, startPoint.transform.position.z), startPoint.transform.rotation);

        return newPlayer;

    }

    // on click functions

    public void ClickedOnDebugBnt()
    {
        currentMode = Mode.Debug;
        Debug.Log("trying to set mode to debug");
    }

    public void ClickedOnQuestonaireBnt()
    {
        currentMode = Mode.questionare;
        Debug.Log("trying to set mode to questionare");
    }

    public void ClickedOnScenarioR_Bnt()
    {
        currentScenario = scenario.scenario_R;
        currentState = State.starting;
        Debug.Log("trying to set scenario to R");
    }

    public void ClickedOnScenarioC_Bnt()
    {
        currentScenario = scenario.scenario_C;
        currentState = State.starting;
        Debug.Log("trying to set scenario to C");
    }

    public void ClickedOnResume_Bnt()
    {
        escPressed = !escPressed; // toggle the esc bnt bool too
        animRef.Play("close");
        Debug.Log("set state to running");
    }

    public void ClickedOnQuit_Bnt()
    {
        if (finalQuestionOkey == false)
        {
            // add eixt questonarie trigger code here
        }
        if(finalQuestionOkey == true && demograhpicOkey == true)
        { 
        // add check to see if questnaire done before quit
        Application.Quit();
        Debug.Log("trying to Quit");
        }
    }

    void SetUpPlayer(GameObject playerPprefab)
    {
        playerRef = SpwanPlayer(playerPprefab);
        SpwanGameObj(eventSphere);
        SpwanGameObj(cameraRig);
        currentState = State.running;
        animRef = playerRef.GetComponentInChildren<Animator>();
    }

}
