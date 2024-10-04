using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameDebugController))]
public class DebugPointerController : MonoBehaviour
{
    GameDebugController _commandConsole;
    [SerializeField] Vector2Int _pointerPos;
    [SerializeField] Transform pointerView;
    void Start()
    {
        _commandConsole = GetComponent<GameDebugController>();
        pointerView.position = (Vector2)_pointerPos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _pointerPos.y++;
            _commandConsole.vector2Int = _pointerPos;
            pointerView.position = (Vector2)_pointerPos;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _pointerPos.y--;
            _commandConsole.vector2Int = _pointerPos;
            pointerView.position = (Vector2)_pointerPos;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _pointerPos.x++;
            _commandConsole.vector2Int = _pointerPos;
            pointerView.position = (Vector2)_pointerPos;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _pointerPos.x--;
            _commandConsole.vector2Int = _pointerPos;
            pointerView.position = (Vector2)_pointerPos;
        }
    }
}
