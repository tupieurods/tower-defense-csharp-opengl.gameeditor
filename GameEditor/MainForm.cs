using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using GameCoClassLibrary.Structures;
using GameCoClassLibrary.Enums;
using GameCoClassLibrary.Classes;
using GameCoClassLibrary.Loaders;
using GameEditor.Properties;
using Settings = GameCoClassLibrary.Classes.Settings;

namespace GameEditor
{
  public partial class MainForm : Form
  {
    #region delegats

    /// <summary>
    /// Parses inValue string and sets to checkResult if inValue valid
    /// </summary>
    /// <param name="inValue">Value to set.</param>
    /// <param name="valueBeforeChange">The value before change.</param>
    /// <param name="checkResult">The check result.</param>
    delegate void Check(string inValue, int valueBeforeChange, out int checkResult);

    #endregion

    /// <summary>
    /// All level configurations
    /// </summary>
    private List<MonsterParam> _levelsConfig;
    /// <summary>
    /// Number of monsters at every level
    /// </summary>
    private List<int> _numberOfMonstersAtLevel;//Число монстров на каждлм из уровней
    /// <summary>
    /// Bonus for successful level finishing
    /// </summary>
    private List<int> _goldForSuccessfulLevelFinish;//Золото за успешное завершение уровня
    /// <summary>
    /// Bonus for monster killing at every level
    /// </summary>
    private List<int> _goldForKillMonster;
    /// <summary>
    /// Current level(in configurator)
    /// </summary>
    private int _currentLevel;//показывает текущий уровень
    /// <summary>
    /// True: Parameter was changed by user, needs saving 
    /// False: Parametr was changed by program, doesn't need saving 
    /// </summary>
    private bool _realChange;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
      _realChange = false;
      _currentLevel = 0;
      InitializeComponent();
      _levelsConfig = new List<MonsterParam>();
      _numberOfMonstersAtLevel = new List<int>();
    }

    #region Установление новых настроек по умолчанию для нового уровня/игровой конфигурации
    /// <summary>
    /// Sets all parameter of level to default, when new level adding
    /// </summary>
    private void DefaultForNewLevel()
    {
      _realChange = false;
      // ReSharper disable LocalizableElement
      mTBGoldForKill.Text = "10";
      mTBHealthPoints.Text = "100";
      mTBNumberOfPhases.Text = "1";
      mTBNumberOfMonstersAtLevel.Text = "20";
      mTBGoldForSuccessfulLevelFinish.Text = "40";
      // ReSharper restore LocalizableElement
      nUDCanvaSpeed.Value = 1;
      PBMosterPict.Image = null;
      RBLeftDirection.Checked = true;
      CBLevelInvisible.Checked = false;
      _realChange = true;
    }

    /// <summary>
    /// Sets all parameter of game to default, when new game creating
    /// </summary>
    private void SetDefault()
    {
      DefaultForNewLevel();
      // ReSharper disable LocalizableElement
      LCurrentNCountLevel.Text = "Level: 0/0";
      TBTowerFolder.Text = "Demo";
      mTBGoldAtStart.Text = "40";
      mTBNumberOfLives.Text = "20";
      // ReSharper restore LocalizableElement
      _levelsConfig = new List<MonsterParam>();
      _numberOfMonstersAtLevel = new List<int>();
      _goldForSuccessfulLevelFinish = new List<int>();
      _goldForKillMonster = new List<int>();
    }

    /// <summary>
    /// Creating a new configuration of the game and setting all the settings
    /// </summary>
    private void CreateNewConfiguration()
    {
      BNewGameConfig.Tag = 1;
      GBLevelConfig.Enabled = true;
      GBMapManage.Enabled = true;
      GBNumberOfDirections.Enabled = false;
      BNextLevel.Enabled = false;
      BPrevLevel.Enabled = false;
      BRemoveLevel.Enabled = false;
      BLoadMonsterPict.Enabled = false;
      CBLevelInvisible.Enabled = false;
      _currentLevel = 0;
      PBMap.Image = null;
      PBMap.Size = new Size(10, 10);
      LMapName.Text = "Map name:";
      SetDefault();
      BSave.Enabled = true;
      _realChange = true;
    }
    #endregion

