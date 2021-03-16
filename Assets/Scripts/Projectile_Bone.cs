using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bone : MonoBehaviour
{
    public float damage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
