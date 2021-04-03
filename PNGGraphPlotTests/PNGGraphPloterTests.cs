using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;

namespace PNGGraphPlot.Tests {
    [TestClass()]
    public class PNGGraphPloterTests {
        const string outdir = "../../../test_outputs/";

        public PNGGraphPloterTests() {
            Directory.CreateDirectory(outdir);
        }

        [TestMethod()]
        public void DrawPointTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawYScale(Color.Black, 0.20m, 0.50m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            graph.DrawPoint(Color.Violet, 0.40, 0.20, 10, PointType.Circle);
            graph.DrawPoint(Color.Violet, 0.80, 0.20, 10, PointType.Circle);
            graph.DrawPoint(Color.Violet, 0.40, 0.50, 10, PointType.Circle);
            graph.DrawPoint(Color.Violet, 0.80, 0.50, 10, PointType.Circle);

            graph.DrawPoint(Color.Black, 0.50, 0.25, 10, PointType.Circle);
            graph.DrawPoint(Color.Red, 0.55, 0.30, 20, PointType.Triangle);
            graph.DrawPoint(Color.Green, 0.60, 0.35, 30, PointType.Square);
            graph.DrawPoint(Color.Blue, 0.65, 0.40, 40, PointType.Pentagon);
            graph.DrawPoint(Color.Orange, 0.70, 0.45, 50, PointType.Hexagon);

            graph.Save(outdir + "draw_graph_point.png");
        }

        [TestMethod()]
        public void DrawPointsTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawYScale(Color.Black, 0.20m, 0.50m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            double[] x = new double[] { 0.50, 0.55, 0.60, 0.65, 0.70 };
            double[] y = new double[] { 0.25, 0.30, 0.35, 0.40, 0.45 };

            graph.DrawPoints(Color.Black, x, y, 20, PointType.Circle);

            graph.Save(outdir + "draw_graph_points.png");
        }

        [TestMethod()]
        public void DrawLineTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.20m, 0.70m, 0.05m);
            graph.DrawYScale(Color.Black, 0.20m, 0.80m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            graph.DrawLine(Color.Black, 0.50, 0.25, 0.25, 0.50, 1);
            graph.DrawLine(Color.Red, 0.55, 0.30, 0.30, 0.55, 2);
            graph.DrawLine(Color.Green, 0.60, 0.35, 0.35, 0.60, 3);
            graph.DrawLine(Color.Blue, 0.65, 0.40, 0.40, 0.65, 4);
            graph.DrawLine(Color.Orange, 0.70, 0.45, 0.45, 0.70, 5);

            graph.Save(outdir + "draw_graph_line.png");
        }

        [TestMethod()]
        public void DrawPolylineTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawYScale(Color.Black, 0.20m, 0.50m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            double[] x = new double[] { 0.50, 0.55, 0.60, 0.65, 0.70 };
            double[] y = new double[] { 0.28, 0.29, 0.32, 0.45, 0.40 };

            graph.DrawPolyline(Color.Black, x, y, 20);

            graph.Save(outdir + "draw_graph_polyline.png");
        }

        [TestMethod()]
        public void DrawCurveTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawYScale(Color.Black, 0.20m, 0.50m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            double[] x = new double[] { 0.50, 0.55, 0.60, 0.65, 0.70 };
            double[] y = new double[] { 0.28, 0.29, 0.32, 0.45, 0.40 };

            graph.DrawCurve(Color.Black, x, y, 20);

            graph.Save(outdir + "draw_graph_polyline.png");
        }

