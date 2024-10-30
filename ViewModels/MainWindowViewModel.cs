using System.Collections.ObjectModel;
using System.Windows.Input;
using Fanban.Models;
using System;
using System.Diagnostics;

namespace Fanban.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<KanbanColumn> Columns { get; }
    public ICommand AddTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }

    public MainWindowViewModel()
    {
        Columns = new ObservableCollection<KanbanColumn>
        {
            new KanbanColumn 
            { 
                Title = "待办", 
                Tasks = new ObservableCollection<KanbanTask>
                {
                    new KanbanTask { Title = "示例任务1", Description = "这是一个示例任务" }
                }
            },
            new KanbanColumn 
            { 
                Title = "进行中",
                Tasks = new ObservableCollection<KanbanTask>()
            },
            new KanbanColumn 
            { 
                Title = "已完成",
                Tasks = new ObservableCollection<KanbanTask>()
            }
        };

        AddTaskCommand = new RelayCommand(_ => AddTask());
        DeleteTaskCommand = new RelayCommand(_ => DeleteTask());
    }

    private void AddTask()
    {
        try
        {
            var newTask = new KanbanTask 
            { 
                Title = "新任务", 
                Description = "请添加任务描述" 
            };
            
            if (Columns.Count > 0)
            {
                Columns[0].Tasks.Add(newTask);
                Debug.WriteLine($"添加了新任务: {newTask.Title}");
                Debug.WriteLine($"当前任务数量: {Columns[0].Tasks.Count}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"添加任务时发生错误: {ex.Message}");
        }
    }

    private void DeleteTask()
    {
        if (Columns.Count > 0 && Columns[0].Tasks.Count > 0)
        {
            Columns[0].Tasks.RemoveAt(Columns[0].Tasks.Count - 1);
        }
    }
}

public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}
