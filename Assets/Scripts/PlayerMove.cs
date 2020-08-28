using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BlockMove))]
public class PlayerMove : MonoBehaviour
{
    private BlockMove _blockMove;
    private Vector2 _moveStack;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        _blockMove = GetComponent<BlockMove>();
        _blockMove.MoveFinishedEvent += MoveStackPop; // 이동 종료 이벤트에 MoveStackPop 함수 연결
    }

    // Unity Input System을 통해 이동에 해당하는 키를 누를 때 아래 함수가 자동으로 호출됨.
    public void OnFront() { Move(new Vector2(0, 1)); }
    public void OnBack() { Move(new Vector2(0, -1)); }
    public void OnRight() { Move(new Vector2(1, 0)); }
    public void OnLeft() { Move(new Vector2(-1, 0)); }

    // 이동중에 마지막으로 누른 방향을 하나 저장해두고,
    // 기존 이동이 끝난 뒤 저장해둔 방향으로 이동한다.
    // 이동 종료 이벤트가 발생할 경우 호출됨.
    void MoveStackPop()
    {
        Move(_moveStack);
        _moveStack = Vector2.zero;
    }

    void Move(Vector2 dir)
    {
        // 이미 이동 중인 경우 
        if (_blockMove.IsMoving())
        {
            _moveStack = dir;
            return;
        }

        // 유효한 방향인지 검사
        if (!(Mathf.Abs(dir.magnitude - 1) < 0.001f 
              && (Mathf.Abs(Mathf.Abs(dir.x) - 1) < 0.001f || Mathf.Abs(Mathf.Abs(dir.y) - 1) < 0.001f))) return;

        // 문이 있는지 검사
        Vector3 position = transform.position;
        position += Vector3.ClampMagnitude((Vector3)dir, 0.45f);
        target = BlockCheck(position);
        if (target != null)
        {
            Door _door = target.GetComponent<Door>();
            
            if (_door != null)
            {
                // 문이 닫혀 있으면 이동하지 않음
                if (!_door.IsOpened()) return;

                // 문이 열려 있으면 블록이 막고 있더라도 이동
                if (_door.IsOpened()) { _blockMove.Move(dir); return; }
            }
        }

        // 이동이 가능한지 검사
        position = transform.position;
        while (true)
        {
            // 한 칸 이동
            position += (Vector3)dir;

            // 이동할 위치의 오브젝트와 컴포넌트 찾기
            target = BlockCheck(position);
            if (target == null) break; // 공간이 비어있음
            BlockMove component = target.GetComponent<BlockMove>();

            // 움직일 수 없음
            if (!component.movable) return;
        }

        // 블록 밀면서 이동하기
        position = transform.position;
        while (true)
        {
            // 한 칸 이동
            position += (Vector3)dir;

            // 이동할 위치의 오브젝트와 컴포넌트 찾기
            target = BlockCheck(position);
            if (target == null) break; // 공간이 비어있음
            BlockMove component = target.GetComponent<BlockMove>();

            // 블록 이동시키기
            component.Move(dir);
        }

        // 자신 이동하기
        _blockMove.Move(dir);
    }

    GameObject BlockCheck(Vector3 position)
    {
        Physics2D.queriesStartInColliders = true;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        Physics2D.queriesStartInColliders = false;
        if (hit.collider != null) return hit.collider.gameObject;
        return null;
    }
}
