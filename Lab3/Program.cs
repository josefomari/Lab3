using System;
using System.IO;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //Input image
            var imagePath = args[0];
            bool isPng = false;
            bool isBmp = false;


            var png = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };   // PNG
            var bmp = new byte[] { 0x42, 0x4D }; // BMP

            if (!File.Exists(imagePath))
            {
                Console.WriteLine("File not found");
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
                Console.WriteLine("This is not a valid .bmp or .png file!");

            if (isPng == true)
            {
                var widthHexa = data[16].ToString("X2") + data[17].ToString("X2") +
                                  data[18].ToString("X2") + data[19].ToString("X2");

                int widthDec = Convert.ToInt32(widthHexa, 16);

                var heightHexa = data[20].ToString("X2") + data[21].ToString("X2") +
                                data[22].ToString("X2") + data[23].ToString("X2");

                int heightDec = Convert.ToInt32(heightHexa, 16);

                Console.WriteLine($"This is a .png image. Resolution: {widthDec}x{heightDec}");

            }
            else if (isBmp == true)
            {
                var widthHexa = data[21].ToString("X2") + data[20].ToString("X2") +
                                data[19].ToString("X2") + data[18].ToString("X2");

                int widthDec = Convert.ToInt32(widthHexa, 16);

                var heightHexa = data[25].ToString("X2") + data[24].ToString("X2") +
                                 data[23].ToString("X2") + data[22].ToString("X2");

                int heightDec = Convert.ToInt32(heightHexa, 16);


                Console.WriteLine($"This is a .bmp image. Resolution: {widthDec}x{heightDec}");
                

            }











        }



    }
}
