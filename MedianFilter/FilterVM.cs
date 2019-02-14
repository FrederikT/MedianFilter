using Microsoft.Win32;
using System.Windows;
using System.Drawing;
using System;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Input;

namespace MedianFilter
{
    class FilterVM : INotifyPropertyChanged
    {
        private Filter filter = new Filter();

        private ICommand convertButtonCommand = null;
        public ICommand ConvertButtonCommand
        {

            get
            {
                if (convertButtonCommand == null)
                {
                    convertButtonCommand = new RelayCommand(ConvertPicture, param => CanExecuteConvert(param));
                }
                return convertButtonCommand;
            }
        }

        private ICommand saveButtonCommand = null;
        public ICommand SaveButtonCommand
        {

            get
            {
                if (saveButtonCommand == null)
                {
                    saveButtonCommand = new RelayCommand(SaveElement, param => CanExecuteConvert(param));
                }
                return saveButtonCommand;
            }
        }
        private ICommand loadButtonCommand = null;
        public ICommand LoadButtonCommand
        {

            get
            {
                if (loadButtonCommand == null)
                {
                    loadButtonCommand = new RelayCommand(LoadElement, param => CanExecuteConvert(param));
                }
                return loadButtonCommand;
            }
        }


        private ImageSource originalImageSource;

        public ImageSource OriginalImageSource
        {
            get { return originalImageSource; }
            set
            {
                originalImageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OriginalImageSource"));
            }
        }

        private ImageSource filteredImageSource;

        public ImageSource FilteredImageSource
        {
            get { return filteredImageSource; }
            set
            {
                filteredImageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FilteredImageSource"));
            }
        }

        private Bitmap originalImage;

        public Bitmap OriginalImage
        {
            get { return originalImage; }
            set
            {
                originalImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OriginalImage"));
            }
        }

        private Bitmap filteredImage;

        public event PropertyChangedEventHandler PropertyChanged;

        public Bitmap FilteredImage
        {
            get { return filteredImage; }
            set
            {
                filteredImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FilteredImage"));
            }
        }


        //___________________________________________________________________________________________________________________________________
        // relevanter Code beginnt ab hier:
        //___________________________________________________________________________________________________________________________________


        public void ConvertPicture(object obj)
        {
            var paramList = (obj as object[]);
            var px = getPx((bool)paramList[0], (bool)paramList[1], (bool)paramList[2], (bool)paramList[3], (bool)paramList[4]); //die radiobutton values werden an getPx übergeben. 
            filteredImage = filter.filterPicture(px, originalImage); // Medianfilter wird auf bitmap angewendet
            FilteredImageSource = BitmapConverter.ImageSourceForBitmap(FilteredImage); // in ImageSource umwandeln. (Für Oberfläche)

        }




        public bool CanExecuteConvert(object obj)
        {
            return true;
        }

        /// <summary>
        /// Returnt "in oberfläche gewählten" radiobutton value
        /// </summary>
        /// <param name="is1">Bool: Radio button Value aus oberfläche</param>
        /// <param name="is3">Bool: Radio button Value aus oberfläche</param>
        /// <param name="is5">Bool: Radio button Value aus oberfläche</param>
        /// <param name="is7">Bool: Radio button Value aus oberfläche</param>
        /// <param name="is9">Bool: Radio button Value aus oberfläche</param>
        /// <returns></returns>
        public int getPx(bool is1, bool is3, bool is5, bool is7, bool is9)
        {
            if (is1) return 1;
            if (is3) return 3;
            if (is5) return 5;
            if (is7) return 7;
            if (is9) return 9;
            return 10;
        }



        /// <summary>
        ///Speichert das element. 
        ///FileDialog wird zur auswahl des Speicherorts geöffnet
        /// </summary>
        /// <param name="obj"> wird nicht verwendet</param>
        public void SaveElement(Object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files(*.jpg; *.bmp; *.gif; *.png)| *.jpg; *.bmp; *.gif; *.png";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    FilteredImage.Save(saveFileDialog.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        /// <summary>
        ///  Lädt die bild-Datei.
        ///  File Dialog wird geöffnet zum wählen des bildes
        /// </summary>
        /// <param name="obj">wird nicht verwendet</param>
        public void LoadElement(Object obj)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP; *.JPG; *.GIF; *.PNG)| *.BMP; *.JPG; *.GIF; *.PNG";
            //string[] files = Directory.GetFiles(openFileDialog.SelectedPath);  // theoretisch für Ordner. rest des Programms aber auf nur 1 file ausgelegt
            openFileDialog.ShowDialog();
            try
            {
                OriginalImage = new Bitmap(openFileDialog.FileName); // offne bild als bitmap
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            OriginalImageSource = BitmapConverter.ImageSourceForBitmap(OriginalImage);
            // Bitmap ggf Zurücksetzen
            if (FilteredImage != null)
            {
                FilteredImage = null;
                FilteredImageSource = null;
            }
        }

    }
}
