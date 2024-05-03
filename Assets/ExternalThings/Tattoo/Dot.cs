using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Vector2 Position => GetComponent<RectTransform>().anchoredPosition;
}
