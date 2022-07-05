using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FractalSharp
{
    public class BinaryTreeNode<T> where T : IComparable
    {
        public BinaryTreeNode(T data1, T data2, T data3)
        {
            Data = new List<T>();
        }

        public BinaryTreeNode() 
        { 
            Data = new List<T>(); 
        }
        public List<T> Data;
        public int counter = 0;
        public int size = 0;
        public List<BinaryTreeNode<T>> children;
        public BinaryTreeNode<T> ParentNode;
        public Color nodeColor;
    }
    public class BinaryTree<T> where T: IComparable
    {
        public BinaryTreeNode<T> RootNode = new BinaryTreeNode<T>();

        public List<List<BinaryTreeNode<T>>> treeLayers;
        public List<int> pointsFilled = new List<int>();

        public void buildEmptyTree(int layers)
        {
            treeLayers = new List<List<BinaryTreeNode<T>>>();
            treeLayers.Add(new List<BinaryTreeNode<T>>());
            RootNode.children = new List<BinaryTreeNode<T>>();
            treeLayers[0].Add(RootNode); // Layer 0
            pointsFilled.Add(0);

            for (int lay = 1; lay <= layers; lay++)
            {
                treeLayers.Add(new List<BinaryTreeNode<T>>());
                pointsFilled.Add(0);

                int layerSize = (int)Math.Pow(4, lay);
                for (int j = 0; j < layerSize; j++)
                {
                    BinaryTreeNode<T> newNode = new BinaryTreeNode<T>();
                    newNode.children = new List<BinaryTreeNode<T>>();
                    treeLayers[lay].Add(newNode);                   
                }

                int counter = 0;
                for (int k = 0; k < layerSize; k += 4)
                {
                    for (int n = 0; n < 4; n++)
                    {
                        treeLayers[lay - 1][counter].children.Add(treeLayers[lay][k + n]);
                        Console.WriteLine("Added children");
                        treeLayers[lay][k + n].ParentNode = treeLayers[lay - 1][counter];
                    }
                    counter++;
                }
            }
        }

    }
}
