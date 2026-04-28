using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VoiceChat.Forms;

namespace VoiceChat
{

    public partial class MainForm : MaterialForm
    {
        private ITcpManager _tcp = new TestTcpManager();
        private string _nickname = "테스트유저";
        private List<RoomInfo> _currentRooms = new List<RoomInfo>();


        public MainForm()
        {
            InitializeComponent();

            InitializeMaterialSkin();

            SubscribeEvents();

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



        private void SubscribeEvents()
        {
            _tcp.OnConnected += OnConnected;
            _tcp.OnConnectFailed += OnConnectFailed;
            _tcp.OnRoomListReceived += OnRoomListReceived;
            _tcp.OnRoomCreated += OnRoomCreated;
        }

        private void OnConnected()
        {
            Invoke((Action)(() =>
            {
                var roomForm = new RoomForm(_nickname, _tcp, this); // this 추가
                roomForm.Show();
                this.Hide();
            }));
        }

        private void OnConnectFailed(string msg)
        {
            Invoke((Action)(() => MessageBox.Show(msg, "연결 실패")));
        }
        private void OnRoomCreated(string roomName)
        {
            Invoke((Action)(() => JoinRoomRequest(roomName)));
        }

        // 2. 이제 List<string>이 아닌 List<RoomInfo>를 받습니다.
        private void UpdateRoomList(List<RoomInfo> rooms)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateRoomList(rooms)));
                return;
            }

            RoomLayoutPanel.Controls.Clear();
            RoomLayoutPanel.SuspendLayout();

            foreach (var room in rooms)
            {
                RoomItemControl item = new RoomItemControl();
                item.RoomName = room.Name;
                //item.RoomCount = $"{room.CurrentUsers}/{room.MaxUsers}";

                // 아이템 간의 간격(Gap)만 설정
                item.Margin = new Padding(0, 0, 0, 10);

                // 너비 계산 (생성자에서 정의한 규칙과 동일하게)
                //item.Width = RoomLayoutPanel.ClientSize.Width - RoomLayoutPanel.Padding.Horizontal - 5;

                item.OnJoinClick += (s, e) => JoinRoomRequest(room.Name);

                RoomLayoutPanel.Controls.Add(item);
            }

            RoomLayoutPanel.ResumeLayout();
        }

        private void JoinRoomRequest(string roomName)
        {
            MessageBox.Show($"{roomName} 입장 요청을 보냅니다.");

            //이민하 : 테스트 코드 추가
            _tcp.Connect("127.0.0.1", 9000, _nickname);

        }

        private void OnRoomListReceived(List<RoomInfo> rooms)
        {
            Invoke((Action)(() =>
            {
                _currentRooms = rooms; // 저장
                UpdateRoomList(_currentRooms);
            }));
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // 3. 테스트용 가짜 데이터 (객체 리스트)
            _tcp.RequestRoomList();
        }

        private void CreateRoomButton_Click(object sender, EventArgs e)
        {
            var dialog = new RoomDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                //임시 테스트용 코드
                {
                    // UI만 추가
                    _currentRooms.Add(new RoomInfo { Name = dialog.RoomName });
                    UpdateRoomList(_currentRooms);

                    // 바로 조인
                    JoinRoomRequest(dialog.RoomName);
                }
               
            }


            // 서버에 요청
            //_tcp.CreateRoom(dialog.RoomName);
            // 서버가 OnRoomListReceived로 갱신된 목록 보내주면 자동 갱신
            // 조인은 OnRoomCreated에서 자동으로
        }
        
    }


    // 1. 방 정보를 담는 데이터 모델 (나중에 서버 JSON 파싱용으로도 사용됨)
    public class RoomInfo
    {
        public string Name { get; set; }
        //public int CurrentUsers { get; set; }
        //public int MaxUsers { get; set; }
    }
}