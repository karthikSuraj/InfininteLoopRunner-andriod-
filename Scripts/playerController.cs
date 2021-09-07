using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
   
    private GameObject _player;
    private CharacterController plr;
    private Vector3 direction;


    private int desiredLane = 1;//0=left 1=middle 2=right
    private float laneDistance = 3.35f;

    private bool isRunning;
    private bool isSliding = false;

    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpForce = 11;
    [SerializeField] private float Gravity = -23f;


     [SerializeField]
    private float speedMultiplier = 1.045f;
    [SerializeField] private float speedIncreaseMilestone = 100;
    private float speedMilestoneCount;

    public Animator anim;


    void Start()
    {
        _player = gameManager.instance.mainPlayer;
        plr = _player.GetComponent<CharacterController>();


        speedMilestoneCount = speedIncreaseMilestone;
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Instance Update
        gameManager.instance.mainPlayer = _player;

        direction.z = speed ;

        direction.y += Gravity * Time.deltaTime;

       
        if(isRunning)
        {
            anim.SetInteger("Jump", 0);
        }

        if (plr.transform.position.z > speedMilestoneCount)
        {
            speedMilestoneCount += speedIncreaseMilestone;
            speedIncreaseMilestone = speedIncreaseMilestone * speedMultiplier;
            speed = speed * speedMultiplier;
            
        }
        moveRight();
        moveLeft();
        Jump();
        Slide();

        Debug.Log(desiredLane);

        /*We changed the player movement using the inputs now using desiredLane that we accquire
         we will change the position in the scene*/

        Vector3 targetPosition = plr.transform.position.z * plr.transform.forward + plr.transform.position.y * plr.transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }

        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        //plr.transform.position = Vector3.Lerp(plr.transform.position, targetPosition, lerpvalue * Time.deltaTime);
        //plr.center = plr.center;

        if (plr.transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - plr.transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            plr.Move(moveDir);
        else
            plr.Move(diff);
       
    }

    private void FixedUpdate()
    {
        plr.Move(direction * Time.fixedDeltaTime);
        isRunning = true;
      
    }

    public void moveRight()
    {
        if(swipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
       
    }

    public void moveLeft()
    {
        if(swipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }
        
    }

    public void Jump()
    {
        if(swipeManager.swipeUp)
        {
            isRunning = false;
            if (plr.isGrounded)
            {
                anim.SetInteger("Jump", 1);
                direction.y = jumpForce;
            }
        }
        
    }

    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstracle")
        {
           gameManager.gameOver = true;
        }

        if (hit.transform.tag == "Coin")
        {
            gameManager.instance.numberofCoins += 1;
            hit.gameObject.SetActive(false);
        }

    }
    public void Slide()
    {
        if(swipeManager.swipeDown)
        {
            if (!isSliding)
            {
                StartCoroutine(SlideCall());
            }
        }
        
        
    }
    private IEnumerator SlideCall()
    {
        isSliding = true;
        anim.SetInteger("Slide", 1);
        plr.center = new Vector3(0, -0.5f, 0);
        plr.height = 1;


        yield return new WaitForSeconds(1);

        plr.center = new Vector3(0, 0.9f, 0);
        plr.height = 3.5f;
        anim.SetInteger("Slide", 0);
        isSliding = false;
    }
}
