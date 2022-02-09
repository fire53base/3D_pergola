﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted_line_custom
{
    public class Dotted_line_custom : MonoBehaviour
    {
        // Inspector fields
        public Sprite Dot_Vertical;

        public Sprite Dot_Horizontal;


        [Range(0.01f, 100f)]
        public float Size=50f;
        [Range(0.1f, 200f)]
        public float Delta=100f;

        //Static Property with backing field
        //private static DottedLine instance;
        //public static DottedLine Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //            instance = FindObjectOfType<DottedLine>();
        //        return instance;
        //    }
        //}

        //Utility fields
        List<Vector3> positions = new List<Vector3>();

        List<GameObject> dots = new List<GameObject>();

        public void DrawDottedLine(Vector3 start, Vector3 end,GameObject dots_parent,string orientation= "vertical")
        {
            Sprite Dot;
            if(orientation=="vertical")
            {
                Dot = Dot_Vertical;
            }
            else
            {
                Dot = Dot_Horizontal;
            }
            //DestroyAllDots();

            Vector3 point = start;
            Vector3 direction = (end - start).normalized;//taking direction to place arrows

            while ((end - start).magnitude > (point - start).magnitude)
            {
                positions.Add(point);
                point += (direction * Delta);
            }

            //Render();
            #region Render
            foreach (var position in positions)
            {
                #region GetOneDot
                var gameObject = new GameObject();
                gameObject.transform.localScale = Vector3.one * Size;
                //gameObject.transform.parent = transform;

                var sr = gameObject.AddComponent<SpriteRenderer>();
                sr.sprite = Dot;
                #endregion
                var g = gameObject;//GetOneDot();
                g.transform.position = position;
                dots.Add(g);

                g.transform.parent = dots_parent.transform;
            }
            #endregion
            positions.Clear();
        }
        // Update is called once per frame
        //void FixedUpdate()
        //{
        //    if (positions.Count > 0)
        //    {
        //        //DestroyAllDots();
        //        positions.Clear();
        //    }

        //}

        //private void DestroyAllDots()
        //{
        //    foreach (var dot in dots)
        //    {
        //        Destroy(dot);
        //    }
        //    dots.Clear();
        //}

        //GameObject GetOneDot()
        //{
        //    var gameObject = new GameObject();
        //    gameObject.transform.localScale = Vector3.one * Size;
        //    gameObject.transform.parent = transform;

        //    var sr = gameObject.AddComponent<SpriteRenderer>();
        //    sr.sprite = Dot;
        //    return gameObject;
        //}


        //private void Render()
        //{
        //    foreach (var position in positions)
        //    {
        //        var g = GetOneDot();
        //        g.transform.position = position;
        //        dots.Add(g);
        //    }
        //}
    }
}
