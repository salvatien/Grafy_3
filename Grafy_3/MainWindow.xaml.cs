using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grafy_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private AdjacencyMatrix adjacencyMatrix;
        private int w = 0;
        private int s = 0;
        private int no = 1;
        private int used = 0;



        public MainWindow()
        {
            InitializeComponent();
        }

       

        //budujemy graf eulerowski i znajdujemy cykl
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            

            //budujemy
            if (NumberV.Text != "0")
            {
                var howManyVertex = Int32.Parse(NumberV.Text);
                NumberV.Background = Brushes.White;
                int maxDegree = howManyVertex - 1;
                int maxRand = maxDegree / 2;

                Random randNum = new Random();
                int[] TabOfInt = new int[howManyVertex];            //tablica do przechowywania stopni wierzcholkow
                for (int i = 0; i< howManyVertex; i++)
                {
                    TabOfInt[i] = randNum.Next(1, maxRand+1) * 2;           // wypelniamy tablice wartosciami (tylko liczby parzyste!)
                }


                AdjacencyMatrix adjacencyMatrix = new AdjacencyMatrix(TabOfInt.Length);
                adjacencyMatrix.Display(MyCanvas, TabOfInt, true, false);

                //sprawdzanie NSS
                int num_of_v = TabOfInt.Length;
                int v = num_of_v;

                Stack<int> stos = new Stack<int>();
                int cn = 0;
                int[] c = new int[v];
                for (int i = 0; i < v; i++)
                {
                    c[i] = 0;
                }
                for (int i = 0; i < v; i++)
                {
                    if (c[i] > 0)
                    {
                        continue;
                    }
                    cn++;
                    stos.Push(i);
                    c[i] = cn;
                    while (stos.Count > 0)
                    {
                        int vv = stos.Pop();
                        List<int> neighbours = new List<int>();
                        for (int j = 0; j < v; j++)
                        {
                            if (adjacencyMatrix.AdjacencyArray[vv, j] == 1)
                            {
                                neighbours.Add(j);
                            }
                        }
                        for (int j = 0; j < neighbours.Count; j++)
                        {
                            if (c[neighbours[j]] > 0)
                            {
                                continue;
                            }
                            stos.Push(neighbours[j]);
                            c[neighbours[j]] = cn;
                        }




                    }


                    if (c.Count(x => x == cn) == v)
                        break;
                }

                int max = 0, maxval = 0;
                for (int i = 1; i <= cn; i++)
                {
                    if (c.Count(x => x == i) > max)
                    {
                        max = c.Count(x => x == i);
                        maxval = i;
                    }
                }

                string ciag = "";
                List<int> doWypisania = new List<int>();
                int ile = 0;
                for (int i = 0; i < v; i++)
                {
                    if (c[i] == maxval)
                    {
                        doWypisania.Add(i);
                        ile++;
                    }
                }

                for (int i = 0; i < num_of_v; i++)
                {
                    for (int j = i + 1; j < num_of_v; j++)
                    {
                        if (doWypisania.Contains(i) && doWypisania.Contains(j))
                        {
                            //////////////////////////
                        }
                        else
                        {
                            adjacencyMatrix.AdjacencyArray[i, j] = 0;
                            adjacencyMatrix.AdjacencyArray[j, i] = 0;
                        }
                    }
                }

                adjacencyMatrix.DrawGraph(num_of_v, MyCanvas);






                //cykl
                Random r = new Random();
                string OutPut;
                Stack stosik = new Stack();
                int vvv = r.Next(0, num_of_v);
                Euler(0, stosik, num_of_v, adjacencyMatrix.AdjacencyArray);

                //wypisz na ekran
                object[] tab = stosik.ToArray();
                string wypisz = "";
                for (int i = 0; i < tab.Length; i++)
                {
                    wypisz = wypisz + tab[i].ToString() + ", ";
                }
                NumberV.Text = wypisz;
            }
            else
            {
                NumberV.Background = Brushes.OrangeRed;
            }


        }

        //znajdowanie cyklu eulera
        public void Euler(int v, Stack stos, int num_of_v, int[,] AdjacencyArray)
        {
            for (int i = 0; i < num_of_v; i++)
            {
                if (AdjacencyArray[v, i] == 1)
                {
                    AdjacencyArray[v, i] = 0;
                    AdjacencyArray[i, v] = 0;
                    Euler(i, stos, num_of_v, AdjacencyArray);
                }
            }
            stos.Push(v + 1);
        }

        /*
        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            var v = Int32.Parse(Ver.Text);
            var k = Int32.Parse(kkk.Text);
            int[] TabOfInt = new int[v];
            for (int i = 0; i < v; i++)
            {
                TabOfInt[i] = (int)k;
            }


            //Ta zmienna mówi nam czy ciąg jest grafuiczny czy nie jest
            bool TrueOrFalse = false;
            //znacznik konca petli
            bool EndLoop = false;
            //idneks ostatniego elementu tablicy
            int max = TabOfInt.Length - 1;

            //tu się dzieja rzeczy
            while (EndLoop == false)
            {
                //bierzemy ostani element i sprawdzamy czy nie jest on za duzy jak np. w ciagu 1,2,8 
                //bo wtedy nie mozemy go odjac i z miejsca taki ciag nie jest graficzny
                Array.Sort(TabOfInt);
                int value = TabOfInt[max];
                if (value > max)
                {
                    TrueOrFalse = false;
                    EndLoop = true;
                }
                //jesli sie zgadza wszytko to po kolei odejmujemy i ustawiamy value na 0
                else
                {
                    for (int i = max - 1; i >= max - value; i--)
                    {
                        TabOfInt[i]--;
                    }
                    TabOfInt[max] = 0;
                    //sortowanie
                    Array.Sort(TabOfInt);
                    //jesli ostatnia, czyli najwieksza liczba, to 0, to znaczy ze jest koniec
                    //tutaj zaznaczamy EndLoop na true (konczymy petle)
                    //zakladamy tez ze mamy same 0 - wtedy ciag jest graficzny - czyli TrueOrFalse = true
                    //potem w petli for sprawdzamy czy nie mamy jakiego (-1) - ciag nie jest graficzny
                    if (TabOfInt[max] == 0)
                    {
                        EndLoop = true;
                        TrueOrFalse = true;
                        for (int i = 0; i <= max; i++)
                        {
                            if (TabOfInt[i] < 0) TrueOrFalse = false;
                        }
                    }
                }

            }

            //wyswietlamy odpowiedni komunikat w miejscu, gdzie byl ciag
            if (TrueOrFalse == true)
            {
                kkk.Background = Brushes.White;
                Ver.Background = Brushes.White;

                for (int i = 0; i < v; i++)
                {
                    TabOfInt[i] = (int)k;
                }

                AdjacencyMatrix adjacencyMatrix = new AdjacencyMatrix(TabOfInt.Length);
                adjacencyMatrix.Display(MyCanvas, TabOfInt, true, true);


            }


            else
            {
                kkk.Background = Brushes.OrangeRed;
                Ver.Background = Brushes.OrangeRed;
            }



        }

    */
     
    }
}
