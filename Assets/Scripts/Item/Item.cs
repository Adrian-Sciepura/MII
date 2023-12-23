public class Item
{
    public readonly DataContainer<ItemData> itemData = new DataContainer<ItemData>();
    public IAttackAction attackAction;
    public IUseAction useAction;
}