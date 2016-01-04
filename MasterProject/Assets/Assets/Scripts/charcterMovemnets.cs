using UnityEngine;
using System.Collections;

public class charcterMovemnets : MonoBehaviour {


    public static charcterMovemnets StaticRef;

    public enum State {canMove, inCutScreen };

	public float speed = 3f;
	public float trunSpeed = 120f;
    public float gravtiy = 20f;

    private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
    private float IdelCount = 5;
    private bool Idel = false;
    private bool canSprint = false; // why does it keep saying it not set. then it's clear set and used in  fixedUpdate >   if (moveVertical >= 1) ...

    public State CurrentState;

    private Animator anim;
	

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

        //get refeances

        controller = GetComponent<CharacterController>();

		anim = GetComponentInChildren<Animator>();

        CurrentState = State.canMove;
	}


    void FixedUpdate()
    {
        //store input axes

        if (CurrentState == State.canMove)
        {

            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            bool isRunning = Input.GetButton("Run");

            transform.Rotate(0, moveHorizontal * trunSpeed * Time.deltaTime, 0);

            if (moveVertical >= 1)
            {
                anim.SetBool("IsWalking", true);
                IdelCount = 15;
                canSprint = true;
            }
            if(moveVertical < 1)
            {
                anim.SetBool("IsWalking", false);
                canSprint = false;
            }

            if (isRunning == true && moveVertical >= 1)
            {
                anim.SetBool("IsRunning", true);
                speed = 6f;
            }

            if (isRunning == false || moveVertical < 1)
            {
                anim.SetBool("IsRunning", false);
                speed = 3f;
            }

            if (moveVertical <= -1)
            {
                anim.SetBool("IsBaking", true);
                IdelCount = 15;
                speed = 1.5f;
            }
            else
            {
                anim.SetBool("IsBaking", false);
            
            }

            if (moveHorizontal >= 1  || moveHorizontal <= -1)
            {
                anim.SetBool("IsTruning", true);
                IdelCount = 15;
            }
            else
            {
                anim.SetBool("IsTruning", false);
            }

            if (controller.isGrounded)
            {
                moveDirection = transform.forward * moveVertical * speed;
                anim.SetFloat("speed", controller.velocity.magnitude);
            }

            // random idel aniamtion


            if (IdelCount <= 0)
            {

                int rand = Random.Range(1, 101);
                Idel = true;

                anim.SetFloat("IdelCounter", IdelCount);
                anim.SetInteger("RandomIdel", rand);

                anim.SetBool("Extra_Idel", Idel);

                Debug.Log("random was : " + rand);

                IdelCount = 10;
            }
            if (IdelCount > 1 && IdelCount < 10)
            {
                Idel = false;
                anim.SetInteger("RandomIdel", 102);

                anim.SetBool("Extra_Idel", Idel);
            }

            IdelCount = IdelCount - Time.deltaTime;

            controller.Move(moveDirection * Time.deltaTime);

            moveDirection.y -= gravtiy * Time.deltaTime;


        }
    }
     
}
