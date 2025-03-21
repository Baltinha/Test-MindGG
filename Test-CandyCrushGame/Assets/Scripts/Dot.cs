using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private Vector2 m_fristPositionTouch;
    private Vector2 m_finalDirectionTouch;
    [SerializeField]private float m_swipeAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log(m_swipeAngle);
    }
}
