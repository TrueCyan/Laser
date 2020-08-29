using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [Flags]
    public enum ColorCode
    {
        Black = 0,
        Red = 1,
        Green = 1 << 1,
        Blue = 1 << 2
    }

    public Vector2 direction;
    public ColorCode color;
    public Laser dependentLaser;

    [HideInInspector] public GameObject hitObj;
    [HideInInspector] public Vector2 end;
    [HideInInspector] public Vector2 reflectionDir;

    
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
        if (color == ColorCode.Black)
        {
            gameObject.SetActive(false);
        }
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
        var realColor = new Color(color.HasFlag(ColorCode.Red) ? 1 : 0,
            color.HasFlag(ColorCode.Green) ? 1 : 0,
            color.HasFlag(ColorCode.Blue) ? 1 : 0);
        _light.startColor = realColor;
        _light.endColor = realColor;


        var prevReflectionDir = reflectionDir;
        reflectionDir = Vector2.Reflect(direction, ray.normal);

        var prevHitObj = hitObj;
        hitObj = ray.transform.gameObject;
        if (!prevHitObj.Equals(hitObj) || prevReflectionDir != reflectionDir)
        {
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
                var reflectiveColor = hitObj.GetComponent<BlockColor>().hasColor;
                if (reflectiveColor == ~ColorCode.Black) reflectiveColor = ColorCode.Black;
                var reflectiveLaser = CreateLaser(pos, Vector2.Reflect(direction, ray.normal), color & reflectiveColor, this);
                var transmissiveLaser = CreateLaser(pos, direction, ~(color & reflectiveColor) & color, this);
                reflectiveLaser.UpdateLaser();
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
    public Laser CreateLaser(Vector2 pos, Vector2 dir, ColorCode laserColor, Laser dependency)
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