    /// <summary>
    /// Shows the level settings.
    /// </summary>
    /// <param name="levelNum">Number of the level.</param>
    private void ShowLevelSettings(int levelNum)
    {
      if (_levelsConfig.Count < levelNum)
        return;
      _realChange = false;
      mTBGoldForKill.Text = _goldForKillMonster[levelNum - 1].ToString(CultureInfo.InvariantCulture);
      mTBHealthPoints.Text = _levelsConfig[levelNum - 1].Base.HealthPoints.ToString(CultureInfo.InvariantCulture);
      mTBNumberOfPhases.Text = _levelsConfig[levelNum - 1].NumberOfPhases.ToString(CultureInfo.InvariantCulture);
      nUDCanvaSpeed.Value = Convert.ToDecimal(_levelsConfig[levelNum - 1].Base.CanvasSpeed);
      mTBArmor.Text = _levelsConfig[levelNum - 1].Base.Armor.ToString(CultureInfo.InvariantCulture);
      mTBNumberOfMonstersAtLevel.Text = _numberOfMonstersAtLevel[levelNum - 1].ToString(CultureInfo.InvariantCulture);
      mTBGoldForSuccessfulLevelFinish.Text = _goldForSuccessfulLevelFinish[levelNum - 1].ToString(CultureInfo.InvariantCulture);
      CBLevelInvisible.Checked = _levelsConfig[levelNum - 1].Base.Invisible;
      LCurrentNCountLevel.Text = "Level: " + levelNum.ToString(CultureInfo.InvariantCulture) + "/" + _levelsConfig.Count.ToString(CultureInfo.InvariantCulture);
      //Monster picture
      MonsterParam tmp = _levelsConfig[levelNum - 1];
      if (tmp[MonsterDirection.Left, 0] != null)
        DrawMonsterPhases(MonsterDirection.Left);
      else
      {
        PBMosterPict.Image = null;
        PBMosterPict.Size = new Size(210, 254);
      }
      switch (tmp.NumberOfDirectionsInFile)
      {
        case 1:
          RBLeftDirection.Checked = true;
          break;
        case 2:
          RBLetfAndUpDirections.Checked = true;
          break;
        case 4:
          RBAllFourDirections.Checked = true;
          break;
      }
      _realChange = true;
    }

    /// <summary>
    /// User clicked to button create New game configuration
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BNewGameConfig_Click(object sender, EventArgs e)
    {
      if (Convert.ToInt32(BNewGameConfig.Tag) == 0)
      {
        CreateNewConfiguration();
        return;
      }
      if (Convert.ToInt32(BNewGameConfig.Tag) == 1)
      {
        if (MessageBox.Show(Resources.Game_configuration_not_saved, Resources.Applictaion_Title_InMessages, MessageBoxButtons.YesNo) == DialogResult.Yes)
        //If needs to save
        {//If user want to save current game configuration, before he create new one
          BSave_Click(sender, e);
          return;
        }
        CreateNewConfiguration();
        return;
      }
      if (Convert.ToInt32(BNewGameConfig.Tag) != 2) return;
      if (MessageBox.Show(Resources.Really_want_new, Resources.Applictaion_Title_InMessages, MessageBoxButtons.YesNo) !=
          DialogResult.Yes) return;
      CreateNewConfiguration();
    }

    #region Save/Load
    /// <summary>
    /// User clicked to button Save game configuration
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BSave_Click(object sender, EventArgs e)
    {
      if (SDForSaveConfiguration.ShowDialog() != DialogResult.OK) return;
      try//Saving main configuration
      {
        BinaryWriter saveMainConf = new BinaryWriter(new FileStream(SDForSaveConfiguration.FileName, FileMode.Create, FileAccess.Write));
        int tmp1 = 40;
        Int32.TryParse(mTBGoldAtStart.Text, out tmp1);
        int tmp2 = 20;
        Int32.TryParse(mTBNumberOfLives.Text, out tmp2);
        SaveNLoad.SaveMainGameConfig(saveMainConf, _numberOfMonstersAtLevel, _goldForSuccessfulLevelFinish, _goldForKillMonster, PBMap.Tag, TBTowerFolder.Text,
                                     _levelsConfig.Count, 5, tmp1, tmp2);
        saveMainConf.Close();
      }
      catch (Exception exc)
      {
        MessageBox.Show(Resources.Save_error + exc.Message);
        return;
      }
      string filePath = SDForSaveConfiguration.FileName.Substring(0, SDForSaveConfiguration.FileName.LastIndexOf('\\') + 1);
      string fileName = SDForSaveConfiguration.FileName.Substring(SDForSaveConfiguration.FileName.LastIndexOf('\\') + 1);
      fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
      try//Saving configurations of levels
      {
        FileStream levelConfSaveStream = new FileStream(filePath + fileName + ".tdlc", FileMode.Create, FileAccess.Write);
        IFormatter formatter = new BinaryFormatter();
        foreach (MonsterParam t in _levelsConfig)
          formatter.Serialize(levelConfSaveStream, t);
        levelConfSaveStream.Close();
      }
      catch (Exception exc)
      {
        MessageBox.Show(Resources.Save_error + exc.Message);
        return;
      }
      BNewGameConfig.Tag = 2;
    }

