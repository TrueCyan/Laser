using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class BlockColor : MonoBehaviour
{
    public Laser.ColorCode hasColor;

#if UNITY_EDITOR
    private SpriteRenderer _sr;

    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var realColor = new Color(hasColor.HasFlag(Laser.ColorCode.Red) ? 1 : 0.2f,
            hasColor.HasFlag(Laser.ColorCode.Green) ? 1 : 0.2f,
            hasColor.HasFlag(Laser.ColorCode.Blue) ? 1 : 0.2f);
        _sr.color = realColor;
    }
#endif
}
