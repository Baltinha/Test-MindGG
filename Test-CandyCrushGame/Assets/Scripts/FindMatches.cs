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
        yield return null; // Deixe o código continuar em um novo frame para evitar bloqueio

        // Recorre sobre todas as células do tabuleiro
        for (int i = 0; i < m_board.Width; i++)
        {
            for (int j = 0; j < m_board.Height; j++)
            {
                GameObject currentDot = m_board.AllDots[i, j];

                if (currentDot != null)
                {
                    Dot currentDotComponent = currentDot.GetComponent<Dot>();

                    // Verificar se há correspondências na linha (horizontal)
                    if (i > 0 && i < m_board.Width - 1)
                    {
                        CheckHorizontalMatch(i, j, currentDot, currentDotComponent);
                    }

                    // Verificar se há correspondências na coluna (vertical)
                    if (j > 0 && j < m_board.Height - 1)
                    {
                        CheckVerticalMatch(i, j, currentDot, currentDotComponent);
                    }
                }
            }
        }
    }

    // Verificar correspondências na linha
    private void CheckHorizontalMatch(int i, int j, GameObject currentDot, Dot currentDotComponent)
    {
        GameObject leftDot = m_board.AllDots[i - 1, j];
        GameObject rightDot = m_board.AllDots[i + 1, j];

        if (leftDot != null && rightDot != null)
        {
            Dot leftDotComponent = leftDot.GetComponent<Dot>();
            Dot rightDotComponent = rightDot.GetComponent<Dot>();

            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
            {
                if (currentDotComponent.RowBomb || leftDotComponent.RowBomb || rightDotComponent.RowBomb)
                    m_currentMatches.Union(GetRowPieces(j));

                AddMatchToCurrentMatches(leftDot, leftDotComponent);
                AddMatchToCurrentMatches(rightDot, rightDotComponent);
                AddMatchToCurrentMatches(currentDot, currentDotComponent);
            }
        }
    }

    // Verificar correspondências na coluna
    private void CheckVerticalMatch(int i, int j, GameObject currentDot, Dot currentDotComponent)
    {
        GameObject upDot = m_board.AllDots[i, j + 1];
        GameObject downDot = m_board.AllDots[i, j - 1];

        if (upDot != null && downDot != null)
        {
            Dot upDotComponent = upDot.GetComponent<Dot>();
            Dot downDotComponent = downDot.GetComponent<Dot>();

            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
            {
                if (currentDotComponent.ColumnBomb || upDotComponent.ColumnBomb || downDotComponent.ColumnBomb)
                    m_currentMatches.Union(GetColumnPieces(i));

                AddMatchToCurrentMatches(upDot, upDotComponent);
                AddMatchToCurrentMatches(downDot, downDotComponent);
                AddMatchToCurrentMatches(currentDot, currentDotComponent);
            }
        }
    }

    // Adicionar uma correspondência ao conjunto de correspondências atuais
    private void AddMatchToCurrentMatches(GameObject dot, Dot dotComponent)
    {
        if (!m_currentMatches.Contains(dot))
        {
            m_currentMatches.Add(dot);
        }
        dotComponent.Mactched = true;
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
                        m_board.AllDots[i, j].GetComponent<Dot>().Mactched = true;
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

    public void CheckBombs() 
    {
        if (m_board.CurrentDot != null)
        {
            if (m_board.CurrentDot.Mactched)
            {
                m_board.CurrentDot.Mactched = false;
                if (m_board.CurrentDot.SwipeAngle > -45 && m_board.CurrentDot.SwipeAngle <= 45
                    ||(m_board.CurrentDot.SwipeAngle < -135 || m_board.CurrentDot.SwipeAngle >= 135))
                    m_board.CurrentDot.MakeRowBomb();
                
                else
                    m_board.CurrentDot.MakeColumnBomb();

            }
            else if (m_board.CurrentDot.OntherDot != null)
            {
                Dot ontherDot = m_board.CurrentDot.OntherDot.GetComponent<Dot>();
                if (ontherDot.Mactched) 
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
