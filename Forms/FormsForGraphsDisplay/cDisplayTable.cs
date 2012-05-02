﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using HCSAnalyzer.Classes;

namespace HCSAnalyzer.Forms.FormsForGraphsDisplay
{
    class cDisplayTable : FormToDisplayTable
    {

        DataTable TableValues = null;


        public cDisplayTable(string Title, string[] ColumnNames, List<string[]> Values, cGlobalInfo GlobalInfo, bool IsMINE)
        {
            this.Text = Title;

            this.TableValues = new DataTable();
            this.dataGridViewForTable.Columns.Clear();

            foreach (string Name in ColumnNames)
            {
                this.TableValues.Columns.Add(new DataColumn(Name, typeof(string)));
            }

            //double[] Mins = new double[IsDisplayColorMap.Count];
            //double[] Maxs = new double[IsDisplayColorMap.Count];

            //for (int idx = 0; idx < Mins.Length; idx++)
            //{
            //    Mins[idx] = double.MaxValue;
            //    Maxs[idx] = double.MinValue;
            //}



            for (int Row = 0; Row < Values.Count; Row++)
            {
                this.TableValues.Rows.Add();

                for (int idxString = 0; idxString < Values[Row].Length; idxString++)
                {
                    string CurrentString = Values[Row][idxString];
                    this.TableValues.Rows[Row][idxString] = CurrentString;

                    //if (idxString==2)
                    //{
                    //    int ConvertedValue = (int)(GlobalInfo.LUT_GREEN_TO_RED[0].Length * double.Parse(CurrentString));

                    //    Color Coul = Color.FromArgb(GlobalInfo.LUT_GREEN_TO_RED[0][ConvertedValue], GlobalInfo.LUT_GREEN_TO_RED[1][ConvertedValue], GlobalInfo.LUT_GREEN_TO_RED[2][ConvertedValue]);


                    //    this.dataGridViewForTable.Rows[Row].Cells[idxString].Style.BackColor = Coul;


                    ////    if (CurrentValue > Maxs[idxString]) Maxs[idxString] = CurrentValue;
                    ////    if (CurrentValue < Mins[idxString]) Mins[idxString] = CurrentValue;
                    //}

                }
            }

            
            //for (int Row = 0; Row < Values.Count; Row++)
            //{
            //    for (int idxString = 0; idxString < Values[Row].Length; idxString++)
            //    {
            //        if (IsDisplayColorMap[idxString])
            //        {
            //            //double CurrentValue = double.Parse(this.TableValues.Rows[Row][idxString].

            //        }
            //    }
            //}





            this.dataGridViewForTable.DataSource = this.TableValues;
            this.Show();

            if (IsMINE)
            {
                for (int Row = 0; Row < Values.Count; Row++)
                {

                    for (int idxString = 0; idxString < Values[Row].Length; idxString++)
                    {
                        string CurrentString = Values[Row][idxString];

                        if ((idxString == 2) || (idxString == 7))
                        {
                            int ConvertedValue = (int)((GlobalInfo.LUT_GREEN_TO_RED[0].Length - 1) * Math.Abs(double.Parse(CurrentString)));

                            Color Coul = Color.FromArgb(GlobalInfo.LUT_GREEN_TO_RED[0][ConvertedValue], GlobalInfo.LUT_GREEN_TO_RED[1][ConvertedValue], GlobalInfo.LUT_GREEN_TO_RED[2][ConvertedValue]);

                            this.dataGridViewForTable.Rows[Row].Cells[idxString].Style.BackColor = Coul;
                            this.dataGridViewForTable.Rows[Row].Cells[idxString].Style.ForeColor = Color.White;

                            //    if (CurrentValue > Maxs[idxString]) Maxs[idxString] = CurrentValue;
                            //    if (CurrentValue < Mins[idxString]) Mins[idxString] = CurrentValue;
                        }
                    }
                }
            }

           
            
        }
    }
}
