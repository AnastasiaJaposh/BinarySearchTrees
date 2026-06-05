using BSTs;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

public class BinarySearchTree<T> : IBinarySearchTree<T> where T : IComparable<T>
{
    public class Node
    {
        public T? value { get; set; }
        public Node? left_child { get; set; }
        public Node? right_child { get; set; }

        public Node(T value, Node? left_child = null, Node? right_child = null)
        {
            this.value = value;
            this.left_child = left_child;
            this.right_child = right_child;
        }
    }

    public Node? root { get; set; }


    public void insert(T value)
    {

        Node insert_node = new(value);
        Node? r = root;
        if (r == null) { root = insert_node; return; }

        while (true)
        {

            int comparison_result = value.CompareTo(r.value);
            if (comparison_result == 0) { return; }
            else if (comparison_result < 0) // value is smaller than current node value
            {
                if (r.left_child == null) { r.left_child = insert_node; return; }
                else { r = r.left_child; }
            }
            else // value is bigger than current node value
            {
                if (r.right_child == null) { r.right_child = insert_node; return; }
                else { r = r.right_child; }
            }

        }

    }

    public void delete(T value)
    {

        Node? r = root;
        Node? parent = null;
        bool is_left_child = false;
        if (r == null) { throw new BTSException("Cannot delete from an empty tree"); }

        while (true)
        {
            int comparison_result = value.CompareTo(r.value);

            if (comparison_result == 0)
            {
                if (r.left_child == null && r.right_child == null)
                {
                    if (parent == null) { root = null; }
                    else if (is_left_child) { parent.left_child = null; }
                    else { parent.right_child = null; }
                    return;
                }
                else if (r.left_child == null)
                {
                    if (parent == null) { root = r.right_child; }
                    else if (is_left_child) { parent.left_child = r.right_child; }
                    else { parent.right_child = r.right_child; }
                    return;
                }
                else if (r.right_child == null)
                {
                    if (parent == null) { root = r.left_child; }
                    else if (r.Equals(parent.left_child)) { parent.left_child = r.left_child; }
                    else { parent.right_child = r.left_child; }
                    return;
                }
                else // replace r with minimum in its right subtree
                {
                    Node p = r;
                    r = r.right_child;
                    T? replacement_value;
                    if (r.left_child == null)
                    {
                        replacement_value = r.value;
                        p.right_child = r.right_child;
                    }
                    else
                    {
                        while (r.left_child != null)
                        {
                            p = r;
                            r = r.left_child;
                        }
                        replacement_value = r.value;
                        p.left_child = r.right_child;
                    }

                    if (parent == null) { root!.value = replacement_value; }
                    else if (is_left_child) { parent.left_child!.value = replacement_value; }
                    else { parent.right_child!.value = replacement_value; }
                }
            }
            else if (comparison_result < 0) // value is smaller than current node value
            {
                if (r.left_child == null) { throw new BTSException("Value is not present in the tree"); }
                else { parent = r; r = r.left_child; is_left_child = true; }
            }
            else // value is bigger than current node value
            {
                if (r.right_child == null) { throw new BTSException("Value is not present in the tree"); }
                else { parent = r; r = r.right_child; is_left_child = true; }
            }

        }
    }

    public bool contains(T value)
    {
        Node? r = root;
        if (r == null)
        {
            return false;
        }

        while (r != null)
        {
            int comparison_result = value.CompareTo(r.value);
            if (comparison_result == 0)
            {
                return true;
            }
            else if (comparison_result < 0) // value is smaller than current node value
            {
                r = r.left_child;
            }
            else // value is bigger than current node value
            {
                r = r.right_child;
            }
        }

        return false;
    }

    public int depth(Node r)
    {
        int depth_(Node r)
        {
            if (r == null) { return 0; }
            return 1 + Math.Max(depth_(r.left_child!), depth_(r.right_child!));
        }

        return depth_(r);
    }


}