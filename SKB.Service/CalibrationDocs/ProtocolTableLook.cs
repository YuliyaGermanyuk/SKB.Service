using System;
using System.Collections.Generic;
using System.Linq;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Globalization;

namespace SKB.Service.CalibrationDocs
{
    class ProtocolTableLook : CertificateTableLook
    {
        /// <summary>
        /// Ширина таблицы.
        /// </summary>
        public int TableWidth;
        /// <summary>
        /// Ширина столбца
        /// </summary>
        public int[] ColumnsWidth;
        /// <summary>
        /// Генератор случайных чисел.
        /// </summary>
        Random RandomValue = new Random();
        /// <summary>
        /// Сформировать таблицу с данными измерений прибора.
        /// </summary>
        /// <param name="ParentDocument"> Родительский документ (из которого выгружаются данные). </param>
        /// <returns></returns>
        public Table GetDeviceTable(WordprocessingDocument ParentDocument)
        {
            int TableWidth = 9781;
            int ColumnsWidth = TableWidth / (ColumnsCount);
            int[] ColumnsWidthList = new int[ColumnsCount];
            for (int i = 0; i < ColumnsWidthList.Length; i++)
            {
                if (i == 0)
                    ColumnsWidthList[i] = ColumnsWidth;
                else
                    ColumnsWidthList[i] = ColumnsWidth;
            }

            Table table = CalibrationLib.NewTable(ColumnsWidthList, 108);

            foreach (TablesRow Row in GetRows(ParentDocument))
            {
                switch (Row.Type)
                {
                    case TablesRow.RowsType.Title:
                        table.Append(CreateTitle(Row, TableWidth));
                        break;
                    case TablesRow.RowsType.Header:
                        table.Append(CreateHeader(Row, ColumnsWidthList));
                        break;
                    case TablesRow.RowsType.Subtitle:
                        table.Append(CreateSubTitle(Row, TableWidth));
                        break;
                    case TablesRow.RowsType.SimpleRow:
                        table.Append(CreateSimpleRow(Row, ColumnsWidthList));
                        break;
                    case TablesRow.RowsType.Result:
                        table.Append(CreateResult(Row, TableWidth));
                        break;
                }
            }
            return table;
        }
        /// <summary>
        /// Сформировать таблицу с данными измерений прибора.
        /// </summary>
        /// <param name="JsonData"> Данные в формате json. </param>
        /// <returns></returns>
        public Table GetDeviceTable(RootObject JsonData)
        {
            int TableWidth = 9781;
            int ColumnsWidth = TableWidth / (ColumnsCount);
            int[] ColumnsWidthList = new int[ColumnsCount];
            for (int i = 0; i < ColumnsWidthList.Length; i++)
            {
                if (i == 0)
                    ColumnsWidthList[i] = ColumnsWidth;
                else
                    ColumnsWidthList[i] = ColumnsWidth;
            }

            Table table = CalibrationLib.NewTable(ColumnsWidthList, 108);

            foreach (TablesRow Row in GetRows(JsonData))
            {
                switch (Row.Type)
                {
                    case TablesRow.RowsType.Title:
                        table.Append(CreateTitle(Row, TableWidth));
                        break;
                    case TablesRow.RowsType.Header:
                        table.Append(CreateHeader(Row, ColumnsWidthList));
                        break;
                    case TablesRow.RowsType.Subtitle:
                        table.Append(CreateSubTitle(Row, TableWidth));
                        break;
                    case TablesRow.RowsType.SimpleRow:
                        table.Append(CreateSimpleRow(Row, ColumnsWidthList));
                        break;
                    case TablesRow.RowsType.Result:
                        table.Append(CreateResult(Row, TableWidth));
                        break;
                }
            }
            return table;
        }
        /// <summary>
        /// Дополнить таблицу с данными измерений прибора.
        /// </summary>
        /// <param name="ParentDocument"> Родительский документ (из которого выгружаются данные). </param>
        /// <returns></returns>
        public Table AdditionDeviceTable(WordprocessingDocument ParentDocument, Table CurrentTable)
        {
            int TableWidth = 9782;
            int ColumnsWidth = TableWidth / (ColumnsCount);
            int[] ColumnsWidthList = new int[ColumnsCount];
            for (int i = 0; i < ColumnsWidthList.Length; i++)
            {
                if (i == 0)
                    ColumnsWidthList[i] = ColumnsWidth;
                else
                    ColumnsWidthList[i] = ColumnsWidth;
            }

            Table table = CalibrationLib.NewTable(ColumnsWidthList);

            foreach (TablesRow Row in GetRows(ParentDocument))
            {
                if (Row.JoinToRow == 0)
                {
                    switch (Row.Type)
                    {
                        case TablesRow.RowsType.Title:
                            CurrentTable.Append(CreateTitle(Row, TableWidth));
                            break;
                        case TablesRow.RowsType.Header:
                            CurrentTable.Append(CreateHeader(Row, ColumnsWidthList));
                            break;
                        case TablesRow.RowsType.Subtitle:
                            CurrentTable.Append(CreateSubTitle(Row, TableWidth));
                            break;
                        case TablesRow.RowsType.SimpleRow:
                            CurrentTable.Append(CreateSimpleRow(Row, ColumnsWidthList));
                            break;
                        case TablesRow.RowsType.Result:
                            CurrentTable.Append(CreateResult(Row, TableWidth));
                            break;
                    }
                }
                else
                {
                    if (CurrentTable.Elements<TableRow>().Count() >= Row.JoinToRow)
                    {
                        TableRow CangingTableRow = CurrentTable.Elements<TableRow>().ElementAt(Row.JoinToRow - 1);
                        ChangingRow(Row, CangingTableRow);
                    }
                }
            }
            return CurrentTable;
        }
        /// <summary>
        /// Создать заголовок таблицы.
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <param name="TableWidth"> Ширина столбца. </param>
        /// <returns></returns>
        internal TableRow CreateTitle(TablesRow NewRow, int TableWidth)
        {
            TableRow NewTableRow = CalibrationLib.NewTableRow((UInt32Value)284U);
            for (int i = 0; i < NewRow.ColumnsCount; i++)
            {
                TableCell NewTableCell = NewTableRow.AddTableCell(TableWidth / NewRow.ColumnsCount, NewRow.Values[i], -113, -113, JustificationValues.Left, 12,
                    ColumnsCount, false, false);
                NewTableCell.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Nil, BorderValues.Nil);
            }
            return NewTableRow;
        }
        /// <summary>
        /// Создать шапку таблицы.
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <param name="TableWidth"> Ширина столбца. </param>
        /// <returns></returns>
        internal TableRow CreateHeader(TablesRow NewRow, int[] ColumnsWidthList)
        {
            TableRow NewTableRow = CalibrationLib.NewTableRow((UInt32Value)284U);

            for (int i = 0; i < NewRow.ColumnsCount; i++)
            {
                TableCell NewTableCell = NewTableRow.AddTableCell(ColumnsWidthList[i], NewRow.Values[i], -113, -113, JustificationValues.Center, 12,
                    NewRow.GridSpan[i], false, false, NewRow.VerticalMerge[i]);
                NewTableCell.SetBorders(BorderValues.Single, BorderValues.Single, BorderValues.Single, BorderValues.Single);
            }
            return NewTableRow;
        }
        /// <summary>
        /// Создать обычную строку таблицы.
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <param name="TableWidth"> Ширина столбца. </param>
        /// <returns></returns>
        internal TableRow CreateSimpleRow(TablesRow NewRow, int[] ColumnsWidthList)
        {
            TableRow NewTableRow = CalibrationLib.NewTableRow((UInt32Value)284U);

            for (int i = 0; i < NewRow.ColumnsCount; i++)
            {
                bool Italic = i == 0 ? false : true;
                TableCell NewTableCell = NewTableRow.AddTableCell(ColumnsWidthList[i], NewRow.Values[i], -113, -113, JustificationValues.Center, 12,
                    NewRow.GridSpan[i], false, Italic, NewRow.VerticalMerge[i]);
                NewTableCell.SetBorders(BorderValues.Single, BorderValues.Single, BorderValues.Single, BorderValues.Single);
            }
            return NewTableRow;
        }
        /// <summary>
        /// Изменить строку таблицы.
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <param name="TableWidth"> Ширина столбца. </param>
        /// <returns></returns>
        internal void ChangingRow(TablesRow NewRow, TableRow ChRow)
        {
            IEnumerable<TableCell> CellCollection = ChRow.Elements<TableCell>();
            for (int i = 0; i < NewRow.ColumnsCount; i++)
            {
                if (NewRow.Values[i] != "")
                    CellCollection.ElementAt(i).Elements<Paragraph>().First().Elements<Run>().First().Append(CalibrationLib.NewText(NewRow.Values[i]));
            }
        }
        /// <summary>
        /// Сформировать таблицу с данными измерений датчика.
        /// </summary>
        /// <param name="ParentDocument"> Родительский документ (из которого выгружаются данные).</param>
        /// <param name="SensorsNumber"> Заводской номер прибора. </param>
        /// <returns></returns>
        public Table GetSensorsTable(WordprocessingDocument ParentDocument, string SensorsNumber)
        {
            Table table = CalibrationLib.NewTable(ColumnsWidth, 108);
            List<TablesRow> TableRows = GetRows(ParentDocument, SensorsNumber);

            int i = 0;
            foreach (TablesRow Row in TableRows)
            {
                i++;
                switch (Row.Type)
                {
                    case TablesRow.RowsType.Title:
                        table.Append(NewTitle(Row));
                        break;
                    case TablesRow.RowsType.Subtitle:
                        table.Append(NewSubtitle(Row));
                        break;
                    case TablesRow.RowsType.Header:
                        table.Append(NewHeader(Row));
                        break;
                    case TablesRow.RowsType.SimpleRow:
                        table.Append(NewSimpleRow(Row));
                        break;
                    case TablesRow.RowsType.Result:
                        table.Append(NewResult(Row));
                        break;
                }
                
            }
            return table;
        }
        /// <summary>
        /// Получить данные строк таблицы
        /// </summary>
        /// <param name="ParentDocument"> Родительский документ (из которого выгружаются данные).</param>
        /// <returns></returns>
        public List<TablesRow> GetRows(WordprocessingDocument ParentDocument)
        {
            Table CurrentTable = ParentDocument == null ? null : ParentDocument.GetTable(ParentTableName);

            List<TablesRow> Collection = new List<TablesRow>();

            string TermNumber = CurrentTable != null ? CurrentTable.GetCellValue(1, 1).ToString() : "";
            Variables = new string[] { TermNumber };

            foreach (TablesRow Row in Rows)
            {
                string[] Values = new string[Row.Values.Count()];
                for (int i = 0; i < Row.Values.Count(); i++)
                { Values[i] = this.ConvertData(Row.Values[i], CurrentTable); }
                Collection.Add(new TablesRow(Values, Row.GridSpan, Row.VerticalMerge, Row.Type, Row.JoinToRow));
            }
            return Collection;
        }
        /// <summary>
        /// Получить данные строки таблицы.
        /// </summary>
        /// <param name="ParentDocument"> Родительский документ (Из которого выгружаются данные). </param>
        /// <param name="SensorsNumber"> Заводской номер датчика. </param>
        /// <returns></returns>
        public List<TablesRow> GetRows(WordprocessingDocument ParentDocument, string SensorsNumber)
        {
            Table CurrentTable = ParentDocument == null ? null : ParentDocument.GetTable(ParentTableName);

            List<TablesRow> Collection = new List<TablesRow>();

            #region // Определяем максимальную погрешность из файла с даными измерений.
            double ErrorValue = Math.Round(Convert.ToDouble(CurrentTable.GetCellValue(3, 1)), 2);

            #endregion

            #region // Определяем вариацию из файла с даными измерений.
            double Variation = Math.Round(Convert.ToDouble(CurrentTable.GetCellValue(2, 1)), 2);
            // Преобразуем вариацию к требуемому виду: вариация должна принимать значения от 0 до 0.3 с шагом 0.05.
            for (double j = 0; j < 0.45; j = j + 0.05d)
            {
                double y = j + 0.05d;
                if ((Variation > j) && (Variation <= y))
                {
                    if (Math.Abs(Variation - j) < Math.Abs(Variation - y))
                    {
                        Variation = j;
                        break;
                    }
                    else
                    {
                        Variation = y >= 0.3 ? 0.3 : y;
                        break;
                    }
                }
            }
            #endregion

            #region // Получение информации об измерительном стержне.
            string RodInfo = "";
            string RodLength = CurrentTable.GetCellValue(5, 1).ToString() == "Выберите длину" ? "" : CurrentTable.GetCellValue(5, 1).ToString();      // Длина стержня (текст)
            string RodNumber = CurrentTable.GetCellValue(6, 1).ToString() == "Введите номер" ? "" : CurrentTable.GetCellValue(6, 1).ToString();       // Номер стержня
            if ((RodLength != "") && (RodNumber != ""))
            { RodInfo = "в комплекте с измерительным стержнем длиной " + RodLength + ", № " + RodNumber; }
            #endregion

            #region // Определяем измеренные значения
            switch (RodLength)
            {
                case "500 мм":
                    RandomMeasuredValue[0] = Math.Round(Math.Round(500d * (1d / 3d), 1) + (RandomMeasuredValue[0] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[1] = Math.Round(Math.Round(500d * (2d / 3d), 1) + (RandomMeasuredValue[1] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[2] = Math.Round(Math.Round(500d * (4d / 5d), 1) + (RandomMeasuredValue[2] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    break;
                case "500":
                    RandomMeasuredValue[0] = Math.Round(Math.Round(500d * (1d / 3d), 1) + (RandomMeasuredValue[0] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[1] = Math.Round(Math.Round(500d * (2d / 3d), 1) + (RandomMeasuredValue[1] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[2] = Math.Round(Math.Round(500d * (4d / 5d), 1) + (RandomMeasuredValue[2] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    break;
                case "700 мм":
                    RandomMeasuredValue[0] = Math.Round(Math.Round(700d * (1d / 3d), 1) + (RandomMeasuredValue[0] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[1] = Math.Round(Math.Round(700d * (2d / 3d), 1) + (RandomMeasuredValue[1] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[2] = Math.Round(Math.Round(700d * (4d / 5d), 1) + (RandomMeasuredValue[2] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    break;
                case "700":
                    RandomMeasuredValue[0] = Math.Round(Math.Round(700d * (1d / 3d), 1) + (RandomMeasuredValue[0] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[1] = Math.Round(Math.Round(700d * (2d / 3d), 1) + (RandomMeasuredValue[1] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[2] = Math.Round(Math.Round(700d * (4d / 5d), 1) + (RandomMeasuredValue[2] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    break;
                case "1000 мм":
                    RandomMeasuredValue[0] = Math.Round(Math.Round(1000d * (1d / 3d), 1) + (RandomMeasuredValue[0] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[1] = Math.Round(Math.Round(1000d * (2d / 3d), 1) + (RandomMeasuredValue[1] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[2] = Math.Round(Math.Round(1000d * (4d / 5d), 1) + (RandomMeasuredValue[2] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    break;
                case "1000":
                    RandomMeasuredValue[0] = Math.Round(Math.Round(1000d * (1d / 3d), 1) + (RandomMeasuredValue[0] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[1] = Math.Round(Math.Round(1000d * (2d / 3d), 1) + (RandomMeasuredValue[1] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    RandomMeasuredValue[2] = Math.Round(Math.Round(1000d * (4d / 5d), 1) + (RandomMeasuredValue[2] * (RandomValue.Next(0, 2) == 1 ? 1 : -1)), 0) + (RandomValue.Next(0, 2) == 1 ? 0 : 0.5d);
                    break;
            }
            #endregion

            #region // Определяем значения погрешности.
            if (RodInfo != "")
            {
                // Преобразуем суммарную погрешность к требуемому виду: погрешность должна принимать значения от -1 до 1 с шагом 0.05.
                for (double i = -1; i < 1; i = Math.Round(i + 0.05d, 2))
                {
                    double x = Math.Round(i + 0.05d, 2);
                    if ((ErrorValue > i) && (ErrorValue <= x))
                    {
                        if (Math.Abs(ErrorValue - i) < Math.Abs(ErrorValue - x))
                        {
                            ErrorValue = i == -1 ? x : i;
                            break;
                        }
                        else
                        {
                            ErrorValue = x == 1 ? i : x;
                            break;
                        }
                    }
                }
            }

            double MaxError = RodInfo != "" ? Math.Round(Math.Abs(ErrorValue) - 0.5 - Variation, 2) : Math.Round(Math.Abs(ErrorValue) - 0.09, 2);
            double[] NewRandomError = new double[RandomMeasuredValue.Length];
            int RandomIndex = RandomValue.Next(0, RandomMeasuredValue.Length - 1);

            for (int i = 0; i < NewRandomError.Count(); i++)
            {

                if (i == RandomIndex)
                { NewRandomError[i] = ErrorValue > 0 ? MaxError : MaxError * -1; }
                else
                {
                    double RandValue = MaxError;

                    if (RodInfo != "")
                    {
                        int steps = Convert.ToInt32(Math.Round(MaxError / 0.05d, 0));
                        RandValue = Math.Round(0.05 * RandomValue.Next(0, steps), 2);
                        NewRandomError[i] = RandomValue.Next(0, 2) == 1 ? RandValue : Math.Round(RandValue * -1, 2);
                    }
                    else
                    {
                        while (RandValue >= MaxError)
                        { RandValue = Math.Round(RandomValue.NextDouble(), 2); }
                        NewRandomError[i] = RandomValue.Next(0, 2) == 1 ? MaxError - RandValue : (MaxError - RandValue) * -1;
                    }
                }
            }
            RandomError = NewRandomError;
            #endregion

            // Определение рассчитанных переменных.
            Variables = new string[] { SensorsNumber, Variation.ToString(), ErrorValue.ToString(), RodInfo };
            // Получение данных таблицы на основе шаблона.
            foreach (TablesRow Row in Rows)
            {
                string[] Values = new string[Row.Values.Count()];
                for (int i = 0; i < Row.Values.Count(); i++)
                { Values[i] = ConvertData(Row.Values[i], CurrentTable); }
                Collection.Add(new TablesRow(Values, Row.GridSpan, Row.VerticalMerge, Row.Type, Row.JoinToRow));
            }
            return Collection;
        }
        /// <summary>
        /// Преобразовать данные
        /// </summary>
        /// <param name="Value"> Значение ячейки таблицы. </param>
        /// <param name="ParentTable"> Родительская таблица (из которой выгружаются данные). </param>
        /// <returns></returns>
        public new string ConvertData(string Value, Table ParentTable)
        {
            string[] Index = Value.Split(';');
            // Простая ссылка на ячейку родительской таблицы в формате "1;2" (где 1 - номер строки родительской таблицы; 2 - номер столбца родительской таблицы)
            if (Index.Count() == 2 && ParentTable != null)
            {
                return ParentTable.GetCellValue(Convert.ToInt32(Index[0]), Convert.ToInt32(Index[1]));
            }
            else
            {
                // Ссылка на ячейку родительской таблицы в формате "1;2;.;3" (где 1 - номер строки родительской таблицы; 2 - номер столбца родительской таблицы; 
                //. - разделитель целой и дробной части; 3 - количество знаков после разделителя, до которых требуется округлить значение)
                if (Index.Count() == 4 && ParentTable != null)
                {
                    String CellValue = ParentTable.GetCellValue(Convert.ToInt32(Index[0]), Convert.ToInt32(Index[1]));
                    String Delimiter = (CellValue.IndexOf(Index[2]) < 0) ? "," : Index[2];
                    Int32 Accuracy = Convert.ToInt32(Index[3]);
                    Int32 ValueLength = Accuracy > 0 ? Accuracy + 1 : Accuracy;
                    string MyCellValue = CellValue.Contains(" ") ? CellValue.Substring(0, CellValue.IndexOf(" ")).Replace(Delimiter, ".") : CellValue.Replace(Delimiter, ".");
                    return Math.Round(Convert.ToDecimal(MyCellValue), Accuracy).ToString();
                    //return CellValue.Substring(0, CellValue.IndexOf(Delimiter) + ValueLength).Replace(Delimiter, ".");
                }

                // Ссылка на ячейку родительской таблицы в формате "1;2;+;3;4;.;5" (где 1 - номер строки первой ячейки родительской таблицы; 2 - номер столбца первой ячейки родительской таблицы; 
                // + - операция над значениями двух ячеек; 3 - номер строки второй ячейки родительской таблицы; 4 - номер столбца второй ячейки родительской таблицы;
                //. - разделитель целой и дробной части; 5 - количество знаков после разделителя, до которых требуется округлить значение)
                if (Index.Count() == 7 && ParentTable != null)
                {
                    String StringOperand1 = ParentTable.GetCellValue(Convert.ToInt32(Index[0]), Convert.ToInt32(Index[1]));
                    String StringOperand2 = ParentTable.GetCellValue(Convert.ToInt32(Index[3]), Convert.ToInt32(Index[4]));
                    String Operator = Index[2];
                    String Delimiter = (StringOperand1.IndexOf(Index[5]) < 0) && (StringOperand1.IndexOf(Index[5]) < 0) ? "," : Index[5];
                    Int32 Accuracy = Convert.ToInt32(Index[6]);
                    Int32 ValueLength = Accuracy > 0 ? Accuracy + 1 : Accuracy;

                    String NumDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    Unit UnitOperand1 = UnitsOfMeasure.GetResistanceValue(StringOperand1, Delimiter); //Convert.ToDouble(StringOperand1.Substring(0, StringOperand1.IndexOf(Delimiter) + ValueLength).Replace(Delimiter, NumDecimalSeparator));
                    Unit UnitOperand2 = UnitsOfMeasure.GetResistanceValue(StringOperand2, Delimiter); // Convert.ToDouble(StringOperand2.Substring(0, StringOperand2.IndexOf(Delimiter) + ValueLength).Replace(Delimiter, NumDecimalSeparator));
                    Unit Result;

                    // Оператор вычитания
                    if (Operator == "-")
                    {
                        Result = UnitsOfMeasure.GetSubtraction(UnitOperand1, UnitOperand2, UnitOperand1.UnitsName);
                        return Math.Round(Result.Value, Accuracy).ToString().Replace(NumDecimalSeparator, ".") + " " + Result.UnitsName;
                    }

                    // Оператор вычисления отношения ((арг1 - арг2)/(арг2))*100
                    if (Operator == "%")
                    {
                        Result = UnitsOfMeasure.GetRatio(UnitOperand1, UnitOperand2, UnitOperand1.UnitsName);
                        if (Result != null)
                            return Math.Round(Result.Value, Accuracy).ToString().Replace(NumDecimalSeparator, ".");
                        else return "-";
                    }
                }
                else
                {
                    if (Value.IndexOf("Random") >= 0)
                    {
                        int randomIndex = Convert.ToInt32(Value.Substring(Value.IndexOf("[") + 1, Value.IndexOf("]") - Value.IndexOf("[") - 1));
                        string value = Value.IndexOf("]") + 1 < Value.Length ? Value.Substring(Value.IndexOf("]") + 1) : "";

                        if (Value.IndexOf("RandomActualValue") >= 0)
                        { return Convert.ToString(RandomActualValue[randomIndex]) + value; }

                        if (Value.IndexOf("RandomActualInterval") >= 0)
                        { return Convert.ToString(RandomActualInterval[randomIndex]) + value; }

                        if (Value.IndexOf("RandomMeasuredValue") >= 0)
                        { return Convert.ToString(RandomMeasuredValue[randomIndex]) + value; }

                        if (Value.IndexOf("RandomMotionStartValue") >= 0)
                        { return Convert.ToString(RandomMotionStartValue[randomIndex]) + value; }

                        if (Value.IndexOf("RandomError") >= 0)
                        {
                            return Value.IndexOf("RandomError") > 0 ?
                                Value.Substring(0, Value.IndexOf("RandomError")) + Convert.ToString(RandomError[randomIndex]) + value :
                                Convert.ToString(RandomError[randomIndex]) + value;
                        }

                        if (Value.IndexOf("RandomIndication") >= 0)
                        { return Convert.ToString(RandomIndication[randomIndex]) + value; }
                    }
                    if (Value.IndexOf("Variables") >= 0)
                    {
                        int randomIndex = Convert.ToInt32(Value.Substring(Value.IndexOf("[") + 1, Value.IndexOf("]") - Value.IndexOf("[") - 1));
                        string value = Value.IndexOf("]") + 1 < Value.Length ? Value.Substring(Value.IndexOf("]") + 1) : "";
                        return Value.IndexOf("Variables") > 0 ? Value.Substring(0, Value.IndexOf("Variables")) + Convert.ToString(Variables[randomIndex]) + value :
                            Convert.ToString(Variables[randomIndex]) + value;
                    }
                }
            }
            return Value;
        }
        /// <summary>
        /// Создать обычную строку
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <returns></returns>
        private TableRow NewSimpleRow(TablesRow NewRow)
        {
            UInt32 RowHeight = 284U;
            int LeftIndentation = -113;
            int RightIndentation = -113;
            JustificationValues JustificationValue = JustificationValues.Center;
            int FontSize = 11;
            TableRow tableRow = CalibrationLib.NewTableRow(RowHeight);

            int[] CellWidthValues = GetCellWidthValues(NewRow);

            for (int i = 0; i < NewRow.Values.Count(); i++ )
            {
                TableCell tableCell = tableRow.AddTableCell(CellWidthValues[i], NewRow.Values[i], LeftIndentation, RightIndentation, JustificationValue, FontSize, NewRow.GridSpan[i]);
                tableCell.SetBorders(BorderValues.Single, BorderValues.Single, BorderValues.Single, BorderValues.Single);
            }
            return tableRow;
        }
        /// <summary>
        /// Создать новый подзаголовок
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <returns></returns>
        private TableRow NewSubtitle(TablesRow NewRow)
        {
            UInt32 RowHeight = 284U;
            int LeftIndentation = -113;
            int RightIndentation = -113;
            JustificationValues JustificationValue = JustificationValues.Center;
            int FontSize = 11;
            TableRow tableRow = CalibrationLib.NewTableRow(RowHeight);

            int[] CellWidthValues = GetCellWidthValues(NewRow);

            TableCell TableCell1 = tableRow.AddTableCell(CellWidthValues[0], NewRow.Values[0], LeftIndentation, RightIndentation, JustificationValue, FontSize, NewRow.GridSpan[0]);
            TableCell1.SetBorders(BorderValues.Single, BorderValues.Single, BorderValues.Single, BorderValues.Single);
            TableCell TableCell2 = tableRow.AddTableCell(CellWidthValues[1], NewRow.Values[1], LeftIndentation, RightIndentation, JustificationValue, FontSize, NewRow.GridSpan[1]);
            TableCell2.SetBorders(BorderValues.Single, BorderValues.Single, BorderValues.Single, BorderValues.Single);
            TableCell TableCell3 = tableRow.AddTableCell(CellWidthValues[2], NewRow.Values[2], LeftIndentation, RightIndentation, JustificationValue, FontSize, NewRow.GridSpan[2]);
            TableCell3.SetBorders(BorderValues.Single, BorderValues.Single, BorderValues.Single, BorderValues.Single);
            TableCell TableCell4 = tableRow.AddTableCell(CellWidthValues[3], NewRow.Values[3], LeftIndentation, RightIndentation, JustificationValue, FontSize, NewRow.GridSpan[3]);
            TableCell4.SetBorders(BorderValues.Single, BorderValues.Single, BorderValues.Single, BorderValues.Single);
            return tableRow;
        }
        /// <summary>
        /// Создать новую результирующую строку.
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <returns></returns>
        private TableRow NewResult(TablesRow NewRow)
        {
            UInt32 RowHeight = 284U;
            int LeftIndentation = -113;
            int RightIndentation = -113;
            int FontSize = 11;
            TableRow tableRow = CalibrationLib.NewTableRow(RowHeight);

            int[] CellWidthValues = GetCellWidthValues(NewRow);

            TableCell TableCell1 = tableRow.AddTableCell(CellWidthValues[0], NewRow.Values[0], LeftIndentation, RightIndentation, JustificationValues.Left, FontSize, NewRow.GridSpan[0]);
            TableCell1.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Nil, BorderValues.Nil);
            TableCell TableCell2 = tableRow.AddTableCell(CellWidthValues[1], NewRow.Values[1], LeftIndentation, RightIndentation, JustificationValues.Center, FontSize, NewRow.GridSpan[1]);
            TableCell2.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Single, BorderValues.Nil);
            TableCell TableCell3 = tableRow.AddTableCell(CellWidthValues[2], NewRow.Values[2], LeftIndentation, RightIndentation, JustificationValues.Left, FontSize, NewRow.GridSpan[2]);
            TableCell3.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Nil, BorderValues.Nil);
            return tableRow;
        }
        /// <summary>
        /// Создать новый заголовок таблицы.
        /// </summary>
        /// <param name="NewRow"></param>
        /// <returns></returns>
        private TableRow NewTitle(TablesRow NewRow)
        {
            UInt32 RowHeight = 284U;
            int LeftIndentation = -113;
            int RightIndentation = -113;
            int FontSize = 11;

            TableRow tableRow = CalibrationLib.NewTableRow(RowHeight);

            int[] CellWidthValues = GetCellWidthValues(NewRow);

            TableCell TableCell1 = tableRow.AddTableCell(CellWidthValues[0], NewRow.Values[0], LeftIndentation, RightIndentation, JustificationValues.Left, FontSize, NewRow.GridSpan[0], true);
            TableCell1.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Nil, BorderValues.Nil);
            //TableCell TableCell2 = tableRow.AddTableCell(CellWidthValues[1], NewRow.Values[1], LeftIndentation, RightIndentation, JustificationValues.Left, FontSize, NewRow.GridSpan[1], true);
            //TableCell2.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Nil, BorderValues.Nil);
            return tableRow;
        }
        /// <summary>
        /// Создать новую шапку таблицы.
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <returns></returns>
        private TableRow NewHeader(TablesRow NewRow)
        {
            UInt32 RowHeight = 284U;
            int LeftIndentation = -113;
            int RightIndentation = -113;
            JustificationValues JustificationValue = JustificationValues.Left;
            int FontSize = 11;
            TableRow tableRow = CalibrationLib.NewTableRow(RowHeight);

            int[] CellWidthValues = GetCellWidthValues(NewRow);

            TableCell TableCell1 = tableRow.AddTableCell(CellWidthValues[0], NewRow.Values[0], LeftIndentation, RightIndentation, JustificationValue, FontSize, NewRow.GridSpan[0]);
            TableCell1.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Nil, BorderValues.Nil);
            return tableRow;
        }
        /// <summary>
        /// Получить значения ширины столбцов.
        /// </summary>
        /// <param name="CurrentRow"> Строка таблицы. </param>
        /// <returns></returns>
        private int[] GetCellWidthValues(TablesRow CurrentRow)
        {
            int[] CellWidthValues = new int[CurrentRow.Values.Count()];
            int j = 0;
            for (int i = 0; i < CurrentRow.Values.Count(); i++)
            {
                CellWidthValues[i] = 0;
                for (int k = 0; k < CurrentRow.GridSpan[i]; k++)
                {
                    CellWidthValues[i] = CellWidthValues[i] + ColumnsWidth[j];
                    j++;
                }
            }
            return CellWidthValues;
        }
        /// <summary>
        /// Получить количество значащих дробных разрядов
        /// </summary>
        /// <param name="Value"> Значение. </param>
        /// <returns></returns>
        private int GetSignificantsDigits(double Value)
        {
            int SignificantsDigitsCount = 0;
            string TextValue = Value.ToString();
            if (TextValue.IndexOf(".") >= 0)
            {SignificantsDigitsCount = TextValue.Substring(TextValue.IndexOf(".")).Length - 1;}
            if (TextValue.IndexOf(",") >= 0)
            {SignificantsDigitsCount = TextValue.Substring(TextValue.IndexOf(",")).Length - 1;}
            return SignificantsDigitsCount;
        }
    }
}
