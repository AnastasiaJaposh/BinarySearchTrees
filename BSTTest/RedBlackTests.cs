using BSTs;
using System;
using System.Runtime.InteropServices.JavaScript;
using Xunit;
using BSTTest;

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
        [MemberData(nameof(TestInput.IntInput), MemberType = typeof(TestInput))]
        public void insert_test(int[] test_arr)
        {
            RedBlackTree<int> rb = new();
            foreach (int i in test_arr)
            {
                rb.insert(i);
                Assert.True(rb.contains(i));
            }

        }

        [Theory]
        [MemberData(nameof(TestInput.IntInput), MemberType = typeof(TestInput))]
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
        [MemberData(nameof(TestInput.IntInput), MemberType = typeof(TestInput))]
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
        [MemberData(nameof(TestInput.IntInput), MemberType = typeof(TestInput))]
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