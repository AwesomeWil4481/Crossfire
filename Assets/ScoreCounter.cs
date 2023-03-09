using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static int totalScore;
    void Update()
    {
        gameObject.GetComponent<Text>().text = totalScore.ToString();  
    }
}
