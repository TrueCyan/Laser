using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<DestroyCount> woods = new List<DestroyCount>();
    private BlockMove _blockMove;

    // Start is called before the first frame update
    void Start()
    {
        _blockMove = GetComponent<BlockMove>();
        _blockMove.MoveFinishedEvent += TurnCount; // 이동 종료 이벤트에 TurnCount 함수 연결
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnCount()
    {
        foreach(DestroyCount wood in woods)
        {
            if(wood) wood.TurnCount();
        }
    }
}
