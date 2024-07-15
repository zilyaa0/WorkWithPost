using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WorkWithPost.Models;
using WorkWithPost.MVVMTools;
using WorkWithPost.Services;

namespace WorkWithPost.ViewModel
{
    public class FileName
    {
        public string Name { get; set; }
    }
    public class SelectedLetterViewModel : ViewModelBase
    {
        #region Fields
        private readonly IApiService _apiService;

        private Letter _letter;
        public Letter Letter
        {
            get { return _letter; }
            set
            {
                _letter = value;
                OnPropertyChanged("Letter");
            }
        }

        private List<FileName> _files;
        public List<FileName> FilesNames
        {
            get { return _files; }
            set
            {
                _files = value;
                OnPropertyChanged("FilesNames");
            }
        }

        private FileName _selectedFile;
        public FileName SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }
        #endregion

        #region Constructor
        public SelectedLetterViewModel(Letter selectedLetter)
        {
            _apiService = new ApiService();
            Letter = selectedLetter;
            FilesNames = _apiService.GetFilesNames(selectedLetter.UniqueId).Result;
        }
        #endregion

        #region Commands
        public ICommand OpenFile
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    var cmd = Path.Combine($"C:\\Users\\Пользователь\\source\\repos\\ask2\\Emails\\", Letter.UniqueId, SelectedFile.Name);
                    var process = new Process();
                    process.StartInfo = new ProcessStartInfo(cmd)
                    {
                        UseShellExecute = true
                    };
                    process.Start();
                });
            }
        }
        #endregion
    }
}
