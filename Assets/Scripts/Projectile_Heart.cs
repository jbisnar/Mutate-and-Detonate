using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Heart : MonoBehaviour
{
    public GameObject player;
    public Transform fsteer;
    public Transform lsteer;
    public Transform rsteer;
    float movespeed = 3f;
    public float damage;
    public LayerMask layerGround;
    public GameObject gasproj;
    public bool emitgas = false;
    public float gasdamage;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_Move>().gameObject;
        if (emitgas)
        {
            StartCoroutine("SpawnGas", .15f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        var playpos = player.transform.position;
        var losray = Physics2D.Raycast(transform.position, playpos - transform.position,Vector2.Distance(transform.position,playpos), layerGround);
        if (!losray)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(playpos.y - transform.position.y, playpos.x - transform.position.x) * Mathf.Rad2Deg + 90, Vector3.forward), 135 * Time.deltaTime);
        }
        else
        {
            var fray = Physics2D.CircleCast(transform.position, .27f, fsteer.position - transform.position, 2f, layerGround);
            if (fray)
            {
                var lray = Physics2D.Raycast(transform.position, lsteer.position - transform.position, 2f, layerGround);
                var rray = Physics2D.Raycast(transform.position, rsteer.position - transform.position, 2f, layerGround);
                var ldist = 2f;
                var rdist = 2f;
                if (lray) { ldist = lray.distance; }
                if (rray) { rdist = rray.distance; }
                if (rdist < ldist)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(lsteer.position.y - transform.position.y, lsteer.position.x - transform.position.x) * Mathf.Rad2Deg + 90, Vector3.forward), 135 * Time.deltaTime);
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(rsteer.position.y - transform.position.y, rsteer.position.x - transform.position.x) * Mathf.Rad2Deg + 90, Vector3.forward), 135 * Time.deltaTime);
                }
            }
        }
        transform.position += -transform.up * movespeed * Time.deltaTime;
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
    IEnumerator SpawnGas(float delay)
    {
        var spawnedGas = GameObject.Instantiate(gasproj, transform.position, transform.rotation);
        spawnedGas.transform.parent = null;
        spawnedGas.GetComponent<Projectile_Gas>().damage = gasdamage;
        yield return new WaitForSeconds(delay);
        StartCoroutine("SpawnGas", .15f);
    }
}
