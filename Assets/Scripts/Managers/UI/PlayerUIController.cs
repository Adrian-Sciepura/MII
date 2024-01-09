using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvas;

    [SerializeField]
    private Slider _healthBarSlider;

    [SerializeField]
    private Image[] _inventorySlots;

    [SerializeField]
    private Sprite _defaultSprite;

    [SerializeField]
    private TextMeshProUGUI _gameTimeText;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        EventManager.Subscribe<OnEntityChangeHeldItemEvent>(PlayerChangedHeldItem);
        EventManager.Subscribe<OnEntityDamageEvent>(OnPlayerDamage);
        EventManager.Subscribe<OnEntityHealEvent>(OnPlayerHeal);
        EventManager.Subscribe<OnEntityPickupItemEvent>(OnPlayerPickupItem);
        EventManager.Subscribe<OnPointsValueChanged>(OnPointsValueChanged);
    }

    private void Update()
    {
        int time = (int)GameManager.GameTime;
        _gameTimeText.text = string.Format("{0:00}:{1:00}", time / 60, time % 60);
    }

    private void Start()
    {
        UpdatePlayerHealth();
        UpdatePlayerInventory();
    }
    
    private void OnPointsValueChanged(OnPointsValueChanged onPointsValueChangedEvent)
    {
        _scoreText.text = GameManager.Points.ToString("0000000");
    }

    private void OnPlayerPickupItem(OnEntityPickupItemEvent entityPickupItemEvent)
    {
        if (entityPickupItemEvent.Entity.EntityCategory != GameEntityCategory.Player)
            return;

        UpdatePlayerInventory();
    }

    private void PlayerChangedHeldItem(OnEntityChangeHeldItemEvent entityChangeHeldItemEvent)
    {
        if (entityChangeHeldItemEvent.Entity.EntityCategory != GameEntityCategory.Player)
            return;

        foreach (var slot in _inventorySlots)
            slot.color = Color.white;

        int currentSlot = entityChangeHeldItemEvent.Entity.HeldItemInventorySlot;

        _inventorySlots[currentSlot].sprite = ItemTypeToSprite(LevelManager.PlayerEntity.Inventory.Items[currentSlot]);
        _inventorySlots[currentSlot].color = Color.red;
    }

    private void OnPlayerDamage(OnEntityDamageEvent entityDamageEvent)
    {
        if (entityDamageEvent.Entity.EntityCategory != GameEntityCategory.Player)
            return;

        UpdatePlayerHealth();
    }

    private void OnPlayerHeal(OnEntityHealEvent entityHealEvent)
    {
        if (entityHealEvent.Entity.EntityCategory != GameEntityCategory.Player)
            return;

        UpdatePlayerHealth();
    }

    private void UpdatePlayerHealth()
    {
        LivingEntityData playerHealthData = LevelManager.PlayerEntity.EntityData.GetData<LivingEntityData>();
        float val = playerHealthData.health / (float)playerHealthData.maxHealth;
        _healthBarSlider.value = val > 0 ? val : 0;
    }

    private void UpdatePlayerInventory()
    {
        GameEntity playerEntity = LevelManager.PlayerEntity;


        for(int i = 0; i < 4; i++)
        {
            if (playerEntity.Inventory.MaxSize < i)
                break;

            _inventorySlots[i].color = Color.white;
            _inventorySlots[i].sprite = ItemTypeToSprite(playerEntity.Inventory.Items[i]);
        }

        _inventorySlots[playerEntity.HeldItemInventorySlot].color = Color.red;
    }

    private Sprite ItemTypeToSprite(ItemType itemType)
    {
        return itemType != ItemType.NONE
                ? GameDataManager.itemRegistry[itemType].itemPrefab.GetComponent<SpriteRenderer>().sprite
                : _defaultSprite;
    }

}