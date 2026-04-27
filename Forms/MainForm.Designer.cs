namespace VoiceChat
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {

            this.CreateRoomButton = new MaterialSkin.Controls.MaterialButton();
            this.RoomLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // CreateRoomButton
            // 
            this.CreateRoomButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CreateRoomButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.CreateRoomButton.Depth = 0;
            this.CreateRoomButton.HighEmphasis = true;
            this.CreateRoomButton.Icon = null;
            this.CreateRoomButton.Location = new System.Drawing.Point(7, 39);
            this.CreateRoomButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.CreateRoomButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.CreateRoomButton.Name = "CreateRoomButton";
            this.CreateRoomButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.CreateRoomButton.Size = new System.Drawing.Size(118, 36);
            this.CreateRoomButton.TabIndex = 0;
            this.CreateRoomButton.Text = "CreateRoom";
            this.CreateRoomButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.CreateRoomButton.UseAccentColor = false;
            this.CreateRoomButton.UseVisualStyleBackColor = true;
            // 
            // RoomLayoutPanel
            // 
            this.RoomLayoutPanel.AutoScroll = true;
            this.RoomLayoutPanel.Location = new System.Drawing.Point(6, 84);
            this.RoomLayoutPanel.Name = "RoomLayoutPanel";
            this.RoomLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            this.RoomLayoutPanel.Size = new System.Drawing.Size(388, 360);
            this.RoomLayoutPanel.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.Controls.Add(this.RoomLayoutPanel);
            this.Controls.Add(this.CreateRoomButton);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_None;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(3, 24, 15, 15);
            this.Text = "VoiceChat";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();


        }

        #endregion

        private MaterialSkin.Controls.MaterialButton CreateRoomButton;
        private System.Windows.Forms.FlowLayoutPanel RoomLayoutPanel;
    }
}

