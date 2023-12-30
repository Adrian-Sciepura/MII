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

    private void Start()
    {
        EventManager.Instance.Subscribe<OnLevelSetupComplete>(LevelStart);
        EventManager.Instance.Subscribe<OnEntityChangeHeldItemEvent>(PlayerChangedHeldItem);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe<OnLevelSetupComplete>(LevelStart);
        EventManager.Instance.Unsubscribe<OnEntityChangeHeldItemEvent>(PlayerChangedHeldItem);
    }

    private void LevelStart(OnLevelSetupComplete levelSetup)
    {
        UpdatePlayerHealth();
        UpdatePlayerInventory();
    }

    private void PlayerChangedHeldItem(OnEntityChangeHeldItemEvent entityChangeHeldItemEvent)
    {
        if (entityChangeHeldItemEvent.entity.EntityType != GameEntityType.Player)
            return;

        foreach (var slot in _inventorySlots)
            slot.color = Color.white;

        _inventorySlots[entityChangeHeldItemEvent.entity.HeldItemInventorySlot].color = Color.red;
    }

    private void UpdatePlayerHealth()
    {
        LivingEntityData playerHealthData = LevelManager.playerEntity.entityData.GetData<LivingEntityData>();
        _healthBarSlider.value = playerHealthData.health / (float)playerHealthData.maxHealth;
    }

    private void UpdatePlayerInventory()
    {
        GameEntity playerEntity = LevelManager.playerEntity;


        for(int i = 0; i < 4; i++)
        {
            if (playerEntity.inventory.size < i)
                break;

            _inventorySlots[i].color = Color.white;
            _inventorySlots[i].sprite = playerEntity.inventory.items[i] != null
                ? GameDataManager.itemRegistry[playerEntity.inventory.items[i].type].itemPrefab.GetComponent<SpriteRenderer>().sprite
                : _defaultSprite;
        }

        _inventorySlots[playerEntity.HeldItemInventorySlot].color = Color.red;
    }

}