    /// <summary>
    /// User clicked to button Load game configuration
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BLoad_Click(object sender, EventArgs e)
    {
      ODForFileSelect.Filter = "File with game configuration|*.tdgc";
      ODForFileSelect.FileName = "*.tdgc";
      if (ODForFileSelect.ShowDialog() == DialogResult.OK)
      {
        if (ODForFileSelect.FileName.LastIndexOf(".tdgc", StringComparison.Ordinal) == -1)
        {
          MessageBox.Show("Wrong file selected");
          return;
        }
        int levelsCount;
        try//Loading main configuration
        {
          BinaryReader loadMainConf = new BinaryReader(new FileStream(ODForFileSelect.FileName, FileMode.Open, FileAccess.Read));
          object[] tmp;
          SaveNLoad.LoadMainGameConf(loadMainConf, out _numberOfMonstersAtLevel, out _goldForSuccessfulLevelFinish, out _goldForKillMonster, out tmp);
          string mapName = (string)tmp[0];//0-Map name
          if (!ShowMapByFileName(mapName))//If map file doesn't exists at path from configuration file, try to get it from folder with configuration
          {
            ShowMapByFileName(ODForFileSelect.FileName.Substring(0, ODForFileSelect.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1) + mapName.Substring(mapName.LastIndexOf("\\", StringComparison.Ordinal)));
          }
          TBTowerFolder.Text = (string)tmp[1];//1-Nam of the folder with towers configurations
          levelsCount = (int)tmp[2];//2-Number of levels
          mTBGoldAtStart.Text = Convert.ToInt32(tmp[4]).ToString(CultureInfo.InvariantCulture);//4-Money
          mTBNumberOfLives.Text = Convert.ToInt32(tmp[5]).ToString(CultureInfo.InvariantCulture);//5-Lives
          loadMainConf.Close();
        }
        catch (Exception exc)
        {
          MessageBox.Show(Resources.Load_error + exc.Message);
          return;
        }
        try//Loading configurations of levels
        {
          string levelFileName = ODForFileSelect.FileName.Substring(0, ODForFileSelect.FileName.LastIndexOf('.')) + ".tdlc";
          FileStream levelLoadStream = new FileStream(levelFileName, FileMode.Open, FileAccess.Read);
          IFormatter formatter = new BinaryFormatter();
          _levelsConfig.Clear();
          for (int i = 0; i < levelsCount; i++)
            _levelsConfig.Add((MonsterParam)(formatter.Deserialize(levelLoadStream)));
          levelLoadStream.Close();//All loaded, stream closing
          _currentLevel = 1;
          ShowLevelSettings(1);
          BRemoveLevel.Enabled = true;
          if (_levelsConfig.Count() > 1)
          {
            BLoadMonsterPict.Enabled = true;
            GBNumberOfDirections.Enabled = true;
          }
        }
        catch (Exception exc)
        {
          MessageBox.Show(Resources.Load_error + exc.Message);
          return;
        }
      }
      BNewGameConfig.Tag = 2;
      GBLevelConfig.Enabled = true;
      GBMapManage.Enabled = true;
      if (_levelsConfig.Count != 0)
        CBLevelInvisible.Enabled = true;
      if (_levelsConfig.Count > 1)
      {
        BPrevLevel.Enabled = true;
        BNextLevel.Enabled = true;
      }
      BSave.Enabled = true;
    }
    #endregion

