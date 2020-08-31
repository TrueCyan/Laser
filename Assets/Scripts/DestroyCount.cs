using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DestroyCount : MonoBehaviour
{
    public int destroyTurn;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 찾아서 연결하기
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        TurnManager _turnManager = _player.GetComponent<TurnManager>();
        DestroyCount _destroyCount = GetComponent<DestroyCount>();
        _turnManager.woods.Add(_destroyCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyTurn == 0)
        {
            Destroy(gameObject);
        }
    }

    public void TurnCount()
    {
        destroyTurn--;
    }
}
