using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    [SerializeField] private int m_colunm, m_row;
    [SerializeField] int m_previousRow, m_previousColunm;
    [SerializeField] int m_targetX;
    [SerializeField] int m_targetY;
    private bool m_mactched;
    private Board m_board;

    [Header("Dot Variables")]
    private GameObject m_otherDot;
    private Vector2 m_fristPositionTouch;
    private Vector2 m_finalDirectionTouch;
    private Vector2 m_tempTargetPos;
    private float m_swipeResist = 1f;
    [SerializeField] float m_lerpVelocity = 0;
    [SerializeField] float m_swipeAngle = 0;
    [SerializeField] float m_canMoveT = 0;
    

    [Header("FindMatches")]
    private FindMatches m_findMatches;

    [Header("PowerUps")]
    [SerializeField] bool m_iscolorBomb;
    [SerializeField] bool m_columnBomb;
    [SerializeField] bool m_rowBomb;
    [SerializeField] GameObject m_rowArrow;
    [SerializeField] GameObject m_columnArrow;
    [SerializeField] GameObject m_colorBomb;

    #region Gets and Set
    public bool ColorBomb { get => m_iscolorBomb; set => m_iscolorBomb = value; }
    public bool ColumnBomb { get => m_columnBomb; set => m_columnBomb = value; }
    public bool RowBomb { get => m_rowBomb; set => m_rowBomb = value; }
    public bool Matched { get => m_mactched; set => m_mactched = value; }
    public int PreviousRow { get => m_previousRow; }
    public int Row { get => m_row; set => m_row = value; }
    public int PreviousColunm { get => m_previousColunm; }
    public int Colunm { get => m_colunm; set => m_colunm = value; }
    public GameObject OntherDot { get => m_otherDot; set => m_otherDot = value; }
    public float SwipeAngle { get => m_swipeAngle; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_columnBomb = false;
        m_rowBomb = false;
        m_findMatches = FindAnyObjectByType<FindMatches>();
        m_board = FindAnyObjectByType<Board>();
    }

    //Isso e para testa e debugar
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            m_columnBomb = true;
            GameObject arrow = Instantiate(m_columnArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
        /*if (Input.GetMouseButtonDown(1))
        {
            m_iscolorBomb = true;
            GameObject color = Instantiate(m_colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;
            m_board.AllDots[m_colunm, m_row].tag = "Rainbow Bomb";
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        m_targetX = m_colunm;
        m_targetY = m_row;
        MovingDots();

    }

    public IEnumerator CheckMoveCo() 
    {
        if (m_iscolorBomb) 
        {
            m_findMatches.BombColorPieces(OntherDot.tag);
            m_mactched = true;
        }
        else if (m_otherDot.GetComponent<Dot>().m_iscolorBomb)
        {
            m_findMatches.BombColorPieces(gameObject.tag);
            m_otherDot.GetComponent<Dot>().Matched = true;
        }
            yield return new WaitForSeconds(.5f);
        if (m_otherDot != null)
        {
            if (!m_mactched && !m_otherDot.GetComponent<Dot>().m_mactched)
            {
                m_otherDot.GetComponent<Dot>().m_row = m_row;
                m_otherDot.GetComponent<Dot>().m_colunm = m_colunm;
                m_row = m_previousRow;
                m_colunm = m_previousColunm;
                yield return new WaitForSeconds(m_canMoveT);
                m_board.CurrentDot = null;
                m_board.State = GamesState.move;
            }
            else 
            {
                m_board.DestroyMetches();
            }

        }

    }
    private void OnMouseDown()
    {
        if (m_board.State == GamesState.move)
            m_fristPositionTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        if (m_board.State == GamesState.move)
        {
            m_finalDirectionTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculeteAngle();
        }

    }

    void CalculeteAngle()
    {
        if (MathF.Abs( m_finalDirectionTouch.y -m_fristPositionTouch.y) > m_swipeResist || 
            MathF.Abs(m_finalDirectionTouch.x - m_fristPositionTouch.x) > m_swipeResist)
        {
            m_swipeAngle = Mathf.Atan2(m_finalDirectionTouch.y - m_fristPositionTouch.y, m_finalDirectionTouch.x - m_fristPositionTouch.x) * 180 / Mathf.PI;
            MovePieces();
            m_board.State = GamesState.wait;
            m_board.CurrentDot = this;
        }
        else
        {
            m_board.State = GamesState.move;
        }

    }

    void MovePieces() 
    {
        if (m_swipeAngle > -45 && m_swipeAngle <= 45 && m_colunm < m_board.Width-1)
        {
            //Mexer para esquerda
            m_otherDot = m_board.AllDots[m_colunm + 1, m_row];
            m_previousRow = m_row;
            m_previousColunm = m_colunm;
            m_otherDot.GetComponent<Dot>().m_colunm -= 1;
            m_colunm += 1;
            
        }
        else if(m_swipeAngle > 45 && m_swipeAngle <= 135 && m_row < m_board.Height-1)
        {
            //Mexer para cima
            m_otherDot = m_board.AllDots[m_colunm, m_row +1];
            m_previousRow = m_row;
            m_previousColunm = m_colunm;
            m_otherDot.GetComponent<Dot>().m_row -= 1;
            m_row += 1;
            
        }
        else if ((m_swipeAngle > 135 || m_swipeAngle <= -135) && m_colunm > 0)
        {
            //Mexer para direita
            m_otherDot = m_board.AllDots[m_colunm - 1, m_row];
            m_previousRow = m_row;
            m_previousColunm = m_colunm;
            m_otherDot.GetComponent<Dot>().m_colunm += 1;
            m_colunm -= 1;
            
        }
        else if (m_swipeAngle < -45 && m_swipeAngle >= -135 && m_row > 0)
        {
            //Mexer para baixo
            m_otherDot = m_board.AllDots[m_colunm, m_row-1];
            m_previousRow = m_row;
            m_previousColunm = m_colunm;
            m_otherDot.GetComponent<Dot>().m_row += 1;
            m_row -= 1;
            
        }
        StartCoroutine(CheckMoveCo());
    }

    void MovingDots() 
    {
        if (Mathf.Abs(m_targetX - transform.position.x) > .1)
        {
            //Mover ate o objetivo
            m_tempTargetPos = new Vector2(m_targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, m_tempTargetPos, m_lerpVelocity);
            if (m_board.AllDots[m_colunm, m_row]!= this.gameObject)
            {
                m_board.AllDots[m_colunm, m_row] = this.gameObject;
            }
            m_findMatches.FindAllMatches();
        }else{

            //Coloca a possi��o para a direto
            m_tempTargetPos = new Vector2(m_targetX, transform.position.y);
            transform.position = m_tempTargetPos;
        }
        if (Mathf.Abs(m_targetY - transform.position.y) > .1)
        {
            //Mover ate o objetivo
            m_tempTargetPos = new Vector2(transform.position.x, m_targetY);
            transform.position = Vector2.Lerp(transform.position, m_tempTargetPos, m_lerpVelocity);
            if (m_board.AllDots[m_colunm, m_row] != this.gameObject)
            {
                m_board.AllDots[m_colunm, m_row] = this.gameObject;
            }
            m_findMatches.FindAllMatches();
        }
        else{

            //Coloca a possi��o para a esquerda
            m_tempTargetPos = new Vector2(transform.position.x, m_targetY);
            transform.position = m_tempTargetPos;
        }
    }

    public void MakeRowBomb() 
    {
        m_rowBomb = true;
        GameObject arrow = Instantiate(m_rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }

    public void MakeColumnBomb() 
    {
        m_columnBomb = true;
        GameObject arrow = Instantiate(m_columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }

    public void MakeColorBomb() 
    {
        m_iscolorBomb = true;
        GameObject color = Instantiate(m_colorBomb, transform.position, Quaternion.identity);
        color.transform.parent = this.transform;
        m_board.AllDots[m_colunm, m_row].tag = "Rainbow Bomb";
    }
}
