using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public bool _open = false;
    public SceneAsset nextScene;
    
    public bool IsOpened() { return _open; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 지나가는지 검사
        GameObject target = BlockCheck(transform.position);
        if (target != null)
        {
            PlayerMove playerMove = target.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                // 다음 씬으로 이동
                Initiate.Fade(nextScene.name, Color.black, 1);
            }
        }
    }

    // 0 : 닫음
    // 1 : 엶
    // 2 : 닫혀 있으면 열고 열려 있으면 닫음
    public void Change(int a)
    {
        switch (a)
        {
            case 0: _open = false; break;
            case 1: _open = true; break;
            case 2:
                if (_open) _open = false;
                else _open = true;
                break;
        }
    }

    GameObject BlockCheck(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null) return hit.collider.gameObject;
        return null;
    }
}
