using BSTs;
using System;
using System.Runtime.InteropServices.JavaScript;
using Xunit;

namespace BSTs.Tests
{
    public class RedBlackTreeTests
    {
        [Fact]
        public void delete_from_empty()
        {
            RedBlackTree<int> rb = new();
            Assert.Throws<BTSException>(() => rb.delete(10));
        }

        [Theory]
        [InlineData(new int[] { 10, 20, 30, 15, 25 })]
        [InlineData(new int[] { 12, 2, 1, 15, 26, 23, 24, 6, 34, 3, 4, 20, 46, 56, 10, 8, 9, 7 })]
        public void insert__test(int[] test_arr)
        {
            RedBlackTree<int> rb = new();
            foreach (int i in test_arr)
            {
                rb.insert(i);
                Assert.True(rb.contains(i));
            }

        }

        [Theory]
        [InlineData(new int[] { 10, 20, 30, 15, 25 })]
        [InlineData(new int[] { 12, 2, 1, 15, 26, 23, 24, 6, 34, 3, 4, 20, 46, 56, 10, 8, 9, 7 })]
        public void delete_test(int[] test_arr)
        {
            RedBlackTree<int> rb = new();
            foreach (int i in test_arr)
            {
                rb.insert(i);
            }
            foreach (int i in test_arr)
            {
                rb.delete(i);
                Assert.False(rb.contains(i));
            }
        }

        [Theory]
        [InlineData(new int[] { 10, 20, 30, 15, 25 })]
        [InlineData(new int[] { 12, 2, 1, 15, 26, 23, 24, 6, 34, 3, 4, 20, 46, 56, 10, 8, 9, 7 })]
        public void red_property_test(int[] test_arr)
        {
            RedBlackTree<int> rb = new();
            foreach (int i in test_arr)
            {
                rb.insert(i);
                Assert.True(rb.red_property_check());
            }
            foreach (int i in test_arr)
            {
                rb.delete(i);
                Assert.True(rb.red_property_check());
            }
        }

        [Theory]
        [InlineData(new int[] { 10, 20, 30, 15, 25 })]
        [InlineData(new int[] { 12, 2, 1, 15, 26, 23, 24, 6, 34, 3, 4, 20, 46, 56, 10, 8, 9, 7 })]
        public void black_property_test(int[] test_arr)
        {
            RedBlackTree<int> rb = new();
            foreach (int i in test_arr)
            {
                rb.insert(i);
                Assert.True(rb.black_property_check());
            }
            foreach (int i in test_arr)
            {
                rb.delete(i);
                Assert.True(rb.black_property_check());
            }
        }
    }
}