    #region Map selection and loading
    /// <summary>
    /// Map file selection
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BSelectMap_Click(object sender, EventArgs e)
    {
      ODForFileSelect.Filter = "File with map configuration|*.efm";
      if (ODForFileSelect.ShowDialog() != DialogResult.OK) return;
      ShowMapByFileName(ODForFileSelect.FileName);
      //if (!ShowMapByFileName(ODForFileSelect.FileName)) return;
      /*
       *Used full name path
       *This is done for convenience.
       *When you start the game, map name will be parsed form this string
       *When you start the game editor, you don't need copy the map into folder with the game configurator
       */
    }

    /// <summary>
    /// Shows the map from map configuration file
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>true if map loaded successfully</returns>
    private bool ShowMapByFileName(string fileName)
    {
      try
      {
        if (fileName == string.Empty)
          throw new ArgumentNullException("fileName");
        PBMap.Image = null;
        PBMap.Size = new Size(0, 0);
        string mapNameGetting = fileName.Substring(fileName.LastIndexOf('\\') + 1);
        LMapName.Text = string.Format("Map name: {0}", mapNameGetting.Substring(0, mapNameGetting.IndexOf('.')));
        Map map = new Map(fileName);
        Bitmap tmpImg = new Bitmap(Convert.ToInt32(map.Width * Settings.ElemSize * map.Scaling),
          Convert.ToInt32(map.Height * Settings.ElemSize * map.Scaling));
        Graphics canva = Graphics.FromImage(tmpImg);//canva
        map.ShowOnGraphics(canva);
        PBMap.Width = Convert.ToInt32(map.Width * Settings.ElemSize * map.Scaling);//sizes setting
        PBMap.Height = Convert.ToInt32(map.Height * Settings.ElemSize * map.Scaling);
        PBMap.Image = tmpImg;
        PBMap.Tag = fileName;
        BNewGameConfig.Tag = 1;
      }
      catch (Exception e)
      {
        MessageBox.Show("Map loading error" + e.Message);
        return false;
      }
      MessageBox.Show("Map loaded Successful");
      return true;
    }
    #endregion

    #region Adding and removing levels
    /// <summary>
    /// New level adding
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BAddLevel_Click(object sender, EventArgs e)
    {
      MonsterParam tmp = new MonsterParam(1, 100, 1, 1, "", 1);
      _levelsConfig.Insert(_currentLevel++, tmp);
      _numberOfMonstersAtLevel.Insert(_currentLevel - 1, 20);
      _goldForSuccessfulLevelFinish.Insert(_currentLevel - 1, 40);
      _goldForKillMonster.Insert(_currentLevel - 1, 10);
      LCurrentNCountLevel.Text = "Level: " + _currentLevel.ToString(CultureInfo.InvariantCulture) + "/" + _levelsConfig.Count.ToString(CultureInfo.InvariantCulture);
      if (_levelsConfig.Count == 2)//If number of levels>1, user needs to switch between them
      {
        BNextLevel.Enabled = true;
        BPrevLevel.Enabled = true;
      }
      if (_levelsConfig.Count == 1)
      {
        BLoadMonsterPict.Enabled = true;//User can add monster picture
        GBNumberOfDirections.Enabled = true;
        CBLevelInvisible.Enabled = true;
      }
      DefaultForNewLevel();//Set a level template
      BRemoveLevel.Enabled = true;
      BNewGameConfig.Tag = 1;
    }

    /// <summary>
    /// Level removing
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BRemoveLevel_Click(object sender, EventArgs e)
    {
      _levelsConfig.RemoveAt(_currentLevel - 1);
      _numberOfMonstersAtLevel.RemoveAt(_currentLevel - 1);
      _goldForSuccessfulLevelFinish.RemoveAt(_currentLevel - 1);
      _goldForKillMonster.RemoveAt(_currentLevel - 1);
      BNewGameConfig.Tag = 1;
      if (_levelsConfig.Count == 0)//There are no levels, no settings
      {
        BLoadMonsterPict.Enabled = false;
        GBNumberOfDirections.Enabled = false;
        BRemoveLevel.Enabled = false;
        CBLevelInvisible.Enabled = false;
        _currentLevel = 0;
      }
      else
      {
        if (_levelsConfig.Count == 1)//If number of levels==1, usen doesn't need to switch beetwen them
        {
          BNextLevel.Enabled = false;
          BPrevLevel.Enabled = false;
          _currentLevel = 1;
        }
        else
        {
          _currentLevel--;
          if (_currentLevel == 0)
            _currentLevel = 1;
        }
        ShowLevelSettings(_currentLevel);
      }
    }
    #endregion

