using System;
using System.Collections;

namespace antCompress
{
    public class AntCompress
    {
        public AntCompress()
        {
            Console.WriteLine("Unicodetest!");

            FileWrite fileWrite = new FileWrite();

            String testFileName = "Anthony_Smith_CV.docx";
            String decompressResultFileName = "Anthony_Smith_CV2.docx";
            String compressedFileName = "compressedfile.huffant";

            Huffman huffman = new Huffman();
            huffman.setCompressFile(fileWrite.Read(testFileName));

            //huffman.printHuffTree();
            fileWrite.WriteLines(huffman.printHuffTree(), compressedFileName);
            fileWrite.AppendLines(new string[] { "###" }, compressedFileName);
            fileWrite.WriteBytes(huffman.compress(), compressedFileName);

            huffman.setDecompressFile(fileWrite.ReadLines(compressedFileName));

            string result = huffman.decompress(fileWrite.ReadBytes(compressedFileName));
            fileWrite.Write(result, decompressResultFileName);



        }
    }
}
