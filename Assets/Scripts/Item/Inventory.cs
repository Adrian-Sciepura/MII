public class Inventory
{
    private int _itemsCount;
    public int size { get; private set; }
    public Item[] items { get; private set; }

    public Inventory(int size = 1)
    {
        _itemsCount = 0;
        this.size = size;
        items = new Item[size];
    }

    public void Resize(int newSize)
    {
        if (newSize < _itemsCount)
            return;

        Item[] newItems = new Item[newSize];

        int addedCount = 0;

        foreach (Item item in items)
            if (item != null)
                newItems[addedCount++] = item;

        size = newSize;
        items = newItems;
    }

    public void AddItem(Item item, int slot = -1)
    {
        if (_itemsCount >= size)
            return;

        if (slot < 0 || slot > size - 1 || items[slot] != null)
        {
            for (int i = 0; i < size; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    break;
                }
            }
        }
        else
        {
            items[slot] = item;
        }

        _itemsCount++;
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < size; i++)
        {
            if (items[i].Equals(item))
            {
                items[i] = null;
                return;
            }
        }
    }

    public void RemoveItem(int slot)
    {
        if (slot < 0 || slot > size - 1)
            return;

        items[slot] = null;
    }
}