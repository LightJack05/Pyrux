// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pyrux.Pages.AboutPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutText : Page
    {
        public AboutText()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The version of the assembly.
        /// </summary>
        public string Version
        {
            get => StaticDataStore.VersionNumber;
        }
        /// <summary>
        /// The text displayed on the about page.
        /// </summary>
        public string AboutTextContent
        {
            get => """
                Pyrux is created by LightJack05.

                [Copyright owned by LightJack05. Do not distribute. Pre-Production Version, to be considered confidential.]

                # __PRE-ALPHA NOTICE__

                This version of the software is pre-alpha. This means it is not intended for production use.
                This edition is not allowed to be shared, distributed, presented, used or published without explicit permission from the author.

                There are currently a LOT of bugs and problems in the software. It is possible that a lot of features will not work, the software might crash, or even break the OS or hardware it is installed on.
                I am not responsible for any damage done by use of this software, as far as permitted by applicable law. This software comes with ABSOLUTELY NO WARRANTY.

                If you encounter any bugs, at this state, do not be surprised. Feel free to open an issue on GitHub should the repo be public at this point in time.

                Otherwise, feel free to reach out via Discord.


                In addition to that, there are currently only a few limited built in levels (if any).
                Custom levels are working, however they are still not verified to work by any means. Crashes and corruptions can occur.
                Also, the level format may change at any time without further notice.
                Improper importing of levels may lead to crashes. Should this happen, please clear out the appdata folder.

                The early version also means that there is currently no tutorial. You should be instructed to use the current version by the developer.
                """;
        }
    }
}
