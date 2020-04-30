using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace odev
{
    public partial class Form1 : Form
    {
        //CKG FONKSİYONU İLE İSTEĞE BAĞLI RASTGELE SAYILARIN ÜRETİLMESİ
        public int CKG(int adet, int alt_sinir, int ust_sinir)
        {
            int genRand = 0;
            Random r = new Random();
            for (int i = 0; i < adet; i++)
            {
                genRand = r.Next(alt_sinir, ust_sinir);
            }
            return genRand;
        }

        //DIAGONAL TRAVERSAL FONKSİYONU
        public int[] diagonalOrder(int[,] matrix, int M, int N)
        {
            int sonuc;
            int[] dizi = new int[M * N];
            for (int line = 1; line <= (M + N - 1); line++)
            {
                int start_col = Math.Max(0, line - M);
                int count = Math.Min(line, Math.Min(
                                      (N - start_col), M));
                for (int j = 0; j < count; j++)
                {
                    sonuc = matrix[Math.Min(M, line)
                                - j - 1, start_col + j];
                    dizi[j] = sonuc;
                }
            }
            return dizi;
        }

        //DİZİNİN 256 PİKSEL İÇEREN BLOKLARA AYRILMASI
        private static List<int[]> splitArray(int[] traversedArray)
        {
            List<int[]> result = new List<int[]>();
            for (int i = 0; i < traversedArray.Length; i += 256)
            {
                int[] buffer = new int[256];
                Buffer.BlockCopy(traversedArray, i, buffer, 0, 256);
                result.Add(buffer);
            }
            return result;
        }

        //PERMÜTASYON FONKSİYONUNUN HESAPLANMASI
        public int Permutasyon_Hesapla(int n, int r)
        {
            int per, fakt, fakt1;
            fakt = n;
            for (int i = n - 1; i >= 1; i--)
            {
                fakt = fakt * i;
            }
            int number;
            number = n - r;
            fakt1 = number;
            for (int i = number - 1; i >= 1; i--)
            {
                fakt1 = fakt1 * i;
            }
            if (fakt1 != 0)
            {
                per = fakt / fakt1;
                return per;
            }
            return 0;
        }

        //FORMUN AÇILMASI VE ÇAĞRILAN FONKSİYONLAR
        public Form1()
        {   
            InitializeComponent();
        }

        //DIALOG PENCERESİNDEN SEÇİLEN RESMİN YÜKLENMESİ
        private void button1_Click(object sender, EventArgs e)
        {
            string imageLocation = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "jpg files(.*jpg)|*.jpg| PNG files(.*png)|*.png| All Files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imageLocation = dialog.FileName;
                pictureBox1.ImageLocation = imageLocation;
            }
        }

        //BUTTON2'YE BASILINCA YAPILACAK İŞLEMLER
        private void button2_Click(object sender, EventArgs e)
        {
            int DONGU_SAYISI = 0, key1 = 0, key2 = 0;
            int[] cikti = new int[256];
            try
            {
                DONGU_SAYISI = int.Parse(textBox2.Text);
                key1 = int.Parse(textBox1.Text);
                key2 = int.Parse(textBox3.Text);
            }
            catch(FormatException)
            {
                MessageBox.Show("Eksik Değer Girdiniz !");
                Application.Restart();
            }
            int KARILMA_SAYISI = DONGU_SAYISI % 4;
            int[] IV = new int[256]; //Initialization Vector tanımlaması

            //RESMİN PİKSELLERE AYRILMASI VE 2 BOYUTLU DİZİYE ATANMASI
            int M = pictureBox1.Image.Width;
            int N = pictureBox1.Image.Height;
            MessageBox.Show("Resim" + " " + M + " x " + N + " " + "boyutunda");
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            int[,] arr = new int[bitmap.Width, bitmap.Height];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    arr[i, j] = bitmap.GetPixel(i, j).ToArgb();
                }
            }

            /*ELDE EDİLEN PİXEL DEĞERLERİNİ GÖRÜNTÜLEME
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Console.WriteLine(arr[x, y]);
                }
                
            }*/
            
            //PİKSEL MATRİSİ ÜZERİNDE DIAGONAL TRAVERSAL İŞLEMİ 
            int[] s = diagonalOrder(arr, M, N);

            /*DIAGONAL OLARAK GEZİLEN DİZİNİN ELEMANLARINI GÖRÜNTÜLEME
            for (int y = 0; y < s.Length; y++)
            {
                Console.WriteLine(s[y]);
            }*/

            //DİZİNİN BLOKLARA AYRILIP BİR LİSTEYE ATANMASI
            List<int[]> lst = splitArray(s);

            //BLOK LİSTEDEKİ 0 OLAN ELEMANLARA RASTGELE DEĞERLER ATANMASI (PADDING)
            int[] cipheredImage = new int[256];
            int inner = 0;
            for (int runs = 0; runs < lst.Count; runs++)
            {
                for (inner = 0; inner < lst[runs].Length; inner++)
                {
                    if (lst[runs][inner] == 0)
                    {
                        lst[runs][inner] = CKG(1, 0, 256);
                    }
                    cipheredImage = lst[runs];
                }
            }

            /*BLOK LİSTENİN ELEMANLARININ VE İNDEKSLERİNİN GÖRÜNTÜLENMESİ  
            int counter = 0;
            foreach (var batch in lst)
            {
                foreach (var eachId in batch)
                {                   
                    Console.WriteLine("Indeks: {0}, Eleman: {1}", counter, eachId);
                }
                counter++;
            }*/

            for (int BASLANGIC = 0; BASLANGIC < DONGU_SAYISI; BASLANGIC++)
            {
                //BLOK LİSTEDEKİ ELEMANLARIN 1.ANAHTAR İLE XOR'LANMASI              
                for (int i = 0; i < 256; i++)
                {
                    cipheredImage[i] = cipheredImage[i] ^ key1;
                }

                //PERMÜTASYON TABLOSUNUN VE ELEMANLARININ OLUŞTURULMASI
                int[,] des_sbox_tablosu = new int[16, 32];
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        des_sbox_tablosu[i, j] = CKG(j, 0, 15);
                        //Console.WriteLine("[{0}, {1}] = {2}", i, j, des_sbox_tablosu[i, j]);
                    }
                }

                //PERMÜTASYON TABLOSUNUN HANGİ SÜTUNLARININ SEÇİLECEĞİNİN BELİRLENMESİ
                int a = CKG(1, 1, 16);
                Thread.Sleep(100);
                int b = CKG(1, 1, 16);
                int[] secilen_a_sutunu = new int[32];
                int[] secilen_b_sutunu = new int[32];
                for (int i = 0; i < 32; i++)
                {
                    secilen_a_sutunu[i] = des_sbox_tablosu[a, i];
                    secilen_b_sutunu[i] = des_sbox_tablosu[b, i];
                }

                /*SEÇİLEN A VE B SÜTUNLARININ GÖRÜNTÜLENMESİ
                for (int i = 0; i < 32; i++)
                {
                    Console.WriteLine("Secilen A sutunu : {0}", secilen_a_sutunu[i]);
                }
                for (int i = 0; i < 32; i++)
                {
                    Console.WriteLine("Secilen B sutunu : {0}", secilen_b_sutunu[i]);
                }*/

                //AES SBOX MATRİSİNİN OLUŞTURULMASI
                int[,] aes_sbox_tablosu = new int[16, 32];
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        aes_sbox_tablosu[i, j] = Permutasyon_Hesapla(a, secilen_a_sutunu[j]);
                    }
                }

                /*SEÇİLEN A VE B SÜTUNLARININ GÖRÜNTÜLENMESİ
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        Console.WriteLine("AES Permutasyon Tablosu : {0}", aes_sbox_tablosu[i, j]);
                    }

                }*/

                //AES SBOX TABLOSUNUN ELEMANLARI İLE 256 PİKSEL BLOĞUNUN XOR'LANMASI
                int k = 0;
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if (k < 256)
                        {

                            cipheredImage[k] = aes_sbox_tablosu[i, j] ^ cipheredImage[i * j];
                            k++;
                        }
                    }
                }

                //BLOK LİSTEDEKİ ELEMANLARIN 2.ANAHTAR İLE XOR'LANMASI            
                /*for (int i = 0; i < cipheredImage.Length; i++)
                {
                    cipheredImage[i] = cipheredImage[i] ^ key2;
                }*/

                //256 PİKSELİN PERMÜTASYON TABLOSUNA GÖRE KARILMASI
                /*for (int i = 0; i < KARILMA_SAYISI; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        cipheredImage[i] = Permutasyon_Hesapla(KARILMA_SAYISI, cipheredImage[i]);
                    }
                }*/
                for(int i=0;i<256;i++)
                {
                    cikti[i] = cipheredImage[i];
                }
            }
            Form2 form2 = new Form2(cikti);
            form2.Show();
            this.Hide();
        }

        //FORMUN TEMİZLENMESİ
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            textBox1.Text = null;   
            textBox2.Text = null;
            textBox3.Text = null;
        }

        //YENİ ANAHTARLARIN İLGİLİ TEXTBOX'LARA ÜRETİLMESİ
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = CKG(1, 0, 256).ToString();
            Thread.Sleep(100);
            textBox3.Text = CKG(1, 0, 256).ToString();
        }

        //UYGULAMADAN ÇIK BUTONU
        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //ÇIKTININ GÖSTERİLMESİ
        private void button5_Click(object sender, EventArgs e)
        {
            
        }
    }
}