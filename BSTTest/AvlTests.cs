using BSTs;
using Microsoft.VisualBasic;
using System;
using System.Runtime.InteropServices.JavaScript;
using Xunit;

namespace BSTs.Tests
{
    public class AvlTreeTests
    {
        [Fact]
        public void delete_from_empty()
        {
            AvlTree<int> avl = new();
            Assert.Throws<BTSException>(() => avl.delete(10));
        }


        [Theory]
        [InlineData(new int[] { 10, 20, 30, 15, 25 })]
        [InlineData(new int[] { 12, 2, 1, 15, 26, 23, 24, 6, 34, 3, 4, 20, 46, 56, 10, 8, 9, 7 })]
        public void insert__test(int[] test_arr)
        {
            AvlTree<int> avl = new();
            foreach (int i in test_arr)
            {
                avl.insert(i);
                Assert.True(avl.contains(i));
            }

        }

        [Theory]
        [InlineData(new int[] { 10, 20, 30, 15, 25 })]
        [InlineData(new int[] { 12, 2, 1, 15, 26, 23, 24, 6, 34, 3, 4, 20, 46, 56, 10, 8, 9, 7 })]
        public void delete_test(int[] test_arr)
        {
            AvlTree<int> avl = new();
            foreach (int i in test_arr)
            {
                avl.insert(i);
            }
            foreach (int i in test_arr)
            {
                avl.delete(i);
                Assert.False(avl.contains(i));
            }
        }

        [Theory]
        [InlineData(new int[] { 10, 20, 30, 15, 25 })]
        [InlineData(new int[] { 12, 2, 1, 15, 26, 23, 24, 6, 34, 3, 4, 20, 46, 56, 10, 8, 9, 7 })]
        public void avl_property_test(int[] test_arr)
        {
            AvlTree<int> avl = new();
            foreach (int i in test_arr)
            {
                avl.insert(i);
                Assert.True(avl.avl_property_check());
            }
            foreach (int i in test_arr)
            {
                avl.delete(i);
                Assert.True(avl.avl_property_check());
            }
        }

        [Theory]
        [InlineData(new int[] { 10, 20, 30, 15, 25 })]
        [InlineData(new int[] { 12, 2, 1, 15, 26, 23, 24, 6, 34, 3, 4, 20, 46, 56, 10, 8, 9, 7 })]
        public void parent_child_test(int[] test_arr)
        {
            AvlTree<int> avl = new();
            foreach (int i in test_arr)
            {
                avl.insert(i);
                Assert.True(avl.parent_child_property_check());
            }
            foreach (int i in test_arr)
            {
                avl.delete(i);
                Assert.True(avl.parent_child_property_check());
            }
        }
    }
}