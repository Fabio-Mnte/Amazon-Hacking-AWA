using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private float cursorSpeed = 1000f;

    [SerializeField]
    private RectTransform cursorTransform;

    [SerializeField]
    private RectTransform canvasRectTransform;

    private bool previousMouseState;
    private Mouse virtualMouse;
    private Camera mainCamera;


    private void AnchorCursor(Vector2 position)
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
        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse"); //Crie um mouse virtual caso um já não exista.
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse); //Adiciona um mouse virtual no inputsystem caso nenhum tenha sido adicionado.
        }
        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransform != null) 
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        //playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        float horizontalDelta = Gamepad.current.leftStick.ReadValue().x;
        horizontalDelta *= cursorSpeed * Time.deltaTime;
        float verticalDelta = Gamepad.current.leftStick.ReadValue().y;
        verticalDelta *= cursorSpeed * Time.deltaTime;
        //Pega as movimentações x e y do joystick.

        

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + new Vector2(horizontalDelta, verticalDelta);
        //Atualiza a variável de posição através das movimentações obtidas pelo joystick.

        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, horizontalDelta + verticalDelta);
        //atualiza a posição do cursor de acordo com os valores obtidos acima.

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed(); //verifica se o botão 'a' foi pressionado.
        if (previousMouseState != aButtonIsPressed) 
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
            AudioManager.Instance.PlaySFX("ClicarBotao"); //Aciona o aúdio SFX vinculado ao objeto de aúdio sfx nomeado 'ClicarBotao'
        }

        AnchorCursor(newPosition);
    }

    
}
