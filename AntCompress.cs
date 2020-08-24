using System;
using System.Collections;

namespace antCompress
{
    public class AntCompress
    {
        public AntCompress()
        {
            String testFileName = "Anthony_Smith_CV.docx";
            String decompressResultFileName = "Anthony_Smith_CV2.docx";
            String compressedFileName = "compressedfile.huffant";

            Huffman huffman = new Huffman();

            // Decompress
            huffman.setDecompressFile(fileWrite.ReadLines(compressedFileName));
            fileWrite.Write(huffman.decompress(fileWrite.ReadBytes(compressedFileName)), decompressResultFileName);



        }
    }
}
