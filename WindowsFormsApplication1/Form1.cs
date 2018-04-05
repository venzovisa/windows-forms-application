using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
	        //pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;             
        }
       
        PictureBox pictureBox1 = new PictureBox();
        PictureBox pictureBox2 = new PictureBox();
        OpenFileDialog open = new OpenFileDialog();        
        Bitmap CurrentBitmap = new Bitmap(512, 512);
        Bitmap CurrentBitmap2 = new Bitmap(512, 512);
        Bitmap FinalBitmap = new Bitmap(512, 512);
        int finalImageWidth = 0;
        int finalImageHeight = 0;
        int CurrentX = 0;
        int CurrentY = 0;
        int ImageHeight = 0;
        int ImageWidth = 0;
        int ImageMaxGreyValue = 255;//Максимална стойност на сивия цвят, чете се от файла
        String CurrentFile = "";// В този string се чете инф-та за цветовете на пикселите(от нач. до кр.на пикселите) в изобр.; 
        String CurrentFile2 = "";
        String CurrentFilePath = "";//Пътят до файла;
        String ImageType = "";// Типа на картинката, чете се от .pgm файл "P2"; 
        ArrayList ColorArrayFiltered = new ArrayList();
        ArrayList ColorArrayFiltered2 = new ArrayList();
        //bool hasImage = false;
        

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
            /*
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                   
                    CurrentBitmap.SetPixel(x, y, Color.FromArgb(150, 150, 150));
                                      
                }
            }
            Point newPoint = new Point(50, 50);
            Graphics newImage = this.CreateGraphics();
            newImage.DrawImage(CurrentBitmap, newPoint); 
 */
            
            open.Filter = "All Supported types |*.jpg;*.jpeg;*.gif;*.bmp;*.pgm|JPEG (*.jpg;*.jpeg)|*.jpg; *.jpeg;|GIF (*.GIF)|*.gif|Bitmap files (*.bmp)|*.bmp|PGM files (*.pgm)|*.pgm";
            open.Multiselect = true;
            //open.Filter-> Филтрира се прозорецът за отваряне на файл, така че да може да се посочат	               
            //само определени видове формати за изображения;

            if (open.ShowDialog() == DialogResult.OK)
            //Проверява  дали е избран бутонът Open от прозореца за посочване на файл; 
            {
                try
                {
                    if (open.FileName.ToUpper().EndsWith(".PGM"))
                    //Проверява  дали посоченият файл завършва с разширенеие .PGM
                    {
                        openPGMfile(0); // Open first image 
                        openPGMfile(1);
                    }
                    else
                    {
                        MessageBox.Show("Invalid image format!");
                    }
                }
                catch (Exception ex)
                {
                    // Ako файловете от кода между try i catch са повредени се извиква следниям MessageBox


                    MessageBox.Show("The image is corrupt or is not in correct format!");
                    // MessageBox.Show(ex.Data.ToString());
                }
            }
              
        }
       
        private void openPGMfile(int fileNumber)
	        {
                if (fileNumber == 0) 
                    CurrentFilePath = open.FileNames[0].ToString();//Пътят до файла; 
                else if (fileNumber == 1)
                    CurrentFilePath = open.FileNames[1].ToString();//Пътят до файла;                                           
	            StreamReader ImageStreamReader = new StreamReader(CurrentFilePath);
	            String LineBuffer;
	              //за запаметяване размерите на изображението дължина и ширина
	            String[] ExtractDimension;
	            PurgeGlobalData();//Функция за нулиране на променливите, които се инизиализират в началото,
	              // използват се в случай, че се отвори следваща картинка;
	            LineBuffer = "#";//Проверка дали файлът не започва с коментар "#";
	            do
	            {
	                LineBuffer = ImageStreamReader.ReadLine();
	            } while (LineBuffer.StartsWith("#"));
	            
	             // Проверка дали файлът започва с подходяща информация за форматът си в 
	             // противен случай се извежда подходящо съобщение
	             
	            if (LineBuffer.StartsWith("P2"))
	            {
	                ImageType = "P2";
	            }
	            else 
	            { 
	                ImageStreamReader.Close();
	                MessageBox.Show("The image is not the write format or is corrupt!!!");
	                PurgeGlobalData(); 
            	            }
	            //Проверка за други коментари в файла и прочитане на размерите за изображението
	            LineBuffer = "#";
	            do
	            {
	                LineBuffer = ImageStreamReader.ReadLine();
	            } while (LineBuffer.StartsWith("#"));
	            
	             // Прочетените размери на изображението се запаметяват в масива ExtractDimensions
	             // след което стойностите им се преобразуват в целочислен вид и се предават на 
	             // Bitmap изображението
	             
	            ExtractDimension = LineBuffer.Split(' ');
	            ImageHeight = int.Parse(ExtractDimension[1]);
                finalImageHeight = ImageHeight;
	            ImageWidth = int.Parse(ExtractDimension[0]);
                finalImageWidth = ImageWidth;
	            CurrentBitmap = new Bitmap(ImageWidth, ImageHeight);
                CurrentBitmap2 = new Bitmap(finalImageWidth, finalImageHeight);
                FinalBitmap = new Bitmap(finalImageWidth, finalImageHeight);
                
	            
	             // Ако файлът започва с правилната първоначална информация "P2", прочитат се 
	             // стойностите за сивия цвят от него, преобразуват се в целочислен вид и 
	             // се предават на променливата ImageMaxGreyValue;
	             
	            if (ImageType == "P2")
	            {
	                LineBuffer = "#";
	                do
	                {
	                    LineBuffer = ImageStreamReader.ReadLine();
	                } while (LineBuffer.StartsWith("#"));
	                ImageMaxGreyValue = int.Parse(LineBuffer);
	            }

                if (fileNumber == 0) {
                    CurrentFile = ImageStreamReader.ReadToEnd();
                    CurrentFile = CurrentFile.Replace("\r", " ");
                    CurrentFile = CurrentFile.Replace("\n", " ");
                }

                else if (fileNumber == 1) {
                    CurrentFile2 = ImageStreamReader.ReadToEnd();
                    CurrentFile2 = CurrentFile2.Replace("\r", " ");
                    CurrentFile2 = CurrentFile2.Replace("\n", " ");
                }
                    
                                           
            //Затваряне на входния поток и извикане на функцията за изобразяване BiuldCanvas()
	            ImageStreamReader.Close();
            if (fileNumber == 0)
	            BuildCanvas(0);
            else if (fileNumber == 1)    
                BuildCanvas(1);
	        }

  
        
        public void BuildCanvas(int fileNumber)
	        {
	            try
	            {
                    String NewString = null;
                    if (fileNumber == 0) {
                        NewString = CurrentFile;
                    }
                    else if (fileNumber == 1) {
                        NewString = CurrentFile2;   
                    }
                                               
	                NewString = NewString.Replace("   ", " ");
	                NewString = NewString.Replace("  ", " ");
                    ArrayList ColorArrayFilteredTemp = new ArrayList();
                    Bitmap CurrentBitmapTemp = new Bitmap(512, 512);
                 
	                String[] ColorArray = NewString.Split(' ');                                   
                    //if (ColorArrayFiltered.Count > 0)
                    //{                      
                        int Counter = 0;
                        int NewCounter = 0;
                        do
                        {
                            if (ColorArray[NewCounter].Length != 0)
                            {                            
                                    ColorArrayFilteredTemp.Add(ColorArray[NewCounter]);                                                                                         
                            }
                            NewCounter += 1;
                            //брои докато свършат елементите в ColorArray
                        } while (NewCounter != ColorArray.Length);

                        // Required data for calculating average
                        if (fileNumber == 0) {
                            ColorArrayFiltered = ColorArrayFilteredTemp;
                        }
                        else if (fileNumber == 1) {
                            ColorArrayFiltered2 = ColorArrayFilteredTemp;
                        }

                        if (ImageType == "P2")
                        {
                            do
                            {
                                if (CurrentX == CurrentBitmap.Width)
                                {
                                    CurrentX = 0;
                                    CurrentY += 1;
                                }
                                if (CurrentY == CurrentBitmap.Height)
                                {
                                    break;
                                }
                                //поставяне на пиксел с определени кординати в CurrentBitmap с опредлелен цвят(R,G,B)
                                //За RGB се поставя една и съща стойност за да стане сив цвят                             
                                    CurrentBitmapTemp.SetPixel(CurrentX, CurrentY, Color.FromArgb(255, 
                                        int.Parse(ColorArrayFilteredTemp[Counter].ToString()), 
                                        int.Parse(ColorArrayFilteredTemp[Counter].ToString()), 
                                        int.Parse(ColorArrayFilteredTemp[Counter].ToString())));                            
                                CurrentX += 1;
                                Counter += 1;
                            } while (Counter <= ColorArrayFilteredTemp.Count);
                            if (fileNumber == 0) {
                                CurrentBitmap = CurrentBitmapTemp;
                            }
                            else if (fileNumber == 1) {
                                CurrentBitmap2 = CurrentBitmapTemp;
                            }
                        }
                    //}                       
                            // Draw first image
                            Point newPoint = new Point(50, 50);
                            Graphics newImage = this.CreateGraphics();
                            newImage.DrawImage(CurrentBitmap, newPoint);

                            // Draw second image
                            Point newPoint2 = new Point(250, 50);
                            Graphics newImage2 = this.CreateGraphics();
                            newImage2.DrawImage(CurrentBitmap2, newPoint2);

                            // Draw final image
                            BuildFinalImage();
                            Point newPoint3 = new Point(50, 120);
                            Graphics newImage3 = this.CreateGraphics();
                            newImage3.DrawImage(FinalBitmap, newPoint3);
                        
                       

                        // Draw second image
                        //if (ColorArrayFiltered2 != null)
                        //{
                        //    MessageBox.Show("Draw second image");
                        //    Point newPoint2 = new Point(250, 50);
                        //    Graphics newImage2 = this.CreateGraphics();
                        //    newImage2.DrawImage(CurrentBitmap2, newPoint2);
                        //}

                        //hasImage = true;
                    
                  
	            }
	            catch (Exception e)
	            {
	                MessageBox.Show("The image is corrupt or is not in correct format!");
	              //  MessageBox.Show(e.GetBaseException().ToString());
	                PurgeGlobalData();
	            }	
            
	        }

        public void BuildFinalImage() {
            
            int Counter = 0;
          
                do
                {
                    if (CurrentX == FinalBitmap.Width)
                    {
                        CurrentX = 0;
                        CurrentY += 1;
                    }
                    if (CurrentY == FinalBitmap.Height)
                    {
                        break;
                    }
                    //поставяне на пиксел с определени кординати в CurrentBitmap с опредлелен цвят(R,G,B)
                    //За RGB се поставя една и съща стойност за да стане сив цвят

                    // Average of the two images
                    int r = (int.Parse(ColorArrayFiltered[Counter].ToString()) + int.Parse(ColorArrayFiltered2[Counter].ToString())) / 2;
                    int g = (int.Parse(ColorArrayFiltered[Counter].ToString()) + int.Parse(ColorArrayFiltered2[Counter].ToString())) / 2; ;
                    int b = (int.Parse(ColorArrayFiltered[Counter].ToString()) + int.Parse(ColorArrayFiltered2[Counter].ToString())) / 2; ;
         
                    FinalBitmap.SetPixel(CurrentX, CurrentY, Color.FromArgb(255, 100, 150, 255));
                    CurrentX += 1;
                    Counter += 1;
                } while (Counter <= ColorArrayFiltered.Count);              
            
        }
        
       /*private void Form1_Load(object sender, EventArgs e)
	        {         
	
	        }*/
	
	    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
	        {
	            this.Dispose();
	        }

        private void PurgeGlobalData()
            {
                //CurrentBitmap = new Bitmap(5, 5);
                //  DisplayPanel.Image = Nothing
                CurrentX = 0;
                CurrentY = 0;
                ImageHeight = 0;
                ImageWidth = 0;
                ImageMaxGreyValue = 255;
                CurrentFile = "";             
                CurrentFilePath = "";
                ImageType = "";
            }
/*
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
	        {
	            if (hasImage == true)
	            {
	               
	                    pictureBox2.Height = pictureBox1.Height;
	                    pictureBox2.Width = pictureBox1.Width;
	                    pictureBox2.Image = pictureBox1.Image;
	                    //zoom2x = false;
	              
	            }
	            else MessageBox.Show(" Open an image first!!!");
	             
	        }
	
        private void xToolStripMenuItem_Click(object sender, EventArgs e)
	        {
	            if (hasImage == true)
	            {
	               //за да може картинката да се уголемява с размерите на самия PictureBox2
	                    pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
	                    pictureBox2.Image = pictureBox1.Image;
	                    pictureBox2.Height = pictureBox1.Height * 2;
	                    pictureBox2.Width = pictureBox1.Width * 2;
	                    //zoom2x = true;
	               
	            }else MessageBox.Show(" Open an image first!!!");         
}

        private void Form1_Load_1(object sender, EventArgs e) {}
        
        private void button1_Click(object sender, EventArgs e)
        { MessageBox.Show(" Open an image first!!!"); }
        */
    }
}
