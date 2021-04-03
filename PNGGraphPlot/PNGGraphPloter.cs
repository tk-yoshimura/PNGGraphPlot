using System;
using System.Drawing;
using System.Linq;

namespace PNGGraphPlot {
    public class PNGGraphPloter {
        readonly PNGPloter ploter;
        readonly Font font;
        readonly int font_size, frame_width, width, height, margin, graph_x, graph_y, graph_width, graph_height;

        readonly AxisType x_axis_type, y_axis_type;

        decimal x_min = 0, y_min = 0, x2_min = 0, y2_min = 0;
        decimal x_max = 1, y_max = 1, x2_max = 1, y2_max = 1;
        decimal x_range = 1, y_range = 1, x2_range = 1, y2_range = 1;
        decimal x_scale, y_scale, x2_scale, y2_scale;

        ScaleType x_scale_type = ScaleType.Linear, y_scale_type = ScaleType.Linear, x2_scale_type = ScaleType.Linear, y2_scale_type = ScaleType.Linear;

        private bool is_set_x_scale = false;
        private bool is_set_x2_scale = false;
        private bool is_set_y_scale = false;
        private bool is_set_y2_scale = false;
        private bool is_clip = false;

        public PNGGraphPloter(uint graph_width, uint graph_height, uint margin, string font_family, uint font_size, uint frame_width, AxisType x_axis_type = AxisType.Single, AxisType y_axis_type = AxisType.Single)
            : base() {

            this.ploter = new PNGPloter(
                (int)graph_width + (int)margin * 2 + ((y_axis_type == AxisType.Single) ? 1 : 2) * (int)font_size * 7 + (int)frame_width * 2,
                (int)graph_height + (int)margin * 2 + ((x_axis_type == AxisType.Single) ? 1 : 2) * (int)font_size * 4 + (int)frame_width * 2);

            this.width = ploter.Width;
            this.height = ploter.Height;
            this.margin = (int)margin;
            this.font_size = (int)font_size;
            this.frame_width = (int)frame_width;
            this.graph_width = (int)graph_width;
            this.graph_height = (int)graph_height;
            this.graph_x = this.margin + this.frame_width + this.font_size * 7;

            if (x_axis_type == AxisType.Single) {
                this.graph_y = this.margin + this.frame_width;
            }
            else {
                this.graph_y = this.margin + this.frame_width + this.font_size * 4;
            }

            this.x_axis_type = x_axis_type;
            this.y_axis_type = y_axis_type;
            this.font_size = (int)font_size;

            this.font = new Font(font_family, font_size);

            ploter.FillColor(Color.White);
            ploter.DrawRect(Color.Black, this.graph_x, this.graph_y, this.graph_width, this.graph_height, this.frame_width, DrawRectOption.Outer);
        }

        public void Save(string file_path) {
            ploter.Save(file_path);
        }

        public void DrawXLabel(Color cr, string text) {
            Clip = false;
            ploter.DrawText(cr, text, font, graph_x + graph_width / 2, height - margin, TextAnchor.Center, TextAnchor.End);
        }

        public void DrawYLabel(Color cr, string text) {
            Clip = false;
            ploter.DrawRotateText(cr, text, font, margin, graph_y + graph_height / 2, 270, TextAnchor.Center, TextAnchor.Begin);
        }

        public void DrawX2Label(Color cr, string text) {
            if (x_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }

            Clip = false;
            ploter.DrawText(cr, text, font, graph_x + graph_width / 2, margin, TextAnchor.Center, TextAnchor.Begin);
        }

        public void DrawY2Label(Color cr, string text) {
            if (y_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }

            Clip = false;
            ploter.DrawRotateText(cr, text, font, width - margin, graph_y + graph_height / 2, 270, TextAnchor.Center, TextAnchor.End);
        }

