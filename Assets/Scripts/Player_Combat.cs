using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Combat : MonoBehaviour
{
    public GameObject bomb;
    bool bombready = true;
    float reloadtime = 1f;
    public float maxhealth = 100f;
    public float curhealth;
    public RectTransform healthBar;
    public RectTransform healthBGBar;
    float healthbarWidth;
    public GameObject grave;

    // Start is called before the first frame update
    void Start()
    {
        curhealth = maxhealth;
        healthbarWidth = healthBar.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.sizeDelta = new Vector2((curhealth / maxhealth) * healthbarWidth, healthBar.sizeDelta.y);

        if (Input.GetMouseButtonDown(1) && bombready)
        {
            var spawnedbomb = GameObject.Instantiate(bomb, transform.position, transform.rotation);
            spawnedbomb.GetComponent<Rigidbody2D>().velocity = GetComponent<Player_Move>().temp;
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity/2f;
            bombready = false;
            StartCoroutine("Reload", reloadtime);
        }

        if (Input.GetKey("escape"))
        {
            Debug.Log("Quitting");
            Application.Quit();
        }

        if (Input.GetKey("r"))
        {
            Debug.Log("Restarting");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Damage(float damageAmount)
    {
        curhealth -= damageAmount;
        if (curhealth <= 0)
        {
            healthBar.sizeDelta = new Vector2(0, healthBar.sizeDelta.y);
            var spawnedgrave = GameObject.Instantiate(grave, transform.position, transform.rotation);
            spawnedgrave.transform.parent = null;
            Destroy(gameObject);
        }
    }

    IEnumerator Reload(float delay)
    {
        yield return new WaitForSeconds(delay);
        bombready = true;
    }
}
