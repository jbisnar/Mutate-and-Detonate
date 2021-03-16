using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Gas : MonoBehaviour
{
    public float damage;
    float shrinkspeed = .5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var curscale = transform.localScale.x;
        curscale -= shrinkspeed * Time.deltaTime;
        transform.localScale = new Vector3(curscale, curscale);
        if (curscale < .1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);

        var player = collision.GetComponent<Player_Combat>();
        if (player != null)
        {
            player.Damage(damage);
        }
    }
}
