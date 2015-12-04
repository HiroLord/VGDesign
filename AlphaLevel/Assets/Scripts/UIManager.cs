using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    GameObject clip1, clip2, clip3, clip4, clip5;
    int clipCount = 5;

    Text ammotext;
    int currentAmmo = 60, maxAmmo = 60;

    Image power1, power2, power3, power4, power5, power6, power7;
    int powerCount = 0;

    public Sprite icon1, icon2, icon3, icon4, icon5;

	// Use this for initialization
	void Start () {
        clip1 = transform.Find("Main Panel/AmmoPanel/Clip Graphics/Clip 1").gameObject;
        clip2 = transform.Find("Main Panel/AmmoPanel/Clip Graphics/Clip 2").gameObject;
        clip3 = transform.Find("Main Panel/AmmoPanel/Clip Graphics/Clip 3").gameObject;
        clip4 = transform.Find("Main Panel/AmmoPanel/Clip Graphics/Clip 4").gameObject;
        clip5 = transform.Find("Main Panel/AmmoPanel/Clip Graphics/Clip 5").gameObject;

        ammotext = transform.Find("Main Panel/AmmoPanel/AmmoText").gameObject.GetComponent<Text>();

        power1 = transform.Find("Main Panel/PowersPanel/Powers/Power 1").gameObject.GetComponent<Image>();
        power2 = transform.Find("Main Panel/PowersPanel/Powers/Power 2").gameObject.GetComponent<Image>();
        power3 = transform.Find("Main Panel/PowersPanel/Powers/Power 3").gameObject.GetComponent<Image>();
        power4 = transform.Find("Main Panel/PowersPanel/Powers/Power 4").gameObject.GetComponent<Image>();
        power5 = transform.Find("Main Panel/PowersPanel/Powers/Power 5").gameObject.GetComponent<Image>();
        power6 = transform.Find("Main Panel/PowersPanel/Powers/Power 6").gameObject.GetComponent<Image>();
        power7 = transform.Find("Main Panel/PowersPanel/Powers/Power 7").gameObject.GetComponent<Image>();
    }

    void Update () {
        //if (Input.GetKeyDown(KeyCode.Minus)) {
        //    removeClip();
        //}

        //if (Input.GetKeyDown(KeyCode.Equals))
        //{
        //    addClip();
        //}
    }

    // Clip Functions //
    public void addClip () {
        if (clipCount < 5) {
            switch (clipCount) {
                case 0: clip1.SetActive(true); break;
                case 1: clip2.SetActive(true); break;
                case 2: clip3.SetActive(true); break;
                case 3: clip4.SetActive(true); break;
                case 4: clip5.SetActive(true); break;
            }
            clipCount++;
        }
    }

    public void removeClip() {
        if (clipCount > 0) {
            switch (clipCount) {
                case 1: clip1.SetActive(false); break;
                case 2: clip2.SetActive(false); break;
                case 3: clip3.SetActive(false); break;
                case 4: clip4.SetActive(false); break;
                case 5: clip5.SetActive(false); break;
            }
            clipCount--;
        }
    }

    // Ammo Text Functions //
    public void updateAmmoText () {
        ammotext.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
    }

    public void setCurrentAmmo (int current) {
        currentAmmo = current;
        updateAmmoText();
    }

    public void reduceCurrentAmmo () {
        if (currentAmmo > 0) {
            currentAmmo--;
            updateAmmoText();
        }
    }

    public void setAmmoMax (int max) {
        maxAmmo = max;
        updateAmmoText();
    }

    // Power Functions //
    public void addPower (int code) {
        if (powerCount < 7) {
            powerCount++;
            switch (powerCount) {
                case 1: power1.sprite = getSprite(code); break;
                case 2: power2.sprite = getSprite(code); break;
                case 3: power3.sprite = getSprite(code); break;
                case 4: power4.sprite = getSprite(code); break;
                case 5: power5.sprite = getSprite(code); break;
                case 6: power6.sprite = getSprite(code); break;
                case 7: power7.sprite = getSprite(code); break;
            }
        }
    }

    private Sprite getSprite (int code) {
        Sprite ret = null;
        switch (code) {
            case 1: ret = icon1; break;
            case 2: ret = icon2; break;
            case 3: ret = icon3; break;
            case 4: ret = icon4; break;
            case 5: ret = icon5; break;
        }
        return ret;
    }
}
