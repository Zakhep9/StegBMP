using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace StegoSystem
{
    public partial class Form1 : Form
    {
        private Bitmap bPic = null;
        private BinaryReader bText = null;
        FileStream rText = null;
        struct YCrCb
        {
            public double Y;
            public int Cr;
            public int Cb;
        }
        public Form1()
        {
            InitializeComponent();
        }
        
        private BitArray ByteToBit(byte src)
        {
            BitArray bitArray = new BitArray(8);
            bool st = false;
            for (int i = 0; i < 8; i++)
            {
                if ((src >> i & 1) == 1)
                {
                    st = true;
                }
                else st = false;
                bitArray[i] = st;
            }
            return bitArray;
        }

        private byte BitToByte(BitArray scr)
        {
            byte num = 0;
            for (int i = 0; i < scr.Count; i++)
                if (scr[i] == true)
                    num += (byte)Math.Pow(2, i);
            return num;
        }
        private bool InCrypt(int i, int j, List<byte> bList,int index)
        {
            Color pixelColor;
            BitArray colorArray;
            BitArray messageArray;
            pixelColor = bPic.GetPixel(i, j);
            colorArray = ByteToBit(pixelColor.R);
            messageArray = ByteToBit(bList[index]);

            colorArray[0] = messageArray[0]; //меняем
            byte newR = BitToByte(colorArray);

            colorArray = ByteToBit(pixelColor.G);
            colorArray[0] = messageArray[1]; // в нашем цвете биты
            byte newG = BitToByte(colorArray);

            colorArray = ByteToBit(pixelColor.B);
            colorArray[0] = messageArray[2]; // в нашем цвете биты
            colorArray[1] = messageArray[3]; // в нашем цвете биты
            byte newB = BitToByte(colorArray);

            Color newColor = Color.FromArgb(newR, newG, newB);
            bPic.SetPixel(i, j, newColor);

            pixelColor = bPic.GetPixel(i + 1, j);
            colorArray = ByteToBit(pixelColor.R);
            //messageArray = ByteToBit(bList[index]);

            colorArray[0] = messageArray[4]; //меняем
            newR = BitToByte(colorArray);

            colorArray = ByteToBit(pixelColor.G);
            colorArray[0] = messageArray[5]; // в нашем цвете биты
            newG = BitToByte(colorArray);

            colorArray = ByteToBit(pixelColor.B);
            colorArray[0] = messageArray[6]; // в нашем цвете биты
            colorArray[1] = messageArray[7]; // в нашем цвете биты
            newB = BitToByte(colorArray);

            newColor = Color.FromArgb(newR, newG, newB);
            bPic.SetPixel(i + 1, j, newColor);
            return true;
        }
        private bool InCryptByte(int i, int j, byte message, int index)
        {
            Color pixelColor;
            BitArray colorArray;
            BitArray messageArray;
            pixelColor = bPic.GetPixel(i, j);
            colorArray = ByteToBit(pixelColor.R);
            messageArray = ByteToBit(message);

            colorArray[0] = messageArray[0]; //меняем
            byte newR = BitToByte(colorArray);

            colorArray = ByteToBit(pixelColor.G);
            colorArray[0] = messageArray[1]; // в нашем цвете биты
            byte newG = BitToByte(colorArray);

            colorArray = ByteToBit(pixelColor.B);
            colorArray[0] = messageArray[2]; // в нашем цвете биты
            colorArray[1] = messageArray[3]; // в нашем цвете биты
            byte newB = BitToByte(colorArray);

            Color newColor = Color.FromArgb(newR, newG, newB);
            bPic.SetPixel(i, j, newColor);

            pixelColor = bPic.GetPixel(i + 1, j);
            colorArray = ByteToBit(pixelColor.R);
            //messageArray = ByteToBit(bList[index]);

            colorArray[0] = messageArray[4]; //меняем
            newR = BitToByte(colorArray);

            colorArray = ByteToBit(pixelColor.G);
            colorArray[0] = messageArray[5]; // в нашем цвете биты
            newG = BitToByte(colorArray);

            colorArray = ByteToBit(pixelColor.B);
            colorArray[0] = messageArray[6]; // в нашем цвете биты
            colorArray[1] = messageArray[7]; // в нашем цвете биты
            newB = BitToByte(colorArray);

            newColor = Color.FromArgb(newR, newG, newB);
            bPic.SetPixel(i + 1, j, newColor);
            return true;
        }
        private BitArray OutCrypt(int i, int j) {
            Color pixelColor = bPic.GetPixel(i, j);
            BitArray colorArray = ByteToBit(pixelColor.R);
            BitArray messageArray = ByteToBit(pixelColor.R);
            messageArray[0] = colorArray[0];

            colorArray = ByteToBit(pixelColor.G);
            messageArray[1] = colorArray[0];

            colorArray = ByteToBit(pixelColor.B);
            messageArray[2] = colorArray[0];
            messageArray[3] = colorArray[1];

            pixelColor = bPic.GetPixel(i + 1, j);
            colorArray = ByteToBit(pixelColor.R);
            messageArray[4] = colorArray[0];

            colorArray = ByteToBit(pixelColor.G);
            messageArray[5] = colorArray[0];

            colorArray = ByteToBit(pixelColor.B);
            messageArray[6] = colorArray[0];
            messageArray[7] = colorArray[1];
            return messageArray;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(bPic!=null)
                bPic.Dispose();
            string FilePic;
            //string FileText;
            OpenFileDialog dPic = new OpenFileDialog();
            dPic.Filter = "Файлы изображений (*.bmp)|*.bmp|Все файлы (*.*)|*.*";
            if (dPic.ShowDialog() == DialogResult.OK)
            {
                FilePic = dPic.FileName;
            }
            else
            {
                FilePic = "";
                return;
            }

            FileStream rFile;
            try
            {
                rFile = new FileStream(FilePic, FileMode.Open); //открываем поток
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            textBox1.Text = FilePic;
            textBox5.Text = "";
            bPic = new Bitmap(rFile);

           

            //List<byte> bList = new List<byte>();
            //while (bText.PeekChar() != -1)
            //{ //считали весь текстовый файл для шифрования в лист байт
            //    bList.Add(bText.ReadByte());
            //}
            //int CountText = bList.Count; // в CountText - количество в байтах текста, который нужно закодировать
            //bText.Close();
            rFile.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bText != null)
                bText.Close();
            string FileText;
            OpenFileDialog dText = new OpenFileDialog();
            dText.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (dText.ShowDialog() == DialogResult.OK)
            {
                FileText = dText.FileName;
            }
            else
            {
                FileText = "";
                return;
            }

            //FileStream rText;
            try
            {
                rText = new FileStream(FileText, FileMode.Open); //открываем поток
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            textBox2.Text = FileText;
            textBox4.Text = "";
            
            bText = new BinaryReader(rText, Encoding.ASCII);
            //rText.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "") {
                MessageBox.Show("Пожалуйста, выберите исходное изображение и скрываемое сообщение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Перед применением алгоритма, выберите файл, в который нужно записать пароль.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            List<byte> bList = new List<byte>();
            string FileText;
            OpenFileDialog dText = new OpenFileDialog();
            dText.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (dText.ShowDialog() == DialogResult.OK)
            {
                FileText = dText.FileName;
            }
            else
            {
                FileText = "";
                return;
            }

            FileStream localText;
            try
            {
                localText = new FileStream(FileText, FileMode.Open); //открываем поток
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            while (bText.PeekChar() != -1)
            { //считали весь текстовый файл для шифрования в лист байт
                bList.Add(bText.ReadByte());
            }
            int CountText = bList.Count; // в CountText - количество в байтах текста, который нужно закодировать
            //bText.Close();
            //rFile.Close();
            int HeightOfEmg;
            if (bPic.Height % 2 == 0)
                HeightOfEmg = bPic.Height;
            else
                HeightOfEmg = bPic.Height-1;
            int WidthOfEmg;
            if (bPic.Width % 2 == 0)
                WidthOfEmg = bPic.Width;
            else
                WidthOfEmg = bPic.Width-1;
            //проверяем, поместиться ли исходный текст в картинке
            if (CountText > ((WidthOfEmg * HeightOfEmg)/4))
            {
                MessageBox.Show("Выбранная картинка мала для размещения выбранного текста", "Ошибка", MessageBoxButtons.OK);
                bPic.Dispose();
                rText.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                return;
            }
           
            StreamWriter brW = new StreamWriter(localText, Encoding.GetEncoding(1251));
            
            byte[] StepList = new byte[CountText+1];
            char[] Stp = new char[CountText + 1];
            int[] Steps = new int[CountText + 1];
            string pass = "";
            int keySymbolCount;
            if (radioButton1.Checked)
                keySymbolCount = 5;
            else if (radioButton2.Checked)
                keySymbolCount = CountText / 2;
            else
                keySymbolCount = CountText;
            int index = 0;
            //bool st = false;
            Random rnd = new Random();
            int i = 0, j = 0;
            int rndValue;
            int maxStep = (int)(((WidthOfEmg * HeightOfEmg) / 4)/CountText );
            if (maxStep >= 254)
                maxStep = 254;
            while (index != CountText)
            {
                
                InCrypt(i, j, bList, index);
                rndValue = rnd.Next(2, maxStep);
                if (index < keySymbolCount)
                {
                    
                    brW.Write(rndValue.ToString());
                    pass += rndValue.ToString();
                    if (index != keySymbolCount - 1)
                    {
                        brW.Write("-");
                        pass += "-";
                    }
                    if(index == CountText - 1)
                    {
                        InCryptByte(i, j + 1, 255, index);
                    }
                }
                else
                {
                    if (index == CountText - 1)
                        InCryptByte(i, j + 1, 255, index);
                    else
                        InCryptByte(i, j + 1, (byte)rndValue, index);
                    //Color pixelColor;
                    //BitArray colorArray;
                    //BitArray messageArray;
                    //pixelColor = bPic.GetPixel(i, j + 1);
                    //colorArray = ByteToBit(pixelColor.R);
                    //messageArray = ByteToBit((byte)rndValue);
                    //if (index == CountText - 1)
                    //    messageArray = ByteToBit(255);
                    //colorArray[0] = messageArray[0]; //меняем
                    //byte newR = BitToByte(colorArray);

                    //colorArray = ByteToBit(pixelColor.G);
                    //colorArray[0] = messageArray[1]; // в нашем цвете биты
                    //byte newG = BitToByte(colorArray);

                    //colorArray = ByteToBit(pixelColor.B);
                    //colorArray[0] = messageArray[2]; // в нашем цвете биты
                    //colorArray[1] = messageArray[3]; // в нашем цвете биты
                    //byte newB = BitToByte(colorArray);

                    //Color newColor = Color.FromArgb(newR, newG, newB);
                    //bPic.SetPixel(i, j + 1, newColor);

                    //pixelColor = bPic.GetPixel(i + 1, j + 1);
                    //colorArray = ByteToBit(pixelColor.R);
                    ////messageArray = ByteToBit(bList[index]);

                    //colorArray[0] = messageArray[4]; //меняем
                    //newR = BitToByte(colorArray);

                    //colorArray = ByteToBit(pixelColor.G);
                    //colorArray[0] = messageArray[5]; // в нашем цвете биты
                    //newG = BitToByte(colorArray);

                    //colorArray = ByteToBit(pixelColor.B);
                    //colorArray[0] = messageArray[6]; // в нашем цвете биты
                    //colorArray[1] = messageArray[7]; // в нашем цвете биты
                    //newB = BitToByte(colorArray);

                    //newColor = Color.FromArgb(newR, newG, newB);
                    //bPic.SetPixel(i + 1, j + 1, newColor);
                }
                StepList[index] = Convert.ToByte(rndValue);
                Stp[index] = (char)rndValue;
                i += rndValue;
                
                if (i+1 >= WidthOfEmg || i >= WidthOfEmg)
                {
                    i = (i+1)%WidthOfEmg;
                    j+=2;
                }
                index++;
            }
            
            richTextBox1.Text = pass;
            //string str = new string(StepList);
            //richTextBox1.Text = str;
            //textBox3.Text = char.Parse(Stp);
            
            pictureBox1.Image = bPic;
            MessageBox.Show("Алгоритм втраивания прошел успешно. Сохраните изображение.", "Завершение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            String sFilePic;
            SaveFileDialog dSavePic = new SaveFileDialog();
            dSavePic.Filter = "Файлы изображений (*.bmp)|*.bmp|Все файлы (*.*)|*.*";
            if (dSavePic.ShowDialog() == DialogResult.OK)
            {
                sFilePic = dSavePic.FileName;
            }
            else
            {
                sFilePic = "";
                return;
            };

            FileStream wFile;
            try
            {
                wFile = new FileStream(sFilePic, FileMode.Create); //открываем поток на запись результатов
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла на запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            brW.Close();
            bPic.Save(wFile, System.Drawing.Imaging.ImageFormat.Bmp);
            wFile.Close(); //закрываем поток
            rText.Close();
            localText.Close();
            textBox1.Text = "";
            textBox2.Text = "";
            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string FilePic;
            OpenFileDialog dPic = new OpenFileDialog();
            dPic.Filter = "Файлы изображений (*.bmp)|*.bmp|Файлы изображений (*.jpeg)|*.jpeg|Файлы изображений (*.jpg)|*.jpg|Все файлы (*.*)|*.*";
            if (dPic.ShowDialog() == DialogResult.OK)
            {
                FilePic = dPic.FileName;
            }
            else
            {
                FilePic = "";
                return;
            }

            FileStream rFile;
            try
            {
                rFile = new FileStream(FilePic, FileMode.Open); //открываем поток
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bPic = new Bitmap(rFile);
            rFile.Close();
            textBox3.Text = FilePic;
            if (radioButton4.Checked)
                radioButton5.Enabled = false;
            else
                radioButton4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if ((radioButton4.Checked && richTextBox2.Text == "" && bPic == null) || ( radioButton5.Checked && bPic == null))
            {
                MessageBox.Show("Пожалуйста, выберите исходное изображение и введите стего-ключ.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int countSymbol = richTextBox2.Text.Length; //считали количество зашифрованных символов
            List<byte> message = new List<byte>();
            BitArray oneBit = new BitArray(8);
            string[] pass;
            if (radioButton4.Checked)
                pass = richTextBox2.Text.Split('-');
            else
                pass = richTextBox5.Text.Split('-');
            int[] passNumber = new int[pass.Length];
            int index = pass.Length*2+1;
            byte outSymbol = 0;
            int i = 0, j = 0;
            double[,] Bmatr;
            double[,] DCTmatr;
            int HeightOfImg = bPic.Height;
            
            int WidthOfImg = bPic.Width;
            if (radioButton4.Checked)
            {
                if (bPic.Height % 2 == 0)
                    HeightOfImg = bPic.Height;
                else
                    HeightOfImg = bPic.Height - 1;
                if (bPic.Width % 2 == 0)
                    WidthOfImg = bPic.Width;
                else
                    WidthOfImg = bPic.Width - 1;
            }
            else
            {
                while ((HeightOfImg + 1) % 8 != 0)
                    HeightOfImg--;              
                while ((WidthOfImg + 1) % 8 != 0)
                    WidthOfImg--;
            }
            if (radioButton4.Checked)
            {
                for (int k = 0; k < pass.Length; k++)
                {
                    outSymbol = BitToByte(OutCrypt(i, j + 1));
                    passNumber[k] = int.Parse(pass[k]);
                    message.Add(BitToByte(OutCrypt(i, j)));
                    i += passNumber[k];
                    if (i + 1 >= WidthOfImg || i >= WidthOfImg)
                    {
                        i = (i + 1) % WidthOfImg;
                        j += 2;
                    }
                }
                while (outSymbol != 255 && j < HeightOfImg )
                {
                    message.Add(BitToByte(OutCrypt(i, j)));
                    outSymbol = BitToByte(OutCrypt(i, j + 1));
                    i += outSymbol;
                    if (i + 1 >= WidthOfImg || i >= WidthOfImg)
                    {
                        i = (i + 1) % WidthOfImg;
                        j += 2;
                    }
                    index--;
                    //index++;
                }
            }
            else
            {
                index = 0;
                while (outSymbol != 255 && j < HeightOfImg)
                {
                    if (index != 0 && index % 8 == 0)
                        message.Add(BitToByte(oneBit));
                    Bmatr = BArray(i, j);
                    DCTmatr = DCT(Bmatr);
                    oneBit[index % 8] = KochZhaoOutCrypt(DCTmatr);
                    if (radioButton7.Checked)
                    {
                        passNumber[index] = int.Parse(pass[index]);
                        i += 8*passNumber[index];
                    }
                    else
                        i += 8;
                    if (i + 7 > WidthOfImg || i > WidthOfImg)
                    {
                        i = 0;
                        j += 8;
                    }
                    index++;
                    if (index != 0 && index % 8 == 0)
                        outSymbol = BitToByte(oneBit);
                }
            }
            string strMessage = Encoding.GetEncoding(1251).GetString(message.ToArray());
            richTextBox3.Text = Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding(1251),Encoding.Default, message.ToArray()));
            //richTextBox3.Text = Encoding.Default.GetString(Encoding.GetEncoding(1251).GetBytes(strMessage));
            string sFileText;
            SaveFileDialog dSaveText = new SaveFileDialog();
            dSaveText.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (dSaveText.ShowDialog() == DialogResult.OK)
            {
                sFileText = dSaveText.FileName;
            }
            else
            {
                sFileText = "";
                return;
            };

            FileStream wFile;
            try
            {
                wFile = new FileStream(sFileText, FileMode.Create); //открываем поток на запись результатов
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла на запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            StreamWriter wText = new StreamWriter(wFile, Encoding.GetEncoding(1251));
            wText.Write(strMessage);
            MessageBox.Show("Текст записан в файл", "Информация", MessageBoxButtons.OK);
            wText.Close();
            wFile.Close(); //закрываем поток
            textBox3.Text = "";
            radioButton4.Enabled = true;
            radioButton5.Enabled = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(bText != null)
                bText.Close();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            panel5.Visible = false;
            panel8.Visible = true;
            panel9.Visible = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            panel5.Visible = true;
            panel8.Visible = false;
            panel9.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(bPic!=null)
                bPic.Dispose();
            string FilePic;
            //string FileText;
            OpenFileDialog dPic = new OpenFileDialog();
            dPic.Filter = "Файлы изображений (*.bmp)|*.bmp|Файлы изображений (*.jpeg)|*.jpeg|Файлы изображений (*.jpg)|*.jpg|Все файлы (*.*)|*.*";
            if (dPic.ShowDialog() == DialogResult.OK)
            {
                FilePic = dPic.FileName;
            }
            else
            {
                FilePic = "";
                return;
            }

            FileStream rFile;
            try
            {
                rFile = new FileStream(FilePic, FileMode.Open); //открываем поток
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            textBox5.Text = FilePic;
            textBox1.Text = "";
            bPic = new Bitmap(rFile);
            rFile.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (bText != null)
                bText.Close();
            string FileText;
            OpenFileDialog dText = new OpenFileDialog();
            dText.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (dText.ShowDialog() == DialogResult.OK)
            {
                FileText = dText.FileName;
            }
            else
            {
                FileText = "";
                return;
            }

            
            try
            {
                rText = new FileStream(FileText, FileMode.Open); //открываем поток
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            textBox4.Text = FileText;
            textBox2.Text = "";
            bText = new BinaryReader(rText, Encoding.ASCII);
            //rText.Close();
        }

        private YCrCb[,] ArrayFromPic(int i, int j)
        {
            YCrCb[,] result = new YCrCb[8, 8];
            for(int k=0; k < 8; k++)
            {
                for(int l = 0; l < 8; l++)
                {
                    Color pixelColor = bPic.GetPixel(k+i, l+j);
                    result[k, l].Y = (pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
                    result[k, l].Cr = (int)(pixelColor.R * 0.5 + pixelColor.G * (-0.4187) + pixelColor.B * (-0.0813) + 128 );
                    result[k, l].Cb = (int)(pixelColor.R *(-0.1687) + pixelColor.G *(-0.3313) + pixelColor.B * 0.5 + 128 );
                }
            }
            return result;
        }
        private bool ToRGBFromY(int k, int l, YCrCb[,] arr)
        {
            YCrCb[,] result = new YCrCb[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    byte R, G, B;
                    R = (byte)(arr[i, j].Y + 1.402 * (arr[i, j].Cr-128));
                    G = (byte)(arr[i, j].Y - 0.34414*(arr[i,j].Cb-128) - 0.71414 * (arr[i, j].Cr - 128));
                    B = (byte)(arr[i, j].Y + 1.772 * (arr[i, j].Cb - 128));
                    bPic.SetPixel(i+k, j+l, Color.FromArgb(R, G, B));
                }
            }
            return true;
        }
        private bool InCryptMatix(int k, int l, double[,] arr)
        {
           
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Color PxColor = bPic.GetPixel(k+i, l+j);
                    //if(arr[i,j] == 255)
                    //    bPic.GetPixel(900, 900);
                    bPic.SetPixel(k+i, l+j, Color.FromArgb(PxColor.R, PxColor.G, (byte)(Math.Round(arr[i,j]))));
                }
            }
            return true;
        }
        private bool InCryptMatix(int k, int l, byte[,] arr)
        {

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Color PxColor = bPic.GetPixel(k + i, l + j);
                    //if(arr[i,j] == 255)
                    //    bPic.GetPixel(900, 900);
                    bPic.SetPixel(k + i, l + j, Color.FromArgb(PxColor.R, PxColor.G, arr[i, j]));
                }
            }
            return true;
        }
        private double[,] BArray(int i, int j)
        {
            double[,] result = new double[8, 8];
            //byte[,] result = new byte[8, 8];
            for (int k = 0; k < 8; k++)
            {
                for (int l = 0; l < 8; l++)
                {
                    Color pixelColor = bPic.GetPixel(k + i, l + j);
                    result[k, l] = pixelColor.B;
                    
                }
            }
            return result;
        }
        private double[,] DCT(YCrCb[,] arr)
        {
            double Cm, Cn, R = 0.0;

            double[,] result = new double[8, 8];
            for (int m = 0; m < 8; m++)
            {
                if (m != 0)
                    Cm = Math.Sqrt(2.0 / 8.0);
                else
                    Cm = Math.Sqrt(1.0 / 8.0);

                for (int n = 0; n < 8; n++)
                {
                    R = 0.0;
                    if (n != 0)
                        Cn = Math.Sqrt(2.0 / 8.0);
                    else
                        Cn = Math.Sqrt(1.0 / 8.0);
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            R += (arr[i, j].Y * Math.Cos(((2 * j + 1) * Math.PI * n) / (2 * 8)) * Math.Cos(((2 * i + 1) * Math.PI * m) / (2 * 8)));

                        }
                    }
                    R *= (Cn * Cm);
                    result[m, n] = R;
                }
            }

            return result;
        }
        private double[,] DCT(double[,] arr)
        {
            double Cm, Cn, R=0.0;

            double[,] result = new double[8, 8];
            for(int m = 0; m < 8; m++) {
                if (m != 0)
                    Cm = Math.Sqrt(2.0 / 8.0);
                else
                    Cm = Math.Sqrt(1.0 / 8.0);
                
                for (int n = 0; n < 8; n++)
                {
                    R = 0.0;
                    if (n != 0)
                        Cn = Math.Sqrt(2.0 / 8.0);
                    else
                        Cn = Math.Sqrt(1.0 / 8.0);
                    for (int i = 0; i < 8; i++)
                    {
                        for(int j =0; j < 8; j++)
                        {
                            R += (arr[i, j] * Math.Cos(((2 * j + 1) * Math.PI * n )/ (2 * 8)) * Math.Cos(((2 * i + 1) * Math.PI * m )/ (2 * 8)));
                           
                        }
                    }
                    R *= (Cn * Cm);
                    result[m, n] = R;
                }
            }
            
            return result;
        }
        private double[,] DCT(byte[,] arr)
        {
            double Cm, Cn, R = 0.0;

            double[,] result = new double[8, 8];
            for (int m = 0; m < 8; m++)
            {
                if (m != 0)
                    Cm = Math.Sqrt(2.0 / 8.0);
                else
                    Cm = Math.Sqrt(1.0 / 8.0);

                for (int n = 0; n < 8; n++)
                {
                    R = 0.0;
                    if (n != 0)
                        Cn = Math.Sqrt(2.0 / 8.0);
                    else
                        Cn = Math.Sqrt(1.0 / 8.0);
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            R += (arr[i, j] * Math.Cos(((2 * j + 1) * Math.PI * n) / (2 * 8)) * Math.Cos(((2 * i + 1) * Math.PI * m) / (2 * 8)));

                        }
                    }
                    R *= (Cn * Cm);
                    result[m, n] = R;
                }
            }

            return result;
        }
        //private YCrCb[,] ReverDCT(double[,] arr, YCrCb[,] result)
        //{
        //    //YCrCb[,] result = new YCrCb[8, 8];
        //    double Cm, Cn, R = 0.0;
        //    for (int m = 0; m < 8; m++)
        //    {


        //        for (int n = 0; n < 8; n++)
        //        {
        //            R = 0.0;

        //            for (int i = 0; i < 8; i++)
        //            {

        //                for (int j = 0; j < 8; j++)
        //                {
        //                    if (i != 0)
        //                        Cn = Math.Sqrt(2.0 / 8.0);
        //                    else
        //                        Cn = Math.Sqrt(1.0 / 8.0);
        //                    if (j != 0)
        //                        Cm = Math.Sqrt(2.0 / 8.0);
        //                    else
        //                        Cm = Math.Sqrt(1.0 / 8.0);
        //                    R += (Cn * Cm * arr[i, j] * Math.Cos(((2 * n + 1) * Math.PI * j) / (2 * 8)) * Math.Cos(((2 * m + 1) * Math.PI * i) / (2 * 8)));

        //                }
        //            }
        //            //R *= (Cn * Cm);
        //            result[m, n].Y = R;
        //        }
        //    }
        //    return result;
        //}
        private double[,] ReverDCT(double[,] arr, double[,] result)
        {
            //YCrCb[,] result = new YCrCb[8, 8];
            double Cm, Cn, R = 0.0;
            for (int m = 0; m < 8; m++)
            {
                
                
                for (int n = 0; n < 8; n++)
                {
                    R = 0.0;
                    
                    for (int i = 0; i < 8; i++)
                    {
                        
                        for (int j = 0; j < 8; j++)
                        {
                            if (i != 0)
                                Cn = Math.Sqrt(2.0 / 8.0);
                            else
                                Cn = Math.Sqrt(1.0 / 8.0);
                            if (j != 0)
                                Cm = Math.Sqrt(2.0 / 8.0);
                            else
                                Cm = Math.Sqrt(1.0 / 8.0);
                            R += (Cn*Cm*arr[i, j] * Math.Cos(((2 * n + 1) * Math.PI * j) / (2 * 8)) * Math.Cos(((2 * m + 1) * Math.PI * i) / (2 * 8)));

                        }
                    }
                    //R *= (Cn * Cm);
                    result[m, n] = R;
                }
            }
            return result;
        }
        private byte[,] ReverDCT(double[,] arr, byte[,] result)
        {
            //YCrCb[,] result = new YCrCb[8, 8];
            double Cm, Cn, R = 0.0;
            for (int m = 0; m < 8; m++)
            {


                for (int n = 0; n < 8; n++)
                {
                    R = 0.0;

                    for (int i = 0; i < 8; i++)
                    {

                        for (int j = 0; j < 8; j++)
                        {
                            if (i != 0)
                                Cn = Math.Sqrt(2.0 / 8.0);
                            else
                                Cn = Math.Sqrt(1.0 / 8.0);
                            if (j != 0)
                                Cm = Math.Sqrt(2.0 / 8.0);
                            else
                                Cm = Math.Sqrt(1.0 / 8.0);
                            R += (Cn * Cm * arr[i, j] * Math.Cos(((2 * n + 1) * Math.PI * j) / (2 * 8)) * Math.Cos(((2 * m + 1) * Math.PI * i) / (2 * 8)));

                        }
                    }
                    //R *= (Cn * Cm);
                    result[m, n] = (byte)(Math.Round(R));
                }
            }
            return result;
        }

        ////ggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg
        //static double[,] dkp(YCrCb[,] one)
        //{
        //    int n = one.GetLength(0);
        //    double[,] two = new double[n, n];
        //    double U, V, temp = 0;
        //    for (int v = 0; v < n; v++)
        //    {
        //        for (int u = 0; u < n; u++)
        //        {
        //            if (v == 0) V = 1.0 / Math.Sqrt(2);
        //            else V = 1;
        //            if (u == 0) U = 1.0 / Math.Sqrt(2);
        //            else U = 1;
        //            temp = 0;
        //            for (int i = 0; i < n; i++)
        //            {
        //                for (int j = 0; j < n; j++)
        //                {
        //                    temp += one[i, j].Y * Math.Cos(Math.PI * v * (2 * i + 1) / (2 * n)) *
        //                        Math.Cos(Math.PI * u * (2 * j + 1) / (2 * n));
        //                }
        //            }
        //            two[v, u] = U * V * temp / (Math.Sqrt(2 * n));
        //        }
        //    }
        //    return two;
        //}
        //static bool odkp(double[,] one, YCrCb[,] result)
        //{
        //    int n = one.GetLength(0);
        //    //double[,] two = new double[n, n];
        //    double U, V, temp = 0;
        //    for (int v = 0; v < n; v++)
        //    {
        //        for (int u = 0; u < n; u++)
        //        {
        //            temp = 0;
        //            for (int i = 0; i < n; i++)
        //            {
        //                for (int j = 0; j < n; j++)
        //                {
        //                    if (i == 0) V = 1.0 / Math.Sqrt(2);
        //                    else V = 1;
        //                    if (j == 0) U = 1.0 / Math.Sqrt(2);
        //                    else U = 1;
        //                    temp += U * V * one[i, j] * Math.Cos(Math.PI * i * (2 * v + 1) / (2 * n)) *
        //                        Math.Cos(Math.PI * j * (2 * u + 1) / (2 * n));
        //                }
        //            }
        //            result[v, u].Y = temp / (Math.Sqrt(2 * n));
        //        }
        //    }
        //    return true;
        //}

        ////ggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg


        private double[,] norm(double[,] one)
        {
            double min = one[0,0], max = one[0,0];

            for (int i = 0; i < one.GetLength(0); i++)
                for (int j = 0; j < one.GetLength(1); j++)
                {
                    if (one[i, j] > max) max = one[i, j];
                    if (one[i, j] < min) min = one[i, j];
                }

            double[,] two = new double[one.GetLength(0), one.GetLength(1)];
            for (int i = 0; i < one.GetLength(0); i++)
                for (int j = 0; j < one.GetLength(1); j++)
                    two[i, j] = 255 * (one[i, j] + Math.Abs(min)) / (max + Math.Abs(min));

            return two;
        }
        private double[,] CopyBlockTo(int i, int j, double[,] arr, double[,] FullMatrix)
        {
            
            for (int k = 0; k < 8; k++)
                for (int l = 0; l < 8; l++)
                {
                    FullMatrix[i + k, j + l] = arr[k, l];
                }
            return FullMatrix;
        }
        private byte[,] CopyBlockTo(int i, int j, byte[,] arr, byte[,] FullMatrix)
        {

            for (int k = 0; k < 8; k++)
                for (int l = 0; l < 8; l++)
                {
                    FullMatrix[i + k, j + l] = arr[k, l];
                }
            return FullMatrix;
        }
        private byte[,] norm(byte[,] one)
        {
            double min = Double.MaxValue, max = Double.MinValue;

            for (int i = 0; i < one.GetLength(0); i++)
                for (int j = 0; j < one.GetLength(1); j++)
                {
                    if (one[i, j] > max) max = one[i, j];
                    if (one[i, j] < min) min = one[i, j];
                }

            byte[,] two = new byte[one.GetLength(0), one.GetLength(1)];
            for (int i = 0; i < one.GetLength(0); i++)
                for (int j = 0; j < one.GetLength(1); j++)
                    two[i, j] = (byte)(255 * (one[i, j] + Math.Abs(min)) / (max + Math.Abs(min)));

            return two;
        }
        private bool KochZhaoOutCrypt(double[,] dct)
        {
           
            if (Math.Abs(dct[5, 1]) > Math.Abs(dct[2, 6]))
                return false;
            else
                return true;
           
        }
        private double[,] KochZhaoIncrypt(double[,] dct, bool bit, int P)
        {
            
            double Abs1, Abs2;
            double z1 = 0, z2 = 0;
            Abs1 = Math.Abs(dct[5, 1]);
            Abs2 = Math.Abs(dct[2, 6]);
            if (dct[5, 1] >= 0) z1 = 1;
            else z1 = -1;
            if (dct[2, 6] >= 0) z2 = 1;
            else z2 = -1;
            //double[,] result = new double[8,8];
            if (bit)
            {
                //while(Math.Abs(dct[5,1]) - Math.Abs(dct[2,6]) > -15)
                //{
                //bPic.GetPixel(900, 900);
                //if (Math.Abs(dct[5, 1]) - Math.Abs(dct[2, 6]) > -15)
                //{
                //    //if (dct[5, 1] > 0 && (dct[5, 1] - 6) > 0)
                //    //{
                //    //    dct[2, 6] = (dct[5, 1] + 10);
                //    //    dct[5, 1] -= 6;
                //    //}
                //    //else
                //    //{
                //    //    dct[2, 6] = (dct[5, 1] - 10);
                //    //    dct[5, 1] += 6;
                //    //}
                //    dct[2, 6] = (dct[5, 1] + 16);
                //}
                //}
                if (Abs1 - Abs2 >= -P)
                    Abs2 = P + Abs1 + 1;
            }
            else
            {
                //while (Math.Abs(dct[5, 1]) - Math.Abs(dct[2, 6]) < 15)
                //{
                //if ((Math.Abs(dct[5, 1]) - Math.Abs(dct[2, 6]) < 15))
                //{
                //    //bPic.GetPixel(900, 900);
                //    //if (dct[2, 6] > 0)
                //    //{
                //    //    dct[5, 1] = (dct[2, 6] + 10);
                //    //    dct[2, 6] -= 6;
                //    //}
                //    //else
                //    //{
                //    //    dct[5, 1] = (dct[2, 6] - 10);
                //    //    dct[2, 6] += 6;
                //    //}
                //    dct[5, 1] = (dct[2, 6] + 16);
                //}
                //}
                if (Abs1 - Abs2 <= P)
                    Abs1 = P + Abs2 + 1;
            }
            //bPic.GetPixel(900, 900);
            dct[5, 1] = z1 * Abs1;
            dct[2, 6] = z2 * Abs2;
            //if (j <= 46 && j + 8 >= 46)
            //    bPic.GetPixel(900, 900);
            return dct;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Пожалуйста, выберите исходное изображение и скрываемое сообщение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Перед применением алгоритма, выберите файл, в который нужно записать пароль.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            List<byte> bList = new List<byte>();
            string FileText;
            OpenFileDialog dText = new OpenFileDialog();
            dText.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (dText.ShowDialog() == DialogResult.OK)
            {
                FileText = dText.FileName;
            }
            else
            {
                FileText = "";
                return;
            }

            FileStream localText;
            try
            {
                localText = new FileStream(FileText, FileMode.Open); //открываем поток
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            while (bText.PeekChar() != -1)
            { //считали весь текстовый файл для шифрования в лист байт
                bList.Add(bText.ReadByte());
            }
            int CountText = bList.Count; // в CountText - количество в байтах текста, который нужно закодировать
            //bText.Close();
            //rFile.Close();
            int HeightOfEmg = bPic.Height;
            while ((HeightOfEmg+1) % 8 != 0)
                HeightOfEmg--;
            
            int WidthOfEmg = bPic.Width;
            while((WidthOfEmg+1) % 8 != 0)
                WidthOfEmg--;
            //проверяем, поместиться ли исходный текст в картинке
            if (CountText > ((WidthOfEmg * HeightOfEmg) / 512))
            {
                MessageBox.Show("Выбранная картинка мала для размещения выбранного текста", "Ошибка", MessageBoxButtons.OK);
                bPic.Dispose();
                rText.Close();
                textBox4.Text = "";
                textBox5.Text = "";
                return;
            }

            StreamWriter brW = new StreamWriter(localText, Encoding.GetEncoding(1251));

            int[] StepList = new int[CountText + 1];
            char[] Stp = new char[CountText + 1];
            int[] Steps = new int[CountText + 1];
            BitArray messageArray = ByteToBit(bList[0]);
            string pass = "";

            double[,] ImgMatrix = new double[bPic.Width,bPic.Height];
            //byte[,] ImgMatrix = new byte[bPic.Width, bPic.Height];
            for (int k = 0; k < bPic.Width; k++)
                for (int l = 0; l < bPic.Height; l++)
                    ImgMatrix[k, l] = bPic.GetPixel(k, l).B;
            
            int index = 0;
            
            Random rnd = new Random();
            int rndValue = 1;
            int i = 0, j = 0;
            YCrCb[,] matr, tmpmatr;
            double[,] Bmatr, tmpBmatr;
            //byte[,] Bmatr, tmpBmatr;
            double[,] DCTmatr, tmpDct;
            int PValue = int.Parse(textBox6.Text);
            int maxStep = (int)(((WidthOfEmg * HeightOfEmg) / 64) / ((CountText+8)*8));
            
            while (index != CountText*8 + 8)
            {
                if((index == 0 || index%8 == 0) && index < CountText*8)
                    messageArray = ByteToBit(bList[index/8]);
                if (radioButton9.Checked)
                {
                    rndValue = rnd.Next(1, maxStep+1);
                    brW.Write(rndValue.ToString());
                    pass += rndValue.ToString();
                    if (index != CountText * 8 + 7)
                    {
                        brW.Write("-");
                        pass += "-";
                    }
                }
                
                //tmpmatr = ArrayFromPic(i, j);
                Bmatr = BArray(i, j);
                //bPic.GetPixel(808, 808);
                tmpBmatr = BArray(i, j);
                //DCTmatr = DCT(Bmatr);
                DCTmatr = DCT(Bmatr);
                if(index < CountText * 8)
                    DCTmatr = KochZhaoIncrypt(DCTmatr, messageArray[index % 8], PValue);
                else
                    DCTmatr = KochZhaoIncrypt(DCTmatr, true, PValue);
                //DCTmatr = norm(DCTmatr);
                //DCTmatr = norm(DCTmatr);

                Bmatr = ReverDCT(DCTmatr, Bmatr);
                //if (j <= 46 && j + 8 >= 46)
                //    bPic.GetPixel(900, 900);
                CopyBlockTo(i, j, Bmatr, ImgMatrix);
                //bPic.GetPixel(808, 808);
                //InCryptMatix(i, j, Bmatr);
                //tmpmatr = ReverDCT(DCTmatr, tmpmatr);


                //bPic.GetPixel(808, 808);
                //Bmatr = ReverDCT(DCTmatr, Bmatr);
                //DCTmatr = norm(DCTmatr);

                //ToRGBFromY(i, j, matr);

                //matr = ArrayFromPic(i, j);
                //tmpBmatr = BArray(i, j);
                //tmpDct = DCT(tmpBmatr);
                //bPic.GetPixel(808, 808);

                if(index == CountText*8-1)
                    InCryptByte(i, j, 255, index);
                i += 8*rndValue;

                if (i + 7 > WidthOfEmg || i > WidthOfEmg)
                {
                    i = 0;
                    j += 8;
                }
                index++;
            }
            
            richTextBox4.Text = pass;
            ImgMatrix = norm(ImgMatrix);
            InCryptMatix(0, 0, ImgMatrix);


            //Bmatr = BArray(0, 0);
            ////bPic.GetPixel(808, 808);
            ////tmpBmatr = BArray(i, j);
            //DCTmatr = DCT(Bmatr);
            //bPic.GetPixel(808, 808);
            //string str = new string(StepList);
            //richTextBox1.Text = str;
            //textBox3.Text = char.Parse(Stp);

            //pictureBox2.Image = bPic;
            MessageBox.Show("Алгоритм втраивания прошел успешно. Сохраните изображение.", "Завершение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            String sFilePic;
            SaveFileDialog dSavePic = new SaveFileDialog();
            dSavePic.Filter = "Файлы изображений (*.bmp)|*.bmp|Файлы изображений (*.jpeg)|*.jpeg|Файлы изображений (*.jpg)|*.jpg|Все файлы (*.*)|*.*";
            if (dSavePic.ShowDialog() == DialogResult.OK)
            {
                sFilePic = dSavePic.FileName;
            }
            else
            {
                sFilePic = "";
                return;
            };

            FileStream wFile;
            try
            {
                wFile = new FileStream(sFilePic, FileMode.Create); //открываем поток на запись результатов
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла на запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            brW.Close();
            bPic.Save(wFile, System.Drawing.Imaging.ImageFormat.Bmp);
            wFile.Close(); //закрываем поток
            localText.Close();
            rText.Close();
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char temp = e.KeyChar;
            if (!Char.IsDigit(temp) && !Char.IsControl(temp))//проверка на то, чтобы введенный символ являлся цифрой или управляющим символом (BackSpace)
            {
                e.Handled = true;
            }
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            bPic = null;
            textBox3.Text = "";
            //if (radioButton4.Checked)
            radioButton5.Enabled = true;
            //else
            radioButton4.Enabled = true;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox5.Enabled = false;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox5.Enabled = true;
        }
    }
}
