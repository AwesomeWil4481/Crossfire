using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            ammoText.enabled = true;
            ammoCount = player.GetComponent<PlayerCombat>().bulletAmmo;
            ammoNumber = maxAmmo.Substring(0, ammoCount);
            ammoText.text = ammoNumber;
        }
        else if (player.GetComponent<PlayerCombat>().debugAmmo == true)
        {
            ammoText.enabled = false;
        }
    }
}
