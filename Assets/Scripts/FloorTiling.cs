#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(PrefabOption))]
public class FloorTiling : MonoBehaviour
{
    public Color evenColor;
    public Color oddColor;
    void TilingRule()
    {
        Vector2 pos = transform.position;
        var isEven = (Mathf.RoundToInt(pos.x + pos.y)%2) == 0;
        var _sr = gameObject.GetComponent<SpriteRenderer>();
        _sr.color = isEven ? evenColor : oddColor;
    }
    // Start is called before the first frame update
    void Start()
    {
        //var _pOption = gameObject.GetComponent<PrefabOption>();
        //_pOption.Placement += TilingRule;
        TilingRule();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
            Destroy(this);
    }
}
#endif