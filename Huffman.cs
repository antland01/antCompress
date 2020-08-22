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

        public void setCompressFile(string iContent)
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
        }

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

            //string[] fileContentArray = iContent.Split("###");
            //string[] huffmanTreeArray = fileContentArray[0].Split("#");
            //string fileRaw = fileContentArray[1];


            //foreach (string character in huffmanTreeArray)
            //{
            //    Console.WriteLine(character);
            //    char symbol = character.ToCharArray()[0];
            //    int frequency = Convert.ToInt32(character.Substring(1));
            //    frequencies.Add(symbol, new HuffmanNode() { Frequency = frequency, Symbol = symbol });
            //}

            HuffTree = TreeConstruction(frequencies);
            HuffBase = HuffTree.First().Value;
        }

        public string[] printHuffTree()
        {
            List<string> result = new List<string>();
            int[] disctFrequencies = frequencies.Select(f=> f.Value.Frequency).Distinct().ToArray();

            if (frequencies.ContainsKey('\n'))
            {
                int newLineFreq = frequencies.Where(i => i.Key == '\n').Select(fr => fr.Value.Frequency).ToList()[0];
                result.Add(newLineFreq + ":lb");
            }

            foreach (int freq in disctFrequencies)
            {
               List<char> sameFreq = frequencies.Where(i => i.Value.Frequency == freq && i.Key != '\n').Select(fr => fr.Key).ToList();
                result.Add(freq+":"+string.Join("",sameFreq));
            }

            string[] arrayResult = result.ToArray();

            return arrayResult;
        }

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


        public bool IsLeaf(HuffmanNode node)
        {
            return (node.Left == null && node.Right == null);
        }
    }
}
