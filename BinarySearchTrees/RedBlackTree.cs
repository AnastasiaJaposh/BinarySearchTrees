using BSTs;
using System.ComponentModel;
using System.Drawing;


public class RedBlackTree<T> : IBinarySearchTree<T> where T : IComparable<T>
{
    public enum Color
    {
        Red,
        Black
    }

    public class RBNode
    {
        public T? value { get; set; } // if value is null, it is an externall node
        public RBNode? left_child { get; set; }
        public RBNode? right_child { get; set; }
        public RBNode? parent { get; set; }
        public Color color { get; set; }

        public RBNode(T value, RBNode? left_child = null, RBNode? right_child = null)
        {
            this.value = value;
            color = Color.Red; // when inserting color is red
            this.left_child = left_child;
            this.right_child = right_child;

        }
    }

    public RBNode? rb_root { get; set; }
    // puts the right child of r in the place of r, and updates the children
    RBNode left_rotation(RBNode r)
    {
        bool was_right_child;
        RBNode switching_place = r.right_child!;
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
        else { rb_root = switching_place; switching_place.parent = null; }
        r.parent = switching_place;

        return switching_place;
    }

    // puts the left child of r in the place of r, and updates the children
    RBNode right_rotation(RBNode r)
    {
        bool was_right_child;
        RBNode switching_place = r.left_child!;
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
        else { rb_root = switching_place; switching_place.parent = null; }
        r.parent = switching_place;

        return switching_place;
    }

    /*
     * note: r is the parent of the inserted node
     * pseudo code:
     *      if parent is red:
     *          2 symmetric cases: 1. r is the left child of its parent, 2. r is the right child of its parent
     *          wlog. if r is the left child of its parent: 
     *              1. if the right sibling of r is red, recolor 2 siblings black, parent red and move up(check if parent being red violates the property)
     *              2. else, first do a rotation on r if the inserted node is the right child of r,
     *                 then recolor r black, parent red and do a right rotation on parent, and return since the properties are restored
    */
    void insert_restore_property(RBNode? r, bool is_right_child)
    {
        if (r == null) { return; }
        if (r.color == Color.Red) // property has been violated
        {
            if (r.Equals(rb_root)) { r.color = Color.Black; return; }
            else if (r == r.parent!.left_child)
            {
                if (r.parent.right_child != null && r.parent.right_child.color == Color.Red) // right sibling of r is red
                {
                    // recolor and move up
                    r.color = Color.Black;
                    r.parent.right_child.color = Color.Black;
                    r.parent.color = Color.Red;
                    insert_restore_property(r.parent.parent, (r.parent.parent != null) && r.parent.parent.right_child == r.parent);
                }
                else
                {
                    if (is_right_child) { r = left_rotation(r); }
                    r.color = Color.Black;
                    r.parent!.color = Color.Red;
                    right_rotation(r.parent);
                    return;
                }
            }
            else if (r == r.parent!.right_child)
            {
                if (r.parent.left_child != null && r.parent.left_child.color == Color.Red)
                {
                    // recolor and move up
                    r.color = Color.Black;
                    r.parent.left_child.color = Color.Black;
                    r.parent.color = Color.Red;
                    insert_restore_property(r.parent.parent, (r.parent.parent != null) && r.parent.parent.right_child == r.parent);
                }
                else
                {
                    if (!is_right_child) { r = right_rotation(r); }
                    r.color = Color.Black;
                    r.parent!.color = Color.Red;
                    left_rotation(r.parent);
                    return;
                }
            }
        }


    }

