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
    public partial class RoomItemControl : UserControl
    {
        public string RoomName
        {
            get => RoomNameLabel.Text;
            set => RoomNameLabel.Text = value;
        }
        public string RoomCount
        {
            get => RoomCountLabel.Text;
            set => RoomCountLabel.Text = value;
        }

        public event EventHandler OnJoinClick;
        public RoomItemControl()
        {
            InitializeComponent();

            // 버튼 클릭 시 등록된 이벤트를 발생시킴
            JoinButton.Click += (s, e) => OnJoinClick?.Invoke(this, e);
        }
    }
}
