using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_AI : MonoBehaviour
{
    public GameObject player;
    public GameObject alphaproj;
    int alphacount = 6;
    float alphadamage = 10f;
    public GameObject betaproj;
    float betadamage = 10f;
    public GameObject gammaproj;
    float gammadamage = 15f;
    public Transform Telenodes;
    bool gasActive = false;
    float gasdamage = 5f;
    public GameObject boneproj;
    float bonedamage = 20f;
    public GameObject heartproj;
    float heartdamage = 20f;
    List<string> availableActions;
    float alphacool = 4f;
    float betacool = 1.5f;
    float gammacool = 1.5f;
    float bonecool = 2f;
    float heartcool = 3f;
    float telecool = 2f;
    float telecool2 = 16f;
    float coolcoef = 1f;
    bool waiting = true;
    bool canattack = false;
    public float maxhealth = 150f;
    public float curhealth;
    public RectTransform healthBar;
    public RectTransform healthBGBar;
    float healthbarWidth;
    public GameObject grave;
    int upgrades = 0;
    bool upgradelock = false;
    List<string> availableUpgrades;
    public Upgrade_Panel upanel1;
    public Upgrade_Panel upanel2;
    public Upgrade_Panel upanel3;
    Upgrade_Panel[] upanelArray = new Upgrade_Panel[3];

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_Move>().gameObject;
        curhealth = maxhealth;
        healthbarWidth = healthBar.rect.width;

        availableActions = new List<string>();
        availableActions.Add("AlphaAttack");
        availableActions.Add("BetaBullet");
        availableActions.Add("GammaRay");
        availableActions.Add("Teleport");

        availableUpgrades = new List<string>();
        availableUpgrades.Add("Third Eye");
        availableUpgrades.Add("Free Hand");
        availableUpgrades.Add("Elbow Grease");
        availableUpgrades.Add("Gut Reaction");
        availableUpgrades.Add("Bare Bones");
        availableUpgrades.Add("Broken Heart");

        upanelArray[0] = upanel1;
        upanelArray[1] = upanel2;
        upanelArray[2] = upanel3;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.sizeDelta = new Vector2((curhealth / maxhealth) * healthbarWidth, healthBar.sizeDelta.y);

        if (canattack)
        {
            var randAttack = availableActions[Random.Range(0,availableActions.Count)];
            if (randAttack == "AlphaAttack")
            {
                AlphaAttack();
            }
            else if (randAttack == "BetaBullet")
            {
                BetaBullet();
            }
            else if (randAttack == "GammaRay")
            {
                GammaRay();
            }
            else if (randAttack == "Teleport")
            {
                Teleport();
            }
            else if (randAttack == "BoneBarrier")
            {
                BoneBarrier();
            }
            else if (randAttack == "HeartMissile")
            {
                HeartMissile();
            }
        }
    }

    void AlphaAttack()
    {
        float initangle = Random.Range(0f, 360 / alphacount);
        for (int i = 0; i < alphacount; i++)
        {
            var projangle = initangle + (360 / alphacount) * i;
            var spawnedproj = GameObject.Instantiate(alphaproj, transform.position, Quaternion.Euler(0f,0f,projangle));
            spawnedproj.transform.parent = null;
            spawnedproj.GetComponent<Projectile_Alpha>().damage = alphadamage;
            if (gasActive)
            {
                spawnedproj.GetComponent<Projectile_Alpha>().emitgas = true;
                spawnedproj.GetComponent<Projectile_Alpha>().gasdamage = gasdamage;
            }
        }
        StartCoroutine("Cooldown", alphacool * coolcoef);
        canattack = false;
    }

    void BetaBullet()
    {
        var spawnedproj = GameObject.Instantiate(betaproj, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f,360f)));
        spawnedproj.transform.parent = null;
        spawnedproj.GetComponent<Projectile_Beta>().damage = betadamage;
        if (gasActive)
        {
            spawnedproj.GetComponent<Projectile_Beta>().emitgas = true;
            spawnedproj.GetComponent<Projectile_Beta>().gasdamage = gasdamage;
        }
        StartCoroutine("Cooldown", betacool * coolcoef);
        canattack = false;
    }

    void GammaRay()
    {
        var projangle = Random.Range(0f, 360f);
        if (player != null)
        {
            projangle = Mathf.Atan2((player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x))*Mathf.Rad2Deg;
        }
        var spawnedproj = GameObject.Instantiate(gammaproj, transform.position, Quaternion.Euler(0f, 0f, projangle));
        spawnedproj.transform.parent = null;
        spawnedproj.GetComponent<Projectile_Gamma>().damage = gammadamage;
        if (gasActive)
        {
            spawnedproj.GetComponent<Projectile_Gamma>().emitgas = true;
            spawnedproj.GetComponent<Projectile_Gamma>().gasdamage = gasdamage;
        }
        StartCoroutine("Cooldown", gammacool * coolcoef);
        canattack = false;
    }

    void BoneBarrier()
    {
        var projangle = Random.Range(0f, 360f);
        var projdist = Random.Range(1f, 3.5f);
        var spawnedproj = GameObject.Instantiate(boneproj, transform.position + new Vector3(projdist*Mathf.Cos(projangle*Mathf.Deg2Rad), projdist * Mathf.Sin(projangle * Mathf.Deg2Rad), 0f), Quaternion.Euler(0f, 0f, projangle));
        spawnedproj.transform.parent = null;
        spawnedproj.GetComponent<Projectile_Bone>().damage = bonedamage;
        StartCoroutine("Cooldown", bonecool * coolcoef);
        canattack = false;
    }

    void HeartMissile()
    {
        var spawnedproj = GameObject.Instantiate(heartproj, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        spawnedproj.transform.parent = null;
        spawnedproj.GetComponent<Projectile_Heart>().damage = heartdamage;
        if (gasActive)
        {
            spawnedproj.GetComponent<Projectile_Heart>().emitgas = true;
            spawnedproj.GetComponent<Projectile_Heart>().gasdamage = gasdamage;
        }
        StartCoroutine("Cooldown", heartcool * coolcoef);
        canattack = false;
    }

    void Teleport()
    {
        var destnode = Telenodes.GetChild(Random.Range(0, Telenodes.childCount-1));
        transform.position = destnode.position;
        destnode.SetAsLastSibling();
        StartCoroutine("Cooldown", telecool * coolcoef);
        StartCoroutine("TeleportCooldown", telecool2);
        canattack = false;
    }

    public void Damage(float damageAmount)
    {
        if (waiting)
        {
            Teleport();
            waiting = false;
        }
        else
        {
            curhealth -= damageAmount;
        }

        if (curhealth <= 0)
        {
            healthBar.sizeDelta = new Vector2(0, healthBar.sizeDelta.y);
            var spawnedgrave = GameObject.Instantiate(grave, transform.position, transform.rotation);
            spawnedgrave.transform.parent = null;
            Destroy(gameObject);
        }
        else if (curhealth < maxhealth * (1f / 4f) && upgrades < 3 && !upgradelock)
        {
            upgradelock = true;
            StartCoroutine("ScrollUpgrade", 3f);
        }
        else if (curhealth < maxhealth * (1f / 2f) && upgrades < 2 && !upgradelock)
        {
            upgradelock = true;
            StartCoroutine("ScrollUpgrade", 3f);
        }
        else if (curhealth < maxhealth * (3f / 4f) && upgrades < 1 && !upgradelock)
        {
            upgradelock = true;
            StartCoroutine("ScrollUpgrade", 3f);
        }
    }

    public void Upgrade()
    {
        upgrades++;
        var chosenUpgrade = Random.Range(0, availableUpgrades.Count);
        var chosenUpgradeString = availableUpgrades[chosenUpgrade];
        availableUpgrades.RemoveAt(chosenUpgrade);
        if (chosenUpgradeString == "Third Eye")
        {
            telecool2 = 8f;
        }
        else if (chosenUpgradeString == "Free Hand")
        {
            coolcoef = (2f / 3f);
        }
        else if (chosenUpgradeString == "Elbow Grease")
        {
            alphadamage = 20f;
            betadamage = 20f;
            gammadamage = 30f;
            gasdamage = 10f;
            bonedamage = 40f;
            heartdamage = 40f;
        }
        else if (chosenUpgradeString == "Gut Reaction")
        {
            gasActive = true;
        }
        else if (chosenUpgradeString == "Bare Bones")
        {
            availableActions.Add("BoneBarrier");
        }
        else
        {
            availableActions.Add("HeartMissile");
        }
        upgradelock = false;
        upanelArray[upgrades - 1].ChooseUpgrade(chosenUpgradeString);
    }

    IEnumerator Cooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        canattack = true;
    }

    IEnumerator TeleportCooldown(float delay)
    {
        availableActions.Remove("Teleport");
        yield return new WaitForSeconds(delay);
        availableActions.Add("Teleport");
    }

    IEnumerator ScrollUpgrade(float delay)
    {
        upanelArray[upgrades].gameObject.SetActive(true);
        upanelArray[upgrades].ScrollUpgrade();
        yield return new WaitForSeconds(delay);
        Upgrade();
    }
}
