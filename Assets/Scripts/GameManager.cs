using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    var obj = Instantiate(Resources.Load<GameObject>("_GameManager/GameManager"));
                    _instance = obj.GetComponent<GameManager>();
                }
            }

            return _instance;
        }
    }


    public PlayerManager player;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if(_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
