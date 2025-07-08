using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TuDus.CustomElements;
using TuDus.Models;

namespace TuDus
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<TuDuItem> TuDuCollection { get; set; }

        private static readonly double CHILD_INDENT_LEVEL = 40d;

        private const string DATA_FILE_NAME = "data.json";

        private bool _isLoadingData = false;

        public MainWindow()
        {
            InitializeComponent();

            TuDuCollection = new ObservableCollection<TuDuItem>();
            MyItemsControl.ItemsSource = TuDuCollection;

            LoadData();

            MyItemsControl.AddHandler(TuDuObject.AddSubItemRequestedEvent, new RoutedEventHandler(OnAddSubItemRequested));

            TuDuCollection.CollectionChanged += MyItems_CollectionChanged;
            foreach (var item in TuDuCollection)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }

            this.Closed += MainWindow_Closed;
        }

        private void TuDuObject_Selected(object sender, RoutedEventArgs e) { }

        private void AddElement_Click(object sender, RoutedEventArgs e)
        {
            TuDuItem newtudu = new TuDuItem
            {
                Description = "Nowe TuDu",
                Tag = "",
                Date = "",
                IsSelected = false,
                IndentLevel = 0,
                RawData = "[ ] Nowe TuDu",
            };
            TuDuCollection.Add(newtudu);

            e.Handled = true;
        }

        private void OnAddSubItemRequested(object sender, RoutedEventArgs e)
        {
            TuDuObject? sourceControl = FindAncestor<TuDuObject>((DependencyObject)e.OriginalSource);

            if (sourceControl != null)
            {
                if (sourceControl.DataContext is TuDuItem clickedItem)
                {
                    int clickedIndex = TuDuCollection.IndexOf(clickedItem);

                    if (clickedIndex != -1)
                    {
                        var newItem = new TuDuItem
                        {
                            Description = "Sub-element",
                            Tag = "",
                            Date = "",
                            IsSelected = false,
                            IndentLevel = clickedItem.IndentLevel + CHILD_INDENT_LEVEL,
                            RawData = "[ ] Sub-element",
                        };

                        int insertIndex = clickedIndex + 1;
                        while (insertIndex < TuDuCollection.Count && TuDuCollection[insertIndex].IndentLevel >= clickedItem.IndentLevel + CHILD_INDENT_LEVEL)
                        {
                            insertIndex++;
                        }
                        TuDuCollection.Insert(insertIndex, newItem);
                    }
                }
            }

            e.Handled = true;
        }

        private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private int GetItemNumber(string text)
        {
            var match = System.Text.RegularExpressions.Regex.Match(text, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int number))
            {
                return number;
            }
            return 0;
        }

        private void LoadData()
        {
            _isLoadingData = true;
            try
            {
                if (File.Exists(DATA_FILE_NAME))
                {
                    string json = File.ReadAllText(DATA_FILE_NAME);
                    var loadedItems = JsonConvert.DeserializeObject<ObservableCollection<TuDuItem>>(json);
                    TuDuCollection.Clear();
                    foreach (var item in loadedItems)
                    {
                        TuDuCollection.Add(item);
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania danych: {ex.Message}", "Błąd ładowania", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _isLoadingData = false;
            }
        }

        private void SaveData()
        {
            if (_isLoadingData) return;

            try
            {
                string json = JsonConvert.SerializeObject(TuDuCollection, Formatting.Indented);
                File.WriteAllText(DATA_FILE_NAME, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisu danych: {ex.Message}", "Błąd zapisu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MyItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (TuDuItem item in e.NewItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (TuDuItem item in e.OldItems)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }
            SaveData();
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveData();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            SaveData();
        }
    }
}
