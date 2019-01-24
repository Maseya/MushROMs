// <copyright file="EditorSelector.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using static System.ComponentModel.DesignerSerializationVisibility;
    using static System.IO.Path;
    using IEditorDictionary = System.Collections.Generic.IDictionary<string, Maseya.Editors.IEditor>;
    using IReadOnlyEditorDictionary = System.Collections.Generic.IReadOnlyDictionary<string, Maseya.Editors.IEditor>;
    using PathEditorPair = System.Collections.Generic.KeyValuePair<string, Maseya.Editors.IEditor>;

    /// <summary>
    /// Provides a configurable collection of <see cref="IEditor"/> instances.
    /// </summary>
    public class EditorSelector : Component
    {
        /// <summary>
        /// The <see cref="IEditor"/> that is considered to be actively in use
        /// by the user.
        /// </summary>
        private IEditor currentEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref=" EditorSelector"/>
        /// class.
        /// </summary>
        public EditorSelector()
        {
            Items = new EditorDictionary(this);
        }

        public EditorSelector(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        /// <summary>
        /// Occurs when <see cref="CurrentEditor"/> is changed.
        /// </summary>
        public event EventHandler<EditorEventArgs> CurrentEditorChanged;

        /// <summary>
        /// Occurs when <see cref="CurrentEditor"/> is removed from <see
        /// cref="Items"/>.
        /// </summary>
        public event EventHandler CurrentEditorRemoved;

        /// <summary>
        /// Occurs when a new <see cref="IEditor"/> is added to <see
        /// cref="Items"/>.
        /// </summary>
        public event EventHandler<EditorEventArgs> EditorAdded;

        /// <summary>
        /// Occurs when an <see cref="IEditor"/> is removed from <see
        /// cref="Items"/>.
        /// </summary>
        public event EventHandler<EditorEventArgs> EditorRemoved;

        /// <summary>
        /// Gets the collection of <see cref="IEditor"/> instances that this
        /// <see cref="EditorSelector"/> can choose between.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public EditorDictionary Items
        {
            get;
        }

        /// <summary>
        /// Gets or sets the <see cref="IEditor"/> that is considered to be
        /// actively in use by the user.
        /// </summary>
        /// <remarks>
        /// If <see langword="null"/> is set, the current editor is removed. If
        /// an <see cref="IEditor"/> is set that does not exist in <see
        /// cref="Items"/>, then it is added to the collection first.
        /// </remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public IEditor CurrentEditor
        {
            get
            {
                return currentEditor;
            }

            set
            {
                if (currentEditor == value)
                {
                    return;
                }

                if (value is null)
                {
                    currentEditor = null;
                    OnCurrentEditorRemoved(EventArgs.Empty);
                    return;
                }

                // Add this editor if it does not exist.
                Items[value.Path] = value;

                currentEditor = value;
                var e = new EditorEventArgs(value);
                OnCurrentEditorChanged(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="CurrentEditorChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EditorEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnCurrentEditorChanged(EditorEventArgs e)
        {
            CurrentEditorChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CurrentEditorRemoved"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnCurrentEditorRemoved(EventArgs e)
        {
            CurrentEditorRemoved?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="EditorAdded"/> event and sets <see
        /// cref="CurrentEditor"/> to the newly added <see cref="IEditor"/>.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EditorEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnEditorAdded(EditorEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            EditorAdded?.Invoke(this, e);

            CurrentEditor = e.Editor;
        }

        /// <summary>
        /// Raises the <see cref="EditorRemoved"/> event and sets <see
        /// cref="CurrentEditor"/> to <see langword="null"/> if it is the <see
        /// cref="IEditor"/> that was removed.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EditorEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnEditorRemoved(EditorEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            EditorRemoved?.Invoke(this, e);

            if (CurrentEditor == e.Editor)
            {
                CurrentEditor = null;
            }
        }

        /// <summary>
        /// Represents a collection of file path keys and <see cref="
        /// IEditor"/> values that raise <see cref="EditorSelector"/> events
        /// when items are added or removed.
        /// </summary>
        /// <remarks>
        /// The dictionary keys can be either a relative or full path <see
        /// cref="String"/>. Internally, the full paths will be stored as the
        /// keys. Comparison among paths is invariant of case.
        /// </remarks>
        public sealed class EditorDictionary :
            IEditorDictionary,
            IReadOnlyEditorDictionary
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="
            /// EditorDictionary"/> class with the specified <see cref="
            /// EditorSelector"/> as its parent.
            /// </summary>
            /// <param name="editorSelector">
            /// The <see cref="EditorSelector"/> that will own this <see
            /// cref="EditorDictionary"/>.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="editorSelector"/> is <see langword="null"/>.
            /// </exception>
            internal EditorDictionary(EditorSelector editorSelector)
            {
                EditorSelector = editorSelector
                    ?? throw new ArgumentNullException(nameof(
                        editorSelector));

                Editors = new Dictionary<string, IEditor>(
                    StringComparer.OrdinalIgnoreCase);
            }

            /// <summary>
            /// Gets the <see cref="EditorSelector"/> that owns this <see
            /// cref="EditorDictionary"/>.
            /// </summary>
            public EditorSelector EditorSelector
            {
                get;
            }

            /// <summary>
            /// Gets the number of key/value pairs in this <see
            /// cref="EditorDictionary"/>.
            /// </summary>
            public int Count
            {
                get
                {
                    return Editors.Count;
                }
            }

            /// <summary>
            /// Gets a collection containing the file path keys in this <see
            /// cref="EditorDictionary"/>.
            /// </summary>
            public Dictionary<string, IEditor>.KeyCollection Keys
            {
                get
                {
                    return Editors.Keys;
                }
            }

            /// <inheritdoc/>
            ///
            ICollection<string> IEditorDictionary.Keys
            {
                get
                {
                    return Keys;
                }
            }

            /// <inheritdoc/>
            ///
            IEnumerable<string> IReadOnlyEditorDictionary.Keys
            {
                get
                {
                    return Keys;
                }
            }

            /// <summary>
            /// Gets a collection containing the <see cref="IEditor"/> values
            /// in this <see cref="EditorDictionary"/>.
            /// </summary>
            public Dictionary<string, IEditor>.ValueCollection Values
            {
                get
                {
                    return Editors.Values;
                }
            }

            /// <inheritdoc/>
            ///
            ICollection<IEditor> IEditorDictionary.Values
            {
                get
                {
                    return Values;
                }
            }

            /// <inheritdoc/>
            ///
            IEnumerable<IEditor> IReadOnlyEditorDictionary.Values
            {
                get
                {
                    return Values;
                }
            }

            /// <inheritdoc/>
            ///
            bool ICollection<PathEditorPair>.IsReadOnly
            {
                get
                {
                    return IDictionary.IsReadOnly;
                }
            }

            /// <summary>
            /// Gets the internal data dictionary that describes the contents
            /// of this <see cref="EditorDictionary"/>.
            /// </summary>
            private Dictionary<string, IEditor> Editors
            {
                get;
            }

            /// <summary>
            /// Gets the interface implementation of <see cref="Editors"/>.
            /// </summary>
            /// <remarks>
            /// This property is just a type caster for the various areas where
            /// we do explicit interface implementations.
            /// </remarks>
            private IEditorDictionary IDictionary
            {
                get
                {
                    return Editors;
                }
            }

            /// <summary>
            /// Gets or sets the <see cref="IEditor"/> associated with a
            /// specific file path.
            /// </summary>
            /// <param name="key">
            /// The file path of the value to get or set.
            /// </param>
            /// <returns>
            /// The <see cref="IEditor"/> associated with the specified file
            /// path. If the specified file path is not found, a get operations
            /// throws a <see cref=" KeyNotFoundException"/>, and a set
            /// operation creates a new <see cref="IEditor"/> with the
            /// specified file path.
            /// </returns>
            /// <remarks>
            /// The file path key can be a full or relative path. The property
            /// will search for the case-invariant full path.
            /// </remarks>
            /// <exception cref="ArgumentException">
            /// <paramref name="key"/> is a zero-length string, contains only
            /// whitespace, or contains one or more of the invalid characters
            /// defined in <see cref=" GetInvalidPathChars"/>.
            /// <para/>
            /// -or-
            /// <para/>
            /// The system could not retrieve the absolute path.
            /// </exception>
            /// <exception cref="System.Security.SecurityException">
            /// The caller does not have the required permissions.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="key"/> is <see langword="null"/>.
            /// <para/>
            /// -or-
            /// <para/>
            /// <paramref name="value"/> is <see langword="null"/>.
            /// </exception>
            /// <exception cref="NotSupportedException">
            /// <paramref name="key"/> contains a colon (":") that is not part
            /// of a volume identifier (for example, "c:\").
            /// </exception>
            /// <exception cref="System.IO.PathTooLongException">
            /// The specified path, file name, or both exceed the
            /// system-defined maximum length.
            /// </exception>
            /// <exception cref="KeyNotFoundException">
            /// The property is retrieved and <paramref name="key"/> does not
            /// exist in the collection.
            /// </exception>
            public IEditor this[string key]
            {
                get
                {
                    return Editors[GetFullPath(key)];
                }

                set
                {
                    if (value is null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }

                    var actualKey = GetFullPath(key);
                    if (TryGetValue(actualKey, out var editor))
                    {
                        if (editor == value)
                        {
                            return;
                        }

                        Remove(actualKey);
                    }

                    Add(actualKey, value);
                    return;
                }
            }

            /// <summary>
            /// Adds the specified file path key and <see cref=" IEditor"/>
            /// value to the <see cref="EditorDictionary"/>.
            /// </summary>
            /// <param name="key">
            /// The file path key of the <see cref="IEditor"/> to add.
            /// </param>
            /// <param name="value">
            /// The <see cref="IEditor"/> of the element to add.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="key"/> is <see langword="null"/>.
            /// <para/>
            /// -or-
            /// <para/>
            /// <paramref name="value"/> is <see langword="null"/>.
            /// </exception>
            /// <exception cref="ArgumentException">
            /// <paramref name="key"/> is a zero-length string, contains only
            /// whitespace, or contains one or more of the invalid characters
            /// defined in <see cref=" GetInvalidPathChars"/>.
            /// <para/>
            /// -or-
            /// <para/>
            /// The system could not retrieve the absolute path.
            /// </exception>
            /// <exception cref="System.Security.SecurityException">
            /// The caller does not have the required permissions.
            /// </exception>
            /// <exception cref="NotSupportedException">
            /// <paramref name="key"/> contains a colon (":") that is not part
            /// of a volume identifier (for example, "c:\").
            /// </exception>
            /// <exception cref="System.IO.PathTooLongException">
            /// The specified path, file name, or both exceed the
            /// system-defined maximum length.
            /// </exception>
            public void Add(string key, IEditor value)
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                var actualKey = GetFullPath(key);
                Editors.Add(actualKey, value);

                var e = new EditorEventArgs(value);
                EditorSelector.OnEditorAdded(e);
            }

            /// <inheritdoc cref="Add(String, IEditor)"/>
            ///
            void ICollection<PathEditorPair>.Add(PathEditorPair item)
            {
                Add(item.Key, item.Value);
            }

            /// <summary>
            /// Removes all keys and values from the <see cref="
            /// EditorDictionary"/>.
            /// </summary>
            public void Clear()
            {
                var values = new List<IEditor>(Values);
                Editors.Clear();
                foreach (var value in values)
                {
                    var e = new EditorEventArgs(value);
                    EditorSelector.OnEditorRemoved(e);
                }
            }

            /// <summary>
            /// Determines whether the <see cref="EditorDictionary"/> contains
            /// the specified file path key.
            /// </summary>
            /// <param name="key">
            /// The file path key to locate in the <see cref="
            /// EditorDictionary"/>.
            /// </param>
            /// <returns>
            /// <see langword="true"/> if the <see cref=" EditorDictionary"/>
            /// contains an element with the specified file path key, otherwise
            /// <see langword=" false"/>.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// <paramref name="key"/> is a zero-length string, contains only
            /// whitespace, or contains one or more of the invalid characters
            /// defined in <see cref=" GetInvalidPathChars"/>.
            /// <para/>
            /// -or-
            /// <para/>
            /// The system could not retrieve the absolute path.
            /// </exception>
            /// <exception cref="System.Security.SecurityException">
            /// The caller does not have the required permissions.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="key"/> is <see langword="null"/>.
            /// </exception>
            /// <exception cref="NotSupportedException">
            /// <paramref name="key"/> contains a colon (":") that is not part
            /// of a volume identifier (for example, "c:\").
            /// </exception>
            /// <exception cref="System.IO.PathTooLongException">
            /// The specified path, file name, or both exceed the
            /// system-defined maximum length.
            /// </exception>
            public bool ContainsKey(string key)
            {
                var actualKey = GetFullPath(key);
                return Editors.ContainsKey(actualKey);
            }

            /// <inheritdoc/>
            ///
            bool ICollection<PathEditorPair>.Contains(PathEditorPair item)
            {
                var itemWithActualKey = new PathEditorPair(
                    GetFullPath(item.Key),
                    item.Value);
                return IDictionary.Contains(itemWithActualKey);
            }

            /// <summary>
            /// Removes the <see cref="IEditor"/> with the specified file path
            /// key.
            /// </summary>
            /// <param name="key">
            /// The file path of the <see cref="IEditor"/> to remove.
            /// </param>
            /// <returns>
            /// <see langword="true"/> if the <see cref="IEditor"/> is
            /// successfully found and removed; otherwise <see
            /// langword="false"/>.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// <paramref name="key"/> is a zero-length string, contains only
            /// whitespace, or contains one or more of the invalid characters
            /// defined in <see cref=" GetInvalidPathChars"/>.
            /// <para/>
            /// -or-
            /// <para/>
            /// The system could not retrieve the absolute path.
            /// </exception>
            /// <exception cref="System.Security.SecurityException">
            /// The caller does not have the required permissions.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="key"/> is <see langword="null"/>.
            /// </exception>
            /// <exception cref="NotSupportedException">
            /// <paramref name="key"/> contains a colon (":") that is not part
            /// of a volume identifier (for example, "c:\").
            /// </exception>
            /// <exception cref="System.IO.PathTooLongException">
            /// The specified path, file name, or both exceed the
            /// system-defined maximum length.
            /// </exception>
            public bool Remove(string key)
            {
                var actualKey = GetFullPath(key);
                if (Editors.TryGetValue(actualKey, out var value))
                {
                    Editors.Remove(actualKey);

                    var e = new EditorEventArgs(value);
                    EditorSelector.OnEditorRemoved(e);
                    return true;
                }

                return false;
            }

            /// <inheritdoc/>
            ///
            bool ICollection<PathEditorPair>.Remove(PathEditorPair item)
            {
                var itemWithActualKey = new PathEditorPair(
                    GetFullPath(item.Key),
                    item.Value);

                if (IDictionary.Remove(itemWithActualKey))
                {
                    var e = new EditorEventArgs(item.Value);
                    EditorSelector.OnEditorRemoved(e);
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Gets the <see cref="IEditor"/> associated with the specified
            /// file path.
            /// </summary>
            /// <param name="key">
            /// The file path of the value to get.
            /// </param>
            /// <param name="value">
            /// When this method returns, contains the <see cref=" IEditor"/>
            /// associated with <paramref name="key"/>, if it is found;
            /// otherwise <see langword="null"/> is returned. This parameter is
            /// passed uninitialized.
            /// </param>
            /// <returns>
            /// <see langword="true"/> if <see cref="EditorDictionary"/>
            /// contains an <see cref="IEditor"/> with the specified file path
            /// key; otherwise <see langword="false"/>.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// <paramref name="key"/> is a zero-length string, contains only
            /// whitespace, or contains one or more of the invalid characters
            /// defined in <see cref=" GetInvalidPathChars"/>.
            /// <para/>
            /// -or-
            /// <para/>
            /// The system could not retrieve the absolute path.
            /// </exception>
            /// <exception cref="System.Security.SecurityException">
            /// The caller does not have the required permissions.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="key"/> is <see langword="null"/>.
            /// </exception>
            /// <exception cref="NotSupportedException">
            /// <paramref name="key"/> contains a colon (":") that is not part
            /// of a volume identifier (for example, "c:\").
            /// </exception>
            /// <exception cref="System.IO.PathTooLongException">
            /// The specified path, file name, or both exceed the
            /// system-defined maximum length.
            /// </exception>
            /// <inheritdoc cref="GetFullPath(String)" select="exception"/>
            ///
            public bool TryGetValue(string key, out IEditor value)
            {
                var actualKey = GetFullPath(key);
                return Editors.TryGetValue(
                    actualKey,
                    out value);
            }

            /// <inheritdoc/>
            ///
            void ICollection<PathEditorPair>.CopyTo(
                PathEditorPair[] array,
                int arrayIndex)
            {
                IDictionary.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// Returns an enumerator that enumerates through the <see
            /// cref="EditorDictionary"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="Dictionary{TKey, TValue}.Enumerator"/> for the
            /// <see cref="EditorDictionary"/>.
            /// </returns>
            public Dictionary<string, IEditor>.Enumerator GetEnumerator()
            {
                return Editors.GetEnumerator();
            }

            /// <inheritdoc/>
            ///
            IEnumerator<PathEditorPair>
                IEnumerable<PathEditorPair>.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <inheritdoc/>
            ///
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
