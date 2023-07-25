using assignment1.wpf;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System;
using System.Linq;
using System.Windows;

namespace assignment1.commands
{
    [Transaction(TransactionMode.Manual)]
    public class DrawClosePolygonWall : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Result result = Result.Failed;
            DrawClosePolygonWallView drawClosePolygonWallView = new DrawClosePolygonWallView(commandData);
            drawClosePolygonWallView.ShowDialog();
            drawClosePolygonWallView.Close();
            result = Result.Succeeded;
            return result;
        }

        public static void DrawPentagon(ExternalCommandData commandData, string wallTypeNameToApply)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            WallType wallTypeToApply = collector.OfClass(typeof(WallType))
                          .Where(wt => wt.Name == wallTypeNameToApply)
                          .FirstOrDefault() as WallType;
            View activeView = commandData.Application.ActiveUIDocument.ActiveView;
            if (activeView.ViewType == ViewType.FloorPlan)
            {
                XYZ centerPoint = new XYZ(0, 0, 0);
                double sideLength = 7.0;
                double radius = sideLength / (2 * Math.Sin(Math.PI / 5));
                double angleIncrement = 2 * Math.PI / 5;
                XYZ[] pentagonPoints = new XYZ[5];
                for (int i = 0; i < 5; i++)
                {
                    double angle = i * angleIncrement;
                    double x = centerPoint.X + radius * Math.Cos(angle);
                    double y = centerPoint.Y + radius * Math.Sin(angle);
                    pentagonPoints[i] = new XYZ(x, y, centerPoint.Z);
                }

                CurveLoop pentagonLoop = new CurveLoop();

                for (int i = 0; i < 5; i++)
                {
                    Line line = Line.CreateBound(pentagonPoints[i], pentagonPoints[(i + 1) % 5]);
                    pentagonLoop.Append(line);
                }
                try
                {
                    using (Transaction transaction = new Transaction(doc, "Create Pentagon Walls"))
                    {
                        transaction.Start();
                        if (activeView.ViewType == ViewType.FloorPlan || activeView.ViewType == ViewType.ThreeD)
                        {
                            ElementId activeLevelId = activeView.GenLevel.Id;
                            foreach (Curve c in pentagonLoop)
                            {
                                Wall wallElem = Wall.Create(doc, c, activeLevelId, false);
                                wallElem.WallType = wallTypeToApply;
                            }
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show($"Error, no wall type named {wallTypeNameToApply} exists.");
            }
        }
    }
}
