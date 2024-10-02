using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Terrain_Data;
using System.IO;
using System.Windows.Markup;

namespace EngineTool
{
    public class MainWindowViewModel : BaseViewModel
    {
        private TerrainData _myData = new();

        public MainWindowViewModel()
        {
            ClearCommand = new DelegateCommand(
                o => { return !string.IsNullOrEmpty(MapSize) || !string.IsNullOrEmpty(MaxTerrainHeight); },
                o =>
                {
                    MapSize = "0,0";
                    MaxTerrainHeight = "0,0";
                }
                );

            SaveDataCommand = new DelegateCommand(
                o => !string.IsNullOrEmpty(MapSize) && !string.IsNullOrEmpty(MaxTerrainHeight),
                o => SaveJsonFile()
                );

            MapSize = "100,00";
            MaxTerrainHeight = "50,00";
        }

        private void SaveJsonFile()
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "Filehousr files (*.fhr)|*.fhr"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string dataListString = JsonConvert.SerializeObject(_myData);
                File.WriteAllText(saveFileDialog.FileName, dataListString);
            }
        }

        public DelegateCommand ClearCommand { get; set; }
        public DelegateCommand SaveDataCommand { get; set; }

        public string MapSize
        {
            get => string.Format("{0:N2}", _myData.MapSize);
            set
            {
                var newVal = SetData(_myData.MapSize, value);
                if (_myData.MapSize != newVal)
                {
                    _myData.MapSize = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MaxTerrainHeight
        {
            get => string.Format("{0:N2}", _myData.MaxTerrainHeight);
            set
            {
                var newVal = SetData(_myData.MaxTerrainHeight, value);
                if (_myData.MaxTerrainHeight != newVal)
                {
                    _myData.MaxTerrainHeight = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string BaseHeight
        {
            get => string.Format("{0:N2}", _myData.BaseHeight);
            set
            {
                var newVal = SetData(_myData.BaseHeight, value);
                if (_myData.BaseHeight != newVal)
                {
                    _myData.BaseHeight = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private dynamic SetData(dynamic data, string value)
        {
            dynamic result = 0;
            bool success = false;

            if (data is float)
            {
                float r = (float)result;
                success = float.TryParse(value, out r);
                result = r;
            }

            if (data is int)
            {
                int r = (int)result;
                success = int.TryParse(value, out r);
                result = r;
            }

            if (!success)
                result = data;

            return result;
        }

        //private void SetIntData(ref int data, string value, [CallerMemberName] string propertyName = "")
        //{
        //    if (int.TryParse(value, out int result))
        //    {
        //        if (data != result)
        //        {
        //            data = result;
        //            RaisePropertyChanged(propertyName);
        //            ClearCommand.RaiseCanExecuteChanged();
        //        }
        //    }
        //}

    }
}
