using System;
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
    private int CurrentLevel = -1;

    public MainForm()
    {
      InitializeComponent();
      CurrentLevel = -1;
    }

    private void DefualtForNewLevel()
    {
      mTBGoldForKill.Text = "1";
      mTBHealthPoints.Text = "100";
      mTBNumberOfPhases.Text = "1";
      nUDCanvaSpeed.Value = 1;
      PBMosterPict.Image = null;
    }

    private void SetDefault()
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

    private void BSave_Click(object sender, EventArgs e)
    {
      BNewGameConfig.Tag = 2;
    }

    private void BSelectMap_Click(object sender, EventArgs e)//Выбор карты
    {
      if (ODForFileSelect.ShowDialog() == DialogResult.OK)
      {
        try
        {
          TMap Map = new TMap(ODForFileSelect.FileName);
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
          return;
        }
        MessageBox.Show("Map loaded Successful");
      }
    }

    private void BAddLevel_Click(object sender, EventArgs e)//Добавление уровня
    {
      MonsterParam tmp = new MonsterParam(1, 100, 1, 1, "");
      LevelsConfig.Add(tmp);//Добавили шаблон нового уровня
      CurrentLevel = LevelsConfig.Count;//установили созданный уровень текущим
      LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + CurrentLevel.ToString();
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

    private void ShowLevelSettings(int LevelNum)
    {
      mTBGoldForKill.Text = LevelsConfig[LevelNum - 1].GoldForKill.ToString();
      mTBHealthPoints.Text = LevelsConfig[LevelNum - 1].HealthPoints.ToString();
      mTBNumberOfPhases.Text = LevelsConfig[LevelNum - 1].NumberOfPhases.ToString();
      nUDCanvaSpeed.Value = LevelsConfig[LevelNum - 1].CanvasSpeed;
      //Вывод картинки
    }

    private void BRemoveLevel_Click(object sender, EventArgs e)
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
        }
        ShowLevelSettings(CurrentLevel);
      }
      LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
    }

    private void BNextLevel_Click(object sender, EventArgs e)
    {
      CurrentLevel++;
      if (CurrentLevel > LevelsConfig.Count)
        CurrentLevel = 1;
      ShowLevelSettings(CurrentLevel);
      LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
    }

    private void BPrevLevel_Click(object sender, EventArgs e)
    {
      CurrentLevel--;
      if (CurrentLevel <= 0)
        CurrentLevel = LevelsConfig.Count;
      ShowLevelSettings(CurrentLevel);
      LCurrentNCountLevel.Text = "Level " + CurrentLevel.ToString() + "/" + LevelsConfig.Count.ToString();
    }

    private void mTBHealthPoints_TextChanged(object sender, EventArgs e)
    {
      if (CurrentLevel <= 0)
        return;
      var Tmp = LevelsConfig[CurrentLevel - 1];

      Tmp.HealthPoints = Convert.ToInt32(mTBHealthPoints.Text.Replace(" ", string.Empty));
      LevelsConfig[CurrentLevel - 1] = Tmp;
    }

    private void mTBGoldForKill_TextChanged(object sender, EventArgs e)
    {
      if (CurrentLevel <= 0)
        return;
      var Tmp = LevelsConfig[CurrentLevel - 1];
      Tmp.GoldForKill = Convert.ToInt32(mTBGoldForKill.Text.Replace(" ", string.Empty));
      LevelsConfig[CurrentLevel - 1] = Tmp;
    }

    private void mTBNumberOfPhases_TextChanged(object sender, EventArgs e)
    {
      if (CurrentLevel <= 0)
        return;
      var Tmp = LevelsConfig[CurrentLevel - 1];
      Tmp.NumberOfPhases = Convert.ToInt32(mTBNumberOfPhases.Text.Replace(" ", string.Empty));
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

  }
}
