using System.Windows;

namespace SyntacticalPetApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MainWindowViewModel = new MainWindowViewModel();
            DataContext = MainWindowViewModel;
            InitializeComponent();
        }

        public MainWindowViewModel MainWindowViewModel { get; set; }
    }
}