﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TheBookNook_WPF.Core
{
    public abstract class VMBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
