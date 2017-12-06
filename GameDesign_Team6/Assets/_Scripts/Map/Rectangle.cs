using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//written by Will Belden Brown
public class Rectangle : ScriptableObject {
    public int x, y, width, height;

    public Rectangle init(int X, int Y, int Width, int Height) {
        x = X;
        y = Y;
        width = Width;
        height = Height;
        return this;
    }
}