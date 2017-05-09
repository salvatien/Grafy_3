using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafy_3
{
    class AdjacencyMatrix
    {

        //nasza macierz :)
        public int[,] AdjacencyArray;




        //zerujemy na poczatek
        public AdjacencyMatrix(int n)
        {
            AdjacencyArray = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    AdjacencyArray[i, j] = 0;
                }
            }
        }



        //rysujemy zranomizowany graf - nawet nie starajcie sie tego zrozumiec
        //dluuugo to naprawialam i sama nie jestem pewna jak, ale dziala
        public void Display(Canvas MyCanvas, int[] TabOfInt, bool randomm, bool draw)
        {

            int max = TabOfInt.Length - 1;
            for (int i = 0; i < max + 1; i++)
            {
                for (int j = 0; j < max + 1; j++)
                {
                    AdjacencyArray[i, j] = 0;
                }
            }

            for (int i = 0; i <= max; i++)
            {
                int value = TabOfInt[i];
                if (value != 0)
                {
                    for (int x = 1; x <= value; x++)
                    {
                        int[] Tab = new int[max + 1];
                        for (int k = 0; k <= max; k++)
                        {
                            Tab[k] = 0;
                        }
                        int j = FindMax(i, TabOfInt, Tab);
                        Tab[j] = 1;
                        bool done = false;
                        while (done == false)
                        {
                            if (AdjacencyArray[i, j] == 0 && i != j)
                            {
                                AdjacencyArray[i, j] = 1;
                                AdjacencyArray[j, i] = 1;
                                TabOfInt[j]--;
                                done = true;
                            }
                            else
                            {
                                j = FindMax(j, TabOfInt, Tab);
                                Tab[j] = 1;
                            }
                        }
                    }
                    TabOfInt[i] = 0;
                }

            }

            




            //wyswietlanie
            if (draw)
                DrawGraph(AdjacencyArray.GetLength(0), MyCanvas);
        }

        //potrzebne do trzenia macierzy - nie probujcie zrozumiec
        public int FindMax(int j, int[] Tab, int[] Tabb)
        {
            int max = Tab.Length - 1;
            int ret = -1;
            if (max == 0)
            {
                ret = 0;
            }
            else
            {
                if (Tabb[0] == 0)
                {
                    for (int i = 0; i <= max; i++)
                    {
                        if (ret == -1)
                        {
                            ret = 0;
                        }
                        else { if (Tab[i] > Tab[ret] && Tabb[i] == 0) ret = i; };
                    }
                }
                else
                {
                    for (int i = 1; i <= max; i++)
                    {
                        if (ret == -1)
                        {
                            ret = 1;
                        }
                        else { if (Tab[i] > Tab[ret] && Tabb[i] == 0) ret = i; };
                    }
                }

            }
            return ret;
        }





        //rysowanie skopiowane z zestawu 1
        public void DrawGraph(int num_v, Canvas MyCanvas)
        {
            MyCanvas.Children.Clear();

            var width = MyCanvas.Width;
            var height = MyCanvas.Height;

            Ellipse myEllipse = new Ellipse();
            myEllipse.Height = 400;
            myEllipse.Width = 400;
            myEllipse.Fill = Brushes.Transparent;
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.LightGray;
            Canvas.SetLeft(myEllipse, width / 2 - 200);
            Canvas.SetTop(myEllipse, height / 2 - 200);
            MyCanvas.Children.Add(myEllipse);

            var r = 200;    //radius

            var x_m = width / 2;    //x middle
            var y_m = height / 2;   //y middle


            for (int i = 1; i <= num_v; i++)
            {
                var angle = (2 * Math.PI) / num_v * i;

                var x_oc = r * Math.Cos(angle) + x_m;   //x on cirlce
                var y_oc = r * Math.Sin(angle) + y_m;   //y on circle

                Ellipse smallPoint = new Ellipse();
                smallPoint.Height = 8;
                smallPoint.Width = 8;
                smallPoint.Fill = Brushes.Black;
                smallPoint.StrokeThickness = 1;
                smallPoint.Stroke = Brushes.Black;
                Canvas.SetLeft(smallPoint, x_oc - 3);
                Canvas.SetTop(smallPoint, y_oc - 3);

                TextBlock smallPointNumber = new TextBlock();
                smallPointNumber.Text = i.ToString();
                smallPointNumber.RenderTransform = new TranslateTransform
                {
                    X = (r + 10) * Math.Cos(angle) + x_m,
                    Y = (r + 15) * Math.Sin(angle) + y_m - 8
                };

                for (int j = i; j <= AdjacencyArray.GetLength(0); j++)
                {
                    if (AdjacencyArray[i - 1, j - 1] != 0)
                    {
                        var angle_2 = (2 * Math.PI) / num_v * j;

                        var x_oc_2 = r * Math.Cos(angle_2) + x_m;   //x on cirlce
                        var y_oc_2 = r * Math.Sin(angle_2) + y_m;   //y on circle

                        Line myLine = new Line();
                        myLine.Stroke = Brushes.Black;
                        myLine.StrokeThickness = 3;
                        myLine.X1 = x_oc;
                        myLine.Y1 = y_oc;
                        myLine.X2 = x_oc_2;
                        myLine.Y2 = y_oc_2;

                        MyCanvas.Children.Add(myLine);
                    }
                }

                MyCanvas.Children.Add(smallPoint);
                MyCanvas.Children.Add(smallPointNumber);

            }
        }

        //budujemy graf hamiltonowski o zadanej liczbie krawedzi
        public void Display(Canvas MyCanvas, int v)
        {
            //cykl eulera
            int[] done = new int[v];
            for (int i = 0; i < v; i++)
            {
                done[i] = 0;
            }
            int ile = 1;
            Random r = new Random();
            int aktualny = r.Next(0, v);
            int pierwszy = aktualny;
            done[aktualny] = 1;
            int nowy;
            while (ile != v)
            {
                nowy = r.Next(0, v);
                while (nowy != aktualny && done[nowy] == 0)
                {
                    AdjacencyArray[nowy, aktualny] = 1;
                    AdjacencyArray[aktualny, nowy] = 1;
                    aktualny = nowy;
                    ile++;
                    done[nowy] = 1;

                }

            }
            AdjacencyArray[pierwszy, aktualny] = 1;
            AdjacencyArray[aktualny, pierwszy] = 1;

            //dodajemy kolejne krawedzie
            double ilemax = v * (v - 1) / 2;
            double p = r.Next(35, 81);
            double ilemabyc = ilemax * p / (double)100.0;
            int iledodac = (int)ilemabyc - v;
            if (iledodac > 0)
            {
                ile = 0;
                while (ile != iledodac)
                {
                    aktualny = r.Next(0, v);
                    nowy = r.Next(0, v);
                    while (nowy != aktualny && AdjacencyArray[nowy, aktualny] == 0)
                    {
                        AdjacencyArray[nowy, aktualny] = 1;
                        AdjacencyArray[aktualny, nowy] = 1;
                        ile++;
                    }
                }
            }


            //wyswietlanie
            DrawGraph(AdjacencyArray.GetLength(0), MyCanvas);
        }






    }
}
