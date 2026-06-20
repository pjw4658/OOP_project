namespace project_cs.Source.UI
{
    partial class NameInputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtName = new TextBox();
            lblMessage = new Label();
            btnOk = new Button();
            btnCancel = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Font = new Font("맑은 고딕", 11F);
            txtName.Location = new Point(254, 204);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "닉네임 입력 (최대 20자)";
            txtName.Size = new Size(343, 47);
            txtName.TabIndex = 0;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Font = new Font("맑은 고딕", 11F);
            lblMessage.ImageAlign = ContentAlignment.TopLeft;
            lblMessage.Location = new Point(192, 37);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(369, 41);
            lblMessage.TabIndex = 1;
            lblMessage.Text = "[크기-난이도] 최종 점수 : ";
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(345, 308);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(174, 91);
            btnOk.TabIndex = 2;
            btnOk.Text = "확인";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(565, 308);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(174, 91);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "건너뛰기";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 11F);
            label1.Location = new Point(122, 207);
            label1.Name = "label1";
            label1.Size = new Size(126, 41);
            label1.TabIndex = 4;
            label1.Text = "닉네임 :";
            // 
            // NameInputForm
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(774, 429);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(lblMessage);
            Controls.Add(txtName);
            Name = "NameInputForm";
            Text = "점수 기록";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtName;
        private Label lblMessage;
        private Button btnOk;
        private Button btnCancel;
        private Label label1;
    }
}