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
                _ => { return !string.IsNullOrEmpty(MapSize) || !string.IsNullOrEmpty(MaxTerrainHeight); },
                _ =>
                {
                    MapSize = "0,0";
                    MaxTerrainHeight = "0,0";
                }
                );

            SaveDataCommand = new DelegateCommand(
                _ => !string.IsNullOrEmpty(MapSize) && !string.IsNullOrEmpty(MaxTerrainHeight),
                _ => SaveJsonFile()
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

        #region Terrain Variables
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

        public string TerrainLayerHeight01
        {
            get => string.Format("{0:N2}", _myData.TerrainLayerHeight01);
            set
            {
                var newVal = SetData(_myData.TerrainLayerHeight01, value);
                if (_myData.TerrainLayerHeight01 != newVal)
                {
                    _myData.TerrainLayerHeight01 = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string TerrainLayerHeight02
        {
            get => string.Format("{0:N2}", _myData.TerrainLayerHeight02);
            set
            {
                var newVal = SetData(_myData.TerrainLayerHeight02, value);
                if (_myData.TerrainLayerHeight02 != newVal)
                {
                    _myData.TerrainLayerHeight02 = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string TerrainLayerHeight03
        {
            get => string.Format("{0:N2}", _myData.TerrainLayerHeight03);
            set
            {
                var newVal = SetData(_myData.TerrainLayerHeight03, value);
                if (_myData.TerrainLayerHeight03 != newVal)
                {
                    _myData.TerrainLayerHeight03 = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public float TransitionSharpness
        {
            get => _myData.TransitionSharpness;
            set
            {
                _myData.TransitionSharpness = value;
            }
        }
        #endregion

        #region Tree Variables
        public string Density
        {
            get => string.Format("{0}", _myData.Density);
            set
            {
                var newVal = SetData(_myData.Density, value);
                if (_myData.Density != newVal)
                {
                    _myData.Density = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string VerticalOffset
        {
            get => string.Format("{0:N2}", _myData.VerticalOffset);
            set
            {
                var newVal = SetData(_myData.VerticalOffset, value);
                if (_myData.VerticalOffset != newVal)
                {
                    _myData.VerticalOffset = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MaxRotationTowardsTerrainNormal
        {
            get => string.Format("{0:N0}", _myData.MaxRotationTowardsTerrainNormal);
            set
            {
                var newVal = SetData(_myData.MaxRotationTowardsTerrainNormal, value);
                if (_myData.MaxRotationTowardsTerrainNormal != newVal)
                {
                    _myData.MaxRotationTowardsTerrainNormal = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MinRotation
        {
            get => string.Format("{0:N0}", _myData.MinRotation);
            set
            {
                var newVal = SetData(_myData.MinRotation, value);
                if (_myData.MinRotation != newVal)
                {
                    _myData.MinRotation = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MaxRotation
        {
            get => string.Format("{0:N0}", _myData.MaxRotation);
            set
            {
                var newVal = SetData(_myData.MaxRotation, value);
                if (_myData.MaxRotation != newVal)
                {
                    _myData.MaxRotation = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MinXScale
        {
            get => string.Format("{0:N2}", _myData.MinXScale);
            set
            {
                var newVal = SetData(_myData.MinXScale, value);
                if (_myData.MinXScale != newVal)
                {
                    _myData.MinXScale = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MinYScale
        {
            get => string.Format("{0:N2}", _myData.MinYScale);
            set
            {
                var newVal = SetData(_myData.MinYScale, value);
                if (_myData.MinYScale != newVal)
                {
                    _myData.MinYScale = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MinZScale
        {
            get => string.Format("{0:N2}", _myData.MinZScale);
            set
            {
                var newVal = SetData(_myData.MinZScale, value);
                if (_myData.MinZScale != newVal)
                {
                    _myData.MinZScale = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MaxXScale
        {
            get => string.Format("{0:N2}", _myData.MaxXScale);
            set
            {
                var newVal = SetData(_myData.MaxXScale, value);
                if (_myData.MaxXScale != newVal)
                {
                    _myData.MaxXScale = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MaxYScale
        {
            get => string.Format("{0:N2}", _myData.MaxYScale);
            set
            {
                var newVal = SetData(_myData.MaxYScale, value);
                if (_myData.MaxYScale != newVal)
                {
                    _myData.MaxYScale = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string MaxZScale
        {
            get => string.Format("{0:N2}", _myData.MaxZScale);
            set
            {
                var newVal = SetData(_myData.MaxZScale, value);
                if (_myData.MaxZScale != newVal)
                {
                    _myData.MaxZScale = newVal;
                    RaisePropertyChanged();
                    ClearCommand.RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

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
