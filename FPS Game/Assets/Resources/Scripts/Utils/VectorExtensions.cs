using System;
using UnityEngine;


public enum Directions
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public static class VectorExtensions
{
    public static Directions Direction(this Vector2 v)
    {
        var isVertical = Math.Abs(v.x) < Math.Abs(v.y);
        if (isVertical)
        {
            return v.y > 0 ? Directions.UP : Directions.DOWN;
        }
        else
        {
            return v.x > 0 ? Directions.RIGHT : Directions.LEFT;
        }
    }
}