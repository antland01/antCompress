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
        public byte[] compress(string iContent)
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
                result.Add(newLineFreq + "lb");
            }

            if (frequencies.ContainsKey(':'))
            {
                int newLineFreq = frequencies.Where(i => i.Key == ':').Select(fr => fr.Value.Frequency).ToList()[0];
                result.Add(":" + newLineFreq + "DD");
            }


            foreach (int freq in disctFrequencies)
            {
                List<char> sameFreq = frequencies.Where(i => i.Value.Frequency == freq && i.Key != '\n' && i.Key != ':').Select(fr => fr.Key).ToList();
                result.Add(":"+freq + string.Join("", sameFreq));
            }

            // Adds the Delimiter
            result.Add("###");

            string[] arrayResult = result.ToArray();
              List<byte> bytesTest = new List<byte>();

            //for (int i = 0; i < arrayResult.Length; i++)
            //{
            //    Console.WriteLine(arrayResult[i]);
            //    char[] charArray = arrayResult[i].ToCharArray();

            //        List<byte> characters = charArray.Select(s => (byte)s).ToList();
            //        bytesTest.AddRange(characters);

            //}

           bytesTest.AddRange(String.Join("", arrayResult).ToCharArray().Select(s => (byte)s).ToList()); 

            byte[] resultTest = bytesTest.ToArray();


            var str = System.Text.Encoding.Default.GetString(resultTest);
            string res = Encoding.UTF8.GetString(resultTest, 0, resultTest.Length);
            string res2 = Encoding.ASCII.GetString(resultTest, 0, resultTest.Length);
            string res3 = Encoding.Unicode.GetString(resultTest, 0, resultTest.Length);

            

            //  Console.WriteLine(str);
            Console.WriteLine(res);

            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < content.Length; i++)
            {
                List<bool> encodedSymbol = HuffBase.Find(content[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            var oneBit = bits[2];

            byte[] bytes = new byte[bits.Length / 8 + (bits.Length % 8 == 0 ? 0 : 1)];
            bits.CopyTo(bytes, 0);

            var comindedBytes = new List<byte>();
            comindedBytes.AddRange(resultTest);
            comindedBytes.AddRange(bytes);

            string res4 = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            string res5 = Encoding.ASCII.GetString(bytes, 0, bytes.Length); 
            string res6 = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
            string res7 = Encoding.Default.GetString(bytes, 0, bytes.Length);
            string res8 = Encoding.BigEndianUnicode.GetString(bytes, 0, bytes.Length);
            string res9 = Encoding.Unicode.GetString(bytes, 0, bytes.Length);

            return comindedBytes.ToArray();

        }

        /// <summary>Decompresses the file contents provided.</summary>
        /// <param name="fileContent">Byte array of the file to be decompressed</param>
        /// <returns> String of the content of the file</returns>
        public string decompress(byte[] fileContent)
        {
            var str = System.Text.Encoding.Default.GetString(fileContent);

            string[] fileParts = str.Split(new string[] { "###" }, StringSplitOptions.None);

            

            string[] items = fileParts[0].Split(":");


            frequencies = new Dictionary<char, HuffmanNode>();

            foreach (string item in items)
            {

                string digits = new String(item.TakeWhile(Char.IsDigit).ToArray());
                string indiviualItem = item.Replace(digits, "");

      

                if (indiviualItem == "lb")
                {
                    frequencies.Add('\n', new HuffmanNode() { Frequency = Convert.ToInt32(digits), Symbol = '\n' });
                }
                if (indiviualItem == "DD")
                {
                    frequencies.Add(':', new HuffmanNode() { Frequency = Convert.ToInt32(digits), Symbol = ':' });
                }

                if (indiviualItem != "DD" && indiviualItem != "lb")
                {
                    char[] invidualItems = indiviualItem.ToArray();

                    foreach (char symbol in indiviualItem)
                    {

                        frequencies.Add(symbol, new HuffmanNode() { Frequency = Convert.ToInt32(digits), Symbol = symbol });

                    }
                }


            }


            HuffTree = TreeConstruction(frequencies);
            HuffBase = HuffTree.First().Value;


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