    #region Switching levels
    /// <summary>
    /// User want to get next level
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BNextLevel_Click(object sender, EventArgs e)
    {
      _currentLevel++;
      if (_currentLevel > _levelsConfig.Count)
        _currentLevel = 1;
      ShowLevelSettings(_currentLevel);
    }

    /// <summary>
    /// User want to get previous level.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BPrevLevel_Click(object sender, EventArgs e)
    {
      _currentLevel--;
      if (_currentLevel <= 0)
        _currentLevel = _levelsConfig.Count;
      ShowLevelSettings(_currentLevel);
    }
    #endregion

    #region Setting settings by user

    /// <summary>
    /// Masked text box changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void maskedTextBoxChanged(object sender, EventArgs e)
    {
      if ((sender as MaskedTextBox) == null)
      {
        MessageBox.Show("BAD BAD BAD programmer! The method is used incorrectly");
        return;
      }
      if (_currentLevel <= 0 || ((sender as MaskedTextBox).Text == string.Empty) || (!_realChange))
        return;
      Check checkInput = (string inValue, int valueBeforeChange, out int checkResult) =>
      {
        checkResult = Convert.ToInt32(inValue.Replace(" ", string.Empty)) == 0 ?
          valueBeforeChange : Convert.ToInt32(inValue.Replace(" ", string.Empty));
      };
      MonsterParam tmp = _levelsConfig[_currentLevel - 1];
      bool needRedraw = false;
      switch ((sender as MaskedTextBox).Name)
      {
        case "mTBHealthPoints":
          checkInput(mTBHealthPoints.Text, tmp.Base.HealthPoints, out tmp.Base.HealthPoints);
          break;
        case "mTBGoldForKill":
          {
            int tmpInt = _goldForKillMonster[_currentLevel - 1];
            checkInput(mTBGoldForKill.Text, tmpInt, out tmpInt);
            _goldForKillMonster[_currentLevel - 1] = tmpInt;
          }
          break;
        case "mTBNumberOfPhases":
          checkInput(mTBNumberOfPhases.Text, tmp.NumberOfPhases, out tmp.NumberOfPhases);
          needRedraw = true;
          break;
        case "mTBArmor":
          checkInput(mTBArmor.Text, tmp.Base.Armor, out tmp.Base.Armor);
          break;
        case "mTBNumberOfMonstersAtLevel":
          {
            int tmpInt = _numberOfMonstersAtLevel[_currentLevel - 1];
            checkInput(mTBNumberOfMonstersAtLevel.Text, tmpInt, out tmpInt);
            _numberOfMonstersAtLevel[_currentLevel - 1] = tmpInt;
          }
          break;
        case "mTBGoldForSuccessfulLevelFinish":
          {
            int tmpInt = _goldForSuccessfulLevelFinish[_currentLevel - 1];
            checkInput(mTBGoldForSuccessfulLevelFinish.Text, tmpInt, out tmpInt);
            _goldForSuccessfulLevelFinish[_currentLevel - 1] = tmpInt;
          }
          break;
      }
      _levelsConfig[_currentLevel - 1] = tmp;
      if (needRedraw)
        DrawMonsterPhases(MonsterDirection.Left);
      BNewGameConfig.Tag = 1;
    }

    /// <summary>
    /// Canva speed setting validation
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void nUDCanvaSpeed_Validated(object sender, EventArgs e)
    {
      if ((_currentLevel <= 0) || (!_realChange))
        return;
      MonsterParam tmp = _levelsConfig[_currentLevel - 1];
      tmp.Base.CanvasSpeed = Convert.ToInt32(nUDCanvaSpeed.Value);
      _levelsConfig[_currentLevel - 1] = tmp;
      BNewGameConfig.Tag = 1;
    }

    /// <summary>
    /// Change setting of invisibility
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void CBLevelInvisible_CheckedChanged(object sender, EventArgs e)
    {
      if ((_currentLevel <= 0) || (!_realChange))
        return;
      MonsterParam tmp = _levelsConfig[_currentLevel - 1];
      tmp.Base.Invisible = CBLevelInvisible.Checked;
      _levelsConfig[_currentLevel - 1] = tmp;
      BNewGameConfig.Tag = 1;
    }
    #endregion

