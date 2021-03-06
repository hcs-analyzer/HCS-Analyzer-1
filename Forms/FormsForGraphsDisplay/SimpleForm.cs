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
using HCSAnalyzer.Forms.FormsForGraphsDisplay;

namespace LibPlateAnalysis
{
    public partial class SimpleForm : Form
    {
        FormForMaxMinRequest RequestWindow = new FormForMaxMinRequest();

        cScreening CompleteScreening = null;

        public SimpleForm(cScreening CompleteScreening)
        {
            InitializeComponent();
            this.CompleteScreening = CompleteScreening;
        }


        public SimpleForm()
        {
            InitializeComponent();

        }
        private void saveGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog CurrSaveFileDialog = new SaveFileDialog();
            CurrSaveFileDialog.Filter = "PNG files (*.png)|*.png";
            System.Windows.Forms.DialogResult Res = CurrSaveFileDialog.ShowDialog();
            if (Res != System.Windows.Forms.DialogResult.OK) return;

            string CurrentPath = CurrSaveFileDialog.FileName;
            this.chartForSimpleForm.SaveImage(CurrentPath, ChartImageFormat.Png);

            MessageBox.Show("File saved !");
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            this.chartForSimpleForm.SaveImage(ms, ChartImageFormat.Bmp);
            Bitmap bm = new Bitmap(ms);
            Clipboard.SetImage(bm);
        }

        public string GetValues()
        {
            StringBuilder sb = new StringBuilder();

            if (this.chartForSimpleForm.Titles.Count != 0)
                sb.Append(this.chartForSimpleForm.Titles[0].Text + "\n");
            sb.Append("Axis X: " + this.chartForSimpleForm.ChartAreas[0].AxisX.Title + "\n");
            sb.Append("Axis Y: " + this.chartForSimpleForm.ChartAreas[0].AxisY.Title + "\n");


            if (this.chartForSimpleForm.Series[0].Name.ToString() == "Matrix")
            {
                sb.Append("\t");

                for (int X = 0; X < this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                    sb.Append(this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels[X].Text + "\t");

                sb.Append("\n");
                for (int Y = 0; Y < this.chartForSimpleForm.ChartAreas[0].AxisY.CustomLabels.Count; Y++)
                {
                    sb.Append(this.chartForSimpleForm.ChartAreas[0].AxisY.CustomLabels[Y].Text + "\t");

                    for (int X = 0; X < this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                        sb.Append(this.chartForSimpleForm.Series[0].Points[X + Y * this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count].ToolTip + "\t");

                    sb.Append("\n");
                }
            }
            else
            {
                //sb.Append("\t");
                for (int Serie = 0; Serie < this.chartForSimpleForm.Series.Count; Serie++)
                {
                    sb.Append(this.chartForSimpleForm.Series[Serie].Label + " X values\t");
                    if (this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count > 0)
                    {
                        for (int X = 0; X < this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                            sb.Append(this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels[X].Text.Replace("\n", " ") + "\t");
                    }
                    else
                    {
                        for (int i = 0; i < this.chartForSimpleForm.Series[Serie].Points.Count; i++)
                            sb.Append(String.Format("{0}\t", this.chartForSimpleForm.Series[Serie].Points[i].XValue));
                    }
                    sb.Append("\n");
                    sb.Append(this.chartForSimpleForm.Series[Serie].Label + " Y Values\t");
                    for (int i = 0; i < this.chartForSimpleForm.Series[Serie].Points.Count; i++)
                        sb.Append(String.Format("{0}\t", this.chartForSimpleForm.Series[Serie].Points[i].YValues[0]));
                }
            }
            return sb.ToString();
        }

        private void dataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GetValues());
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.chartForSimpleForm.Printing.PageSetup();
            this.chartForSimpleForm.Printing.PrintPreview();
            this.chartForSimpleForm.Printing.Print(false);
        }



        static DataPoint PtToTransfer;
        void ChangeClass(object sender, EventArgs e)
        {
            cWell WellToTransfer = (cWell)(PtToTransfer.Tag);
            if (WellToTransfer == null) return;
            WellToTransfer.SetClass(int.Parse(sender.ToString().Remove(0, 6)));
            WellToTransfer.AssociatedPlate.UpdateNumberOfClass();
            PtToTransfer.Color = WellToTransfer.GetColor();
        }




        private void parametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.chartForSimpleForm.Series[0].Points.Count >= 1)
                RequestWindow.numericUpDownMarkerSize.Value = (decimal)this.chartForSimpleForm.Series[0].Points[0].MarkerSize;


