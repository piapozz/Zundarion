using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera : MonoBehaviour
{
    private GameObject _target;
    private bool _canControl;

    struct CameraSetting
    {
        public Vector3 anchor;
        public Vector3 rotate;
        public float FoV;
    }

    CameraSetting neutral = new CameraSetting
    {
        anchor = new Vector3(0, 0, -10),
        rotate = new Vector3(0, 0, 0),
        FoV = 60,
    };
    CameraSetting parry = new CameraSetting
    {
        anchor = new Vector3(0, 0, -10),
        rotate = new Vector3(0, 0, 20),
        FoV = 100,
    };

    void Start()
    {
        _canControl = true;
    }

    void Update()
    {
        
    }

    void UpdatePos(Vector3 targetPos)
    {

    }

    void SetTarget(GameObject target)
    {
        _target = target;
    }
}
