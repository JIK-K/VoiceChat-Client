namespace VoiceChat.Forms
{
    partial class RoomItemControl
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
            this.JoinButton = new MaterialSkin.Controls.MaterialButton();
            this.RoomNameLabel = new MaterialSkin.Controls.MaterialLabel();
            this.RoomCountLabel = new MaterialSkin.Controls.MaterialLabel();
            this.RoomCard = new MaterialSkin.Controls.MaterialCard();
            this.RoomCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // JoinButton
            // 
            this.JoinButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.JoinButton.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.JoinButton.Depth = 0;
            this.JoinButton.HighEmphasis = true;
            this.JoinButton.Icon = null;
            this.JoinButton.Location = new System.Drawing.Point(246, 14);
            this.JoinButton.Margin = new System.Windows.Forms.Padding(6, 9, 6, 9);
            this.JoinButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.JoinButton.Name = "JoinButton";
            this.JoinButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.JoinButton.Size = new System.Drawing.Size(64, 36);
            this.JoinButton.TabIndex = 0;
            this.JoinButton.Text = "Join";
            this.JoinButton.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.JoinButton.UseAccentColor = false;
            this.JoinButton.UseVisualStyleBackColor = true;
            // 
            // RoomNameLabel
            // 
            this.RoomNameLabel.AutoSize = true;
            this.RoomNameLabel.Depth = 0;
            this.RoomNameLabel.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RoomNameLabel.Location = new System.Drawing.Point(17, 14);
            this.RoomNameLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.RoomNameLabel.Name = "RoomNameLabel";
            this.RoomNameLabel.Size = new System.Drawing.Size(37, 19);
            this.RoomNameLabel.TabIndex = 1;
            this.RoomNameLabel.Text = "방제목";
            // 
            // RoomCountLabel
            // 
            this.RoomCountLabel.AutoSize = true;
            this.RoomCountLabel.Depth = 0;
            this.RoomCountLabel.Font = new System.Drawing.Font("돋움", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RoomCountLabel.Location = new System.Drawing.Point(17, 33);
            this.RoomCountLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.RoomCountLabel.Name = "RoomCountLabel";
            this.RoomCountLabel.Size = new System.Drawing.Size(37, 19);
            this.RoomCountLabel.TabIndex = 2;
            this.RoomCountLabel.Text = "인원수";
            // 
            // RoomCard
            // 
            this.RoomCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RoomCard.Controls.Add(this.JoinButton);
            this.RoomCard.Controls.Add(this.RoomCountLabel);
            this.RoomCard.Controls.Add(this.RoomNameLabel);
            this.RoomCard.Depth = 0;
            this.RoomCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.RoomCard.Location = new System.Drawing.Point(0, 0);
            this.RoomCard.Margin = new System.Windows.Forms.Padding(0);
            this.RoomCard.MouseState = MaterialSkin.MouseState.HOVER;
            this.RoomCard.Name = "RoomCard";
            this.RoomCard.Padding = new System.Windows.Forms.Padding(14);
            this.RoomCard.Size = new System.Drawing.Size(330, 65);
            this.RoomCard.TabIndex = 3;
            // 
            // RoomItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.RoomCard);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "RoomItemControl";
            this.Size = new System.Drawing.Size(330, 65);
            this.RoomCard.ResumeLayout(false);
            this.RoomCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialButton JoinButton;
        private MaterialSkin.Controls.MaterialLabel RoomNameLabel;
        private MaterialSkin.Controls.MaterialLabel RoomCountLabel;
        private MaterialSkin.Controls.MaterialCard RoomCard;
    }
}
