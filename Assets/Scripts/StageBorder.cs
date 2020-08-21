#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StageBorder : MonoBehaviour
{
    [Flags]
    public enum Blocked
    {
        None = 0,
        Right = 1,
        RightUp = 1 << 1,
        Up = 1 << 2,
        UpLeft = 1 << 3,
        Left = 1 << 4,
        LeftDown = 1 << 5,
        Down = 1 << 6,
        DownRight = 1 << 7
    }

    public Blocked blockedFaces;
    public Sprite corner;
    public Sprite oneSide; //Original: Bottom Side
    public Sprite twoSideOrthogonal; //Original: Bottom + Right
    public Sprite twoSideParallel; //Original: Bottom + Up
    public Sprite threeSide; //Original: All Side Except Left
    public Sprite fourSide;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateSprite()
    {
        var sr = GetComponent<SpriteRenderer>();

        //Fitting borders against blocked faces (Hard coded)
        //Order: 4 faces --> 3 faces --> 2 faces (orthogonal) --> 2 faces (parallel) --> 1 face --> corner

        if (blockedFaces.HasFlag(Blocked.Right)
            && blockedFaces.HasFlag(Blocked.Up)
            && blockedFaces.HasFlag(Blocked.Left)
            && blockedFaces.HasFlag(Blocked.Down))
        {
            sr.sprite = fourSide;
        }
        else if (blockedFaces.HasFlag(Blocked.Right)
                && blockedFaces.HasFlag(Blocked.Up)
                && blockedFaces.HasFlag(Blocked.Down))
        {
            sr.sprite = threeSide;
            transform.rotation = Quaternion.identity;
            sr.flipX = false;
            sr.flipY = false;
        }
        else if (blockedFaces.HasFlag(Blocked.Right)
                 && blockedFaces.HasFlag(Blocked.Up)
                 && blockedFaces.HasFlag(Blocked.Left))
        {
            sr.sprite = threeSide;
            transform.rotation = Quaternion.identity;
            transform.Rotate(0, 0, 90);
            sr.flipX = false;
            sr.flipY = false;
        }
        else if (blockedFaces.HasFlag(Blocked.Down)
                 && blockedFaces.HasFlag(Blocked.Up)
                 && blockedFaces.HasFlag(Blocked.Left))
        {
            sr.sprite = threeSide;
            transform.rotation = Quaternion.identity;
            sr.flipX = true;
            sr.flipY = false;
        }
        else if (blockedFaces.HasFlag(Blocked.Down)
                 && blockedFaces.HasFlag(Blocked.Right)
                 && blockedFaces.HasFlag(Blocked.Left))
        {
            sr.sprite = threeSide;
            transform.rotation = Quaternion.identity;
            transform.Rotate(0, 0, 90);
            sr.flipX = true;
            sr.flipY = false;
        }
        else if (blockedFaces.HasFlag(Blocked.Right)
                 && blockedFaces.HasFlag(Blocked.Down))
        {
            sr.sprite = twoSideOrthogonal;
            transform.rotation = Quaternion.identity;
            sr.flipX = false;
            sr.flipY = false;
        }
        else if (blockedFaces.HasFlag(Blocked.Right)
                 && blockedFaces.HasFlag(Blocked.Up))
        {
            sr.sprite = twoSideOrthogonal;
            transform.rotation = Quaternion.identity;
            sr.flipX = false;
            sr.flipY = true;
        }
        else if (blockedFaces.HasFlag(Blocked.Left)
                 && blockedFaces.HasFlag(Blocked.Up))
        {
            sr.sprite = twoSideOrthogonal;
            transform.rotation = Quaternion.identity;
            sr.flipX = true;
            sr.flipY = true;
        }
        else if (blockedFaces.HasFlag(Blocked.Left)
                 && blockedFaces.HasFlag(Blocked.Down))
        {
            sr.sprite = twoSideOrthogonal;
            transform.rotation = Quaternion.identity;
            sr.flipX = true;
            sr.flipY = false;
        }
        else if (blockedFaces.HasFlag(Blocked.Up)
                 && blockedFaces.HasFlag(Blocked.Down))
        {
            sr.sprite = twoSideParallel;
            transform.rotation = Quaternion.identity;
        }
        else if (blockedFaces.HasFlag(Blocked.Left)
                 && blockedFaces.HasFlag(Blocked.Right))
        {
            sr.sprite = twoSideParallel;
            transform.rotation = Quaternion.identity;
            transform.Rotate(0,0,90);
        }
        else if (blockedFaces.HasFlag(Blocked.Down))
        {
            sr.sprite = oneSide;
            transform.rotation = Quaternion.identity;
            sr.flipX = false;
            sr.flipY = false;
        }
        else if (blockedFaces.HasFlag(Blocked.Left))
        {
            sr.sprite = oneSide;
            transform.rotation = Quaternion.identity;
            transform.Rotate(0, 0, 90);
            sr.flipX = false;
            sr.flipY = true;
        }
        else if (blockedFaces.HasFlag(Blocked.Up))
        {
            sr.sprite = oneSide;
            transform.rotation = Quaternion.identity;
            sr.flipX = false;
            sr.flipY = true;

        }
        else if (blockedFaces.HasFlag(Blocked.Right))
        {
            sr.sprite = oneSide;
            transform.rotation = Quaternion.identity;
            transform.Rotate(0, 0, 90);
            sr.flipX = false;
            sr.flipY = false;
        }
        else if (blockedFaces.HasFlag(Blocked.DownRight))
        {
            sr.sprite = corner;
            transform.rotation = Quaternion.identity;
            sr.flipX = false;
            sr.flipY = false;
        }
        else if (blockedFaces.HasFlag(Blocked.RightUp))
        {
            sr.sprite = corner;
            transform.rotation = Quaternion.identity;
            sr.flipX = false;
            sr.flipY = true;
        }
        else if (blockedFaces.HasFlag(Blocked.UpLeft))
        {
            sr.sprite = corner;
            transform.rotation = Quaternion.identity;
            sr.flipX = true;
            sr.flipY = true;
        }
        else if (blockedFaces.HasFlag(Blocked.LeftDown))
        {
            sr.sprite = corner;
            transform.rotation = Quaternion.identity;
            sr.flipX = true;
            sr.flipY = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
            Destroy(this);
    }
}
#endif