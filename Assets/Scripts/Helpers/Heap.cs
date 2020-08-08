using System;

namespace Sweet_And_Salty_Studios
{
    public class Heap<T> where T : IHeapItem<T>
    {
        #region VARIABLES

        private readonly T[] items;

        #endregion VARIABLES

        #region PROPERTIES

        public int Count
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        #endregion CONSTRUCTORS

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void Add(T item)
        {
            item.HeapIndex = Count;
            items[Count] = item;

            SortUp(item);

            Count++;
        }

        public T RemoveFirst()
        {
            var firstItem = items[0];

            Count--;

            items[0] = items[Count];
            items[0].HeapIndex = 0;

            SortDown(items[0]);
            return firstItem;
        }

        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }    

        // REFACTOR?

        private void SortUp(T item)
        {
            var parentIndex = (item.HeapIndex - 1) / 2;

            while(true)
            {
                var parentItem = items[parentIndex];

                if(item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    return;
                }

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        // REFACTOR?

        private void SortDown(T item)
        {
            while(true)
            {
                var childIndexLeft = item.HeapIndex * 2 + 1;
                var childIndexRight = item.HeapIndex * 2 + 2;
                var swapIndex = 0;

                if(childIndexLeft < Count)
                {
                    swapIndex = childIndexLeft;

                    if(childIndexRight < Count)
                    {
                        if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if(item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        private void Swap(T item_A, T item_B)
        {
            items[item_A.HeapIndex] = item_B;
            items[item_B.HeapIndex] = item_A;

            var tempIndex = item_A.HeapIndex;
            item_A.HeapIndex = item_B.HeapIndex;
            item_B.HeapIndex = tempIndex;
        }

        #endregion CUSTOM_FUNCTIONS
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }
}
