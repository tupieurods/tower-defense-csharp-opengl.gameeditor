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

    //Для проверки на соответствие при изменении значений в maskedTextBox'ах
    delegate void Check(string inValue, int valueBeforeChange, out int checkResult);

    #endregion

    private List<MonsterParam> _levelsConfig;
    private List<int> _numberOfMonstersAtLevel;//Число монстров на каждлм из уровней
    private List<int> _goldForSuccessfulLevelFinish;//Золото за успешное завершение уровня
    private List<int> _goldForKillMonster;
    private int _currentLevel;//показывает текущий уровень
    private bool _realChange;//показывает как был изменён текст в maskedTextBox или других элементах редактирования
    //человеком или программно

    public MainForm()
    {
      _realChange = false;
      _currentLevel = 0;
      InitializeComponent();
      _levelsConfig = new List<MonsterParam>();
      _numberOfMonstersAtLevel = new List<int>();
    }

    #region Установление новых настроек по умолчанию для нового уровня/игровой конфигурации
    private void DefualtForNewLevel()//Заполнение параметров при добавлении уровня
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

    private void SetDefault()//Default для новой конфигурации игры
    {
      DefualtForNewLevel();//Для нового уровня
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
      SetDefault();
      BSave.Enabled = true;
      _realChange = true;
    }
    #endregion

    private void ShowLevelSettings(int levelNum)//Показ состояния настроек уровня
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
      //Вывод картинки
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

    private void BNewGameConfig_Click(object sender, EventArgs e)//Создание новой игры
    {
      if (Convert.ToInt32(BNewGameConfig.Tag) == 0)
      {
        CreateNewConfiguration();
        return;
      }
      if (Convert.ToInt32(BNewGameConfig.Tag) == 1)
      {
        if (MessageBox.Show(Resources.Game_configuration_not_saved, Resources.Applictaion_Title_InMessages, MessageBoxButtons.YesNo) == DialogResult.Yes)
        //Ещё не сохраняли после изменений
        {//Если хотим сохранять сохраняем
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
    private void BSave_Click(object sender, EventArgs e)//Сохранение конфигурации игры
    {
      if (SDForSaveConfiguration.ShowDialog() != DialogResult.OK) return;
      try
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
      try
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

    private void BLoad_Click(object sender, EventArgs e)
    {
      ODForFileSelect.Filter = "Файл с конфигурацией игры|*.tdgc";
      ODForFileSelect.FileName = "*.tdgc";
      if (ODForFileSelect.ShowDialog() == DialogResult.OK)
      {
        if (ODForFileSelect.FileName.LastIndexOf(".tdgc", StringComparison.Ordinal) == -1)
        {
          MessageBox.Show("Wrong file selected");
          return;
        }
        int levelsCount;
        try
        {
          BinaryReader loadMainConf = new BinaryReader(new FileStream(ODForFileSelect.FileName, FileMode.Open, FileAccess.Read));
          object[] tmp;
          SaveNLoad.LoadMainGameConf(loadMainConf, out _numberOfMonstersAtLevel, out _goldForSuccessfulLevelFinish, out _goldForKillMonster, out tmp);
          string mapName = (string)tmp[0];//0-имя карты
          ShowMapByFileName(mapName);
          TBTowerFolder.Text = (string)tmp[1];//1-имя папки с описанием башен
          levelsCount = (int)tmp[2];//2-число уровней
          mTBGoldAtStart.Text = Convert.ToInt32(tmp[4]).ToString(CultureInfo.InvariantCulture);//4-золото при старте
          mTBNumberOfLives.Text = Convert.ToInt32(tmp[5]).ToString(CultureInfo.InvariantCulture);//5-Число жизней в начале
          loadMainConf.Close();
        }
        catch (Exception exc)
        {
          MessageBox.Show(Resources.Load_error + exc.Message);
          return;
        }
        try
        {
          string levelFileName = ODForFileSelect.FileName.Substring(0, ODForFileSelect.FileName.LastIndexOf('.')) + ".tdlc";
          FileStream levelLoadStream = new FileStream(levelFileName, FileMode.Open, FileAccess.Read);
          IFormatter formatter = new BinaryFormatter();
          _levelsConfig.Clear();
          for (int i = 0; i < levelsCount; i++)
            _levelsConfig.Add((MonsterParam)(formatter.Deserialize(levelLoadStream)));
          levelLoadStream.Close();
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

    #region Загрузка и показ изображения карты
    private void BSelectMap_Click(object sender, EventArgs e)//Выбор карты
    {
      ODForFileSelect.Filter = "Файл с конфигурацией карты|*.efm";
      if (ODForFileSelect.ShowDialog() != DialogResult.OK) return;
      if (!ShowMapByFileName(ODForFileSelect.FileName)) return;
      /*PBMap.Tag = ODForFileSelect.FileName;
      BNewGameConfig.Tag = 1;*/
      /* Для чего задаётся полный путь к файлу.
          * Для удобства разработчика. При запуске игры, а не конфигуратора, 
          * будет извлечено имя карты и она будет загружена из каталога data,
           * При запуске же конфигуратора отпадает необходимость каждый раз закидывать карту в каталог с редактором
           */
    }

    private bool ShowMapByFileName(string fileName)
    {
      try
      {
        if (fileName == string.Empty)
          return false;
        PBMap.Image = null;
        PBMap.Size=new Size(0,0);
        string mapNameGetting = fileName.Substring(fileName.LastIndexOf('\\') + 1);
        LMapName.Text = string.Format("Map name: {0}", mapNameGetting.Substring(0, mapNameGetting.IndexOf('.')));
        TMap map = new TMap(fileName);
        Bitmap tmpImg = new Bitmap(Convert.ToInt32(map.Width * Settings.ElemSize * map.Scaling),
          Convert.ToInt32(map.Height * Settings.ElemSize * map.Scaling));
        Graphics canva = Graphics.FromImage(tmpImg);//создали канву
        map.ShowOnGraphics(canva);
        PBMap.Width = Convert.ToInt32(map.Width * Settings.ElemSize * map.Scaling);//Установили размеры
        PBMap.Height = Convert.ToInt32(map.Height * Settings.ElemSize * map.Scaling);
        PBMap.Image = tmpImg;
        PBMap.Tag = fileName;
        BNewGameConfig.Tag = 1;
      }
      catch
      {
        MessageBox.Show("Map loading error");
        return false;
      }
      MessageBox.Show("Map loaded Successful");
      return true;
    }
    #endregion

    #region Добавление и удаление уровней
    private void BAddLevel_Click(object sender, EventArgs e)//Добавление уровня
    {
      MonsterParam tmp = new MonsterParam(1, 100, 1, 1, "", 1);
      _levelsConfig.Insert(_currentLevel++, tmp);
      _numberOfMonstersAtLevel.Insert(_currentLevel - 1, 20);
      _goldForSuccessfulLevelFinish.Insert(_currentLevel - 1, 40);
      _goldForKillMonster.Insert(_currentLevel - 1, 10);
      LCurrentNCountLevel.Text = "Level: " + _currentLevel.ToString(CultureInfo.InvariantCulture) + "/" + _levelsConfig.Count.ToString(CultureInfo.InvariantCulture);
      if (_levelsConfig.Count == 2)//Если число уровней больше двух и нужно реализовать переключение между ними
      {
        BNextLevel.Enabled = true;
        BPrevLevel.Enabled = true;
      }
      if (_levelsConfig.Count == 1)
      {
        BLoadMonsterPict.Enabled = true;//разрешить добавление картинки
        GBNumberOfDirections.Enabled = true;
        CBLevelInvisible.Enabled = true;
      }
      DefualtForNewLevel();//установить шаблон
      BRemoveLevel.Enabled = true;
      BNewGameConfig.Tag = 1;
    }

    private void BRemoveLevel_Click(object sender, EventArgs e)//Удаление уровня
    {
      _levelsConfig.RemoveAt(_currentLevel - 1);
      _numberOfMonstersAtLevel.RemoveAt(_currentLevel - 1);
      _goldForSuccessfulLevelFinish.RemoveAt(_currentLevel - 1);
      _goldForKillMonster.RemoveAt(_currentLevel - 1);
      BNewGameConfig.Tag = 1;
      if (_levelsConfig.Count == 0)//Если число уровней равно нулю
      {
        BLoadMonsterPict.Enabled = false;
        GBNumberOfDirections.Enabled = false;
        BRemoveLevel.Enabled = false;
        CBLevelInvisible.Enabled = false;
        _currentLevel = 0;
      }
      else
      {
        if (_levelsConfig.Count == 1)//Если число уровней равное единице
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
      //LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
    }
    #endregion

    #region переключение уровней
    private void BNextLevel_Click(object sender, EventArgs e)
    {
      _currentLevel++;
      if (_currentLevel > _levelsConfig.Count)
        _currentLevel = 1;
      ShowLevelSettings(_currentLevel);
      //LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
    }

    private void BPrevLevel_Click(object sender, EventArgs e)
    {
      _currentLevel--;
      if (_currentLevel <= 0)
        _currentLevel = _levelsConfig.Count;
      ShowLevelSettings(_currentLevel);
      //LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
    }
    #endregion

    #region Изменение текста в mTB задающих параметры

    private void maskedTextBoxChanged(object sender, EventArgs e)
    {
      using (var maskedTextBox = sender as MaskedTextBox)
      {
        if (maskedTextBox != null && ((_currentLevel <= 0) || (maskedTextBox.Text == string.Empty) || (!_realChange)))
          return;
      }
      if ((sender as MaskedTextBox) == null)
      {
        MessageBox.Show("BAD BAD BAD programmer! Used metod incorrect. KILL HIM");
        return;
      }
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

    private void nUDCanvaSpeed_Validated(object sender, EventArgs e)
    {
      if ((_currentLevel <= 0) || (!_realChange))
        return;
      MonsterParam tmp = _levelsConfig[_currentLevel - 1];
      tmp.Base.CanvasSpeed = Convert.ToInt32(nUDCanvaSpeed.Value);
      _levelsConfig[_currentLevel - 1] = tmp;
      BNewGameConfig.Tag = 1;
    }

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

    #region Загрузка/Отрисовка изображения монстра
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
            tmp[direction, phaseNum].Width, tmp[direction, phaseNum].Height);//Приходится указывать размеры, т.к без них происходит прорисовка в дюймах
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

    private void BLoadMonsterPict_Click(object sender, EventArgs e)
    {
      ODForFileSelect.Filter = "Файл с изображением монстра|*.bmp";
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

    private void LBDirectionSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
      //MonsterParam tmp = _levelsConfig[_currentLevel - 1];
      DrawMonsterPhases((MonsterDirection)LBDirectionSelect.SelectedIndex);
    }
    #endregion

    private void TBTowerFolder_KeyPress(object sender, KeyPressEventArgs e)//Чтобы невозможно было символ в имени папки, который запрещён в windows
    {
      const string badSymbols = "\\|/:*?\"<>|";
      if (badSymbols.IndexOf(e.KeyChar.ToString()) != -1)
        e.Handled = true;
      else
        if (_realChange)
          BNewGameConfig.Tag = 1;
    }

    private void RBLeftDirection_CheckedChanged(object sender, EventArgs e)//Изменение числа направлений
    {
      if (Convert.ToInt32(GBNumberOfDirections.Tag) == 0)//Т.к это один обработчик для всех RadioButton
      //определяющих число направлений, чтобы не проводилось две обработки используется свойство Tag
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