    #region Monster loading and rendering
    /// <summary>
    /// Draws the monster phases.
    /// </summary>
    /// <param name="direction">The direction.</param>
    private void DrawMonsterPhases(MonsterDirection direction)
    {
      MonsterParam tmp = _levelsConfig[_currentLevel - 1];
      try
      {
        if (tmp[MonsterDirection.Left, 0] == null)
          return;
        Bitmap tmpForDrawing = new Bitmap(PBMosterPict.Width, (tmp[direction, 0].Height * tmp.NumberOfPhases) + ((20 * tmp.NumberOfPhases) - 1));
        PBMosterPict.Height = tmpForDrawing.Height;
        Graphics canva = Graphics.FromImage(tmpForDrawing);
        canva.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(0, 0, PBMosterPict.Width, PBMosterPict.Height));
        for (int phaseNum = 0; phaseNum < tmp.NumberOfPhases; phaseNum++)
        {
          canva.DrawImage(tmp[direction, phaseNum], (PBMosterPict.Width / 2) - (tmp[direction, phaseNum].Width / 2), (phaseNum * tmp[direction, phaseNum].Height + 20 * phaseNum),
            tmp[direction, phaseNum].Width, tmp[direction, phaseNum].Height);
        }
        PBMosterPict.Image = tmpForDrawing;
        PMonsterPict.Refresh();
      }
      catch (Exception e)
      {
        MessageBox.Show(e.Message);
        return;
      }
    }

    /// <summary>
    /// Monster picture loading
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void BLoadMonsterPict_Click(object sender, EventArgs e)
    {
      ODForFileSelect.Filter = "Monster picture|*.bmp";
      ODForFileSelect.FileName = "*.bmp";
      if (ODForFileSelect.ShowDialog() == DialogResult.OK)
      {
        try
        {
          var tmp = _levelsConfig[_currentLevel - 1];
          tmp.SetMonsterPict = ODForFileSelect.FileName;
          _levelsConfig[_currentLevel - 1] = tmp;
          DrawMonsterPhases(MonsterDirection.Left);
        }
        catch
        {
          MessageBox.Show("Bitmap loading error");
          return;
        }
        MessageBox.Show("Bitmap loaded Successful");
        if (_realChange)
          BNewGameConfig.Tag = 1;
      }
    }

    /// <summary>
    /// User changed monster direction(previewing, how picture will be parsed)
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void LBDirectionSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
      DrawMonsterPhases((MonsterDirection)LBDirectionSelect.SelectedIndex);
    }
    #endregion

    /// <summary>
    /// Tower folder name validation
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
    private void TBTowerFolder_KeyPress(object sender, KeyPressEventArgs e)
    {
      const string badSymbols = "\\|/:*?\"<>|";
      if (badSymbols.IndexOf(e.KeyChar.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) != -1)
        e.Handled = true;
      else
        if (_realChange)
          BNewGameConfig.Tag = 1;
    }

    /// <summary>
    /// Change the number of directions in the image of monsters
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void RBLeftDirection_CheckedChanged(object sender, EventArgs e)
    {
      if (Convert.ToInt32(GBNumberOfDirections.Tag) == 0)//One handler for all RadioButtons, 
      //Therefore, the Tag property is used to prevent double processing
      {
        GBNumberOfDirections.Tag = 1;
      }
      else
      {
        GBNumberOfDirections.Tag = 0;
        return;
      }
      if (!_realChange) return;
      MonsterParam tmp = _levelsConfig[_currentLevel - 1];
      if (RBLeftDirection.Checked)
      {
        tmp.NumberOfDirectionsInFile = 1;
      }
      else if (RBLetfAndUpDirections.Checked)
      {
        tmp.NumberOfDirectionsInFile = 2;
      }
      else if (RBAllFourDirections.Checked)
      {
        tmp.NumberOfDirectionsInFile = 4;
      }
      _levelsConfig[_currentLevel - 1] = tmp;
      BNewGameConfig.Tag = 1;
      DrawMonsterPhases(MonsterDirection.Left);
    }

    /// <summary>
    /// Form closing, checking, may be user want to save game configuration, before game edito will be closed
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (Convert.ToInt32(BNewGameConfig.Tag) != 1) return;
      if (
        MessageBox.Show(Resources.Game_configuration_not_saved, Resources.Applictaion_Title_InMessages,
                        MessageBoxButtons.YesNo) != DialogResult.Yes) return;
      BSave_Click(sender, new EventArgs());
      e.Cancel = true;
    }
  }
}
