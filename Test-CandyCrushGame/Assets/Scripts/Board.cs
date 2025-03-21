using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]private int m_width;
    [SerializeField] private int m_height;
    private BackgroundTile[,] m_allTiles;
    private GameObject[,] m_allDots;
    [SerializeField]private GameObject[] m_dots;


    public GameObject TilePrefab;

    // Start is called before the first frame update
    void Start()
    {
       m_allTiles = new BackgroundTile[m_width, m_height];
       m_allDots = new GameObject[m_width, m_height];
       SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetUp(){
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++) 
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backGroundTile = Instantiate(TilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backGroundTile.transform.parent = transform;
                backGroundTile.name = "( " + i + "," + j + ")";
                int dotToUse = Random.Range(0, m_dots.Length);
                GameObject dot = Instantiate(m_dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "( " + i + "," + j + ")";
                m_allDots[i,j] = dot;
            }
        }
    }
}
