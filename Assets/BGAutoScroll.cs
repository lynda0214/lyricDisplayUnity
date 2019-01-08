using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGAutoScroll : MonoBehaviour
{
    private RawImage bg;
    private float curY;
    public float scrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        bg = this.GetComponent<RawImage>();
        curY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        curY += scrollSpeed;
        if (curY >= 1) 
            curY = 0;
        bg.uvRect = new Rect(0, curY, 1, 1);
    }
}
