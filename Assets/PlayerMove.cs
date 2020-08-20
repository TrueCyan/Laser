using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public BlockMove blockMove;

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (Input.inputString)
        {
            case "W": case "w": move(0, 1); break;
            case "S": case "s": move(0, -1); break;
            case "A": case "a": move(-1, 0); break;
            case "D": case "d": move(1, 0); break;
        }
    }

    void move(int dx, int dy)
    {
        // 이동이 가능한지 검사
        Vector3 position = transform.position;
        while (true)
        {
            // 한 칸 이동
            position.x += dx; position.y += dy;

            // 이동할 위치의 오브젝트와 컴포넌트 찾기
            target = BlockCheck(position);
            if (target == null) break; // 공간이 비어있음
            BlockMove component = (BlockMove)target.GetComponent("BlockMove");

            // 움직일 수 없음
            if (!component.movable) return;
        }

        // 블록 밀면서 이동하기
        position = transform.position;
        while (true)
        {
            // 한 칸 이동
            position.x += dx; position.y += dy;

            // 이동할 위치의 오브젝트와 컴포넌트 찾기
            target = BlockCheck(position);
            if (target == null) break; // 공간이 비어있음
            BlockMove component = (BlockMove)target.GetComponent("BlockMove");

            // 블록 이동시키기
            component.move(dx, dy);
        }

        // 자신 이동하기
        blockMove.move(dx, dy);
    }

    GameObject BlockCheck(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null) return hit.collider.gameObject;
        else return null;
    }
}
