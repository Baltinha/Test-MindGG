using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    [Header("FindMaches")]
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

    private List<GameObject> IsRowBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.RowBomb)
            m_currentMatches.Union(GetRowPieces(dot1.Row));
        

        if (dot2.RowBomb)
            m_currentMatches.Union(GetRowPieces(dot2.Row));
        

        if (dot3.RowBomb)
            m_currentMatches.Union(GetRowPieces(dot3.Row));
        
        return currentDots;
    }

    private List<GameObject> IsColumnBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.ColumnBomb)
            m_currentMatches.Union(GetColumnPieces(dot1.Colunm));
        

        if (dot2.ColumnBomb)
            m_currentMatches.Union(GetColumnPieces(dot2.Colunm));
        

        if (dot3.ColumnBomb)
            m_currentMatches.Union(GetColumnPieces(dot3.Colunm));
        
        return currentDots;
    }

    private void AddToListAndMatch(GameObject dot)
    {
        if (!m_currentMatches.Contains(dot))
            m_currentMatches.Add(dot);
        
        dot.GetComponent<Dot>().Matched = true;
    }

    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < m_board.Width; i++)
        {
            for (int j = 0; j < m_board.Height; j++)
            {
                GameObject currentDot = m_board.AllDots[i, j];
                if (currentDot != null)
                {
                    Dot currentDotDot = currentDot.GetComponent<Dot>();
                    if (i > 0 && i < m_board.Width - 1)
                    {
                        GameObject leftDot = m_board.AllDots[i - 1, j];
                        GameObject rightDot = m_board.AllDots[i + 1, j];
                        
                        if (leftDot != null && rightDot != null)
                        {
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                m_currentMatches.Union(IsRowBomb(leftDotDot, currentDotDot, rightDotDot));
                                m_currentMatches.Union(IsColumnBomb(leftDotDot, currentDotDot, rightDotDot));
                                GetNearbyPieces(leftDot, currentDot, rightDot);

                            }
                        }
                    }

                    if (j > 0 && j < m_board.Height - 1)
                    {
                        GameObject upDot = m_board.AllDots[i, j + 1];
                        GameObject downDot = m_board.AllDots[i, j - 1];
                        if (upDot != null && downDot != null)
                        {
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            Dot downDotDot = downDot.GetComponent<Dot>();
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                m_currentMatches.Union(IsColumnBomb(upDotDot, currentDotDot, downDotDot));
                                m_currentMatches.Union(IsRowBomb(upDotDot, currentDotDot, downDotDot));
                                GetNearbyPieces(upDot, currentDot, downDot);

                            }
                        }
                    }

                }
            }
        }

    }

    public void BombColorPieces(string color) 
    {
        for (int i = 0; i < m_board.Width; i++) 
        {
            for (int j = 0; j < m_board.Height; j++) 
            {
                if (m_board.AllDots[i,j] != null)
                {
                    if (m_board.AllDots[i, j].CompareTag(color))
                    {
                        m_board.AllDots[i, j].GetComponent<Dot>().Matched = true;
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
                m_board.AllDots[Column, i].GetComponent<Dot>().Matched = true;
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
                m_board.AllDots[i,Row].GetComponent<Dot>().Matched = true;
            }
        }
        return Dots;
    }

    public void CheckBombs() 
    {
        if (m_board.CurrentDot != null)
        {
            if (m_board.CurrentDot.Matched)
            {
                m_board.CurrentDot.Matched = false;
                if (m_board.CurrentDot.SwipeAngle > -45 && m_board.CurrentDot.SwipeAngle <= 45
                    ||(m_board.CurrentDot.SwipeAngle < -135 || m_board.CurrentDot.SwipeAngle >= 135))
                    m_board.CurrentDot.MakeRowBomb();
                
                else
                    m_board.CurrentDot.MakeColumnBomb();

            }
            else if (m_board.CurrentDot.OntherDot != null)
            {
                Dot ontherDot = m_board.CurrentDot.OntherDot.GetComponent<Dot>();
                if (ontherDot.Matched) 
                {
                    if (m_board.CurrentDot.SwipeAngle > -45 && m_board.CurrentDot.SwipeAngle <= 45
                    || (m_board.CurrentDot.SwipeAngle < -135 || m_board.CurrentDot.SwipeAngle >= 135))
                        ontherDot.MakeRowBomb();
                    
                    else
                        ontherDot.MakeColumnBomb();


                }
            }
        }
    }
}
