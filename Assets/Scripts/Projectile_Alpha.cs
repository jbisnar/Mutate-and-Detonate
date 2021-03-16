using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Alpha : MonoBehaviour
{
    public float projspeed = 2.5f;
    public float damage;
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

    IEnumerator SpawnGas(float delay)
    {
        var spawnedGas = GameObject.Instantiate(gasproj, transform.position, transform.rotation);
        spawnedGas.transform.parent = null;
        spawnedGas.GetComponent<Projectile_Gas>().damage = gasdamage;
        yield return new WaitForSeconds(delay);
        StartCoroutine("SpawnGas", .15f);
    }
}
