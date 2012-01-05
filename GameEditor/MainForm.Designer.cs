namespace GameEditor
{
  partial class MainForm
  {
    /// <summary>
    /// Требуется переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором форм Windows

    /// <summary>
    /// Обязательный метод для поддержки конструктора - не изменяйте
    /// содержимое данного метода при помощи редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
      this.ODForFileSelect = new System.Windows.Forms.OpenFileDialog();
      this.LMapPath = new System.Windows.Forms.Label();
      this.PMapPictBox = new System.Windows.Forms.Panel();
      this.PBMap = new System.Windows.Forms.PictureBox();
      this.BSelectMap = new System.Windows.Forms.Button();
      this.GBMapManage = new System.Windows.Forms.GroupBox();
      this.BNewGameConfig = new System.Windows.Forms.Button();
      this.BSave = new System.Windows.Forms.Button();
      this.BLoad = new System.Windows.Forms.Button();
      this.GBLevelConfig = new System.Windows.Forms.GroupBox();
      this.BPrevLevel = new System.Windows.Forms.Button();
      this.BNextLevel = new System.Windows.Forms.Button();
      this.nUDCanvaSpeed = new System.Windows.Forms.NumericUpDown();
      this.mTBGoldForKill = new System.Windows.Forms.MaskedTextBox();
      this.mTBHealthPoints = new System.Windows.Forms.MaskedTextBox();
      this.BRemoveLevel = new System.Windows.Forms.Button();
      this.LGoldForKill = new System.Windows.Forms.Label();
      this.BAddLevel = new System.Windows.Forms.Button();
      this.LMovingSpeed = new System.Windows.Forms.Label();
      this.LHealtPoints = new System.Windows.Forms.Label();
      this.BLoadMonsterPict = new System.Windows.Forms.Button();
      this.mTBNumberOfPhases = new System.Windows.Forms.MaskedTextBox();
      this.LBDirectionSelect = new System.Windows.Forms.ListBox();
      this.LMonsterSelect = new System.Windows.Forms.Label();
      this.LNumberOfPhases = new System.Windows.Forms.Label();
      this.PMonsterPict = new System.Windows.Forms.Panel();
      this.PBMosterPict = new System.Windows.Forms.PictureBox();
      this.LCurrentNCountLevel = new System.Windows.Forms.Label();
      this.LTowerPath = new System.Windows.Forms.Label();
      this.TBTowerFolder = new System.Windows.Forms.TextBox();
      this.PMapPictBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PBMap)).BeginInit();
      this.GBMapManage.SuspendLayout();
      this.GBLevelConfig.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nUDCanvaSpeed)).BeginInit();
      this.PMonsterPict.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PBMosterPict)).BeginInit();
      this.SuspendLayout();
      // 
      // ODForFileSelect
      // 
      this.ODForFileSelect.FileName = "*.*";
      this.ODForFileSelect.Filter = "Все файлы|*.*";
      // 
      // LMapPath
      // 
      this.LMapPath.AutoSize = true;
      this.LMapPath.Location = new System.Drawing.Point(6, 25);
      this.LMapPath.Name = "LMapPath";
      this.LMapPath.Size = new System.Drawing.Size(142, 24);
      this.LMapPath.TabIndex = 0;
      this.LMapPath.Text = "Path to map file:";
      // 
      // PMapPictBox
      // 
      this.PMapPictBox.AutoScroll = true;
      this.PMapPictBox.Controls.Add(this.PBMap);
      this.PMapPictBox.Location = new System.Drawing.Point(10, 52);
      this.PMapPictBox.Name = "PMapPictBox";
      this.PMapPictBox.Size = new System.Drawing.Size(473, 173);
      this.PMapPictBox.TabIndex = 4;
      // 
      // PBMap
      // 
      this.PBMap.Location = new System.Drawing.Point(3, 3);
      this.PBMap.Name = "PBMap";
      this.PBMap.Size = new System.Drawing.Size(470, 167);
      this.PBMap.TabIndex = 0;
      this.PBMap.TabStop = false;
      // 
      // BSelectMap
      // 
      this.BSelectMap.Location = new System.Drawing.Point(489, 55);
      this.BSelectMap.Name = "BSelectMap";
      this.BSelectMap.Size = new System.Drawing.Size(108, 32);
      this.BSelectMap.TabIndex = 5;
      this.BSelectMap.Text = "Set map";
      this.BSelectMap.UseVisualStyleBackColor = true;
      this.BSelectMap.Click += new System.EventHandler(this.BSelectMap_Click);
      // 
      // GBMapManage
      // 
      this.GBMapManage.Controls.Add(this.BSelectMap);
      this.GBMapManage.Controls.Add(this.PMapPictBox);
      this.GBMapManage.Controls.Add(this.LMapPath);
      this.GBMapManage.Enabled = false;
      this.GBMapManage.Location = new System.Drawing.Point(555, 12);
      this.GBMapManage.Name = "GBMapManage";
      this.GBMapManage.Size = new System.Drawing.Size(603, 234);
      this.GBMapManage.TabIndex = 6;
      this.GBMapManage.TabStop = false;
      this.GBMapManage.Text = "Map manage";
      // 
      // BNewGameConfig
      // 
      this.BNewGameConfig.Location = new System.Drawing.Point(12, 12);
      this.BNewGameConfig.Name = "BNewGameConfig";
      this.BNewGameConfig.Size = new System.Drawing.Size(231, 33);
      this.BNewGameConfig.TabIndex = 7;
      this.BNewGameConfig.Tag = "0";
      this.BNewGameConfig.Text = "New game configuration";
      this.BNewGameConfig.UseVisualStyleBackColor = true;
      this.BNewGameConfig.Click += new System.EventHandler(this.BNewGameConfig_Click);
      // 
      // BSave
      // 
      this.BSave.Location = new System.Drawing.Point(249, 12);
      this.BSave.Name = "BSave";
      this.BSave.Size = new System.Drawing.Size(75, 33);
      this.BSave.TabIndex = 8;
      this.BSave.Text = "Save";
      this.BSave.UseVisualStyleBackColor = true;
      this.BSave.Click += new System.EventHandler(this.BSave_Click);
      // 
      // BLoad
      // 
      this.BLoad.Location = new System.Drawing.Point(330, 12);
      this.BLoad.Name = "BLoad";
      this.BLoad.Size = new System.Drawing.Size(75, 33);
      this.BLoad.TabIndex = 9;
      this.BLoad.Text = "Load";
      this.BLoad.UseVisualStyleBackColor = true;
      // 
      // GBLevelConfig
      // 
      this.GBLevelConfig.Controls.Add(this.BPrevLevel);
      this.GBLevelConfig.Controls.Add(this.BNextLevel);
      this.GBLevelConfig.Controls.Add(this.nUDCanvaSpeed);
      this.GBLevelConfig.Controls.Add(this.mTBGoldForKill);
      this.GBLevelConfig.Controls.Add(this.mTBHealthPoints);
      this.GBLevelConfig.Controls.Add(this.BRemoveLevel);
      this.GBLevelConfig.Controls.Add(this.LGoldForKill);
      this.GBLevelConfig.Controls.Add(this.BAddLevel);
      this.GBLevelConfig.Controls.Add(this.LMovingSpeed);
      this.GBLevelConfig.Controls.Add(this.LHealtPoints);
      this.GBLevelConfig.Controls.Add(this.BLoadMonsterPict);
      this.GBLevelConfig.Controls.Add(this.mTBNumberOfPhases);
      this.GBLevelConfig.Controls.Add(this.LBDirectionSelect);
      this.GBLevelConfig.Controls.Add(this.LMonsterSelect);
      this.GBLevelConfig.Controls.Add(this.LNumberOfPhases);
      this.GBLevelConfig.Controls.Add(this.PMonsterPict);
      this.GBLevelConfig.Enabled = false;
      this.GBLevelConfig.Location = new System.Drawing.Point(25, 251);
      this.GBLevelConfig.Name = "GBLevelConfig";
      this.GBLevelConfig.Size = new System.Drawing.Size(834, 294);
      this.GBLevelConfig.TabIndex = 10;
      this.GBLevelConfig.TabStop = false;
      this.GBLevelConfig.Text = "Level configuration";
      // 
      // BPrevLevel
      // 
      this.BPrevLevel.Enabled = false;
      this.BPrevLevel.Location = new System.Drawing.Point(102, 258);
      this.BPrevLevel.Name = "BPrevLevel";
      this.BPrevLevel.Size = new System.Drawing.Size(136, 30);
      this.BPrevLevel.TabIndex = 16;
      this.BPrevLevel.Text = "Previous level";
      this.BPrevLevel.UseVisualStyleBackColor = true;
      this.BPrevLevel.Click += new System.EventHandler(this.BPrevLevel_Click);
      // 
      // BNextLevel
      // 
      this.BNextLevel.Enabled = false;
      this.BNextLevel.Location = new System.Drawing.Point(244, 258);
      this.BNextLevel.Name = "BNextLevel";
      this.BNextLevel.Size = new System.Drawing.Size(120, 30);
      this.BNextLevel.TabIndex = 15;
      this.BNextLevel.Text = "Next level";
      this.BNextLevel.UseVisualStyleBackColor = true;
      this.BNextLevel.Click += new System.EventHandler(this.BNextLevel_Click);
      // 
      // nUDCanvaSpeed
      // 
      this.nUDCanvaSpeed.Location = new System.Drawing.Point(315, 96);
      this.nUDCanvaSpeed.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
      this.nUDCanvaSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.nUDCanvaSpeed.Name = "nUDCanvaSpeed";
      this.nUDCanvaSpeed.Size = new System.Drawing.Size(82, 29);
      this.nUDCanvaSpeed.TabIndex = 14;
      this.nUDCanvaSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.nUDCanvaSpeed.Validated += new System.EventHandler(this.nUDCanvaSpeed_Validated);
      // 
      // mTBGoldForKill
      // 
      this.mTBGoldForKill.Location = new System.Drawing.Point(145, 138);
      this.mTBGoldForKill.Mask = "00000";
      this.mTBGoldForKill.Name = "mTBGoldForKill";
      this.mTBGoldForKill.Size = new System.Drawing.Size(100, 29);
      this.mTBGoldForKill.TabIndex = 13;
      this.mTBGoldForKill.Text = "10";
      this.mTBGoldForKill.ValidatingType = typeof(int);
      this.mTBGoldForKill.TextChanged += new System.EventHandler(this.mTBGoldForKill_TextChanged);
      // 
      // mTBHealthPoints
      // 
      this.mTBHealthPoints.Location = new System.Drawing.Point(125, 74);
      this.mTBHealthPoints.Mask = "000000";
      this.mTBHealthPoints.Name = "mTBHealthPoints";
      this.mTBHealthPoints.Size = new System.Drawing.Size(100, 29);
      this.mTBHealthPoints.TabIndex = 11;
      this.mTBHealthPoints.Text = "100";
      this.mTBHealthPoints.TextChanged += new System.EventHandler(this.mTBHealthPoints_TextChanged);
      // 
      // BRemoveLevel
      // 
      this.BRemoveLevel.Enabled = false;
      this.BRemoveLevel.Location = new System.Drawing.Point(163, 28);
      this.BRemoveLevel.Name = "BRemoveLevel";
      this.BRemoveLevel.Size = new System.Drawing.Size(136, 32);
      this.BRemoveLevel.TabIndex = 12;
      this.BRemoveLevel.Text = "Remove level";
      this.BRemoveLevel.UseVisualStyleBackColor = true;
      this.BRemoveLevel.Click += new System.EventHandler(this.BRemoveLevel_Click);
      // 
      // LGoldForKill
      // 
      this.LGoldForKill.AutoSize = true;
      this.LGoldForKill.Location = new System.Drawing.Point(6, 141);
      this.LGoldForKill.Name = "LGoldForKill";
      this.LGoldForKill.Size = new System.Drawing.Size(133, 24);
      this.LGoldForKill.TabIndex = 10;
      this.LGoldForKill.Text = "Gold for killing:";
      // 
      // BAddLevel
      // 
      this.BAddLevel.Location = new System.Drawing.Point(10, 28);
      this.BAddLevel.Name = "BAddLevel";
      this.BAddLevel.Size = new System.Drawing.Size(136, 32);
      this.BAddLevel.TabIndex = 11;
      this.BAddLevel.Text = "Add level";
      this.BAddLevel.UseVisualStyleBackColor = true;
      this.BAddLevel.Click += new System.EventHandler(this.BAddLevel_Click);
      // 
      // LMovingSpeed
      // 
      this.LMovingSpeed.AutoSize = true;
      this.LMovingSpeed.Location = new System.Drawing.Point(6, 101);
      this.LMovingSpeed.Name = "LMovingSpeed";
      this.LMovingSpeed.Size = new System.Drawing.Size(303, 24);
      this.LMovingSpeed.TabIndex = 9;
      this.LMovingSpeed.Text = "MovingSpeed(pixel per game tick):";
      // 
      // LHealtPoints
      // 
      this.LHealtPoints.AutoSize = true;
      this.LHealtPoints.Location = new System.Drawing.Point(6, 77);
      this.LHealtPoints.Name = "LHealtPoints";
      this.LHealtPoints.Size = new System.Drawing.Size(113, 24);
      this.LHealtPoints.TabIndex = 8;
      this.LHealtPoints.Text = "Healt points:";
      // 
      // BLoadMonsterPict
      // 
      this.BLoadMonsterPict.Enabled = false;
      this.BLoadMonsterPict.Location = new System.Drawing.Point(381, 258);
      this.BLoadMonsterPict.Name = "BLoadMonsterPict";
      this.BLoadMonsterPict.Size = new System.Drawing.Size(220, 30);
      this.BLoadMonsterPict.TabIndex = 7;
      this.BLoadMonsterPict.Text = "Load monster picture";
      this.BLoadMonsterPict.UseVisualStyleBackColor = true;
      this.BLoadMonsterPict.Click += new System.EventHandler(this.BLoadMonsterPict_Click);
      // 
      // mTBNumberOfPhases
      // 
      this.mTBNumberOfPhases.Location = new System.Drawing.Point(504, 63);
      this.mTBNumberOfPhases.Mask = "00000";
      this.mTBNumberOfPhases.Name = "mTBNumberOfPhases";
      this.mTBNumberOfPhases.Size = new System.Drawing.Size(100, 29);
      this.mTBNumberOfPhases.TabIndex = 6;
      this.mTBNumberOfPhases.Text = "2";
      this.mTBNumberOfPhases.ValidatingType = typeof(int);
      this.mTBNumberOfPhases.TextChanged += new System.EventHandler(this.mTBNumberOfPhases_TextChanged);
      // 
      // LBDirectionSelect
      // 
      this.LBDirectionSelect.FormattingEnabled = true;
      this.LBDirectionSelect.ItemHeight = 24;
      this.LBDirectionSelect.Items.AddRange(new object[] {
            "Up",
            "Right",
            "Down",
            "Left"});
      this.LBDirectionSelect.Location = new System.Drawing.Point(481, 152);
      this.LBDirectionSelect.Name = "LBDirectionSelect";
      this.LBDirectionSelect.Size = new System.Drawing.Size(120, 100);
      this.LBDirectionSelect.TabIndex = 5;
      this.LBDirectionSelect.SelectedIndexChanged += new System.EventHandler(this.LBDirectionSelect_SelectedIndexChanged);
      // 
      // LMonsterSelect
      // 
      this.LMonsterSelect.AutoSize = true;
      this.LMonsterSelect.Location = new System.Drawing.Point(387, 125);
      this.LMonsterSelect.Name = "LMonsterSelect";
      this.LMonsterSelect.Size = new System.Drawing.Size(217, 24);
      this.LMonsterSelect.TabIndex = 4;
      this.LMonsterSelect.Text = "Select monster direction:";
      // 
      // LNumberOfPhases
      // 
      this.LNumberOfPhases.AutoSize = true;
      this.LNumberOfPhases.Location = new System.Drawing.Point(372, 32);
      this.LNumberOfPhases.Name = "LNumberOfPhases";
      this.LNumberOfPhases.Size = new System.Drawing.Size(232, 24);
      this.LNumberOfPhases.TabIndex = 2;
      this.LNumberOfPhases.Text = "Number of moving phases";
      // 
      // PMonsterPict
      // 
      this.PMonsterPict.AutoScroll = true;
      this.PMonsterPict.Controls.Add(this.PBMosterPict);
      this.PMonsterPict.Location = new System.Drawing.Point(607, 25);
      this.PMonsterPict.Name = "PMonsterPict";
      this.PMonsterPict.Size = new System.Drawing.Size(217, 260);
      this.PMonsterPict.TabIndex = 0;
      // 
      // PBMosterPict
      // 
      this.PBMosterPict.Location = new System.Drawing.Point(3, 3);
      this.PBMosterPict.Name = "PBMosterPict";
      this.PBMosterPict.Size = new System.Drawing.Size(210, 254);
      this.PBMosterPict.TabIndex = 0;
      this.PBMosterPict.TabStop = false;
      // 
      // LCurrentNCountLevel
      // 
      this.LCurrentNCountLevel.AutoSize = true;
      this.LCurrentNCountLevel.Location = new System.Drawing.Point(21, 224);
      this.LCurrentNCountLevel.Name = "LCurrentNCountLevel";
      this.LCurrentNCountLevel.Size = new System.Drawing.Size(90, 24);
      this.LCurrentNCountLevel.TabIndex = 13;
      this.LCurrentNCountLevel.Text = "Level: 0/0";
      // 
      // LTowerPath
      // 
      this.LTowerPath.AutoSize = true;
      this.LTowerPath.Location = new System.Drawing.Point(12, 200);
      this.LTowerPath.Name = "LTowerPath";
      this.LTowerPath.Size = new System.Drawing.Size(306, 24);
      this.LTowerPath.TabIndex = 14;
      this.LTowerPath.Text = "Name of towers configuration folder";
      // 
      // TBTowerFolder
      // 
      this.TBTowerFolder.Location = new System.Drawing.Point(324, 195);
      this.TBTowerFolder.MaxLength = 200;
      this.TBTowerFolder.Name = "TBTowerFolder";
      this.TBTowerFolder.Size = new System.Drawing.Size(225, 29);
      this.TBTowerFolder.TabIndex = 15;
      this.TBTowerFolder.Text = "Demo";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1174, 557);
      this.Controls.Add(this.TBTowerFolder);
      this.Controls.Add(this.LTowerPath);
      this.Controls.Add(this.LCurrentNCountLevel);
      this.Controls.Add(this.GBLevelConfig);
      this.Controls.Add(this.BLoad);
      this.Controls.Add(this.BSave);
      this.Controls.Add(this.BNewGameConfig);
      this.Controls.Add(this.GBMapManage);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.Margin = new System.Windows.Forms.Padding(6);
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "GameEditor";
      this.PMapPictBox.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PBMap)).EndInit();
      this.GBMapManage.ResumeLayout(false);
      this.GBMapManage.PerformLayout();
      this.GBLevelConfig.ResumeLayout(false);
      this.GBLevelConfig.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nUDCanvaSpeed)).EndInit();
      this.PMonsterPict.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PBMosterPict)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog ODForFileSelect;
    private System.Windows.Forms.Label LMapPath;
    private System.Windows.Forms.Panel PMapPictBox;
    private System.Windows.Forms.PictureBox PBMap;
    private System.Windows.Forms.Button BSelectMap;
    private System.Windows.Forms.GroupBox GBMapManage;
    private System.Windows.Forms.Button BNewGameConfig;
    private System.Windows.Forms.Button BSave;
    private System.Windows.Forms.Button BLoad;
    private System.Windows.Forms.GroupBox GBLevelConfig;
    private System.Windows.Forms.Button BAddLevel;
    private System.Windows.Forms.Button BRemoveLevel;
    private System.Windows.Forms.Label LCurrentNCountLevel;
    private System.Windows.Forms.Label LTowerPath;
    private System.Windows.Forms.TextBox TBTowerFolder;
    private System.Windows.Forms.Panel PMonsterPict;
    private System.Windows.Forms.PictureBox PBMosterPict;
    private System.Windows.Forms.Label LMonsterSelect;
    private System.Windows.Forms.Label LNumberOfPhases;
    private System.Windows.Forms.ListBox LBDirectionSelect;
    private System.Windows.Forms.MaskedTextBox mTBNumberOfPhases;
    private System.Windows.Forms.Button BLoadMonsterPict;
    private System.Windows.Forms.Label LGoldForKill;
    private System.Windows.Forms.Label LMovingSpeed;
    private System.Windows.Forms.Label LHealtPoints;
    private System.Windows.Forms.MaskedTextBox mTBGoldForKill;
    private System.Windows.Forms.MaskedTextBox mTBHealthPoints;
    private System.Windows.Forms.NumericUpDown nUDCanvaSpeed;
    private System.Windows.Forms.Button BPrevLevel;
    private System.Windows.Forms.Button BNextLevel;
  }
}

