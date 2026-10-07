// <copyright file="AutoSaveHelper.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Core;

using System;
using System.Collections.Generic;
using System.Globalization;

public static class AutoSaveHelper
{
    public const string DateTimeFormat = "yyyy\"-\"MM\"-\"dd\"T\"HH\"h\"mm\"m\"ss\"s\"";

    public static readonly CultureInfo FormatCulture = CultureInfo.InvariantCulture;

    public static void AutoSave(string basePath, byte[] data)
    {
        var dir = Path.GetDirectoryName(basePath) ?? String.Empty;
        var ext = Path.GetExtension(basePath) ?? String.Empty;

        var timeStamp = DateTime.Now.ToUniversalTime().ToString(
            DateTimeFormat,
            FormatCulture);

        var dest = Path.Combine(
            dir,
            timeStamp + ext);

        try
        {
            if (!Directory.Exists(dir))
            {
                _ = Directory.CreateDirectory(dir);
            }

            File.WriteAllBytes(dest, data);
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static void PruneOldAutoSaves(string basePath, TimeSpan ago, bool constant = false)
    {
        var dir = Path.GetDirectoryName(basePath) ?? String.Empty;
        var files = Directory.GetFiles(dir);
        var items = new SortedDictionary<DateTime, string>();

        foreach (var file in files)
        {
            var utcTime = GetDateTime(basePath, file);
            if (!utcTime.HasValue)
            {
                continue;
            }

            var localTime = utcTime.Value.ToLocalTime();
            items.Add(localTime, file);
        }

        if (items.Count == 0)
        {
            return;
        }

        var cutoff = items.Last().Key - ago;
        using var en = items.GetEnumerator();
        while (en.MoveNext())
        {
            if (en.Current.Key < cutoff)
            {
                var skip = 0;
                do
                {
                    for (var i = 0; i < skip; i++)
                    {
                        if (!en.MoveNext())
                        {
                            return;
                        }
                    }

                    if (!constant)
                    {
                        skip++;
                    }

                    File.Delete(en.Current.Value);
                } while (en.MoveNext());
            }
        }
    }

    private static DateTime? GetDateTime(string basePath, string actualPath)
    {
        var dir = Path.GetDirectoryName(basePath) ?? String.Empty;
        var ext = Path.GetExtension(basePath) ?? String.Empty;

        var comparer = StringComparer.OrdinalIgnoreCase;
        if (!comparer.Equals(dir, Path.GetDirectoryName(actualPath)))
        {
            return null;
        }

        if (!comparer.Equals(ext, Path.GetExtension(actualPath)))
        {
            return null;
        }

        var actualName = Path.GetFileNameWithoutExtension(actualPath);
        return DateTime.TryParseExact(
            actualName,
            DateTimeFormat,
            FormatCulture,
            DateTimeStyles.None,
            out var date)
            ? date
            : null;
    }
}
