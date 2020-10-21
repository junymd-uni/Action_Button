using UnityEngine;
using UnityEngine.EventSystems;

public class bodywalk : MonoBehaviour
{
    //Button Object
    public GameObject leftbutton;
    public GameObject rightbutton;

    //Events to register on the button. Both on and off.
    private EventTrigger.Entry entry = new EventTrigger.Entry();
    private EventTrigger.Entry entry0 = new EventTrigger.Entry();
    private EventTrigger.Entry entry2 = new EventTrigger.Entry();
    private EventTrigger.Entry entry20 = new EventTrigger.Entry();

    //Press and hold timer.
    private float tm = 0;

    //The flag for a single button press
    private bool leftok = true;
    private bool rightok = true;

    //Walking motion switching
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        setButton();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void setButton()
    {
        //Create an event for the left button.
        EventTrigger _leftbutton = leftbutton.GetComponent<EventTrigger>();
        entry.eventID = EventTriggerType.PointerDown;  //When the button is pressed.
        entry0.eventID = EventTriggerType.PointerUp;  //When the button is released.

        //LeftMove() is registered as an event when the button is pressed
        entry.callback.AddListener((eventData) => { LeftMove(); });

        //Register LeftMoveOff() as an event when the button is released
        entry0.callback.AddListener((eventData) => { LeftMoveOff(); });

        //Register the created event to the button.
        _leftbutton.triggers.Add(entry);
        _leftbutton.triggers.Add(entry0);

        //Create an event for the right button.
        EventTrigger _rightbutton = rightbutton.GetComponent<EventTrigger>();
        entry2.eventID = EventTriggerType.PointerDown;
        entry20.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((eventData) => { RightMove(); });
        entry20.callback.AddListener((eventData) => { RightMoveOff(); });
        _rightbutton.triggers.Add(entry2);
        _rightbutton.triggers.Add(entry20);
    }


    //When the left button is pressed by a single shot.
    void LeftMove()
    {
        if (leftok)
        {
            animator.SetBool("walking", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);

            //Initialize the timer when you keep pressing it.
            tm = 0;
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
            if(transform.position.x < -9.0f)
            {
                transform.position = new Vector3(-9.0f, transform.position.y, transform.position.z);
            }
        }
        //Press and hold flag
        leftok = false;
    }

    //When the left button is released
    void LeftMoveOff()
    {
        animator.SetBool("walking", false);

        //When the button is released, the single-shot flag is set to true.
        leftok = true;
    }

    //Right button
    void RightMove()
    {
        if (rightok)
        {
            animator.SetBool("walking", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            tm = 0;
            transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
            if (transform.position.x > 9.0f)
            {
                transform.position = new Vector3(9.0f, transform.position.y, transform.position.z);
            }
        }
        rightok = false;
    }

    void RightMoveOff()
    {
        animator.SetBool("walking", false);
        rightok = true;
    }

    //Behaviour when pressed and held down
    void Moving()
    {
        if (!leftok && rightok)
        {
            //Immediately after pressing, tm is 0, so it is added by Time.deltaTime.
            if (tm == 0)
            {
                tm += Time.deltaTime;
            }
            //Move to position when tm is 0.25 seconds.
            else if (tm >= 0.25 && tm < 0.26)
            {
                transform.position += transform.right / 20;
                if (transform.position.x < -9.0f)
                {
                    transform.position = new Vector3(-9.0f, transform.position.y, transform.position.z);
                }
            }
            //Move to position when tm is 0.35 seconds.
            else if (tm >= 0.35)
            {
                transform.position += transform.right / 20;
                if (transform.position.x < -9.0f)
                {
                    transform.position = new Vector3(-9.0f, transform.position.y, transform.position.z);
                }
                //The tm returns to 0.15 seconds and is pushed again after 0.1 seconds.
                tm = 0.15f;
            }
            //Wait for 0.25 seconds.
            else { tm += Time.deltaTime; }
        }
        else if (leftok && !rightok)
        {
            if (tm == 0)
            {
                tm += Time.deltaTime;
            }
            else if (tm >= 0.25 && tm < 0.26)
            {
                transform.position += transform.right / 20;
                if (transform.position.x > 9.0f)
                {
                    transform.position = new Vector3(9.0f, transform.position.y, transform.position.z);
                }
            }
            else if (tm >= 0.35)
            {
                transform.position += transform.right / 20;
                if (transform.position.x > 9.0f)
                {
                    transform.position = new Vector3(9.0f, transform.position.y, transform.position.z);
                }
                tm = 0.15f;
            }
            else { tm += Time.deltaTime; }
        }
        //Initialize tm when the button leaves
        else if (leftok || rightok)
        {
            tm = 0;
        }
    }
}
