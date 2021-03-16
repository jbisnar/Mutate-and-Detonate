using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bomb : MonoBehaviour
{
    float gravNormal = 3f;
    float maxdamage = 10f;
    public Vector2 temp;
    public GameObject explosion;
    Boss_AI enemyInRange;
    public float distToEnemy = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temp = transform.GetComponent<Rigidbody2D>().velocity;
        temp.y -= gravNormal * Time.deltaTime;
        transform.GetComponent<Rigidbody2D>().velocity = temp;

        if (enemyInRange != null)
        {
            if (distToEnemy < Vector2.Distance(transform.position, enemyInRange.transform.position))
            {
                Explode();
            }
            else
            {
                distToEnemy = Vector2.Distance(transform.position, enemyInRange.transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyInRange = collision.GetComponent<Boss_AI>();
        if (enemyInRange != null)
        {
            distToEnemy = Vector2.Distance(transform.position, enemyInRange.transform.position);
        }
    }

    public void Explode()
    {
        var spawnedExp = GameObject.Instantiate(explosion, transform.position, transform.rotation);
        spawnedExp.transform.parent = null;
        spawnedExp.GetComponent<Projectile_Explosion>().expdamage = Mathf.Max(((2f - distToEnemy) / 1.5f), .2f) *maxdamage;
        Destroy(gameObject);
    }
}
