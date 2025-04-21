using ContextEf;
using Microsoft.Win32;
using System.IO;

namespace LoggerReader.Services.UserDialogs;

public interface IUserDialog<out T> 
{
    T Show();
}
public interface IUserDialog
{
    bool Show();
}

internal class FileUserDialog : IUserDialog
{
    private readonly DataBaseSettings _dataBaseSettings;
    public FileUserDialog(DataBaseSettings dataBaseSettings)
    {
        _dataBaseSettings = dataBaseSettings;
    }
    public bool Show()
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Database files (*.db)|*.db",
            FilterIndex = 2,
            RestoreDirectory = true
        };
        if (openFileDialog.ShowDialog() is not true) return false;
        _dataBaseSettings.FilePath = openFileDialog.FileName;
        return true;
    }
}


