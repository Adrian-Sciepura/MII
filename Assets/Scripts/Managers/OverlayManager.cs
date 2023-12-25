using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public static class OverlayManager
{
    private class DialogueCanvasInfo
    {
        public GameObject Prefab;
        public GameObject Canvas;
        public TextMeshProUGUI SenderNameText;
        public TextMeshProUGUI MessageText;
    }


    private static readonly DialogueCanvasInfo _dialogueCanvasInfo = new DialogueCanvasInfo();
    public static bool IsDialogueBoxShown { get => _dialogueCanvasInfo.Canvas.activeSelf; }



    public static void Setup()
    {
        var searchForCanvas = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Canvas" });

        foreach(var asset in searchForCanvas)
        {
            GameObject canvas = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(asset));

            if(canvas != null)
            {
                if(canvas.name == "DialogueCanvas")
                    _dialogueCanvasInfo.Prefab = canvas;
            }
        }

        EventManager.Subscribe<OnHighPriorityLevelLoadEvent>(SetupOverlayForLevel);
        EventManager.Subscribe<OnInteractionStartEvent>(SetupOverlayForInteraction);
        EventManager.Subscribe<OnInteractionEndEvent>(RestoreDefaultOverlay);

        GameDataManager.input.Overlay.ContinueDialogue.performed += ContinueDialogue;
    }

    private static void SetupOverlayForLevel(OnHighPriorityLevelLoadEvent levelLoadEvent)
    {
        _dialogueCanvasInfo.Canvas = Object.Instantiate(_dialogueCanvasInfo.Prefab);
        Transform panelTransform = _dialogueCanvasInfo.Canvas.transform.Find("DialoguePanel");

        _dialogueCanvasInfo.MessageText = panelTransform.Find("MessageText").gameObject.GetComponent<TextMeshProUGUI>();
        _dialogueCanvasInfo.SenderNameText = panelTransform.Find("SpeakerNameText").gameObject.GetComponent<TextMeshProUGUI>();

        _dialogueCanvasInfo.Canvas.SetActive(false);
    }

    private static void SetupOverlayForInteraction(OnInteractionStartEvent interactionStartEvent)
    {
        GameDataManager.input.Player.Disable();
        GameDataManager.input.Overlay.Enable();
        // TODO: If interaction event has hideOverlay turned on -> turn off every default canvas
    }

    private static void RestoreDefaultOverlay(OnInteractionEndEvent interactionEndEvent)
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
    }

    public static void ShowDialogue(string message)
    {
        _dialogueCanvasInfo.Canvas.SetActive(true);
        _dialogueCanvasInfo.MessageText.text = message;
    }


}