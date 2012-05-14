﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using LibPlateAnalysis;
using System.Windows.Forms;
using HCSAnalyzer.Forms;
using HCSAnalyzer.Forms.FormsForImages;

namespace HCSAnalyzer.Classes
{




    public class cGlobalInfo
    {
        public FormForDRCDesign WindowForDRCDesign = new FormForDRCDesign();
        public string WindowName;
        private bool _Is3DVisualization = false;
        private bool _IsDistributionMode = false;
        public HCSAnalyzer WindowHCSAnalyzer = null;

        List<cImageViewer> ListImageViewers = new List<cImageViewer>();

        public void DisplayViewer(cImageViewer ImageViewer)
        {
            ListImageViewers.Add(ImageViewer);
            ListImageViewers[ListImageViewers.Count-1].Display();

        }

        public bool IsDistributionMode()
        {
            return this._IsDistributionMode;
        }

        public void SwitchDistributionMode()
        {
            _IsDistributionMode = !_IsDistributionMode;

            if (_IsDistributionMode)
            {
                string DistributionMode = "";
                if (OptionsWindow.radioButtonDistributionMetricEuclidean.Checked)
                    DistributionMode += OptionsWindow.radioButtonDistributionMetricEuclidean.Text;
                if (OptionsWindow.radioButtonDistributionMetricManhattan.Checked)
                    DistributionMode += OptionsWindow.radioButtonDistributionMetricManhattan.Text;
                if (OptionsWindow.radioButtonDistributionMetricCosine.Checked)
                    DistributionMode += OptionsWindow.radioButtonDistributionMetricCosine.Text;
                if (OptionsWindow.radioButtonDistributionMetricBhattacharyya.Checked)
                    DistributionMode += OptionsWindow.radioButtonDistributionMetricBhattacharyya.Text;
                if (OptionsWindow.radioButtonDistributionMetricEMD.Checked)
                    DistributionMode += OptionsWindow.radioButtonDistributionMetricEMD.Text;

                WindowHCSAnalyzer.Text = WindowName + " (Histogram Mode - " + DistributionMode + ")";
            }
            else
                WindowHCSAnalyzer.Text = WindowName + " (Scalar Mode)";

            if (_IsDistributionMode == false) CurrentScreen.Reference = null;
            else
            {
                List<cWell> Ref = new List<cWell>();
                foreach (cWell WellForRef in CurrentScreen.GetCurrentDisplayPlate().ListActiveWells)
                {
                    if (WellForRef.GetClass() == 0)
                        Ref.Add(WellForRef);
                }

                // Ref.Add(CurrentScreen.GetCurrentDisplayPlate().ListActiveWells[0]);
                if (Ref.Count == 0) CurrentScreen.Reference = null;
                else
                    CurrentScreen.Reference = new cReference(Ref);
            }

            for (int idxP = 0; idxP < CurrentScreen.ListPlatesActive.Count; idxP++)
                CurrentScreen.ListPlatesActive[idxP].UpDataMinMax();

            //StartingUpDateUI();
            CurrentScreen.GetCurrentDisplayPlate().DisplayDistribution(CurrentScreen.ListDescriptors.CurrentSelectedDescriptor, false);
        }


        public bool Is3DVisu()
        {
            return this._Is3DVisualization;
        }


        public int[] WinSize = new int[] { 750, 400 };

        /// <summary>
        /// switch 3D mode
        /// </summary>
        public void SwitchVisuMode()
        {
            _Is3DVisualization = !_Is3DVisualization;

            if (_Is3DVisualization == false)
            {
                CurrentScreen.Close3DView();
            }
            else
            {
                CurrentScreen.GetCurrentDisplayPlate().DisplayDistribution(CurrentScreen.ListDescriptors.CurrentSelectedDescriptor, false);
            }

        }


        public Panel panelForPlate;
        public RichTextBox CurrentRichTextBox;
        public ComboBox ComboForSelectedDesc;
        public CheckedListBox CheckedListBoxForDescActive;
        public bool IsDisplayClassOnly = false;
        public PlatesListForm PlateListWindow;
        public cScreening CurrentScreen = null;
        public float SizeHistoWidth = 26;
        public float SizeHistoHeight = 18;
        public float ShiftX = 26;
        public float ShiftY = 18;

        public FormForOptionsWindow OptionsWindow;

        public byte[][] LUT;


        public void ChangeLUT(byte[][] NewLUT)
        {
            this.LUT = NewLUT;
        }

