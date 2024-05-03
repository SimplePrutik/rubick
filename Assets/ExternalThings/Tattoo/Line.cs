using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

    private (Dot dot1, Dot dot2) adjustedDots;
    private RectTransform rectTransform => GetComponent<RectTransform>();
    void Update()
    {
        
    }

    public void ChangeVertex(Dot dot)
    {
        if (dot == adjustedDots.dot1)
            adjustedDots.dot1 = dot;
        if (dot == adjustedDots.dot2)
            adjustedDots.dot2 = dot;
    }

    public void SetVertices(Dot dot1, Dot dot2)
    {
        adjustedDots = (dot1, dot2);
        Recalculate();
    }

    public void Recalculate()
    {
        var dot1Position = adjustedDots.dot1.Position;
        var dot2Position = adjustedDots.dot2.Position;
        rectTransform.anchoredPosition = new Vector2(
            (dot1Position.x + dot2Position.x) / 2f,
            (dot1Position.y + dot2Position.y) / 2f);

        rectTransform.sizeDelta = new Vector2
            (Vector2.Distance(dot1Position, dot2Position), rectTransform.sizeDelta.y);

        var _x = Mathf.Abs(dot1Position.x - dot2Position.x);
        var _y = Mathf.Abs(dot1Position.y - dot2Position.y);
        var sign = dot1Position.x >= dot2Position.x ^ dot1Position.y >= dot2Position.y;
        rectTransform.rotation = Quaternion.Euler(0f, 0f,
            (Mathf.Abs(_x) < 0.0001f ? Mathf.Atan(Mathf.Infinity) : Mathf.Atan(_y / _x))
            * Mathf.Rad2Deg * (sign ? -1 : 1));
    }
}
