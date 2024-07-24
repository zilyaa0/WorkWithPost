using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        }
        #endregion

        #region Public Methods
        public async Task Load()
        {
            FilesNames = await _apiService.GetFilesNames(Letter.UniqueId);
        }
        #endregion

        #region Commands
        public ICommand OpenFile
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    SelectedFile = new FileName()
                    {
                        Name = obj as string
                    };
                    await OpenSelectedFile();
                });
            }
        }

        public ICommand CloseWin
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Window window = obj as Window;
                    window.Close();
                });
            }
        }
        #endregion

        #region Private Methods
        private async Task OpenSelectedFile()
        {
            await _apiService.LoadFile(Letter.UniqueId, SelectedFile.Name);
        }
        #endregion
    }
}
