using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
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
    private RectTransform cursorPrefab; // Prefab do cursor

    [SerializeField]
    private RectTransform canvasRectTransform;

    // Variáveis de cooldown
    [SerializeField]
    private float buttonPressCooldown = 0.5f; // meio segundo de cooldown

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        // Adiciona cursores para todos os controles já conectados
        foreach (var playerInput in PlayerInput.all)
        {
            OnPlayerJoined(playerInput);
        }

        // Inscreve-se para eventos de conexão e desconexão de dispositivos
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDestroy()
    {
        // Remove a inscrição dos eventos de conexão e desconexão de dispositivos
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Se um novo controle foi conectado, adiciona um novo cursor
        if (change == InputDeviceChange.Added)
        {
            foreach (var playerCursor in playerCursors)
            {
                foreach (var deviceInPlayerInput in playerCursor.playerInput.devices)
                {
                    if (deviceInPlayerInput == device)
                    {
                        OnPlayerJoined(playerCursor.playerInput);
                        return;
                    }
                }
            }
        }
        // Se um controle foi removido, remove o cursor correspondente
        else if (change == InputDeviceChange.Removed)
        {
            for (int i = 0; i < playerCursors.Count; i++)
            {
                foreach (var deviceInPlayerInput in playerCursors[i].playerInput.devices)
                {
                    if (deviceInPlayerInput == device)
                    {
                        OnPlayerLeft(playerCursors[i]);
                        return;
                    }
                }
            }
        }
    }

   private void OnPlayerJoined(PlayerInput playerInput)
{
    // Instancia um novo cursor
    RectTransform newCursor = Instantiate(cursorPrefab, canvas.transform);

    // Adiciona um novo PlayerCursor à lista
    PlayerCursor newPlayerCursor = new PlayerCursor();
    newPlayerCursor.playerInput = playerInput;
    newPlayerCursor.cursorTransform = newCursor;

    // Cria um mouse virtual e faz a associação com o PlayerInput
    newPlayerCursor.virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
    InputUser.PerformPairingWithDevice(newPlayerCursor.virtualMouse, playerInput.user);

    // Adiciona o novo PlayerCursor à lista de cursores de jogador
    playerCursors.Add(newPlayerCursor);
}


        private void OnPlayerLeft(PlayerCursor playerCursor)
    {
        // Remove o cursor da hierarquia
        Destroy(playerCursor.cursorTransform.gameObject);

        // Remove o mouse virtual associado ao controle
        InputSystem.RemoveDevice(playerCursor.virtualMouse);

        // Remove o PlayerCursor da lista
        playerCursors.Remove(playerCursor);
    }

    private void OnEnable()
    {
        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    private void UpdateMotion()
    {
        foreach (var playerCursor in playerCursors)
        {
            if (Gamepad.all.Count <= playerCursors.IndexOf(playerCursor))
            {
                continue;
            }

            var gamepad = Gamepad.all[playerCursors.IndexOf(playerCursor)];

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

                if (
                    aButtonIsPressed
                    && Time.time >= playerCursor.lastButtonPressTime + buttonPressCooldown
                )
                {
                    playerCursor.lastButtonPressTime = Time.time;
                    // AudioManager.Instance.PlaySFX("ClicarBotao"); //Aciona o aúdio SFX vinculado ao objeto de aúdio sfx nomeado 'ClicarBotao'
                }
            }

            AnchorCursor(playerCursor.cursorTransform, newPosition);
        }
    }

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
}
