using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoiceChat.Forms
{
    public partial class RoomForm : MaterialForm
    {
        public RoomForm()
        {
            InitializeComponent();

      

            InitializeMaterialSkin();
            InitializeLayout();
        }
        private void InitializeLayout()
        {

        }
        private void InitializeMaterialSkin()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.BlueGrey900, Primary.BlueGrey900, Primary.BlueGrey500,
                Accent.LightBlue200, TextShade.WHITE
            );
        }

        private void RoomForm_Load(object sender, EventArgs e)
        {

        }

        private Panel PnlServerList;
        private Panel PnlChannelList;
        private Panel PnlContent;
    }
}
