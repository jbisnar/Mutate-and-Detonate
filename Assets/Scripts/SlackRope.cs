using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlackRope : MonoBehaviour
{
    int nodecount = 10;
    public GameObject node;
    float ropelen = 0;
    public Transform player;
    public Transform hook;
    public LineRenderer slackline;

    // Start is called before the first frame update
    void Start()
    {
        slackline = GetComponent<LineRenderer>();
        slackline.positionCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        slackline.SetPosition(0, transform.position);
        for (int i = 0; i < transform.childCount; i++)
        {
            slackline.SetPosition(i + 1, transform.GetChild(i).position);
        }
        //slackline.SetPosition(transform.childCount + 1, hook.position);
    }

    public void MakeRope()
    {
        Vector3 segoffset = (hook.position - player.position) / (nodecount);
        ropelen = Vector2.Distance(hook.position, player.position);
        for (int i = 1; i <= nodecount; i++)
        {
            GameObject newnode = GameObject.Instantiate(node, transform.position + segoffset * i, transform.rotation, transform);
            if (i == 1)
            {
                newnode.GetComponent<DistanceJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
            }
            else
            {
                newnode.GetComponent<DistanceJoint2D>().connectedBody = transform.GetChild(i - 2).GetComponent<Rigidbody2D>();
            }
            if (i == nodecount)
            {
                newnode.GetComponent<FixedJoint2D>().enabled = true;
                newnode.GetComponent<FixedJoint2D>().connectedBody = hook.GetComponent<Rigidbody2D>();
            }
            newnode.GetComponent<DistanceJoint2D>().distance = ropelen / nodecount;
        }
        slackline.positionCount = nodecount + 1;
    }

    public void BreakRope()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        slackline.enabled = false;
        //slackline.positionCount = 1;
    }
}
