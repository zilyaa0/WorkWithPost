using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkWithPost.Commands;
using WorkWithPost.Models;
using WorkWithPost.Services;

namespace WorkWithPost.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private List<Letter> _Letters;
        private string _Sender;
        private string _Headers;
        private string _Text;
        private string _SearchString;
        private int _Limit;
        private int _Page;
        private int _PageCount;
        private int _TotalCount;
        private ApiService apiService;

        public MainViewModel()
        {
            apiService = new ApiService();
            Page = 1;
            Limit = 5;
        }

        public List<Letter> Letters
        {
            get { return _Letters; }
            set
            {
                _Letters = value;
                OnPropertyChanged("Letters");
            }
        }

        public string Sender
        {
            get { return _Sender; }
            set
            {
                _Sender = value;
                OnPropertyChanged("Sender");
            }
        }

        public string Header
        {
            get { return _Headers; }
            set
            {
                _Headers = value;
                OnPropertyChanged("Header");
            }
        }

        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                OnPropertyChanged("Text");
            }
        }

        public string SearchString
        {
            get { return _SearchString; }
            set
            {
                _SearchString = value;
                OnPropertyChanged("SearchString");
            }
        }

        public int Limit
        {
            get { return _Limit; }
            set
            {
                _Limit = value;
                OnPropertyChanged("Limit");
            }
        }

        public int Page
        {
            get { return _Page; }
            set
            {
                _Page = value;
                OnPropertyChanged("Page");
            }
        }

        public int PageCount
        {
            get { return _PageCount; }
            set
            {
                _PageCount = value;
                OnPropertyChanged("PageCount");
            }
        }

        public int TotalCount
        {
            get { return _TotalCount; }
            set
            {
                _TotalCount = value;
                OnPropertyChanged("TotalCount");
            }
        }

        public ICommand GetLettersByQuery
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    var result = await apiService.GetLetters(SearchString, Page, Limit);
                    if (result != null)
                    {
                        Letters = result.Letters;
                        PageCount = (int)Math.Ceiling(Convert.ToSingle(result.TotalCount) / Convert.ToSingle(Limit));
                        TotalCount = result.TotalCount;
                    }
                });
            }
        }

        public ICommand NextPage
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Page++;
                    GetLettersByQuery.Execute(obj);
                }, (obj) => Page < PageCount);
            }
        }

        public ICommand PreviousPage
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Page--;
                    GetLettersByQuery.Execute(obj);
                }, (obj) => Page > 1);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
