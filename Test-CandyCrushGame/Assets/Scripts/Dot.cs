using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private int m_colunm;
    [SerializeField] private int m_row;
    [SerializeField] private int m_targetX;
    [SerializeField] private int m_targetY;
    private Board m_board;
    private GameObject m_ontherDot;
    private Vector2 m_fristPositionTouch;
    private Vector2 m_finalDirectionTouch;
    private Vector2 m_tempTargetPos;
    [SerializeField]private float m_swipeAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_board = FindAnyObjectByType<Board>();
        m_targetX = (int)transform.position.x;
        m_targetY = (int)transform.position.y;
        m_row = m_targetY;
        m_colunm = m_targetX;
    }

    // Update is called once per frame
    void Update()
    {
        m_targetX = m_colunm;
        m_targetY = m_row;

        if (Mathf.Abs(m_targetX - transform.position.x) > .1)
        {
            //Mover ate o objetivo
            m_tempTargetPos = new Vector2(m_targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, m_tempTargetPos, .5f);
        } else 
        {
            //Coloca a possição direto
            m_tempTargetPos = new Vector2(m_targetX, transform.position.y);
            transform.position = m_tempTargetPos;
            m_board.AllDots[m_colunm, m_row] = this.gameObject;
        }
        if (Mathf.Abs(m_targetY - transform.position.y) > .1)
        {
            //Mover ate o objetivo
            m_tempTargetPos = new Vector2( transform.position.x, m_targetY);
            transform.position = Vector2.Lerp(transform.position, m_tempTargetPos, .5f);
        }
        else
        {
            //Coloca a possição direto
            m_tempTargetPos = new Vector2(transform.position.x, m_targetY);
            transform.position = m_tempTargetPos;
            m_board.AllDots[m_colunm, m_row] = this.gameObject;
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
        m_swipeAngle = Mathf.Atan2(m_finalDirectionTouch.y - m_fristPositionTouch.y, m_finalDirectionTouch.x - m_fristPositionTouch.x) * 180/ Mathf.PI ;
        //Debug.Log(m_swipeAngle);
        MovePieces();
    }

    void MovePieces() 
    {
        if (m_swipeAngle > -45 && m_swipeAngle <= 45 && m_colunm < m_board.Width)
        {
            //Mexer para esquerda
            m_ontherDot = m_board.AllDots[m_colunm + 1, m_row];
            m_ontherDot.GetComponent<Dot>().m_colunm -= 1;
            m_colunm += 1;
            
        }
        else if(m_swipeAngle > 45 && m_swipeAngle <= 135 && m_row < m_board.Height)
        {
            //Mexer para cima
            m_ontherDot = m_board.AllDots[m_colunm, m_row +1];
            m_ontherDot.GetComponent<Dot>().m_row -= 1;
            m_row += 1;
            
        }
        else if ((m_swipeAngle > 135 || m_swipeAngle <= -135) && m_colunm > 0)
        {
            //Mexer para direita
            m_ontherDot = m_board.AllDots[m_colunm - 1, m_row];
            m_ontherDot.GetComponent<Dot>().m_colunm += 1;
            m_colunm -= 1;
            
        }
        else if (m_swipeAngle < -45 && m_swipeAngle >= -135 && m_row > 0)
        {
            //Mexer para baixo
            m_ontherDot = m_board.AllDots[m_colunm, m_row-1];
            m_ontherDot.GetComponent<Dot>().m_row += 1;
            m_row -= 1;
            
        }
    }
}
