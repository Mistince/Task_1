using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

class Program
{
    //if you want to disable my incredible intro, comment out line 84. I thought it was funny though, hopefully brings you some amusement too
    public class node
    {

        public node left;
        public node right;
        public int key;



    }

    static void Main(string[] args)
    {
        Print("Creating BST and AVL trees and timing...");
        Stopwatch stopwatch1 = new Stopwatch();
        Stopwatch stopwatch2 = new Stopwatch();
        stopwatch1.Start();

        node bstRoot = null;
        node avlRoot = null;

        int size = 150000;
        int[] BSTarray = new int[size];
        int[] AVLarray = new int[size];
        Random random = new Random();
        for (int i = 0; i < size; i++)
        {
            BSTarray[i] = random.Next(size);
            AVLarray [i] = random.Next(size);
        }

        BSTTree bstTree = new BSTTree();
        AVLTree avlTree = new AVLTree();


        stopwatch1.Start();
        for (int i = 0; i < size; i++)
        {
            avlRoot = avlTree.insertion(avlRoot, AVLarray[i]);
        }
        stopwatch1.Stop();
        stopwatch2.Start();
        for (int i = 0; i < size; i++)
        {
            bstRoot = bstTree.insertion(bstRoot, BSTarray[i]);
        }
         stopwatch2.Stop();

        Console.WriteLine("Time taken to generate 150000 size avl tree: {0:hh\\:mm\\:ss}", stopwatch1.Elapsed);
        Console.WriteLine("Time taken to generate 150000 size bst tree: {0:hh\\:mm\\:ss}", stopwatch2.Elapsed);



        if (avlRoot != null || bstRoot != null)
        {
            mainmenu(avlRoot, bstRoot); 
        }
        else
        {
            Console.WriteLine("AVL OR BST ROOT IS NULL");
        }
        
    }


    public static void mainmenu(node BSTroot, node AVLroot)
    {
        bool mainmenu = true;
        while (mainmenu == true){
            Print("                  Welcome to \n                    Mist's \n                   Sorting \n                   Maddness \n ");
           Print("                  #############                    \r\n              #####################                \r\n            #########################              \r\n          #############################            \r\n         ###############################           \r\n         ############       ############           \r\n        ###########          ############          \r\n        ###########           ###########          \r\n        ##########            ###########          \r\n                              ###########          \r\n                             ###########           \r\n                            ############           \r\n                           ############            \r\n                         #############             \r\n                       ##############              \r\n                     ##############                \r\n                   ###############                 \r\n                 ###############                   \r\n               ###############                     \r\n              ##############                       \r\n            ##############                         \r\n           #############                           \r\n          ##############################           \r\n         ################################          \r\n        #################################          \r\n       ##################################          \r\n       ##################################          \r\n      ###################################          ", delay: 1);
           Print("(now with multiple sorting methods and a randomly generated dataset) wow!");
            Print("1) Search for a number all data structures \n 2) Exit");
            var userInput = Console.ReadLine();
            switch (userInput)
            {
                case "1":
                    searchForUserInput(BSTroot, AVLroot);
                    break;
                case "2":
                    mainmenu = false;
                    break;
                default:
                    break;
            }
        }
    }

    public static void searchForUserInput(node BSTroot, node AVLroot)
    {
        Print("Please enter the number you wish to search for:");
        if (int.TryParse(Console.ReadLine(), out int searchNumber)) //convert user input into  int
        {
            Stopwatch BSTwatch = new Stopwatch();
            Stopwatch AVLwatch = new Stopwatch();

            BSTwatch.Start();
            node BSTresult = BSTTree.Search(BSTroot, searchNumber);
            BSTwatch.Stop();

            AVLwatch.Start();
            node AVLresult = AVLTree.Search(AVLroot, searchNumber);
            AVLwatch.Stop();

            if (BSTresult == null)
                Console.WriteLine(searchNumber + " not found in BST");
            else
                Console.WriteLine(searchNumber + " found in BST");

            Console.WriteLine($"Time taken to search in BST: {BSTwatch.Elapsed}");

            if (AVLresult == null)
                Console.WriteLine(searchNumber + " not found in AVL");
            else
                Console.WriteLine(searchNumber + " found in AVL");

            Console.WriteLine($"Time taken to search in AVL: {AVLwatch.Elapsed}");
        }
    }





