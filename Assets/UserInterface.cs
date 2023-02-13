using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public string ammoNumber = "";
    public string maxAmmo = "IIIIIIIIIIIIIIIIIIII";
    public int ammoCount;
    public Text ammoText;

    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player.GetComponent<PlayerCombat>().debugAmmo == false)
        {
            print("Ammo On");
            ammoText.enabled = true;
            ammoCount = player.GetComponent<PlayerCombat>().bulletAmmo;
            ammoNumber = maxAmmo.Substring(0, ammoCount);
            ammoText.text = ammoNumber;
        }
        else if (player.GetComponent<PlayerCombat>().debugAmmo == true)
        {
            print("Ammo Off");
            ammoText.enabled = false;
        }
    }
}