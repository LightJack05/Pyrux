// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.ApplicationWindows
{

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DocumentationWindow : Window
    {
        public static DocumentationWindow Instance { get; private set; }
        public string TitleText
        {
            get => "[PRE-ALPHA] Pyrux v" + StaticDataStore.VersionNumber + " - Documentation";
        }

        public DocumentationWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            Instance = this;
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {

        }
    }
}
