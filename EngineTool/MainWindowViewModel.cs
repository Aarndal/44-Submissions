using Microsoft.Win32;
using Newtonsoft.Json;
using Terrain_Data;
using System.IO;

namespace EngineTool
{
    public class MainWindowViewModel : BaseViewModel
    {
        //private readonly float _FLOAT_MIN = 0.0001f;

        private TerrainData _myData = new();

        public MainWindowViewModel()
        {
            ResetCommand = new DelegateCommand(_ => SetBaseValues());

            SaveDataCommand = new DelegateCommand(
                _ => PropertiesAreNotNull(),
                _ => SaveJsonFile()
                );

            SetBaseValues();
        }

        public DelegateCommand ResetCommand { get; set; }
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public float MaxRotationTowardsTerrainNormal
        {
            get => _myData.MaxRotationTowardsTerrainNormal;
            set
            {
                _myData.MaxRotationTowardsTerrainNormal = value;
            }
        }

        public float MinRotation
        {
            get => _myData.MinRotation;
            set
            {
                _myData.MinRotation = value;
            }
        }

        public float MaxRotation
        {
            get => _myData.MaxRotation;
            set
            {
                _myData.MaxRotation = value;
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
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
                    ResetCommand.RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        private void SetBaseValues()
        {
            MapSize = "600,00";
            MaxTerrainHeight = "80,00";
            BaseHeight = "0,00";
            TerrainLayerHeight01 = "20,00";
            TerrainLayerHeight02 = "60,00";
            TerrainLayerHeight03 = "80,00";
            TransitionSharpness = 0.5f;

            Density = "100000";
            VerticalOffset = "0,50";
            MaxRotationTowardsTerrainNormal = 2.0f;
            MinRotation = 0;
            MaxRotation = 360;
            MinXScale = "0,5";
            MinYScale = "0,5";
            MinZScale = "0,5";
            MaxXScale = "3,0";
            MaxYScale = "3,0";
            MaxZScale = "3,0";
        }

        private bool PropertiesAreNotNull()
        {
            return !string.IsNullOrEmpty(MapSize) && !string.IsNullOrEmpty(MaxTerrainHeight);
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

        private static dynamic SetData(dynamic data, string value)
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
