using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IvySpin : MonoBehaviour
{
    float turnspeed = 180f;
    float curzrot = 180f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curzrot += turnspeed * Time.deltaTime;
        if (curzrot > 360) { curzrot = 0f; }
        transform.rotation = Quaternion.Euler(-90f, 0f, curzrot);
    }
}
