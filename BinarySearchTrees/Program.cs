using BSTs;

Compare comparer = new();

Console.WriteLine("enter insert/delete range to compare AVL, RB and normal BSTs: ");
long range = Convert.ToInt64(Console.ReadLine());
Console.WriteLine($"Comparing all trees with {range} elements");
comparer.compare_altogether(new AvlTree<long>(), new RedBlackTree<long>(), new BinarySearchTree<long>(), 1000);

Console.WriteLine("enter insert/delete range to compare AVL and RB trees: ");
range = Convert.ToInt64(Console.ReadLine());
Console.WriteLine($"\nComparing balanced trees with {range} elements");
comparer.compare_balanced_trees(new AvlTree<long>(), new RedBlackTree<long>(), range);