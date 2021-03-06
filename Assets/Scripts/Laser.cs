﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{

    
    public Vector2 direction;
    public LaserInfo.ColorCode color;
    public Laser dependentLaser;

    public GameObject hitObj;
    public Vector2 end;
    public Vector2 reflectionDir;
    public Vector2 normalDir;


    private LineRenderer _light;
    private List<Laser> _subLasers = new List<Laser>();


    private void Start()
    {
        Initialize();
    }

    // Start is called before the first frame update
    private void Initialize()
    {
        _light = GetComponent<LineRenderer>();
        if (color == LaserInfo.ColorCode.Black)
        {
            gameObject.SetActive(false);
        }
    }

    private LaserInfo.BlockFace ConvertNormal(Vector2 normal)
    {
        if (normal == Vector2.up) return LaserInfo.BlockFace.Up;
        if (normal == Vector2.down) return LaserInfo.BlockFace.Down;
        if (normal == Vector2.left) return LaserInfo.BlockFace.Left;
        if (normal == Vector2.right) return LaserInfo.BlockFace.Right;
        Debug.LogError(normal);
        return LaserInfo.BlockFace.None;
    }

    // Update is called once per frame
    public void UpdateLaser()
    {
        if (dependentLaser)
        {
            transform.position = dependentLaser.end;
        }
        var pos = transform.position;
        var ray = Physics2D.Raycast(pos, direction, Mathf.Infinity, LayerMask.GetMask("Object"));
        if (ray.transform == null)
        {
            Debug.Log(pos+" / "+direction);
            return;
        }

        
        end = ray.point;

        _light.positionCount = 2;
        _light.SetPositions(new [] {pos, (Vector3)end});
        var realColor = new Color(color.HasFlag(LaserInfo.ColorCode.Red) ? 1 : 0,
            color.HasFlag(LaserInfo.ColorCode.Green) ? 1 : 0,
            color.HasFlag(LaserInfo.ColorCode.Blue) ? 1 : 0);
        _light.startColor = realColor;
        _light.endColor = realColor;

        var prevNormalDir = normalDir;
        normalDir = ray.normal;

        var prevReflectionDir = reflectionDir;
        reflectionDir = Vector2.Reflect(direction, normalDir);

        var prevHitObj = hitObj?hitObj:null;
        hitObj = ray.transform.gameObject;
        //이전에 어떤 오브젝트와도 충돌하지 않은 경우 or 이전과 다른 오브젝트와 충돌한 경우 or 법선 방향이 달라진 경우
        if (prevHitObj == null || !prevHitObj.Equals(hitObj) || prevReflectionDir != reflectionDir)
        {
            if (prevHitObj != null && ConvertNormal(prevNormalDir) != LaserInfo.BlockFace.None)
            {
                var prevBlock = prevHitObj.GetComponent<BlockMove>();
                prevBlock.laserInfo.RemoveLaserInfoOnFace(ConvertNormal(prevNormalDir),color);
            }
            var block = hitObj.GetComponent<BlockMove>();
            block.laserInfo.AddLaserInfoOnFace(ConvertNormal(normalDir), color);

            foreach (var laser in _subLasers)
            {
                laser.Destroy();
            }
            _subLasers.Clear();
            if (hitObj.CompareTag("Mirror"))
            {
                var reflectiveColor = hitObj.GetComponent<BlockColor>().hasColor;
                var laser = CreateLaser(pos, Vector2.Reflect(direction, ray.normal), reflectiveColor & color, this);
                laser.UpdateLaser();
            }
            else if (hitObj.CompareTag("Glass"))
            {
                var transmissiveColor = hitObj.GetComponent<BlockColor>().hasColor;
                var transmissiveLaser = CreateLaser(pos, direction, color & transmissiveColor, this);
                transmissiveLaser.UpdateLaser();
            }
        }
        else
        {
            foreach (var laser in _subLasers)
            {
                laser.UpdateLaser();
            }
        }
    }

    public void Destroy()
    {
        if (hitObj)
        {
            var block = hitObj.GetComponent<BlockMove>();
            block.laserInfo.RemoveLaserInfoOnFace(ConvertNormal(normalDir), color);
        }
        
        foreach (var laser in _subLasers)
        {
            laser.Destroy();
        }
        Destroy(gameObject);
    }
    public void Activate()
    {
        gameObject.SetActive(true);
        if (_subLasers.Count == 0) return;
        foreach (var laser in _subLasers)
        {
            laser.gameObject.SetActive(true);
            laser.Activate();
        }
    }
    public void Deactivate()
    {
        foreach (var laser in _subLasers)
        {
            laser.Deactivate();
            laser.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    public Laser CreateLaser(Vector2 pos, Vector2 dir, LaserInfo.ColorCode laserColor, Laser dependency)
    {
        var prefab = (GameObject)Resources.Load("SystemPrefab/Laser", typeof(GameObject));
        var obj = Instantiate(prefab);
        var laser = obj.GetComponent<Laser>();
        _subLasers.Add(laser);
        obj.transform.position = pos;
        laser.dependentLaser = dependency;
        laser.direction = dir;
        laser.color = laserColor;
        laser.Initialize();
        return laser;
    }
}
