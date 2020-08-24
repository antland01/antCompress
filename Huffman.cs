using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antCompress
{
    public class Huffman : iHuffman
    {
        string content;
        Dictionary<Char, HuffmanNode> HuffTree;
        HuffmanNode HuffBase;
        Dictionary<Char, HuffmanNode> frequencies;


        public Huffman()
        {

        }

        /// <summary>Generates and returns the Huffman Tree for printing into the file</summary>
        /// <param name="iContent">The content to be written to the file</param>
        /// <returns>String array of each line of the Huffman Tree</returns>
        public string[] getHuffmanTree(string iContent)
        {
            content = iContent;
            frequencies = new Dictionary<char, HuffmanNode>();

            foreach (char c in content)
            {
                if (frequencies.ContainsKey(c))
                {
                    frequencies[c].Frequency = frequencies[c].Frequency + 1;
                }
                else
                {
                    frequencies.Add(c, new HuffmanNode() { Frequency = 1, Symbol = c });
                }
            }

            HuffTree = TreeConstruction(frequencies);
            HuffBase = HuffTree.First().Value;

            List<string> result = new List<string>();
            int[] disctFrequencies = frequencies.Select(f => f.Value.Frequency).Distinct().ToArray();

            if (frequencies.ContainsKey('\n'))
            {
                int newLineFreq = frequencies.Where(i => i.Key == '\n').Select(fr => fr.Value.Frequency).ToList()[0];
                result.Add(newLineFreq + ":lb");
            }

            foreach (int freq in disctFrequencies)
            {
                List<char> sameFreq = frequencies.Where(i => i.Value.Frequency == freq && i.Key != '\n').Select(fr => fr.Key).ToList();
                result.Add(freq + ":" + string.Join("", sameFreq));
            }

            // Adds the Delimiter
            result.Add("###");

            string[] arrayResult = result.ToArray();
            return arrayResult;
        }

        /// <summary>-To Be reworked!--</summary>
        /// <param name="iContent">The ID of the client being returned</param>
        public void setDecompressFile(string[] iContent)
        {
            frequencies = new Dictionary<char, HuffmanNode>();

            int delimiterIndex = Array.IndexOf(iContent, "###");

            foreach (string line in iContent)
            {
                Console.WriteLine(line);
                int frequenciesNo = Convert.ToInt32(line.Split(":")[0]);
                foreach (char symbol in line.Skip(2))
                {
                frequencies.Add(symbol, new HuffmanNode() { Frequency = frequenciesNo, Symbol = symbol });

                }


            }

            HuffTree = TreeConstruction(frequencies);
            HuffBase = HuffTree.First().Value;
        }

        /// <summary>Creates the Huffman Tree for use in Compression and Decompression</summary>
        /// <param name="frequencies">The ID of the client being returned</param>
        /// <returns> The next step of the frequincess</returns>
        private Dictionary<Char, HuffmanNode> TreeConstruction(Dictionary<Char, HuffmanNode> frequencies)
        {
            // Returns the Frequencies as they have been all constructed.
            if (frequencies.Count==1)
            { 

                return frequencies;
            }

            // Sorts frequencies from least likely to most likely
            frequencies = frequencies.OrderBy(k => k.Value.Frequency).ToDictionary(pair => pair.Key, pair => pair.Value);

            // Retrieves the two least likely.
           List<HuffmanNode> toBeMerged = frequencies.Values.Take(2).ToList();

            // Creates the new merged huffman node.
            HuffmanNode newNode = new HuffmanNode() {Symbol = toBeMerged[0].Symbol, Frequency = toBeMerged[0].Frequency+toBeMerged[1].Frequency, Left=toBeMerged[0], Right = toBeMerged[1], };

            // Removes the old ones from the list
            frequencies.Remove(toBeMerged[0].Symbol);
            frequencies.Remove(toBeMerged[1].Symbol);

            // Adds the new node to the list
            frequencies.Add(newNode.Symbol, newNode);

            // Starts the process again.
            return TreeConstruction(frequencies);

        }

        /// <summary>Compressses the file into a Byte array</summary>
        /// <param name="id"></param>
        /// <returns> The byte array of the file</returns>
        public byte[] compress()
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < content.Length; i++)
            {
                List<bool> encodedSymbol = HuffBase.Find(content[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            byte[] bytes = new byte[bits.Length / 8 + (bits.Length % 8 == 0 ? 0 : 1)];
            bits.CopyTo(bytes, 0);

            return bytes;

        }

        /// <summary>Decompresses the file contents provided.</summary>
        /// <param name="fileContent">Byte array of the file to be decompressed</param>
        /// <returns> String of the content of the file</returns>
        public string decompress(byte[] fileContent)
        {
            BitArray bits = new BitArray(fileContent);

            HuffmanNode current = this.HuffBase;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.HuffBase;
                }
            }

            return decoded;

        }

        /// <summary>Detects whether or not the specified Node is the leaf or not</summary>
        /// <param name="node">The Node to be checked</param>
        /// <returns> True if Leaf, False if not</returns>
        public bool IsLeaf(HuffmanNode node)
        {
            return (node.Left == null && node.Right == null);
        }
    }
}
