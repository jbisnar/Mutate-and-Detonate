using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Explosion : MonoBehaviour
{
    public float expdamage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Disappate", .05f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Boss_AI>();
        if (enemy != null)
        {
            enemy.Damage(expdamage);
        }
    }

    IEnumerator Disappate(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
