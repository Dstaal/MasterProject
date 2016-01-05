using UnityEngine;
using System.Collections;

public class GuideController : MonoBehaviour {

    public GameObject guide;

    public windBubleScript windbuble_script;

    private Renderer guideSphere;

    private Color color;

    // Use this for initialization
    void Start () {

        guideSphere = guide.GetComponent<Renderer>();

        color = guideSphere.material.color;

        color.a = 0.25f;

        guideSphere.material.color = color;           
    }
	
	// Update is called once per frame
	void Update () {

    }


}
