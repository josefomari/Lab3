using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {

            //Input image
            var imagePath = args[0];
            //var imagePath = @"C:\Users\josef\Desktop\discproffe.png";
            bool isPng = false;
            bool isBmp = false;

            var png = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }; // PNG
            var bmp = new byte[] { 0x42, 0x4D }; // BMP

            if (!File.Exists(imagePath))
            {
                Console.WriteLine("\nFile not found");
                return;
            }

            var fs = new FileStream(imagePath, FileMode.Open);
            var fileSize = (int)fs.Length;

            var data = new byte[fileSize];

            fs.Read(data, 0, data.Length);
            fs.Close();


            int i = 0;
            if (data[i] == png[i])
            {
                while (data[i] == png[i])
                {
                    i++;
                    if (i == png.Length)
                    {
                        isPng = true;
                        break;
                    }
                }
            }
            else if (data[i] == bmp[i])
            {
                while (data[i] == bmp[i])
                {
                    i++;
                    if (i == bmp.Length)
                    {
                        isBmp = true;
                        break;
                    }
                }
            }
            else if (data[i] != bmp[i] | data[i] != png[i])
            {
                Console.WriteLine("\nThis is not a valid .bmp or .png file!");
            }

            if (isPng == true)
            {
                /*Eftersom png signaturen alltid är lika stor (8bytes) och att width och height alltid finns i IHDR datan (8bytes in i IHDR)
                  så vet jag direkt att width är på index 16-19
                  och att height är på index 20-23
                */

                var widthHexa = data[16].ToString("X2") + data[17].ToString("X2") +
                                data[18].ToString("X2") + data[19].ToString("X2");

                int widthDec = Convert.ToInt32(widthHexa, 16);

                var heightHexa = data[20].ToString("X2") + data[21].ToString("X2") +
                                 data[22].ToString("X2") + data[23].ToString("X2");

                int heightDec = Convert.ToInt32(heightHexa, 16);

                Console.WriteLine($"\nThis is a .png image. Resolution: {widthDec}x{heightDec}\n");

                string chunkLengthHexa = "";
                var chunkType = "";
                var totalByteSize = 0;

                // Datan börjas läsa på index 8 där första chunken kommer
                for (int j = 8; j < data.Length; j++)
                {
                    //Length = 4 första Bytes i chunken
                    chunkLengthHexa = (data[j].ToString("X2") + data[j + 1].ToString("X2") + data[j + 2].ToString("X2") +
                                       data[j + 3].ToString("X2"));

                    //Här får man reda på antalet bytes i datan när man konverterar length från hexa till dec
                    int chunkLengthDec = Convert.ToInt32(chunkLengthHexa, 16);

                    //Type = 4 nästa Bytes i chunken
                    chunkType = data[j + 4].ToString("X2") + data[j + 5].ToString("X2") + data[j + 6].ToString("X2") +
                                data[j + 7].ToString("X2");

                    totalByteSize = (chunkLengthDec);

                    //Använder metoden Hex2Ascii för att få ut chunktypen till ascii
                    Console.WriteLine(@$"Found chunk type'{Hex2Ascii(chunkType)}' with a total data of {totalByteSize} Bytes");

                    /* Eftersom varje chunk har 12 garanterade bytes så sätter jag j = j + datan + 11 eftersom varje loop lägger till en extra siffra också
                       så i slutendan så blir j = j + data + 12 och läser då av nästa chunk direkt i varje loop. */

                    j = j + totalByteSize + 11;
                }



            }
            else if (isBmp == true)
            {
                // Fick googla mig framåt vart width och height datan låg, samt så måste man läsa av hexan baklänges här tydligen

                var widthHexa = data[21].ToString("X2") + data[20].ToString("X2") +
                                data[19].ToString("X2") + data[18].ToString("X2");

                int widthDec = Convert.ToInt32(widthHexa, 16);

                var heightHexa = data[25].ToString("X2") + data[24].ToString("X2") +
                                 data[23].ToString("X2") + data[22].ToString("X2");

                int heightDec = Convert.ToInt32(heightHexa, 16);

                Console.WriteLine($"This is a .bmp image. Resolution: {widthDec}x{heightDec}");

            }

            Console.ReadKey();
        }

        // Metod för att få fram chunkens hexa namn till Ascii
        public static string Hex2Ascii(string hexString)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hs = hexString.Substring(i, 2);
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2),
                    System.Globalization.NumberStyles.HexNumber))));
            }

            return sb.ToString();
        }
    }
}
