using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Panel : MonoBehaviour
{
    public Sprite eye;
    public Sprite hand;
    public Sprite elbow;
    public Sprite gut;
    public Sprite bone;
    public Sprite heart;
    public List<Sprite> upSprites;
    public Upgrade_Panel sibling1;
    public Upgrade_Panel sibling2;
    bool scrolling = false;
    public Image icon;
    int spriteframe = 0;
    public RectTransform DescPanel;
    public Text uTitle;
    public Text uDesc;

    // Start is called before the first frame update
    void Start()
    {
        upSprites = new List<Sprite>();
        upSprites.Add(eye);
        upSprites.Add(hand);
        upSprites.Add(elbow);
        upSprites.Add(gut);
        upSprites.Add(bone);
        upSprites.Add(heart);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (scrolling && spriteframe % 6 == 0)
        {
            icon.sprite = upSprites[Random.Range(0,upSprites.Count)];
        }
        spriteframe++;
        spriteframe = spriteframe % 6;
    }

    public void ScrollUpgrade()
    {
        scrolling = true;
    }

    public void ChooseUpgrade(string upgrade)
    {
        scrolling = false;
        if (upgrade == "Third Eye")
        {
            icon.sprite = eye;
            upSprites.Remove(eye);
            sibling1.upSprites.Remove(eye);
            sibling2.upSprites.Remove(eye);
            uTitle.text = "Third Eye";
            uDesc.text = "Teleports more";
            DescPanel.gameObject.SetActive(true);
        }
        else if (upgrade == "Free Hand")
        {
            icon.sprite = hand;
            upSprites.Remove(hand);
            sibling1.upSprites.Remove(hand);
            sibling2.upSprites.Remove(hand);
            uTitle.text = "Free Hand";
            uDesc.text = "Attacks faster";
            DescPanel.gameObject.SetActive(true);
        }
        else if (upgrade == "Elbow Grease")
        {
            icon.sprite = elbow;
            upSprites.Remove(elbow);
            sibling1.upSprites.Remove(elbow);
            sibling2.upSprites.Remove(elbow);
            uTitle.text = "Elbow Grease";
            uDesc.text = "More damage";
            DescPanel.gameObject.SetActive(true);
        }
        else if (upgrade == "Gut Reaction")
        {
            icon.sprite = gut;
            upSprites.Remove(gut);
            sibling1.upSprites.Remove(gut);
            sibling2.upSprites.Remove(gut);
            uTitle.text = "Gut Reaction";
            uDesc.text = "Gas trails";
            DescPanel.gameObject.SetActive(true);
        }
        else if (upgrade == "Bare Bones")
        {
            icon.sprite = bone;
            upSprites.Remove(bone);
            sibling1.upSprites.Remove(bone);
            sibling2.upSprites.Remove(bone);
            uTitle.text = "Bare Bones";
            uDesc.text = "New attack";
            DescPanel.gameObject.SetActive(true);
        }
        else
        {
            icon.sprite = heart;
            upSprites.Remove(heart);
            sibling1.upSprites.Remove(heart);
            sibling2.upSprites.Remove(heart);
            uTitle.text = "Broken Heart";
            uDesc.text = "New attack";
            DescPanel.gameObject.SetActive(true);
        }
        StartCoroutine("HideDesc", 5f);
    }

    IEnumerator HideDesc(float delay)
    {
        yield return new WaitForSeconds(delay);
        DescPanel.gameObject.SetActive(false);
    }
}
