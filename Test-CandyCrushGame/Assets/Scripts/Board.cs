using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]private int m_width;
    [SerializeField] private int m_height;
    private BackgroundTile[,] m_allTiles;
    private GameObject[,] m_allDots;
    [SerializeField]private GameObject[] m_dots;


    public GameObject TilePrefab;

    public GameObject [,] AllDots { get =>m_allDots; }

    public int Width { get => m_width; }
    public int Height { get => m_height; }

    // Start is called before the first frame update
    void Start()
    {
       m_allTiles = new BackgroundTile[m_width, m_height];
       m_allDots = new GameObject[m_width, m_height];
       SetUp();
    }


    private void SetUp(){
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++) 
            {
                Vector2 tempPosition = new Vector2(i, j);
                /*GameObject backGroundTile = Instantiate(TilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backGroundTile.transform.parent = transform;
                backGroundTile.name = "( " + i + "," + j + ")";*/
                int dotToUse = Random.Range(0, m_dots.Length);
                while (MatchesAt(i, j, m_dots[dotToUse]) ) 
                {
                    dotToUse = Random.Range(0, m_dots.Length);
                    
                }
                GameObject dot = Instantiate(m_dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "( " + i + "," + j + ")";
                m_allDots[i,j] = dot;
            }
        }
    }


    private bool MatchesAt(int Colunm, int Row, GameObject Piece) 
    {
        if (Colunm > 1) 
        {
            if (m_allDots[Colunm -1 , Row].GetComponent<Dot>().tag == Piece.GetComponent<Dot>().tag && m_allDots[Colunm - 2, Row].GetComponent<Dot>().tag == Piece.GetComponent<Dot>().tag)
            {
                return true;
            }
        }
        if (Row > 1) 
        {
            if (m_allDots[Colunm, Row - 1].GetComponent<Dot>().tag == Piece.GetComponent<Dot>().tag && m_allDots[Colunm, Row - 2].GetComponent<Dot>().tag == Piece.GetComponent<Dot>().tag)
            {
                return true;
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int Column, int Row) 
    {
        if (m_allDots[Column, Row].GetComponent<Dot>().Mactched)
        {
            Destroy(m_allDots[Column, Row]);
            m_allDots[Column, Row] = null;
        }
    }

    public void DestroyMetches() 
    {
        for (int i = 0; i < m_width; i++) 
        {
            for (int j=0; j < m_height; j++) 
            {
                if (m_allDots[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
       StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo() 
    {
        int nullCount = 0;

        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                if (m_allDots[i,j]== null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    m_allDots[i, j].GetComponent<Dot>().Row -= nullCount;
                    m_allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefilBoard()
    {
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                if (m_allDots[i,j] == null)
                {
                    Vector2 tempPosition = new Vector2(i,j);
                    int dotToUse = Random.Range(0, m_dots.Length);
                    GameObject piece = Instantiate(m_dots[dotToUse], tempPosition, Quaternion.identity);
                    m_allDots[i,j] = piece;
                }
            }
        }
    }

    private bool MatchesOnBoard() 
    {
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                if (m_allDots[i, j] != null)
                {
                    if (m_allDots[i,j].GetComponent<Dot>().Mactched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo() 
    {
        RefilBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard()) 
        {
            yield return new WaitForSeconds(.5f);
            DestroyMetches();
        }
    }

}