        private void DrawXScale(Color cr, string text, decimal x) {
            if (is_set_x_scale) {
                throw new InvalidOperationException("already drawn.");
            }

            if (x >= x_min && x <= x_max) {
                float x_pos = (float)LinearCoordX((double)x);

                Clip = false;
                ploter.DrawLine(cr, x_pos, graph_y + graph_height + frame_width - 1, x_pos, graph_y + graph_height + frame_width + font_size * 3 / 5, frame_width);
                ploter.DrawText(cr, text, font, x_pos, graph_y + graph_height + frame_width + font_size * 4 / 5, TextAnchor.Center, TextAnchor.Begin);
            }
        }

        private void DrawYScale(Color cr, string text, decimal y) {
            if (is_set_y_scale) {
                throw new InvalidOperationException("already drawn.");
            }

            if (y >= y_min && y <= y_max) {
                float y_pos = (float)LinearCoordY((double)y);

                Clip = false;
                ploter.DrawLine(cr, graph_x - frame_width, y_pos, graph_x - frame_width - font_size * 3 / 5, y_pos, frame_width);
                ploter.DrawText(cr, text, font, graph_x - frame_width - font_size * 4 / 5, y_pos, TextAnchor.End, TextAnchor.Center);
            }
        }

        private void DrawX2Scale(Color cr, string text, decimal x2) {
            if (x_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }
            if (is_set_x2_scale) {
                throw new InvalidOperationException("already drawn.");
            }

            if (x2 >= x2_min && x2 <= x2_max) {
                float x2_pos = (float)LinearCoordX2((double)x2);

                Clip = false;
                ploter.DrawLine(cr, x2_pos, graph_y - frame_width, x2_pos, graph_y - frame_width - font_size * 3 / 5, frame_width);
                ploter.DrawText(cr, text, font, x2_pos, graph_y - frame_width - font_size * 4 / 5, TextAnchor.Center, TextAnchor.End);
            }
        }

        private void DrawY2Scale(Color cr, string text, decimal y2) {
            if (y_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }
            if (is_set_y2_scale) {
                throw new InvalidOperationException("already drawn.");
            }

            if (y2 >= y2_min && y2 <= y2_max) {
                float y2_pos = (float)LinearCoordY2((double)y2);

                Clip = false;
                ploter.DrawLine(cr, graph_x + graph_width + frame_width - 1, y2_pos, graph_x + graph_width + frame_width + font_size * 3 / 5, y2_pos, frame_width);
                ploter.DrawText(cr, text, font, graph_x + graph_width + frame_width + font_size * 4 / 5, y2_pos, TextAnchor.Begin, TextAnchor.Center);
            }
        }

        public void DrawXScale(Color cr, decimal min, decimal max, decimal scale, string format) {
            if (is_set_x_scale) {
                throw new InvalidOperationException("already drawn.");
            }
            if (min >= max || (max - min) < scale || scale <= 0) {
                throw new ArgumentException($"{min}<{max}, {scale}>{max}-{min}");
            }

            x_min = min;
            x_max = max;
            x_range = x_max - x_min;
            x_scale = scale;
            x_scale_type = ScaleType.Linear;

            int i_min = (int)(x_min / x_scale), i_max = (int)(x_max / x_scale);
            decimal x = i_min * x_scale;

            for (int i = i_min; i <= i_max; i++) {
                DrawXScale(cr, x.ToString(format), x);
                x += x_scale;
            }

            is_set_x_scale = true;
        }

        public void DrawXScale(Color cr, decimal min, decimal max, decimal scale) {
            DrawXScale(cr, min, max, scale, "0.###");
        }

