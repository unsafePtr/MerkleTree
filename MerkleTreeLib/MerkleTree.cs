using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleTreeLib
{
    public class MerkleTree
    {
        private MerkleNode root;
        public byte[] MerkleRoot => root?.Hash;

        public Func<byte[], byte[]> HashFunc { get; }
        public int Height { get; }
        
        public MerkleTree(ICollection<byte[]> collection, Func<byte[], byte[]> hashFunc)
        {
            if(collection == null || collection.Count == 0 || hashFunc == null)
            {
                return;
            }

            HashFunc = hashFunc;
            if (collection.Count == 1)
            {
                using (var enumerator = collection.GetEnumerator())
                {
                    enumerator.MoveNext();
                    root = new MerkleNode(HashFunc(enumerator.Current));
                }

                return;
            }

            Height = (int)Math.Ceiling(Math.Log(collection.Count, 2)) - 1;
            Queue<MerkleNode> leafs = new Queue<MerkleNode>(collection.Select(c => new MerkleNode(HashFunc(c))));
            
            if(IsPowerOfTwo(Height))
            {
                ConstructIdealCase(leafs);
            }
            else
            {
                ConstructNormalCase(leafs);
            }
        }
        
        private void ConstructIdealCase(Queue<MerkleNode> queue)
        {
            while (queue.Count != 1)
            {
                MerkleNode left = queue.Dequeue();
                MerkleNode right = queue.Dequeue();

                MerkleNode node = MerkleNode.Concat(left, right, HashFunc);
                queue.Enqueue(node);
            }

            this.root = queue.Dequeue();
        }

        private void ConstructNormalCase(Queue<MerkleNode> queue)
        {
            int count = queue.Count;
            if (count % 2 == 0)
            {
                int countHalf = count / 2;
                while (count >= countHalf)
                {
                    MerkleNode left = queue.Dequeue();
                    MerkleNode right = queue.Dequeue();

                    MerkleNode node = MerkleNode.Concat(left, right, HashFunc);
                    queue.Enqueue(node);

                    count = count - 2;
                }
            }
            else
            {
                count = count - 1;
                int countHalf = count / 2;
                while (count >= countHalf)
                {
                    MerkleNode left = queue.Dequeue();
                    MerkleNode right = queue.Dequeue();

                    MerkleNode node = MerkleNode.Concat(left, right, HashFunc);
                    queue.Enqueue(node);

                    count = count - 2;
                }

                MerkleNode lastNode = queue.Dequeue();
                MerkleNode selfPairedNode = MerkleNode.Concat(lastNode, lastNode, HashFunc);
                queue.Enqueue(selfPairedNode);
            }

            if (queue.Count == 1)
            {
                this.root = queue.Dequeue();
            }
            else
            {
                ConstructNormalCase(queue);
            }
        }

        private bool IsPowerOfTwo(int x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }
    }
}
