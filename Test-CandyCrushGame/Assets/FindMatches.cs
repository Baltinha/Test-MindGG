using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board m_board;
    [SerializeField]private List<GameObject> m_currentMatches = new List<GameObject>();
     
    // Start is called before the first frame update
    void Start()
    {
        m_board = FindAnyObjectByType<Board>();
    }

    

}
