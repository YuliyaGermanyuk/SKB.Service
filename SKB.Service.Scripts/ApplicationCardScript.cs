using System;
using System.Windows.Forms;

using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.WinForms;

using DocsVision.Platform;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectModel;
using DocsVision.Platform.WinForms;

using SKB.Service.Cards;

namespace Service
{
    public class ApplicationCardScript : ScriptClassBase
    {

        #region Properties

        ApplicationCard Card = null;

        #endregion

        #region Methods

        #endregion

        #region Event Handlers

        private void ApplicationCard_CardActivated (Object sender, CardActivatedEventArgs e)
        {
            Card = new ApplicationCard(this, e);
        }

        #endregion

    }
}
