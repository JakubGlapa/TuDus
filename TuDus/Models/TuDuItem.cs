
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TuDus.Parser;

namespace TuDus.Models
{
    public enum Priority
    {
        None = 0,
        Low = 10,
        Medium = 20,
        High = 30,
    }

    public class TuDuItem : INotifyPropertyChanged
    {
        private bool _isCheckboxActive;

        private bool _isSelected;

        private string _description = string.Empty;

        private string _tag = string.Empty;

        private string _dueDate = string.Empty;

        private double _indentLevel = 0d;

        private string _rawData = string.Empty;


        public string RawData
        {
            get {  return _rawData; }
            set
            {
                if (_rawData != value)
                {
                    _rawData = value;
                    OnPropertyChanged(nameof(RawData));
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string Tag
        {
            get { return _tag; }
            set
            {
                if (_tag != value)
                {
                    _tag = value;
                    OnPropertyChanged(nameof(Tag));
                }
            }
        }

        public string Date
        {
            get { return _dueDate; }
            set
            {
                if (_dueDate != value)
                {
                    _dueDate = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        public double IndentLevel
        {
            get { return _indentLevel; }
            set
            {
                if (_indentLevel != value)
                {
                    _indentLevel = value;
                    OnPropertyChanged(nameof(IndentLevel));
                }
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public bool IsCheckboxActive
        {
            get { return _isCheckboxActive; }
            set
            {
                if (_isCheckboxActive != value)
                {
                    _isCheckboxActive = value;
                    OnPropertyChanged(nameof(IsCheckboxActive));
                }
            }
        }

        public void UpdateData()
        {
            ParseRawData(_rawData);
        }

        protected void OnPropertyChanged(string propertyName) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ParseRawData(string input)
        {
            var item = TuDuParser.Parse(input);
            RawData = input;
            Description = item.Description;
            Tag = item.Tag;
            Date = item.Date;
            IsSelected = item.IsSelected;
            IsCheckboxActive = item.IsCheckboxActive;
        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
    }
}