    // color the inserted node red, and restore the properties if they are violated
    public void insert(T value)
    {
        RBNode insert_node = new(value);
        if (rb_root == null) { rb_root = insert_node; insert_node.color = Color.Black; return; }

        RBNode r = rb_root!;
        bool is_right_child;
        while (true)
        {
            int comparison_result = value.CompareTo(r.value);

            if (comparison_result == 0) { return; }
            else if (comparison_result < 0) // value is smaller than current node value
            {
                if (r.left_child == null)
                {
                    r.left_child = insert_node; insert_node.parent = r; is_right_child = false; break;
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
                    r.right_child = insert_node; insert_node.parent = r; is_right_child = true; break;
                }
                else
                {
                    r = r.right_child;
                }
            }
        }

        // r is the parent of the inserted node
        insert_restore_property(r, is_right_child);
        if (rb_root.color == Color.Red) { rb_root.color = Color.Black; }
    }

    /*
     * pseudo code:
     *  if sibling is black:
     *      if its an external node, we can't do anything in the empty subtree, so we move up
     *      if its not an external node:
     *          1. if it has 2 black children, we can recolor the sibling red to reduce the black height of the sibling subtree and 
     *          then recolor the parent black to increase it for both(sibling and r), if the parent is already black, we still have to handle double black
     *          2. if sibling has at least 1 red child, we have 2 symmetric cases
     *          wlog. left child is red:
     *              if sibling is the left child and r is the right child:
     *                  we color the said child black, give the sibling the parent's color and put it in place of the parent by right rotation
     *              if the sibling is the right child and r is the left child:
     *                  essentially, the red left child will be put in place of the parent, so we give it parent's color
     *              at the end, we color the parent black
     * if sibling is red:
     *      we recolor it black, give the parent red color and put the sibling in place of the parent.
     *      we haven't changed the black height of the subtree, so we move up
    */
    void handle_double_black(RBNode? r, RBNode? parent, bool is_right_child)
    {
        if (r == rb_root)
        {
            return;
        }

        RBNode? sibling = is_right_child ? parent!.left_child : parent!.right_child;
        if (sibling == null)
        {
            handle_double_black(parent, parent.parent, parent.parent != null && (parent.parent.right_child == parent));
        }
        else if (sibling.color == Color.Red)
        {
            sibling.color = Color.Black;
            parent.color = Color.Red;
            // sibling is right child, if r is not right child
            if (!is_right_child) { left_rotation(parent); }
            else { right_rotation(parent); }
            handle_double_black(r, parent, is_right_child);
        }
        else // sibling is not null, and it is black
        {
            if ((sibling.left_child == null || sibling.left_child.color == Color.Black) && // sibling has 2 black children
             (sibling.right_child == null || sibling.right_child.color == Color.Black))
            {
                sibling.color = Color.Red;
                if (parent.color == Color.Black)
                {
                    // if parent.parent is null, it is the root and it will be handled first
                    handle_double_black(parent, parent.parent, parent.parent != null && (parent.parent.right_child == parent));
                }
                else { parent.color = Color.Black; }
            }
            else // has at least 1 red child
            {
                if (sibling.left_child != null && sibling.left_child.color == Color.Red)
                {
                    if (is_right_child)
                    {
                        sibling.left_child.color = sibling.color;
                        sibling.color = parent.color;
                        right_rotation(parent);
                    }
                    else
                    {
                        sibling.left_child.color = parent.color;
                        right_rotation(sibling);
                        left_rotation(parent);
                    }
                }
                else // right is the red child, so it is not null
                {
                    if (!is_right_child)
                    {
                        sibling.right_child!.color = sibling.color;
                        sibling.color = parent.color;
                        left_rotation(parent);
                    }
                    else
                    {
                        sibling.right_child!.color = parent.color;
                        left_rotation(sibling);
                        right_rotation(parent);
                    }
                }
                parent.color = Color.Black;
            }
        }
    }

    /*
     * delete normally like in a bst,
     * if both the replacement and the deleted node are black, the black property will be violated, and we need to handle double black
     * if either the replacement or the deleted node is red, we can color the replacement black and we will be done.
     * note: we can't have 2 consequitive red nodes, so these 2 are the only cases
    */
    void delete_restore_property(RBNode? r, RBNode? parent, bool is_right_child, bool double_black)
    {
        if (r == rb_root) { rb_root!.color = Color.Black; return; }

        if (!double_black)
        {
            if (r != null) { r.color = Color.Black; }
        }
        else // double black node, need to restore black node property
        {
            handle_double_black(r, parent, is_right_child);
        }
    }

