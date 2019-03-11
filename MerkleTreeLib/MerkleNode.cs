using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleTreeLib
{
    internal class MerkleNode
    {
        public MerkleNode Left { get; }
        public MerkleNode Right { get; }
        public byte[] Hash { get; }

        public MerkleNode(byte[] value) : this(value, null, null)
        {

        }

        public MerkleNode(byte[] value, MerkleNode left, MerkleNode right)
        {
            Hash = value;
            Left = left;
            Right = right;
        }

        public static MerkleNode Concat(MerkleNode left, MerkleNode right, Func<byte[], byte[]> hashFunc)
        {
            byte[] buffer = new byte[left.Hash.Length + right.Hash.Length];
            Buffer.BlockCopy(left.Hash, 0, buffer, 0, left.Hash.Length);
            Buffer.BlockCopy(right.Hash, 0, buffer, left.Hash.Length, right.Hash.Length);

            return new MerkleNode(hashFunc(buffer), left, right);
        }
    }
}
