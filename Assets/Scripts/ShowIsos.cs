using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowIsos : MonoBehaviour
{
    public float value;
    public property type;
    PropertyTrace pt;
    Button btn;
    bool clicked;
    // Start is called before the first frame update
    void Awake()
    {
        btn = GetComponent<Button>();
        pt = FindObjectOfType<PropertyTrace>();
    //    btn.onClick.AddListener(btclicked);
    }

    // Update is called once per frame
    //void btclicked()
    //{
    //    clicked = !clicked;
        
    //    switch (type)
    //    {
    //        case property.pressure:
    //            if (clicked) pt.ShowIsobar(value); else pt.HideIsobar();
    //            break;
    //        case property.temp:
    //            if (clicked) pt.ShowIsotherm(value); else pt.HideIsotherm();
    //                break;
    //        default:
    //            break;
    //    }
    //}
}
