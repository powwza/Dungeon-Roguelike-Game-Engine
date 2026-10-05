namespace goodbyeworld{
    public struct Coord(int xin, int yin)
    {
        public int x = xin;
        public int y = yin;
    }
    public class PriorityQueue:Queue
    {
        private int[,] distanceMap;
        public PriorityQueue(int size) : base(size)
        {
        }
        public void Enqueue(Coord coordIn, int[,] distanceMapIn)
        {
            distanceMap=distanceMapIn;
            bool lessThanEqualTo = false;
            int index = front;
            int coordInWeight = distanceMap[coordIn.y,coordIn.x];
            int coordWeight;
            Coord coord;
            Coord temp;
            if (!IsFull())
            {
                rear++;
                while (!lessThanEqualTo&&index<rear) //while the input coord weight is not less than or equal to the current coord weight
                {
                    coord = queue[index];
                    coordWeight = distanceMap[coord.y,coord.x];
                    if (coordWeight > coordInWeight) //if the input coord weight is less than or equal to the current coord weight
                    {
                        lessThanEqualTo=true;
                        coord=queue[index];
                        queue[index]=coordIn;
                        for(int i = index+1; i < rear; i++)
                        {
                            temp=queue[i];
                            queue[i]=coord;
                            coord=temp;
                        }
                        queue[rear]=coord;
                    }
                    else
                    {
                        index++;
                    }
                }
                if (!lessThanEqualTo)
                {
                    queue[rear]=coordIn;
                }
            }
            else
            {
                throw new Exception("Attempted to enqueue to a full queue");
            }
        }
    }
    public class Queue
    {
        protected Coord[] queue;
        protected int front;
        protected int rear;
        public Queue(int size)
        {
            queue = new Coord[size];
            front = 0;
            rear = -1;
        }
        public void Enqueue(Coord coord)
        {
            if (!IsFull())
            {
                rear++;
                queue[rear] = coord;
            }
        }
        public Coord Dequeue()
        {
            if (!IsEmpty())
            {
                front++;
                return queue[front - 1];
            }
            return new Coord(-1, -1);
        }
        public bool IsEmpty()
        {
            if (rear < front)
            {
                return true;
            }
            return false;
        }
        public bool IsFull()
        {
            if (rear >= queue.Length - 1)
            {
                return true;
            }
            return false;
        }
    }
}