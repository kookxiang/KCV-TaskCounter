using Livet;
using TaskCounter.Models;

namespace TaskCounter.ViewModels {
    public class TaskViewModel : ViewModel {
        public Task BindedTask;

        public TaskViewModel() {
        }

        public TaskViewModel(Task BindedTask) {
            this.BindedTask = BindedTask;
        }

        private string _Name;
        public string Name {
            get {
                return _Name;
            }
            set {
                _Name = value;
                RaisePropertyChanged();
            }
        }

        private int _Precentage;
        public int Precentage {
            get {
                return _Precentage;
            }
            set {
                _Precentage = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ProgressBar_Fill");
                RaisePropertyChanged("ProgressBar_Blank");
            }
        }

        public string ProgressBar_Fill {
            get {
                return _Precentage + "*";
            }
        }
        public string ProgressBar_Blank {
            get {
                return 100 - _Precentage + "*";
            }
        }

        private string _Description;
        public string Description {
            get {
                return _Description;
            }
            set {
                _Description = value;
                RaisePropertyChanged();
            }
        }
    }
}
