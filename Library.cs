using System;

public class Library
{
    private Book[] mBooks;
    private int mBookCount;

    private Borrower[] mBorrowers;
    private int mBorrowerCount;

    private BorrowEvent[] mBorrowEvents;
    private int mBorrowEventCount;

    private DeweyCountTable mSubjectCounts;
    private bool mIsCatalogueSorted;

    public Library(int maxBooks, int maxBorrowers, int maxEvents, int tableCapacity)
    {
        if (maxBooks < 1 || maxBorrowers < 1 || maxEvents < 1 || tableCapacity < 1)
            throw new ArgumentOutOfRangeException(nameof(maxBooks), "all capacities must be at least 1");

        mBooks = new Book[maxBooks];
        mBookCount = 0;

        mBorrowers = new Borrower[maxBorrowers];
        mBorrowerCount = 0;

        mBorrowEvents = new BorrowEvent[maxEvents];
        mBorrowEventCount = 0;

        mSubjectCounts = new DeweyCountTable(tableCapacity);
        mIsCatalogueSorted = false;
    }

    // Starter implementation
    // This implementation is correct. Improve it only if needed to satisfy the assignment specification.
    // The efficiency of this method is not considered during marking
    public bool AddBook(string deweyNumber, string title)
    {
        if (mBookCount >= mBooks.Length)
            return false;

        for (int i = 0; i < mBookCount; i++)
        {
            if (mBooks[i].DeweyNumber == deweyNumber)
                return false;
        }

        mBooks[mBookCount] = new Book(deweyNumber, title);
        mBookCount++;
        mIsCatalogueSorted = false;
        return true;
    }

    // Starter implementation
    // This implementation is correct. Improve it only if needed to satisfy the assignment specification.
    // The efficiency of this method is not considered during marking
    public bool AddBorrower(string studentId, string name)
    {
        if (mBorrowerCount >= mBorrowers.Length)
            return false;

        for (int i = 0; i < mBorrowerCount; i++)
        {
            if (mBorrowers[i].StudentId == studentId)
                return false;
        }

        mBorrowers[mBorrowerCount] = new Borrower(studentId, name);
        mBorrowerCount++;
        return true;
    }

    // Starter implementation
    // This implementation is correct but incomplete with respect to the final assignment requirements.
    // Starter implementation does NOT update mSubjectCounts.
    // In the completed system, you should add the required table update.
    // The efficiency of this method is not considered during marking    
    public bool RecordBorrowEvent(string studentId, string deweyNumber)
    {
        if (mBorrowEventCount >= mBorrowEvents.Length)
            return false;

        bool borrowerExists = false;
        for (int i = 0; i < mBorrowerCount; i++)
        {
            if (mBorrowers[i].StudentId == studentId)
            {
                borrowerExists = true;
                break;
            }
        }

        if (!borrowerExists)
            return false;

        if (!ContainsBook(deweyNumber))
            return false;

        mBorrowEvents[mBorrowEventCount] = new BorrowEvent(studentId, deweyNumber);
        mBorrowEventCount++;
        mSubjectCounts.Increment(deweyNumber);
        return true;
    }

    // Starter implementation
    // This implementation is correct. Improve it only if needed to satisfy the assignment specification.
    // The efficiency of this method is not considered during marking
    public bool ContainsBook(string deweyNumber)
    {
        for (int i = 0; i < mBookCount; i++)
        {
            if (mBooks[i].DeweyNumber == deweyNumber)
                return true;
        }

        return false;
    }

    // Not yet implemented - you must implement it. /// DONE 
    public void SortCatalogue()
    {
        if (mBookCount <= 1)
        {
            mIsCatalogueSorted = true;
            return;
        }

        Book[] temp = new Book[mBookCount];

        MergeSort(
            mBooks,
            0,
            mBookCount - 1,
            temp,
            (a, b) => string.CompareOrdinal(a.DeweyNumber, b.DeweyNumber)
        );
        // for (int i = 0; i < mBookCount; i++)
        // {
        //     Console.WriteLine($"Sorted Book {i}: {mBooks[i].DeweyNumber} - {mBooks[i].Title}");
        // }
        mIsCatalogueSorted = true;
    }

    // Not yet implemented - you must implement it.
    // If the catalogue is not currently marked as sorted (including before the first
    // call to SortCatalogue), this method must throw InvalidOperationException.
    public int GetBookIndex(string deweyNumber)
    {
        if(!mIsCatalogueSorted)
            throw new InvalidOperationException("Catalogue is not sorted");
        else
        {
            return FindBookIndexBinSearch(deweyNumber);
        }
    }

    // Starter implementation
    // This implementation is correct but does not meet the completed-system performance requirement.
    public string FindBorrowLimitViolator(string deweyNumber)
    {
        if (!ContainsBook(deweyNumber))
            return null;

        string[] matchingIds = new string[mBorrowEventCount];
        int matchCount = 0;

        for (int i = 0; i < mBorrowEventCount; i++)
        {
            if (mBorrowEvents[i].DeweyNumber == deweyNumber)
            {
                matchingIds[matchCount] = mBorrowEvents[i].StudentId;
                matchCount++;
            }
        }

        if (matchCount == 0)
            return null;

        string[] temp = new string[matchCount];

        MergeSort(
            matchingIds,
            0,
            matchCount - 1,
            temp,
            (a, b) => string.CompareOrdinal(a, b)
        );

        int currentCount = 1;

        for (int i = 1; i < matchCount; i++)
        {
            if (matchingIds[i] == matchingIds[i - 1])
            {
                currentCount++;
                if (currentCount >= 3)
                    return matchingIds[i];
            }
            else
            {
                currentCount = 1;
            }
        }

        return null;
    }

    // Starter implementation
    // This implementation is correct but does not meet the completed-system requirement below.
    // In the completed system, you should replace this with a call to
    // mSubjectCounts.GetMostFrequentKey().
    public string MostBorrowedSubject()
    {
        if (mBorrowEventCount == 0)
            return null;

        return mSubjectCounts.GetMostFrequentKey();
    }

    // Optional private helper methods may be added below this line.
    // Optional private helper fields may also be added below this line.
    // Do not add any new public methods.
    int FindBookIndexBinSearch(string deweyNumber)
    {
        int left = 0;
        int right = mBookCount - 1;

        while (left <= right)
        {
            int mid = (right + left)/ 2;

            if (mBooks[mid].DeweyNumber == deweyNumber)
                return mid;
            else if (string.CompareOrdinal(mBooks[mid].DeweyNumber, deweyNumber) < 0)
                left = mid + 1;
            else
                right = mid - 1;
        }

        return -1; 
    }

    private void MergeSort<T>(T[] arr, int left, int right, T[] temp, Comparison<T> compare)
    {
        if (left >= right)
            return;

        int mid = (left + right) / 2;

        MergeSort(arr, left, mid, temp, compare);
        MergeSort(arr, mid + 1, right, temp, compare);
        Merge(arr, left, mid, right, temp, compare);
    }

    private void Merge<T>(T[] arr, int left, int mid, int right, T[] temp, Comparison<T> compare)
    {
        int i = left;
        int j = mid + 1;
        int k = left;

        while (i <= mid && j <= right)
        {
            if (compare(arr[i], arr[j]) <= 0)
            {
                temp[k++] = arr[i++];
            }
            else
            {
                temp[k++] = arr[j++];
            }
        }

        while (i <= mid)
            temp[k++] = arr[i++];

        while (j <= right)
            temp[k++] = arr[j++];

        for (int idx = left; idx <= right; idx++)
            arr[idx] = temp[idx];
    }
}
