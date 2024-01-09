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

    [SerializeField]
    private Button _answer1Button;

    [SerializeField]
    private GameObject AnswerPanel;

    [SerializeField]
    private Button _answer2Button;

    [SerializeField]
    private TextMeshProUGUI _answer1Text;

    [SerializeField]
    private TextMeshProUGUI _answer2Text;

    private DialogueInteractionItem _currentDialogue;
    private Coroutine _currentCoroutine;

    private AdvancedDialogueInteractionItem _currentAdvancedDialogInteraction = null;

    private void Start()
    {
        _canvas.SetActive(false);
        _currentCoroutine = null;
        GameDataManager.input.Overlay.ContinueDialogue.performed += CloseDialogue;
        _answer1Button.onClick.AddListener(Answer1ButtonClicked);
        _answer2Button.onClick.AddListener(Answer2ButtonClicked);
        EventManager.Subscribe<OnInteractionItemStartEvent<DialogueInteractionItem>>(DialogueEvent);
        EventManager.Subscribe<OnInteractionItemStartEvent<AdvancedDialogueInteractionItem>>(AdvancedDialogueEvent);
    }

    private void OnDestroy()
    {
        GameDataManager.input.Overlay.ContinueDialogue.performed -= CloseDialogue;
        _answer1Button.onClick.RemoveListener(Answer1ButtonClicked);
        _answer2Button.onClick.RemoveListener(Answer2ButtonClicked);
        EventManager.Unsubscribe<OnInteractionItemStartEvent<DialogueInteractionItem>>(DialogueEvent);
        EventManager.Unsubscribe<OnInteractionItemStartEvent<AdvancedDialogueInteractionItem>>(AdvancedDialogueEvent);
    }


    private void AdvancedDialogueEvent(OnInteractionItemStartEvent<AdvancedDialogueInteractionItem> advancedDialogueInteraction)
    {
        GameEntity sender;
        if (!LevelManager.SpawnedEntities.TryGetValue(advancedDialogueInteraction.Data.performer.GUID, out sender) || _currentCoroutine != null)
        {
            EventManager.Publish(new OnInteractionItemFinishEvent<DialogueInteractionItem>());
            return;
        }

        _canvas.SetActive(true);
        AnswerPanel.SetActive(true);
        _currentAdvancedDialogInteraction = advancedDialogueInteraction.Data;
        _senderNameText.text = sender.EntityData.GetData<InteractionEntityData>()?.DisplayName ?? sender.GUID;
        _speakerImage.sprite = sender.GetComponent<SpriteRenderer>().sprite;
        _messageText.text = advancedDialogueInteraction.Data.message;
        _answer1Text.text = advancedDialogueInteraction.Data.answer1.text;
        _answer2Text.text = advancedDialogueInteraction.Data.answer2.text;
    }

    private void Answer1ButtonClicked()
    {
        _canvas.SetActive(false);
        InteractionManager.StartSubInteraction(_currentAdvancedDialogInteraction.answer1.actions);
        EventManager.Publish(new OnInteractionItemFinishEvent<AdvancedDialogueInteractionItem>());
    }

    private void Answer2ButtonClicked()
    {
        _canvas.SetActive(false);
        InteractionManager.StartSubInteraction(_currentAdvancedDialogInteraction.answer2.actions);
        EventManager.Publish(new OnInteractionItemFinishEvent<AdvancedDialogueInteractionItem>());
    }


    private void DialogueEvent(OnInteractionItemStartEvent<DialogueInteractionItem> dialogueInteraction)
    {
        GameEntity sender;
        if (!LevelManager.SpawnedEntities.TryGetValue(dialogueInteraction.Data.performer.GUID, out sender))
        {
            EventManager.Publish(new OnInteractionItemFinishEvent<DialogueInteractionItem>());
            return;
        }

        GameDataManager.input.Overlay.Enable();

        _currentDialogue = dialogueInteraction.Data;

        _canvas.SetActive(true);
        AnswerPanel.SetActive(false);
        _senderNameText.text = sender.EntityData.GetData<InteractionEntityData>()?.DisplayName ?? sender.GUID;
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
        EventManager.Publish(new OnInteractionItemFinishEvent<DialogueInteractionItem>());
    }

    private IEnumerator WriteText()
    {
        int index = 0;
        while (index < _currentDialogue.message.Length) 
        {
            _messageText.text += _currentDialogue.message[index];
            index++;
            yield return new WaitForSeconds(0.05f);
        }

        _currentCoroutine = null;
    }
}
