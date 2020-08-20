using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    public bool movable = true;
    bool moving = false;
    Vector3 velocity = Vector3.zero;
    int movingTime = 30; // 다음 칸으로 이동하기 위한 프레임 수
    int movingCount = 0; // 지금까지 움직인 프레임 수

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 이동 방향으로 이동
        if (moving)
        {
            transform.position += velocity;
            movingCount++;
        }

        // 이동을 끝냈을 때
        if (movingCount == movingTime)
        {
            moving = false;
            movingCount = 0;
            velocity = Vector3.zero;

            // 좌표를 정수로 스냅
            Vector3 currentPoint = transform.position;
            currentPoint.x = Mathf.Round(currentPoint.x);
            currentPoint.y = Mathf.Round(currentPoint.y);
            transform.position = currentPoint;
        }
    }

    public void move(int dx, int dy)
    {
        if (!moving)
        {
            moving = true;
            velocity.x = (float)1 / movingTime * dx;
            velocity.y = (float)1 / movingTime * dy;
        }
    }
}
