using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public SpriteRenderer sr;

    void Start()
    {
        GridBS grid = new GridBS(3, 3);

        grid[0, 0].isEmpty = false;
        grid[0, 2].isEmpty = false;

        grid[2, 0].isEmpty = false;
        grid[2, 2].isEmpty = false;


        Texture2D tex = new Texture2D(3 * 10, 3 * 10);
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                tex.SetPixel(x, y, !grid[x / 10, y / 10].isEmpty ? Color.clear : Color.black);
            }
        }

        tex.Apply();
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;

        sr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 100f);
    }

}
