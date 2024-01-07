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
        EventManager.Subscribe<OnEntityChangeHeldItemEvent>(PlayerChangedHeldItem);
        UpdatePlayerHealth();
        UpdatePlayerInventory();
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe<OnEntityChangeHeldItemEvent>(PlayerChangedHeldItem);
    }

    private void PlayerChangedHeldItem(OnEntityChangeHeldItemEvent entityChangeHeldItemEvent)
    {
        if (entityChangeHeldItemEvent.Entity.EntityCategory != GameEntityCategory.Player)
            return;

        foreach (var slot in _inventorySlots)
            slot.color = Color.white;

        _inventorySlots[entityChangeHeldItemEvent.Entity.HeldItemInventorySlot].color = Color.red;
    }

    private void UpdatePlayerHealth()
    {
        LivingEntityData playerHealthData = LevelManager.PlayerEntity.EntityData.GetData<LivingEntityData>();
        _healthBarSlider.value = playerHealthData.health / (float)playerHealthData.maxHealth;
    }

    private void UpdatePlayerInventory()
    {
        GameEntity playerEntity = LevelManager.PlayerEntity;


        for(int i = 0; i < 4; i++)
        {
            if (playerEntity.Inventory.MaxSize < i)
                break;

            _inventorySlots[i].color = Color.white;
            _inventorySlots[i].sprite = playerEntity.Inventory.Items[i] != ItemType.NONE
                ? GameDataManager.itemRegistry[playerEntity.Inventory.Items[i]].itemPrefab.GetComponent<SpriteRenderer>().sprite
                : _defaultSprite;
        }

        _inventorySlots[playerEntity.HeldItemInventorySlot].color = Color.red;
    }

}