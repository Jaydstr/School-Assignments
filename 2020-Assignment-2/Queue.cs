
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hospital_emergency_room
{
    public class CustomQueue<T>         //generic queue class for waititng queues
    {
        private Queue<T> queue;         //queue
            
        public CustomQueue()            //create a customeQueue method
        {
            queue = new Queue<T>();     //creates the queue
        }

        public void Enqueue(T item)     //enque method dates in 1 parameter
        {
            queue.Enqueue(item);        //Adds an item to the back of the queue
        }

        public T Dequeue()              //dequeue method 
        {
            return queue.Dequeue();     //removes item from queue
        }

        public int Count                //returns the size of the queue
        {
            get { return queue.Count; }
        }

    }
}


