using Galileo6;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
// Patrick Wheatley
// 30053724
// 6/02/2023
// A .NET Multi platform APP UI which uses the Galileo.DLL
namespace GalileoSensorApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillCBX(cbxMu, 35, 75, 50);
            FillCBX(cbxSigma, 10, 20, 10);
        }

        // Initializing the LinkedLists for Sensor A and Sensor B. (4.1)
        private LinkedList<double> sensorAList = new LinkedList<double>();
        private LinkedList<double> sensorBList = new LinkedList<double>();


        private void FillCBX(ComboBox cbx, int Min, int Max, int dft)
        {
            cbx.Items.Clear();
            for (int i = Min; i <= Max; i++)
            {
                cbx.Items.Add(i.ToString());
            }
            cbx.Text = dft.ToString();
        }

        // 4.2
        private void LoadData()
        {
            const int MaxSize = 400; // Hard coding the size of the linked list. 
            ReadData readData = new ReadData(); // Instance of "ReadData" from Galileo6.DLL
            sensorAList.Clear();
            sensorBList.Clear();
            for (int i = 0; i < MaxSize; i++) // A for loop to populate the LinkedLists with the data.
            {
                sensorAList.AddFirst(readData.SensorA(int.Parse(cbxMu.Text), int.Parse(cbxSigma.Text))); // Adding the data to the LinkedLists by reading the data from the DLL. (mu, sigma)
                sensorBList.AddFirst(readData.SensorB(int.Parse(cbxMu.Text), int.Parse(cbxSigma.Text)));
            }
        }

        // 4.3
        private void ShowAllSensorData()
        {
            LVHolder.Items.Clear(); // Clearing the data if the user accidentally clicked the button more than once. 
            int MaxSize = NumberOfNodes(sensorAList); // The size of each of the sensors.
            for (int i = 0; i < MaxSize; i++) // Using a for loop to iterate through the sensors.
            {
                LVHolder.Items.Add(new
                {
                    sensorA = sensorAList.ElementAt(i), // Adding the data to the correct Columns via binding. 
                    sensorB = sensorBList.ElementAt(i)
                });
            }
        }

        // 4.4
        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            // Loads the data and displays the raw data / data in the listbox.
            LoadData();
            ShowAllSensorData();
            DisplayListBoxData(LBSensorA, sensorAList);
            DisplayListBoxData(LBSensorB, sensorBList);
        }

        // 4.5
        private int NumberOfNodes(LinkedList<double> list)
        {
            return list.Count; // Returning the amount of nodes there are in the LinkedList that has been passed through.
        }
        // 4.6
        private void DisplayListBoxData(ListBox LB, LinkedList<double> LL)
        {
            LB.Items.Clear();
            foreach (double item in LL) // foreach loop since im going through a LinkedList.
            {
                LB.Items.Add(item); // Adding the items to the ListBox.
            }
        }

        // 4.7
        private bool SelectionSort(LinkedList<double> LL)
        {
            Stopwatch sw = new Stopwatch();  // stop watch to count how long it takes to sort.
            bool fin = true;
            int min = 0;
            int max = NumberOfNodes(LL);
            sw.Reset();
            sw.Start();
            for (int i = 0; i < max; i++)
            {
                min = i;
                for (int j = i + 1; j < max; j++)
                {
                    if (LL.ElementAt(j) < LL.ElementAt(min))
                    {
                        min = j;
                    }
                }

                LinkedListNode<double> currentMin = LL.Find(LL.ElementAt(min));
                LinkedListNode<double> currentI = LL.Find(LL.ElementAt(i));

                var temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }
            sw.Stop();
            tbxSelectionSortTime.Text = sw.ElapsedTicks.ToString() + " Ticks.";
            DisplayListBoxData(LBSensorA, sensorAList);
            return fin;

        }


        // 4.7
        private void btnSelectionSort_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorAList);
            
        }

        private void tbxSelectionSortTime_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // 4.8
        private bool InsertionSort(LinkedList<double> LL)
        {
            Stopwatch sw = Stopwatch.StartNew(); // Stop watch so measure how long it takes.
            sw.Reset();
            sw.Start();
            bool swapped = true;
            int max = NumberOfNodes(LL);
            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (LL.ElementAt(j - 1) > LL.ElementAt(j))
                    {
                        LinkedListNode<double> current = LL.Find(LL.ElementAt(j));
                        LinkedListNode<double> previous = LL.Find(LL.ElementAt(j - 1));
                        var temp = previous.Value;
                        previous.Value = current.Value;
                        current.Value = temp;
                    }
                }
            }
            sw.Stop();
            tbxInsertionSortTime.Text = sw.ElapsedTicks.ToString() + " Ticks.";
            DisplayListBoxData(LBSensorA, sensorAList);
            return swapped;

        }

        // 4.8
        private void btnInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(sensorAList);
            
        }

        // 4.9
        private int BinarySearchIterative(LinkedList<double> LL, double SearchValue, int Minimum, int Maximum)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            while (Minimum <= Maximum - 1)
            {
                int mid = (Minimum + Maximum) / 2;

                if (SearchValue == LL.ElementAt(mid))
                {
                    sw.Stop();
                    tbxBinarySearchIterative.Text = sw.ElapsedTicks.ToString() + " Ticks.";
                    return mid;
                }
                else if (SearchValue < LL.ElementAt(mid))
                {
                    Maximum = mid - 1;
                }
                else
                {
                    Minimum = mid + 1;
                }

            }
            sw.Stop();
            tbxBinarySearchIterative.Text = sw.ElapsedTicks.ToString() + " Ticks.";
            return Minimum;
        }

        // 4.9
        private void btnBinarySearchIterative_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionSort(sensorAList))
            {
                try
                {
                    double searchItem = Double.Parse(tbxSearchItemA.Text);
                    int value = BinarySearchIterative(sensorAList, searchItem, 0, NumberOfNodes(sensorAList));
                    MessageBox.Show("Your search was found at index " + value.ToString() + " for Sensor A.", "Search Found.", MessageBoxButton.OK, MessageBoxImage.Information);
                    LBSensorA.SelectedIndex = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter in a valid search number.", "Searching", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 4.10
        private int BinarySearchRecursive(LinkedList<double> LL, double SearchValue, int Minimum, int Maximum )
        {

            if (Minimum< Maximum - 1)
            {
                int mid = (Minimum + Maximum) / 2;
                if (SearchValue == LL.ElementAt(mid))
                {
                    return mid;
                }
                else if (SearchValue < LL.ElementAt(mid))
                {
                    return BinarySearchRecursive(LL, SearchValue, Minimum, mid - 1);
                }
                else
                {
                    return BinarySearchRecursive(LL, SearchValue, mid + 1, Maximum);
                }
            }
            return Minimum;
        }

        // 4.10
        private void btnBinarySearchRecursive_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionSort(sensorAList))
            {
                try
                {
                    double searchItem = Double.Parse(tbxSearchItemA.Text);
                    Stopwatch sw = new Stopwatch();
                    sw.Reset();
                    sw.Start();
                    int value = BinarySearchRecursive(sensorAList, searchItem, 0, NumberOfNodes(sensorAList));
                    sw.Stop();
                    tbxBinarySearchRecursive.Text = sw.ElapsedTicks.ToString() + " Ticks.";
                    MessageBox.Show("Your search was found at index " + value.ToString() + " for Sensor A.", "Search Found.", MessageBoxButton.OK, MessageBoxImage.Information);
                    LBSensorA.SelectedIndex = value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter in a valid search number.", "Searching", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void tbxSearchItemA_SelectionChanged(object sender, RoutedEventArgs e)
        {
            tbxSearchItemA.Clear();
        }
    }
}
