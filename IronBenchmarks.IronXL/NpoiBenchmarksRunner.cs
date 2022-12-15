﻿using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace IronBenchmarks.IronXL
{
    internal class NpoiBenchmarksRunner : BaseBenchmarksRunner<ISheet>
    {
        public NpoiBenchmarksRunner(string resultsFolder) : base(resultsFolder)
        {
        }

        protected override string BenchmarkRunnerName => typeof(NpoiBenchmarksRunner).Name.Replace("BenchmarksRunner", "") ?? "NPOI";
        public override string NameAndVersion => $"{BenchmarkRunnerName} v.{GetAssemblyVersion(typeof(ICell))}";
        public override string LicenseKey { set => throw new NotImplementedException(); }

        protected override void PerformBenchmarkWork(Action<ISheet> benchmarkWork, string fileName, bool savingResultingFile)
        {
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet();

            benchmarkWork(sheet);

            if (savingResultingFile)
            {
                workbook.Write(File.Create(fileName));
            }
        }
        protected override void LoadingBigFile(ISheet worksheet)
        {
            _ = new XSSFWorkbook(_largeFileName);
        }
        protected override void CreateRandomCells(ISheet worksheet)
        {
            var rand = new Random();
            for (var i = 0; i <= RandomCellsRowNumber; i++)
            {
                var row = worksheet.CreateRow(i);
                row.CreateCell(0).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(1).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(2).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(3).SetCellValue(rand.Next(32));
                row.CreateCell(4).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(5).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(6).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(7).SetCellValue(rand.Next(13));
                row.CreateCell(8).SetCellValue(GetRandomDate(rand));
                row.CreateCell(9).SetCellValue(GetRandomDate(rand));
                row.CreateCell(10).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(11).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(12).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(13).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(14).SetCellValue((double)GetRandomDecimal(rand));
                row.CreateCell(15).SetCellValue((double)GetRandomDecimal(rand));
            }
        }
        protected override void CreateDateCells(ISheet worksheet)
        {
            var style = worksheet.Workbook.CreateCellStyle();
            style.DataFormat = worksheet.Workbook.CreateDataFormat().GetFormat("dd/MM/yyyy");

            for (var i = 0; i < DateCellsNumber; i++)
            {
                var cell = worksheet.CreateRow(i).CreateCell(0);
                cell.SetCellValue(DateTime.Now);
                cell.CellStyle = style;
            }
        }
        protected override void MakeStyleChanges(ISheet worksheet)
        {
            var font = worksheet.Workbook.CreateFont();
            font.FontHeightInPoints = 22;

            var style = worksheet.Workbook.CreateCellStyle();
            style.SetFont(font);
            style.VerticalAlignment = VerticalAlignment.Top;
            style.Alignment = HorizontalAlignment.Right;

            for (var i = 0; i < StyleChangeRowNumber; i++)
            {
                var row = worksheet.CreateRow(i);
                for (var j = 0; j < 15; j++)
                {
                    var cell = row.CreateCell(j);
                    cell.SetCellValue(_cellValue);
                    cell.CellStyle = style;
                }
            }
        }
        protected override void GenerateFormulas(ISheet worksheet)
        {
            var rnd = new Random();

            for (var i = 0; i < GenerateFormulasRowNumber; i++)
            {
                var row = worksheet.CreateRow(i);
                for (var j = 0; j < 10; j++)
                {
                    var cellA = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    var cellB = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    row.CreateCell(j).SetCellFormula($"{cellA}/{cellB}");
                }
            }

            for (var i = GenerateFormulasRowNumber; i < GenerateFormulasRowNumber * 2; i++)
            {
                var row = worksheet.CreateRow(i);
                for (var j = 0; j < 10; j++)
                {
                    row.CreateCell(j).SetCellValue(GetRandomRandInt(rnd));
                }
            }
        }

        protected override void SortRange(ISheet worksheet)
        {
            
        }
    }
}
