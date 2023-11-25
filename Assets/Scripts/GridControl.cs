using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridControl : MonoBehaviour
{
  
    RectTransform myrect;
    public RectTransform canvas;
    public DomeDraw2D dome;
    public float topPadding;
    // Start is called before the first frame update
    void Awake()
    {

        
        myrect = GetComponent<RectTransform>();

    }

    private void Start()
    {
        Vector2 pointstart, pointend, height;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Camera.main.WorldToScreenPoint(dome.origin), Camera.main, out pointstart);
        myrect.localPosition = pointstart;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Camera.main.WorldToScreenPoint(dome.points.Last()), Camera.main, out pointend);
        //you should find the max using maxindex. but you are lazy and dont want to write a comparer
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Camera.main.WorldToScreenPoint(dome.points.ElementAt(dome.maxindex)), Camera.main, out height);
        pointend = new Vector2(pointend.x, height.y+ topPadding);
        myrect.sizeDelta = pointend  - pointstart;
    }
    // Update is called once per frame
    void GrabEnd(Vector3 Pos)
    {
        
    }

    //public static int MaxIndex<T>(this IEnumerable<T> source)
    //{
    //    IComparer<T> comparer = Comparer<T>.Default;
    //    using (var iterator = source.GetEnumerator())
    //    {
    //        if (!iterator.MoveNext())
    //        {
    //            throw new InvalidOperationException("Empty sequence");
    //        }
    //        int maxIndex = 0;
    //        T maxElement = iterator.Current;
    //        int index = 0;
    //        while (iterator.MoveNext())
    //        {
    //            index++;
    //            T element = iterator.Current;
    //            if (comparer.Compare(element, maxElement) > 0)
    //            {
    //                maxElement = element;
    //                maxIndex = index;
    //            }
    //        }
    //        return maxIndex;
    //    }
    //}
}
