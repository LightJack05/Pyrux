// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
using Pyrux.Pages.DocsPages;
namespace Pyrux.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DocsPage : Page
    {
        Dictionary<int, TeachingTip> PageTeachingTips = new()
        {

        };


        public DocsPage()
        {
            this.InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitTutorial();
        }



        private void btnPopoutDocs_Click(object sender, RoutedEventArgs e)
        {
            if(ApplicationWindows.DocumentationWindow.Instance != null)
            {
                ApplicationWindows.DocumentationWindow.Instance.Close();
            }
            Window docsWindow = new ApplicationWindows.DocumentationWindow();
            docsWindow.Activate();
        }

        private void InitTutorial()
        {
            PageTeachingTips.Clear();
            PageTeachingTips.Add(21, tctDocsPageIntro);
            PageTeachingTips.Add(22, tctPopout);
            PageTeachingTips.Add(23, tctTutorialEnd);

            if (!PyruxSettings.SkipTutorialEnabled)
            {
                try
                {
                    PageTeachingTips[PyruxSettings.TutorialStateId].IsOpen = true;
                }
                catch
                {

                }
            }
        }

        private void TeachingTipNext_Click(object sender, RoutedEventArgs e)
        {
            PageTeachingTips[PyruxSettings.TutorialStateId].IsOpen = false;
            PyruxSettings.TutorialStateId++;
            if (PageTeachingTips.Count + 21 <= PyruxSettings.TutorialStateId)
            {

            }
            else
            {
                PageTeachingTips[PyruxSettings.TutorialStateId].IsOpen = true;
            }
        }

        private void TeachingTipCloseButtonClick(TeachingTip sender, object args)
        {
            PyruxSettings.SkipTutorialEnabled = true;
            PyruxSettings.TutorialStateId = 0;
        }

        private void TeachingTipLastButtonClick(Object sender, object args)
        {
            tctTutorialEnd.IsOpen = false;
            PyruxSettings.SkipTutorialEnabled = true;
            PyruxSettings.TutorialStateId = 0;
        }

    }
}
