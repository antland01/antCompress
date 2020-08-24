using System;
using System.Collections.Generic;
namespace antCompress
{
    public class HuffmanNode
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }
        public HuffmanNode Right { get; set; }
        public HuffmanNode Left { get; set; }

        public HuffmanNode()
        {
        }

        /// <summary>Recruses through the tree to find the Character</summary>
        /// <param name="symbol">Character to be found</param>
        /// <param name="data">Three being traversed</param>
        /// <returns> The tree</returns>
        public List<bool> Find(char symbol, List<bool> data)
        {
            // Leaf
            if (Right == null && Left == null)
            {
                if (symbol.Equals(this.Symbol))
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Left.Find(symbol, leftPath);
                }

                if (Right != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.Find(symbol, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }

        }
    }
}