        public void DrawXLogScale(Color cr, decimal min, decimal max, int scale) {
            if (is_set_x_scale) {
                throw new InvalidOperationException("already drawn.");
            }
            if (min >= max || (max - min) < scale || scale <= 0) {
                throw new ArgumentException($"{min}<{max}, {scale}>{max}-{min}");
            }

            x_min = min;
            x_max = max;
            x_range = x_max - x_min;
            x_scale = scale;
            x_scale_type = ScaleType.Log10;

            int i_min = (int)(x_min / scale), i_max = (int)(x_max / scale);

            for (int i, j = i_min; j <= i_max; j++) {
                i = j * scale;

                if (Math.Abs(i) < 3) {
                    switch (i) {
                        case -2:
                            DrawXScale(cr, "0.01", i);
                            break;
                        case -1:
                            DrawXScale(cr, "0.1", i);
                            break;
                        case 0:
                            DrawXScale(cr, "1", i);
                            break;
                        case 1:
                            DrawXScale(cr, "10", i);
                            break;
                        case 2:
                            DrawXScale(cr, "100", i);
                            break;
                    }
                }
                else {
                    DrawXScale(cr, "10e" + i.ToString("+#;-#"), i);
                }
            }

            is_set_x_scale = true;
        }

        public void DrawYScale(Color cr, decimal min, decimal max, decimal scale, string format) {
            if (is_set_y_scale) {
                throw new InvalidOperationException("already drawn.");
            }
            if (min >= max || (max - min) < scale || scale <= 0) {
                throw new ArgumentException($"{min}<{max}, {scale}>{max}-{min}");
            }

            y_min = min;
            y_max = max;
            y_range = y_max - y_min;
            y_scale = scale;
            y_scale_type = ScaleType.Linear;

            int i_min = (int)(y_min / y_scale), i_max = (int)(y_max / y_scale);
            decimal y = i_min * y_scale;

            for (int i = i_min; i <= i_max; i++) {
                DrawYScale(cr, y.ToString(format), y);
                y += y_scale;
            }

            is_set_y_scale = true;
        }

        public void DrawYScale(Color cr, decimal min, decimal max, decimal scale) {
            DrawYScale(cr, min, max, scale, "0.###");
        }

        public void DrawYLogScale(Color cr, decimal min, decimal max, int scale) {
            if (is_set_y_scale) {
                throw new InvalidOperationException("already drawn.");
            }
            if (min >= max || (max - min) < scale || scale <= 0) {
                throw new ArgumentException($"{min}<{max}, {scale}>{max}-{min}");
            }

            y_min = min;
            y_max = max;
            y_range = y_max - y_min;
            y_scale = scale;
            y_scale_type = ScaleType.Log10;

            int i_min = (int)(y_min / scale), i_max = (int)(y_max / scale);

            for (int i, j = i_min; j <= i_max; j++) {
                i = j * scale;

                if (Math.Abs(i) < 3) {
                    switch (i) {
                        case -2:
                            DrawYScale(cr, "0.01", i);
                            break;
                        case -1:
                            DrawYScale(cr, "0.1", i);
                            break;
                        case 0:
                            DrawYScale(cr, "1", i);
                            break;
                        case 1:
                            DrawYScale(cr, "10", i);
                            break;
                        case 2:
                            DrawYScale(cr, "100", i);
                            break;
                    }
                }
                else {
                    DrawYScale(cr, "10e" + i.ToString("+#;-#"), i);
                }
            }

            is_set_y_scale = true;
        }

        public void DrawX2Scale(Color cr, decimal min, decimal max, decimal scale) {
            if (x_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }
            if (is_set_x2_scale) {
                throw new InvalidOperationException("already drawn.");
            }
            if (min >= max || (max - min) < scale || scale <= 0) {
                throw new ArgumentException($"{min}<{max}, {scale}>{max}-{min}");
            }

            x2_min = min;
            x2_max = max;
            x2_range = x2_max - x2_min;
            x2_scale = scale;
            x2_scale_type = ScaleType.Linear;

            int i_min = (int)(x2_min / x2_scale), i_max = (int)(x2_max / x2_scale);
            decimal x2 = i_min * x2_scale;

            for (int i = i_min; i <= i_max; i++) {
                DrawX2Scale(cr, x2.ToString("0.###"), x2);
                x2 += x2_scale;
            }

            is_set_x2_scale = true;
        }

