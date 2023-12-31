using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Responsible for moving player by joystick image on the screen
/// </summary>
public class JoyStickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Rigidbody2D playerRigidbody;
    public float movementSpeed = 3f;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private Transform target;
    private bool isHolding;
    private bool isCircle;
    private Action currentInput;
    private RectTransform rect;
    private Transform playerTransform;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        currentInput = Application.isMobilePlatform ? OnMobileInput : OnMouseInput;
        playerTransform = playerRigidbody.transform;
    }

    private void Update()
    {
        currentInput?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isCircle = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isCircle = false;
    }

    private void OnMobileInput()
    {
        Vector2 pos = Input.touchCount == 1 ? Input.GetTouch(0).position : Vector2.zero;
        if (Input.touchCount == 1)
        {
            if (isCircle)
            {
                if (!isHolding)
                {
                    // On pointer down
                    isHolding = true;
                }
                else
                {
                    ApplyMove(pos);
                }
            }
            else
            {
                isHolding = false;
            }
        }
        else if (Input.touchCount == 0 && isHolding)
        {
            StopMove();
            isHolding = false;
        }
    }

    private void OnMouseInput()
    {
        Vector2 pos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0) && isCircle)
        {
            if (isCircle)
            {
                if (!isHolding)
                {
                    // On pointer down
                    isHolding = true;
                }
            }
        }
        if (Input.GetMouseButton(0) && isCircle)
        {
            if (isCircle)
            {
                if (isHolding)
                {
                    ApplyMove(pos);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isHolding)
            {
                StopMove();
                isHolding = false;
            }
        }
    }

    private void ApplyMove(Vector2 pos)
    {
        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
        Vector2 clickPosition = pos - screenCenter;

        Vector2 elementCenterPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        Vector2 circleCenter = elementCenterPosition - screenCenter;

        Vector2 direction = (clickPosition - circleCenter).normalized;

        Vector2 targetVelocity = direction * movementSpeed;

        if (targetVelocity.magnitude > movementSpeed)
        {
            targetVelocity = targetVelocity.normalized * movementSpeed;
        }

        target.position = (Vector2)playerTransform.position + new Vector2(1f, 1f) * direction;

        pointer.anchoredPosition = (targetVelocity.normalized * pointer.rect.width / 2);
        playerRigidbody.velocity = targetVelocity;
    }

    private void StopMove()
    {
        pointer.anchoredPosition = Vector2.zero;
        playerRigidbody.velocity = Vector2.zero;
    }

    private Vector2 GetMousePositionRelativeToCenter()
    {
        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
        Vector2 mousePosition = Input.mousePosition;

        return mousePosition - screenCenter;
    }
}
