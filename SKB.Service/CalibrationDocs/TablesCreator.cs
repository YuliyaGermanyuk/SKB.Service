using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SKB.Service.CalibrationDocs
{
    class TablesCreator
    {
        private static Random RandomValue = new Random();

        #region Таблицы с данными измерений датчиков для "Сертификата о калибровке"

        public CertificateTableLook[] CertificateSensorTablesCollection = new CertificateTableLook[]
        {
            #region ДП12
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ДП12"},
                TableName = "ДП12",
                ColumnsCount = 4,
                ParentTableName = "Результаты приемосдаточных испытаний:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "ДП12", "№", "Variables[0]", "Variables[1]" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерения линейных перемещений, мм", "Variables[2]" }, TablesRow.RowsType.Header),
                }
            },
            #endregion

            #region ДП21
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ДП21"},
                TableName = "ДП21",
                ColumnsCount = 4,
                ParentTableName = "Результаты приемосдаточных испытаний:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "ДП21", "№", "Variables[0]", "" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] {  "Основная абсолютная погрешность измерения угловых перемещений, градус", "Variables[2]" }, TablesRow.RowsType.Header),
                }
            },
            #endregion

            #region ДП22
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ДП22"},
                TableName = "ДП22",
                ColumnsCount = 4,
                ParentTableName = "Результаты приемосдаточных испытаний:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "ДП22", "№", "Variables[0]", "" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] {  "Основная абсолютная погрешность измерений угловых перемещений, градус", "Variables[2]" }, TablesRow.RowsType.Header),
                }
            }
            #endregion
        };
        #endregion

        #region Таблицы с данными измерений приборов для "Сертификата о калибровке"
        public CertificateTableLook[] CertificateDeviceTablesCollection = new CertificateTableLook[]
        { 
            #region МИКО-1
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-1"},
                TableName = "Основная относительная погрешность измерений электрического сопротивления",
                ColumnsCount = 4,
                ParentTableName = "Результаты измерений метрологических характеристик",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерений электрического сопротивления" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, μΩ", "Измеренное значение, μΩ", "Действительное значение, μΩ", "Относительная погрешность, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "1", "2;2", "2;1", "2;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10", "3;2", "3;1", "3;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100", "4;2", "4;1", "4;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "5;2", "5;1", "5;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000", "6;2", "6;1", "6;4" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-2
            // МИКО-2, таблица 1
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.2", "МИКО-2.3"},
                TableName = "Режим «Микроомметр»",
                ColumnsCount = 4,
                ParentTableName = "Режим «Микроомметр»",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Микроомметр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // МИКО-2, таблица 2
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.2", "МИКО-2.3"},
                TableName = "Режим «Миллиомметр»",
                ColumnsCount = 4,
                ParentTableName = "Режим «Миллиомметр»",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Миллиомметр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6;0", "6;1", "6;2", "6;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "7;0", "7;1", "7;2", "7;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8;0", "8;1", "8;2", "8;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9;0", "9;1", "9;2", "9;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // МИКО-2, таблица 3
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.2", "МИКО-2.3"},
                TableName = "Режим «Килоомметр»",
                ColumnsCount = 4,
                ParentTableName = "Режим «Килоомметр»",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Килоомметр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6;0", "6;1", "6;2", "6;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "7;0", "7;1", "7;2", "7;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // МИКО-2, таблица 4
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.2", "МИКО-2.3"},
                TableName = "Режим «Термометр»",
                ColumnsCount = 4,
                ParentTableName = "Режим «Термометр» (Пределы допускаемой абсолютной погрешности измерений температуры ±1˚С)",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Термометр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Термометр К411 № Variables[0]"}, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Измеренное значение, \xB0С", "Действительное значение, \xB0С", "Абсолютная погрешность, \xB0С" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "-20\xB0С", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0\xB0С", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "+100\xB0С", "6;1", "6;2", "6;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-7
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-7"},
                TableName = "Основная относительная погрешность измерения электрического сопротивления",
                ColumnsCount = 4,
                ParentTableName = "Основная относительная погрешность измерения электрического сопротивления",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерения электрического сопротивления" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6;0", "6;1", "6;2", "6;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "7;0", "7;1", "7;2", "7;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8;0", "8;1", "8;2", "8;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9;0", "9;1", "9;2", "9;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10;0", "10;1", "10;2", "10;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-7М (А)
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-7М", "МИКО-7МА"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3", "1;4", "1;5" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3", "4;4", "4;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3", "5;4", "5;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6;0", "6;1", "6;2", "6;3", "6;4", "6;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "7;0", "7;1", "7;2", "7;3", "7;4", "7;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8;0", "8;1", "8;2", "8;3", "8;4", "8;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9;0", "9;1", "9;2", "9;3", "9;4", "9;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10;0", "10;1", "10;2", "10;3", "10;4", "10;5" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-8
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-8"},
                TableName = "Основная относительная погрешность измерения электрического сопротивления",
                ColumnsCount = 4,
                ParentTableName = "Основная относительная погрешность измерения электрического сопротивления",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерения электрического сопротивления" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6;0", "6;1", "6;2", "6;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "7;0", "7;1", "7;2", "7;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8;0", "8;1", "8;2", "8;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9;0", "9;1", "9;2", "9;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10;0", "10;1", "10;2", "10;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "11;0", "11;1", "11;2", "11;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-8М (А)
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-8М", "МИКО-8МА"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3", "1;4", "1;5" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3", "4;4", "4;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3", "5;4", "5;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6;0", "6;1", "6;2", "6;3", "6;4", "6;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "7;0", "7;1", "7;2", "7;3", "7;4", "7;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8;0", "8;1", "8;2", "8;3", "8;4", "8;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9;0", "9;1", "9;2", "9;3", "9;4", "9;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10;0", "10;1", "10;2", "10;3", "10;4", "10;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "11;0", "11;1", "11;2", "11;3", "11;4", "11;5" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-9 (А)
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-9", "МИКО-9А"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3", "1;4", "1;5" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3", "4;4", "4;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3", "5;4", "5;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6;0", "6;1", "6;2", "6;3", "6;4", "6;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "7;0", "7;1", "7;2", "7;3", "7;4", "7;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8;0", "8;1", "8;2", "8;3", "8;4", "8;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9;0", "9;1", "9;2", "9;3", "9;4", "9;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10;0", "10;1", "10;2", "10;3", "10;4", "10;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "11;0", "11;1", "11;2", "11;3", "11;4", "11;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "12;0", "12;1", "12;2", "12;3", "12;4", "12;5" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-10
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-10"},
                TableName = "Основная относительная погрешность измерения электрического сопротивления",
                ColumnsCount = 5,
                ParentTableName = "Определение погрешности измерения сопротивления в режиме «ОДНОКРАТНЫЙ»",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,
                RandomActualValue = new double[] {0,9901},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(6930, 12871) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерения электрического сопротивления" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Сила тока, А", "Измеренное значение", "Действительное значение", "Погрешность измерения, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "1 мкОм", "10", "2;3", "2;1", "2;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 мкОм", "10", "3;3", "3;1", "3;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 мОм", "10", "4;3", "4;1", "4;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 мОм", "10", "5;3", "5;1", "5;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 мОм", "1", "6;3", "6;1", "6;4" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-21
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"МИКО-21"},
                TableName = "Основная относительная погрешность измерения электрического сопротивления",
                ColumnsCount = 5,
                ParentTableName = "Основная относительная погрешность измерения электрического сопротивления",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерения электрического сопротивления" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3" }, new int[] {1, 1, 1, 2 }, new int[] {1, 1, 1, 0}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "", "", "", "2;3", "2;4" }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 2, 0, 0}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3", "4;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3", "5;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6;0", "6;1", "6;2", "6;3", "6;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "7;0", "7;1", "7;2", "7;3", "7;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8;0", "8;1", "8;2", "8;3", "8;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9;0", "9;1", "9;2", "9;3", "9;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10;0", "10;1", "10;2", "10;3", "10;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "11;0", "11;1", "11;2", "11;3", "11;4" }, TablesRow.RowsType.SimpleRow),
                }
            },
            #endregion

            #region ПКВ/В1
            // ПКВ/В1, таблица 1
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/В1"},
                TableName = "Основная абсолютная погрешность интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.None,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Замыкание контактов" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Размыкание контактов" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/В1, таблица 2
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/В1"},
                TableName = "Основная относительная погрешность измерения силы электрического тока",
                ColumnsCount = 4,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.None,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерения силы электрического тока" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Измеренное значение, А", "Действительное значение, А", "Погрешность, А" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/М3
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М3"},
                TableName = "Основная относительная погрешность измерения силы электрического тока",
                ColumnsCount = 4,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.None,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерения силы электрического тока" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, А", "Измеренное значение, А", "Действительное значение, А", "Погрешность, А" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Поддиапазон измерения электрического тока от 0 до 10 А" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Поддиапазон измерения электрического тока от 0 до 20 А" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Поддиапазон измерения электрического тока от 0 до 35 А" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "", "" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/М4
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М4"},
                TableName = "Основная абсолютная погрешность измерения интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.None,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерения интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, мс", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Местный пуск (размыкание)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Местный пуск (замыкание)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Дистанционный пуск (размыкание)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Дистанционный пуск (замыкание)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "", "", "" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "", "", "" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/М5
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М5", "ПКВ/М5А", "ПКВ/М5Н"},
                TableName = "Основная абсолютная погрешность измерений интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "Определение погрешности измерения временных интервалов по каналам включения и отключения при дистанционном пуске",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, мс", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Дистанционный пуск (размыкание)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "3;2", "3;1", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "4;2", "4;1", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Дистанционный пуск (замыкание)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "3;5", "3;4", "3;6" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "4;5", "4;4", "4;6" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Местный пуск (размыкание)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "8;2", "8;1", "8;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "9;2", "9;1", "9;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Местный пуск (замыкание)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "8;5", "8;4", "8;6" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "9;5", "9;4", "9;6" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/М6
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М6", "ПКВ/М6Н"},
                TableName = "Основная абсолютная погрешность измерений интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "Определение основной абсолютной погрешности измерения интервалов времени",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, мс", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Замыкание контактов" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "3;3", "3;2", "3;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "6;3", "6;2", "6;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Размыкание контактов" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "3;6", "3;5", "3;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "6;6", "6;5", "6;7" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/М7
            // ПКВ/М7, таблица 1
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Основная абсолютная погрешность измерений силы постоянного электрического тока каналами силового коммутатора",
                ColumnsCount = 7,
                ParentTableName = "Определение основной абсолютной погрешности измерений силы постоянного электрического тока каналами ВКЛ и ОТКЛ силового коммутатора",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений силы постоянного электрического тока каналами силового коммутатора" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, А", "1;1", "1;2", "1;3", "1;4", "1;5", "1;6" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "ВКЛ" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "2;1", "2;2", "2;3", "2;4", "2;5", "2;6", "2;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;1", "3;2", "3;3", "3;4", "3;5", "3;6", "3;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "ОТКЛ" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "2;1", "4;2", "4;3", "4;4", "4;5", "4;6", "4;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;1", "5;2", "5;3", "5;4", "5;5", "5;6", "5;7" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/М7, таблица 2
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Основная абсолютная погрешность измерений электрического напряжения постоянного тока каналом НАПРЯЖЕНИЕ КОММУТАТОРА",
                ColumnsCount = 8,
                ParentTableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом НАПРЯЖЕНИЕ КОММУТАТОРА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений электрического напряжения постоянного тока каналом НАПРЯЖЕНИЕ КОММУТАТОРА" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;1", "1;2", "1;3", "1;4", "1;5", "1;6", "1;7" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5", "2;6", "2;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5", "3;6", "3;7" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/М7, таблица 3
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Основная абсолютная погрешность измерений электрического напряжения постоянного тока в диапазоне ±1 В каналом ВХОД АНАЛОГОВЫЙ",
                ColumnsCount = 8,
                ParentTableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока в диапазоне ±1 В каналом ВХОД АНАЛОГОВЫЙ",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений электрического напряжения постоянного тока в диапазоне ±1 В каналом ВХОД АНАЛОГОВЫЙ" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;1", "1;2", "1;3", "1;4", "1;5", "1;6", "1;7" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5", "2;6", "2;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5", "3;6", "3;7" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/М7, таблица 4
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Основная абсолютная погрешность измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Униполярный режим",
                ColumnsCount = 5,
                ParentTableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Униполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Униполярный режим" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;1", "1;2", "1;3", "1;4" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/М7, таблица 5
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Основная абсолютная погрешность измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Биполярный режим",
                ColumnsCount = 8,
                ParentTableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Биполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Биполярный режим" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;1", "1;2", "1;3", "1;4", "1;5", "1;6", "1;7" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5", "2;6", "2;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5", "3;6", "3;7" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/М7, таблица 6
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Основная абсолютная погрешность измерений электрического сопротивления постоянному току в диапазоне от 0 до 2500 Ом",
                ColumnsCount = 6,
                ParentTableName = "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 2500 Ом",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений электрического сопротивления постоянному току в диапазоне от 0 до 2500 Ом" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;1", "1;2", "1;3", "1;4", "1;5" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/М7, таблица 7
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Основная абсолютная погрешность измерений электрического сопротивления постоянному току в диапазоне от 0 до 160 Ом",
                ColumnsCount = 6,
                ParentTableName = "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 160 Ом",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений электрического сопротивления постоянному току в диапазоне от 0 до 160 Ом" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;1", "1;2", "1;3", "1;4", "1;5" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/М7, таблица 8
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Основная абсолютная погрешность измерений интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "Определение основной абсолютной погрешности измерений интервалов времени",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, мс", "tд, мс", "tизм, мс", "Δ, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "100", "2;1", "3;1", "4;1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "2;2", "3;2", "4;2" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У2
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У2"},
                TableName = "Основная абсолютная погрешность измерения интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "Определение погрешности задания интервалов времени",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерения интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, мс", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    /*new TablesRow(new string[] { "Замыкание" }, TablesRow.RowsType.Subtitle),*/
                    new TablesRow(new string[] { "100", "3;1", "2;1", "4;1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "3;2", "2;2", "4;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8000", "3;3", "2;3", "4;3" }, TablesRow.RowsType.SimpleRow),
                    /*new TablesRow(new string[] { "Размыкание" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "4;1", "3;1", "5;1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "4;2", "3;2", "5;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8000", "4;3", "3;3", "5;3" }, TablesRow.RowsType.SimpleRow)*/
                }
            },
            #endregion

            #region ПКВ/У3

            #region ПКВ/У3, таблица 1, Погрешность силы тока
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Основная абсолютная погрешность измерений силы постоянного электрического тока каналами силового коммутатора",
                ColumnsCount = 8,
                ParentTableName = "Основная абсолютная погрешность измерений силы постоянного электрического тока каналами силового коммутатора",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений силы постоянного электрического тока каналами силового коммутатора" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, А", "1;1", "1;2", "1;3", "1;4", "1;5", "1;6" }, new int[] { 2, 1, 1, 1, 1, 1, 1}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5", "2;6", "2;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5", "3;6", "3;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "2;1", "4;2", "4;3", "4;4", "4;5", "4;6", "4;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "3;1", "5;2", "5;3", "5;4", "5;5", "5;6", "5;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

                #region ПКВ/У3, таблица 2, Напряжение коммутатора
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом \"Входное напряжение коммутатора\"",
                ColumnsCount = 9,
                ParentTableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом \"Входное напряжение коммутатора\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом \"Входное напряжение коммутатора\"" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3", "1;4", "1;5", "1;6", "1;7" }, new int[] { 2, 1, 1, 1, 1, 1, 1, 1}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5", "2;6", "2;7" }, new int[] { 2, 1, 1, 1, 1, 1, 1, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5", "3;6", "3;7" }, new int[] { 2, 1, 1, 1, 1, 1, 1, 1}, TablesRow.RowsType.SimpleRow)
                }
            },
                #endregion

                #region ПКВ/У3, таблица 3, Униполярный режим
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в униполярном режиме",
                ColumnsCount = 5,
                ParentTableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в униполярном режиме",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в униполярном режиме" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3"}, new int[] { 2, 1, 1, 1}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4" }, new int[] { 1, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", }, new int[] { 1, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3", "4;4" }, new int[] { 1, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3", "5;4" }, new int[] { 1, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
                #endregion

                #region ПКВ/У3, таблица 4, Биполярный режим
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в биполярном режиме",
                ColumnsCount = 8,
                ParentTableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в биполярном режиме",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в биполярном режиме" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3", "1;4", "1;5", "1;6" }, new int[] { 2, 1, 1, 1, 1, 1, 1, 1}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5", "2;6", "2;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5", "3;6", "3;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3", "4;4", "4;5", "4;6", "4;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3", "5;4", "5;5", "5;6", "5;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },
                #endregion

                #region ПКВ/У3, таблица 5, канал Uшунта
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналом «Uшунта»",
                ColumnsCount = 7,
                ParentTableName = "Напряжение, канал Uшунта",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналом «Uшунта»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "δ, %",                "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
                #endregion

                #region ПКВ/У3, таблица 6, Токовые клещи
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналом «Токовые клещи»",
                ColumnsCount = 8,
                ParentTableName = "Определение погрешности измерений электрического напряжения постоянного тока каналом «Токовые клещи»",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений электрического напряжения постоянного тока каналом ТОКОВЫЕ КЛЕЩИ" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3", "1;4", "1;5", "1;6" }, new int[] { 2, 1, 1, 1, 1, 1, 1, 1}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5", "2;6", "2;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5", "3;6", "3;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3", "4;4", "4;5", "4;6", "4;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3", "5;4", "5;5", "5;6", "5;7" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },
                #endregion               

                #region ПКВ/У3, таблица 6, диапазон от 0 до 2500 Ом и от 0 до 160 Ом
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Основная абсолютная погрешность измерений электрического сопротивления постоянному току в диапазонах от 0 до 2500 Ом и от 0 до 160 Ом",
                ColumnsCount = 14,
                ParentTableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 4 мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений электрического сопротивления постоянному току в диапазонах от 0 до 2500 Ом и от 0 до 160 Ом" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3", "1;4", "", "", "", "", "" }, new int[] { 3, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3", "2;4", "2;5", "", "", "", "", "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3", "3;4", "3;5", "", "", "", "", "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3", "4;4", "4;5", "", "", "", "", "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0", "5;1", "5;2", "5;3", "5;4", "5;5", "", "", "", "", "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },

            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Основная абсолютная погрешность измерений электрического сопротивления постоянному току в диапазонах от 0 до 2500 Ом и от 0 до 160 Ом",
                ColumnsCount = 14,
                ParentTableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 60 мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "", "", "", "", "", "1;0", "1;1", "1;2", "1;3", "1;4" }, TablesRow.RowsType.Header, 2),
                    new TablesRow(new string[] { "", "", "", "", "", "", "2;0", "2;1", "2;2", "2;3", "2;4", "2;5" }, TablesRow.RowsType.SimpleRow, 3),
                    new TablesRow(new string[] { "", "", "", "", "", "", "3;0", "3;1", "3;2", "3;3", "3;4", "3;5" }, TablesRow.RowsType.SimpleRow, 4),
                    new TablesRow(new string[] { "", "", "", "", "", "", "4;0", "4;1", "4;2", "4;3", "4;4", "4;5" }, TablesRow.RowsType.SimpleRow, 5),
                    new TablesRow(new string[] { "", "", "", "", "", "", "5;0", "5;1", "5;2", "5;3", "5;4", "5;5" }, TablesRow.RowsType.SimpleRow, 6)
                }
            },
                #endregion

                #region ПКВ/У3, таблица 7, канал реостатных датчиков
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 14,
                ParentTableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0",  "1;1",  "1;2",  "1;3",  "1;4",  "",  "",  "",  "",  "" }, new int[] { 3, 1, 1, 1, 1, 3, 1, 1, 1, 1}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0",  "2;1",  "2;2",  "2;3",  "2;4",  "2;5",  "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0",  "3;1",  "3;2",  "3;3",  "3;4",  "3;5",  "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0",  "4;1",  "4;2",  "4;3",  "4;4",  "4;5", "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5;0",  "5;1",  "5;2",  "5;3",  "5;4",  "5;5",  "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6;0",  "6;1",  "6;2",  "6;3",  "6;4",  "6;5",  "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "7;0",  "7;1",  "7;2",  "7;3",  "7;4",  "7;5",  "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8;0",  "8;1",  "8;2",  "8;3",  "8;4",  "8;5",  "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9;0",  "9;1",  "9;2",  "9;3",  "9;4",  "9;5", "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10;0", "10;1", "10;2", "10;3", "10;4", "10;5", "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "11;0", "11;1", "11;2", "11;3", "11;4", "11;5", "",  "",  "",  "",  "", "" }, new int[] { 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1}, new int[] { 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 14,
                ParentTableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "",  "",  "",  "",  "",  "1;0",  "1;1",  "1;2",  "1;3",  "1;4"}, TablesRow.RowsType.Header, 2),
                    new TablesRow(new string[] { "", "", "", "", "", "",  "12;0",  "12;1",  "12;2",  "12;3",  "12;4",  "12;5" }, TablesRow.RowsType.SimpleRow, 3),
                    new TablesRow(new string[] { "", "", "", "", "", "",  "13;0",  "13;1",  "13;2",  "13;3",  "13;4",  "13;5" }, TablesRow.RowsType.SimpleRow, 4),
                    new TablesRow(new string[] { "", "", "", "", "", "", "14;0",  "14;1",  "14;2",  "14;3",  "14;4",  "14;5" }, TablesRow.RowsType.SimpleRow, 5),
                    new TablesRow(new string[] { "", "", "", "", "", "", "15;0",  "15;1",  "15;2",  "15;3",  "15;4",  "15;5" }, TablesRow.RowsType.SimpleRow, 6),
                    new TablesRow(new string[] { "", "", "", "", "", "", "16;0",  "16;1",  "16;2",  "16;3",  "16;4",  "16;5" }, TablesRow.RowsType.SimpleRow, 7),
                    new TablesRow(new string[] { "", "", "", "", "", "", "17;0",  "17;1",  "17;2",  "17;3",  "17;4",  "17;5" }, TablesRow.RowsType.SimpleRow, 8),
                    new TablesRow(new string[] { "", "", "", "", "", "", "18;0",  "18;1",  "18;2",  "18;3",  "18;4",  "18;5" }, TablesRow.RowsType.SimpleRow, 9),
                    new TablesRow(new string[] { "", "", "", "", "", "", "19;0",  "19;1",  "19;2",  "19;3",  "19;4",  "19;5" }, TablesRow.RowsType.SimpleRow, 10),
                    new TablesRow(new string[] { "", "", "", "", "", "", "20;0",  "20;1",  "20;2",  "20;3",  "20;4",  "20;5" }, TablesRow.RowsType.SimpleRow, 11),
                    new TablesRow(new string[] { "", "", "", "", "", "", "21;0", "21;1", "21;2", "21;3", "21;4", "21;5" }, TablesRow.RowsType.SimpleRow, 12)
                }
            },
                #endregion

                #region ПКВ/У3, таблица 9, Интервалы времени
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Основная абсолютная погрешность измерений интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "Основная абсолютная погрешность измерений интервалов времени",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "1;0", "1;1", "1;2", "1;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2;0", "2;1", "2;2", "2;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "3;0", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4;0", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
                #endregion

            #endregion

            #region ПКР-1
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКР-1"},
                TableName = "Основная абсолютная погрешность измерений интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "Определение абсолютной погрешности измерений интервалов времени",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, мс", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Канал А (U+)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "4;3", "4;2", "4;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "4;6", "4;5", "4;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Канал А (U-)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "5;3", "5;2", "5;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "5;6", "5;5", "5;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Канал В (U+)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "6;3", "6;2", "6;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "6;6", "6;5", "6;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Канал В (U-)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "7;3", "7;2", "7;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "7;6", "7;5", "7;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Канал С (U+)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "8;3", "8;2", "8;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "8;6", "8;5", "8;7" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Канал С (U-)" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "9;3", "9;2", "9;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5000", "9;6", "9;5", "9;7" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКР-2
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКР-2", "ПКР-2М"},
                TableName = "Основная абсолютная погрешность измерений интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "Определение основной абсолютной погрешности измерений интервалов времени",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерений интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Канал А" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100 мс", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000 мс", "4;4", "4;5", "4;6" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Канал В" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100 мс", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000 мс", "5;4", "5;5", "5;6" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Канал С" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100 мс", "6;1", "6;2", "6;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000 мс", "6;4", "6;5", "6;6" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПУВ-10
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПУВ-10"},
                TableName = "Основная абсолютная погрешность задания длительности импульсов",
                ColumnsCount = 4,
                ParentTableName = "Определение погрешности задания интервалов времени по каналам включения и отключения",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность задания длительности импульсов" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, мс", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "200", "2;1", "1;1", "3;1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "400", "2;2", "1;2", "3;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "600", "2;3", "1;3", "3;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПУВ-50
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПУВ-50"},
                TableName = "Основная абсолютная погрешность задания временных интервалов",
                ColumnsCount = 4,
                ParentTableName = "Определение погрешности задания временных интервалов",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность задания временных интервалов" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, мс", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Длительность импульса включения" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "850", "4;2", "4;1", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1700", "5;2", "5;1", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Длительность импульса отключения" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "850", "2;2", "2;1", "2;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1700", "3;2", "3;1", "3;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПУВ-регулятор, ПКВ-35, ПУВ-35
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПУВ-регулятор", "ПКВ-35", "ПУВ-35"},
                TableName = "Основная абсолютная погрешность задания временных интервалов",
                ColumnsCount = 4,
                ParentTableName = "Определение погрешности задания интервалов времени по каналам включения и отключения",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность задания временных интервалов" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, с", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Длительность по каналу «ВКЛ.»" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "0,5", "2;5", "500", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1,9", "2;3", "1900", "3;1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Длительность по каналу «ОТКЛ.»" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "0,5", "2;8", "500", "3;6" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1,9", "2;6", "1900", "3;4" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ТК-026, ТК-021
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ТК-026", "ТК-021"},
                TableName = "Относительная погрешность измерений силы постоянного электрического тока",
                ColumnsCount = 4,
                ParentTableName = "Определение относительной погрешности измерений силы постоянного электрического тока",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность задания временных интервалов" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, А", "Измеренное значение, А", "Действительное значение, А", "Относительная погрешность, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Диапазон до 60 А" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "-54", "4;1", "3;1", "5;1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "-24", "4;2", "3;2", "5;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "-6", "4;3", "3;3", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "6", "4;4", "3;4", "5;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "24", "4;5", "3;5", "5;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "54", "4;6", "3;6", "5;6" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Диапазон до 600 А" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "-540", "10;1", "9;1", "11;1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "-240", "10;2", "9;2", "11;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "-60", "10;3", "9;3", "11;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "60", "10;4", "9;4", "11;4" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "240", "10;5", "9;5", "11;5" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "540", "10;6", "9;6", "11;6" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/В3, ПКВ/В3А
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/В3", "ПКВ/В3А"},
                TableName = "Основная абсолютная погрешность интервалов времени",
                ColumnsCount = 4,
                ParentTableName = "Определение основной абсолютной погрешности измерения интервалов времени",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность интервалов времени" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, мс", "Измеренное значение, мс", "Действительное значение, мс", "Погрешность, мс" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Размыкание контактов" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "4;1", "3;1", "5;1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "4;2", "3;2", "5;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Замыкание контактов" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "100", "4;3", "3;3", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "4;4", "3;4", "5;4" }, TablesRow.RowsType.SimpleRow),
                }
            },
            #endregion

            #region ПКВ/В3, ПКВ/В3А
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/В3", "ПКВ/В3А"},
                TableName = "Основная абсолютная погрешность измерения силы электрического тока",
                ColumnsCount = 4,
                ParentTableName = "Определение основной абсолютной погрешности измерений силы тока",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.CalibrationProtocol,

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная абсолютная погрешность измерения силы электрического тока" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, А", "Измеренное значение, А", "Действительное значение, А", "Погрешность, А" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Поддиапазон измерения электрического тока от 0 до 10 А" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "2", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "5", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "8", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Поддиапазон измерения электрического тока от 0 до 20 А" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "2", "7;1", "7;2", "7;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10", "8;1", "8;2", "8;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "18", "9;1", "9;2", "9;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Поддиапазон измерения электрического тока от 0 до 30 А" }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "5", "11;1", "11;2", "11;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "15", "12;1", "12;2", "12;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "25", "13;1", "13;2", "13;3" }, TablesRow.RowsType.SimpleRow)
                }
            }
            #endregion
        };
        #endregion

        #region Таблицы с дополнительными данными для "Сертификата о калибровке"
        public CertificateTableLook[] CertificateAdditionalTablesCollection = new CertificateTableLook[]
        {
            #region ПКВ/М7
            /*new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Продолжение",
                ColumnsCount = 1,
                ParentTableName = "Продолжение",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.None,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Продолжение см. в Приложении к настоящему Сертификату на одном листе." }, TablesRow.RowsType.Result),
                }
            },*/
            #endregion

            #region ПКВ/У2, ПКВ/У3.0, ПКВ/У3.0-01, ПКВ/У3.1
            new CertificateTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У2" }, // "ПКВ/У3.0", "ПКВ/У3.0-01", "ПКВ/У3.1"},
                TableName = "Продолжение",
                ColumnsCount = 1,
                ParentTableName = "Продолжение",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.None,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Продолжение см. в Приложении к настоящему Сертификату на двух листах." }, TablesRow.RowsType.Result),
                }
            },
            #endregion
        };
        #endregion

        #region Таблицы с данными измерений датчиков для "Протокола калибровки"
        public ProtocolTableLook[] ProtokolSensorTablesCollection = new ProtocolTableLook[]
        {
            /*#region ДП12
            new ProtocolTableLook
            {
                TableWidth = 9781,
                ColumnsWidth = new int[] { 2268, 2269, 708, 1169, 106, 448, 262, 777, 1207, 426 },
                DeviceTypes = new string[] {"ДП12"},
                TableName = "Определение погрешности измерения линейных перемещений ДП12 №",
                ParentTableName = "Результаты приемосдаточных испытаний:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,

                RandomMeasuredValue = new double[]
                {
                    0.5,
                    0.5,
                    0.5
                },
                RandomMotionStartValue = new double[]
                {
                    Math.Round((double)RandomValue.Next(45000, 55000) / 100d, 2),
                    Math.Round((double)RandomValue.Next(24000, 30000) / 100d, 2),
                    Math.Round((double)RandomValue.Next(12000, 15000) / 100d, 2),
                    Math.Round((double)RandomValue.Next(18000, 25000) / 100d, 2)
                },

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерения линейных перемещений ДП12 № ", "Variables[0]" }, new int[] { 7, 3 }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Variables[3]", "" }, new int[] { 7, 3 }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки*", "Показания штангенрейсмаса, мм", "Действительный интервал, мм", "Показания прибора, мм", "Погрешность, мм" }, new int[] { 1, 1, 2, 4, 2 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Начало перемещений", "–", "ХХХ", "0,0", "ХХХ" }, new int[] { 1, 1, 2, 4, 2 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "RandomMotionStartValue[0]", "RandomIndication[0]", "RandomActualInterval[0]", "RandomMeasuredValue[0]", "±RandomError[0]" }, new int[] { 1, 1, 2, 4, 2 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "RandomMotionStartValue[1]", "RandomIndication[1]", "RandomActualInterval[1]", "RandomMeasuredValue[1]", "±RandomError[1]" }, new int[] { 1, 1, 2, 4, 2 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "RandomMotionStartValue[2]", "RandomIndication[2]", "RandomActualInterval[2]", "RandomMeasuredValue[2]", "±RandomError[2]" }, new int[] { 1, 1, 2, 4, 2 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "*Точки калибровки зависят от длины применяемого измерительного стержня" }, new int[] { 10 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Проверка вариации датчика линейных перемещений" }, new int[] { 10 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "", "Нулевая точка, мм", "Точка смены показания, мм", "Разность, мм" }, new int[] { 1, 1, 4, 4 }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "Показания штангенрейсмаса, мм", "RandomMotionStartValue[3]", "Variables[1]", "2;1" }, new int[] { 1, 1, 4, 4 }, TablesRow.RowsType.Subtitle),
                    new TablesRow(new string[] { "Погрешность измерений линейных перемещений, мм", "±Variables[2]", "" }, new int[] { 3, 2, 5 }, TablesRow.RowsType.Result),
                }
            },
            #endregion

            #region ДП21
            new ProtocolTableLook
            {
                TableWidth = 9781,
                ColumnsWidth = new int[] { 2127, 2268, 2551, 142, 1134, 142, 1186, 89 },
                DeviceTypes = new string[] {"ДП21"},
                TableName = "Определение абсолютной погрешности измерений угловых перемещений ДП21 №",
                ParentTableName = "Результаты приемосдаточных испытаний:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,

                RandomMeasuredValue = new double[]
                {
                    Math.Round((double)RandomValue.Next(10000, 20000)/1000d, 2),
                    Math.Round((double)RandomValue.Next(25000, 35000)/1000d, 2),
                    Math.Round((double)RandomValue.Next(85000, 95000)/1000d, 2),
                    Math.Round((double)RandomValue.Next(175000, 185000)/1000d, 2),
                    Math.Round((double)RandomValue.Next(265000, 275000)/1000d, 2),
                    Math.Round((double)RandomValue.Next(355000, 365000)/1000d, 2)
                },

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение абсолютной погрешности измерений угловых перемещений ДП21 № ", "Variables[0]" }, new int[] {5, 3}, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка проверки", "Измеренное значение", "Действительное значение", "Погрешность" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0", "0", "0", "0" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "15", "RandomMeasuredValue[0]", "RandomActualValue[0]", "±RandomError[0]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "30", "RandomMeasuredValue[1]", "RandomActualValue[1]", "±RandomError[1]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "90", "RandomMeasuredValue[2]", "RandomActualValue[2]", "±RandomError[2]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "180", "RandomMeasuredValue[3]", "RandomActualValue[3]", "±RandomError[3]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "270", "RandomMeasuredValue[4]", "RandomActualValue[4]", "±RandomError[4]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "360", "RandomMeasuredValue[5]", "RandomActualValue[5]", "±RandomError[5]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Максимальная абсолютная  погрешность измерений угловых перемещений, град", "±Variables[2]", "" }, new int[] {5, 2, 1}, TablesRow.RowsType.Result),
                }
            },
            #endregion*/

            #region ДП12
            new ProtocolTableLook
            {
                TableWidth = 10065,

                ColumnsWidth = new int[] { 952, 1550, 1057, 2228, 65, 1676, 47, 1243, 547, 700 },
                DeviceTypes = new string[] {"ДП12"},
                TableName = "Определение основной абсолютной погрешности измерений линейных перемещений ДП12 №",
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,

                RandomMeasuredValue = new double[]
                {
                    Math.Round((double)RandomValue.Next(0, 500) / 10d, 1),
                    Math.Round((double)RandomValue.Next(0, 500) / 10d, 1),
                    Math.Round((double)RandomValue.Next(0, 500) / 10d, 1)
                },

                Rows = new List<TablesRow>()
                {
                    // Variables[0] - заводской номер прибора
                    // Variables[1] - вариация (определяется из родительского документа)
                    // Variables[2] - максимальная абсолютная погрешность (определяется из родительского документа)
                    // Variables[3] - описание измерительного стержня
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений линейных перемещений ДП12 № Variables[0]"}, new int[] { 10 }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Variables[3]" }, new int[] { 10 }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, мм", "Lизм, мм", "Lд, мм", "Δi, мм" }, new int[] { 3, 2, 3, 2 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1/3", "RandomMeasuredValue[0]", "RandomActualValue[0]", "RandomError[0]" }, new int[] { 3, 2, 3, 2 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "2/3", "RandomMeasuredValue[1]", "RandomActualValue[1]", "RandomError[1]" }, new int[] { 3, 2, 3, 2 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4/5", "RandomMeasuredValue[2]", "RandomActualValue[2]", "RandomError[2]" }, new int[] { 3, 2, 3, 2 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Вариация", "Variables[1]", "" }, new int[] { 1, 1, 8 }, TablesRow.RowsType.Result),
                    new TablesRow(new string[] { "Абсолютная погрешность измерений линейных перемещений, мм", "Variables[2]", "" }, new int[] { 6, 2, 2 }, TablesRow.RowsType.Result)
                }
            },
            #endregion

            #region ДП21
            new ProtocolTableLook
            {
                TableWidth = 9640,
                ColumnsWidth = new int[] { 2127, 2268, 2551, 142, 1134, 142, 1186, 89 },
                DeviceTypes = new string[] {"ДП21"},
                TableName = "Определение основной абсолютной погрешности измерений угловых перемещений ДП21 №",
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,

                RandomMeasuredValue = new double[]
                {
                    Math.Round((double)RandomValue.Next(1450, 1550)/100d, 2),
                    Math.Round((double)RandomValue.Next(2950, 3050)/100d, 2),
                    Math.Round((double)RandomValue.Next(5950, 6050)/100d, 2),
                    Math.Round((double)RandomValue.Next(8950, 9050)/100d, 2),
                    Math.Round((double)RandomValue.Next(11950, 12050)/100d, 2),
                    Math.Round((double)RandomValue.Next(17950, 18050)/100d, 2),
                    Math.Round((double)RandomValue.Next(23950, 24050)/100d, 2),
                    Math.Round((double)RandomValue.Next(29950, 30050)/100d, 2),
                    Math.Round((double)RandomValue.Next(35950, 36050)/100d, 2)
                },

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений угловых перемещений ДП21 № Variables[0]" }, new int[] {8}, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка поверки, град", "Измеренное значение, град", "Действительное значение, град", "Δi, град" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "15", "RandomMeasuredValue[0]", "RandomActualValue[0]", "RandomError[0]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "30", "RandomMeasuredValue[1]", "RandomActualValue[1]", "RandomError[1]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "60", "RandomMeasuredValue[2]", "RandomActualValue[2]", "RandomError[2]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "90", "RandomMeasuredValue[3]", "RandomActualValue[3]", "RandomError[3]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "120", "RandomMeasuredValue[4]", "RandomActualValue[4]", "RandomError[4]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "180", "RandomMeasuredValue[5]", "RandomActualValue[5]", "RandomError[5]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "240", "RandomMeasuredValue[6]", "RandomActualValue[6]", "RandomError[6]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "300", "RandomMeasuredValue[7]", "RandomActualValue[7]", "RandomError[7]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "360", "RandomMeasuredValue[8]", "RandomActualValue[8]", "RandomError[8]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Максимальная абсолютная погрешность измерений угловых перемещений, град:", "Variables[2]", "" }, new int[] {5, 2, 1}, TablesRow.RowsType.Result)
                }
            },
            #endregion

            #region ДП22
            new ProtocolTableLook
            {
                TableWidth = 9640,
                ColumnsWidth = new int[] { 2127, 2268, 2551, 142, 1134, 142, 1186, 89 },
                DeviceTypes = new string[] {"ДП22"},
                TableName = "Определение абсолютной погрешности измерений угловых перемещений ДП22 №",
                ParentTableName = "Результаты приемосдаточных испытаний:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,

                RandomMeasuredValue = new double[]
                {
                    Math.Round((double)RandomValue.Next(10000, 20000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(25000, 35000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(85000, 95000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(175000, 185000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(265000, 275000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(355000, 365000)/1000d, 3)
                },
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение абсолютной погрешности измерений угловых перемещений ДП22 № Variables[0]" }, new int[] {8}, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка проверки", "Измеренное значение", "Действительное значение", "Погрешность" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0", "0", "0", "0" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "15", "RandomMeasuredValue[0]", "RandomActualValue[0]", "RandomError[0]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "30", "RandomMeasuredValue[1]", "RandomActualValue[1]", "RandomError[1]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "90", "RandomMeasuredValue[2]", "RandomActualValue[2]", "RandomError[2]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "180", "RandomMeasuredValue[3]", "RandomActualValue[3]", "RandomError[3]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "270", "RandomMeasuredValue[4]", "RandomActualValue[4]", "RandomError[4]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "360", "RandomMeasuredValue[5]", "RandomActualValue[5]", "RandomError[5]" }, new int[] {1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Максимальная абсолютная  погрешность измерений угловых перемещений, град", "Variables[2]", "" }, new int[] {5, 2, 1}, TablesRow.RowsType.Result),
                }
            }
            #endregion
        };
        #endregion

        #region Таблицы с данными измерений приборов для "Протокола калибровки"
        //public CertificateTableLook[] ProtokolDeviceTablesCollection = new CertificateTableLook[]
        public ProtocolTableLook[] ProtokolDeviceTablesCollection = new ProtocolTableLook[]
        {
            #region МИКО-2
            // МИКО-2, таблица 1
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.2", "МИКО-2.3"},
                TableName = "Режим «Микроомметр»",
                ColumnsCount = 4,
                ParentTableName = "Режим микроомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Микроомметр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Измеренное значение", "Действительное значение", "Относительная погрешность" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0,1 мОм", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 мОм", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 мОм", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 мОм", "6;1", "6;2", "6;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // МИКО-2, таблица 2
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.2", "МИКО-2.3"},
                TableName = "Режим «Миллиомметр»",
                ColumnsCount = 4,
                ParentTableName = "Режим миллиомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Миллиомметр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Измеренное значение", "Действительное значение", "Относительная погрешность" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0,1 мОм", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 мОм", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 мОм", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 мОм", "6;1", "6;2", "6;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 Ом", "7;1", "7;2", "7;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 Ом", "8;1", "8;2", "8;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 Ом", "9;1", "9;2", "9;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000 Ом", "10;1", "10;2", "10;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            // МИКО-2, таблица 3
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.2", "МИКО-2.3"},
                TableName = "Режим «Килоомметр»",
                ColumnsCount = 4,
                ParentTableName = "Режим килоомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Килоомметр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Измеренное значение", "Действительное значение", "Относительная погрешность" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "100 Ом", "2;1", "2;2", "2;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 кОм", "3;1", "3;2", "3;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 кОм", "4;1", "4;2", "4;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 кОм", "5;1", "5;2", "5;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "200 кОм", "6;1", "6;2", "6;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "300 кОм", "7;1", "7;2", "7;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-7
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-7"},
                TableName = "Основная относительная погрешность измерения электрического сопротивления",
                ColumnsCount = 4,
                ParentTableName = "Режим миллиомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                RandomActualValue = new double[] {9.0909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(87000, 95000) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерения электрического сопротивления" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Действительное значение", "Измеренное значение", "Погрешность измерения, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0,00001 Ом", "RandomMeasuredValue[0] мкОм", "RandomActualValue[0] мкОм", "RandomError[0]"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0,0001 Ом", "3;2", "3;1", "3;1;%;3;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0,001 Ом", "4;2", "4;1", "4;1;%;4;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0,01 Ом", "5;2", "5;1", "5;1;%;5;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0,1 Ом", "6;2", "6;1", "6;1;%;6;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 Ом", "7;2", "7;1", "7;1;%;7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 Ом", "8;2", "8;1", "8;1;%;8;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 Ом", "9;2", "9;1", "9;1;%;9;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000 Ом", "10;2", "10;1", "10;1;%;10;2;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-7М, МИКО-7МА
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-7М", "МИКО-7МА"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,

                RandomActualValue = new double[] {9.0909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(87409, 94409) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка калибровки, Ом", "Ток, А", "Измеренное значение", "Действительное значение", "Фактическая погрешность измерений, %", "Пределы допускаемой относительной погрешности, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0.00001", "10", "RandomMeasuredValue[0] мкОм", "RandomActualValue[0] мкОм", "RandomError[0]", "±3.85" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001", "10", "0;1", "0;0", "0;1;%;0;0;.;3", "±0.47" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001", "10", "1;1", "1;0", "1;1;%;1;0;.;3", "±0.137" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01", "10", "2;1", "2;0", "2;1;%;2;0;.;3", "±0.104" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1", "1", "3;1", "3;0", "3;1;%;3;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1", "1", "4;1", "4;0", "4;1;%;4;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10", "0.1", "5;1", "5;0", "5;1;%;5;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100", "0.1", "6;1", "6;0", "6;1;%;6;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "0.01", "7;1", "7;0", "7;1;%;7;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-8
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-8"},
                TableName = "Основная относительная погрешность измерения электрического сопротивления",
                ColumnsCount = 4,
                ParentTableName = "Режим миллиомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,

                RandomActualValue = new double[] {9.0909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(86365, 95453) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерения электрического сопротивления" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки", "Действительное значение", "Измеренное значение", "Погрешность измерения, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "10 мкОм", "RandomActualValue[0] мкОм", "RandomMeasuredValue[0] мкОм", "RandomError[0]" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 мкОм", "3;2", "3;1", "3;1;%;3;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0,001 Ом", "4;2", "4;1", "4;1;%;4;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0,01 Ом", "5;2", "5;1", "5;1;%;5;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0,1 Ом", "6;2", "6;1", "6;1;%;6;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 Ом", "7;2", "7;1", "7;1;%;7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 Ом", "8;2", "8;1", "8;1;%;8;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 Ом", "9;2", "9;1", "9;1;%;9;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000 Ом", "10;2", "10;1", "10;1;%;10;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000 Ом", "11;2", "11;1", "11;1;%;11;2;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-8М, МИКО-8МА
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-8М", "МИКО-8МА"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,

                RandomActualValue = new double[] {9.0909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(87409, 94409) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка калибровки, Ом", "Ток, А", "Измеренное значение", "Действительное значение", "Фактическая погрешность измерений, %", "Пределы допускаемой относительной погрешности, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0.00001", "10", "RandomMeasuredValue[0] мкОм", "RandomActualValue[0] мкОм", "RandomError[0]", "±3.85" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001", "10", "0;1", "0;0", "0;1;%;0;0;.;3", "±0.47" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001", "10", "1;1", "1;0", "1;1;%;1;0;.;3", "±0.137" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01", "10", "2;1", "2;0", "2;1;%;2;0;.;3", "±0.104" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1", "1", "3;1", "3;0", "3;1;%;3;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1", "1", "4;1", "4;0", "4;1;%;4;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10", "0.1", "5;1", "5;0", "5;1;%;5;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100", "0.1", "6;1", "6;0", "6;1;%;6;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "0.01", "7;1", "7;0", "7;1;%;7;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000", "0.001", "8;1", "8;0", "8;1;%;8;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-9, МИКО-9А
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-9", "МИКО-9А"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,

                RandomActualValue = new double[] {9.0909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(87409, 94409) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка калибровки, Ом", "Ток, А", "Измеренное значение", "Действительное значение", "Фактическая погрешность измерений, %", "Пределы допускаемой относительной погрешности, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0.00001", "10", "RandomMeasuredValue[0] мкОм", "RandomActualValue[0] мкОм", "RandomError[0]", "±3.85" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001", "10", "1;1", "1;0", "1;1;%;1;0;.;3", "±0.47" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001", "10", "2;1", "2;0", "2;1;%;2;0;.;3", "±0.137" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01", "10", "4;1", "4;0", "4;1;%;4;0;.;3", "±0.104" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1", "1", "6;1", "6;0", "6;1;%;6;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1", "1", "8;1", "8;0", "8;1;%;8;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10", "0.1", "10;1", "10;0", "10;1;%;10;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100", "0.1", "12;1", "12;0", "12;1;%;12;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "0.01", "15;1", "15;0", "15;1;%;15;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000", "0.001", "16;1", "16;0", "16;1;%;16;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "30000", "0.0005", "18;1", "18;0", "18;1;%;18;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-10
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-10"},
                TableName = "Определение погрешности измерения сопротивления в режиме «ОДНОКРАТНЫЙ»",
                ColumnsCount = 6,
                ParentTableName = "Режим микроомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                RandomActualValue = new double[] {0.9909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(5886, 13932) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерения сопротивления в режиме «ОДНОКРАТНЫЙ»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, Ом", "Фактическое значение меры", "Сила тока, А", "Измеренное значение сопротивления", "Погрешность измерения фактического значения сопротивления, %", "Пределы допускаемой относительной погрешности измерений, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "10SuperScript[-6]", "RandomActualValue[0] мкОм", "10", "RandomMeasuredValue[0] мкОм", "RandomError[0]", "±40.6" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10SuperScript[-4]", "4;2", "10", "4;1", "4;1;%;4;2;.;3", "±2.2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10SuperScript[-3]", "5;2", "10", "5;1", "5;1;%;5;2;.;3", "±0.6" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10SuperScript[-2]", "6;2", "10", "6;1", "6;1;%;6;2;.;3", "±0.2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10SuperScript[-1]", "7;2", "1", "7;1", "7;1;%;7;2;.;3", "±0.2" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region МИКО-21
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-21"},
                TableName = "Основная относительная погрешность измерения электрического сопротивления",
                ColumnsCount = 5,
                ParentTableName = "Режим микроомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(19980, 20020) / 10000d,
                                                    (double)RandomValue.Next(90605, 91213) / 10000d,
                                                    (double)RandomValue.Next(9747, 10055) / 10000d,
                                                    (double)RandomValue.Next(914, 1084) / 10000d},
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Основная относительная погрешность измерения электрического сопротивления" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Установленное значение сопротивления", "Сила тока, А", "Измеренное значение", "Допустимое значение"}, new int[] {1, 1, 1, 2 }, new int[] {1, 1, 1, 0}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "", "", "", "Верхняя граница", "Нижняя граница"}, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 2, 0, 0}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2 Ом", "1", "RandomMeasuredValue[0] Ом", "2.0020 Ом", "1.9980 Ом"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 Ом", "1", "7;1", "1.0011 Ом",  "998.90 мОм"},  TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1 Ом", "10", "6;1", "100.10 мОм", "99.899 мОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01 Ом", "100", "5;1", "10.005 мОм", "9.9950 мОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001 Ом", "200", "4;1", "1.0006 мОм", "999.45 мкОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001 Ом", "200", "3;1", "100.10 мкОм", "99.904 мкОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9.0909 мкОм", "200", "RandomMeasuredValue[1] мкОм", "9.1213 мкОм", "9.0605 мкОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.9901 мкОм", "200", "RandomMeasuredValue[2] мкОм", "1.0055 мкОм", "0.9747 мкОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0999 мкОм", "200", "RandomMeasuredValue[3] мкОм", "0.1084 мкОм", "0.0914 мкОм"}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/М7
            // ПКВ/М7, таблица 1
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом НАПРЯЖЕНИЕ КОММУТАТОРА",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Напряжение коммутатора\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом НАПРЯЖЕНИЕ КОММУТАТОРА" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3",          "7;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3",          "7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3",    "5;2;-;5;0;.;3",    "6;2;-;6;0;.;3",    "7;2;-;7;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 2
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока  в диапазоне ±1 В каналом ВХОД АНАЛОГОВЫЙ",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Ан. Вход\" в режиме токовых клещей",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока в диапазоне ±1 В каналом ВХОД АНАЛОГОВЫЙ" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3",          "7;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3",          "7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3",    "5;2;-;5;0;.;3",    "6;2;-;6;0;.;3",    "7;2;-;7;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 3
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Униполярный режим",
                ColumnsCount = 5,
                ParentTableName = "Напряжение, канал \"Ан. Вход\", униполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Униполярный режим" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 4
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Биполярный режим",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Ан. Вход\", биполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Биполярный режим" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3",          "7;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3",          "7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3",    "5;2;-;5;0;.;3",    "6;2;-;6;0;.;3",    "7;2;-;7;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 5
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 2500 Ом",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"Ан. Вход\" 4мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 2500 Ом" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, Ом", "1;0;.;2",          "2;0;.;2",          "3;0;.;2",          "4;0;.;2",          "5;0;.;2" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Rизм, Ом",             "1;2;.;2",          "2;2;.;2",          "3;2;.;2",          "4;2;.;2",          "5;2;.;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, Ом",                "1;2;-;1;0;.;2",    "2;2;-;2;0;.;2",    "3;2;-;3;0;.;2",    "4;2;-;4;0;.;2",    "5;2;-;5;0;.;2" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 6
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 160 Ом",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"Ан. Вход\" 60мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 160 Ом" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, Ом", "1;0;.;2",          "2;0;.;2",          "3;0;.;2",          "4;0;.;2",          "5;0;.;2" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Rизм, Ом",             "1;2;.;2",          "2;2;.;2",          "3;2;.;2",          "4;2;.;2",          "5;2;.;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, Ом",                "1;2;-;1;0;.;2",    "2;2;-;2;0;.;2",    "3;2;-;3;0;.;2",    "4;2;-;4;0;.;2",    "5;2;-;5;0;.;2" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У3

            #region ПКВ/У3, таблица 1, Напряжение коммутатора
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом \"Входное напряжение коммутатора\"",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Напряжение коммутатора\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом \"Входное напряжение коммутатора\"" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3",          "7;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3",          "7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3",    "5;2;-;5;0;.;3",    "6;2;-;6;0;.;3",    "7;2;-;7;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У3, таблица 3, Униполярный режим
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в униполярном режиме",
                ColumnsCount = 5,
                ParentTableName = "Напряжение, канал \"Вход 1\", униполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в униполярном режиме" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "2;0;.;3",          "3;0;.;3",          "4;0;.;3" },    new int[] {2, 1, 1, 1}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "«Вход 1»", "Uизм, В", "2;2;.;3",          "3;2;.;3",          "4;2;.;3" },      new int[] {1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3" },      new int[] {1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 3, Униполярный режим
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в униполярном режиме",
                ColumnsCount = 5,
                ParentTableName = "Напряжение, канал \"Вход 2\", униполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "«Вход 2»", "Uизм, В",             "2;2;.;3",          "3;2;.;3",          "4;2;.;3" },          new int[] {1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",                "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3" },    new int[] {1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У3, таблица 4, Биполярный режим
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в биполярном режиме",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Вход 1\", биполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в биполярном режиме" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В",         "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3" }, new int[] {2, 1, 1, 1, 1, 1, 1 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "«Вход 1»", "Uизм, В",           "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" },  new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",              "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" },  new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 4, Биполярный режим
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в биполярном режиме",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Вход 2\", биполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "«Вход 2»", "Uизм, В",   "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",      "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У3, таблица 5, канал Uшунта
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналом «Uшунта»",
                ColumnsCount = 7,
                ParentTableName = "Напряжение, канал Uшунта",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналом «Uшунта»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "δ, %",                "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion
            
            #region ПКВ/У3, таблица 2, Токовые клещи
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналом «Токовые клещи»",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал токовые клещи, \"Вкл.\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналом «Токовые клещи»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3" }, new int[] {2, 1, 1, 1, 1, 1, 1 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Канал «ВКЛ»", "Uизм, В",      "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",         "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/У3, таблица 2, Токовые клещи
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналом «Токовые клещи»",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал токовые клещи, \"Откл.\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Канал «ОТКЛ»", "Uизм, В",     "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",        "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У3, таблица 5, при силе измерительного тока 4 мА
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 4 мА",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"Вход 1\" 4мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 4 мА" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, Ом",  "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3" }, new int[] {2, 1, 1, 1, 1 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "«Вход 1»", "Rизм, Ом",    "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",       "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 5, при силе измерительного тока 4 мА
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 4 мА",
                ColumnsCount = 7,
                ParentTableName = "Сопротивление, канал \"Вход 2\" 4мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "«Вход 2»", "Rизм, Ом",             "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",                "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion
            
            #region ПКВ/У3, таблица 6, при силе измерительного тока 60 мА
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 60 мА",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"Вход 1\" 60мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 60 мА" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, Ом",  "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3" }, new int[] {2, 1, 1, 1, 1 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "«Вход 1»", "Rизм, Ом",    "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",       "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },
            
            // ПКВ/У3, таблица 6, при силе измерительного тока 60 мА
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 60 мА",
                ColumnsCount = 7,
                ParentTableName = "Сопротивление, канал \"Вход 2\" 60мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "«Вход 2»", "Rизм, Ом",             "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",                "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion
            
            #region ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД1\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки калибровки, Ом", "40", "80", "120", "160"}, new int[] {2, 1,1,1,1}, TablesRow.RowsType.Header),
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД1\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД1", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД2\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД2", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД3\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД3", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД4\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД4", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД5\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД5", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД6\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД6", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД7\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД7", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД8\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД8", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД9\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД9", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new ProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД10\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД9", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion
            #endregion
        };
        #endregion

        #region Таблицы с данными измерений датчиков для "Протокола поверки"
        public VerificationProtocolTableLook[] VerificationProtokolSensorTablesCollection = new VerificationProtocolTableLook[]
        {
            #region ДП12
            new VerificationProtocolTableLook
            {
                TableWidth = 10065,

                ColumnsWidth = new int[] { 952, 1550, 1057, 2228, 65, 1676, 47, 1243, 1247 },
                DeviceTypes = new string[] {"ДП12"},
                TableName = "Определение основной абсолютной погрешности измерений линейных перемещений ДП12 №",
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,

                RandomMeasuredValue = new double[]
                {
                    Math.Round((double)RandomValue.Next(0, 500) / 10d, 1),
                    Math.Round((double)RandomValue.Next(0, 500) / 10d, 1),
                    Math.Round((double)RandomValue.Next(0, 500) / 10d, 1)
                },

                Rows = new List<TablesRow>()
                {
                    // Variables[0] - заводской номер прибора
                    // Variables[1] - вариация (определяется из родительского документа)
                    // Variables[2] - максимальная абсолютная погрешность (определяется из родительского документа)
                    // Variables[3] - описание измерительного стержня
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений линейных перемещений ДП12 № Variables[0]" }, new int[] { 9 }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Variables[3]" }, new int[] { 9 }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, мм", "Lизм, мм", "Lд, мм", "Δi, мм" }, new int[] { 3, 2, 3, 1 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1/3", "RandomMeasuredValue[0]", "RandomActualValue[0]", "RandomError[0]" }, new int[] { 3, 2, 3, 1 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "2/3", "RandomMeasuredValue[1]", "RandomActualValue[1]", "RandomError[1]" }, new int[] { 3, 2, 3, 1 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "4/5", "RandomMeasuredValue[2]", "RandomActualValue[2]", "RandomError[2]" }, new int[] { 3, 2, 3, 1 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Вариация", "Variables[1]", "" }, new int[] { 1, 1, 7 }, TablesRow.RowsType.Result),
                    new TablesRow(new string[] { "Абсолютная погрешность измерений линейных перемещений, мм", "Variables[2]", "" }, new int[] { 5, 2, 2 }, TablesRow.RowsType.Result)
                }
            },
            #endregion

            #region ДП21
            new VerificationProtocolTableLook
            {
                TableWidth = 10065,
                ColumnsWidth = new int[] { 2156, 2713, 2122, 364, 1351, 1327 },
                DeviceTypes = new string[] {"ДП21"},
                TableName = "Определение основной абсолютной погрешности измерений угловых перемещений ДП21 №",
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,

                RandomMeasuredValue = new double[]
                {
                    Math.Round((double)RandomValue.Next(1450, 1550)/100d, 2),
                    Math.Round((double)RandomValue.Next(2950, 3050)/100d, 2),
                    Math.Round((double)RandomValue.Next(5950, 6050)/100d, 2),
                    Math.Round((double)RandomValue.Next(8950, 9050)/100d, 2),
                    Math.Round((double)RandomValue.Next(11950, 12050)/100d, 2),
                    Math.Round((double)RandomValue.Next(17950, 18050)/100d, 2),
                    Math.Round((double)RandomValue.Next(23950, 24050)/100d, 2),
                    Math.Round((double)RandomValue.Next(29950, 30050)/100d, 2),
                    Math.Round((double)RandomValue.Next(35750, 35950)/100d, 2)
                },

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений угловых перемещений ДП21 № Variables[0]" }, new int[] {6}, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка поверки, град", "Измеренное значение, град", "Действительное значение, град", "Δi, град" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "15", "RandomMeasuredValue[0]", "RandomActualValue[0]", "RandomError[0]" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "30", "RandomMeasuredValue[1]", "RandomActualValue[1]", "RandomError[1]" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "60", "RandomMeasuredValue[2]", "RandomActualValue[2]", "RandomError[2]" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "90", "RandomMeasuredValue[3]", "RandomActualValue[3]", "RandomError[3]" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "120", "RandomMeasuredValue[4]", "RandomActualValue[4]", "RandomError[4]" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "180", "RandomMeasuredValue[5]", "RandomActualValue[5]", "RandomError[5]" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "240", "RandomMeasuredValue[6]", "RandomActualValue[6]", "RandomError[6]" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "300", "RandomMeasuredValue[7]", "RandomActualValue[7]", "RandomError[7]" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "360", "RandomMeasuredValue[8]", "RandomActualValue[8]", "RandomError[8]" }, new int[] {1, 1, 2, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Максимальная абсолютная погрешность измерений угловых перемещений, град:", "Variables[2]", "" }, new int[] {4, 1, 1}, TablesRow.RowsType.Result)
                }
            },
            #endregion

            #region ДП22
            new VerificationProtocolTableLook
            {
                TableWidth = 9640,
                ColumnsWidth = new int[] { 1736, 1736, 1736, 1738, 142, 1134, 142, 1186, 89 },
                DeviceTypes = new string[] {"ДП22"},
                TableName = "Определение абсолютной погрешности измерений угловых перемещений ДП22 №",
                ParentTableName = "Результаты приемосдаточных испытаний:",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.AcceptanceTestingProtocol,

                RandomMeasuredValue = new double[]
                {
                    Math.Round((double)RandomValue.Next(15000, 25000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(35000, 45000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(55000, 65000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(75000, 85000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(95000, 105000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(115000, 125000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(135000, 145000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(155000, 165000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(175000, 185000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(195000, 205000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(215000, 225000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(235000, 245000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(255000, 265000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(275000, 285000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(295000, 305000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(315000, 325000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(335000, 345000)/1000d, 3),
                    Math.Round((double)RandomValue.Next(355000, 365000)/1000d, 3)
                },
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение абсолютной погрешности измерений угловых перемещений ДП22 № Variables[0]" }, new int[] {9}, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка проверки", "Измеренное значение", "Действительное значение", "Погрешность", "Предел допускаемой абсолютной погрешности" }, new int[] {1, 1, 1, 1, 5}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "20", "RandomMeasuredValue[0]", "RandomActualValue[0]", "RandomError[0]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "40", "RandomMeasuredValue[1]", "RandomActualValue[1]", "RandomError[1]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "60", "RandomMeasuredValue[2]", "RandomActualValue[2]", "RandomError[2]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "80", "RandomMeasuredValue[3]", "RandomActualValue[3]", "RandomError[3]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100", "RandomMeasuredValue[4]", "RandomActualValue[4]", "RandomError[4]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "120", "RandomMeasuredValue[5]", "RandomActualValue[5]", "RandomError[5]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "140", "RandomMeasuredValue[6]", "RandomActualValue[6]", "RandomError[6]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "160", "RandomMeasuredValue[7]", "RandomActualValue[7]", "RandomError[7]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "180", "RandomMeasuredValue[8]", "RandomActualValue[8]", "RandomError[8]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "200", "RandomMeasuredValue[9]", "RandomActualValue[9]", "RandomError[9]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "220", "RandomMeasuredValue[10]", "RandomActualValue[10]", "RandomError[10]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "240", "RandomMeasuredValue[11]", "RandomActualValue[11]", "RandomError[11]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "260", "RandomMeasuredValue[12]", "RandomActualValue[12]", "RandomError[12]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "280", "RandomMeasuredValue[13]", "RandomActualValue[13]", "RandomError[13]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "300", "RandomMeasuredValue[14]", "RandomActualValue[14]", "RandomError[14]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "320", "RandomMeasuredValue[15]", "RandomActualValue[15]", "RandomError[15]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "340", "RandomMeasuredValue[16]", "RandomActualValue[16]", "RandomError[16]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "360", "RandomMeasuredValue[17]", "RandomActualValue[17]", "RandomError[17]", "Variables[4]" }, new int[] {1, 1, 1, 1, 5}, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Максимальная абсолютная  погрешность измерений угловых перемещений, град", "Variables[2]", "" }, new int[] {6, 2, 1}, TablesRow.RowsType.Result),
                }
            }
            #endregion
        };
        #endregion

        #region Таблицы с данными измерений приборов для "Протокола поверки"
        public VerificationProtocolTableLook[] VerifyProtokolDeviceTablesCollection = new VerificationProtocolTableLook[]
        {
        #region МИКО-2.3
            // МИКО-2.3, таблица 1
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.3"},
                TableName = "Режим «Микроомметр»",
                ColumnsCount = 5,
                ParentTableName = "Режим микроомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Микроомметр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка поверки", "Измеренное значение", "Действительное значение", "Фактическая погрешность измерений", "Пределы допускаемой относительной погрешности" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0,1 мОм", "3;1", "3;2", "3;3", "±0.2%" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 мОм", "4;1", "4;2", "4;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 мОм", "5;1", "5;2", "5;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 мОм", "6;1", "6;2", "6;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow)
                }
            },
            // МИКО-2.3, таблица 2
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.3"},
                TableName = "Режим «Миллиомметр»",
                ColumnsCount = 5,
                ParentTableName = "Режим миллиомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Миллиомметр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка поверки", "Измеренное значение", "Действительное значение", "Фактическая погрешность измерений", "Пределы допускаемой относительной погрешности" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0,1 мОм", "3;1", "3;2", "3;3", "±2%" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 мОм", "4;1", "4;2", "4;3", "±0.2%" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 мОм", "5;1", "5;2", "5;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 мОм", "6;1", "6;2", "6;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 Ом", "7;1", "7;2", "7;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 Ом", "8;1", "8;2", "8;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 Ом", "9;1", "9;2", "9;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000 Ом", "10;1", "10;2", "10;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow)
                }
            },
            // МИКО-2.3, таблица 3
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-2.3"},
                TableName = "Режим «Килоомметр»",
                ColumnsCount = 5,
                ParentTableName = "Режим килоомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Режим «Килоомметр»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка поверки", "Измеренное значение", "Действительное значение", "Фактическая погрешность измерений", "Пределы допускаемой относительной погрешности" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "100 Ом", "2;1", "2;2", "2;3", "±0.5%" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 кОм", "3;1", "3;2", "3;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10 кОм", "4;1", "4;2", "4;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100 кОм", "5;1", "5;2", "5;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "200 кОм", "6;1", "6;2", "6;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "300 кОм", "7;1", "7;2", "7;3", "" }, new int[] {1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

        #region МИКО-7
        // МИКО-7, таблица 1
        new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-7"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "Режим миллиомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                RandomActualValue = new double[] {9.0909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(87000, 95000) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, Ом", "Предел выходной", "Действительное значение", "Измеренное значение", "Погрешность, %", "Допускаемое значение погрешности СИ, %"}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0.00001", "62 Вт",    "RandomActualValue[0] мкОм",    "RandomMeasuredValue[0] мкОм",  "RandomError[0]",       "±5"    }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 1, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001",  "", "3;2", "3;1",                          "3;1;%;3;2;.;3",        "±0.5"  }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 2, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001",   "", "4;2", "4;1",                          "4;1;%;4;2;.;3",        "±0.1"  }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 2, 0, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01",    "", "5;2", "5;1",                          "5;1;%;5;2;.;3",        ""      }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 2, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1",     "0,3 Вт", "6;2", "6;1",                    "6;1;%;6;2;.;3",        ""      }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 1, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1.0",     "", "7;2", "7;1",                          "7;1;%;7;2;.;3",        ""      }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 2, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10",      "", "8;2", "8;1",                          "8;1;%;8;2;.;3",        ""      }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 2, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100",     "", "9;2", "9;1",                          "9;1;%;9;2;.;3",        ""      }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 2, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000",    "", "10;2", "10;1",                        "10;1;%;10;2;.;3",      ""      }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 2, 0, 0, 0, 2}, TablesRow.RowsType.SimpleRow)
                }
            },
        #endregion

        #region МИКО-7М, МИКО-7МА
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-7М", "МИКО-7МА"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,

                RandomActualValue = new double[] {9.0909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(87409, 94409) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка поверки, Ом", "Ток, А", "Измеренное значение", "Действительное значение", "Фактическая погрешность измерений, %", "Пределы допускаемой относительной погрешности, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0.00001", "10", "RandomMeasuredValue[0] мкОм", "RandomActualValue[0] мкОм", "RandomError[0]", "±3.85" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001", "10", "0;1", "0;0", "0;1;%;0;0;.;3", "±0.47" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001", "10", "1;1", "1;0", "1;1;%;1;0;.;3", "±0.137" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01", "10", "2;1", "2;0", "2;1;%;2;0;.;3", "±0.104" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1", "1", "3;1", "3;0", "3;1;%;3;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1", "1", "4;1", "4;0", "4;1;%;4;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10", "0.1", "5;1", "5;0", "5;1;%;5;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100", "0.1", "6;1", "6;0", "6;1;%;6;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "0.01", "7;1", "7;0", "7;1;%;7;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

        #region МИКО-8
        // МИКО-8
        new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-8"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 5,
                ParentTableName = "Режим миллиомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                RandomActualValue = new double[] {9.0909,
                                                  9.0909,
                                                  9.0909,
                                                  100.0016,
                                                  100.0016,
                                                  1000.071,
                                                  1000.071,
                                                  10.00026,
                                                  10.00026,
                                                  100.0167,
                                                  100.0167,
                                                  999.910,
                                                  999.910,
                                                  9.99983,
                                                  9.99983,
                                                  100.0035,
                                                  100.0035,
                                                  1000.036,
                                                  1000.036,
                                                  10000,
                                                  10000},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(86365, 95453) / 10000d,
                                                    (double)RandomValue.Next(86365, 95453) / 10000d,
                                                    (double)RandomValue.Next(86365, 95453) / 10000d,
                                                    (double)RandomValue.Next(995017, 1005016) / 10000d,
                                                    (double)RandomValue.Next(995017, 1005016) / 10000d,
                                                    (double)RandomValue.Next(9990710, 10010710) / 10000d,
                                                    (double)RandomValue.Next(9990710, 10010710) / 10000d,
                                                    (double)RandomValue.Next(999026, 1001026) / 100000d,
                                                    (double)RandomValue.Next(999026, 1001026) / 100000d,
                                                    (double)RandomValue.Next(999167, 1001167) / 10000d,
                                                    (double)RandomValue.Next(999167, 1001167) / 10000d,
                                                    (double)RandomValue.Next(9989101, 10009099) / 10000d,
                                                    (double)RandomValue.Next(9989101, 10009099) / 10000d,
                                                    (double)RandomValue.Next(998984, 1000983) / 100000d,
                                                    (double)RandomValue.Next(998984, 1000983) / 100000d,
                                                    (double)RandomValue.Next(999035, 1001035) / 10000d,
                                                    (double)RandomValue.Next(999035, 1001035) / 10000d,
                                                    (double)RandomValue.Next(999036, 1001036) / 1000d,
                                                    (double)RandomValue.Next(999036, 1001036) / 1000d,
                                                    (double)RandomValue.Next(99900, 100100) / 10d,
                                                    (double)RandomValue.Next(99900, 100100) / 10d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, Ом", "Действительное значение", "Измеренное значение", "Погрешность, %", "Допускаемое значение погрешности СИ, %"}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0.00001", "RandomActualValue[0] мкОм", "RandomMeasuredValue[0] мкОм",    "RandomError[0]",  "±5"    }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "RandomMeasuredValue[1] мкОм",    "RandomError[1]",  ""     }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "", "RandomMeasuredValue[2] мкОм",    "RandomError[2]",  ""     }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001",  "3;2",  "3;1", "3;1;%;3;2;.;3",        "±0.5"                   }, new int[] {1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",  "", "RandomMeasuredValue[3] мкОм", "RandomError[3]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",  "", "RandomMeasuredValue[4] мкОм", "RandomError[4]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001",   "4;2",  "4;1", "4;1;%;4;2;.;3",        "±0.1"                   }, new int[] {1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",   "", "RandomMeasuredValue[5] мкОм", "RandomError[5]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",   "", "RandomMeasuredValue[6] мкОм", "RandomError[6]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01",    "5;2", "5;1", "5;1;%;5;2;.;3",        "±0.1"                    }, new int[] {1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",    "", "RandomMeasuredValue[7] мОм", "RandomError[7]",        ""   }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",    "", "RandomMeasuredValue[8] мОм", "RandomError[8]",        ""   }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1",     "6;2", "6;1", "6;1;%;6;2;.;3",        "±0.1"                    }, new int[] {1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",     "", "RandomMeasuredValue[9] мОм", "RandomError[9]",        ""   }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",     "", "RandomMeasuredValue[10] мОм", "RandomError[10]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1.0",     "7;2", "7;1", "7;1;%;7;2;.;3",        "±0.1"                    }, new int[] {1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",     "", "RandomMeasuredValue[11] мОм", "RandomError[11]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",     "", "RandomMeasuredValue[12] мОм", "RandomError[12]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10",      "8;2", "8;1", "8;1;%;8;2;.;3",        "±0.1"                    }, new int[] {1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",      "", "RandomMeasuredValue[13] Ом", "RandomError[13]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",      "", "RandomMeasuredValue[14] Ом", "RandomError[14]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100",     "9;2", "9;1", "9;1;%;9;2;.;3",        "±0.1"                    }, new int[] {1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",     "", "RandomMeasuredValue[15] Ом", "RandomError[15]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",     "", "RandomMeasuredValue[16] Ом", "RandomError[16]",        ""  }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000",    "10;2", "10;1", "10;1;%;10;2;.;3",    "±0.1"                    }, new int[] {1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",    "", "RandomMeasuredValue[17] Ом", "RandomError[17]",    ""    }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",    "", "RandomMeasuredValue[18] Ом", "RandomError[18]",    ""    }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000",    "11;2", "11;1", "11;1;%;11;2;.;3",   "±0.1"                    }, new int[] {1, 1, 1, 1, 1 }, new int[] {1, 1, 0, 0, 1}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",    "", "RandomMeasuredValue[19] Ом", "RandomError[19]",   ""    }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "",    "", "RandomMeasuredValue[20] Ом", "RandomError[20]",   ""    }, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 0, 0, 2}, TablesRow.RowsType.SimpleRow)
                }
            },
        #endregion

        #region МИКО-8М, МИКО-8МА
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-8М", "МИКО-8МА"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,

                RandomActualValue = new double[] {9.0909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(87409, 94409) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка поверки, Ом", "Ток, А", "Измеренное значение", "Действительное значение", "Фактическая погрешность измерений, %", "Пределы допускаемой относительной погрешности, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0.00001", "10", "RandomMeasuredValue[0] мкОм", "RandomActualValue[0] мкОм", "RandomError[0]", "±3.85" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001", "10", "0;1", "0;0", "0;1;%;0;0;.;3", "±0.47" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001", "10", "1;1", "1;0", "1;1;%;1;0;.;3", "±0.137" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01", "10", "2;1", "2;0", "2;1;%;2;0;.;3", "±0.104" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1", "1", "3;1", "3;0", "3;1;%;3;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1", "1", "4;1", "4;0", "4;1;%;4;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10", "0.1", "5;1", "5;0", "5;1;%;5;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100", "0.1", "6;1", "6;0", "6;1;%;6;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "0.01", "7;1", "7;0", "7;1;%;7;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000", "0.001", "8;1", "8;0", "8;1;%;8;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

        #region МИКО-9, МИКО-9А
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-9", "МИКО-9А"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 6,
                ParentTableName = "",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,

                RandomActualValue = new double[] {9.0909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(87409, 94409) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точка поверки, Ом", "Ток, А", "Измеренное значение", "Действительное значение", "Фактическая погрешность измерений, %", "Пределы допускаемой относительной погрешности, %" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "0.00001", "10", "RandomMeasuredValue[0] мкОм", "RandomActualValue[0] мкОм", "RandomError[0]", "±3.85" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001", "10", "1;1", "1;0", "1;1;%;1;0;.;3", "±0.47" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001", "10", "2;1", "2;0", "2;1;%;2;0;.;3", "±0.137" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01", "10", "4;1", "4;0", "4;1;%;4;0;.;3", "±0.104" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1", "1", "6;1", "6;0", "6;1;%;6;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1", "1", "8;1", "8;0", "8;1;%;8;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10", "0.1", "10;1", "10;0", "10;1;%;10;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "100", "0.1", "12;1", "12;0", "12;1;%;12;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1000", "0.01", "15;1", "15;0", "15;1;%;15;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10000", "0.001", "16;1", "16;0", "16;1;%;16;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "30000", "0.0005", "18;1", "18;0", "18;1;%;18;0;.;3", "±0.1" }, TablesRow.RowsType.SimpleRow)
                }
            },
        #endregion

        #region МИКО-10
        // МИКО-10
        new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-10"},
                TableName = "Определение погрешности измерения сопротивления в режиме \"ОДНОКРАТНЫЙ\"",
                ColumnsCount = 6,
                ParentTableName = "Режим микроомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                RandomActualValue = new double[] {0.9909},
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(5886, 13932) / 10000d},

                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерения сопротивления в режиме \"ОДНОКРАТНЫЙ\"" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, Ом", "Фактическое значение меры", "Сила тока, А", "Измеренное значение сопротивления", "Погрешность измерения фактического значения сопротивления, %", "Пределы допускаемой относительной погрешности измерений, %"}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "10SuperScript[-6]", "RandomActualValue[0] мкОм", "10",    "RandomMeasuredValue[0] мкОм",  "RandomError[0]",       "±40.6"    }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10SuperScript[-4]",  "4;2", "10", "4;1", "4;1;%;4;2;.;3", "±2.2"  }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10SuperScript[-3]",  "5;2", "10", "5;1", "5;1;%;5;2;.;3", "±0.6"  }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10SuperScript[-2]",  "6;2", "10", "6;1", "6;1;%;6;2;.;3", "±0.2"  }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "10SuperScript[-1]",  "7;2", "1", "7;1", "7;1;%;7;2;.;3", "±0.2"   }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {0, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
        #endregion

        #region МИКО-21
        // МИКО-21
        new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"МИКО-21"},
                TableName = "Результаты проверки метрологических характеристик приведены в таблице:",
                ColumnsCount = 5,
                ParentTableName = "Режим микроомметра",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                RandomMeasuredValue = new double[] {(double)RandomValue.Next(19980, 20020) / 10000d,
                                                    (double)RandomValue.Next(90605, 91213) / 10000d,
                                                    (double)RandomValue.Next(9747, 10055) / 10000d,
                                                    (double)RandomValue.Next(914, 1084) / 10000d},
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Результаты проверки метрологических характеристик приведены в таблице:" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Установленное значение сопротивления", "Сила тока, А", "Измеренное значение", "Допустимое значение"}, new int[] {1, 1, 1, 2 }, new int[] {1, 1, 1, 0}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "", "", "", "Верхняя граница", "Нижняя граница"}, new int[] {1, 1, 1, 1, 1 }, new int[] {2, 2, 2, 0, 0}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "2 Ом", "1", "RandomMeasuredValue[0] Ом", "2.0020 Ом", "1.9980 Ом"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "1 Ом", "1", "7;1", "1.0011 Ом",  "998.90 мОм"},  TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.1 Ом", "10", "6;1", "100.10 мОм", "99.899 мОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.01 Ом", "100", "5;1", "10.005 мОм", "9.9950 мОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.001 Ом", "200", "4;1", "1.0006 мОм", "999.45 мкОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0001 Ом", "200", "3;1", "100.10 мкОм", "99.904 мкОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "9.0909 мкОм", "200", "RandomMeasuredValue[1] мкОм", "9.1213 мкОм", "9.0605 мкОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.9901 мкОм", "200", "RandomMeasuredValue[2] мкОм", "1.0055 мкОм", "0.9747 мкОм"}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "0.0999 мкОм", "200", "RandomMeasuredValue[3] мкОм", "0.1084 мкОм", "0.0914 мкОм"}, TablesRow.RowsType.SimpleRow)
                }
            },
        #endregion

        #region ПКВ/М7
            // ПКВ/М7, таблица 1
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом НАПРЯЖЕНИЕ КОММУТАТОРА",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Напряжение коммутатора\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом НАПРЯЖЕНИЕ КОММУТАТОРА" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3",          "7;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3",          "7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3",    "5;2;-;5;0;.;3",    "6;2;-;6;0;.;3",    "7;2;-;7;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 2
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока  в диапазоне ±1 В каналом ВХОД АНАЛОГОВЫЙ",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Ан. Вход\" в режиме токовых клещей",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока в диапазоне ±1 В каналом ВХОД АНАЛОГОВЫЙ" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3",          "7;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3",          "7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3",    "5;2;-;5;0;.;3",    "6;2;-;6;0;.;3",    "7;2;-;7;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 3
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Униполярный режим",
                ColumnsCount = 5,
                ParentTableName = "Напряжение, канал \"Ан. Вход\", униполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Униполярный режим" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 4
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Биполярный режим",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Ан. Вход\", биполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом ВХОД АНАЛОГОВЫЙ. Биполярный режим" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3",          "7;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3",          "7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3",    "5;2;-;5;0;.;3",    "6;2;-;6;0;.;3",    "7;2;-;7;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 5
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 2500 Ом",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"Ан. Вход\" 4мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 2500 Ом" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, Ом", "1;0;.;2",          "2;0;.;2",          "3;0;.;2",          "4;0;.;2",          "5;0;.;2" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Rизм, Ом",             "1;2;.;2",          "2;2;.;2",          "3;2;.;2",          "4;2;.;2",          "5;2;.;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, Ом",                "1;2;-;1;0;.;2",    "2;2;-;2;0;.;2",    "3;2;-;3;0;.;2",    "4;2;-;4;0;.;2",    "5;2;-;5;0;.;2" }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/М7, таблица 6
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/М7"},
                TableName = "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 160 Ом",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"Ан. Вход\" 60мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического сопротивления постоянному току в диапазоне от 0 до 160 Ом" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, Ом", "1;0;.;2",          "2;0;.;2",          "3;0;.;2",          "4;0;.;2",          "5;0;.;2" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Rизм, Ом",             "1;2;.;2",          "2;2;.;2",          "3;2;.;2",          "4;2;.;2",          "5;2;.;2" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, Ом",                "1;2;-;1;0;.;2",    "2;2;-;2;0;.;2",    "3;2;-;3;0;.;2",    "4;2;-;4;0;.;2",    "5;2;-;5;0;.;2" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

        #region ПКВ/У3

            #region ПКВ/У3, таблица 1, Напряжение коммутатора
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом \"Входное напряжение коммутатора\"",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Напряжение коммутатора\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение основной абсолютной погрешности измерений электрического напряжения постоянного тока каналом \"Входное напряжение коммутатора\"" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3",          "7;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3",          "7;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "Δ, В",                "1;2;-;1;0;.;3",    "2;2;-;2;0;.;3",    "3;2;-;3;0;.;3",    "4;2;-;4;0;.;3",    "5;2;-;5;0;.;3",    "6;2;-;6;0;.;3",    "7;2;-;7;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У3, таблица 3, Униполярный режим
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в униполярном режиме",
                ColumnsCount = 5,
                ParentTableName = "Напряжение, канал \"Вход 1\", униполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в униполярном режиме" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, В", "2;0;.;3",          "3;0;.;3",          "4;0;.;3" },    new int[] {2, 1, 1, 1}, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "«Вход 1»", "Uизм, В", "2;2;.;3",          "3;2;.;3",          "4;2;.;3" },      new int[] {1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3" },      new int[] {1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 3, Униполярный режим
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в униполярном режиме",
                ColumnsCount = 5,
                ParentTableName = "Напряжение, канал \"Вход 2\", униполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "«Вход 2»", "Uизм, В",             "2;2;.;3",          "3;2;.;3",          "4;2;.;3" },          new int[] {1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",                "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3" },    new int[] {1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У3, таблица 4, Биполярный режим
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в биполярном режиме",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Вход 1\", биполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в биполярном режиме" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, В",         "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3" }, new int[] {2, 1, 1, 1, 1, 1, 1 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "«Вход 1»", "Uизм, В",           "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" },  new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",              "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" },  new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 4, Биполярный режим
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналами «Вход 1», «Вход 2» в биполярном режиме",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал \"Вход 2\", биполярный режим",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "«Вход 2»", "Uизм, В",   "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",      "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У3, таблица 8, канал Uшунта
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналом «Uшунта»",
                ColumnsCount = 7,
                ParentTableName = "Напряжение, канал Uшунта",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналом «Uшунта»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3" }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Uизм, В",             "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "δ, %",                "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" }, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion
            
            #region ПКВ/У3, таблица 2, Токовые клещи
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналом «Токовые клещи»",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал токовые клещи, \"Вкл.\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического напряжения постоянного тока каналом «Токовые клещи»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, В", "1;0;.;3",          "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3",          "6;0;.;3" }, new int[] {2, 1, 1, 1, 1, 1, 1 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "Канал «ВКЛ»", "Uизм, В",      "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",         "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/У3, таблица 2, Токовые клещи
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического напряжения постоянного тока каналом «Токовые клещи»",
                ColumnsCount = 8,
                ParentTableName = "Напряжение, канал токовые клещи, \"Откл.\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Канал «ОТКЛ»", "Uизм, В",     "1;2;.;3",          "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3",          "6;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",        "1;2;%;1;0;.;3",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3",    "6;2;%;6;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion

            #region ПКВ/У3, таблица 5, при силе измерительного тока 4 мА
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 4 мА",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"Вход 1\" 4мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 4 мА" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, Ом",  "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3" }, new int[] {2, 1, 1, 1, 1 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "«Вход 1»", "Rизм, Ом",    "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",       "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 5, при силе измерительного тока 4 мА
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 4 мА",
                ColumnsCount = 7,
                ParentTableName = "Сопротивление, канал \"Вход 2\" 4мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "«Вход 2»", "Rизм, Ом",             "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",                "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion
            
            #region ПКВ/У3, таблица 6, при силе измерительного тока 60 мА
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 60 мА",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"Вход 1\" 60мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 60 мА" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, Ом",  "2;0;.;3",          "3;0;.;3",          "4;0;.;3",          "5;0;.;3" }, new int[] {2, 1, 1, 1, 1 }, TablesRow.RowsType.Header),
                    new TablesRow(new string[] { "«Вход 1»", "Rизм, Ом",    "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {1, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",       "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1 }, new int[] {2, 0, 0, 0, 0, 0 }, TablesRow.RowsType.SimpleRow)
                }
            },
            
            // ПКВ/У3, таблица 6, при силе измерительного тока 60 мА
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.1", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Вход 1», «Вход 2» при силе измерительного тока 60 мА",
                ColumnsCount = 7,
                ParentTableName = "Сопротивление, канал \"Вход 2\" 60мА",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "«Вход 2»", "Rизм, Ом",             "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",                "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion
            
            #region ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД1\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»" }, TablesRow.RowsType.Title),
                    new TablesRow(new string[] { "Точки поверки, Ом", "40", "80", "120", "160"}, new int[] {2, 1,1,1,1}, TablesRow.RowsType.Header),
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД1\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД1", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД2\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД2", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД3\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД3", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД4\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД4", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД5\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД5", "Rизм, Ом", "2;2;.;3",          "3;2;.;3",          "4;2;.;3",          "5;2;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %",    "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3"}, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД6\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД6", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД7\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД7", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД8\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД8", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД9\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД9", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },

            // ПКВ/У3, таблица 7, канал реостатных датчиков
            new VerificationProtocolTableLook
            {
                DeviceTypes = new string[] {"ПКВ/У3.0", "ПКВ/У3.0-01"},
                TableName = "Определение погрешности измерений электрического сопротивления постоянному току каналами «Реостатные датчики»",
                ColumnsCount = 6,
                ParentTableName = "Сопротивление, канал \"РД10\"",
                ParentDocumentCategory = CertificateTableLook.DocumentCategory.MeasuringData,
                Rows = new List<TablesRow>()
                {
                    new TablesRow(new string[] { "РД9", "Rизм, Ом", "2;2;.;3", "3;2;.;3", "4;2;.;3", "5;2;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {1, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow),
                    new TablesRow(new string[] { "", "δ, %", "2;2;%;2;0;.;3",    "3;2;%;3;0;.;3",    "4;2;%;4;0;.;3",    "5;2;%;5;0;.;3" }, new int[] {1, 1, 1, 1, 1, 1}, new int[] {2, 0, 0, 0, 0, 0}, TablesRow.RowsType.SimpleRow)
                }
            },
            #endregion
        #endregion
    };
        
    #endregion
    }
}