        public void DrawX2LogScale(Color cr, decimal min, decimal max, int scale) {
            if (x_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }
            if (is_set_x2_scale) {
                throw new InvalidOperationException("already drawn.");
            }
            if (min >= max || (max - min) < scale || scale <= 0) {
                throw new ArgumentException($"{min}<{max}, {scale}>{max}-{min}");
            }

            x2_min = min;
            x2_max = max;
            x2_range = x2_max - x2_min;
            x2_scale = scale;
            x2_scale_type = ScaleType.Log10;

            int i_min = (int)(x2_min / scale), i_max = (int)(x2_max / scale);

            for (int i, j = i_min; j <= i_max; j++) {
                i = j * scale;

                if (Math.Abs(i) < 3) {
                    switch (i) {
                        case -2:
                            DrawX2Scale(cr, "0.01", i);
                            break;
                        case -1:
                            DrawX2Scale(cr, "0.1", i);
                            break;
                        case 0:
                            DrawX2Scale(cr, "1", i);
                            break;
                        case 1:
                            DrawX2Scale(cr, "10", i);
                            break;
                        case 2:
                            DrawX2Scale(cr, "100", i);
                            break;
                    }
                }
                else {
                    DrawX2Scale(cr, "10e" + i.ToString("+#;-#"), i);
                }
            }

            is_set_x2_scale = true;
        }

        public void DrawY2Scale(Color cr, decimal min, decimal max, decimal scale) {
            if (y_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }
            if (is_set_y2_scale) {
                throw new InvalidOperationException("already drawn.");
            }
            if (min >= max || (max - min) < scale || scale <= 0) {
                throw new ArgumentException($"{min}<{max}, {scale}>{max}-{min}");
            }

            y2_min = min;
            y2_max = max;
            y2_range = y2_max - y2_min;
            y2_scale = scale;
            y2_scale_type = ScaleType.Linear;

            int i_min = (int)(y2_min / y2_scale), i_max = (int)(y2_max / y2_scale);
            decimal y2 = i_min * y2_scale;

            for (int i = i_min; i <= i_max; i++) {
                DrawY2Scale(cr, y2.ToString("0.###"), y2);
                y2 += y2_scale;
            }

            is_set_y2_scale = true;
        }

        public void DrawY2LogScale(Color cr, decimal min, decimal max, int scale) {
            if (y_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }
            if (is_set_y2_scale) {
                throw new InvalidOperationException("already drawn.");
            }
            if (min >= max || (max - min) < scale || scale <= 0) {
                throw new ArgumentException($"{min}<{max}, {scale}>{max}-{min}");
            }

            y2_min = min;
            y2_max = max;
            y2_range = y2_max - y2_min;
            y2_scale = scale;
            y2_scale_type = ScaleType.Log10;

            int i_min = (int)(y2_min / scale), i_max = (int)(y2_max / scale);

            for (int i, j = i_min; j <= i_max; j++) {
                i = j * scale;

                if (Math.Abs(i) < 3) {
                    switch (i) {
                        case -2:
                            DrawY2Scale(cr, "0.01", i);
                            break;
                        case -1:
                            DrawY2Scale(cr, "0.1", i);
                            break;
                        case 0:
                            DrawY2Scale(cr, "1", i);
                            break;
                        case 1:
                            DrawY2Scale(cr, "10", i);
                            break;
                        case 2:
                            DrawY2Scale(cr, "100", i);
                            break;
                    }
                }
                else {
                    DrawY2Scale(cr, "10e" + i.ToString("+#;-#"), i);
                }
            }

            is_set_y2_scale = true;
        }

        public void DrawPoint(Color cr, double x, double y, double size, PointType point_type = PointType.Circle, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            float x_pos, y_pos;
            x_pos = (float)((x_coord == Coord.X) ? CoordX(x) : CoordX2(x));
            y_pos = (float)((y_coord == Coord.Y) ? CoordY(y) : CoordY2(y));

            if (double.IsNaN(x_pos) || double.IsNaN(y_pos)) {
                return;
            }

            Clip = true;
            ploter.DrawPoint(cr, (float)size, point_type, x_pos, y_pos);
        }

