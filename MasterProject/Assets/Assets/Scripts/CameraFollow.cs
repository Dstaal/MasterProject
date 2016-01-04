using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float zMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float zSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
	public Vector3 maxXAndZ;		// The maximum x and y coordinates the camera can have.
	public Vector3 minXAndZ;        // The minimum x and y coordinates the camera can have.

    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

   // float zoomLevel = 0.0F;
   
   // public Camera cam;


    private Transform player;		// Reference to the player's transform.
	
	
	void Start ()
	{
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // cam = this.GetComponentInChildren<Camera>();
    }
	
	
	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}
	
	
	bool CheckZMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.z - player.position.z) > zMargin;
	}
	
    void Update ()
    {

        if(player == null)
        {
            
        }

        // rotates the camera in left mouseBnt
        if (Input.GetMouseButton(0))
        {
            rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
     
/*
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > -0.5f && scroll < 0.25f)
        {
            zoomLevel = zoomLevel + scroll;

            Debug.Log(zoomLevel);
            if (zoomLevel < -0.5f)
            {
                zoomLevel = -0.5f;
            }
            if (zoomLevel > 0.25f)
            {
                zoomLevel = 0.25f;
            }
        }

    */

    }
	
	void FixedUpdate ()
	{
        TrackPlayer();
	}
	
	
	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x ;
		float targetZ = transform.position.z ;
        float targetY = transform.position.y;

        // If the player has moved beyond the x margin...
        if (CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x , player.position.x, xSmooth * Time.deltaTime);
		
		// If the player has moved beyond the y margin...
		if(CheckZMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetZ = Mathf.Lerp(transform.position.z , player.position.z, zSmooth * Time.deltaTime) ;
		
        targetY = Mathf.Lerp(transform.position.y, player.position.y, zSmooth * Time.deltaTime);

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndZ.x, maxXAndZ.x);
		targetZ = Mathf.Clamp(targetZ, minXAndZ.z, maxXAndZ.z);
		
		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, targetZ);
	}


}