        [TestMethod()]
        public void DrawRectTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.20m, 0.70m, 0.05m);
            graph.DrawYScale(Color.Black, 0.20m, 0.80m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            graph.DrawRect(Color.Black, 0.50, 0.25, 0.25, 0.50);
            graph.DrawRect(Color.Red, 0.55, 0.30, 0.30, 0.55);
            graph.DrawRect(Color.Green, 0.60, 0.35, 0.35, 0.60);
            graph.DrawRect(Color.Blue, 0.65, 0.40, 0.40, 0.65);
            graph.DrawRect(Color.Orange, 0.70, 0.45, 0.45, 0.70);

            graph.Save(outdir + "draw_graph_rect.png");
        }

        [TestMethod()]
        public void DrawHorizontalBarTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.20m, 0.70m, 0.05m);
            graph.DrawYScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            graph.DrawHorizontalBar(Color.Black, 0.3, 0.5, 0.01);

            graph.Save(outdir + "draw_graph_hbar.png");
        }

        [TestMethod()]
        public void DrawVerticalBarTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.20m, 0.70m, 0.05m);
            graph.DrawYScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            graph.DrawVerticalBar(Color.Black, 0.3, 0.5, 0.01);

            graph.Save(outdir + "draw_graph_vbar.png");
        }

        [TestMethod()]
        public void DrawHistogramTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.20m, 0.70m, 0.05m);
            graph.DrawYScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            double[] vs = new double[] { 0.42, 0.53, 0.73, 0.45, 0.55, 0.60, 0.40, 0.70, 0.75 };

            graph.DrawHistogram(Color.Black, 0.25m, 0.65m, vs);

            graph.Save(outdir + "draw_graph_hist.png");
        }

        [TestMethod()]
        public void DrawLineGraphTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.20m, 0.70m, 0.05m);
            graph.DrawYScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            double[] vs = new double[] { 0.42, 0.53, 0.73, 0.45, 0.55, 0.60, 0.40, 0.70, 0.75 };

            graph.DrawLineGraph(Color.Black, 0.25m, 0.65m, vs, 5);

            graph.Save(outdir + "draw_graph_linegraph.png");
        }

        [TestMethod()]
        public void DrawSmoothLineGraphTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXScale(Color.Black, 0.20m, 0.70m, 0.05m);
            graph.DrawYScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            double[] vs = new double[] { 0.42, 0.53, 0.73, 0.45, 0.55, 0.60, 0.40, 0.70, 0.75 };

            graph.DrawSmoothLineGraph(Color.Black, 0.25m, 0.65m, vs, 5);

            graph.Save(outdir + "draw_graph_smoothlinegraph.png");
        }

        [TestMethod()]
        public void Axis2Test() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4, AxisType.Twin, AxisType.Twin);

            graph.DrawXScale(Color.Black, 0.40m, 0.80m, 0.05m);
            graph.DrawYScale(Color.Black, 0.20m, 0.50m, 0.05m);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            graph.DrawX2Scale(Color.Black, 0.50m, 0.70m, 0.05m);
            graph.DrawY2Scale(Color.Black, 0.25m, 0.45m, 0.05m);
            graph.DrawX2Label(Color.Black, "u");
            graph.DrawY2Label(Color.Black, "v");

            double[] x = new double[] { 0.50, 0.55, 0.60, 0.65, 0.70 };
            double[] y = new double[] { 0.25, 0.30, 0.35, 0.40, 0.45 };

            graph.DrawPoints(Color.Black, x, y, 20, PointType.Circle, Coord.X2, Coord.Y2);

            graph.Save(outdir + "draw_graph_axis2.png");
        }

        [TestMethod()]
        public void LogPlotTest() {
            PNGGraphPloter graph = new(1600, 1000, 20, "Times New Roman", 30, 4);

            graph.DrawXLogScale(Color.Black, -10, +5, 2);
            graph.DrawYLogScale(Color.Black, -5, +2, 2);
            graph.DrawXLabel(Color.Black, "x");
            graph.DrawYLabel(Color.Black, "y");

            double[] us = new double[] { 1e-8, 1e-7, 1e-6, 1e-5, 1e-4, 1e-3, 1e-2, 1e-1, 1e+0, 1e+1, 1e+2, 1e+3 };
            double[] vs = new double[] { 1e-4, 2e-4, 3e-3, 4e-3, 1e-2, 1e-1, 5e-2, 1e+0, 2e-2, 1e-2, 1e-1, 4e-2 };

            graph.DrawPolyline(Color.Black, us, vs, 5);

            graph.Save(outdir + "draw_graph_logplot.png");
        }
    }
}