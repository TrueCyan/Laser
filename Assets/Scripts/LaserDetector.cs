using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    public BlockMove _blockMove;
    private BlockColor _blockColor;

    public bool detected = false;

    // Start is called before the first frame update
    void Start()
    {
        var ray = Physics2D.Raycast(transform.position, -transform.up, 100, LayerMask.GetMask("Object"));
        var attached = ray.transform.gameObject;
        transform.parent = attached.transform;

        _blockMove = transform.parent.GetComponent<BlockMove>();
        _blockColor = GetComponent<BlockColor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BlockCheck(transform.position))
        {
            Destroy(gameObject);
            return;
        }
        if (RecievingColor() == _blockColor.hasColor) detected = true;
        else detected = false;
    }

    LaserInfo.ColorCode RecievingColor()
    {
        Vector3 dir = transform.up;
        if (Mathf.RoundToInt(dir.y) == 1) return _blockMove.laserInfo.up;
        if (Mathf.RoundToInt(dir.y) == -1) return _blockMove.laserInfo.down;
        if (Mathf.RoundToInt(dir.x) == -1) return _blockMove.laserInfo.left;
        if (Mathf.RoundToInt(dir.x) == 1) return _blockMove.laserInfo.right;
        Debug.Log("no face");
        return LaserInfo.ColorCode.Black;
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
