// <copyright file="EditorDataTests.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class EditorDataTests
    {
        [Fact]
        public void DefaultConstructorIsEmpty()
        {
            var data = new IntegerData();
            Assert.Empty(data);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        [InlineData(-100)]
        [InlineData(Int32.MinValue)]
        public void CapcityContructorThrowsLessThanZero(
            int capacity)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new IntegerData(capacity));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void CapacityConstructorHasCapacityAndIsEmpty(
            int capacity)
        {
            var data = new IntegerData(capacity);
            Assert.Equal(capacity, data.Capacity);
            Assert.Empty(data);
        }

        [Fact]
        public void EnumerableConstructorThrowsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new IntegerData(null));
        }

        [Fact]
        public void EnumerableConstructorMirrorsData()
        {
            var range = Enumerable.Range(0, 0x100).Select(x => (byte)x);

            var data = new IntegerData(range);
            Assert.Equal(range, data.InternalDataAsReadOnly());
        }

        [Fact]
        public void ListConstructorThrowsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => IntegerData.FromExternalData(null));
        }

        [Fact]
        public void ListConstructorRetainsData()
        {
            var list = new List<byte>();
            var data = IntegerData.FromExternalData(list);
            Assert.Empty(data);

            list.AddRange(new byte[] { 0x12, 0x34, 0x56, 0x78 });
            Assert.Equal(list, data.InternalDataAsReadOnly());
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(1, -1)]
        [InlineData(1, 1)]
        [InlineData(100, 101)]
        [InlineData(0, Int32.MaxValue)]
        public void StartOffsetThrowsOutOfRange(
            int count,
            int startOffset)
        {
            var data = new IntegerData(new List<byte>(count));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => data.StartOffset = startOffset);
        }

        private class IntegerData : EditorData<int>
        {
            public IntegerData()
                : base()
            {
            }

            public IntegerData(int capacity)
                : base(capacity)
            {
            }

            public IntegerData(IEnumerable<byte> data)
                : base(data)
            {
            }

            private IntegerData(List<byte> data)
                : base(data)
            {
            }

            public static IntegerData FromExternalData(List<byte> data)
            {
                return new IntegerData(data);
            }

            protected override IEnumerable<byte> GetByteData(int item)
            {
                return new[]
                {
                    (byte)(item >> 0x00),
                    (byte)(item >> 0x08),
                    (byte)(item >> 0x10),
                    (byte)(item >> 0x18)
                };
            }

            protected override int GetItem(IEnumerable<byte> data)
            {
                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                var list = new List<byte>(data.Take(sizeof(int)));
                return
                    (list[0] << 0x00) |
                    (list[1] << 0x08) |
                    (list[2] << 0x10) |
                    (list[3] << 0x18);
            }

            public override int GetIndex(int offset, int startOffset)
            {
                return (offset - startOffset) / sizeof(int);
            }

            public override int GetOffset(int index, int startOffset)
            {
                return startOffset + (index * sizeof(int));
            }

            public override int GetSizeOfItemAtOffset(int offset)
            {
                return sizeof(int);
            }

            public override int GetSizeOfItem(int item)
            {
                return sizeof(int);
            }
        }
    }
}
