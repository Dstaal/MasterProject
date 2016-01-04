using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class QustonaireHandler : MonoBehaviour
{

    public static QustonaireHandler StaticRef;

    // public Button maleBnt;
    public Toggle mToogle;
    // public Button femaleBnt;
    public Toggle fToogle;
    // public Button ccontinuedBnt;
    public Dropdown ageDrop;
    public Toggle ageToggle;
    public InputField occupationIn;
    public Toggle oToggle;

    public InputField duringWhyInput;
    public Toggle duringWhyToggle;

    public Text likertValueText;

    public Text notAllAveswered;

    string genderChossen = null;
    string ageChossen = null;
    string occupationAnswered = null;
    string playingScenario = null;
    string duringWhyAnswered = null;
    string duringCD = null;

    private float cTimer = 0;

    public string fileName = "myfile.csv";

    private WWWForm answersForm = null;

    private WWW www, requestWWW;

    void Awake()
    {
        if (StaticRef == null) // if theres is not allerdaye a static ref makes this the static ref
        {
            DontDestroyOnLoad(gameObject);
            StaticRef = this;
        }
        else if (StaticRef != this) // is the allready is a ref , destory this
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        //   mToogle = maleBnt.GetComponentInChildren<UnityEngine.UI.Toggle>();
        //   fToogle = femaleBnt.GetComponentInChildren<UnityEngine.UI.Toggle>();
/*
        WWWForm form = new WWWForm();
        form.AddField("var1", "value1");
        form.AddField("var2", "value2");
        WWW www = new WWW(url, form);

        answersForm.AddField("testinfo", "42");
        StartCoroutine(WaitForRequest(www));
        StartCoroutine(SendForm());
        */
    }


    // Update is called once per frame
    void Update()
    {

        cTimer = cTimer - Time.deltaTime;

        if (cTimer <= 0)
        {
            notAllAveswered.text = " ";
        }
    }

    public void maleClicked()
    {
        if (!mToogle.isOn)
        {
            mToogle.isOn = true;
            fToogle.isOn = false;
            genderChossen = "Male";
        }

    }

    public void femaleClicked()
    {
        if (!fToogle.isOn)
        {
            fToogle.isOn = true;
            mToogle.isOn = false;
            genderChossen = "Female";
        }

    }

    public void ageSelected()
    {
        if (ageDrop.value == 0 || ageDrop == null)
        {
            Debug.Log("age not chosen yet");
            ageToggle.isOn = false;
        }
        if (ageDrop.value != 0 && ageDrop != null)
        {
            ageToggle.isOn = true;
        }
        if (ageDrop.value == 1)
        {
            ageChossen = "18 - 25";
        }
        if (ageDrop.value == 2)
        {
            ageChossen = "26 - 30";
        }
        if (ageDrop.value == 3)
        {
            ageChossen = "31 - 35";
        }
        if (ageDrop.value == 4)
        {
            ageChossen = "36 - 40";
        }
        if (ageDrop.value == 5)
        {
            ageChossen = "40 - 50";
        }
        if (ageDrop.value == 6)
        {
            ageChossen = "50 +";
        }


    }

    public void continuedClick()
    {
        if (genderChossen == null)
        {
            Debug.Log("no gender choosen yet");
        }
        else if (genderChossen != null)
        {
            Debug.Log("genderChossen chosen = : " + genderChossen);
        }
        if (ageDrop.value == 0 || ageDrop == null)
        {
            Debug.Log("age not chosen yet");
        }
        else if (ageDrop.value != 0 || ageDrop != null)
        {
            Debug.Log("age chosen = : " + ageChossen);
        }
        if (occupationAnswered == null)
        {
            Debug.Log("no gender occupationAnswered yet");
        }
        else
        {
            Debug.Log("occupationAnswered is : " + occupationAnswered);
        }


        if (genderChossen == null || ageDrop.value == 0 || occupationIn == null)
        {
            notAllAveswered.text = ("plase anwser all the questions");
            cTimer = 2;
        }
        else
        {
            Debug.Log("contuinesd press and everying is selected. it's okey to contuines");

            int rand = Random.Range(1, 101);

            if (rand <= 50)
            {
                GameController_script.StaticRef.currentScenario = GameController_script.scenario.scenario_C;
                GameController_script.StaticRef.currentState = GameController_script.State.starting;
                playingScenario = "scenario_C";
            }
            else
            {
                GameController_script.StaticRef.currentScenario = GameController_script.scenario.scenario_R;
                GameController_script.StaticRef.currentState = GameController_script.State.starting;
                playingScenario = "scenario_R";
            }

            GameController_script.StaticRef.demograhpicQuestion.alpha = 0;
            GameController_script.StaticRef.demograhpicQuestion.blocksRaycasts = false;
            GameController_script.StaticRef.demograhpicQuestion.interactable = false;

            save(); // with te current metohde i should only "save" or submit then the test is over. otherwise if two players are playing at the same time. their data might get mixed.

        }
    }

    public void continuedDuringClick()
    {
        if (duringCD == null)
        {
            Debug.Log("no duringCD");
        }
        else 
        {
            Debug.Log("duringCD chosen = : " + duringCD);
        }
      
        if (duringWhyAnswered == null)
        {
            Debug.Log("no duringWhyAnswered yet");
        }
        else
        {
            Debug.Log("duringWhyAnswered is : " + duringWhyAnswered);
        }


        if (duringCD == null || duringWhyAnswered == null) //change to during stuff
        {
            notAllAveswered.text = ("plase anwser all the questions");
            cTimer = 2;
        }
        else
        {
            Debug.Log("contuinesd press and everying is selected. it's okey to contuines");

            GameController_script.StaticRef.currentState = GameController_script.State.running;

            GameController_script.StaticRef.duringQuestions.alpha = 0;
            GameController_script.StaticRef.duringQuestions.blocksRaycasts = false;
            GameController_script.StaticRef.duringQuestions.interactable = false;

        //    save(); // with te current metohde i should only "save" or submit then the test is over. otherwise if two players are playing at the same time. their data might get mixed.

        }



    }

    public void occupationInExit()
    {
        occupationAnswered = occupationIn.text;

        if (occupationIn.text != null && occupationIn.text != "" && occupationIn.text != " " && occupationIn.text != "  " && occupationIn.text != "   " && occupationIn.text != "    " && occupationIn.text != "     " && occupationIn.text != "      ")
        {
            oToggle.isOn = true;
        }
        else
        {
            oToggle.isOn = false;
        }
    }

    public void duringWhyInputExit()
    {
        duringWhyAnswered = duringWhyInput.text;

        if (duringWhyInput.text != null && duringWhyInput.text != "" && duringWhyInput.text != " " && duringWhyInput.text != "  " && duringWhyInput.text != "   " && duringWhyInput.text != "    " && duringWhyInput.text != "     " && duringWhyInput.text != "      ")
        {
            duringWhyToggle.isOn = true;
        }
        else
        {
            duringWhyToggle.isOn = false;
        }
    }


    public void likertOnOff()
        {
        Debug.Log( "likert text is : " + likertValueText.text);
        duringCD = likertValueText.text;

        }


    void save()
    {

        var path = Path.Combine(Application.dataPath, this.fileName);

        if (File.Exists(path)) // if the fil exists just make a newline
            {
                using (var file = File.AppendText(path))
                    file.WriteLine("");
                    Debug.Log("file found makeing new line");
            }
            else // otherwise craete the file and the header
            {
                using (var file = File.CreateText(path))
                {
                    file.WriteLine("Age; Gender; Occupation; playingScenario; ");
                    Debug.Log("didn't find the save file so made a new");
                }
            }

        var myString = ageChossen + ";" + genderChossen + ";" + occupationAnswered + ";" + playingScenario + ";";
        
        File.AppendAllText(path, myString); // i don't need the close file thingy?
        
    }


    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    /*
    IEnumerator SendForm()
    {
        requestWWW = new WWW(url, answersForm);

        yield return requestWWW;

        // Print the error to the console		
        if (!string.IsNullOrEmpty(requestWWW.error))
        {
            Debug.LogWarning("WWW request error: " + requestWWW.error);
            yield return null;
        }
        else {
            Debug.Log("WWW returned text: " + requestWWW.text);
         //   bHasSentData = true;
            yield return requestWWW.text;
        }
    }
    */
}