        public void DrawPoints(Color cr, double[] x, double[] y, double size, PointType point_type = PointType.Circle, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            if (x.Length != y.Length) {
                throw new ArgumentException($"mismatch length {nameof(x)},{nameof(y)}");
            }
            if (!(size > 0)) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            PointF[] v = Remap(x, y, x_coord, y_coord);

            Clip = true;
            ploter.DrawPoints(cr, (float)size, point_type, v);
        }

        public void DrawLine(Color cr, double x1, double y1, double x2, double y2, double stroke_width = 1, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            float x1_pos, y1_pos, x2_pos, y2_pos;

            x1_pos = (float)((x_coord == Coord.X) ? CoordX(x1) : CoordX2(x1));
            x2_pos = (float)((x_coord == Coord.X) ? CoordX(x2) : CoordX2(x2));
            y1_pos = (float)((y_coord == Coord.Y) ? CoordY(y1) : CoordY2(y1));
            y2_pos = (float)((y_coord == Coord.Y) ? CoordY(y2) : CoordY2(y2));

            if (double.IsNaN(x1_pos) || double.IsNaN(y1_pos) || double.IsNaN(x2_pos) || double.IsNaN(y2_pos)) {
                return;
            }

            Clip = true;
            ploter.DrawLine(cr, x1_pos, y1_pos, x2_pos, y2_pos, (float)stroke_width);
        }

        public void DrawPolyline(Color cr, double[] x, double[] y, double stroke_width = 1, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            if (x.Length != y.Length) {
                throw new ArgumentException($"mismatch length {nameof(x)},{nameof(y)}");
            }

            PointF[] v = Remap(x, y, x_coord, y_coord);

            Clip = true;
            ploter.DrawPolyline(cr, v, (float)stroke_width);
        }

        public void DrawCurve(Color cr, double[] x, double[] y, double stroke_width = 1, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            if (x.Length != y.Length) {
                throw new ArgumentException($"mismatch length {nameof(x)},{nameof(y)}");
            }

            PointF[] v = Remap(x, y, x_coord, y_coord);

            Clip = true;
            ploter.DrawCurve(cr, v, (float)stroke_width);
        }

        public void DrawRect(Color cr, double x1, double y1, double x2, double y2, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            float x1_pos, y1_pos, x2_pos, y2_pos;

            x1_pos = (float)((x_coord == Coord.X) ? CoordX(x1) : CoordX2(x1));
            x2_pos = (float)((x_coord == Coord.X) ? CoordX(x2) : CoordX2(x2));
            y1_pos = (float)((y_coord == Coord.Y) ? CoordY(y1) : CoordY2(y1));
            y2_pos = (float)((y_coord == Coord.Y) ? CoordY(y2) : CoordY2(y2));

            if (double.IsNaN(x1_pos) || double.IsNaN(y1_pos) || double.IsNaN(x2_pos) || double.IsNaN(y2_pos)) {
                return;
            }

            Clip = true;
            ploter.DrawFillRect(cr, x1_pos, y1_pos, x2_pos, y2_pos);
        }

        public void DrawHorizontalBar(Color cr, double x, double y, double thickness, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            DrawRect(cr, 0, y - thickness / 2, x, y + thickness / 2, x_coord, y_coord);
        }

        public void DrawVerticalBar(Color cr, double x, double y, double thickness, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            DrawRect(cr, x - thickness / 2, 0, x + thickness / 2, y, x_coord, y_coord);
        }

        public void DrawHistogram(Color cr, decimal histogram_begin, decimal histogram_end, double[] data, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            if (!(histogram_begin < histogram_end)) {
                throw new ArgumentException($"{histogram_begin}<{histogram_end}");
            }
            if (data.Length < 2) {
                throw new ArgumentException(nameof(data));
            }

            double x = (double)histogram_begin, dx = (double)(histogram_end - histogram_begin) / (data.Length - 1);

            for (int i = 0; i < data.Length; i++) {
                DrawVerticalBar(cr, x, data[i], dx, x_coord, y_coord);
                x += dx;
            }
        }