            RequestWindow.numericUpDownMax.Value = (decimal)this.chartForSimpleForm.ChartAreas[0].AxisY.Maximum;
            RequestWindow.numericUpDownMin.Value = (decimal)this.chartForSimpleForm.ChartAreas[0].AxisY.Minimum;

            if (RequestWindow.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (RequestWindow.numericUpDownMax.Value <= RequestWindow.numericUpDownMin.Value) return;

            this.chartForSimpleForm.ChartAreas[0].AxisY.Maximum = (double)RequestWindow.numericUpDownMax.Value;
            this.chartForSimpleForm.ChartAreas[0].AxisY.Minimum = (double)RequestWindow.numericUpDownMin.Value;
            foreach (DataPoint Pt in this.chartForSimpleForm.Series[0].Points)
            {
                Pt.MarkerSize = (int)RequestWindow.numericUpDownMarkerSize.Value;

            }
        }

        private void chartForSimpleForm_MouseClick_1(object sender, MouseEventArgs e)
        {
            if ((e.Button != System.Windows.Forms.MouseButtons.Right) || (CompleteScreening == null)) return;
            HitTestResult Res = this.chartForSimpleForm.HitTest(e.X, e.Y, ChartElementType.DataPoint);
            if (Res.Series == null) return;

            ContextMenuStrip contextMenuStripActorPicker = new ContextMenuStrip();
            for (int i = 0; i < CompleteScreening.GlobalInfo.GetNumberofDefinedClass(); i++)
            {
                ToolStripItem ChangeClassItem = new ToolStripMenuItem("Class " + i);
                ChangeClassItem.Click += new System.EventHandler(this.ChangeClass);
                contextMenuStripActorPicker.Items.Add(ChangeClassItem);
            }

            if ((Res.Series.Points[Res.PointIndex].Tag == null) || (Res.Series.Points[Res.PointIndex].Tag.GetType().Name.ToString() != "cWell")) return;
            PtToTransfer = Res.Series.Points[Res.PointIndex];
            contextMenuStripActorPicker.Show(Control.MousePosition);
        }

        private void chartForSimpleForm_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            if (CompleteScreening == null) return;
            HitTestResult Res = this.chartForSimpleForm.HitTest(e.X, e.Y, ChartElementType.DataPoint);

            if ((Res.Series == null) || (Res.Series.Points[Res.PointIndex].Tag == null) || (Res.Series.Points[Res.PointIndex].Tag.GetType().Name.ToString() != "cWell")) return;

            cWell TmpWell = (cWell)(Res.Series.Points[Res.PointIndex].Tag);
            if (TmpWell == null) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // CompleteScreening.GlobalInfo.WindowHCSAnalyzer.tabControlMain.SelectedTab = CompleteScreening.GlobalInfo.WindowHCSAnalyzer.tabPageDistribution;
                int PosPlate = CompleteScreening.GlobalInfo.WindowHCSAnalyzer.toolStripcomboBoxPlateList.FindStringExact(TmpWell.AssociatedPlate.Name);
                CompleteScreening.GlobalInfo.WindowHCSAnalyzer.toolStripcomboBoxPlateList.SelectedIndex = PosPlate;
                CompleteScreening.CurrentDisplayPlateIdx = PosPlate;
                CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
                TmpWell.DisplayInfoWindow();
            }
        }



    }
}
