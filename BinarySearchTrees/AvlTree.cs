using BSTs;
using System.Formats.Asn1;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

public class AvlTree<T> : IBinarySearchTree<T> where T : IComparable<T>
{
    public class AvlNode
    {
        public T? value { get; set; }
        public AvlNode? left_child { get; set; }
        public AvlNode? right_child { get; set; }
        public int balance_factor { get; set; }
        public int subtree_height { get; set; }

        public AvlNode? parent { get; set; }

        public AvlNode(T value, AvlNode? left_child = null, AvlNode? right_child = null)
        {
            this.value = value;
            this.left_child = left_child;
            this.right_child = right_child;
            parent = null;
            balance_factor = 0;
            subtree_height = 1;
        }
        public bool is_valid()
        {
            return balance_factor >= -1 && balance_factor <= 1;
        }

        public void update_balance_factor()
        {
            if (left_child == null && right_child == null) { balance_factor = 0; }
            else if (left_child == null) { balance_factor = right_child!.subtree_height; }
            else if (right_child == null) { balance_factor = -left_child.subtree_height; }
            else { balance_factor = right_child.subtree_height - left_child.subtree_height; }
        }

        public void update_height()
        {
            if (left_child == null && right_child == null) { subtree_height = 1; }
            else if (left_child == null) { subtree_height = 1 + right_child!.subtree_height; }
            else if (right_child == null) { subtree_height = 1 + left_child.subtree_height; }
            else { subtree_height = 1 + Math.Max(right_child.subtree_height, left_child.subtree_height); }
        }

        public void update()
        {
            update_height();
            update_balance_factor();
        }

    }

    public AvlNode? avl_root { get; set; }

    // we make the right child of r to be in the position of r
    AvlNode left_rotation(AvlNode r)
    {
        bool was_right_child;
        AvlNode switching_place = r.right_child!;
        r.right_child = switching_place.left_child;
        if (switching_place.left_child != null) { switching_place.left_child.parent = r; }
        switching_place.left_child = r;
        if (r.parent != null)
        {
            was_right_child = r.parent.right_child == r;
            switching_place.parent = r.parent;
            if (was_right_child) { r.parent.right_child = switching_place; }
            else { r.parent.left_child = switching_place; }
        }
        else { avl_root = switching_place; switching_place.parent = null; }
        r.parent = switching_place;


        r.update_height();
        switching_place.update_height();

        r.update_balance_factor();
        switching_place.update_balance_factor();

        return switching_place;
    }

    // we make the left child of r to be in the position of r
    AvlNode right_rotation(AvlNode r)
    {
        bool was_right_child;
        AvlNode switching_place = r.left_child!;
        r.left_child = switching_place.right_child;
        if (switching_place.right_child != null) { switching_place.right_child.parent = r; }
        switching_place.right_child = r;
        if (r.parent != null)
        {
            was_right_child = r.parent.right_child == r;
            switching_place.parent = r.parent;
            if (was_right_child) { r.parent.right_child = switching_place; }
            else { r.parent.left_child = switching_place; }
        }
        else { avl_root = switching_place; switching_place.parent = null; }
        r.parent = switching_place;

        r.update_height();
        switching_place.update_height();

        r.update_balance_factor();
        switching_place.update_balance_factor();

        return switching_place;
    }

    // decides which rotation to do and then updates the heights and balance factors of the nodes
    AvlNode balance_node(AvlNode r, out bool send_signal)
    {
        int previous_height = r.subtree_height;
        if (r.balance_factor < -1)
        {
            if (r.left_child!.balance_factor <= 0) { r = right_rotation(r); }
            else { r.left_child = left_rotation(r.left_child); r = right_rotation(r); }
        }
        else if (r.balance_factor > 1)
        {
            if (r.right_child!.balance_factor >= 0) { r = left_rotation(r); }
            else { r.right_child = right_rotation(r.right_child!); r = left_rotation(r); }
        }
        send_signal = previous_height != r.subtree_height;

        return r;
    }

    // we go up the tree by parent parameter to see if the avl property has been violated, if yes rebalance
    // send signal:= if the node doesn't send the signal, it means that there is no need to continue checking
    void rebalance(AvlNode? r)
    {
        while (r != null)
        {
            r.update();

            if (!r.is_valid())
            {
                r = balance_node(r, out bool send_signal);
                if (!send_signal) { break; }
            }
            r = r.parent;
        }
    }

    // insert the node like in a normal binary search tree and then rebalance
    public void insert(T value)
    {
        AvlNode insert_node = new(value);
        if (avl_root == null) { avl_root = insert_node; return; }

        AvlNode? r = avl_root;
        while (true)
        {
            int comparison_result = value.CompareTo(r.value);

            if (comparison_result == 0) { return; }
            else if (comparison_result < 0) // value is smaller than current node value
            {
                if (r.left_child == null)
                {
                    r.left_child = insert_node; insert_node.parent = r; break;
                }
                else
                {
                    r = r.left_child;
                }
            }
            else // value is bigger than current node value
            {
                if (r.right_child == null)
                {
                    r.right_child = insert_node; insert_node.parent = r; break;
                }
                else
                {
                    r = r.right_child;
                }
            }
        }

        rebalance(r);
    }

