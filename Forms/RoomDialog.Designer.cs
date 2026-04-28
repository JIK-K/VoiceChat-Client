namespace VoiceChat.Forms
{
    partial class RoomDialog
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_makeroom = new MaterialSkin.Controls.MaterialButton();
            this.btn_cancel = new MaterialSkin.Controls.MaterialButton();
            this.tb_roomname = new MaterialSkin.Controls.MaterialTextBox();
            this.lbl_roomname = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // btn_makeroom
            // 
            this.btn_makeroom.AutoSize = false;
            this.btn_makeroom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_makeroom.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_makeroom.Depth = 0;
            this.btn_makeroom.Font = new System.Drawing.Font("굴림", 9F);
            this.btn_makeroom.HighEmphasis = true;
            this.btn_makeroom.Icon = null;
            this.btn_makeroom.Location = new System.Drawing.Point(66, 196);
            this.btn_makeroom.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_makeroom.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_makeroom.Name = "btn_makeroom";
            this.btn_makeroom.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_makeroom.Size = new System.Drawing.Size(120, 36);
            this.btn_makeroom.TabIndex = 1;
            this.btn_makeroom.Text = "방 만들기";
            this.btn_makeroom.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_makeroom.UseAccentColor = false;
            this.btn_makeroom.UseVisualStyleBackColor = true;
            this.btn_makeroom.Click += new System.EventHandler(this.btn_makeroom_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.AutoSize = false;
            this.btn_cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_cancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_cancel.Depth = 0;
            this.btn_cancel.HighEmphasis = true;
            this.btn_cancel.Icon = null;
            this.btn_cancel.Location = new System.Drawing.Point(236, 196);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_cancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_cancel.Size = new System.Drawing.Size(120, 36);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "취소하기";
            this.btn_cancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_cancel.UseAccentColor = false;
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // tb_roomname
            // 
            this.tb_roomname.AnimateReadOnly = false;
            this.tb_roomname.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_roomname.Depth = 0;
            this.tb_roomname.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tb_roomname.LeadingIcon = null;
            this.tb_roomname.Location = new System.Drawing.Point(66, 107);
            this.tb_roomname.MaxLength = 50;
            this.tb_roomname.MouseState = MaterialSkin.MouseState.OUT;
            this.tb_roomname.Multiline = false;
            this.tb_roomname.Name = "tb_roomname";
            this.tb_roomname.Size = new System.Drawing.Size(290, 50);
            this.tb_roomname.TabIndex = 3;
            this.tb_roomname.Text = "";
            this.tb_roomname.TrailingIcon = null;
      
            // 
            // lbl_roomname
            // 
            this.lbl_roomname.AutoSize = true;
            this.lbl_roomname.Depth = 0;
            this.lbl_roomname.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbl_roomname.Location = new System.Drawing.Point(182, 51);
            this.lbl_roomname.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_roomname.Name = "lbl_roomname";
            this.lbl_roomname.Size = new System.Drawing.Size(57, 19);
            this.lbl_roomname.TabIndex = 4;
            this.lbl_roomname.Text = "[ 방 제목 ]";
            // 
            // RoomDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 300);
            this.Controls.Add(this.lbl_roomname);
            this.Controls.Add(this.tb_roomname);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_makeroom);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.StatusAndActionBar_None;
            this.Name = "RoomDialog";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Load += new System.EventHandler(this.RoomDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MaterialSkin.Controls.MaterialButton btn_makeroom;
        private MaterialSkin.Controls.MaterialButton btn_cancel;
        private MaterialSkin.Controls.MaterialTextBox tb_roomname;
        private MaterialSkin.Controls.MaterialLabel lbl_roomname;
    }
}
