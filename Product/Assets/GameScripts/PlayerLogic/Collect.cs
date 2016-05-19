using MG;

public class Collect
{
    private int[] Items = new int[GameDefine.CollectCount];

    public void AddCollectItem(int key, int value)
    {
        if (key < 0 || key > Items.Length)
            return;

        Items[key] += value;
    }
}
