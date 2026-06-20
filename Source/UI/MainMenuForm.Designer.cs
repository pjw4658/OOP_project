namespace project_cs.Source.UI
{
    partial class MainMenuForm
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
            lblTitle = new Label();
            lblSubTitle = new Label();
            grpBoardSize = new GroupBox();
            rbLarge = new RadioButton();
            rbMedium = new RadioButton();
            rbSmall = new RadioButton();
            grpDifficulty = new GroupBox();
            rbHard = new RadioButton();
            rbNormal = new RadioButton();
            rbEasy = new RadioButton();
            btnStart = new Button();
            rankingBoardControl1 = new project_cs.Source.UI.Components.RankingBoardControl();
            grpBoardSize.SuspendLayout();
            grpDifficulty.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("맑은 고딕", 21F);
            lblTitle.Location = new Point(700, 75);
            lblTitle.Margin = new Padding(6, 0, 6, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(600, 75);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "사과게임";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubTitle
            // 
            lblSubTitle.Font = new Font("맑은 고딕", 11F);
            lblSubTitle.Location = new Point(500, 160);
            lblSubTitle.Margin = new Padding(6, 0, 6, 0);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(1000, 38);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "드래그해서 합이 10이 되는 사과를 제거하세요";
            lblSubTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpBoardSize
            // 
            grpBoardSize.BackColor = SystemColors.ButtonHighlight;
            grpBoardSize.Controls.Add(rbLarge);
            grpBoardSize.Controls.Add(rbMedium);
            grpBoardSize.Controls.Add(rbSmall);
            grpBoardSize.Location = new Point(140, 223);
            grpBoardSize.Margin = new Padding(6);
            grpBoardSize.Name = "grpBoardSize";
            grpBoardSize.Padding = new Padding(6);
            grpBoardSize.Size = new Size(877, 151);
            grpBoardSize.TabIndex = 2;
            grpBoardSize.TabStop = false;
            grpBoardSize.Text = "보드 크기 선택";
            // 
            // rbLarge
            // 
            rbLarge.AutoSize = true;
            rbLarge.Location = new Point(612, 66);
            rbLarge.Margin = new Padding(6);
            rbLarge.Name = "rbLarge";
            rbLarge.Size = new Size(178, 36);
            rbLarge.TabIndex = 2;
            rbLarge.TabStop = true;
            rbLarge.Text = "대형 (14x14)";
            rbLarge.UseVisualStyleBackColor = true;
            // 
            // rbMedium
            // 
            rbMedium.AutoSize = true;
            rbMedium.Location = new Point(320, 66);
            rbMedium.Margin = new Padding(6);
            rbMedium.Name = "rbMedium";
            rbMedium.Size = new Size(178, 36);
            rbMedium.TabIndex = 1;
            rbMedium.TabStop = true;
            rbMedium.Text = "중형 (10x10)";
            rbMedium.UseVisualStyleBackColor = true;
            // 
            // rbSmall
            // 
            rbSmall.AutoSize = true;
            rbSmall.Location = new Point(49, 66);
            rbSmall.Margin = new Padding(6);
            rbSmall.Name = "rbSmall";
            rbSmall.Size = new Size(152, 36);
            rbSmall.TabIndex = 0;
            rbSmall.TabStop = true;
            rbSmall.Text = "소형 (7x7)";
            rbSmall.UseVisualStyleBackColor = true;
            // 
            // grpDifficulty
            // 
            grpDifficulty.BackColor = SystemColors.ButtonHighlight;
            grpDifficulty.Controls.Add(rbHard);
            grpDifficulty.Controls.Add(rbNormal);
            grpDifficulty.Controls.Add(rbEasy);
            grpDifficulty.Location = new Point(1096, 223);
            grpDifficulty.Margin = new Padding(6);
            grpDifficulty.Name = "grpDifficulty";
            grpDifficulty.Padding = new Padding(6);
            grpDifficulty.Size = new Size(746, 151);
            grpDifficulty.TabIndex = 5;
            grpDifficulty.TabStop = false;
            grpDifficulty.Text = "난이도 선택";
            // 
            // rbHard
            // 
            rbHard.AutoSize = true;
            rbHard.Location = new Point(562, 66);
            rbHard.Margin = new Padding(6);
            rbHard.Name = "rbHard";
            rbHard.Size = new Size(117, 36);
            rbHard.TabIndex = 2;
            rbHard.TabStop = true;
            rbHard.Text = "어려움";
            rbHard.UseVisualStyleBackColor = true;
            // 
            // rbNormal
            // 
            rbNormal.AutoSize = true;
            rbNormal.Location = new Point(311, 66);
            rbNormal.Margin = new Padding(6);
            rbNormal.Name = "rbNormal";
            rbNormal.Size = new Size(93, 36);
            rbNormal.TabIndex = 1;
            rbNormal.TabStop = true;
            rbNormal.Text = "보통";
            rbNormal.UseVisualStyleBackColor = true;
            // 
            // rbEasy
            // 
            rbEasy.AutoSize = true;
            rbEasy.Location = new Point(49, 66);
            rbEasy.Margin = new Padding(6);
            rbEasy.Name = "rbEasy";
            rbEasy.Size = new Size(93, 36);
            rbEasy.TabIndex = 0;
            rbEasy.TabStop = true;
            rbEasy.Text = "쉬움";
            rbEasy.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            btnStart.BackColor = SystemColors.ActiveCaption;
            btnStart.Font = new Font("맑은 고딕", 12.2264156F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnStart.Location = new Point(795, 414);
            btnStart.Margin = new Padding(6);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(400, 85);
            btnStart.TabIndex = 3;
            btnStart.Text = "게임시작";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // rankingBoardControl1
            // 
            rankingBoardControl1.Location = new Point(140, 558);
            rankingBoardControl1.Margin = new Padding(12, 11, 12, 11);
            rankingBoardControl1.Name = "rankingBoardControl1";
            rankingBoardControl1.Size = new Size(1702, 841);
            rankingBoardControl1.TabIndex = 2;
            // 
            // MainMenuForm
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1968, 1557);
            Controls.Add(rankingBoardControl1);
            Controls.Add(btnStart);
            Controls.Add(grpDifficulty);
            Controls.Add(grpBoardSize);
            Controls.Add(lblSubTitle);
            Controls.Add(lblTitle);
            Margin = new Padding(6);
            Name = "MainMenuForm";
            Text = "Form1";
            grpBoardSize.ResumeLayout(false);
            grpBoardSize.PerformLayout();
            grpDifficulty.ResumeLayout(false);
            grpDifficulty.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitle;
        private Label lblSubTitle;
        private GroupBox grpBoardSize;
        private RadioButton rbLarge;
        private RadioButton rbMedium;
        private RadioButton rbSmall;
        private GroupBox grpDifficulty;
        private RadioButton rbHard;
        private RadioButton rbNormal;
        private RadioButton rbEasy;
        private Button btnStart;
        private Components.RankingBoardControl rankingBoardControl1;
    }
}