    public void delete(T value)
    {
        RBNode? r = rb_root;
        if (r == null) { throw new BTSException("Cannot delete from an empty tree"); }

        bool replaced_by_right = false;
        bool double_black = false;
        while (true)
        {
            int comparison_result = value.CompareTo(r.value);

            if (comparison_result == 0)
            {
                if (r.left_child == null && r.right_child == null)
                {
                    if (r.parent == null) { rb_root = null; return; }
                    else if (r.parent.left_child == r) { r.parent.left_child = null; }
                    else { r.parent.right_child = null; }
                    replaced_by_right = true;
                    break;
                }
                else if (r.left_child == null)
                {
                    if (r.parent == null) { rb_root = r.right_child; rb_root!.parent = null; }
                    else if (r.parent.left_child == r) { r.parent.left_child = r.right_child; r.right_child!.parent = r.parent; }
                    else { r.parent.right_child = r.right_child; r.right_child!.parent = r.parent; }
                    replaced_by_right = true;
                    break;
                }
                else if (r.right_child == null)
                {
                    if (r.parent == null) { rb_root = r.left_child; rb_root!.parent = null; }
                    else if (r == r.parent.left_child) { r.parent.left_child = r.left_child; r.left_child.parent = r.parent; }
                    else { r.parent.right_child = r.left_child; r.left_child.parent = r.parent; }
                    replaced_by_right = false;
                    break;
                }
                else // replace r with minimum in its right subtree
                {
                    RBNode p = r; // store the pointer of the original node to modify its parents child later
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
                    replaced_by_right = true;

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

        RBNode? replacement = replaced_by_right ? r.right_child : r.left_child;
        double_black = !(r.color == Color.Red || (replacement != null && replacement.color == Color.Red));
        if (replacement != rb_root)
        {
            // r is the pointer to a deleted node
            delete_restore_property(replacement, r.parent, replacement == r.parent!.right_child, double_black);
        }

        if (rb_root != null && rb_root.color != Color.Black)
        {
            rb_root.color = Color.Black;
        }


    }

    public bool contains(T value)
    {
        RBNode? r = rb_root;
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

    // just a helper fucntion
    public void print_tree()
    {
        void print_tree_(RBNode? r, int h)
        {
            if (r == null) { return; }
            print_tree_(r.right_child, h + 1);
            char c = r.color == Color.Black ? 'b' : 'c';
            Console.WriteLine(String.Concat(Enumerable.Repeat("   ", h)) + r.value + c);
            print_tree_(r.left_child, h + 1);
        }
        print_tree_(rb_root!, 0);
    }

    // used for testing. check if red nodes never have red children
    public bool red_property_check()
    {
        RBNode? r = rb_root;
        bool test_success = true;
        void go(RBNode? r)
        {
            if (r == null)
            {
                return;
            }
            else if (r.color == Color.Red)
            {
                if ((r.right_child != null && r.right_child.color == Color.Red) || (r.left_child != null && r.left_child.color == Color.Red))
                {
                    test_success = false;
                }
            }

            go(r.right_child);
            go(r.left_child);
        }

        go(r);
        return test_success;

    }

    // used for testing. check if black heights is the same for every path
    public bool black_property_check()
    {
        RBNode? r = rb_root;
        bool test_success = true;
        int go(RBNode? r)
        {
            if (r == null)
            {
                return 0;
            }
            int le = go(r.left_child);
            int ri = go(r.right_child);
            if (le != ri) { test_success = false; }
            if (r.color == Color.Black)
            {
                return 1 + le;
            }
            else
            {
                return le;
            }
        }

        go(r);
        return test_success;
    }
}