        public void DrawLineGraph(Color cr, decimal linegraph_begin, decimal linegraph_end, double[] data, double stroke_width = 1, PointType point_type = PointType.None, double point_size = 2, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            if (!(linegraph_begin < linegraph_end)) {
                throw new ArgumentException($"{linegraph_begin}<{linegraph_end}");
            }
            if (data.Length < 2) {
                throw new ArgumentException(nameof(data));
            }

            double x = (double)linegraph_begin, dx = (double)(linegraph_end - linegraph_begin) / (data.Length - 1);
            double[] vx = new double[data.Length];

            for (int i = 0; i < data.Length; i++) {
                vx[i] = x;
                x += dx;
            }

            DrawPolyline(cr, vx, data, stroke_width, x_coord, y_coord);

            if (point_type != PointType.None) {
                DrawPoints(cr, vx, data, point_size, point_type, x_coord, y_coord);
            }
        }

        public void DrawSmoothLineGraph(Color cr, decimal linegraph_begin, decimal linegraph_end, double[] data, double stroke_width = 1, PointType point_type = PointType.None, double point_size = 2, Coord x_coord = Coord.X, Coord y_coord = Coord.Y) {
            if (!(linegraph_begin < linegraph_end)) {
                throw new ArgumentException($"{linegraph_begin}<{linegraph_end}");
            }
            if (data.Length < 2) {
                throw new ArgumentException(nameof(data));
            }

            double x = (double)linegraph_begin, dx = (double)(linegraph_end - linegraph_begin) / (data.Length - 1);
            double[] vx = new double[data.Length];

            for (int i = 0; i < data.Length; i++) {
                vx[i] = x;
                x += dx;
            }

            DrawCurve(cr, vx, data, stroke_width, x_coord, y_coord);

            if (point_type != PointType.None) {
                DrawPoints(cr, vx, data, point_size, point_type, x_coord, y_coord);
            }
        }

        public double LinearCoordX(double x) {
            return graph_x + (x - (double)x_min) / (double)x_range * (graph_width - 1);
        }

        public double[] LinearCoordX(double[] xs) {
            double x_min = (double)this.x_min, x_range = (double)this.x_range;

            return xs.Select((x) => graph_x + (x - x_min) / x_range * (graph_width - 1)).ToArray();
        }

        public double LinearCoordX2(double x2) {
            return graph_x + (x2 - (double)x2_min) / (double)x2_range * (graph_width - 1);
        }

        public double[] LinearCoordX2(double[] xs2) {
            double x2_min = (double)this.x2_min, x2_range = (double)this.x2_range;

            return xs2.Select((x) => graph_x + (x - x2_min) / x2_range * (graph_width - 1)).ToArray();
        }

        public double LinearCoordY(double y) {
            return graph_y + (1 - (y - (double)y_min) / (double)y_range) * (graph_height - 1);
        }

        public double[] LinearCoordY(double[] ys) {
            double y_min = (double)this.y_min, y_range = (double)this.y_range;

            return ys.Select((y) => graph_y + (1 - (y - (double)y_min) / (double)y_range) * (graph_height - 1)).ToArray();
        }

        public double LinearCoordY2(double y2) {
            return graph_y + (1 - (y2 - (double)y2_min) / (double)y2_range) * (graph_height - 1);
        }

        public double[] LinearCoordY2(double[] ys2) {
            double y2_min = (double)this.y2_min, y2_range = (double)this.y2_range;

            return ys2.Select((y) => graph_y + (1 - (y - (double)y2_min) / (double)y2_range) * (graph_height - 1)).ToArray();
        }

        public double LogCoordX(double x) {
            return LinearCoordX(Math.Log10(x));
        }

        public double LogCoordX2(double x2) {
            return LinearCoordX2(Math.Log10(x2));
        }

