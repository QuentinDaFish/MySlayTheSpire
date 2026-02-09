using UnityEngine;
using UnityEngine.InputSystem;

public class PointerSystem : InsistentSingleton<PointerSystem>
{
    private GameObject lastGameObject;
    private GameObject currentGameObject = null;
    private IPointerEnter currentEnter = null;
    private IPointerExit currentExit = null;
    private IPointerExit lastExit = null;
    private IPointerDown currentDown = null;

    private GameObject pressedGameObject = null;
    private IPointerDrag pressedDrag = null;
    private IPointerUp pressedUp;

    private void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        currentGameObject = null;
        currentEnter = null;
        currentExit = null;
        currentDown = null;

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null)
        {
            currentGameObject = hit.collider.gameObject;
            currentEnter = currentGameObject.GetComponent<IPointerEnter>();
            currentExit = currentGameObject.GetComponent<IPointerExit>();
            currentDown = currentGameObject.GetComponent<IPointerDown>();
        }

        if (currentGameObject != lastGameObject)
        {
            lastExit?.OnPointerExit();
            currentEnter?.OnPointerEnter();

            lastGameObject = currentGameObject;
            lastExit = currentExit;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentDown?.OnPointerDown();

            pressedGameObject = currentGameObject;
            pressedDrag = pressedGameObject != null ? pressedGameObject.GetComponent<IPointerDrag>() : null;
            pressedUp = pressedGameObject != null ? pressedGameObject.GetComponent<IPointerUp>() : null;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            pressedDrag?.OnPointerDrag(worldPos);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (pressedGameObject != null && currentGameObject == pressedGameObject)
            {
                pressedUp?.OnPointerUp();
            }

            pressedGameObject = null;
            pressedDrag = null;
            pressedUp = null;
        }
    }

    public bool Down(int index)
    {
        if (index == 0) return Mouse.current.leftButton.wasPressedThisFrame;
        else if (index == 1) return Mouse.current.rightButton.wasPressedThisFrame;
        return false;
    }
    public bool Pressed(int index)
    {
        if (index == 0) return Mouse.current.leftButton.isPressed;
        else if (index == 1) return Mouse.current.rightButton.isPressed;
        return false;
    }
    public bool Up(int index)
    {
        if (index == 0) return Mouse.current.leftButton.wasReleasedThisFrame;
        else if (index == 1) return Mouse.current.rightButton.wasReleasedThisFrame;
        return false;
    }

    public Vector2 MousePos()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return worldPos;
    }
}