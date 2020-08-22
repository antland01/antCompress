using System;
using System.Collections;
using System.Collections.Generic;

namespace antCompress
{
    public interface iHuffman
    {
       public string[] printHuffTree();
      // private  Dictionary<Char, HuffmanNode> TreeConstruction(Dictionary<Char, HuffmanNode> frequencies);
       public Byte[] compress();
        public string decompress(byte[] fileContent);
       public bool IsLeaf(HuffmanNode node);
       


    }
}
