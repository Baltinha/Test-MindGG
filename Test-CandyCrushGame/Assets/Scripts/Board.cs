using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]private int m_width;
    [SerializeField] private int m_height;
    private BackgroundTile[,] m_tiles;

    // Start is called before the first frame update
    void Start()
    {
       m_tiles = new BackgroundTile[m_width, m_height]; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
