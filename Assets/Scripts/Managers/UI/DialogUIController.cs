using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvas;

    [SerializeField]
    private TextMeshProUGUI _senderNameText;
    
    [SerializeField] 
    private TextMeshProUGUI _messageText;
    
    [SerializeField] 
    private Image _speakerImage;

    private DialogueInteractionItem _currentDialogue;
    private Coroutine _currentCoroutine;


    private void Start()
    {
        _canvas.SetActive(false);
        _currentCoroutine = null;
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
        if (!LevelManager.spawnedEntities.TryGetValue(dialogueInteraction.Data.performerGUID, out sender))
            return;

        GameDataManager.input.Player.Disable();
        GameDataManager.input.Overlay.Enable();

        _currentDialogue = dialogueInteraction.Data;

        _canvas.SetActive(true);
        _senderNameText.text = GameDataManager.entityRegistry[sender.EntityType].displayName;
        _speakerImage.sprite = sender.GetComponent<SpriteRenderer>().sprite;
        _messageText.text = string.Empty;

        if(_currentCoroutine == null)
            _currentCoroutine = StartCoroutine(WriteText());
    }

    private void CloseDialogue(InputAction.CallbackContext context)
    {
        if (!_canvas.activeSelf)
            return;

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _messageText.text = _currentDialogue.message;
            _currentCoroutine = null;
            return;
        }
        
        _canvas.SetActive(false);

        GameDataManager.input.Overlay.Disable();
        GameDataManager.input.Player.Enable();
        EventManager.Instance.Publish(new OnInteractionItemFinishEvent<DialogueInteractionItem>());
    }

    private IEnumerator WriteText()
    {
        int index = 0;
        while(index < _currentDialogue.message.Length) 
        {
            _messageText.text += _currentDialogue.message[index];
            index++;
            yield return new WaitForSeconds(0.1f);
        }

        _currentCoroutine = null;
    }
}
