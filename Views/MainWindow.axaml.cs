using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Fanban.Models;

namespace Fanban.Views;

public partial class MainWindow : Window
{
    private KanbanTask _draggedTask;
    private KanbanColumn _sourceColumn;

    public MainWindow()
    {
        InitializeComponent();
        
        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }

    private void OnTaskPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            var taskBorder = sender as Border;
            if (taskBorder?.DataContext is KanbanTask task)
            {
                _draggedTask = task;
                // 找到源列
                var itemsControl = taskBorder.FindAncestorOfType<ItemsControl>();
                if (itemsControl?.DataContext is KanbanColumn column)
                {
                    _sourceColumn = column;
                }

                var data = new DataObject();
                data.Set("task", task);
                DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
            }
        }
    }

    private void DragOver(object sender, DragEventArgs e)
    {
        if (_draggedTask != null)
        {
            e.DragEffects = DragDropEffects.Move;
        }
        else
        {
            e.DragEffects = DragDropEffects.None;
        }
    }

    private void Drop(object sender, DragEventArgs e)
    {
        if (_draggedTask != null)
        {
            var targetElement = e.Source as Control;
            var targetColumn = FindTargetColumn(targetElement);

            if (targetColumn != null && _sourceColumn != null)
            {
                // 从源列移除任务
                _sourceColumn.Tasks.Remove(_draggedTask);
                // 添加到目标列
                targetColumn.Tasks.Add(_draggedTask);
            }

            _draggedTask = null;
            _sourceColumn = null;
        }
    }

    private KanbanColumn FindTargetColumn(Control? element)
    {
        while (element != null)
        {
            if (element.DataContext is KanbanColumn column)
            {
                return column;
            }
            element = element.Parent as Control;
        }
        return null;
    }
}