using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Fanban.Models
{
    public class KanbanColumn
    {
        public string Title { get; set; }
        public ObservableCollection<KanbanTask> Tasks { get; set; }
    }
} 