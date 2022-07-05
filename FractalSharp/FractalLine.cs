using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace FractalSharp
{
    public partial class FractalLine : Form
    {
        public FractalLine()
        {
            InitializeComponent();
        }

        int lx = 5, ly = 5;
        int X =0, Y = 0;
        int layersNumber = 0;
        
        BinaryTree<Tuple<Point, Point>> tree;
        Pen pen = new Pen(Color.Black);
        List<Color> layersColors;
        private void FractalLine_Load(object sender, EventArgs e)
        {

        }
        private void DrawPart(int lx, int ly, int layer)
        {
            layer = layersNumber - layer;

            if (layer == 0)
            {
                tree.treeLayers[0][0].Data.Add(new Tuple<Point, Point>(new Point(X, Y), new Point(X + lx, Y + ly)));

                X = X + lx;
                Y = Y + ly;
            } else
            {
                int counter =tree.pointsFilled[layer];
                tree.treeLayers[layer][counter].Data.Add(new Tuple<Point, Point>(new Point(X, Y), new Point(X + lx, Y + ly)));
                if(tree.treeLayers[layer][counter].Data.Count == 3)
                {
                    tree.pointsFilled[layer]++;
                }

                X = X + lx;
                Y = Y + ly;
            }

            pictureBox1.Refresh();
        }
        void up(int i)
        {
            if (i >= 0)
            {
                left(i - 1);
                DrawPart(0, +ly, i);
                up(i - 1);
                DrawPart(lx, 0, i);
                up(i - 1);
                DrawPart(0, -ly, i);
                right(i - 1);
            }
            pictureBox1.Refresh();
        }
        void down(int i)
        {
            if (i >= 0)
            {
                right(i - 1);
                DrawPart(0, -ly, i);
                down(i - 1);
                DrawPart(-lx, 0, i);
                down(i - 1);
                DrawPart(0, ly, i);
                left(i - 1);
            }
            pictureBox1.Refresh();
        }
        void right(int i)
        {
            if (i >= 0)
            {
                down(i - 1);
                DrawPart(-lx, 0, i);
                right(i - 1);
                DrawPart(0, -ly, i);
                right(i - 1);
                DrawPart(lx, 0, i);
                up(i - 1);
            }
            pictureBox1.Refresh();
        }
        void left(int i)
        {
            if (i >= 0)
            {
                up(i - 1);
                DrawPart(lx, 0, i);
                left(i - 1);
                DrawPart(0, ly, i);
                left(i - 1);
                DrawPart(-lx, 0, i);
                down(i - 1);
            }
            pictureBox1.Refresh();
        }

        private int fracrtalSize(int layer)
        {
            if(layer == 0)
            {
                return ly;
            } else
            {
                return ly + 2*fracrtalSize(layer-1);
            }
        }
        private void buildFractalSteps(int x, int y)
        {
            X = x;
            Y = y;

            for(int i = 0; i <= layersNumber; i++)
            {
                Color newColor = layersColors[0];
                pen.Color = newColor;
                layersColors.Add(newColor);
                buildFractal(createFractalTree(i), X, Y);
                Y = Y + fracrtalSize(i) + 20;
            }
        } 
        private void buildFractal(List<List<BinaryTreeNode<Tuple<Point, Point>>>> treeLayers, int x, int y)
        {
           
            Graphics G = pictureBox1.CreateGraphics();
            foreach (List<BinaryTreeNode<Tuple<Point, Point>>> list in treeLayers)
            {
                foreach (BinaryTreeNode<Tuple<Point, Point>> node in list)
                {
                    for(int i = 0; i<3;i++)
                    {
                        Point p1 = new Point(node.Data[i].Item1.X + x, node.Data[i].Item1.Y + y);
                        Point p2 = new Point(node.Data[i].Item2.X + x, node.Data[i].Item2.Y + y);
                        
                        Pen colorPen = new Pen(node.nodeColor);
                        G.DrawLine(colorPen, p1, p2);
                       // Console.WriteLine("X: " + X);
                    }
                    Thread.Sleep(70);
                }
            }
        }

        int firstNodeNum(int treelayer)
        {
            if (treelayer == 1)
            {
                return 0;
            } else
            {
                return (int)Math.Pow(4, treelayer-1) + firstNodeNum(treelayer-1);
            }
        }

        private List<List<BinaryTreeNode<Tuple<Point, Point>>>> createFractalTree(int layer)
        {
            int treelayer = layersNumber - layer;
            Graphics G = pictureBox1.CreateGraphics();

            if(treelayer == 0)
            {
                return tree.treeLayers;
            } else
            {
                int offset = 0;
                for (int i = layer; i < layersNumber; i++)
                {
                    offset = offset + ly + fracrtalSize(i);
                }

            

                Console.WriteLine("offset = " + offset);

                List<List<BinaryTreeNode<Tuple<Point, Point>>>> treeLayers = new List<List<BinaryTreeNode<Tuple<Point, Point>>>>();
                int counter = 0;
                int secondNode = firstNodeNum(treelayer)+1;
                treeLayers.Add(new List<BinaryTreeNode<Tuple<Point, Point>>>());
                BinaryTreeNode<Tuple<Point, Point>> origianlNode = tree.treeLayers[treelayer][secondNode];

                BinaryTreeNode<Tuple<Point, Point>> newNode = new BinaryTreeNode<Tuple<Point, Point>>();
                for(int n = 0; n<3; n++)
                {
                    newNode.Data.Add(new Tuple<Point, Point>(
                        new Point(origianlNode.Data[n].Item1.X, origianlNode.Data[n].Item1.Y - offset), 
                        new Point(origianlNode.Data[n].Item2.X, origianlNode.Data[n].Item2.Y - offset)));
                }
                newNode.nodeColor = origianlNode.nodeColor;

                newNode.children = origianlNode.children;

                treeLayers[counter].Add(newNode);
                counter++;
                List<BinaryTreeNode<Tuple<Point, Point>>> currNodes = new List<BinaryTreeNode<Tuple<Point, Point>>>();
                currNodes.Add(newNode);



                while (currNodes[0].children.Count !=0)
                {
                    Console.WriteLine("Count = " + currNodes[0].children.Count);
                    List<BinaryTreeNode<Tuple<Point, Point>>> newCurrNodes = new List<BinaryTreeNode<Tuple<Point, Point>>>();
                    foreach (BinaryTreeNode<Tuple<Point, Point>> node in currNodes)
                    {
                        foreach (BinaryTreeNode<Tuple<Point, Point>> childNode in node.children)
                        {
                            treeLayers.Add(new List<BinaryTreeNode<Tuple<Point, Point>>>());

                            BinaryTreeNode<Tuple<Point, Point>> newChildNode = new BinaryTreeNode<Tuple<Point, Point>>();

                            for (int n = 0; n < 3; n++)
                            {
                                newChildNode.Data.Add(new Tuple<Point, Point>(
                                    new Point(childNode.Data[n].Item1.X, childNode.Data[n].Item1.Y -offset),
                                    new Point(childNode.Data[n].Item2.X, childNode.Data[n].Item2.Y - offset)));
                            }
                            newChildNode.children = childNode.children;
                            newChildNode.nodeColor = childNode.nodeColor;

                            treeLayers[counter].Add(newChildNode);
                            newCurrNodes.Add(newChildNode);
                        }
                            
                    }
                    currNodes = newCurrNodes;
                    counter++;
                }
                
                return treeLayers;
            }  
        }

        private void drawTree(List<List<BinaryTreeNode<Tuple<Point, Point>>>> tree)
        {
            int size = 50;
            int offsetX = 40;
            int offsetY = 40;
            int x = 0;
            int y = 0;
            Graphics G = pictureBox2.CreateGraphics();
            G.Clear(BackColor);
            Pen blackpen = new Pen(Color.Black);
            Font drawFont = new Font("Arial", size/4);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            Color newColor;
            Random random = new Random();

        // Color tree nodes
        for (int i = 0; i <= layersNumber; i++)
            {                      
                newColor = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));      
                layersColors.Add(newColor);
                

                if (i == 0)
                {
                    foreach (List<BinaryTreeNode<Tuple<Point, Point>>> layersNumber in tree)
                    {
                        foreach (BinaryTreeNode<Tuple<Point, Point>> node in layersNumber)
                        {
                            node.nodeColor = newColor;
                        }
                    }
                } else
                {
                    int secondNode = firstNodeNum(i) + 1;
                    BinaryTreeNode<Tuple<Point, Point>> origianlNode = tree[i][secondNode];
                    tree[i][secondNode].nodeColor = newColor;
                    


                    List<BinaryTreeNode<Tuple<Point, Point>>> currNodes = new List<BinaryTreeNode<Tuple<Point, Point>>>();
                    currNodes.Add(origianlNode);

                    while (currNodes[0].children.Count != 0)
                    {
                        List<BinaryTreeNode<Tuple<Point, Point>>> newCurrNodes = new List<BinaryTreeNode<Tuple<Point, Point>>>();
                        foreach (BinaryTreeNode<Tuple<Point, Point>> node in currNodes)
                        {
                            foreach (BinaryTreeNode<Tuple<Point, Point>> childNode in node.children)
                            {

                                childNode.nodeColor = newColor;

                                newCurrNodes.Add(childNode);
                            }

                        }
                        currNodes = newCurrNodes;
                    }

                }
        }


 

        List<Point> prevBottomPoints = new List<Point>();

            for (int i = 0; i <= layersNumber; i++)
            {
                int nodesCount = (int)Math.Pow(4, i);

                int counter = 0;
                List<Point> newPrevBottomPoints = new List<Point>();

                for (int node = 0; node < nodesCount; node++)
                {
                    Pen newPen = new Pen(tree[i][node].nodeColor);
                    newPen.Width = 2;

                    G.DrawEllipse(newPen, x, y, size, size);

                    G.DrawString(tree[i][node].Data[0].Item1.X.ToString() + " " + tree[i][node].Data[0].Item1.Y.ToString() + " " +
                        tree[i][node].Data[0].Item2.X.ToString() + " " + tree[i][node].Data[0].Item2.Y.ToString()
                        , drawFont, drawBrush, new Point(x, y + size/7));

                    G.DrawString(tree[i][node].Data[1].Item1.X.ToString() + " " + tree[i][node].Data[1].Item1.Y.ToString() + " " + 
                        tree[i][node].Data[1].Item2.X.ToString() + " " + tree[i][node].Data[1].Item2.Y.ToString()
                        , drawFont, drawBrush, new Point(x, y + 2*size / 7 + 3));


                    G.DrawString(tree[i][node].Data[2].Item1.X.ToString() + " " + tree[i][node].Data[2].Item1.Y.ToString() + " " +
                        tree[i][node].Data[2].Item2.X.ToString() + " " + tree[i][node].Data[2].Item2.Y.ToString()
                        , drawFont, drawBrush, new Point(x, y + 4*size / 7 ));

                    newPrevBottomPoints.Add(new Point(x + size/2, y + size));

                    if (i != 0)
                    {
                        G.DrawLine(blackpen, new Point(x + size/2, y), prevBottomPoints[counter/4]);
                    }
                    counter++;

                    x = x + size + offsetX;
                }

                prevBottomPoints = newPrevBottomPoints;

                x = 0;
                y = y + size + offsetY;
            }

           

            

            

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            X = 0;
            Y = 0;
            layersNumber = Convert.ToInt32(numericUpDown1.Value);
           layersColors = new List<Color>(); 

            tree = new BinaryTree<Tuple<Point, Point>>();
            tree.buildEmptyTree(layersNumber);

            up(layersNumber);

            drawTree(tree.treeLayers);  

            buildFractalSteps(0, 0);

           
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
