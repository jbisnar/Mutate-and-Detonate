using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Beta : MonoBehaviour
{
    public float projspeed = 4f;
    public float damage;
    int bounces = 6;
    public LayerMask layerGround;
    public GameObject gasproj;
    public bool emitgas = false;
    public float gasdamage;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = (new Vector2(Mathf.Cos(transform.localRotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.localRotation.eulerAngles.z * Mathf.Deg2Rad))) * projspeed;
        if (emitgas)
        {
            StartCoroutine("SpawnGas", .15f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 curvel = GetComponent<Rigidbody2D>().velocity;
        bool bounceH = Physics2D.OverlapArea(new Vector2(transform.position.x - .07f, transform.position.y - .02f),
            new Vector2(transform.position.x + .07f, transform.position.y - .02f), layerGround);
        bool bounceV = Physics2D.OverlapArea(new Vector2(transform.position.x - .02f, transform.position.y - .07f),
            new Vector2(transform.position.x + .02f, transform.position.y - .07f), layerGround);
        var bounceray = Physics2D.Raycast(transform.position, curvel, .07f, layerGround);
        
        if (bounceray)
        {
            curvel = Vector2.Reflect(curvel, bounceray.normal);
            GetComponent<Rigidbody2D>().velocity = curvel;
            bounces--;
            if (bounces < 1)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player_Combat>();
        if (player != null)
        {
            player.Damage(damage);
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnGas(float delay)
    {
        var spawnedGas = GameObject.Instantiate(gasproj, transform.position, transform.rotation);
        spawnedGas.transform.parent = null;
        spawnedGas.GetComponent<Projectile_Gas>().damage = gasdamage;
        yield return new WaitForSeconds(delay);
        StartCoroutine("SpawnGas", .15f);
    }
}
