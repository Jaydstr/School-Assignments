
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hospital_emergency_room
{
    public interface IPriorityQueue<T> where T : IComparable<T> //given priority queue with the two added peek and removeAt methods at the very end of the class
    {
        void Insert(T item);
        void Remove();
        T Front();
        void MakeEmpty();
        bool Empty();
        int Size();
    }

    public class PriorityQueue<T> : IPriorityQueue<T> where T : IComparable<T>
    {
        private T[] A;          //creates thearray A
        private int capacity;   //variable for capacity
        private int count;      //variable for the count 

        public PriorityQueue()  //create an empty priority queue
        {
            capacity = 3;       //start with a capacity of 3
            A = new T[capacity + 1];
            MakeEmpty();        
        }

        public PriorityQueue(T[] inputArray)
        {
            count = capacity = inputArray.Length;
            A = new T[capacity + 1];

            for (int i = 0; i < capacity; i++)
            {
                A[i + 1] = inputArray[i];
            }

            BuildHeap();
        }

        public void MakeEmpty()         //makes the priority queue empty
        {
            count = 0;
        }

        public bool Empty()             //returns true or false is the priority queue is emppty or not
        {
            return count == 0;
        }

        public int Size()               //return the size of the priority queue
        {
            return count;
        }

        private void DoubleCapacity()   //double the capacity of the priority queue 
        {
            T[] oldA = A;
            capacity = 2 * capacity;
            A = new T[capacity + 1];

            for (int i = 1; i <= count; i++)
            {
                A[i] = oldA[i];
            }
        }

        private void PercolateUp(int i) //moves up in the priority queue
        {
            int child = i, parent;

            while (child > 1)
            {
                parent = child / 2;

                if (A[child].CompareTo(A[parent]) > 0)
                {
                    T item = A[child];
                    A[child] = A[parent];
                    A[parent] = item;
                    child = parent;
                }
                else
                    return;
            }
        }

        public void Insert(T item)  //insert method that inserts into the priority queue
        {
            if (count == capacity)
            {
                DoubleCapacity();   //doubles capacity if the queue is full
            }
            A[++count] = item;
            PercolateUp(count);
        }

        private void PercolateDown(int i)   //moves through the queue down
        {
            int parent = i, child;

            while (2 * parent <= count)
            {
                child = 2 * parent;

                if (child < count)
                    if (A[child + 1].CompareTo(A[child]) > 0)
                        child++;

                if (A[child].CompareTo(A[parent]) > 0)
                {
                    T item = A[child];
                    A[child] = A[parent];
                    A[parent] = item;
                    parent = child;
                }
                else
                    return;
            }
        }

        public void Remove()    //removes from the priority queue
        {
            if (Empty())
                throw new InvalidOperationException("Priority queue is empty");
            else
            {
                A[1] = A[count--];
                PercolateDown(1);
            }
        }

        public T Front()
        {
            if (Empty())
                throw new InvalidOperationException("Priority queue is empty");
            else
                return A[1];
        }

        private void BuildHeap()    //builds the heap 
        {
            for (int i = count / 2; i >= 1; i--)
            {
                PercolateDown(i);
            }
        }

        public void HeapSort(T[] inputArray)    //sorts the heap
        {
            capacity = count = inputArray.Length;

            for (int i = capacity - 1; i >= 0; i++)
            {
                A[i + 1] = inputArray[i];
            }

            BuildHeap();

            for (int i = 0; i < capacity; i++)
            {
                inputArray[i] = Front();
                Remove();
            }
        }

        public T Peek(int i)            //added Peek method takes one parameter
        {
            if (i < 1 || i > count)     //iff the index is out of range throw the expection
                throw new IndexOutOfRangeException("Index is out of range.");

            return A[i];                //return the index 
        }

        public void RemoveAt(int i)     //added RemoveAt method takes one parameter
        {
            if (i < 1 || i > count)     // if the index is out of range throw the exception
                throw new IndexOutOfRangeException("Index is out of range.");

            A[i] = A[count--];          //move through the indexs and remove 
            PercolateDown(i);           
            PercolateUp(i);             
        }
    } 
}




    

