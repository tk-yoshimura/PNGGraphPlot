using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace PNGGraphPlot {
    public class PNGPloter : IDisposable {
        readonly Bitmap bitmap;
        readonly Graphics g;

        public PNGPloter(int width, int height) {
            if (width <= 0 || height <= 0) {
                throw new ArgumentOutOfRangeException($"{width},{height}");
            }

            bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
        }

        public void Save(string file_path) {
            if (!file_path.EndsWith(".png")) {
                file_path += ".png";
            }

            bitmap.Save(file_path, ImageFormat.Png);
        }

        public void DrawLine(Color cr, float x1, float y1, float x2, float y2, float stroke_width) {
            using Pen p = new(cr, stroke_width);
            g.DrawLine(p, x1, y1, x2, y2);
        }

        public void DrawRect(Color cr, float x1, float y1, float x2, float y2, float stroke_width) {
            float x, y, w, h;
            x = Math.Min(x1, x2);
            w = Math.Abs(x1 - x2);
            y = Math.Min(y1, y2);
            h = Math.Abs(y1 - y2);

            using Pen p = new(cr, stroke_width);
            g.DrawRectangle(p, x, y, w, h);
        }

        public void DrawFillRect(Color cr, float x1, float y1, float x2, float y2) {
            float x, y, w, h;
            x = Math.Min(x1, x2);
            w = Math.Abs(x1 - x2);
            y = Math.Min(y1, y2);
            h = Math.Abs(y1 - y2);

            using SolidBrush b = new(cr);
            g.FillRectangle(b, x, y, w, h);
        }

        public void DrawRect(Color cr, int x, int y, int w, int h, int stroke_width, DrawRectOption option) {
            if (stroke_width < 1) {
                return;
            }

            g.SmoothingMode = SmoothingMode.None;

            using (SolidBrush b = new(cr)) {
                if (option == DrawRectOption.Inner) {
                    g.FillRectangle(b, x, y, w - stroke_width, stroke_width);
                    g.FillRectangle(b, x + stroke_width, y + h - stroke_width, w - stroke_width, stroke_width);
                    g.FillRectangle(b, x, y + stroke_width, stroke_width, h - stroke_width);
                    g.FillRectangle(b, x + w - stroke_width, y, stroke_width, h - stroke_width);
                }
                else if (option == DrawRectOption.Outer) {
                    g.FillRectangle(b, x - stroke_width, y - stroke_width, w + stroke_width, stroke_width);
                    g.FillRectangle(b, x, y + h, w + stroke_width, stroke_width);
                    g.FillRectangle(b, x - stroke_width, y, stroke_width, h + stroke_width);
                    g.FillRectangle(b, x + w, y - stroke_width, stroke_width, h + stroke_width);
                }
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
        }

        public void DrawCircle(Color cr, float cx, float cy, float r, float stroke_width) {
            using Pen p = new(cr, stroke_width);
            g.DrawEllipse(p, cx - r, cy - r, 2 * r, 2 * r);
        }

        public void DrawFillCircle(Color cr, float cx, float cy, float r) {
            using SolidBrush b = new(cr);
            g.FillEllipse(b, cx - r, cy - r, 2 * r, 2 * r);
        }

        public void DrawEllipse(Color cr, float cx, float cy, float rx, float ry, float stroke_width) {
            using Pen p = new(cr, stroke_width);
            g.DrawEllipse(p, cx - rx, cy - ry, 2 * rx, 2 * ry);
        }

        public void DrawFillEllipse(Color cr, float cx, float cy, float rx, float ry) {
            using SolidBrush b = new(cr);
            g.FillEllipse(b, cx - rx, cy - ry, 2 * rx, 2 * ry);
        }

        public void DrawTriangle(Color cr, float x1, float y1, float x2, float y2, float x3, float y3, float stroke_width) {
            PointF[] v = new PointF[3] { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3) };

            using Pen p = new(cr, stroke_width);
            g.DrawPolygon(p, v);
        }

        public void DrawFillTriangle(Color cr, float x1, float y1, float x2, float y2, float x3, float y3) {
            PointF[] v = new PointF[3] { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3) };

            using SolidBrush b = new(cr);
            g.FillPolygon(b, v);
        }

        public void DrawPolyline(Color cr, PointF[] v, float stroke_width) {
            using Pen p = new(cr, stroke_width);
            g.DrawLines(p, v);
        }

        public void DrawPolygon(Color cr, PointF[] v, float stroke_width) {
            using Pen p = new(cr, stroke_width);
            g.DrawPolygon(p, v);
        }

        public void DrawFillPolygon(Color cr, PointF[] v) {
            using SolidBrush b = new(cr);
            g.FillPolygon(b, v);
        }

        public void DrawRegularPolygon(Color cr, int vertex_num, int cx, int cy, int r, int angle, float stroke_width) {
            PointF[] v = new PointF[vertex_num];
            float s = 2 * (float)Math.PI / vertex_num, t = angle / (float)180.0 * (float)Math.PI;

            for (int i = 0; i < vertex_num; i++) {
                v[i].X = cx + (float)Math.Sin(t) * r;
                v[i].Y = cy - (float)Math.Cos(t) * r;
                t -= s;
            }

            DrawPolygon(cr, v, stroke_width);
        }

        public void DrawRegularPolygon(Color cr, int vertex_num, float cx, float cy, float r, float angle, float stroke_width) {
            PointF[] v = new PointF[vertex_num];
            float s = 2 * (float)Math.PI / vertex_num, t = angle / (float)180.0 * (float)Math.PI;

            for (int i = 0; i < vertex_num; i++) {
                v[i].X = cx + (float)Math.Sin(t) * r;
                v[i].Y = cy - (float)Math.Cos(t) * r;
                t -= s;
            }

            DrawPolygon(cr, v, stroke_width);
        }

        public void DrawFillRegularPolygon(Color cr, int vertex_num, float cx, float cy, float r, float angle) {
            PointF[] v = new PointF[vertex_num];
            float s = 2 * (float)Math.PI / vertex_num, t = angle / (float)180.0 * (float)Math.PI;

            for (int i = 0; i < vertex_num; i++) {
                v[i].X = cx + (float)Math.Sin(t) * r;
                v[i].Y = cy - (float)Math.Cos(t) * r;
                t -= s;
            }

            DrawFillPolygon(cr, v);
        }

        public void DrawCurve(Color cr, PointF[] v, float stroke_width) {
            using Pen p = new(cr, stroke_width);
            g.DrawCurve(p, v);
        }

        public void DrawClosedCurve(Color cr, PointF[] v, float stroke_width) {
            using Pen p = new(cr, stroke_width);
            g.DrawClosedCurve(p, v);
        }

        public void DrawFillClosedCurve(Color cr, PointF[] v) {
            using SolidBrush b = new(cr);
            g.FillClosedCurve(b, v);
        }

        public void DrawPoint(Color cr, float size, PointType point_type, float x, float y) {
            float sizef = size * 0.5f, s, t;
            PointF[] p;

            using SolidBrush b = new(cr); switch (point_type) {
                case PointType.Circle:
                    sizef *= 1.128379167095512f;
                    g.FillEllipse(b, x - sizef, y - sizef, 2 * sizef, 2 * sizef);
                    return;
                case PointType.Square:
                    g.FillRectangle(b, x - sizef, y - sizef, 2 * sizef, 2 * sizef);
                    return;
                case PointType.Triangle:
                    sizef *= 1.754765346932842f;
                    p = new PointF[3];
                    s = 2.0f / 3.0f * (float)Math.PI;
                    t = (float)Math.PI;
                    break;
                case PointType.Pentagon:
                    sizef *= 1.297050330592269f;
                    p = new PointF[5];
                    s = 2.0f / 5.0f * (float)Math.PI;
                    t = (float)Math.PI;
                    break;
                case PointType.Hexagon:
                    sizef *= 1.240806478860636f;
                    p = new PointF[6];
                    s = (float)Math.PI / 3.0f;
                    t = (float)Math.PI / 6.0f;
                    break;
                default:
                    return;
            }

            for (int i = 0; i < p.Length; i++) {
                p[i].X = x + (float)Math.Sin(t) * sizef;
                p[i].Y = y + (float)Math.Cos(t) * sizef;
                t -= s;
            }
            g.FillPolygon(b, p);
        }

        public void DrawPoints(Color cr, float size, PointType point_type, PointF[] v) {
            float sizef = size * 0.5f, s, t;
            PointF[] dp, p;

            using SolidBrush b = new(cr); switch (point_type) {
                case PointType.Circle:
                    sizef *= 1.128379167095512f;
                    for (int i = 0; i < v.Length; i++) {
                        g.FillEllipse(b, v[i].X - sizef, v[i].Y - sizef, 2 * sizef, 2 * sizef);
                    }
                    return;
                case PointType.Square:
                    for (int i = 0; i < v.Length; i++) {
                        g.FillRectangle(b, v[i].X - sizef, v[i].Y - sizef, 2 * sizef, 2 * sizef);
                    }
                    return;
                case PointType.Triangle:
                    sizef *= 1.754765346932842f;
                    dp = new PointF[3];
                    p = new PointF[3];
                    s = 2.0f / 3.0f * (float)Math.PI;
                    t = (float)Math.PI;
                    break;
                case PointType.Pentagon:
                    sizef *= 1.297050330592269f;
                    dp = new PointF[5];
                    p = new PointF[5];
                    s = 2.0f / 5.0f * (float)Math.PI;
                    t = (float)Math.PI;
                    break;
                case PointType.Hexagon:
                    sizef *= 1.240806478860636f;
                    dp = new PointF[6];
                    p = new PointF[6];
                    s = (float)Math.PI / 3.0f;
                    t = (float)Math.PI / 6.0f;
                    break;
                default:
                    return;
            }

            for (int i = 0; i < dp.Length; i++) {
                dp[i].X = (float)Math.Sin(t) * sizef;
                dp[i].Y = (float)Math.Cos(t) * sizef;
                t -= s;
            }

            for (int i = 0; i < v.Length; i++) {
                for (int j = 0; j < dp.Length; j++) {
                    p[j].X = v[i].X + dp[j].X;
                    p[j].Y = v[i].Y + dp[j].Y;
                }

                g.FillPolygon(b, p);
            }
        }

        public void DrawText(Color cr, string text, Font font, float x, float y) {
            using SolidBrush b = new(cr); g.DrawString(text, font, b, x, y);
        }

        public void DrawText(Color cr, string text, Font font, float x, float y, TextAnchor x_anchor, TextAnchor y_anchor = TextAnchor.Begin) {
            if (x_anchor != TextAnchor.Begin || y_anchor != TextAnchor.Begin) {
                SizeF text_size = g.MeasureString(text, font);

                if (x_anchor == TextAnchor.Center) {
                    x -= text_size.Width * 0.5f;
                }
                else if (x_anchor == TextAnchor.End) {
                    x -= text_size.Width;
                }

                if (y_anchor == TextAnchor.Center) {
                    y -= text_size.Height * 0.5f;
                }
                else if (y_anchor == TextAnchor.End) {
                    y -= text_size.Height;
                }
            }

            DrawText(cr, text, font, x, y);
        }

        public void DrawRotateText(Color cr, string text, Font font, float x, float y, float angle, TextAnchor x_anchor = TextAnchor.Begin, TextAnchor y_anchor = TextAnchor.Begin) {
            g.TranslateTransform(-x, -y);
            g.RotateTransform(angle, MatrixOrder.Append);
            g.TranslateTransform(x, y, MatrixOrder.Append);

            if (x_anchor != TextAnchor.Begin || y_anchor != TextAnchor.Begin) {
                SizeF text_size = g.MeasureString(text, font);

                if (x_anchor == TextAnchor.Center) {
                    x -= text_size.Width * 0.5f;
                }
                else if (x_anchor == TextAnchor.End) {
                    x -= text_size.Width;
                }

                if (y_anchor == TextAnchor.Center) {
                    y -= text_size.Height * 0.5f;
                }
                else if (y_anchor == TextAnchor.End) {
                    y -= text_size.Height;
                }
            }

            DrawText(cr, text, font, x, y);

            g.ResetTransform();
        }

        public void DrawVerticalText(Color cr, string text, Font font, float x, float y) {
            using SolidBrush b = new(cr); using StringFormat sf = new(StringFormatFlags.DirectionVertical); g.DrawString(text, font, b, x, y, sf);
        }

        public void DrawVerticalText(Color cr, string text, Font font, float x, float y, TextAnchor x_anchor, TextAnchor y_anchor = TextAnchor.Begin) {
            if (x_anchor != TextAnchor.Begin || y_anchor != TextAnchor.Begin) {
                SizeF text_size = g.MeasureString(text, font);

                if (x_anchor == TextAnchor.Center) {
                    y -= text_size.Width * 0.5f;
                }
                else if (x_anchor == TextAnchor.End) {
                    y -= text_size.Width;
                }

                if (y_anchor == TextAnchor.Center) {
                    x -= text_size.Height * 0.5f;
                }
                else if (y_anchor == TextAnchor.End) {
                    x -= text_size.Height;
                }
            }

            DrawVerticalText(cr, text, font, x, y);
        }

        public void DrawRotateVerticalText(Color cr, string text, Font font, float x, float y, float angle, TextAnchor x_anchor = TextAnchor.Begin, TextAnchor y_anchor = TextAnchor.Begin) {
            g.TranslateTransform(-x, -y);
            g.RotateTransform(angle, MatrixOrder.Append);
            g.TranslateTransform(x, y, MatrixOrder.Append);

            if (x_anchor != TextAnchor.Begin || y_anchor != TextAnchor.Begin) {
                SizeF text_size = g.MeasureString(text, font);

                if (x_anchor == TextAnchor.Center) {
                    y -= text_size.Width * 0.5f;
                }
                else if (x_anchor == TextAnchor.End) {
                    y -= text_size.Width;
                }

                if (y_anchor == TextAnchor.Center) {
                    x -= text_size.Height * 0.5f;
                }
                else if (y_anchor == TextAnchor.End) {
                    x -= text_size.Height;
                }
            }

            DrawVerticalText(cr, text, font, x, y);

            g.ResetTransform();
        }

        public void FillColor(Color cr) {
            g.Clear(cr);
        }

        public void SetClip(Rectangle rect) {
            g.SetClip(rect);
        }

        public void ResetClip() {
            g.ResetClip();
        }

        public int Width {
            get {
                return bitmap.Width;
            }
        }

        public int Height {
            get {
                return bitmap.Height;
            }
        }

        public Image Image {
            get {
                return bitmap;
            }
        }

        public virtual void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                g.Dispose();
                bitmap.Dispose();
            }
        }

        ~PNGPloter() {
            Dispose(false);
        }
    }
}
