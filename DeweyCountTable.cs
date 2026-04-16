using System;

public class DeweyCountTable
{
    private string[] mKeys;
    private int[] mCounts;
    private bool[] mOccupied;
    private int mCapacity;
    private int mSize;

    public DeweyCountTable(int tableCapacity)
    {
        if (tableCapacity < 1)
            throw new ArgumentOutOfRangeException(nameof(tableCapacity), "capacity must be at least 1");

        mKeys = new string[tableCapacity];
        mCounts = new int[tableCapacity];
        mOccupied = new bool[tableCapacity];
        mCapacity = tableCapacity;
        mSize = 0;
    }

    // Not yet implemented - you must implement it.
    // You must implement your own hash function.
    // Do NOT use string.GetHashCode().
    private int Hash(string key)
    {
        int hash = 0;

        for (int i = 0; i < key.Length; i++)
        {
            hash = (hash * 31 + key[i]) % mCapacity;
        }

        return hash;

    }

    // Not yet implemented - you must implement it.
    public bool Contains(string key)
    {
        int index = Hash(key);
        int startIndex = index;

        while (mOccupied[index])
        {
            if (mKeys[index] == key)
                return true;

            index = (index + 1) % mCapacity;

            if (index == startIndex)
                break;
        }

        return false;
    }

    // Not yet implemented - you must implement it.
    public int GetCount(string key)
    {
        int index = Hash(key);
        int startIndex = index;

        while (mOccupied[index])
        {
            if (mKeys[index] == key)
                return mCounts[index];

            index = (index + 1) % mCapacity;

            if (index == startIndex)
                break;
        }

        return -1;
    }

    // Not yet implemented - you must implement it.
    public void Increment(string key)
    {
        // Recommended behaviour if the table is full and key is not already present:
        // throw new InvalidOperationException(...)
        int index = Hash(key);
        int startIndex = index;

        while (mOccupied[index])
        {
            if (mKeys[index] == key)
            {
                mCounts[index]++;
                return;
            }

            index = (index + 1) % mCapacity;

            if (index == startIndex)
                break;
        }

        if (mSize == mCapacity)
            throw new InvalidOperationException("DeweyCountTable is full.");

        mKeys[index] = key;
        mCounts[index] = 1;
        mOccupied[index] = true;
        mSize++;
    }

    // Not yet implemented - you must implement it.
    // In the completed system, Library.MostBorrowedSubject() should call this method.
    // A correct implementation may scan the table arrays internally.
    public string GetMostFrequentKey()
    {
        if (mSize == 0)
            return null;

        string mostFrequentKey = null;
        int highestCount = -1;

        for (int i = 0; i < mCapacity; i++)
        {
            if (mOccupied[i] && mCounts[i] > highestCount)
            {
                highestCount = mCounts[i];
                mostFrequentKey = mKeys[i];
            }
        }

        return mostFrequentKey;
    }
}
