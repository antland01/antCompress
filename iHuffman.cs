using System;
using System.Collections;
using System.Collections.Generic;

namespace antCompress
{
    public interface iHuffman
    {
       public Byte[] compress(string iContent);
       public string decompress(byte[] fileContent);
       public bool IsLeaf(HuffmanNode node);
    }
}
