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
    public MainForm()
    {
      InitializeComponent();
    }

    private void SetDefault()
    {
      mTBGoldForKill.Text = "10";
      mTBHealthPoints.Text = "100";
      mTBNumberOfPhases.Text = "1";
      TBTowerFolder.Text = "Demo";
    }

    private void BNewGameConfig_Click(object sender, EventArgs e)
    {
      if (Convert.ToInt32(BNewGameConfig.Tag) == 0)
      {
        BNewGameConfig.Tag=1;
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

    private void BSelectMap_Click(object sender, EventArgs e)
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
  }
}
