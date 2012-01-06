using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameCoClassLiblary;

namespace GameEditor
{
  public partial class MainForm : Form
  {
    private List<MonsterParam> LevelsConfig;
    private int CurrentLevel = 0;

    public MainForm()
    {
      InitializeComponent();
      //CurrentLevel = 0;
      LevelsConfig = new List<MonsterParam>();
    }

    private void DefualtForNewLevel()//Заполнение параметров при добавлении уровня
    {
      mTBGoldForKill.Text = "1";
      mTBHealthPoints.Text = "100";
      mTBNumberOfPhases.Text = "2";
      nUDCanvaSpeed.Value = 1;
      PBMosterPict.Image = null;
    }

    private void ShowLevelSettings(int LevelNum)//Показ состояния настроек уровня
    {
      if (LevelsConfig.Count < LevelNum)
        return;
      mTBGoldForKill.Text = LevelsConfig[LevelNum - 1].GoldForKill.ToString();
      mTBHealthPoints.Text = LevelsConfig[LevelNum - 1].HealthPoints.ToString();
      mTBNumberOfPhases.Text = LevelsConfig[LevelNum - 1].NumberOfPhases.ToString();
      nUDCanvaSpeed.Value = LevelsConfig[LevelNum - 1].CanvasSpeed;
      mTBArmor.Text = LevelsConfig[LevelNum - 1].Armor.ToString();
      LCurrentNCountLevel.Text = "Level: " + LevelNum.ToString() + "/" + LevelsConfig.Count.ToString();
      //Вывод картинки
    }

    private void SetDefault()//Default для нового уровня
    {
      DefualtForNewLevel();
      TBTowerFolder.Text = "Demo";
      LevelsConfig = new List<MonsterParam>();
    }