    // delete the node like in a normal binary search tree and then rebalance
    public void delete(T value)
    {
        AvlNode? r = avl_root;
        if (r == null) { throw new BTSException("Cannot delete from an empty tree"); }

        while (true)
        {
            int comparison_result = value.CompareTo(r.value);

            if (comparison_result == 0)
            {
                if (r.left_child == null && r.right_child == null)
                {
                    if (r.parent == null) { avl_root = null; return; }
                    else if (r.parent.left_child == r) { r.parent.left_child = null; }
                    else { r.parent.right_child = null; }
                    break;
                }
                else if (r.left_child == null)
                {
                    if (r.parent == null) { avl_root = r.right_child; avl_root!.parent = null; }
                    else if (r.parent.left_child == r) { r.parent.left_child = r.right_child; r.right_child!.parent = r.parent; }
                    else { r.parent.right_child = r.right_child; r.right_child!.parent = r.parent; }
                    break;
                }
                else if (r.right_child == null)
                {
                    if (r.parent == null) { avl_root = r.left_child; avl_root!.parent = null; }
                    else if (r == r.parent.left_child) { r.parent.left_child = r.left_child; r.left_child.parent = r.parent; }
                    else { r.parent.right_child = r.left_child; r.left_child.parent = r.parent; }
                    break;
                }
                else // replace r with minimum in its right subtree
                {
                    AvlNode p = r; // store the pointer of the original node to modify its parents child later
                    r = r.right_child;
                    T? replacement_value;
                    if (r.left_child == null) // if min in the right subtree is the right child itself
                    {
                        replacement_value = r.value;
                        r.parent!.right_child = r.right_child;
                        if (r.right_child != null) { r.right_child.parent = r.parent; } // update parent
                    }
                    else
                    {
                        while (r.left_child != null)
                        {
                            r = r.left_child;
                        }
                        replacement_value = r.value;
                        r.parent!.left_child = r.right_child;
                        if (r.right_child != null) { r.right_child.parent = r.parent; } // update parent
                    }

                    // replace the value with minimum of the right subtree
                    p.value = replacement_value;
                    break;
                }
            }
            else if (comparison_result < 0) // value is smaller than current node value
            {
                if (r.left_child == null) { throw new BTSException("Value is not present in the tree"); }
                else { r = r.left_child; }
            }
            else // value is bigger than current node value
            {
                if (r.right_child == null) { throw new BTSException("Value is not present in the tree"); }
                else { r = r.right_child; }
            }
        }


        if (r.parent != null) { r = r.parent; rebalance(r); }
    }

    // just a helper function
    public void print_tree()
    {
        void print_tree_(AvlNode? r, int h)
        {
            if (r == null) { return; }
            print_tree_(r.right_child, h + 1);
            Console.WriteLine(String.Concat(Enumerable.Repeat("   ", h)) + r.value);
            print_tree_(r.left_child, h + 1);
        }
        print_tree_(avl_root!, 0);
    }

    public bool contains(T value)
    {
        AvlNode? r = avl_root;
        if (r == null)
        {
            return false;
        }

        while (r != null)
        {
            int comparison_result = value.CompareTo(r.value);
            if (comparison_result == 0) { return true; }
            else if (comparison_result < 0) { r = r.left_child; } // value is smaller than current node value
            else { r = r.right_child; }// value is bigger than current node value
        }

        return false;
    }

    // just a helper function
    public int depth(AvlNode r)
    {
        int depth_(AvlNode r)
        {
            if (r == null) { return 0; }
            return 1 + Math.Max(depth_(r.left_child!), depth_(r.right_child!));
        }

        return depth_(r);
    }

    // used for testing. checks if the parent child relationships are correct
    public bool parent_child_property_check()
    {
        bool property_check_(AvlNode? r)
        {
            if (r == null) { return true; }

            if (r.left_child != null && r != r.left_child.parent)
            {
                Console.WriteLine($"{r.left_child.value}'s parent is {r.left_child.parent!.value} instead of {r.value}");
                return false;
            }
            if (r.right_child != null && r != r.right_child.parent)
            {
                Console.WriteLine($"{r.right_child.value}'s parent is {r.right_child.parent!.value} instead of {r.value}");
                return false;
            }

            bool left = property_check_(r.left_child);
            bool right = property_check_(r.right_child);

            return left && right;
        }
        return property_check_(avl_root);
    }

    // used for testing. checks if the balance factor of each node is between -1 and 1
    public bool avl_property_check()
    {
        bool avl_prop_check_(AvlNode? r)
        {
            if (r == null) { return true; }
            if (!r.is_valid()) { return false; }

            bool left = avl_prop_check_(r.left_child);
            bool right = avl_prop_check_(r.right_child);
            return left && right;
        }

        return avl_prop_check_(avl_root);
    }
}