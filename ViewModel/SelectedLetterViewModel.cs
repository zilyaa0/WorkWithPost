using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithPost.Models;
using WorkWithPost.MVVMTools;

namespace WorkWithPost.ViewModel
{
    public class SelectedLetterViewModel : ViewModelBase
    {
        #region Fields
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
        #endregion

        #region Constructor
        public SelectedLetterViewModel(Letter selectedLetter)
        {
            //Sender = selectedLetter.Sender;
            //Header = selectedLetter.Headers;
            //Text = selectedLetter.Text;
        }
        #endregion
    }
}