    private void BNewGameConfig_Click(object sender, EventArgs e)//Создание новой игры
    {
      if (Convert.ToInt32(BNewGameConfig.Tag) == 0)
      {
        BNewGameConfig.Tag = 1;
        GBLevelConfig.Enabled = true;
        GBMapManage.Enabled = true;
        SetDefault();
        BSave.Enabled = true;
        return;
      }
      if (Convert.ToInt32(BNewGameConfig.Tag) == 1)
      {
        if (MessageBox.Show("Game configuration not saved! Save game configuration?", "Game configurator", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          return;
        }
      }
      if (Convert.ToInt32(BNewGameConfig.Tag) == 2)
      {
        if (MessageBox.Show("Do you really want create new game configuration?", "Game configurator", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          return;
        }
      }
    }

    #region Save/Load
    private void BSave_Click(object sender, EventArgs e)//Сохранение конфигурации игры
    {
      if (SDForSaveConfiguration.ShowDialog() == DialogResult.OK)
      {
        try
        {
          BinaryWriter SaveMainConf = new BinaryWriter(new FileStream(SDForSaveConfiguration.FileName, FileMode.Create, FileAccess.Write));
          SaveMainConf.Write(Convert.ToString(PBMap.Tag));
          SaveMainConf.Write(TBTowerFolder.Text);
          SaveMainConf.Write(LevelsConfig.Count);
          SaveMainConf.Write(0);//Запись числа опций, если в будущем опции появятся, то они должны будут быть записаны далее
          SaveMainConf.Close();
        }
        catch (Exception exc)
        {
          MessageBox.Show("File save error:\n" + exc.Message);
          return;
        }
        string FilePath = SDForSaveConfiguration.FileName.Substring(0, SDForSaveConfiguration.FileName.LastIndexOf('\\') + 1);
        string FileName = SDForSaveConfiguration.FileName.Substring(SDForSaveConfiguration.FileName.LastIndexOf('\\') + 1);
        FileName = FileName.Substring(0, FileName.LastIndexOf('.'));
        try
        {
          FileStream LevelConfSaveStream = new FileStream(FilePath + FileName + ".tdlc", FileMode.Create, FileAccess.Write);
          IFormatter Formatter = new BinaryFormatter();
          for (int i = 0; i < LevelsConfig.Count; i++)
            Formatter.Serialize(LevelConfSaveStream, LevelsConfig[i]);
          LevelConfSaveStream.Close();
        }
        catch (Exception exc)
        {
          MessageBox.Show("File save error:\n" + exc.Message);
          return;
        }
        BNewGameConfig.Tag = 2;
      }
    }

    private void BLoad_Click(object sender, EventArgs e)
    {
      ODForFileSelect.Filter = "Файл с конфигурацией игры|*.tdgc";
      if (ODForFileSelect.ShowDialog() == DialogResult.OK)
      {
        int LevelsCount;
        try
        {
          BinaryReader LoadMainConf = new BinaryReader(new FileStream(ODForFileSelect.FileName, FileMode.Open, FileAccess.Read));
          string MapName = LoadMainConf.ReadString();
          ShowMapByFileName(MapName);
          TBTowerFolder.Text = LoadMainConf.ReadString();
          LevelsCount = LoadMainConf.ReadInt32();
          //Считываем число опций и читаем сами опции, в текущей версии 0 опций
          LoadMainConf.Close();
        }
        catch (Exception exc)
        {
          MessageBox.Show("File load error:\n" + exc.Message);
          return;
        }
        try
        {
          string LevelFileName = ODForFileSelect.FileName.Substring(0, ODForFileSelect.FileName.LastIndexOf('.')) + ".tdlc";
          FileStream LevelLoadStream = new FileStream(LevelFileName, FileMode.Open, FileAccess.Read);
          IFormatter Formatter = new BinaryFormatter();
          LevelsConfig.Clear();
          for (int i = 0; i < LevelsCount; i++)
            LevelsConfig.Add((MonsterParam)(Formatter.Deserialize(LevelLoadStream)));
          CurrentLevel = 1;
          ShowLevelSettings(1);
          LevelLoadStream.Close();
        }
        catch (Exception exc)
        {
          MessageBox.Show("File load error:\n" + exc.Message);
          return;
        }
      }
      BNewGameConfig.Tag = 1;
      GBLevelConfig.Enabled = true;
      GBMapManage.Enabled = true;
      if (LevelsConfig.Count > 1)
      {
        BPrevLevel.Enabled = true;
        BNextLevel.Enabled = true;
      }
      BSave.Enabled = true;
    }
    #endregion

    private void BSelectMap_Click(object sender, EventArgs e)//Выбор карты
    {
      ODForFileSelect.Filter = "Файл с конфигурацией карты|*.efm";
      if (ODForFileSelect.ShowDialog() == DialogResult.OK)
      {
        if (ShowMapByFileName(ODForFileSelect.FileName))
        {
          PBMap.Tag = ODForFileSelect.FileName;
          /* Для чего задаётся полный путь к файлу.
          * Для удобства разработчика. При запуске игры, а не конфигуратора, 
          * будет извлечено имя карты и она будет загружена из каталога data,
           * При запуске же конфигуратора отпадает необходимость каждый раз закидывать карту в каталог с редактором
           */
        }
      }
    }

    private bool ShowMapByFileName(string FileName)
    {
      try
      {
        TMap Map = new TMap(FileName);
        Bitmap TmpImg = new Bitmap(Map.Width * 15, Map.Height * 15);
        Graphics Canva = Graphics.FromImage(TmpImg);//создали канву
        Map.ShowOnGraphics(Canva);
        PBMap.Width = Map.Width * 15;//Установили размеры
        PBMap.Height = Map.Height * 15;
        PBMap.Image = TmpImg;
      }
      catch
      {
        MessageBox.Show("Map loading error");
        return false;
      }
      MessageBox.Show("Map loaded Successful");
      return true;
    }

    #region Добавление и удаление уровней
    private void BAddLevel_Click(object sender, EventArgs e)//Добавление уровня
    {
      MonsterParam tmp = new MonsterParam(1, 100, 1, 10, 1, "");
      //LevelsConfig.Add(tmp);//Добавили шаблон нового уровня в конец
      LevelsConfig.Insert(CurrentLevel++, tmp);
      //CurrentLevel = LevelsConfig.Count;//установили созданный уровень текущим
      //LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + CurrentLevel.ToString();
      LCurrentNCountLevel.Text = "Level: " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
      if (LevelsConfig.Count == 2)//Если число уровней больше двух и нужно реализовать переключение между ними
      {
        BNextLevel.Enabled = true;
        BPrevLevel.Enabled = true;
      }
      if (LevelsConfig.Count == 1)
      {
        BLoadMonsterPict.Enabled = true;//разрешить добавление картинки
      }
      DefualtForNewLevel();//установить шаблон
      BRemoveLevel.Enabled = true;
    }

    private void BRemoveLevel_Click(object sender, EventArgs e)//Удаление уровня
    {
      LevelsConfig.RemoveAt(CurrentLevel - 1);
      if (LevelsConfig.Count == 0)//Если число уровней равно нулю
      {
        BLoadMonsterPict.Enabled = false;
        BRemoveLevel.Enabled = false;
        CurrentLevel = 0;
      }
      else
      {
        if (LevelsConfig.Count == 1)//Если число уровней равное единице
        {
          BNextLevel.Enabled = false;
          BPrevLevel.Enabled = false;
          CurrentLevel = 1;
        }
        else
        {
          CurrentLevel--;
          if (CurrentLevel == 0)
            CurrentLevel = 1;
        }
        ShowLevelSettings(CurrentLevel);
      }
      //LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
    }
    #endregion

    #region переключение уровней
    private void BNextLevel_Click(object sender, EventArgs e)
    {
      CurrentLevel++;
      if (CurrentLevel > LevelsConfig.Count)
        CurrentLevel = 1;
      ShowLevelSettings(CurrentLevel);
      //LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
    }

    private void BPrevLevel_Click(object sender, EventArgs e)
    {
      CurrentLevel--;
      if (CurrentLevel <= 0)
        CurrentLevel = LevelsConfig.Count;
      ShowLevelSettings(CurrentLevel);
      //LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
    }
    #endregion

    #region Изменение текста в mTB задающих параметры
    private void mTBHealthPoints_TextChanged(object sender, EventArgs e)
    {
      if ((CurrentLevel <= 0) || (mTBHealthPoints.Text == string.Empty))
        return;
      var Tmp = LevelsConfig[CurrentLevel - 1];
      Tmp.HealthPoints = Convert.ToInt32(mTBHealthPoints.Text.Replace(" ", string.Empty)) == 0 ?
        100 : Convert.ToInt32(mTBHealthPoints.Text.Replace(" ", string.Empty));
      LevelsConfig[CurrentLevel - 1] = Tmp;
    }

    private void mTBGoldForKill_TextChanged(object sender, EventArgs e)
    {
      if ((CurrentLevel <= 0) || (mTBGoldForKill.Text == string.Empty))
        return;
      var Tmp = LevelsConfig[CurrentLevel - 1];
      Tmp.GoldForKill = Convert.ToInt32(mTBGoldForKill.Text.Replace(" ", string.Empty)) == 0 ?
        10 : Convert.ToInt32(mTBGoldForKill.Text.Replace(" ", string.Empty));
      LevelsConfig[CurrentLevel - 1] = Tmp;
    }

    private void mTBNumberOfPhases_TextChanged(object sender, EventArgs e)
    {
      if ((CurrentLevel <= 0) || (mTBNumberOfPhases.Text == string.Empty))
        return;
      var Tmp = LevelsConfig[CurrentLevel - 1];
      Tmp.NumberOfPhases = Convert.ToInt32(mTBNumberOfPhases.Text.Replace(" ", string.Empty)) == 0 ?
        1 : Convert.ToInt32(mTBNumberOfPhases.Text.Replace(" ", string.Empty));
      LevelsConfig[CurrentLevel - 1] = Tmp;
    }

    private void mTBArmor_TextChanged(object sender, EventArgs e)
    {
      if ((CurrentLevel <= 0) || (mTBArmor.Text == string.Empty))
        return;
      var Tmp = LevelsConfig[CurrentLevel - 1];
      Tmp.Armor = Convert.ToInt32(mTBArmor.Text.Replace(" ", string.Empty)) == 0 ?
        1 : Convert.ToInt32(mTBArmor.Text.Replace(" ", string.Empty));
      LevelsConfig[CurrentLevel - 1] = Tmp;
    }

    private void nUDCanvaSpeed_Validated(object sender, EventArgs e)
    {
      if (CurrentLevel <= 0)
        return;
      var Tmp = LevelsConfig[CurrentLevel - 1];
      Tmp.CanvasSpeed = Convert.ToInt32(nUDCanvaSpeed.Value);
      LevelsConfig[CurrentLevel - 1] = Tmp;
    }
    #endregion

    private void DrawMonsterPhases(int Direction)
    {
      MonsterParam Tmp = LevelsConfig[CurrentLevel - 1];
      Bitmap TmpForDrawing;
      TmpForDrawing = new Bitmap(PBMosterPict.Width, (Tmp[Direction, 0].Height * Tmp.NumberOfPhases) + ((20 * Tmp.NumberOfPhases) - 1));
      PBMosterPict.Height = TmpForDrawing.Height;
      Graphics Canva = Graphics.FromImage(TmpForDrawing);
      Canva.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(0, 0, PBMosterPict.Width, PBMosterPict.Height));
      for (int PhaseNum = 0; PhaseNum < Tmp.NumberOfPhases; PhaseNum++)
      {
        Canva.DrawImage(Tmp[Direction, PhaseNum], (PBMosterPict.Width / 2) - (Tmp[Direction, PhaseNum].Width / 2), (PhaseNum * Tmp[Direction, PhaseNum].Height + 20 * PhaseNum),
          Tmp[Direction, PhaseNum].Width, Tmp[Direction, PhaseNum].Height);//Приходится указывать размеры, т.к без них происходит прорисовка в дюймах
      }
      PBMosterPict.Image = TmpForDrawing;
    }

    private void BLoadMonsterPict_Click(object sender, EventArgs e)
    {
      ODForFileSelect.Filter = "Файл с изображением монстра|*.bmp";
      if (ODForFileSelect.ShowDialog() == DialogResult.OK)
      {
        try
        {
          var Tmp = LevelsConfig[CurrentLevel - 1];
          Tmp.SetMonsterPict = ODForFileSelect.FileName;
          LevelsConfig[CurrentLevel - 1] = Tmp;
          DrawMonsterPhases(3);
        }
        catch
        {
          MessageBox.Show("Bitmap loading error");
          return;
        }
        MessageBox.Show("Bitmap loaded Successful");
      }
    }

    private void LBDirectionSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
      var Tmp = LevelsConfig[CurrentLevel - 1];
      if (Tmp[0, 0] != null)
      {
        DrawMonsterPhases(LBDirectionSelect.SelectedIndex);
      }
    }

    private void TBTowerFolder_KeyPress(object sender, KeyPressEventArgs e)//Чтобы невозможно было символ в имени папки, который запрещён в windows
    {
      string BadSymbols = "\\|/:*?\"<>|";
      if (BadSymbols.IndexOf(e.KeyChar.ToString()) != -1)
        e.Handled = true;
    }

  }
}
