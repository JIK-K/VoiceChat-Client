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
    public partial class RoomDialog : MaterialForm
    {

        public string RoomName { get; private set; }
        public RoomDialog()
        {
            InitializeComponent();
            InitializeMaterialSkin();
        }

        private void InitializeMaterialSkin()
        {
            var skin = MaterialSkinManager.Instance;
            skin.AddFormToManage(this);
            skin.ColorScheme = new ColorScheme(
                Primary.BlueGrey900, Primary.BlueGrey900,
                Primary.BlueGrey500, Accent.LightBlue200,
                TextShade.WHITE
            );
        }

        private void RoomDialog_Load(object sender, EventArgs e)
        {

        }

        private void tb_roomname_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_makeroom_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tb_roomname.Text)) return;

            RoomName = tb_roomname.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();


        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
