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
        }           
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
        ArrayList FinalImageData = new ArrayList();               

        private void toolStripButton1_Click(object sender, EventArgs e)
        {                     
            //open.Filter = "All Supported types |PGM files (*.pgm)|*.pgm";
            // Open two files
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

                    MessageBox.Show("File reading error!");
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
	                MessageBox.Show("File error!");
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
                CurrentBitmap2 = new Bitmap(ImageWidth, ImageHeight);
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

                            // Save each image data in separate string
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

                            // Triggers if there are two images opended
                            if (fileNumber == 1) {
                                // Draw second image
                                Point newPoint2 = new Point(250, 50);
                                Graphics newImage2 = this.CreateGraphics();
                                newImage2.DrawImage(CurrentBitmap2, newPoint2);

                                // Draw final image;
                                BuildFinalImage(1);  
                            }                                                                                                                                          
	            }
	            catch (Exception)
	            {
	                MessageBox.Show("Error in image view!");	             
	                PurgeGlobalData();
	            }	
            
	        }

        public void BuildFinalImage(int fileNumber) {         

            if (fileNumber == 1)
            {
                int Counter = 0;
                CurrentY = 0;
                CurrentX = 0;
                int r, g, b; 

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
                    r = (int.Parse(ColorArrayFiltered[Counter].ToString()) + int.Parse(ColorArrayFiltered2[Counter].ToString())) / 2;
                    g = (int.Parse(ColorArrayFiltered[Counter].ToString()) + int.Parse(ColorArrayFiltered2[Counter].ToString())) / 2; ;
                    b = (int.Parse(ColorArrayFiltered[Counter].ToString()) + int.Parse(ColorArrayFiltered2[Counter].ToString())) / 2; ;

                    // Save data to export in .PGM format
                    FinalImageData.Add(r);
                    FinalImageData.Add(g);
                    FinalImageData.Add(b);

                    FinalBitmap.SetPixel(CurrentX, CurrentY, Color.FromArgb(255, r, g, b));
                    CurrentX += 1;
                    Counter += 1;
                } while (Counter <= ColorArrayFiltered.Count);
                               
  
                Point newPoint3 = new Point(450, 50);
                Graphics newImage3 = this.CreateGraphics();
                newImage3.DrawImage(FinalBitmap, newPoint3);
            }

        }
        
	    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
	        {
	            this.Dispose();
	        }

        private void PurgeGlobalData()
            {               
                CurrentX = 0;
                CurrentY = 0;
                ImageHeight = 0;
                ImageWidth = 0;
                ImageMaxGreyValue = 255;
                CurrentFile = "";             
                CurrentFilePath = "";
                ImageType = "";
            }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {              
            int Counter = 0;                    
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"finalimage.pgm", false))
            {                
                file.WriteLine("P2");
                file.WriteLine("# Comment goes here");
                file.WriteLine(ImageWidth + " " + ImageHeight);
                file.WriteLine(255);
                do
                {                 
                    file.Write(FinalImageData[Counter].ToString() + " ");
                    Counter++;
                }
                while (Counter < FinalImageData.Count);                            
            }
        }
    }
}