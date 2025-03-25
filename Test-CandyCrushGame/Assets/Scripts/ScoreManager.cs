using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] TextMeshProUGUI m_scoreUI;
    [SerializeField] TextMeshProUGUI m_nextFaseUI;
    [SerializeField] private int m_score;
    [Header("NextLevel")]
    [SerializeField] string m_levelName;
    [SerializeField] int m_nexlevel = 0;
    [SerializeField] GameObject m_nextFase;

    public int Score { get => m_score; set => m_score = value; }

    private void Start()
    {
        m_nextFaseUI.text = m_nexlevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        m_scoreUI.text = m_score.ToString();

        if (m_score >= m_nexlevel)
        {
            m_nextFase.gameObject.SetActive(true);
        }
    }

    public void IncreaseScore(int AmountToIncrise) 
    {
       m_score += AmountToIncrise; 
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(m_levelName);
    }
}
