using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(BlockMove))]
public class DestroyCount : MonoBehaviour
{
    public int destroyTurn;
    private int _destroyCount;

    private BlockMove _block;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 찾아서 연결하기
        GameManager.Instance.player.TurnEndEvent += TurnCount;
        _block = gameObject.GetComponent<BlockMove>();
    }

    public void TurnCount()
    {
        if (!_block.laserInfo.IsReceivingLaser())
        {
            _destroyCount = 0;
            return;
        }

        _destroyCount++;
        if (_destroyCount == destroyTurn)
        {
            Destroy(gameObject);
        }
    }
}