    class BSTTree 
    {

        public virtual node insertion(node root, int key)
        {
            if (root == null) //if the root doesn't exist, make one with the current value
            {
                root = new node();
                root.key = key;
            }
            else if (key > root.key)  //if root isnt null, and ur bigger then the parent, go to the right
            {

                root.right = insertion(root.right, key);
            }
            else if (key < root.key) //if ur smaller then the parent, go to the left
            {

                root.left = insertion(root.left, key);
            }

            

            return root; 
        }

        public static node Search(node root, int key)
        {
            // Base Cases: root is null or key is present at root
            if (root == null || root.key == key)
                return root;

            // Key is greater than root's key
            if (root.key < key)
                return Search(root.right, key);

            // Key is smaller than root's key
            return Search(root.left, key);
        }

        public virtual void traverse(node root)
        {
            if (root == null)
            {
                return;
            }
            Console.Write(root.key + " ");
            traverse(root.left);
            traverse(root.right);
        }


    }
    class AVLTree : BSTTree
    {


        public override node insertion(node root, int key)
        {


            if (root == null) //if the root doesn't exist, make one with the current value
            {
                root = new node();
                root.key = key;
            }
            else if (key > root.key)  //if root isnt null, and ur bigger then the parent, go to the right
            { 
                root.right = insertion(root.right, key);
            }
            else if (key < root.key) //if ur smaller then the parent, go to the left
            {
                root.left = insertion(root.left, key);
  
            }

            root = balanceTree(root);
           return root;
        }



        public override void traverse(node root)
        {
            if (root == null)
            {
                return;
            }
            Console.Write(root.key + " ");
            traverse(root.left);
            traverse(root.right);
        }

        public int findheight(node root) //another recursion method. if no root return -1 to show no height. 
        {
            int height = 0;
            if (root != null)
            {
                int leftheight = findheight(root.left);
                int rightheight = findheight(root.right);
                int h = Math.Max(leftheight,rightheight); //return the larger height 
                height = h + 1;
            }
            return height;


        }

        public int CalcBalance(node root)
        {
            int leftH = findheight(root.left); //calc height of left subtree
            int rightH = findheight(root.right); //calc height of right subtree
            int b_factor = leftH - rightH; 
            return b_factor;
        }
        private node balanceTree(node root)
        {

            int b_Factor =  CalcBalance(root); // get balance factor of tree
            if (b_Factor > 1)
            {
                if (CalcBalance(root.left) > 0) //check left tree's height
                {
                    root = rotateLL(root);
                }
                else if (root.left == null)
                {
                    Console.WriteLine(" root is null in balance tree left height");
                }
                else
                {
                    root = rotateLR(root);
                }
            }
            else if (b_Factor < -1)
            {
                if (CalcBalance(root.right) > 0)
                {
                    root = rotateRL(root);
                }
                else if (root.right == null)
                {
                    Console.WriteLine(" root is null in balance tree right height");
                }
                else
                {
                    root = rotateRR(root);
                }
            }
            return root;
        }

        private node rotateRR(node parent)
        {
            node pivot = parent.right;
            parent.right = pivot.left;
            pivot.left = parent;
            Console.WriteLine("Returning RR Pivot");
            return pivot;
        }
        private node rotateLL(node parent)
        {
            node pivot = parent.left;
            parent.left = pivot.right;
            pivot.right = parent;
            Console.WriteLine("Returning LL Pivot");
            return pivot;
        }
        private node rotateLR(node parent)
        {
            node pivot = parent.left;
            parent.left = rotateRR(pivot);
            Console.WriteLine("Returning LR parent");
            return rotateLL(parent);
        }
        private node rotateRL(node parent)
        {
            node pivot = parent.right;
            parent.right = rotateLL(pivot);
            Console.WriteLine("Returning RR parent");
            return rotateRR(parent);
        }
    }


    public static void Print(string text, int delay = 20) //type writer effect
    {
        foreach (var c in text)
        {
            Console.Write(c);
            System.Threading.Thread.Sleep(delay);
        }
        Console.WriteLine();

    }
}
