// <copyright file="TestRomGenerator.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using System;

/// <summary>
/// Generates test ROM data using member configuration values.
/// </summary>
/// <remarks>
/// The member will be serialized through JSON files.
/// </remarks>
internal class TestRomGenerator
{
    public string? AddressMode
    {
        get; set;
    }

    public int Size
    {
        get; set;
    }

    public string? LoRom
    {
        get; set;
    }

    public string? HiRom
    {
        get; set;
    }

    public string? ExLoRom
    {
        get; set;
    }

    public string? ExHiRom
    {
        get; set;
    }

    public Rom CreateRom()
    {
        return new Rom(CreateRomData());
    }

    public byte[] CreateRomData()
    {
        var result = new byte[Size];
        result[0] = 0x78;
        result[1] = 0x9C;
        for (var i = 0; i < 0x200; i++)
        {
            result[i] = 0xFF;
        }

        if (!String.IsNullOrEmpty(LoRom))
        {
            var loRom = Convert.FromBase64String(LoRom);
            loRom.CopyTo(result, 0x7FB0);
        }

        if (!String.IsNullOrEmpty(HiRom))
        {
            var hiRom = Convert.FromBase64String(HiRom);
            hiRom.CopyTo(result, 0xFFB0);
        }

        if (!String.IsNullOrEmpty(ExLoRom))
        {
            var exLoRom = Convert.FromBase64String(ExLoRom);
            exLoRom.CopyTo(result, 0x407FB0);
        }

        if (!String.IsNullOrEmpty(ExHiRom))
        {
            var exHiRom = Convert.FromBase64String(ExHiRom);
            exHiRom.CopyTo(result, 0x40FFB0);
        }

        return result;
    }
}
