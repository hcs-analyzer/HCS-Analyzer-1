﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using LibPlateAnalysis;

namespace HCSAnalyzer.Forms
{
    public partial class FormForWellInformation : Form
    {
        cWell CurrentWell;

        public FormForWellInformation(cWell CurrentWell)
        {
            InitializeComponent();
            this.CurrentWell = CurrentWell;
        }

        public string GetValues()
        {

            StringBuilder sb = new StringBuilder();

            if (this.chartForFormWell.Titles.Count != 0)
                sb.Append(this.chartForFormWell.Titles[0].Text + "\n");
            sb.Append("Axis X: " + this.chartForFormWell.ChartAreas[0].AxisX.Title + "\n");
            sb.Append("Axis Y: " + this.chartForFormWell.ChartAreas[0].AxisY.Title + "\n");


            if (this.chartForFormWell.Series[0].Name.ToString() == "Matrix")
            {
                sb.Append("\t");

                for (int X = 0; X < this.chartForFormWell.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                    sb.Append(this.chartForFormWell.ChartAreas[0].AxisX.CustomLabels[X].Text + "\t");

                sb.Append("\n");
                for (int Y = 0; Y < this.chartForFormWell.ChartAreas[0].AxisY.CustomLabels.Count; Y++)
                {
                    sb.Append(this.chartForFormWell.ChartAreas[0].AxisY.CustomLabels[Y].Text + "\t");

                    for (int X = 0; X < this.chartForFormWell.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                        sb.Append(this.chartForFormWell.Series[0].Points[X + Y * this.chartForFormWell.ChartAreas[0].AxisX.CustomLabels.Count].ToolTip + "\t");

                    sb.Append("\n");
                }
            }
            else
            {
                for (int Serie = 0; Serie < this.chartForFormWell.Series.Count; Serie++)
                {
                    sb.Append(this.chartForFormWell.Series[Serie].Label + " X values\t");
                    if (this.chartForFormWell.ChartAreas[0].AxisX.CustomLabels.Count > 0)
                    {
                        for (int X = 0; X < this.chartForFormWell.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                            sb.Append(this.chartForFormWell.ChartAreas[0].AxisX.CustomLabels[X].Text.Replace("\n", " ") + "\t");

                    }
                    else
                    {

                        for (int i = 0; i < this.chartForFormWell.Series[Serie].Points.Count; i++)
                            sb.Append(String.Format("{0}\t", this.chartForFormWell.Series[Serie].Points[i].XValue));
                    }
                    sb.Append("\n");

                    sb.Append(this.chartForFormWell.Series[Serie].Label + " Y Values\t");
                    for (int i = 0; i < this.chartForFormWell.Series[Serie].Points.Count; i++)
                        sb.Append(String.Format("{0}\t", this.chartForFormWell.Series[Serie].Points[i].YValues[0]));
                }
            }
            return sb.ToString();
        }



        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            this.chartForFormWell.SaveImage(ms, ChartImageFormat.Bmp);
            Bitmap bm = new Bitmap(ms);
            Clipboard.SetImage(bm);
        }

        private void dataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GetValues());
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            this.chartForFormWell.SaveImage(ms, ChartImageFormat.Bmp);
            Bitmap bm = new Bitmap(ms);
            Clipboard.SetImage(bm);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.chartForFormWell.Printing.PageSetup();
            this.chartForFormWell.Printing.PrintPreview();
            this.chartForFormWell.Printing.Print(false);
        }

        private void numericUpDownImageMin_ValueChanged(object sender, EventArgs e)
        {
            List<double> lMin = new List<double>();
            lMin.Add((double)numericUpDownImageMin.Value);

            this.CurrentWell.DrawPic(this, lMin, null);
        }

        private void numericUpDownImageMax_ValueChanged(object sender, EventArgs e)
        {
            List<double> lMax = new List<double>();
            lMax.Add((double)numericUpDownImageMax.Value);

            this.CurrentWell.DrawPic(this, null, lMax);
        }

        private void numericUpDownIdxImage_ValueChanged_1(object sender, EventArgs e)
        {
            this.CurrentWell.DrawPic(this, null, null);
        }

        private void pictureBoxForImage_Paint(object sender, PaintEventArgs e)
        {
            this.CurrentWell.DrawPic(this, null, null);
        }

        private void panelForImage_Paint(object sender, PaintEventArgs e)
        {
            this.CurrentWell.DrawPic(this, null, null);
        }

        private void buttonUpdateAndClose_Click(object sender, EventArgs e)
        {
            CurrentWell.Info = this.textBoxInfo.Text;
            CurrentWell.Name = this.textBoxName.Text;
            double Concen = 0;
            if (double.TryParse(this.textBoxConcentration.Text, out Concen))
                CurrentWell.Concentration = Concen;

            CurrentWell.AssociatedPlate.ParentScreening.GetCurrentDisplayPlate().DisplayDistribution(CurrentWell.AssociatedPlate.ParentScreening.ListDescriptors.CurrentSelectedDescriptor, false);
        }







    }
}
