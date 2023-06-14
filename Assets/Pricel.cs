using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pricel : MonoBehaviour
{
    [SerializeField] private Texture2D _cursor;

    private void Start()
    {
        Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);
    }
}
