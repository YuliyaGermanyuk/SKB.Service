using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SKB.Service.CalibrationDocs
{
    class CertificateTableLook
    {
        /// <summary>
        /// Ширина таблицы
        /// </summary>
        ///int TableWidth = 9782;
        /// <summary>
        /// Ширины столбцов
        /// </summary>
        /*int[] ColumnsWidthList
        {
            get
            {
                try
                {
                    if (ColumnsWidthList.Count() != 0)
                        return ColumnsWidthList;
                    else
                    {
                        int[] NewList = new int[ColumnsCount];
                        int ColumnsWidth = TableWidth / (ColumnsCount + 1);
                        for (int i = 0; i < ColumnsCount; i++)
                        {
                            if (i == 0)
                                NewList[i] = ColumnsWidth * 2;
                            else
                                NewList[i] = ColumnsWidth;
                        }
                        return NewList;
                    }
                }
                catch
                {
                    int[] NewList = new int[ColumnsCount];
                    int ColumnsWidth = TableWidth / (ColumnsCount + 1);
                    for (int i = 0; i < ColumnsCount; i++)
                    {
                        if (i == 0)
                            NewList[i] = ColumnsWidth * 2;
                        else
                            NewList[i] = ColumnsWidth;
                    }
                    return NewList;
                }
            }
        }*/
        /// <summary>
        /// Типы приборов
        /// </summary>
        public string[] DeviceTypes;
        /// <summary>
        /// Название таблицы
        /// </summary>
        public string TableName;
        /// <summary>
        /// Количетво столбцов в таблице
        /// </summary>
        public int ColumnsCount;
        /// <summary>
        /// Название родительской таблицы (из которой выгружаются данные)
        /// </summary>
        public string ParentTableName;

        /// <summary>
        /// Категории родительских документов.
        /// </summary>
        public enum DocumentCategory
        {
            /// <summary>
            /// Нет.
            /// </summary>
            [Description("Нет")]
            None = 0,
            /// <summary>
            /// Протокол калибровки.
            /// </summary>
            [Description("Протокол калибровки")]
            CalibrationProtocol = 1,
            /// <summary>
            /// Данные измерений.
            /// </summary>
            [Description("Данные измерений")]
            MeasuringData = 2,
            /// <summary>
            /// Протокол приемосдаточных испытаний.
            /// </summary>
            [Description("Протокол приемосдаточных испытаний")]
            AcceptanceTestingProtocol = 3,
            /// <summary>
            /// Протокол поверки.
            /// </summary>
            [Description("Протокол поверки")]
            VerificationProtocol = 4,
        };
        /// <summary>
        /// Категория родительского документа
        /// </summary>
        public DocumentCategory ParentDocumentCategory;
        /// <summary>
        /// Случайные действительные значения
        /// </summary>
        private double[] randomActualValue;
        /// <summary>
        /// Случайные измеренные значения
        /// </summary>
        private double[] randomMeasuredValue;
        /// <summary>
        /// Случайные стартовые значения
        /// </summary>
        private double[] randomMotionStartValue;
        /// <summary>
        /// Случайные значения погрешности
        /// </summary>
        private double[] randomError;
        /// <summary>
        /// 
        /// </summary>
        private double[] randomIndication;
        /// <summary>
        /// 
        /// </summary>
        private double[] randomActualInterval;
        /// <summary>
        /// Значения переменных
        /// </summary>
        private string[] variables;
        /// <summary>
        /// Случайные действительные значения
        /// </summary>
        public double[] RandomActualValue
        {
            get 
            {
                if ((randomActualValue != null) && (randomActualValue.Length > 0))
                {return randomActualValue;}
                else
                {
                    int Length = Math.Min(RandomError.Length, RandomMeasuredValue.Length);
                    double[] ActualValue = new double[Length];
                    for(int i = 0; i < Length; i++)
                    {ActualValue[i] = Math.Round(RandomMeasuredValue[i] - RandomError[i], 3);}
                    return ActualValue;
                }
            }
            set{randomActualValue = value;}
        }
        /// <summary>
        /// Действительный интервал.
        /// </summary>
        public double[] RandomActualInterval
        {
            get
            {
                if ((randomActualInterval != null) && (randomActualInterval.Length > 0))
                { return randomActualInterval; }
                else
                {
                    int Length = Math.Min(RandomError.Length, RandomMeasuredValue.Length);
                    double[] ActualInterval = new double[Length];
                    for (int i = 0; i < Length; i++)
                    { ActualInterval[i] = Math.Round(RandomError[i] + RandomMeasuredValue[i], 2); }
                    return ActualInterval;
                }
            }
            set { randomActualValue = value; }
        }
        /// <summary>
        /// Случайные измеренные значения
        /// </summary>
        public double[] RandomMeasuredValue
        {
            get 
            {
                if ((randomMeasuredValue != null) || (randomMeasuredValue.Length > 0))
                {return randomMeasuredValue;}
                else
                {
                    int Length = Math.Min(RandomActualValue.Length, RandomError.Length);
                    double[] MeasuredValue = new double[Length];
                    for(int i = 0; i < Length; i++)
                    {MeasuredValue[i] = Math.Round(RandomActualValue[i] - RandomError[i], 2);}
                    return MeasuredValue;
                }
            }
            set{randomMeasuredValue = value;}
        }
        /// <summary>
        /// Случайные стартовые значения
        /// </summary>
        public double[] RandomMotionStartValue
        {
            get {return randomMotionStartValue;}
            set {randomMotionStartValue = value;}
        }
        /// <summary>
        /// Случайные значения погрешности
        /// </summary>
        public double[] RandomError
        {
            get 
            {
                if ((randomError != null) && (randomError.Length > 0))
                {return randomError;}
                else
                {
                    int Length = Math.Min(RandomActualValue.Length, RandomMeasuredValue.Length);
                    double[] Error = new double[Length];
                    for(int i = 0; i < Length; i++)
                    {
                        double rmv = RandomMeasuredValue[i];
                        double rav = RandomActualValue[i];
                        Error[i] = Math.Round(((rmv - rav) / rav) * 100, 3);
                        //Error[i] = Math.Abs(Math.Round(((rmv - rav) / rav) * 100, 2));
                        /*System.Windows.Forms.MessageBox.Show("RandomMeasuredValue = " + rmv);
                        System.Windows.Forms.MessageBox.Show("RandomActualValue = " + rav);
                        System.Windows.Forms.MessageBox.Show("error = " + ((rmv - rav) / rav) * 100);
                        System.Windows.Forms.MessageBox.Show("error = " +  Math.Round(((rmv - rav) / rav) * 100));
                        System.Windows.Forms.MessageBox.Show("Error[i] = " + Error[i]);*/

                    }
                    return Error;
                }
            }
            set{randomError = value;}
        }
        /// <summary>
        /// Случайные показания.
        /// </summary>
        public double[] RandomIndication
        {
            get 
            {
                if ((randomIndication != null) && (randomIndication.Length > 0))
                {return randomIndication;}
                else
                {
                    //int Length = Math.Min(RandomActualValue.Length, RandomMeasuredValue.Length);
                    int Length = RandomMeasuredValue.Length;
                    double[] Indication = new double[Length];
                    for(int i = 0; i < Length; i++)
                    {Indication[i] = Math.Round(RandomMotionStartValue[i] + RandomActualInterval[i], 2);}
                    return Indication;
                }
            }
            set{randomError = value;}
        }
        /// <summary>
        /// Значения переменных
        /// </summary>
        public string[] Variables
        {
            get {return variables;}
            set {variables = value;}
        }
        /// <summary>
        /// Данные строк таблицы
        /// </summary>
        public List<TablesRow> Rows;
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
                {Values[i] = ConvertData(Row.Values[i], CurrentTable);}
                Collection.Add(new TablesRow(Values, Row.GridSpan, Row.VerticalMerge, Row.Type, Row.JoinToRow));
            }
            return Collection;
        }
        /// <summary>
        /// Получить данные строк таблицы
        /// </summary>
        /// <param name="ParentDocument"> Родительский документ (из которого выгружаются данные).</param>
        /// <returns></returns>
        public List<TablesRow> GetRows(RootObject ParentDocument)
        {
            List<TablesRow> Collection = new List<TablesRow>();

            foreach (TablesRow Row in Rows)
            {
                string[] Values = new string[Row.Values.Count()];
                for (int i = 0; i < Row.Values.Count(); i++)
                { Values[i] = ConvertData(Row.Values[i], ParentDocument); }
                Collection.Add(new TablesRow(Values, Row.GridSpan, Row.VerticalMerge, Row.Type, Row.JoinToRow));
            }
            return Collection;
        }
        /// <summary>
        /// Получить данные строк таблицы.
        /// </summary>
        /// <param name="ParentDocument"> Родительский документ (из которого выгружаются данные). </param>
        /// <param name="SensorsNumber"> Заводской номер датчика. </param>
        /// <returns></returns>
        public List<TablesRow> GetRows(WordprocessingDocument ParentDocument, string SensorsNumber)
        {
            Table CurrentTable = ParentDocument == null ? null : ParentDocument.GetTable(ParentTableName);

            string RodInfo = "";
            string RodLength = CurrentTable.GetCellValue(5, 1).ToString() == "Выберите длину" ? "" : CurrentTable.GetCellValue(5, 1).ToString();      // Длина стержня
            string RodNumber = CurrentTable.GetCellValue(6, 1).ToString() == "Введите номер" ? "" : CurrentTable.GetCellValue(6, 1).ToString();       // Номер стержня
            string ErrorValue = CurrentTable.GetCellValue(3, 1).ToString();       // Максимальная погрешность
            if ((RodLength != "") && (RodNumber != ""))
            { RodInfo = "в комплекте со стержнем длиной " + RodLength + ", № " + RodNumber; }

            List<TablesRow> Collection = new List<TablesRow>();
            Variables = new string[] { SensorsNumber, RodInfo, ErrorValue };

            foreach (TablesRow Row in Rows)
            {
                string[] Values = new string[Row.Values.Count()];
                for (int i = 0; i < Row.Values.Count(); i++)
                {Values[i] = ConvertData(Row.Values[i], CurrentTable);}
                Collection.Add(new TablesRow(Values, Row.GridSpan, Row.VerticalMerge, Row.Type, Row.JoinToRow));
            }
            return Collection;
        }
        /// <summary>
        /// Преобразовать данные
        /// </summary>
        /// <param name="Value"> Значение ячейки таблицы. </param>
        /// <param name="CurrentTable"> Текущая таблица. </param>
        /// <returns></returns>
        public string ConvertData(string Value, Table CurrentTable)
        {
            string[] Index = Value.Split(';');

            if (Index.Count() == 2 && CurrentTable != null)
            {
                return CurrentTable.GetCellValue(Convert.ToInt32(Index[0]), Convert.ToInt32(Index[1]));
            }
            else
            {
                if (Index.Count() == 4 && CurrentTable != null)
                {
                    String CellValue = CurrentTable.GetCellValue(Convert.ToInt32(Index[0]), Convert.ToInt32(Index[1]));
                    String Delimiter = (CellValue.IndexOf(Index[2]) < 0) ? "," : Index[2];
                    Int32 Accuracy = Convert.ToInt32(Index[3]);
                    Int32 ValueLength = Accuracy > 0 ? Accuracy + 1 : Accuracy;

                    return CellValue.Substring(0, CellValue.IndexOf(Delimiter) + ValueLength).Replace(Delimiter, ".");
                }
                if (Index.Count() == 7 && CurrentTable != null)
                {
                    String StringOperand1 = CurrentTable.GetCellValue(Convert.ToInt32(Index[0]), Convert.ToInt32(Index[1]));
                    String StringOperand2 = CurrentTable.GetCellValue(Convert.ToInt32(Index[3]), Convert.ToInt32(Index[4]));
                    String Operator = Index[2];
                    String Delimiter = (StringOperand1.IndexOf(Index[5]) < 0) && (StringOperand1.IndexOf(Index[5]) < 0) ? "," : Index[5];
                    Int32 Accuracy = Convert.ToInt32(Index[6]);
                    Int32 ValueLength = Accuracy > 0 ? Accuracy + 1 : Accuracy;

                    String NumDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    Double DoubleOperand1 = Convert.ToDouble(StringOperand1.Substring(0, StringOperand1.IndexOf(Delimiter) + ValueLength).Replace(Delimiter, NumDecimalSeparator));
                    Double DoubleOperand2 = Convert.ToDouble(StringOperand2.Substring(0, StringOperand2.IndexOf(Delimiter) + ValueLength).Replace(Delimiter, NumDecimalSeparator));

                    if (Operator == "-")
                    {
                        return (Math.Round(DoubleOperand1 - DoubleOperand2, Accuracy)).ToString().Replace(NumDecimalSeparator, ".");
                    }
                    if (Operator == "%")
                    {
                        return Math.Round((DoubleOperand1 - DoubleOperand2) / DoubleOperand1, Accuracy).ToString().Replace(NumDecimalSeparator, ".");
                    }
                }
                else
                {
                    if (Value.IndexOf("Random") >= 0)
                    {
                        int randomIndex = Convert.ToInt32(Value.Substring(Value.IndexOf("[") + 1, Value.IndexOf("]") - Value.IndexOf("[") - 1));
                        string value = Value.IndexOf("]") + 1 < Value.Length ? Value.Substring(Value.IndexOf("]") + 1) : "";

                        if (Value.IndexOf("RandomActualValue") >= 0)
                        { return Convert.ToString(RandomActualValue[randomIndex]).Replace(".", ",") + value; }

                        if (Value.IndexOf("RandomActualInterval") >= 0)
                        { return Convert.ToString(RandomActualInterval[randomIndex]).Replace(".", ",") + value; }

                        if (Value.IndexOf("RandomMeasuredValue") >= 0)
                        { return Convert.ToString(RandomMeasuredValue[randomIndex]).Replace(".", ",") + value; }

                        if (Value.IndexOf("RandomMotionStartValue") >= 0)
                        { return Convert.ToString(RandomMotionStartValue[randomIndex]).Replace(".", ",") + value; }

                        if (Value.IndexOf("RandomError") >= 0)
                        {
                            return Value.IndexOf("RandomError") > 0 ?
                                Value.Substring(0, Value.IndexOf("RandomError")) + Convert.ToString(RandomError[randomIndex]).Replace(".", ",") + value :
                                Convert.ToString(RandomError[randomIndex]).Replace(".", ",") + value;
                        }

                        if (Value.IndexOf("RandomIndication") >= 0)
                        { return Convert.ToString(RandomIndication[randomIndex]).Replace(".", ",") + value; }
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
        /// Преобразовать данные json
        /// </summary>
        /// <param name="Value"> Значение ячейки таблицы. </param>
        /// <param name="ParentDocument"> Объект с данными измерений. </param>
        /// <returns></returns>
        public string ConvertData(string Value, RootObject ParentDocument)
        {
            string[] Index = Value.Split(';');

            if (Index.Count() == 2 && ParentDocument != null)
            {
                return ParentDocument.GetMeasure(Convert.ToInt32(Index[0])).Index(Convert.ToInt32(Index[1]));
            }
            else
            {
                 if (Index.Count() == 4 && ParentDocument != null)
                {
                    String CellValue = ParentDocument.GetMeasure(Convert.ToInt32(Index[0])).Index(Convert.ToInt32(Index[1]));
                    String Delimiter = (CellValue.IndexOf(Index[2]) < 0) ? "," : Index[2];
                    Int32 Accuracy = Convert.ToInt32(Index[3]);
                    Int32 ValueLength = Accuracy > 0 ? Accuracy + 1 : Accuracy;

                    return CellValue.Substring(0, CellValue.IndexOf(Delimiter) + ValueLength).Replace(Delimiter, ".");
                }
                if (Index.Count() == 7 && ParentDocument != null)
                {
                    String StringOperand1 = ParentDocument.GetMeasure(Convert.ToInt32(Index[0])).Index(Convert.ToInt32(Index[1]));
                    String StringOperand2 = ParentDocument.GetMeasure(Convert.ToInt32(Index[3])).Index(Convert.ToInt32(Index[4]));
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
                        {
                            return Convert.ToString(RandomActualValue[randomIndex]) + value;
                        }

                        if (Value.IndexOf("RandomActualInterval") >= 0)
                        {
                            return Convert.ToString(RandomActualInterval[randomIndex]) + value;
                        }

                        if (Value.IndexOf("RandomMeasuredValue") >= 0)
                        {
                            return Convert.ToString(RandomMeasuredValue[randomIndex]) + value;
                        }

                        if (Value.IndexOf("RandomMotionStartValue") >= 0)
                        {
                            return Convert.ToString(RandomMotionStartValue[randomIndex]) + value;
                        }

                        if (Value.IndexOf("RandomError") >= 0)
                        {
                            return Value.IndexOf("RandomError") > 0 ?
                                Value.Substring(0, Value.IndexOf("RandomError")) + Convert.ToString(RandomError[randomIndex]) + value :
                                Convert.ToString(RandomError[randomIndex]) + value;
                        }

                        if (Value.IndexOf("RandomIndication") >= 0)
                        {
                            return Convert.ToString(RandomIndication[randomIndex]) + value;
                        }
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
        /// Сформировать таблицу с данными измерений прибора.
        /// </summary>
        /// <param name="ParentDocument"> Родительский документ (из которого выгружаются данные). </param>
        /// <returns></returns>
        public Table GetDeviceTable(WordprocessingDocument ParentDocument)
        {
            int TableWidth = 9782;
            //if (ColumnsWidthList)
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
        /// Сформировать таблицу с данными измерений датчика.
        /// </summary>
        /// <param name="ParentDocument"> Родительский документ (из которого выгружаются данные). </param>
        /// <param name="SensorsNumber"> Заводской номер датчика. </param>
        /// <returns></returns>
        public Table GetSensorsTable(WordprocessingDocument ParentDocument, string SensorsNumber)
        {
            const int TableWidth = 9782;
            int[] ColumnsWidth = new int [] {1277, 284, 1275, 5103, 1666, 177};

            Table table = CalibrationLib.NewTable(ColumnsWidth);
            List<TablesRow> TableRows = GetRows(ParentDocument, SensorsNumber);

            TableRow tableRow1 = CalibrationLib.NewTableRow((UInt32Value)198U);
            tableRow1.AddTableCell(ColumnsWidth[0], TableRows[0].Values[0], -113, -113, JustificationValues.Right, 8, 1, true, true);
            tableRow1.AddTableCell(ColumnsWidth[1], TableRows[0].Values[1], -113, -113, JustificationValues.Center, 8, 1, true, true);
            tableRow1.AddTableCell(ColumnsWidth[2], TableRows[0].Values[2], -113, -113, JustificationValues.Center, 8, 1, true, true).SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Single, BorderValues.Nil);
            tableRow1.AddTableCell(ColumnsWidth[3] + ColumnsWidth[4] + ColumnsWidth[5], TableRows[0].Values[3], -113, -113, JustificationValues.Left, 8, 3, true, true);

            TableRow tableRow2 = CalibrationLib.NewTableRow((UInt32Value)198U);
            tableRow2.AddTableCell(ColumnsWidth[0] + ColumnsWidth[1] + ColumnsWidth[2] + ColumnsWidth[3], TableRows[1].Values[0], -113, -113, JustificationValues.Left, 8, 4);
            tableRow2.AddTableCell(ColumnsWidth[4] + ColumnsWidth[5], TableRows[1].Values[1], -113, -113, JustificationValues.Center, 12, 2, false, true).SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Dotted, BorderValues.Nil);

            table.Append(tableRow1);
            table.Append(tableRow2);

            return table;
        }
        /// <summary>
        /// Создать заголовок таблицы.
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <param name="TableWidth"> Ширина столбца. </param>
        /// <returns></returns>
        internal TableRow CreateTitle(TablesRow NewRow, int TableWidth)
        {
            TableRow NewTableRow = CalibrationLib.NewTableRow((UInt32Value)198U);
            
            for (int i = 0; i < NewRow.ColumnsCount; i++)
            {
                TableCell NewTableCell = NewTableRow.AddTableCell(TableWidth / NewRow.ColumnsCount, NewRow.Values[i], -113, -113, JustificationValues.Left, 8, 
                    ColumnsCount, false, false);
                NewTableCell.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Nil, BorderValues.Nil);
            }
            return NewTableRow;
        }
        /// <summary>
        /// Создать подзаголовок таблицы.
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <param name="TableWidth"> Ширина столбца. </param>
        /// <returns></returns>
        internal TableRow CreateSubTitle(TablesRow NewRow, int TableWidth)
        {
            TableRow NewTableRow = CalibrationLib.NewTableRow((UInt32Value)198U);

            for (int i = 0; i < NewRow.ColumnsCount; i++)
            {
                TableCell NewTableCell = NewTableRow.AddTableCell(TableWidth / NewRow.ColumnsCount, NewRow.Values[0], -113, -113, JustificationValues.Center, 8,
                    ColumnsCount, false, true);
                NewTableCell.SetBorders(BorderValues.Single, BorderValues.Single, BorderValues.Single, BorderValues.Single);
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
            TableRow NewTableRow = CalibrationLib.NewTableRow((UInt32Value)198U);

            for (int i = 0; i < NewRow.ColumnsCount; i++)
            {
                TableCell NewTableCell = NewTableRow.AddTableCell(ColumnsWidthList[i], NewRow.Values[i], -113, -113, JustificationValues.Center, 8,
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
            TableRow NewTableRow = CalibrationLib.NewTableRow((UInt32Value)198U);

            for (int i = 0; i < NewRow.ColumnsCount; i++)
            {
                bool Italic = i == 0 ? false : true;
                TableCell NewTableCell = NewTableRow.AddTableCell(ColumnsWidthList[i], NewRow.Values[i], -113, -113, JustificationValues.Center, 8,
                    NewRow.GridSpan[i], false, Italic, NewRow.VerticalMerge[i]);
                NewTableCell.SetBorders(BorderValues.Single, BorderValues.Single, BorderValues.Single, BorderValues.Single);
            }
            return NewTableRow;
        }
        /// <summary>
        /// Создать результирующую строку таблицы.
        /// </summary>
        /// <param name="NewRow"> Строка таблицы. </param>
        /// <param name="TableWidth"> Ширина столбца. </param>
        /// <returns></returns>
        internal TableRow CreateResult(TablesRow NewRow, int TableWidth)
        {
            TableRow NewTableRow = CalibrationLib.NewTableRow((UInt32Value)198U);
            for (int i = 0; i < NewRow.ColumnsCount; i++)
            {
                TableCell NewTableCell = NewTableRow.AddTableCell(TableWidth / NewRow.ColumnsCount, NewRow.Values[i], -113, -113, JustificationValues.Center, 8,
                    NewRow.GridSpan[i], false, true, NewRow.VerticalMerge[i]);
                NewTableCell.SetBorders(BorderValues.Nil, BorderValues.Nil, BorderValues.Nil, BorderValues.Nil);
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
    }

    class TablesRow
    {
        /// <summary>
        /// Тип строки.
        /// </summary>
        public enum RowsType
        {
            /// <summary>
            /// Заголовок таблицы.
            /// </summary>
            [Description("Заголовок")]
            Title = 0,
            /// <summary>
            /// Шапка таблицы.
            /// </summary>
            [Description("Шапка")]
            Header = 1,
            /// <summary>
            /// Подзаголовок.
            /// </summary>
            [Description("Подзаголовок")]
            Subtitle = 2,
            /// <summary>
            /// Обычная строка.
            /// </summary>
            [Description("Строка")]
            SimpleRow = 3,
            /// <summary>
            /// Итог.
            /// </summary>
            [Description("Итог")]
            Result = 4,
        };
        public RowsType Type;
        public int ColumnsCount;
        public int[] GridSpan;
        public string[] Values;
        public int[] VerticalMerge;
        public int JoinToRow;

        public TablesRow(string[] Values, RowsType Type, int JoinToRow = 0)
        {
            this.Type = Type;
            this.Values = Values;
            this.JoinToRow = JoinToRow;
            this.ColumnsCount = Values != null ? Values.Count() : 1;

            this.GridSpan = new int[this.ColumnsCount];
            for (int i = 0; i < this.ColumnsCount; i++)
                this.GridSpan[i] = 1;

            this.VerticalMerge = new int[this.ColumnsCount];
            for (int i = 0; i < this.ColumnsCount; i++)
                this.VerticalMerge[i] = 0;
        }
        public TablesRow(string[] Values, int[] GridSpan, RowsType Type, int JoinToRow = 0)
        {
            this.Type = Type;
            this.Values = Values;
            this.ColumnsCount = Values != null ? Values.Count() : 1;
            this.GridSpan = GridSpan;
            this.JoinToRow = JoinToRow;

            this.VerticalMerge = new int[this.ColumnsCount];
            for (int i = 0; i < this.ColumnsCount; i++)
                this.VerticalMerge[i] = 0;
        }
        public TablesRow(string[] Values, int[] GridSpan, int[] VerticalMerge, RowsType Type, int JoinToRow = 0)
        {
            this.Type = Type;
            this.Values = Values;
            this.ColumnsCount = Values != null ? Values.Count() : 1;
            this.GridSpan = GridSpan;
            this.VerticalMerge = VerticalMerge;
            this.JoinToRow = JoinToRow;
        }
    }
}
