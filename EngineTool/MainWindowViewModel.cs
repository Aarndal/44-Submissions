using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace EngineTool
{
    public class MainWindowViewModel : BaseViewModel
    {
        private float _mapSize = 0;
        private float _maxTerrainHeight = 0;

        public MainWindowViewModel()
        {
            ClearCommand = new DelegateCommand(
                o => !string.IsNullOrEmpty(MapSize),
                o => { MapSize = "0,0"; MaxTerrainHeight = "0,0"; }
                );

            MapSize = "100,00";
            MaxTerrainHeight = "50,00";
        }

        public DelegateCommand ClearCommand { get; set; }

        public string MapSize
        {
            get => string.Format("{0:N2}", _mapSize);
            set
            {
                if (float.TryParse(value, out float result))
                {
                    if (_mapSize != result)
                    {
                        _mapSize = result;
                        this.RaisePropertyChanged();
                        ClearCommand.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public string MaxTerrainHeight
        {
            get => string.Format("{0:N2}", _maxTerrainHeight);
            set
            {
                if (float.TryParse(value, out float result))
                {
                    if (_maxTerrainHeight != result)
                    {
                        _maxTerrainHeight = result;
                        this.RaisePropertyChanged();
                        ClearCommand.RaiseCanExecuteChanged();
                    }
                }
            }
        }
    }
}
