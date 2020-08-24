using System;
using System.Collections;
using System.Collections.Generic;

namespace antCompress
{
    public interface iHuffman
    {
       public string[] getHuffmanTree(string iContent);
       public Byte[] compress();
       public string decompress(byte[] fileContent);
       public bool IsLeaf(HuffmanNode node);
    }
}
