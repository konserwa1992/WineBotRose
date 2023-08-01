namespace CodeInject
{
    partial class cBot
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.bRemoveMonster2Attack = new System.Windows.Forms.Button();
            this.bAddMonster2Attack = new System.Windows.Forms.Button();
            this.lMonster2Attack = new System.Windows.Forms.ListBox();
            this.lFullMonsterList = new System.Windows.Forms.ListBox();
            this.bHuntToggle = new System.Windows.Forms.Button();
            this.tSearchMobTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lNPClist = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bSkillRemove = new System.Windows.Forms.Button();
            this.bSkillAdd = new System.Windows.Forms.Button();
            this.lUseSkill = new System.Windows.Forms.ListBox();
            this.bSkillRefresh = new System.Windows.Forms.Button();
            this.lSkillList = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tPickupRadius = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lNearItemsList = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.pickUpTimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(498, 521);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.bRemoveMonster2Attack);
            this.tabPage1.Controls.Add(this.bAddMonster2Attack);
            this.tabPage1.Controls.Add(this.lMonster2Attack);
            this.tabPage1.Controls.Add(this.lFullMonsterList);
            this.tabPage1.Controls.Add(this.bHuntToggle);
            this.tabPage1.Controls.Add(this.tSearchMobTextBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(490, 495);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Hunt";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 7;
            // 
            // bRemoveMonster2Attack
            // 
            this.bRemoveMonster2Attack.Location = new System.Drawing.Point(359, 168);
            this.bRemoveMonster2Attack.Name = "bRemoveMonster2Attack";
            this.bRemoveMonster2Attack.Size = new System.Drawing.Size(75, 23);
            this.bRemoveMonster2Attack.TabIndex = 6;
            this.bRemoveMonster2Attack.Text = "Remove";
            this.bRemoveMonster2Attack.UseVisualStyleBackColor = true;
            this.bRemoveMonster2Attack.Click += new System.EventHandler(this.bRemoveMonster2Attack_Click);
            // 
            // bAddMonster2Attack
            // 
            this.bAddMonster2Attack.Location = new System.Drawing.Point(235, 168);
            this.bAddMonster2Attack.Name = "bAddMonster2Attack";
            this.bAddMonster2Attack.Size = new System.Drawing.Size(75, 23);
            this.bAddMonster2Attack.TabIndex = 5;
            this.bAddMonster2Attack.Text = "Add";
            this.bAddMonster2Attack.UseVisualStyleBackColor = true;
            this.bAddMonster2Attack.Click += new System.EventHandler(this.bAddMonster2Attack_Click);
            // 
            // lMonster2Attack
            // 
            this.lMonster2Attack.FormattingEnabled = true;
            this.lMonster2Attack.Location = new System.Drawing.Point(215, 198);
            this.lMonster2Attack.Name = "lMonster2Attack";
            this.lMonster2Attack.Size = new System.Drawing.Size(257, 147);
            this.lMonster2Attack.TabIndex = 4;
            // 
            // lFullMonsterList
            // 
            this.lFullMonsterList.FormattingEnabled = true;
            this.lFullMonsterList.Location = new System.Drawing.Point(215, 67);
            this.lFullMonsterList.Name = "lFullMonsterList";
            this.lFullMonsterList.Size = new System.Drawing.Size(257, 95);
            this.lFullMonsterList.TabIndex = 3;
            // 
            // bHuntToggle
            // 
            this.bHuntToggle.Location = new System.Drawing.Point(381, 463);
            this.bHuntToggle.Name = "bHuntToggle";
            this.bHuntToggle.Size = new System.Drawing.Size(103, 23);
            this.bHuntToggle.TabIndex = 1;
            this.bHuntToggle.Text = "Start";
            this.bHuntToggle.UseVisualStyleBackColor = true;
            this.bHuntToggle.Click += new System.EventHandler(this.bHuntToggle_Click);
            // 
            // tSearchMobTextBox
            // 
            this.tSearchMobTextBox.Location = new System.Drawing.Point(215, 41);
            this.tSearchMobTextBox.Name = "tSearchMobTextBox";
            this.tSearchMobTextBox.Size = new System.Drawing.Size(257, 20);
            this.tSearchMobTextBox.TabIndex = 2;
            this.tSearchMobTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search monster:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lNPClist);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 480);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "NPC List";
            // 
            // lNPClist
            // 
            this.lNPClist.FormattingEnabled = true;
            this.lNPClist.Location = new System.Drawing.Point(6, 19);
            this.lNPClist.Name = "lNPClist";
            this.lNPClist.Size = new System.Drawing.Size(188, 446);
            this.lNPClist.TabIndex = 1;
            this.lNPClist.SelectedIndexChanged += new System.EventHandler(this.lNPClist_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bSkillRemove);
            this.tabPage2.Controls.Add(this.bSkillAdd);
            this.tabPage2.Controls.Add(this.lUseSkill);
            this.tabPage2.Controls.Add(this.bSkillRefresh);
            this.tabPage2.Controls.Add(this.lSkillList);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(490, 495);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Skill";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bSkillRemove
            // 
            this.bSkillRemove.Location = new System.Drawing.Point(132, 84);
            this.bSkillRemove.Name = "bSkillRemove";
            this.bSkillRemove.Size = new System.Drawing.Size(42, 23);
            this.bSkillRemove.TabIndex = 6;
            this.bSkillRemove.Text = "<";
            this.bSkillRemove.UseVisualStyleBackColor = true;
            this.bSkillRemove.Click += new System.EventHandler(this.bSkillRemove_Click);
            // 
            // bSkillAdd
            // 
            this.bSkillAdd.Location = new System.Drawing.Point(132, 55);
            this.bSkillAdd.Name = "bSkillAdd";
            this.bSkillAdd.Size = new System.Drawing.Size(42, 23);
            this.bSkillAdd.TabIndex = 5;
            this.bSkillAdd.Text = ">";
            this.bSkillAdd.UseVisualStyleBackColor = true;
            this.bSkillAdd.Click += new System.EventHandler(this.bSkillAdd_Click);
            // 
            // lUseSkill
            // 
            this.lUseSkill.FormattingEnabled = true;
            this.lUseSkill.Location = new System.Drawing.Point(180, 6);
            this.lUseSkill.Name = "lUseSkill";
            this.lUseSkill.Size = new System.Drawing.Size(120, 134);
            this.lUseSkill.TabIndex = 4;
            // 
            // bSkillRefresh
            // 
            this.bSkillRefresh.Location = new System.Drawing.Point(6, 146);
            this.bSkillRefresh.Name = "bSkillRefresh";
            this.bSkillRefresh.Size = new System.Drawing.Size(120, 23);
            this.bSkillRefresh.TabIndex = 3;
            this.bSkillRefresh.Text = "Refresh";
            this.bSkillRefresh.UseVisualStyleBackColor = true;
            this.bSkillRefresh.Click += new System.EventHandler(this.bSkillRefresh_Click);
            // 
            // lSkillList
            // 
            this.lSkillList.FormattingEnabled = true;
            this.lSkillList.Location = new System.Drawing.Point(6, 6);
            this.lSkillList.Name = "lSkillList";
            this.lSkillList.Size = new System.Drawing.Size(120, 134);
            this.lSkillList.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.tPickupRadius);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(490, 495);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Pick UP";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(212, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Pick up radius";
            // 
            // tPickupRadius
            // 
            this.tPickupRadius.Location = new System.Drawing.Point(212, 31);
            this.tPickupRadius.Name = "tPickupRadius";
            this.tPickupRadius.Size = new System.Drawing.Size(100, 20);
            this.tPickupRadius.TabIndex = 4;
            this.tPickupRadius.Text = "10,0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.lNearItemsList);
            this.groupBox2.Location = new System.Drawing.Point(6, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 474);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Near items";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 445);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(194, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // lNearItemsList
            // 
            this.lNearItemsList.FormattingEnabled = true;
            this.lNearItemsList.Location = new System.Drawing.Point(6, 19);
            this.lNearItemsList.Name = "lNearItemsList";
            this.lNearItemsList.Size = new System.Drawing.Size(188, 407);
            this.lNearItemsList.TabIndex = 2;
            this.lNearItemsList.SelectedIndexChanged += new System.EventHandler(this.lNearItemsList_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // pickUpTimer
            // 
            this.pickUpTimer.Interval = 150;
            this.pickUpTimer.Tick += new System.EventHandler(this.pickUpTimer_Tick);
            // 
            // cBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 522);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "cBot";
            this.Text = "cBot";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lNPClist;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button bSkillRefresh;
        private System.Windows.Forms.ListBox lSkillList;
        private System.Windows.Forms.ListBox lUseSkill;
        private System.Windows.Forms.Button bSkillRemove;
        private System.Windows.Forms.Button bSkillAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tSearchMobTextBox;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button bHuntToggle;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Timer pickUpTimer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lNearItemsList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tPickupRadius;
        private System.Windows.Forms.ListBox lFullMonsterList;
        private System.Windows.Forms.Button bAddMonster2Attack;
        private System.Windows.Forms.ListBox lMonster2Attack;
        private System.Windows.Forms.Button bRemoveMonster2Attack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}