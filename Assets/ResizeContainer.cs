using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResizeContainer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(350, Screen.height*0.7f);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
