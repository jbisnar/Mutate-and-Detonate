using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    float gravNormal = 3f;
    float gravJump = 3f;
    float gravDown = 15f;
    float gravSwing = 10f;
    float gravWall = 0f;
    float velWalk = 6f;
    float accelWalk = 10f;
    float accelSwitch = 60f;
    float accelSlow = 30f;
    float velAir = 5f;
    float accelAir = 5f;
    float accelAirSwitch = 20f;
    float accelAirSlow = 0f;
    float velJumpGround = 3f;
    float velJumpWallH = 3f;
    float velJumpWallV = 2f;
    float velWallSlideDown = 1f;

    public bool swinging = false;

    public GameObject hook;
    public DistanceJoint2D ropeJoint;
    public LineRenderer ropeLine;
    public LineRenderer slackLine;
    float grappleRange = 15f;

    float savedVel = 0;
    float savedVelAir = 0;
    float savedVelWallKick = 0;

    public bool grounded = false;
    public bool jumping = false;
    public bool walledL = false;
    public bool wallslideL = false;
    public bool walledR = false;
    public bool wallslideR = false;
    float walljumpgrace = .05f;
    float walljumpcontrol = .5f;
    float perfectkickgrace = .15f;
    float walljumpgracetime;
    float walljumpcontroltime;
    float perfectkicktime;
    public LayerMask layerGround;
    public Vector2 temp;

    Vector2 mousePos;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LineRenderer>().positionCount = 2;
        ropeJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //AIMING
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
        var aim = mousePos - new Vector2(transform.position.x, transform.position.y);
        aim = aim.normalized;

        temp = transform.GetComponent<Rigidbody2D>().velocity;
        grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - .11f, transform.position.y - .11f),
            new Vector2(transform.position.x + .11f, transform.position.y - .14f), layerGround);
        if ((!walledL && !walledR) && (Physics2D.OverlapArea(new Vector2(transform.position.x - .2f, transform.position.y + .36f),
            new Vector2(transform.position.x - .18f, transform.position.y - .30f), layerGround) || Physics2D.OverlapArea(new Vector2(transform.position.x + .18f, transform.position.y + .36f),
            new Vector2(transform.position.x + .2f, transform.position.y - .30f), layerGround)))
        {
            perfectkicktime = Time.time + perfectkickgrace;
        }
        walledL = Physics2D.OverlapArea(new Vector2(transform.position.x - .14f, transform.position.y + .11f),
            new Vector2(transform.position.x - .11f, transform.position.y - .11f), layerGround);
        walledR = Physics2D.OverlapArea(new Vector2(transform.position.x + .11f, transform.position.y + .11f),
            new Vector2(transform.position.x + .14f, transform.position.y - .11f), layerGround);
        if (walledL || walledR)
        {
            walljumpgracetime = Time.time + walljumpgrace;
        }

        //HORIZONTAL CONTROL
        if (Input.GetAxisRaw("Horizontal") == 0)
        { //Slow down
            if (swinging)
            {
                if (grounded)
                {
                    temp.x -= accelSlow * Mathf.Sign(temp.x) * Time.deltaTime;
                }
            }
            else if (Mathf.Abs(temp.x) < accelSlow * Time.deltaTime)
            {
                temp.x = 0;
            }
            else if (temp.x > 0)
            {
                if (grounded)
                {
                    temp.x -= accelSlow * Time.deltaTime;
                }
                else if (Time.time > walljumpcontroltime)
                {
                    temp.x -= accelAirSlow * Time.deltaTime;
                }
            }
            else
            {
                if (grounded)
                {
                    temp.x += accelSlow * Time.deltaTime;
                }
                else if (Time.time > walljumpcontroltime)
                {
                    temp.x += accelAirSlow * Time.deltaTime;
                }
            }
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        { //Right
            if (walledR)
            {
                temp.x = 0;
            }
            else if (grounded)
            {
                if (temp.x > velWalk)
                {
                    temp.x = velWalk;
                }
                else if (temp.x < 0)
                {
                    temp.x += accelSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x += accelWalk * Time.deltaTime;
                }
                savedVelAir = temp.x;
            }
            else
            {
                if (swinging)
                {
                    temp.x = temp.x;
                }
                else if (Time.time < walljumpcontroltime)
                {
                    temp.x = temp.x;
                }
                else if (temp.x > velAir)
                {
                    temp.x = temp.x;
                }
                else if (temp.x < 0)
                {
                    temp.x += accelAirSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x += accelAir * Time.deltaTime;
                }
            }
        }
        else
        { //Left
            if (walledL)
            {
                temp.x = 0;
            }
            else if (grounded)
            {
                if (temp.x < -velWalk)
                {
                    temp.x = -velWalk;
                }
                else if (temp.x > 0)
                {
                    temp.x -= accelSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x -= accelWalk * Time.deltaTime;
                }
            }
            else
            {
                if (swinging)
                {
                    temp.x = temp.x;
                }
                else if (Time.time < walljumpcontroltime)
                {
                    temp.x = temp.x;
                }
                else if (temp.x < -velAir)
                {
                    temp.x = temp.x;
                }
                else if (temp.x > 0)
                {
                    temp.x -= accelAirSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x -= accelAir * Time.deltaTime;
                }
            }
        }
        if (temp.x != 0)
        {
            savedVel = temp.x;
        }

        //GRAVITY
        if (grounded)
        {
            jumping = false;
            temp.y = 0;
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                temp.y = velJumpGround;
                jumping = true;
            }
        }
        else if (walledL)
        {
            jumping = false;
            if (Input.GetKeyDown("w"))
            {
                if (savedVel < -velJumpWallH && Time.time < perfectkicktime)
                {
                    temp.x = -savedVel;
                }
                else
                {
                    temp.x = velJumpWallH;
                }
                temp.y = velJumpWallV;
                jumping = true;
                walljumpcontroltime = Time.time + walljumpcontrol;
            }
            if (wallslideL && temp.y < -velWallSlideDown)
            {
                temp.y = -velWallSlideDown;
            }
            else if (Input.GetAxisRaw("Vertical") > 0 && temp.y > 0)
            {
                temp.y -= gravJump * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                jumping = false;
                temp.y -= gravDown * Time.deltaTime;
            }
            else
            {
                jumping = false;
                temp.y -= gravNormal * Time.deltaTime;
            }
        }
        else if (walledR)
        {
            jumping = false;
            if (Input.GetKeyDown("w"))
            {
                if (savedVel > velJumpWallH && Time.time < perfectkicktime)
                {
                    temp.x = -savedVel;
                }
                else
                {
                    temp.x = -velJumpWallH;
                }
                temp.y = velJumpWallV;
                jumping = true;
                walljumpcontroltime = Time.time + walljumpcontrol;
            }
            if (wallslideR && temp.y < -velWallSlideDown)
            {
                temp.y = -velWallSlideDown;
            }
            else if (Input.GetKey("w") && temp.y > 0)
            {
                temp.y -= gravJump * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                jumping = false;
                temp.y -= gravDown * Time.deltaTime;
            }
            else
            {
                jumping = false;
                temp.y -= gravNormal * Time.deltaTime;
            }
        }
        else if (Time.time < walljumpgracetime)
        {
            jumping = false;
            if (Input.GetKeyDown("w"))
            {
                //temp.x = -velJumpWallH;
                temp.y = velJumpWallV;
                jumping = true;
                walljumpcontroltime = Time.time + walljumpcontrol;
            }
        }
        else //MIDAIR
        {
            if (Input.GetKey("w") && temp.y > 0 && jumping)
            {
                temp.y -= gravJump * Time.deltaTime;
            }
            else if (swinging && temp.y < 0)
            {
                temp.y -= gravSwing * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                jumping = false;
                temp.y = Mathf.Min(-velJumpGround, temp.y - gravDown * Time.deltaTime);
            }
            else
            {
                jumping = false;
                temp.y -= gravNormal * Time.deltaTime;
            }
        }

        //ABILITIES
        if (Input.GetMouseButtonDown(0))
        {
            if (!swinging)
            {
                //Grapple
                //Raycast, put hook at hit
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, .1f, aim, Mathf.Infinity, layerGround);
                if (hit.collider != null)
                {
                    swinging = true;
                    hook.transform.position = hit.point;
                    hook.transform.position += Vector3.back;
                    hook.transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg), Vector3.forward);
                    hook.SetActive(true);
                    ropeJoint.distance = hit.distance;
                    ropeJoint.enabled = true;
                    ropeLine.enabled = true;
                }
                GetComponentInChildren<SlackRope>().MakeRope();
            }
            else if (swinging)
            {
                //Ungrapple
                swinging = false;
                hook.SetActive(false);
                ropeJoint.enabled = false;
                ropeLine.enabled = false;
                GetComponentInChildren<SlackRope>().BreakRope();
            }
        }

        if (swinging)
        {
            ropeLine.SetPosition(0, transform.position);
            ropeLine.SetPosition(1, hook.transform.position);
            if (Mathf.Abs(Vector2.Distance(hook.transform.position, transform.position) - ropeJoint.distance) < .001f)
            {
                ropeLine.enabled = true;
                slackLine.enabled = false;
            }
            else
            {
                ropeLine.enabled = false;
                slackLine.enabled = true;
            }
        }

        transform.GetComponent<Rigidbody2D>().velocity = temp;
    }
}
