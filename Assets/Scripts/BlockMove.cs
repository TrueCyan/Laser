using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    public bool movable = true;
    private bool _moving = false; //실제 이동 중 + 이동 사이에 정지해 있는 시간 동안 True 
    private bool _onDelay = false; //이동 사이에 정지해 있는 시간 동안 True
    private Vector2 _moveStack; // 이동 중에 입력된 이동을 저장
    private Vector3 _velocity = Vector3.zero;
    private float _movingTime = 0.1f; // 다음 칸으로 이동하는데 걸리는 시간 (프레임 변화에 대해 일정하도록 초 단위로 정의)
    private float _movingDelay = 0.05f; // 이동 사이에 정지해있는 시간
    private float _movingElapsed = 0; // 지금까지 움직인 시간

    public delegate void MoveFinished();
    public event MoveFinished MoveFinishedEvent;

    public bool IsMoving() { return _moving; }


    // Update is called once per frame
    void Update()
    {
        // 이동 방향으로 이동
        if (_moving && !_onDelay)
        {
            transform.position += _velocity * Time.deltaTime;
            _movingElapsed += Time.deltaTime;
        }

        // 이동을 끝냈을 때
        if (_movingElapsed >= _movingTime)
        {
            _onDelay = true;
            _movingElapsed = 0;
            _velocity = Vector3.zero;

            // 좌표를 .5로 스냅
            Vector3 currentPoint = transform.position;
            currentPoint.x = Mathf.Round(currentPoint.x + 0.5f) - 0.5f;
            currentPoint.y = Mathf.Round(currentPoint.y + 0.5f) - 0.5f;
            transform.position = currentPoint;

            // 이동이 끝난 후 일정 시간동안 정지하기 위해 코루틴 호출
            StartCoroutine(Delay());
        }
    }

    // 일정 시간동안 정지 후 이동 종료 이벤트 발생
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(_movingDelay); // 정해진 시간이 지난 후 아래 코드 실행
        _onDelay = false;
        _moving = false;
        MoveFinishedEvent?.Invoke();

        // 스택에서 꺼내기
        Move(_moveStack);
        _moveStack = Vector2.zero;
    }

    public void Move(Vector2 dir)
    {
        if (_moving) _moveStack = dir;
        else
        {
            // 유효한 방향인지 검사
            if (!(Mathf.Abs(dir.magnitude - 1) < 0.001f
                  && (Mathf.Abs(Mathf.Abs(dir.x) - 1) < 0.001f || Mathf.Abs(Mathf.Abs(dir.y) - 1) < 0.001f))) return;

            _moving = true;
            _velocity = (1 / _movingTime) * dir;
        }
    }
}
