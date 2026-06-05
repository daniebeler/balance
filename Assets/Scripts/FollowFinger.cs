using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityEngine.InputSystem;

public class FollowFinger : MonoBehaviour
{
    private Rigidbody2D rigid;
   private Vector3 targetWorldPos;
    private bool canDrag = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        targetWorldPos = new Vector3(0, -0.8f, 0);
    }

    void Update()
    {
        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            if (!PointerIsOverUIButton(screenPos))
                canDrag = true;
        }

        if (Pointer.current.press.wasReleasedThisFrame)
        {
            canDrag = false;
        }

        if (canDrag && Pointer.current.press.isPressed)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
            targetWorldPos = new Vector3(world.x, world.y, 0);
        }

        transform.position = targetWorldPos;
    }

    private bool PointerIsOverUIButton(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var r in results)
        {
            if (r.gameObject.GetComponent<Button>() != null)
                return true;
        }

        return false;
    }
}
