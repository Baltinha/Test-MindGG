using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] TextMeshProUGUI m_scoreUI;
    [SerializeField] private int m_score;
    [SerializeField] int m_nexlevel = 2000;

    public int Score { get => m_score; set => m_score = value; } 


    // Update is called once per frame
    void Update()
    {
        m_scoreUI.text = m_score.ToString();
    }

    public void IncreaseScore(int AmountToIncrise) 
    {
       m_score += AmountToIncrise; 
    }

    public void NextLevel()
    {
        if (m_score >= m_nexlevel)
        {
            Debug.Log("Proxima Fase");
        }
    }
}
