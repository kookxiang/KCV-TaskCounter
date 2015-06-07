using Livet;
using System.Collections.Generic;

namespace TaskCounter.ViewModels {
    public class PluginPanelViewModel : ViewModel {
        private List<TaskViewModel> _List = new List<TaskViewModel>();
        public List<TaskViewModel> List {
            get {
                return _List;
            }
            set {
                _List = value;
                RaisePropertyChanged();
            }
        }

        public void Update() {
            RaisePropertyChanged();
        }

        public PluginPanelViewModel() {
        }
    }
}
