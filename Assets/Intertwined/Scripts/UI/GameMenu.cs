using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference inputAction;

    private Canvas _canvas;
    
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        inputAction.action.performed += OnMenu;
    }
    
    private void OnDisable()
    {
        inputAction.action.performed -= OnMenu;
    }

    private void OnMenu(InputAction.CallbackContext context)
    {
        ToggleGameMenu();
    }

    public void ToggleGameMenu()
    {
        _canvas.enabled = !_canvas.enabled;
        Cursor.visible = _canvas.enabled;
        Time.timeScale = _canvas.enabled ? 0 : 1;
    }
}
