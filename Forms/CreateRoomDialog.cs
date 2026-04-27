using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChat.Forms
{
    public class CreateRoomDialog : MaterialForm
    {
        public string RoomName { get; private set; }

        private MaterialTextBox _txtRoomName;

        public CreateRoomDialog()
        {
            //InitializeMaterialSkin();
           // BuildUI();
        }
    }
}