        #region LUT
        public byte[][] LUT_HSV = { new byte[]{255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,252,246,240,234,228,222,216,210,204,198,192,186,180,174,168,162,156,150,144,138,132,126,120,114,108,102,96,90,84,78,72,66,60,54,48,42,36,30,24,18,12,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,12,18,24,30,36,42,48,54,60,66,72,78,84,90,96,102,108,114,120,126,132,138,144,150,156,162,168,174,180,186,192,198,204,210,216,222,228,234,240,246,252,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255},
                            new byte[]{0,6,12,18,24,30,36,42,48,54,60,66,72,78,84,90,96,102,108,114,120,126,132,138,144,150,156,162,168,174,180,186,192,198,204,210,216,222,228,234,240,246,252,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,252,246,240,234,228,222,216,210,204,198,192,186,180,174,168,162,156,150,144,138,132,126,120,114,108,102,96,90,84,78,72,66,60,54,48,42,36,30,24,18,12,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,12,18,24,30,36,42,48,54,60,66,72,78,84,90,96,102,108,114,120,126,132,138,144,150,156,162,168,174,180,186,192,198,204,210,216,222,228,234,240,246,252,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,252,246,240,234,228,222,216,210,204,198,192,186,180,174,168,162,156,150,144,138,132,126,120,114,108,102,96,90,84,78,72,66,60,54,48,42,36,30,24,18,12,6,0}};

