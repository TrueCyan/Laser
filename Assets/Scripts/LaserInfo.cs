using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LaserInfo
{
    [Flags]
    public enum ColorCode
    {
        Black = 0,
        Red = 1,
        Green = 1 << 1,
        Blue = 1 << 2
    }
    public enum BlockFace
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public ColorCode up = ColorCode.Black;
    public ColorCode down = ColorCode.Black;
    public ColorCode left = ColorCode.Black;
    public ColorCode right = ColorCode.Black;

    public bool IsReceivingLaser()
    {
        return up != ColorCode.Black 
               || down != ColorCode.Black 
               || left != ColorCode.Black 
               || right != ColorCode.Black;
    }

    public bool IsReceivingLaserOnFace(BlockFace face)
    {
        if (face == BlockFace.Up)
        {
            return up != ColorCode.Black;
        }
        if (face == BlockFace.Down)
        {
            return down != ColorCode.Black;
        }
        if (face == BlockFace.Left)
        {
            return left != ColorCode.Black;
        }
        if (face == BlockFace.Right)
        {
            return right != ColorCode.Black;
        }

        return false;
    }

    public void SetLaserInfoOnFace(BlockFace face, ColorCode color)
    {
        if (face == BlockFace.Up)
        {
            up = color;
        }
        if (face == BlockFace.Down)
        {
            down = color;
        }
        if (face == BlockFace.Left)
        {
            left = color;
        }
        if (face == BlockFace.Right)
        {
            right = color;
        }
    }

    public void AddLaserInfoOnFace(BlockFace face, ColorCode color)
    {
        if (face == BlockFace.Up)
        {
            up |= color;
        }
        if (face == BlockFace.Down)
        {
            down |= color;
        }
        if (face == BlockFace.Left)
        {
            left |= color;
        }
        if (face == BlockFace.Right)
        {
            right |= color;
        }
    }
    public void RemoveLaserInfoOnFace(BlockFace face, ColorCode color)
    {
        if (face == BlockFace.Up)
        {
            up &= ~color;
        }
        if (face == BlockFace.Down)
        {
            down &= ~color;
        }
        if (face == BlockFace.Left)
        {
            left &= ~color;
        }
        if (face == BlockFace.Right)
        {
            right &= ~color;
        }
    }
}
