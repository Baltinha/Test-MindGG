using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board m_board;
    [SerializeField]private List<GameObject> m_currentMatches = new List<GameObject>();
     
    public List<GameObject> CurrentMatches { get => m_currentMatches; set => m_currentMatches = value; } 
    // Start is called before the first frame update
    void Start()
    {
        m_board = FindAnyObjectByType<Board>();
    }

    public void FindAllMatches() 
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo() 
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < m_board.Width; i++)
        {
            for (int j = 0; j < m_board.Height; j++)
            {
                GameObject currentDot = m_board.AllDots[i,j];

                if (currentDot != null) 
                {
                    if (i>0 && i < m_board.Width -1)
                    {
                        GameObject leftDot = m_board.AllDots[i - 1, j];
                        GameObject rightDot = m_board.AllDots[i + 1, j];
                        if (leftDot != null && rightDot != null) 
                        {
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                if (currentDot.GetComponent<Dot>().RowBomb || leftDot.GetComponent<Dot>().RowBomb
                                    || rightDot.GetComponent<Dot>().RowBomb)
                                {
                                    m_currentMatches.Union(GetRowPieces(j)); 
                                }

                                if (!m_currentMatches.Contains(leftDot)) 
                                   m_currentMatches.Add(leftDot);
                                leftDot.GetComponent<Dot>().Mactched = true;
                                if (!m_currentMatches.Contains(rightDot))
                                    m_currentMatches.Add(rightDot);
                                rightDot.GetComponent<Dot>().Mactched = true;
                                if (!m_currentMatches.Contains(currentDot))
                                    m_currentMatches.Add(currentDot);
                                currentDot.GetComponent<Dot>().Mactched = true;
                            }
                        }
                    }
                    if (j > 0 && j < m_board.Height - 1)
                    {
                        GameObject upDot = m_board.AllDots[i , j + 1];
                        GameObject downDot = m_board.AllDots[i , j - 1];
                        if (upDot != null && downDot != null)
                        {
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                if (!m_currentMatches.Contains(upDot))
                                    m_currentMatches.Add(upDot);
                                upDot.GetComponent<Dot>().Mactched = true;
                                if (!m_currentMatches.Contains(downDot))
                                    m_currentMatches.Add(downDot);
                                downDot.GetComponent<Dot>().Mactched = true;
                                if (!m_currentMatches.Contains(currentDot))
                                    m_currentMatches.Add(currentDot);
                                currentDot.GetComponent<Dot>().Mactched = true;
                            }
                        }
                    }
                }

            }
        }
    }

    List<GameObject> GetColumnPieces(int Column) 
    {
        List<GameObject> Dots = new List<GameObject>();
        for (int i = 0; i < m_board.Height; i++) 
        {
            if (m_board.AllDots[Column, i] != null)
            {
                Dots.Add(m_board.AllDots[Column, i]);
                m_board.AllDots[Column, i].GetComponent<Dot>().Mactched = true;
            }
        }
        return Dots;
    }

    List<GameObject> GetRowPieces(int Row)
    {
        List<GameObject> Dots = new List<GameObject>();
        for (int i = 0; i < m_board.Width; i++)
        {
            if (m_board.AllDots[i, Row] != null)
            {
                Dots.Add(m_board.AllDots[i,Row]);
                m_board.AllDots[i,Row].GetComponent<Dot>().Mactched = true;
            }
        }
        return Dots;
    }
}
