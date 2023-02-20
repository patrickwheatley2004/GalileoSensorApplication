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


        // 4.13
        private void FillCBX(ComboBox cbx, int Min, int Max, int dft)
        {
            // filling the combo box with numbers as a substitue for a NUD Box.
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
            Random randy = new Random();
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
            bool fin = true;
            int min = 0;
            int max = NumberOfNodes(LL);
            for (int i = 0; i < max; i++)
            {
                min = i;
                for (int j = i + 1; j < max; j++) 
                {
                    if (LL.ElementAt(j) < LL.ElementAt(min)) // if j is less than min, min is equal to j.
                    {
                        min = j;
                    }
                }

                // SWAP.
                LinkedListNode<double> currentMin = LL.Find(LL.ElementAt(min));
                LinkedListNode<double> currentI = LL.Find(LL.ElementAt(i));

                var temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }

            return fin;

        }


        // 4.12
        private void btnSelectionSort_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch(); // using a stopwatch to count how long the sort takes. 
            sw.Reset();
            sw.Start();
            SelectionSort(sensorAList);
            sw.Stop();
            tbxSelectionSortTime.Text = sw.ElapsedTicks.ToString() + " Ticks."; // telling the user how long the sort took. 
            ShowAllSensorData();
            DisplayListBoxData(LBSensorA, sensorAList);
        }

        private void tbxSelectionSortTime_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // 4.12
        private bool InsertionSort(LinkedList<double> LL)
        {
            
            bool swapped = true;
            int max = NumberOfNodes(LL);
            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (LL.ElementAt(j - 1) > LL.ElementAt(j)) // checking if j-1 is more than j
                    {
                        // SWAP.
                        LinkedListNode<double> current = LL.Find(LL.ElementAt(j));
                        LinkedListNode<double> previous = LL.Find(LL.ElementAt(j - 1));
                        var temp = previous.Value;
                        previous.Value = current.Value;
                        current.Value = temp;
                    }
                }
            }

            return swapped;

        }

        // 4.8
        private void btnInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch(); // using a stopwatch to count how long the sort takes.
            sw.Reset();
            sw.Start();
            InsertionSort(sensorAList);
            sw.Stop();
            tbxInsertionSortTime.Text = sw.ElapsedTicks.ToString() + " Ticks."; // displaying how long the sort tooks. 
            ShowAllSensorData();
            DisplayListBoxData(LBSensorA, sensorAList);
        }

        // 4.9
        private int BinarySearchIterative(LinkedList<double> LL, double SearchValue, int Minimum, int Maximum)
        {
            // binary search.
            // trying to match the mid to our searched for value. 
            // using a loop to try match the mid to our search value. 
            while (Minimum <= Maximum - 1)
            {
                int mid = (Minimum + Maximum) / 2;

                if (SearchValue == LL.ElementAt(mid))
                {
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
            return Minimum;
        }

        // 4.11
        private void btnBinarySearchIterative_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionSort(sensorAList))
            {
                try // using a try-catch to parse the search value in the search textbox to an integer. 
                {
                    double searchItem = Double.Parse(tbxSearchItemA.Text);
                    Stopwatch sw = new Stopwatch(); // a stopwatch to count how long the search takes.
                    sw.Reset();
                    sw.Start();
                    int value = BinarySearchIterative(sensorAList, searchItem, 0, NumberOfNodes(sensorAList));
                    sw.Stop();
                    tbxBinarySearchIterative.Text = sw.ElapsedTicks.ToString() + " Ticks."; // displaying how long the search took.
                    DisplayListBoxData(LBSensorA, sensorAList);
                    MessageBox.Show("Your search was found at index " + value.ToString() + " for Sensor A.", "Iterative Search Found A.", MessageBoxButton.OK, MessageBoxImage.Information); // displaying the search found information to the user.
                    Highlight(value, LBSensorA, sensorAList); // highlighting the searched for value.
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter in a valid search number.", "Iterative Searching A", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 4.10
        private int BinarySearchRecursive(LinkedList<double> LL, double SearchValue, int Minimum, int Maximum )
        {
            // binary search.
            // we are trying to match the mid to our search value. 
            // instead of using a loop we are using a recursive method which means the method is called over and over until the mid is equal to our searched value. 
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

        // 4.11
        private void btnBinarySearchRecursive_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionSort(sensorAList))
            {
                try // using a try-catch to parse the item in the search textbox as an integer.
                {
                    double searchItem = Double.Parse(tbxSearchItemA.Text);
                    Stopwatch sw = new Stopwatch(); // starting the stopwatch to count how long the search took.
                    sw.Reset();
                    sw.Start();
                    int value = BinarySearchRecursive(sensorAList, searchItem, 0, NumberOfNodes(sensorAList));
                    sw.Stop();
                    tbxBinarySearchRecursive.Text = sw.ElapsedTicks.ToString() + " Ticks.";// displaying how long the search took.
                    DisplayListBoxData(LBSensorA, sensorAList);
                    MessageBox.Show("Your search was found at index " + value.ToString() + " for Sensor A.", "Recursive Search Found A.", MessageBoxButton.OK, MessageBoxImage.Information); // telling the user that their search was found.
                    Highlight(value, LBSensorA, sensorAList); // highlighting the searched for value.
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter in a valid search number.", "Recursive Searching A", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 4.11
        private void Highlight(int valueindex, ListBox listBox, LinkedList<double> LL)
        {
            listBox.Items.Clear(); // clearing the listbox.
            foreach (double data in LL)
            {
                listBox.Items.Add(data); // adding the items back to the listbox. 
            }

            for (int i = valueindex - 2; i < valueindex + 3; i++)
            {
                if (i > -1 && i < NumberOfNodes(LL)) // checking if the index is below 0 or over the maximum.
                {
                    listBox.SelectedItems.Add(listBox.Items.GetItemAt(i)); // selecting the searched values + values close to the selected one.
                }
                
            }

        }

        private void tbxSearchItemA_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void tbxSearchItemA_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // clearing the textboxes whenever the mouse hovers over. 
            tbxSearchItemA.Text = "";
            tbxSearchItemB.Text = "";
        }
        // 4.11
        private void btnBinarySearchIterativeB_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionSort(sensorBList))
            {
                try // using a try-catch to parse the item in the search textbox to an integer.
                {
                    double searchItem = Double.Parse(tbxSearchItemB.Text);
                    Stopwatch sw = new Stopwatch(); // starting a stopwatch to count how long the search took. 
                    sw.Reset();
                    sw.Start();
                    int value = BinarySearchIterative(sensorBList, searchItem, 0, NumberOfNodes(sensorBList));
                    sw.Stop();
                    tbxBinarySearchIterativeB.Text = sw.ElapsedTicks.ToString() + " Ticks."; // displaying the search.
                    DisplayListBoxData(LBSensorB, sensorBList);
                    MessageBox.Show("Your search was found at index " + value.ToString() + " for Sensor B.", "Iterative Search Found B.", MessageBoxButton.OK, MessageBoxImage.Information); // telling the user that their sort was found.
                    Highlight(value, LBSensorB, sensorBList); // highlighting the instances found in the listbox. 
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter in a valid search number.", "Iterative Searching B", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // 4.11
        private void btnBinarySearchRecursiveB_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionSort(sensorBList))
            {
                try // using a try-catch to parse the item in the search textbox to an integer.
                {
                    double searchItem = Double.Parse(tbxSearchItemB.Text);
                    Stopwatch sw = new Stopwatch(); // starting a stopwatch to count how long the search took. 
                    sw.Reset();
                    sw.Start();
                    int value = BinarySearchRecursive(sensorBList, searchItem, 0, NumberOfNodes(sensorBList));
                    sw.Stop();
                    tbxBinarySearchRecursiveB.Text = sw.ElapsedTicks.ToString() + " Ticks."; // displaying the search.
                    DisplayListBoxData(LBSensorB, sensorBList);
                    MessageBox.Show("Your search was found at index " + value.ToString() + " for Sensor B.", "Recursive Search Found B.", MessageBoxButton.OK, MessageBoxImage.Information); // telling the user that their sort was found.
                    Highlight(value, LBSensorB, sensorBList); // highlighting the instances found in the listbox.
                }
                catch (Exception)
                {
                    MessageBox.Show("Please enter in a valid search number.", "Recursive Searching B", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // 4.12
        private void btnSelectionSortB_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch(); // starting the stopwatch to count how long the sort takes
            sw.Reset();
            sw.Start();
            SelectionSort(sensorBList);
            sw.Stop();
            tbxSelectionSortTimeB.Text = sw.ElapsedTicks.ToString() + " Ticks."; // displaying how long the sort took.
            ShowAllSensorData();
            DisplayListBoxData(LBSensorB, sensorBList);
        }

        // 4.12
        private void btnInsertionSortB_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch(); // starting the stopwatch to count how long the sort takes.
            sw.Reset();
            sw.Start();
            InsertionSort(sensorBList);
            sw.Stop();
            tbxInsertionSortTimeB.Text = sw.ElapsedTicks.ToString() + " Ticks."; // displaying how long the sort took.
            ShowAllSensorData();
            DisplayListBoxData(LBSensorB, sensorBList);
        }


        // 4.14
        private void tbxSearchItemA_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // 4.14
        private void tbxSearchItemB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
