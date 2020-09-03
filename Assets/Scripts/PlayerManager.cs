using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private BlockMove _blockMove;

    public delegate void TurnEnd();
    public event TurnEnd TurnEndEvent;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.player = this;
        _blockMove = GetComponent<BlockMove>();
        _blockMove.MoveFinishedEvent += OnTurnEnd; // 이동 종료 이벤트에 TurnCount 함수 연결
    }

    public void OnTurnEnd()
    {
        TurnEndEvent?.Invoke();
    }
}
