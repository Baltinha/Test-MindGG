using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    [SerializeField] private int m_colunm, m_row;
    [SerializeField] private int m_previousRow, m_previousColunm;
    [SerializeField] private int m_targetX;
    [SerializeField] private int m_targetY;
    private bool m_mactched;
    private Board m_board;

    [Header("Dot Variables")]
    private GameObject m_otherDot;
    private Vector2 m_fristPositionTouch;
    private Vector2 m_finalDirectionTouch;
    private Vector2 m_tempTargetPos;
    [SerializeField]private float m_lerpVelocity = 0;
    [SerializeField]private float m_swipeAngle = 0;
    private  float m_swipeResist = 1f;
    private int row;

    public bool Mactched { get => m_mactched; }
    public int PreviousRow { get => m_previousRow; }

    public int Row { get => m_row; set => m_row = value; }

    public int PreviousColunm { get => m_previousColunm; }

    public int Colunm { get => m_colunm; set => m_colunm = value; }

    // Start is called before the first frame update
    void Start()
    {
        m_board = FindAnyObjectByType<Board>();
        m_targetX = (int)transform.position.x;
        m_targetY = (int)transform.position.y;
        m_row = m_targetY;
        m_colunm = m_targetX;
        m_previousColunm = m_colunm;
        m_previousRow = m_row;
    }

    // Update is called once per frame
    void Update()
    {
        m_targetX = m_colunm;
        m_targetY = m_row;


        FindMeches();
        if (m_mactched) 
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, 2f);
        }
        MovingDots();

    }

    public IEnumerator CheckMoveCo() 
    {
        yield return new WaitForSeconds(.5f);
        if (m_otherDot != null)
        {
            if (!m_mactched && !m_otherDot.GetComponent<Dot>().m_mactched)
            {
                m_otherDot.GetComponent<Dot>().m_row = m_row;
                m_otherDot.GetComponent<Dot>().m_colunm = m_colunm;
                m_row = m_previousRow;
                m_colunm = m_previousColunm;
            }
                    else 
        {
            m_board.DestroyMetches();
        }
            m_otherDot = null;
        }

    }
    private void OnMouseDown()
    {
        m_fristPositionTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(m_fristPositionTouch);
    }
    private void OnMouseUp()
    {
        m_finalDirectionTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculeteAngle();
    }

    void CalculeteAngle()
    {
        if (MathF.Abs( m_finalDirectionTouch.y -m_fristPositionTouch.y) > m_swipeResist || 
            MathF.Abs(m_finalDirectionTouch.x - m_fristPositionTouch.x) > m_swipeResist)
        {
            m_swipeAngle = Mathf.Atan2(m_finalDirectionTouch.y - m_fristPositionTouch.y, m_finalDirectionTouch.x - m_fristPositionTouch.x) * 180 / Mathf.PI;
            MovePieces();
        }

    }

    void MovePieces() 
    {
        if (m_swipeAngle > -45 && m_swipeAngle <= 45 && m_colunm < m_board.Width-1)
        {
            //Mexer para esquerda
            m_otherDot = m_board.AllDots[m_colunm + 1, m_row];
            m_otherDot.GetComponent<Dot>().m_colunm -= 1;
            m_colunm += 1;
            
        }
        else if(m_swipeAngle > 45 && m_swipeAngle <= 135 && m_row < m_board.Height-1)
        {
            //Mexer para cima
            m_otherDot = m_board.AllDots[m_colunm, m_row +1];
            m_otherDot.GetComponent<Dot>().m_row -= 1;
            m_row += 1;
            
        }
        else if ((m_swipeAngle > 135 || m_swipeAngle <= -135) && m_colunm > 0)
        {
            //Mexer para direita
            m_otherDot = m_board.AllDots[m_colunm - 1, m_row];
            m_otherDot.GetComponent<Dot>().m_colunm += 1;
            m_colunm -= 1;
            
        }
        else if (m_swipeAngle < -45 && m_swipeAngle >= -135 && m_row > 0)
        {
            //Mexer para baixo
            m_otherDot = m_board.AllDots[m_colunm, m_row-1];
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
        }else{

            //Coloca a possição para a direto
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
        }
        else{

            //Coloca a possição para a esquerda
            m_tempTargetPos = new Vector2(transform.position.x, m_targetY);
            transform.position = m_tempTargetPos;
        }
    }

    void FindMeches() 
    {
        if (m_colunm > 0 && m_colunm < m_board.Width -1) 
        {
            GameObject leftDot1 = m_board.AllDots[m_colunm - 1, m_row];
            GameObject rightDot1 = m_board.AllDots[m_colunm + 1, m_row];
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().m_mactched = true;
                    rightDot1.GetComponent<Dot>().m_mactched = true;
                    m_mactched = true;
                }
            }
        }

        if (m_row > 0 && m_row < m_board.Height - 1)
        {
            GameObject upDot1 = m_board.AllDots[m_colunm , m_row + 1];
            GameObject downDot1 = m_board.AllDots[m_colunm, m_row -1];

            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().m_mactched = true;
                    downDot1.GetComponent<Dot>().m_mactched = true;
                    m_mactched = true;
                }
            }
        }
    }
}
