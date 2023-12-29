using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public static class OverlayManager
{
    private class DialogueCanvasInfo
    {
        public GameObject Prefab;
        public GameObject Canvas;
        public TextMeshProUGUI SenderNameText;
        public TextMeshProUGUI MessageText;
        public Image speakerImage;
    }


    private static readonly DialogueCanvasInfo _dialogueCanvasInfo = new DialogueCanvasInfo();
    public static bool IsDialogueBoxShown { get => _dialogueCanvasInfo.Canvas.activeSelf; }

    public static void Setup()
    {
        _dialogueCanvasInfo.Prefab = GameDataManager.prefabRegistry["DialogueCanvas"];

        EventManager.Instance.Subscribe<OnHighPriorityLevelLoadEvent>(SetupOverlayForLevel);
        EventManager.Instance.Subscribe<OnInteractionStartEvent>(SetupOverlayForInteraction);
        EventManager.Instance.Subscribe<OnInteractionFinishEvent>(RestoreDefaultOverlay);
        EventManager.Instance.Subscribe<OnInteractionItemStartEvent<DialogueInteractionItem>>(DialogueEvent);

        GameDataManager.input.Overlay.ContinueDialogue.performed += ContinueDialogue;
    }

    private static void DialogueEvent(OnInteractionItemStartEvent<DialogueInteractionItem> dialogueInteraction)
    {
        GameEntity sender;
        if (!LevelManager.spawnedEntities.TryGetValue(dialogueInteraction.data.performerGUID, out sender))
            return;

        _dialogueCanvasInfo.Canvas.SetActive(true);
        _dialogueCanvasInfo.SenderNameText.text = GameDataManager.entityRegistry[sender.entityType].displayName;
        _dialogueCanvasInfo.MessageText.text = dialogueInteraction.data.message;
        _dialogueCanvasInfo.speakerImage.sprite = sender.GetComponent<SpriteRenderer>().sprite;
    }

    private static void SetupOverlayForLevel(OnHighPriorityLevelLoadEvent levelLoadEvent)
    {
        _dialogueCanvasInfo.Canvas = Object.Instantiate(_dialogueCanvasInfo.Prefab);
        Transform panelTransform = _dialogueCanvasInfo.Canvas.transform.Find("DialoguePanel");

        _dialogueCanvasInfo.MessageText = panelTransform.Find("MessageText").gameObject.GetComponent<TextMeshProUGUI>();
        _dialogueCanvasInfo.SenderNameText = panelTransform.Find("SpeakerNameText").gameObject.GetComponent<TextMeshProUGUI>();
        _dialogueCanvasInfo.speakerImage = _dialogueCanvasInfo.Canvas.transform.Find("SpeakerImage").gameObject.GetComponent<Image>();

        _dialogueCanvasInfo.Canvas.SetActive(false);
    }

    private static void SetupOverlayForInteraction(OnInteractionStartEvent interactionStartEvent)
    {
        GameDataManager.input.Player.Disable();
        GameDataManager.input.Overlay.Enable();
        // TODO: If interaction event has hideOverlay turned on -> turn off every default canvas
    }

    private static void RestoreDefaultOverlay(OnInteractionFinishEvent interactionEndEvent)
    {
        GameDataManager.input.Overlay.Disable();
        GameDataManager.input.Player.Enable();
        // TODO: Turn off every unnecessary canvas. Turn on every default canvas like player stats
    }

    private static void ContinueDialogue(InputAction.CallbackContext context)
    {
        if (!IsDialogueBoxShown)
            return;

        _dialogueCanvasInfo.Canvas.SetActive(false);
        EventManager.Instance.Publish(new OnInteractionItemFinishEvent<DialogueInteractionItem>());
    }


}