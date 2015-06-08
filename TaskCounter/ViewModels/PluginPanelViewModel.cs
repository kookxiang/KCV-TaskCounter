using Livet;
using System.Collections.Generic;

namespace TaskCounter.ViewModels {
    public class PluginPanelViewModel : ViewModel {
        private List<TaskViewModel> _AcceptedList = new List<TaskViewModel>();
        public List<TaskViewModel> AcceptedList {
            get {
                return _AcceptedList;
            }
            set {
                _AcceptedList = value;
                RaisePropertyChanged();
            }
        }

        private List<TaskViewModel> _AvailableList = new List<TaskViewModel>();
        public List<TaskViewModel> AvailableList {
            get {
                return _AvailableList;
            }
            set {
                _AvailableList = value;
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
