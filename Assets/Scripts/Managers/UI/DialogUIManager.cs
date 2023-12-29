using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvas;

    [SerializeField]
    private TextMeshProUGUI _senderNameText;
    
    [SerializeField] 
    private TextMeshProUGUI _messageText;
    
    [SerializeField] 
    private Image _speakerImage;


    private void Start()
    {
        _canvas.SetActive(false);
        GameDataManager.input.Overlay.ContinueDialogue.performed += CloseDialogue;
        EventManager.Instance.Subscribe<OnInteractionItemStartEvent<DialogueInteractionItem>>(DialogueEvent);
    }

    private void OnDestroy()
    {
        GameDataManager.input.Overlay.ContinueDialogue.performed -= CloseDialogue;

        EventManager.Instance.Unsubscribe<OnInteractionItemStartEvent<DialogueInteractionItem>>(DialogueEvent);
    }

    private void DialogueEvent(OnInteractionItemStartEvent<DialogueInteractionItem> dialogueInteraction)
    {
        GameEntity sender;
        if (!LevelManager.spawnedEntities.TryGetValue(dialogueInteraction.data.performerGUID, out sender))
            return;

        GameDataManager.input.Player.Disable();
        GameDataManager.input.Overlay.Enable();
        _canvas.SetActive(true);
        _senderNameText.text = GameDataManager.entityRegistry[sender.entityType].displayName;
        _messageText.text = dialogueInteraction.data.message;
        _speakerImage.sprite = sender.GetComponent<SpriteRenderer>().sprite;
    }

    private void CloseDialogue(InputAction.CallbackContext context)
    {
        if (!_canvas.activeSelf)
            return;

        _canvas.SetActive(false);
        GameDataManager.input.Overlay.Disable();
        GameDataManager.input.Player.Enable();
        EventManager.Instance.Publish(new OnInteractionItemFinishEvent<DialogueInteractionItem>());
    }
}
