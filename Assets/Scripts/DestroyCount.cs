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
        GameManager.Instance.player.TurnEndEvent += TurnCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnCount()
    {
        destroyTurn--;
        if (destroyTurn == 0)
        {
            Destroy(gameObject);
        }
    }
}
