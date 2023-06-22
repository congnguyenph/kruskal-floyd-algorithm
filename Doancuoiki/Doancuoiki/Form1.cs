using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;


namespace Doancuoiki
{
    public partial class Form1 : Form
    {
        int dothi = 0;
        int index = 0;
        List<Pointer> pointers = new List<Pointer>();
        List<Linepointer> lp = new List<Linepointer>();
        List<Edge> ed = new List<Edge>();
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X;
                var y = e.Location.Y;
                var lb = new Label();
                lb.AutoSize = true;
                lb.Text = Char.ConvertFromUtf32(1 + index);
                lb.Name = $"pointer{index}";
                lb.Location = new Point(x, y);
                //this.panel1.Controls.Add(lb);

                pointers.Add(new Pointer
                {
                    X = x,
                    Y = y,
                    Name = lb.Name,
                    Text = lb.Text
                });
                var g = panel1.CreateGraphics();
                Brush cbrush = new SolidBrush(Color.Wheat);
                Pen pCircle = new Pen(Color.Black, 2);
                Point pt = new Point(e.Location.X - 10, e.Location.Y - 10);
                Size sizeCircle = new Size(20, 20);
                Rectangle rec = new Rectangle(pt, sizeCircle);
                DrawNode(g, cbrush, pCircle, rec, pt, lb.Text,30);
                if (index != 0)
                {
                    int count = pointers.Count;
                    int index1 = count - 1;
                    int index2 = count - 2;
                    var indexbefore = pointers[index1];
                    var indexafter = pointers[index2];
                    Pen pline = new Pen(Color.Blue, 3);
                    DrawLine(g, pline, indexbefore, indexafter);
                }
                index++;
            }

        }
        protected override void WndProc(ref Message m)
        {
            // Suppress the WM_UPDATEUISTATE message
            if (m.Msg == 0x128) return;
            base.WndProc(ref m);
        }
        private void DrawLine(Graphics g, Pen pLine, Pointer ptStart, Pointer ptEnd)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.DrawLine(pLine, new Point(ptStart.X, ptStart.Y), new Point(ptEnd.X, ptEnd.Y));

        }

        public void DrawNode(Graphics g, Brush bFillNode, Pen pEllipse, Rectangle rect, Point pt, string name,int number)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.FillEllipse(bFillNode, rect);
            g.DrawEllipse(pEllipse, rect);
            Font stringFont = new Font("Arial", 9);

            SizeF stringSize = g.MeasureString(name, stringFont);
            g.DrawString(name, stringFont, Brushes.Black,
                        pt.X + (number - stringSize.Width) / 2,
                        pt.Y + (number - stringSize.Height) / 2);
        }

        private void labelindex_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        int[,] matran = new int[101, 101];
        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            string test = textBox1.Text;
            int len = test.Length;
           // MessageBox.Show(test);
           // MessageBox.Show(len.ToString());
            int n = 0;
            n = test[0] - '0';
            for (int i = 1; i <= n; i++)
            {
                Random rm = new Random();
                Point pt = new Point();
                pt = new Point(rm.Next(15, this.panel1.Width - 15), rm.Next(15, this.panel1.Height - 15));
                Label lb = new Label();
                lb.Text = $"{i}";
                lb.Name = $"Pointer{i}";
                lb.Location = new Point(pt.X, pt.Y);
                //DrawNode(g, cbrush, pCircle, rec, pt, lb.Text);
                //  this.panel1.Controls.Add(lb);
                pointers.Add(new Pointer
                {
                    X = pt.X,
                    Y = pt.Y,
                    Name = $"Pointer{i}",
                    Text = $"{i}"
                });
            }
            for (int i = 0; i <= pointers.Count; i++)
                for (int j = 0; j <= pointers.Count; j++)
                    matran[i, j] = 0;
            int h = 0;
            int k = 0;
            int tmp = 0;
            int num = 0;
            for (int i = 1; i < len; i++)
            {
                if (test[i] == ' ')
                {
                    if (tmp == 0)
                    {
                        k++;
                        continue;
                    }
                    matran[h, k] = tmp;
                    lp.Add(new Linepointer
                    {
                        pta = pointers[h],
                        ptb = pointers[k]
                    });
                    ed.Add(new Edge
                    {
                        x = h,
                        y = k,
                        w = tmp
                    });
                    k++;
                    tmp = 0;
                    continue;
                }
                if (test[i] >= '0' && test[i] <= '9')
                {
                    tmp *= 10;
                    tmp += test[i] - '0';
                    num++;
                    continue;
                }
                else
                {
                    if ((test[i] < 'a' || test[i] > 'z') && (test[i] < 'A' || test[i] > 'Z'))
                    {
                        if (tmp > 0)
                        {
                            matran[h, k] = tmp;
                            lp.Add(new Linepointer
                            {
                                pta = pointers[h],
                                ptb = pointers[k]
                            });
                            ed.Add(new Edge
                            {
                                x = h,
                                y = k,
                                w = tmp
                            });
                            tmp = 0;
                            num = 0;
                            h++;
                            k = 0;
                            continue;
                        }
                        if (tmp == 0 && num > 0)
                        {
                            h++;
                            k = 0;
                            num = 0;
                        }
                    }
                }
            }
            if (tmp > 0)
            {
                matran[h, k] = tmp;
                ed.Add(new Edge
                {
                    x = h,
                    y = k,
                    w = tmp
                });
                lp.Add(new Linepointer
                {
                    pta = pointers[h],
                    ptb = pointers[k]
                });
                tmp = 0;
            }
           // MessageBox.Show(pointers.Count.ToString());
           // MessageBox.Show(h.ToString());
            if (h != pointers.Count-1)
            {
                gotoclear();
                MessageBox.Show("LỖI NHẬP HỆ THỐNG!");
            }
            else
            {
                if (dothi>=1)
                    paintarrow();
                else
                    paint();
                /* MessageBox.Show(System.Text.Json.JsonSerializer.Serialize(pointers));
                 MessageBox.Show(System.Text.Json.JsonSerializer.Serialize(lp));
                 MessageBox.Show(System.Text.Json.JsonSerializer.Serialize(ed));*/
                paintmatrix();
            }
        }
        void gotoclear()
        {
            panel1.Enabled = true;
            panel1.Controls.Clear();
            ed.Clear();
            lp.Clear();
            pointers.Clear();
            line.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            label2.Text = "...";
            Array.Clear(a, 0, a.Length);
            Array.Clear(t, 0, t.Length);
            comboBox1.Items.Clear();
            matrix.Clear();
            comboBox2.Items.Clear();
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            dothi = 0;
            button7.Enabled = true;
            button8.Enabled = true;
        }
        public void paint()
        {
            var g = panel1.CreateGraphics();
            Brush cbrush = new SolidBrush(Color.Wheat);
            Brush cbrush1 = new SolidBrush(Color.Yellow);
            Pen pCircle = new Pen(Color.Black, 3.0F);
            Size sizeCircle = new Size(30, 30);
            Size sizeCircle1 = new Size(20, 20);
            Pen pline = new Pen(Color.Black, 3.0F);
            for (int i = 0; i < lp.Count; i++)
            {
                Thread.Sleep(100);
                DrawLine(g, pline, lp[i].pta, lp[i].ptb);
                
            }
            for (int i = 0; i < lp.Count; i++)
            {
                var td = new Pointer();
                td.X = (lp[i].pta.X + lp[i].ptb.X) / 2;
                td.Y = (lp[i].pta.Y + lp[i].ptb.Y) / 2;
                Point pt = new Point(td.X - 10, td.Y - 10);
                Rectangle rec = new Rectangle(pt, sizeCircle1);
                DrawNode(g, cbrush1, pCircle, rec, pt, matran[int.Parse(lp[i].pta.Text) - 1, int.Parse(lp[i].ptb.Text) - 1].ToString(), 20);
            }
            for (int i = 0; i < pointers.Count; i++)
            {
                Point pt = new Point(pointers[i].X - 15, pointers[i].Y - 15);
                Rectangle rec = new Rectangle(pt, sizeCircle);
                Thread.Sleep(100);
                DrawNode(g, cbrush, pCircle, rec, pt, pointers[i].Text,30);
            }
            pCircle.Dispose();
            pline.Dispose();
            
        }
        public void paintarrow()
        {
            /* Graphics line = CreateGraphics();
             Pen redPen = new Pen(Brushes.Red, 3.0F);
             redPen.EndCap = LineCap.ArrowAnchor;
             var g = panel1.CreateGraphics();
             line.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;*/
            var g = panel1.CreateGraphics();
            Brush cbrush = new SolidBrush(Color.Wheat);
            Brush cbrush1 = new SolidBrush(Color.Yellow);
            Pen pCircle = new Pen(Color.Black, 3.0F);
            Size sizeCircle = new Size(30, 30);
            Size sizeCircle1 = new Size(20, 20);
            Pen pline = new Pen(Color.Black, 3.0F);
            Pen plinep = new Pen(Color.Black, 5.0F);
           // pline.EndCap = LineCap.ArrowAnchor;
            double M_Pi = 3.14;
            for (int i = 0; i < lp.Count; i++)
            {
                Thread.Sleep(100);
                DrawLine(g, pline, lp[i].pta, lp[i].ptb);
            }
            for (int i = 0; i < lp.Count; i++)
            {                
                double xstart = (double)lp[i].pta.X;
                double ystart = (double)lp[i].pta.Y;
                double xend = (double)lp[i].ptb.X;
                double yend = (double)lp[i].ptb.Y;
                double x1, x2, x3, y1, y2, y3,x4,y4;
                double arrow_length = 30;
                double arrow_degrees = 0.3;
                double angle = Math.Atan2(yend - ystart, xend - xstart)+M_Pi;
                x1 = xend + arrow_length * Math.Cos(angle - arrow_degrees);
                y1 = yend + arrow_length * Math.Sin(angle - arrow_degrees);
                x2 = xend + arrow_length * Math.Cos(angle + arrow_degrees);
                y2 = yend + arrow_length * Math.Sin(angle + arrow_degrees);
                x3 = xend + 20 * Math.Cos(angle);
                y3 = yend + 20 * Math.Sin(angle);
                x4 = xend + 23 * Math.Cos(angle);
                y4 = yend + 23 * Math.Sin(angle);
                Pointer line1 = new Pointer();
                Pointer line2 = new Pointer();
                Pointer line3 = new Pointer();
                Pointer line4 = new Pointer();
                line1.X = (int)x1;
                line1.Y = (int)y1;
                line2.X = (int)x2;
                line2.Y = (int)y2;
                line3.X = (int)x3;
                line3.Y = (int)y3;
                line4.X = (int)x4;
                line4.Y = (int)y4;
                Thread.Sleep(100);
                 DrawLine(g, pline, line1, line2);
               // Thread.Sleep(300);
                DrawLine(g, pline, line2, line3);
               // Thread.Sleep(300);
                DrawLine(g, plinep, line1, line3);
                DrawLine(g, plinep, line4, line2);
                DrawLine(g, plinep, line4, line3);
                DrawLine(g, plinep, line4, line1);
            }
            for (int i = 0; i < lp.Count; i++)
            {
                var td = new Pointer();
                td.X = (lp[i].pta.X + lp[i].ptb.X) / 2;
                td.Y = (lp[i].pta.Y + lp[i].ptb.Y) / 2;
                td.X = (lp[i].ptb.X + td.X) / 2;
                td.Y = (lp[i].ptb.Y + td.Y) / 2;
                Point pt = new Point(td.X - 10, td.Y - 10);
                Rectangle rec = new Rectangle(pt, sizeCircle1);
                DrawNode(g, cbrush1, pCircle, rec, pt, matran[int.Parse(lp[i].pta.Text) - 1, int.Parse(lp[i].ptb.Text) - 1].ToString(), 20);
            }
            for (int i = 0; i < pointers.Count; i++)
            {
                Point pt = new Point(pointers[i].X - 15, pointers[i].Y - 15);
                Rectangle rec = new Rectangle(pt, sizeCircle);
                Thread.Sleep(100);
                DrawNode(g, cbrush, pCircle, rec, pt, pointers[i].Text,30);
            }
            pCircle.Dispose();
            plinep.Dispose();
        }
        public void paintback()
        {
            var g = panel1.CreateGraphics();
            Brush cbrush = new SolidBrush(Color.Wheat);
          
            Pen pCircle = new Pen(Color.Black, 3.0F);
            Size sizeCircle = new Size(30, 30);
            Brush cbrush1 = new SolidBrush(Color.Yellow);
            Size sizeCircle1 = new Size(20, 20);
            Pen pline = new Pen(Color.Black, 3.0F);
            for (int i = 0; i < lp.Count; i++)
            {
                DrawLine(g, pline, lp[i].pta, lp[i].ptb);
            }
            for (int i = 0; i < lp.Count; i++)
            {
                var td = new Pointer();
                td.X = (lp[i].pta.X + lp[i].ptb.X) / 2;
                td.Y = (lp[i].pta.Y + lp[i].ptb.Y) / 2;
                Point pt = new Point(td.X - 10, td.Y - 10);
                Rectangle rec = new Rectangle(pt, sizeCircle1);
                DrawNode(g, cbrush1, pCircle, rec, pt, matran[int.Parse(lp[i].pta.Text) - 1, int.Parse(lp[i].ptb.Text) - 1].ToString(), 20);
            }
            for (int i = 0; i < pointers.Count; i++)
            {
                Point pt = new Point(pointers[i].X - 15, pointers[i].Y - 15);
                Rectangle rec = new Rectangle(pt, sizeCircle);
                DrawNode(g, cbrush, pCircle, rec, pt, pointers[i].Text,30);
            }
            pCircle.Dispose();
            pline.Dispose();
        }
        public class Pointer
        {
            public int X { set; get; }
            public int Y { set; get; }
            public string Name { set; get; }
            public string Text { set; get; }
        }
        public class Linepointer
        {
            public Pointer pta { set; get; }
            public Pointer ptb { set; get; }
        }
        public class Edge
        {
            public int x { set; get; }
            public int y { set; get; }
            public int w { set; get; }

        }
        void paintmatrix()
        {
            matrix.Columns.Add("");
            for (int i = 1; i <= pointers.Count; i++)
            {
                matrix.Columns.Add(i.ToString());
            }
            string[] tmp = new string[pointers.Count + 1];
            for (int i = 1; i <= pointers.Count; i++)
            {

                tmp[0] = i.ToString();
                for (int j = 1; j <= pointers.Count; j++)
                    tmp[j] = matran[i - 1, j - 1].ToString();
                ListViewItem itm;
                itm = new ListViewItem(tmp);
                matrix.Items.Add(itm);
            }
            
        }
        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            panel1.Controls.Clear();
            ed.Clear();
            lp.Clear();
            pointers.Clear();
            line.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            label2.Text = "...";
            Array.Clear(a, 0, a.Length);
            Array.Clear(t, 0, t.Length);
            comboBox1.Items.Clear();
            matrix.Clear();
            comboBox2.Items.Clear();
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            dothi = 0;
            button7.Enabled = true;
            button8.Enabled = true;
            index = 0;
        }
        List<Linepointer> line = new List<Linepointer>();
        int[,] a = new int[101, 101];
        int[,] t = new int[101, 101];
        private void button2_Click(object sender, EventArgs e)
        {
            if (dothi >= 1)
                paintbackarrow();
            else
                paintback();
            label3.Enabled = true;
            label4.Enabled = true;
            button6.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            int len = pointers.Count;
            for (int i = 0; i < len; i++)
            {
                comboBox1.Items.Add((i + 1).ToString());
                comboBox2.Items.Add((i + 1).ToString());
                for (int j = 0; j < len; j++)
                {
                    t[i, j] = 0;
                    a[i, j] = 100011;
                }
            }
            foreach (var i in ed)
            {
                t[i.x, i.y] = i.y;
                a[i.x, i.y] = i.w;
            }
            for (int k = 0; k < len; k++)
            {
                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < len; j++)
                    {
                        if (a[i, j] > a[i, k] + a[k, j])
                        {
                            a[i, j] = a[i, k] + a[k, j];
                            t[i, j] = t[i, k];
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            line.Clear();
            textBox2.Clear();
            int case1 = int.Parse(comboBox1.Text) - 1;
            int case2 = int.Parse(comboBox2.Text) - 1;
            if (case1==case2)
            {
                MessageBox.Show("Cạnh bị trùng. Vui lòng thử lại!");
                textBox2.Clear();
                line.Clear();
                label2.Text = "...";
                if (dothi >= 1)
                    paintbackarrow();
                else
                    paintback();
                return;
            }
          //  MessageBox.Show(case1.ToString());
          //  MessageBox.Show(case2.ToString());
            label2.Text = a[case1, case2].ToString();
            int chay = 0;
            while (case1 != case2)
            {
                textBox2.Text += (case1 + 1).ToString() + "->";
                chay = case1;
               // MessageBox.Show(case1.ToString());
                case1 = t[case1, case2];
                //MessageBox.Show(case1.ToString());
                if (chay == case1 )
                {
                    MessageBox.Show("Không có đường đi");
                    textBox2.Clear();
                    line.Clear();
                    label2.Text = "...";
                    if (dothi >= 1)
                        paintbackarrow();
                    else
                        paintback();
                    return;
                }
                line.Add(new Linepointer
                {
                    pta = pointers[chay],
                    ptb = pointers[case1]
                });
            }
            textBox2.Text += (case2 + 1).ToString();
            line.Add(new Linepointer
            {
                pta = pointers[chay],
                ptb = pointers[case1]
            });
            int[] ktra = new int[pointers.Count];
            for (int i = 0; i < pointers.Count; i++)
                ktra[i] = 0;
            if (dothi >=1)
            {
                paintbackarrow();
                paintwayarrow(ktra);
            }
            else
            {
                paintback();
                paintway(ktra);
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            line.Clear();
            if (dothi >= 1)
                paintbackarrow();
            else
                paintback();
            ed.Sort(
                (p1, p2) =>
                {
                    if (p1.w == p2.w) return 0;
                    if (p1.w < p2.w) return -1;
                    return 1;
                }
                );
            int len = pointers.Count;
            int[] hang = new int[len];
            int[] check = new int[len];
            int[] ktra = new int[1001];
            for (int i = 0; i < len; i++)
            {
                ktra[i] = 0;
                hang[i] = 0;
                check[i] = i;
            }

            int res = 0;

            foreach (var i in ed)
            {
                int u = find(i.x, check);
                int v = find(i.y, check);
                if (u == v)
                    continue;
                res += i.w;
                line.Add(new Linepointer
                {
                    pta = pointers[i.x],
                    ptb = pointers[i.y]
                });
                if (hang[u] == hang[v]) hang[u]++;
                if (hang[u] > hang[v]) check[v] = u;
                else check[u] = v;
            }

            /*foreach (var i in line)
            {
                int a = int.Parse(i.pta.Text);
                int b = int.Parse(i.ptb.Text);
                if (ktra[a - 1] == 0)
                    ktra[a-1] = 1;
                if (ktra[b - 1] == 0)
                    ktra[b-1] = 1;
            }*/
            // MessageBox.Show(System.Text.Json.JsonSerializer.Serialize(line));
            if (dothi >= 1)
                paintwayarrow(ktra);
            else
                paintway(ktra);
            foreach (var i in line)
                textBox2.Text = textBox2.Text + ("{" + i.pta.Text + "," + i.ptb.Text + "}");
            label2.Text = (res.ToString());
        }
        void paintbackarrow()
        {
            Brush cbrush1 = new SolidBrush(Color.Yellow);
            Size sizeCircle1 = new Size(20, 20);
            var g = panel1.CreateGraphics();
            Brush cbrush = new SolidBrush(Color.Wheat);
            Pen pCircle = new Pen(Color.Black, 3.0F);
            Size sizeCircle = new Size(30, 30);
            Pen pline = new Pen(Color.Black, 3.0F);
            Pen plinep = new Pen(Color.Black, 5.0F);
            // pline.EndCap = LineCap.ArrowAnchor;
            double M_Pi = 3.14;
            for (int i = 0; i < lp.Count; i++)
            {
              //  Thread.Sleep(300);
                DrawLine(g, pline, lp[i].pta, lp[i].ptb);
            }
            for (int i = 0; i < lp.Count; i++)
            {
                double xstart = (double)lp[i].pta.X;
                double ystart = (double)lp[i].pta.Y;
                double xend = (double)lp[i].ptb.X;
                double yend = (double)lp[i].ptb.Y;
                double x1, x2, x3, y1, y2, y3, x4, y4;
                double arrow_length = 30;
                double arrow_degrees = 0.3;
                double angle = Math.Atan2(yend - ystart, xend - xstart) + M_Pi;
                x1 = xend + arrow_length * Math.Cos(angle - arrow_degrees);
                y1 = yend + arrow_length * Math.Sin(angle - arrow_degrees);
                x2 = xend + arrow_length * Math.Cos(angle + arrow_degrees);
                y2 = yend + arrow_length * Math.Sin(angle + arrow_degrees);
                x3 = xend + 20 * Math.Cos(angle);
                y3 = yend + 20 * Math.Sin(angle);
                x4 = xend + 23 * Math.Cos(angle);
                y4 = yend + 23 * Math.Sin(angle);
                Pointer line1 = new Pointer();
                Pointer line2 = new Pointer();
                Pointer line3 = new Pointer();
                Pointer line4 = new Pointer();
                line1.X = (int)x1;
                line1.Y = (int)y1;
                line2.X = (int)x2;
                line2.Y = (int)y2;
                line3.X = (int)x3;
                line3.Y = (int)y3;
                line4.X = (int)x4;
                line4.Y = (int)y4;
                //Thread.Sleep(300);
                DrawLine(g, pline, line1, line2);
                // Thread.Sleep(300);
                DrawLine(g, pline, line2, line3);
                // Thread.Sleep(300);
                DrawLine(g, plinep, line1, line3);
                DrawLine(g, plinep, line4, line2);
                DrawLine(g, plinep, line4, line3);
                DrawLine(g, plinep, line4, line1);
            }
            for (int i = 0; i < lp.Count; i++)
            {
                var td = new Pointer();
                td.X = (lp[i].pta.X + lp[i].ptb.X) / 2;
                td.Y = (lp[i].pta.Y + lp[i].ptb.Y) / 2;
                td.X = (lp[i].ptb.X + td.X) / 2;
                td.Y = (lp[i].ptb.Y + td.Y) / 2;
                Point pt = new Point(td.X - 10, td.Y - 10);
                Rectangle rec = new Rectangle(pt, sizeCircle1);
                DrawNode(g, cbrush1, pCircle, rec, pt, matran[int.Parse(lp[i].pta.Text) - 1, int.Parse(lp[i].ptb.Text) - 1].ToString(), 20);
            }
            for (int i = 0; i < pointers.Count; i++)
            {
                Point pt = new Point(pointers[i].X - 15, pointers[i].Y - 15);
                Rectangle rec = new Rectangle(pt, sizeCircle);
              //  Thread.Sleep(300);
                DrawNode(g, cbrush, pCircle, rec, pt, pointers[i].Text,30);
            }
            pCircle.Dispose();
            plinep.Dispose();
        }
        void paintwayarrow(int[] ktra)
        {
            var g = panel1.CreateGraphics();
            Brush cbrush = new SolidBrush(Color.LightYellow);
            Pen pline = new Pen(Color.Red, 3.0F);
            pline.DashStyle = DashStyle.Dash;
            Pen pclear = new Pen(Color.White, 3.0F);
            Size sizeCircle = new Size(30, 30);
            Pen pCircle = new Pen(Color.Black, 3.0F);
            Pen plinep = new Pen(Color.Red, 5.0F);
            Brush cbrush1 = new SolidBrush(Color.Red);
            Size sizeCircle1 = new Size(20, 20);
            double M_Pi = 3.14;
            foreach (var i in line)
            {
                Thread.Sleep(500); // xoa cac duong di 
                DrawLine(g, pclear, i.pta, i.ptb);
            }
            foreach (var i in line)
            {
                Thread.Sleep(500); // ve duong mau do
                DrawLine(g, pline, i.pta, i.ptb);
            }
            foreach (var i in line) // ve mui ten
            {
                double xstart = (double)i.pta.X;
                double ystart = (double)i.pta.Y;
                double xend = (double)i.ptb.X;
                double yend = (double)i.ptb.Y;
                double x1, x2, x3, y1, y2, y3, x4, y4;
                double arrow_length = 30;
                double arrow_degrees = 0.3;
                double angle = Math.Atan2(yend - ystart, xend - xstart) + M_Pi;
                x1 = xend + arrow_length * Math.Cos(angle - arrow_degrees);
                y1 = yend + arrow_length * Math.Sin(angle - arrow_degrees);
                x2 = xend + arrow_length * Math.Cos(angle + arrow_degrees);
                y2 = yend + arrow_length * Math.Sin(angle + arrow_degrees);
                x3 = xend + 20 * Math.Cos(angle);
                y3 = yend + 20 * Math.Sin(angle);
                x4 = xend + 23 * Math.Cos(angle);
                y4 = yend + 23 * Math.Sin(angle);
                Pointer line1 = new Pointer();
                Pointer line2 = new Pointer();
                Pointer line3 = new Pointer();
                Pointer line4 = new Pointer();
                line1.X = (int)x1;
                line1.Y = (int)y1;
                line2.X = (int)x2;
                line2.Y = (int)y2;
                line3.X = (int)x3;
                line3.Y = (int)y3;
                line4.X = (int)x4;
                line4.Y = (int)y4;
                //Thread.Sleep(300);
                DrawLine(g, pline, line1, line2);
                // Thread.Sleep(300);
                DrawLine(g, pline, line2, line3);
                // Thread.Sleep(300);
                DrawLine(g, plinep, line1, line3);
                DrawLine(g, plinep, line4, line2);
                DrawLine(g, plinep, line4, line3);
                DrawLine(g, plinep, line4, line1);
            }
            foreach (var i in line)
            {
                var td = new Pointer();
                td.X = (i.pta.X + i.ptb.X) / 2;
                td.Y = (i.pta.Y + i.ptb.Y) / 2;
                td.X = (i.ptb.X + td.X) / 2;
                td.Y = (i.ptb.Y + td.Y) / 2;
                Point pt = new Point(td.X - 10, td.Y - 10);
                Rectangle rec = new Rectangle(pt, sizeCircle1);
                Thread.Sleep(300); // vẽ trọng số của đường đi
                DrawNode(g, cbrush1, pCircle, rec, pt, matran[int.Parse(i.pta.Text) - 1, int.Parse(i.ptb.Text) - 1].ToString(), 20);
            }
            foreach (var i in line) // vẻ cái node cho sáng thôi
            {
                int a = int.Parse(i.pta.Text);
                int b = int.Parse(i.ptb.Text);
                if (ktra[a - 1] == 0)
                {
                    Point ptCircle = new Point(i.pta.X - 15, i.pta.Y - 15);
                    Rectangle rec = new Rectangle(ptCircle, sizeCircle);
                    Thread.Sleep(150);
                    DrawNode(g, cbrush, pCircle, rec, ptCircle, pointers[a - 1].Text,30); // vẻ cái node đỉnh ra
                    ktra[a - 1] = 1;
                }
                if (ktra[b - 1] == 0)
                {
                    Point ptCircle = new Point(i.ptb.X - 15, i.ptb.Y - 15);
                    Rectangle rec = new Rectangle(ptCircle, sizeCircle);
                    Thread.Sleep(150);
                    DrawNode(g, cbrush, pCircle, rec, ptCircle, pointers[b - 1].Text,30);
                    ktra[b - 1] = 1;
                }
            }
            pline.Dispose();
            pclear.Dispose();
            pCircle.Dispose();
        }
        void paintway(int[] ktra)
        {
            var g = panel1.CreateGraphics();
            Brush cbrush = new SolidBrush(Color.LightYellow);
            Pen pline = new Pen(Color.Red, 3.0F);
            pline.DashStyle = DashStyle.Dash;
            Pen pclear = new Pen(Color.White, 3.0F);
            Size sizeCircle = new Size(30, 30);
            Pen pCircle = new Pen(Color.Black, 3.0F);
            Brush cbrush1 = new SolidBrush(Color.Red);
            Size sizeCircle1 = new Size(20, 20);
            foreach (var i in line)
            {
                Thread.Sleep(500); // xoa đường đi ra 
                DrawLine(g, pclear, i.pta, i.ptb);
            }
            foreach (var i in line)
            {
                Thread.Sleep(500); // vẻ đường đi màu đỏ
                DrawLine(g, pline, i.pta, i.ptb);
            }
            foreach(var i in line)
            {
                var td = new Pointer();
                td.X = (i.pta.X + i.ptb.X) / 2;
                td.Y = (i.pta.Y + i.ptb.Y) / 2;
                Point pt = new Point(td.X - 10, td.Y - 10);
               
                Rectangle rec = new Rectangle(pt, sizeCircle1); // vẻ lại trọng số
                DrawNode(g, cbrush1, pCircle, rec, pt, matran[int.Parse(i.pta.Text) - 1, int.Parse(i.ptb.Text) - 1].ToString(), 20);
            }
            foreach (var i in line)
            {
                int a = int.Parse(i.pta.Text);
                int b = int.Parse(i.ptb.Text);
                if (ktra[a - 1] == 0)
                {
                    Point ptCircle = new Point(i.pta.X - 15, i.pta.Y - 15);
                    Rectangle rec = new Rectangle(ptCircle, sizeCircle);
                    Thread.Sleep(150); // vẻ cái node ra
                    DrawNode(g, cbrush, pCircle, rec, ptCircle, pointers[a - 1].Text,30);
                    ktra[a - 1] = 1;
                }
                if (ktra[b - 1] == 0)
                {
                    Point ptCircle = new Point(i.ptb.X - 15, i.ptb.Y - 15);
                    Rectangle rec = new Rectangle(ptCircle, sizeCircle);
                    Thread.Sleep(150);
                    DrawNode(g, cbrush, pCircle, rec, ptCircle, pointers[b - 1].Text,30);
                    ktra[b - 1] = 1;
                }
            }
            pline.Dispose();
            pclear.Dispose();
            pCircle.Dispose();
        }
        int find(int a, int[] check)
        {
            if (a == check[a]) return a;
            return a = find(check[a], check);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dothi++;
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dothi = 0;
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