        public byte[][] LUT_FIRE = { new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,4,7,10,13,16,19,22,25,28,31,34,37,40,43,46,49,52,55,58,61,64,67,70,73,76,79,82,85,88,91,94,98,101,104,107,110,113,116,119,122,125,128,131,134,137,140,143,146,148,150,152,154,156,158,160,162,163,164,166,167,168,170,171,173,174,175,177,178,179,181,182,184,185,186,188,189,190,192,193,195,196,198,199,201,202,204,205,207,208,209,210,212,213,214,215,217,218,220,221,223,224,226,227,229,230,231,233,234,235,237,238,240,241,243,244,246,247,249,250,252,252,252,253,253,253,254,254,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255},
                                new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,5,7,8,10,12,14,16,19,21,24,27,29,32,35,37,40,43,46,48,51,54,57,59,62,65,68,70,73,76,79,81,84,87,90,92,95,98,101,103,105,107,109,111,113,115,117,119,121,123,125,127,129,131,133,134,136,138,140,141,143,145,147,148,150,152,154,155,157,159,161,162,164,166,168,169,171,173,175,176,178,180,182,184,186,188,190,191,193,195,197,199,201,203,205,206,208,210,212,213,215,217,219,220,222,224,226,228,230,232,234,235,237,239,241,242,244,246,248,248,249,250,251,252,253,254,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255},
                                new byte[]{31,34,38,42,46,49,53,57,61,65,69,74,78,82,87,91,96,100,104,108,113,117,121,125,130,134,138,143,147,151,156,160,165,168,171,175,178,181,185,188,192,195,199,202,206,209,213,216,220,220,221,222,223,224,225,226,227,224,222,220,218,216,214,212,210,206,202,199,195,191,188,184,181,177,173,169,166,162,158,154,151,147,143,140,136,132,129,125,122,118,114,111,107,103,100,96,93,89,85,82,78,74,71,67,64,60,56,53,49,45,42,38,35,31,27,23,20,16,12,8,5,4,3,3,2,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,8,13,17,21,26,30,35,42,50,58,66,74,82,90,98,105,113,121,129,136,144,152,160,167,175,183,191,199,207,215,223,227,231,235,239,243,247,251,255,255,255,255,255,255,255,255}};

        public byte[][] LUT_GREEN_TO_RED = {new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,33,35,37,39,41,43,45,47,49,51,53,55,57,59,61,63,65,67,69,71,73,75,77,79,81,83,85,87,89,91,93,95,97,99,101,103,105,107,109,111,113,115,117,119,121,123,125,127,129,131,133,135,137,139,141,143,145,147,149,151,153,155,157,159,161,163,165,167,169,171,173,175,177,179,181,183,185,187,189,191,193,195,197,199,201,203,205,207,209,211,213,215,217,219,221,223,225,227,229,231,233,235,237,239,241,243,245,247,249,251,253,255},
                                new byte[]{255,253,251,249,247,245,243,241,239,237,235,233,231,229,227,225,223,221,219,217,215,213,211,209,207,205,203,201,199,197,195,193,191,189,187,185,183,181,179,177,175,173,171,169,167,165,163,161,159,157,155,153,151,149,147,145,143,141,139,137,135,133,131,129,127,125,123,121,119,117,115,113,111,109,107,105,103,101,99,97,95,93,91,89,87,85,83,81,79,77,75,73,71,69,67,65,63,61,59,57,55,53,51,49,47,45,43,41,39,37,35,33,31,29,27,25,23,21,19,17,15,13,11,9,7,5,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};


        public byte[][] LUT_JET = {new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,16,32,48,64,80,96,112,128,143,159,175,191,207,223,239,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,239,223,207,191,175,159,143,128},
                                new byte[]{0,0,0,0,0,0,0,0,16,32,48,64,80,96,112,128,143,159,175,191,207,223,239,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,239,223,207,191,175,159,143,128,112,96,80,64,48,32,16,0,0,0,0,0,0,0,0,0},
                                new byte[]{143,159,175,191,207,223,239,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,239,223,207,191,175,159,143,128,112,96,80,64,48,32,16,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};

        public byte[][] LUT_HOT = {new byte[]{11,21,32,43,53,64,74,85,96,106,117,128,138,149,159,170,181,191,202,213,223,234,244,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255},
                                new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11,21,32,43,53,64,74,85,96,106,117,128,138,149,159,170,181,191,202,213,223,234,244,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255},
                                new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,16,32,48,64,80,96,112,128,143,159,175,191,207,223,239,255}};

        public byte[][] LUT_COOL = {new byte[]{0,4,8,12,16,20,24,28,32,36,40,45,49,53,57,61,65,69,73,77,81,85,89,93,97,101,105,109,113,117,121,125,130,134,138,142,146,150,154,158,162,166,170,174,178,182,186,190,194,198,202,206,210,215,219,223,227,231,235,239,243,247,251,255},
                                                                                                                                                                new byte[]{255,251,247,243,239,235,231,227,223,219,215,210,206,202,198,194,190,186,182,178,174,170,166,162,158,154,150,146,142,138,134,130,125,121,117,113,109,105,101,97,93,89,85,81,77,73,69,65,61,57,53,49,45,40,36,32,28,24,20,16,12,8,4,0},
                                                                                                                                                                new byte[]{255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255}};
        public byte[][] LUT_SPRING = {new byte[]{255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255},
                                                                                                                                                                new byte[]{0,4,8,12,16,20,24,28,32,36,40,45,49,53,57,61,65,69,73,77,81,85,89,93,97,101,105,109,113,117,121,125,130,134,138,142,146,150,154,158,162,166,170,174,178,182,186,190,194,198,202,206,210,215,219,223,227,231,235,239,243,247,251,255},
                                                                                                                                                                new byte[]{255,251,247,243,239,235,231,227,223,219,215,210,206,202,198,194,190,186,182,178,174,170,166,162,158,154,150,146,142,138,134,130,125,121,117,113,109,105,101,97,93,89,85,81,77,73,69,65,61,57,53,49,45,40,36,32,28,24,20,16,12,8,4,0}};
        public byte[][] LUT_SUMMER = {new byte[]{0,4,8,12,16,20,24,28,32,36,40,45,49,53,57,61,65,69,73,77,81,85,89,93,97,101,105,109,113,117,121,125,130,134,138,142,146,150,154,158,162,166,170,174,178,182,186,190,194,198,202,206,210,215,219,223,227,231,235,239,243,247,251,255},
                                                                                                                                                                new byte[]{128,130,132,134,136,138,140,142,144,146,148,150,152,154,156,158,160,162,164,166,168,170,172,174,176,178,180,182,184,186,188,190,192,194,196,198,200,202,204,206,208,210,212,215,217,219,221,223,225,227,229,231,233,235,237,239,241,243,245,247,249,251,253,255},
                                                                                                                                                                new byte[]{102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102,102}};
        public byte[][] LUT_AUTOMN = {new byte[]{255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255},
                                                                                                                                                                new byte[]{0,4,8,12,16,20,24,28,32,36,40,45,49,53,57,61,65,69,73,77,81,85,89,93,97,101,105,109,113,117,121,125,130,134,138,142,146,150,154,158,162,166,170,174,178,182,186,190,194,198,202,206,210,215,219,223,227,231,235,239,243,247,251,255},
                                                                                                                                                                new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
        public byte[][] LUT_WINTER = {new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                                                                                                                                                new byte[]{0,4,8,12,16,20,24,28,32,36,40,45,49,53,57,61,65,69,73,77,81,85,89,93,97,101,105,109,113,117,121,125,130,134,138,142,146,150,154,158,162,166,170,174,178,182,186,190,194,198,202,206,210,215,219,223,227,231,235,239,243,247,251,255},
                                                                                                                                                                new byte[]{255,253,251,249,247,245,243,241,239,237,235,233,231,229,227,225,223,221,219,217,215,213,210,208,206,204,202,200,198,196,194,192,190,188,186,184,182,180,178,176,174,172,170,168,166,164,162,160,158,156,154,152,150,148,146,144,142,140,138,136,134,132,130,128}};
        public byte[][] LUT_BONE = {new byte[]{0,4,7,11,14,18,21,25,28,32,35,39,43,46,50,53,57,60,64,67,71,74,78,81,85,89,92,96,99,103,106,110,113,117,120,124,128,131,135,138,142,145,149,152,156,159,163,166,172,178,183,189,194,200,205,211,216,222,227,233,238,244,249,255},
                                                                                                                                                                new byte[]{0,4,7,11,14,18,21,25,28,32,35,39,43,46,50,53,57,60,64,67,71,74,78,81,86,91,96,101,106,111,116,120,125,130,135,140,145,150,155,159,164,169,174,179,184,189,193,198,202,205,209,213,216,220,223,227,230,234,237,241,244,248,251,255},
                                                                                                                                                                new byte[]{1,6,11,16,21,26,31,35,40,45,50,55,60,65,70,74,79,84,89,94,99,104,108,113,117,120,124,128,131,135,138,142,145,149,152,156,159,163,166,170,174,177,181,184,188,191,195,198,202,205,209,213,216,220,223,227,230,234,237,241,244,248,251,255}};
        public byte[][] LUT_COPPER = {new byte[]{0,5,10,15,20,25,30,35,40,46,51,56,61,66,71,76,81,86,91,96,101,106,111,116,121,126,132,137,142,147,152,157,162,167,172,177,182,187,192,197,202,207,212,218,223,228,233,238,243,248,253,255,255,255,255,255,255,255,255,255,255,255,255,255},
                                                                                                                                                                new byte[]{0,3,6,9,13,16,19,22,25,28,32,35,38,41,44,47,51,54,57,60,63,66,70,73,76,79,82,85,89,92,95,98,101,104,108,111,114,117,120,123,126,130,133,136,139,142,145,149,152,155,158,161,164,168,171,174,177,180,183,187,190,193,196,199},
                                                                                                                                                                new byte[]{0,2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,32,34,36,38,40,42,44,46,48,50,52,54,56,58,60,62,64,66,68,70,72,75,77,79,81,83,85,87,89,91,93,95,97,99,101,103,105,107,109,111,113,115,117,119,121,123,125,127}};


        #endregion

        public Color[] ColorForDRCCurves = new Color[] { Color.Blue, Color.Red, Color.Black, Color.Orange, Color.Yellow, Color.LightGreen, Color.Pink, Color.Purple, Color.Cyan };

        private Color[] ColorForClass = new Color[] { Color.LightGreen, Color.Tomato, Color.Olive, Color.Orange, Color.Yellow, Color.Violet, Color.Pink, Color.Purple, Color.Salmon, Color.RoyalBlue };
        public string[] ListArtifacts = new string[] { "edge effect", "column artifact", "row artifact", "bowl effect" };

        /// <summary>
        /// Return the number of available class (correspond to the number of colors defined by the developper)
        /// </summary>
        /// <returns>number of class (here 10)</returns>
        public int GetNumberofDefinedClass()
        {
            return ColorForClass.Length;
        }


        public void ConsoleWriteLine(string DispText)
        {
            if ((CurrentRichTextBox == null) || (CurrentRichTextBox.IsDisposed)) CurrentRichTextBox = new RichTextBox();
            CurrentRichTextBox.AppendText(DispText + "\n");
            CurrentRichTextBox.Update();
        }


        public string ConvertIntPosToStringPos(int Pos)
        {
            string ReturnString = "";

            int First = Pos / 27;
            int Second = Pos - First * 26;


            if (First != 0) ReturnString = ((char)(First + 64)).ToString();
            if (Second != 0) ReturnString += ((char)(Second + 64)).ToString();

            return ReturnString;
        }


        public cGlobalInfo(cScreening CurrentScreen, HCSAnalyzer WindowHCSAnalyzer)
        {
            OptionsWindow = new FormForOptionsWindow(CurrentScreen);
            this.CurrentScreen = CurrentScreen;
            this.WindowHCSAnalyzer = WindowHCSAnalyzer;
            this.WindowName = WindowHCSAnalyzer.Text;
            this.LUT = LUT_JET;
        }

        public Color GetColor(int Class)
        {
            return ColorForClass[Class];
        }

        internal void ChangeSize(float Factor)
        {
            SizeHistoWidth *= Factor;
            SizeHistoHeight *= Factor;

            ShiftX = SizeHistoWidth;
            ShiftY = SizeHistoHeight;
        }

        public Kitware.VTK.RenderWindowControl renderWindowControlForVTK = null;
    }
}
