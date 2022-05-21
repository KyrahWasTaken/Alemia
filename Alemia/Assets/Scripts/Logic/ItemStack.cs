[System.Serializable]
public class ItemStack
{
    public Item item;
    public int count;
    public ItemStack(Item i, int c)
        {
            item = i; count = c;
        }
}
