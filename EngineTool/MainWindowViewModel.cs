using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace EngineTool
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private float _mapSize;

        public MainWindowViewModel()
        {
            MapSize = 100;
        }

        public float MapSize
        {
            get => _mapSize;
            set
            {

                if (_mapSize != value)
                {
                    _mapSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "") // CallerMemberName: Automatically gets the name of the calling property if no string is provided
        {
            if (!string.IsNullOrEmpty(propertyName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
