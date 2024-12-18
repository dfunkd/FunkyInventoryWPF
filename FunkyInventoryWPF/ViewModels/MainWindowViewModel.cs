﻿using FunkyInventoryWPF.Models.UserModels;
using System.ComponentModel;

namespace FunkyInventoryWPF.ViewModels;

internal class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private string title = "Main Window";
    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged();
            }
        }
    }

    private User? loggedInUser = null;
    public User? LoggedInUser
    {
        get => loggedInUser;
        set
        {
            if (loggedInUser != value)
            {
                loggedInUser = value;
                OnPropertyChanged();
            }
        }
    }
}