// <copyright file="Obj16Tile.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static Helper.StringHelper;

    public struct Obj16Tile :
        IList,
        IList<ObjTile>,
        IReadOnlyList<ObjTile>,
        IEquatable<Obj16Tile>
    {
        public const int NumberOfTiles = 4;

        public const int TopLeftIndex = 0;
        public const int BottomLeftIndex = 1;
        public const int TopRightIndex = 2;
        public const int BottomRightIndex = 3;

        public const int SizeOf = NumberOfTiles * ObjTile.SizeOf;

        // FIXME: Change signature
        public Obj16Tile(
            ObjTile topLeft,
            ObjTile topRight,
            ObjTile bottomLeft,
            ObjTile bottomRight)
        {
            TopLeft = topLeft;
            BottomLeft = bottomLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
        }

        public ObjTile TopLeft
        {
            get;
            set;
        }

        public ObjTile BottomLeft
        {
            get;
            set;
        }

        public ObjTile TopRight
        {
            get;
            set;
        }

        public ObjTile BottomRight
        {
            get;
            set;
        }

        int ICollection<ObjTile>.Count
        {
            get
            {
                return NumberOfTiles;
            }
        }

        int IReadOnlyCollection<ObjTile>.Count
        {
            get
            {
                return NumberOfTiles;
            }
        }

        int ICollection.Count
        {
            get
            {
                return NumberOfTiles;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        bool ICollection<ObjTile>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public ObjTile this[int index]
        {
            get
            {
                switch (index)
                {
                case TopLeftIndex:
                    return TopLeft;

                case BottomLeftIndex:
                    return TopRight;

                case TopRightIndex:
                    return BottomLeft;

                case BottomRightIndex:
                    return BottomRight;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(index));
                }
            }

            set
            {
                switch (index)
                {
                case TopLeftIndex:
                    TopLeft = value;
                    return;

                case BottomLeftIndex:
                    TopRight = value;
                    return;

                case TopRightIndex:
                    BottomLeft = value;
                    return;

                case BottomRightIndex:
                    BottomRight = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(index));
                }
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = (ObjTile)value;
            }
        }

        public static bool operator ==(Obj16Tile left, Obj16Tile right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Obj16Tile left, Obj16Tile right)
        {
            return !(left == right);
        }

        public static int GetXCoordinate(int index)
        {
            return index / 2;
        }

        public static int GetYCoordinate(int index)
        {
            return index % 2;
        }

        public Obj16Tile FlipX()
        {
            return new Obj16Tile(
                BottomLeft.FlipX(),
                BottomRight.FlipX(),
                TopLeft.FlipX(),
                TopRight.FlipX());
        }

        public Obj16Tile FlipY()
        {
            return new Obj16Tile(
                TopRight.FlipY(),
                TopLeft.FlipY(),
                BottomRight.FlipY(),
                BottomLeft.FlipY());
        }

        public bool Equals(Obj16Tile other)
        {
            return
                TopLeft.Equals(other.TopLeft) &&
                TopRight.Equals(other.TopRight) &&
                BottomLeft.Equals(other.BottomLeft) &&
                BottomRight.Equals(other.BottomRight);
        }

        public override bool Equals(object obj)
        {
            return obj is Obj16Tile tile ? Equals(tile) : false;
        }

        public override int GetHashCode()
        {
            return (TopLeft.Value | (TopRight.Value << 0x10)) ^
                (BottomLeft.Value | (BottomRight.Value << 0x10));
        }

        public override string ToString()
        {
            return GetString(
                "{0}-{1}-{2}-{3}",
                TopLeft,
                TopRight,
                BottomLeft,
                BottomRight);
        }

        public bool Contains(ObjTile tile)
        {
            foreach (var item in this)
            {
                if (Equals(item, tile))
                {
                    return true;
                }
            }

            return false;
        }

        bool IList.Contains(object value)
        {
            return Contains((ObjTile)value);
        }

        public void CopyTo(ObjTile[] array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (arrayIndex + NumberOfTiles > array.Length)
            {
                throw new ArgumentException();
            }

            for (var i = 0; i < NumberOfTiles; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index >= array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (index + NumberOfTiles > array.Length)
            {
                throw new ArgumentException();
            }

            for (var i = 0; i < NumberOfTiles; i++)
            {
                array.SetValue(index + i, this[i]);
            }
        }

        public int IndexOf(ObjTile item)
        {
            for (var i = 0; i < NumberOfTiles; i++)
            {
                if (Equals(this[i], item))
                {
                    return i;
                }
            }

            return -1;
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((ObjTile)value);
        }

        public IEnumerable<byte> GetBytes()
        {
            for (var i = 0; i < NumberOfTiles; i++)
            {
                yield return (byte)this[i];
                yield return (byte)(this[i] >> 8);
            }
        }

        public IEnumerator<ObjTile> GetEnumerator()
        {
            yield return TopLeft;
            yield return BottomLeft;
            yield return TopRight;
            yield return BottomRight;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<ObjTile>.Add(ObjTile item)
        {
            throw new NotSupportedException();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        void ICollection<ObjTile>.Clear()
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        void IList<ObjTile>.Insert(int index, ObjTile item)
        {
            throw new NotSupportedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        bool ICollection<ObjTile>.Remove(ObjTile item)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void IList<ObjTile>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
    }
}
