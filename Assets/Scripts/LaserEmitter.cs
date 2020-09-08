using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class LaserEmitter : MonoBehaviour
{
    public enum LaserDir
    {
        left,
        front,
        right
    }

    public LaserInfo.ColorCode color;
    public LaserDir direction;
    public Laser emitLaser;
    public bool activated = true;

    private LaserDir _prevDir;

    // Start is called before the first frame update
    void Start()
    {
        var ray = Physics2D.Raycast(transform.position, -transform.up, 100, LayerMask.GetMask("Object"));
        var attached = ray.transform.gameObject;
        transform.parent = attached.transform;

        DirectionUpdate();
    }

    private void DirectionUpdate()
    {
        var dir = transform.up;

        switch (direction)
        {
            case LaserDir.left:
                dir -= transform.right;
                break;
            case LaserDir.right:
                dir += transform.right;
                break;
        }

        emitLaser.direction = dir;
        emitLaser.color = color;

        _prevDir = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if(direction != _prevDir) DirectionUpdate();
        if (BlockCheck(transform.position))
        {
            emitLaser.Destroy();
            Destroy(gameObject);
            return;
        }
        if (activated)
        {
            emitLaser.Activate();
            emitLaser.UpdateLaser();
        }
        else
        {
            emitLaser.Deactivate();
        }
    }
    GameObject BlockCheck(Vector3 position)
    {
        Physics2D.queriesStartInColliders = true;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, 0, LayerMask.GetMask("Object"));
        Physics2D.queriesStartInColliders = false;
        
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player")) return null;
            return hit.collider.gameObject;
        }
        return null;
    }
}