        public double LogCoordY(double y) {
            return LinearCoordY(Math.Log10(y));
        }

        public double LogCoordY2(double y2) {
            return LinearCoordY2(Math.Log10(y2));
        }

        public double[] LogCoordX(double[] xs) {
            double x_min = (double)this.x_min, x_range = (double)this.x_range;

            return xs.Select((x) => graph_x + (Math.Log10(x) - x_min) / x_range * (graph_width - 1)).ToArray();
        }

        public double[] LogCoordX2(double[] xs2) {
            double x2_min = (double)this.x2_min, x2_range = (double)this.x2_range;

            return xs2.Select((x) => graph_x + (Math.Log10(x) - x2_min) / x2_range * (graph_width - 1)).ToArray();
        }

        public double[] LogCoordY(double[] ys) {
            double y_min = (double)this.y_min, y_range = (double)this.y_range;

            return ys.Select((y) => graph_y + (1 - (Math.Log10(y) - (double)y_min) / (double)y_range) * (graph_height - 1)).ToArray();
        }

        public double[] LogCoordY2(double[] ys2) {
            double y2_min = (double)this.y2_min, y2_range = (double)this.y2_range;

            return ys2.Select((y) => graph_y + (1 - (Math.Log10(y) - (double)y2_min) / (double)y2_range) * (graph_height - 1)).ToArray();
        }

        public double CoordX(double x) {
            return (x_scale_type == ScaleType.Linear) ? LinearCoordX(x) : LogCoordX(x);
        }

        public double CoordX2(double x2) {
            if (x_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }

            return (x2_scale_type == ScaleType.Linear) ? LinearCoordX2(x2) : LogCoordX2(x2);
        }

        public double CoordY(double y) {
            return (y_scale_type == ScaleType.Linear) ? LinearCoordY(y) : LogCoordY(y);
        }

        public double CoordY2(double y2) {
            if (y_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }

            return (y2_scale_type == ScaleType.Linear) ? LinearCoordY2(y2) : LogCoordY2(y2);
        }

        public double[] CoordX(double[] xs) {
            return (x_scale_type == ScaleType.Linear) ? LinearCoordX(xs) : LogCoordX(xs);
        }

        public double[] CoordX2(double[] xs2) {
            if (x_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }

            return (x2_scale_type == ScaleType.Linear) ? LinearCoordX2(xs2) : LogCoordX2(xs2);
        }

        public double[] CoordY(double[] ys) {
            return (y_scale_type == ScaleType.Linear) ? LinearCoordY(ys) : LogCoordY(ys);
        }

        public double[] CoordY2(double[] ys2) {
            if (y_axis_type == AxisType.Single) {
                throw new InvalidOperationException("specified a single axis.");
            }

            return (y2_scale_type == ScaleType.Linear) ? LinearCoordY2(ys2) : LogCoordY2(ys2);
        }

        private PointF[] Remap(double[] x, double[] y, Coord x_coord, Coord y_coord) {
            if (x.Length != y.Length) {
                throw new ArgumentException($"mismatch length {nameof(x)},{nameof(y)}");
            }

            int num = x.Length;

            double[] u = (x_coord == Coord.X) ? CoordX(x) : CoordX2(x);
            double[] v = (y_coord == Coord.Y) ? CoordY(y) : CoordY2(y);

            PointF[] p = new PointF[num];

            for (int i = 0; i < num; i++) {
                p[i].X = (float)u[i];
                p[i].Y = (float)v[i];
            }

            return p;
        }

        public bool Clip {
            set {
                if (value != is_clip) {
                    if (value) {
                        ploter.SetClip(new Rectangle(graph_x, graph_y, graph_width, graph_height));
                    }
                    else {
                        ploter.ResetClip();
                    }
                    is_clip = value;
                }
            }

            get {
                return is_clip;
            }
        }

        public virtual void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                font.Dispose();
                ploter.Dispose();
            }
        }

        ~PNGGraphPloter() {
            Dispose(false);
        }
    }
}
