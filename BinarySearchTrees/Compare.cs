using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BSTs
{
    public class Compare
    {
        Stopwatch stopwatch = new();
        // all the methods are overloaded, one for each: AVL, RB and normal BST
        public TimeSpan get_insert_time(BinarySearchTree<long> bst, long range)
        {
            stopwatch.Restart();
            for (int i = 0; i < range; i++)
            {
                bst.insert(i);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public TimeSpan get_delete_time(BinarySearchTree<long> bst, long range)
        {
            stopwatch.Restart();
            for (int i = 0; i < range; i++)
            {
                bst.delete(i);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public TimeSpan get_lookup_time(BinarySearchTree<long> bst, long value)
        {
            stopwatch.Restart();
            bst.contains(value);
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public TimeSpan get_insert_time(AvlTree<long> avl, long range)
        {
            stopwatch.Restart();
            for (int i = 0; i < range; i++)
            {
                avl.insert(i);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public TimeSpan get_delete_time(AvlTree<long> avl, long range)
        {
            stopwatch.Restart();
            for (int i = 0; i < range; i++)
            {
                avl.delete(i);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public TimeSpan get_insert_time(RedBlackTree<long> rb, long range)
        {
            stopwatch.Restart();
            for (int i = 0; i < range; i++)
            {
                rb.insert(i);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public TimeSpan get_delete_time(RedBlackTree<long> rb, long range)
        {
            stopwatch.Restart();
            for (int i = 0; i < range; i++)
            {
                rb.delete(i);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public TimeSpan get_lookup_time(RedBlackTree<long> rb, long value)
        {
            stopwatch.Restart();
            rb.contains(value);
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public TimeSpan get_lookup_time(AvlTree<long> avl, long value)
        {
            stopwatch.Restart();
            avl.contains(value);
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public TimeSpan compare_insert(AvlTree<long> avl, RedBlackTree<long> rb, long range)
        {
            TimeSpan avl_time = get_insert_time(avl, range);
            TimeSpan rb_time = get_insert_time(rb, range);
            Console.WriteLine($"AVL tree insert time: {avl_time}");
            Console.WriteLine($"Red-Black Tree insert time: {rb_time}");
            return avl_time - rb_time;
        }
        public TimeSpan compare_lookup(AvlTree<long> avl, RedBlackTree<long> rb, long range)
        {
            TimeSpan avl_time = new(0, 0, 0, 0, 0, 0);
            TimeSpan rb_time = new(0, 0, 0, 0, 0, 0);
            Random rand = new();
            for (long i = 0; i < range; i++)
            {
                long value = rand.NextInt64(range);
                avl_time += get_lookup_time(avl, value);
                rb_time += get_lookup_time(rb, value);

            }
            Console.WriteLine($"AVL tree lookup time: {avl_time}");
            Console.WriteLine($"Red-Black Tree lookup time: {rb_time}");
            return avl_time - rb_time;
        }
        public TimeSpan compare_delete(AvlTree<long> avl, RedBlackTree<long> rb, long range)
        {
            TimeSpan avl_time = get_delete_time(avl, range);
            TimeSpan rb_time = get_delete_time(rb, range);
            Console.WriteLine($"AVL tree delete time: {avl_time}");
            Console.WriteLine($"Red-Black Tree delete time: {rb_time}");
            return avl_time - rb_time;
        }
        public void compare_lookup_altogether(AvlTree<long> avl, RedBlackTree<long> rb, BinarySearchTree<long> bst, long range)
        {
            TimeSpan avl_time = new(0, 0, 0, 0, 0, 0);
            TimeSpan rb_time = new(0, 0, 0, 0, 0, 0);
            TimeSpan bst_time = new(0, 0, 0, 0, 0, 0);
            Random rand = new();
            for (long i = 0; i < range; i++)
            {
                long value = rand.NextInt64(range);
                avl_time += get_lookup_time(avl, value);
                rb_time += get_lookup_time(rb, value);
                bst_time += get_lookup_time(bst, value);
            }
            Console.WriteLine($"AVL tree lookup time: {avl_time}");
            Console.WriteLine($"Red-Black Tree lookup time: {rb_time}");
            Console.WriteLine($"Normal BST lookup time: {bst_time}");
        }
        public void compare_altogether(AvlTree<long> avl, RedBlackTree<long> rb, BinarySearchTree<long> bst, long range)
        {
            Console.WriteLine($"Normal BST insert time: {get_insert_time(bst, range)}");
            compare_insert(avl, rb, range);
            Console.WriteLine();
            compare_lookup_altogether(avl, rb, bst, range);
            Console.WriteLine();
            Console.WriteLine($"Normal BST delete time: {get_delete_time(bst, range)}");
            compare_delete(avl, rb, range);
            Console.WriteLine();
        }
        public void compare_balanced_trees(AvlTree<long> avl, RedBlackTree<long> rb, long range)
        {
            TimeSpan insert_diff = compare_insert(avl, rb, range);
            Console.WriteLine($"Insert time difference (AVL - RB): {insert_diff} \n");
            TimeSpan lookup_diff = compare_lookup(avl, rb, range);
            Console.WriteLine($"Lookup time difference (AVL - RB): {lookup_diff} \n");
            TimeSpan delete_diff = compare_delete(avl, rb, range);
            Console.WriteLine($"Delete time difference (AVL - RB): {delete_diff}  \n");
        }
    }
}
