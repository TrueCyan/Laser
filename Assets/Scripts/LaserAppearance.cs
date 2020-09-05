using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LaserEmitter))]
public class LaserAppearance : MonoBehaviour
{
    public Transform laserHead;
    private LaserEmitter _le;
    // Start is called before the first frame update
    void Start()
    {
        _le = GetComponent<LaserEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_le.direction)
        {
            case LaserEmitter.LaserDir.left:
                laserHead.localRotation = Quaternion.Euler(0,0,45);
                break;
            case LaserEmitter.LaserDir.front:
                laserHead.localRotation = Quaternion.identity;
                break;
            case LaserEmitter.LaserDir.right:
                laserHead.localRotation = Quaternion.Euler(0, 0, -45);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
