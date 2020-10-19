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
    public class ServicesCardScript : ScriptClassBase
    {
        #region Properties

        SKB.Service.Cards.ServicesCard Card = null;

        #endregion

        #region Methods

        #endregion

        #region Event Handlers

        private void ServicesCard_CardActivated (Object sender, CardActivatedEventArgs e)
        {
            Card = new ServicesCard(this, e);
        }

        #endregion

    }
}
