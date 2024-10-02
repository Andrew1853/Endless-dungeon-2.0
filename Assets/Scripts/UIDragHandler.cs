using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIDragHandler : MonoBehaviour
{
    public event Action onDragStarted;
    public event Action onDragging;
    public event Action onDragEnded;

    public static UIDragHandler instance;

    private bool isDragging = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    public GameObject LandedObject { get; private set; }
    void Awake()
    {
        instance = this;
        canvas = GameObject.FindObjectOfType<Canvas>();
        eventSystem = EventSystem.current;
        raycaster = canvas.GetComponent<GraphicRaycaster>();
    }
    void Update()
    {
        if (isDragging)
        {
            // ������� �� �������� ����
            Vector2 rectPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out rectPos);
            rectTransform.anchoredPosition = rectPos;

            onDragging?.Invoke();
            if (Input.GetMouseButtonDown(0))
            {
                EndDragging();
            }
            // ��������� �������������� �� ������� ������ Escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndDragging();
            }
        }
    }
    public void StartDragging(RectTransform rect)
    {
        rectTransform = rect;
        onDragStarted?.Invoke();
    }
    IEnumerator WainEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        isDragging = true;
    }
    public void EndDragging()
    {
        // ��������� ��������������
        isDragging = false;
        RecordTopUIElementUnderCursor();
        onDragEnded?.Invoke();
    }
    private void RecordTopUIElementUnderCursor()
    {
        // ������ ��� �������� ���� �������� ��� ��������
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        raycaster.Raycast(pointerData, raycastResults);

        if (raycastResults.Count > 0)
        {
            // ������� ����� ������� ������� (������ � ������)
            LandedObject = raycastResults[0].gameObject;
            Debug.Log("����� ������� UI ������� ��� ��������: " + LandedObject.name);
        }
        else
        {
            Debug.Log("��� UI ��������� ��� ��������.");
        }
    }
}