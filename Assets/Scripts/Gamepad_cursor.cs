using System.Collections.Generic; // Certifique-se de incluir esta linha
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class MultiPlayerCursorController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerCursor
    {
        public PlayerInput playerInput;
        public RectTransform cursorTransform;
        public Mouse virtualMouse;
        public bool previousMouseState;
        public float lastButtonPressTime;
    }

    [SerializeField]
    private List<PlayerCursor> playerCursors = new List<PlayerCursor>();

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private float cursorSpeed = 1000f;

    [SerializeField]
    private RectTransform canvasRectTransform;

    // Variáveis de cooldown
    [SerializeField]
    private float buttonPressCooldown = 0.5f; // meio segundo de cooldown

    private Camera mainCamera;

    private void AnchorCursor(RectTransform cursorTransform, Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out anchoredPosition
        );
        cursorTransform.anchoredPosition = anchoredPosition;
    }

    private void OnEnable()
    {
        mainCamera = Camera.main;

        foreach (var playerCursor in playerCursors)
        {
            if (playerCursor.virtualMouse == null)
            {
                playerCursor.virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (!playerCursor.virtualMouse.added)
            {
                InputSystem.AddDevice(playerCursor.virtualMouse);
            }

            InputUser.PerformPairingWithDevice(playerCursor.virtualMouse, playerCursor.playerInput.user);

            if (playerCursor.cursorTransform != null)
            {
                Vector2 position = playerCursor.cursorTransform.anchoredPosition;
                InputState.Change(playerCursor.virtualMouse.position, position);
            }
        }

        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        foreach (var playerCursor in playerCursors)
        {
            InputSystem.RemoveDevice(playerCursor.virtualMouse);
        }
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    private void UpdateMotion()
    {
        for (int i = 0; i < playerCursors.Count; i++)
        {
            var playerCursor = playerCursors[i];

            // Verifique se há gamepads suficientes
            if (Gamepad.all.Count <= i)
            {
                continue; // Pule este loop se não houver gamepad correspondente
            }

            var gamepad = Gamepad.all[i];

            float horizontalDelta = gamepad.leftStick.ReadValue().x;
            horizontalDelta *= cursorSpeed * Time.deltaTime;
            float verticalDelta = gamepad.leftStick.ReadValue().y;
            verticalDelta *= cursorSpeed * Time.deltaTime;

            Vector2 currentPosition = playerCursor.virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + new Vector2(horizontalDelta, verticalDelta);

            newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
            newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

            InputState.Change(playerCursor.virtualMouse.position, newPosition);
            InputState.Change(playerCursor.virtualMouse.delta, horizontalDelta + verticalDelta);

            bool aButtonIsPressed = gamepad.aButton.IsPressed();
            if (playerCursor.previousMouseState != aButtonIsPressed)
            {
                playerCursor.virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
                InputState.Change(playerCursor.virtualMouse, mouseState);
                playerCursor.previousMouseState = aButtonIsPressed;

                if (aButtonIsPressed && Time.time >= playerCursor.lastButtonPressTime + buttonPressCooldown)
                {
                    playerCursor.lastButtonPressTime = Time.time;
                    // AudioManager.Instance.PlaySFX("ClicarBotao"); //Aciona o aúdio SFX vinculado ao objeto de aúdio sfx nomeado 'ClicarBotao'
                }
            }

            AnchorCursor(playerCursor.cursorTransform, newPosition);
        }
    }
}
