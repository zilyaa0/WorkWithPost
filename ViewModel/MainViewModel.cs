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
using WorkWithPost.MVVMTools;
using WorkWithPost.Models;
using WorkWithPost.Services;
using WorkWithPost.Views;
using System.Diagnostics;

namespace WorkWithPost.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        #region Fields
        private readonly IApiService _apiService;

        private List<Letter> _letters;
        public List<Letter> Letters
        {
            get { return _letters; }
            set
            {
                _letters = value;
                OnPropertyChanged("Letters");
            }
        }

        private string _sender;
        public string Sender
        {
            get { return _sender; }
            set
            {
                _sender = value;
                OnPropertyChanged("Sender");
            }
        }

        private string _headers;
        public string Header
        {
            get { return _headers; }
            set
            {
                _headers = value;
                OnPropertyChanged("Header");
            }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged("SearchString");
            }
        }

        private int _limit;
        public int Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                OnPropertyChanged("Limit");
            }
        }

        private int _page;
        public int Page
        {
            get { return _page; }
            set
            {
                _page = value;
                OnPropertyChanged("Page");
            }
        }

        private int _pageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
                OnPropertyChanged("PageCount");
            }
        }

        private int _totalCount;
        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                OnPropertyChanged("TotalCount");
            }
        }

        private Letter _selectedLetter;
        public Letter SelectedLetter
        {
            get { return _selectedLetter; }
            set
            {
                _selectedLetter = value;
                OnPropertyChanged("SelectedLetter");
            }
        }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            _apiService = new ApiService();
            SearchString = "Строка поиска";
            Page = 1;
            Limit = 5;
        }
        #endregion

        #region Commands
        public ICommand GetLettersByQuery
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    Page = 1;
                    await GetLetters();
                });
            }
        }

        public ICommand NextPage
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    Page++;
                    await GetLetters();
                }, (obj) => Page < PageCount);
            }
        }

        public ICommand PreviousPage
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    Page--;
                    await GetLetters();
                }, (obj) => Page > 1);
            }
        }

        public ICommand ShowSelectedLetterWindow
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    var letter = SelectedLetter;
                    var win = new SelectedLetterWindow();
                    win.DataContext = new SelectedLetterViewModel(letter);
                    win.Show();
                });
            }
        }

        public ICommand GotFocusCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    SearchString = "";
                }, (obj) => SearchString == "Строка поиска");
            }
        }

        public ICommand LostFocusCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    SearchString = "Строка поиска";
                }, (obj) => SearchString == "");
            }
        }
        #endregion

        #region Private Methods
        private async Task GetLetters()
        {
            LettersQueryResult result;
            if (SearchString == "Строка поиска")
                result = await _apiService.GetAllLetters(Page, Limit);
            else
                result = await _apiService.GetLetters(SearchString, Page, Limit);
            if (result != null)
            {
                foreach (var item in result.Letters)
                {
                    item.Text = item.Text.Replace(Environment.NewLine, " ");
                }
                Letters = result.Letters;
                PageCount = (int)Math.Ceiling(Convert.ToSingle(result.TotalCount) / Convert.ToSingle(Limit));
                TotalCount = result.TotalCount;
            }
        }
        #endregion
    }
}
