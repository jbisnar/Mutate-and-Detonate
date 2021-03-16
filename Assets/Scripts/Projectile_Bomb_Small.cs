using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bomb_Small : MonoBehaviour
{
    float directDamage = 20f;
    public GameObject explosion;

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
        var spawnedExp = GameObject.Instantiate(explosion, transform.position, transform.rotation);
        spawnedExp.transform.parent = null;
        spawnedExp.GetComponent<Projectile_Explosion>().expdamage = directDamage;
        Destroy(transform.parent.gameObject);
    }
}
